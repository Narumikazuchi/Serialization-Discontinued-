using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Narumikazuchi.Serialization.Bytes
{
    /// <summary>
    /// Base class for the 2 different byte serializer classes, which can't be inherited.
    /// </summary>
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract partial class SharedByteSerializer
    {
        /// <summary>
        /// Applies the specified strategy for the specified type, when serializing that type.
        /// </summary>
        /// <param name="forType">The type the strategy aims to serialize and deserialize.</param>
        /// <param name="strategy">The strategy to use during serialization and deserialization.</param>
        /// <exception cref="ArgumentNullException" />
        public void ApplyStrategy(Type forType,
                                  ISerializationStrategy<Byte[]> strategy)
        {
            ExceptionHelpers.ThrowIfNull(forType);
            ExceptionHelpers.ThrowIfNull(strategy);

            if (this._strategies.ContainsKey(forType))
            {
                this._strategies[forType] = strategy;
                return;
            }
            this._strategies.Add(forType,
                                 strategy);
        }
    }

    // Non-Public, Non-Private
    partial class SharedByteSerializer
    {
        private protected SharedByteSerializer()
        { }

        private protected SharedByteSerializer(IReadOnlyDictionary<Type, ISerializationStrategy<Byte[]>> strategies) =>
            this._strategies = new(strategies);

        /**
         * FF FF FF FF FF FF FF FF -> Number of bytes for the entire object
         * FF FF FF FF FF FF FF FF -> Number of bytes for the HEAD
         * FF FF FF FF FF FF FF FF -> Number of bytes for the BODY
         * HEAD:
         * FF FF FF FF -> Count of serialized members
         * {
         *   FF FF FF FF FF FF FF FF -> Position/Index in the BODY
         *   FF FF FF FF FF FF FF FF -> Number of bytes in the BODY
         *   FF FF FF FF -> Size of typename String
         *   [FF..] -> Typename String
         *   FF FF FF FF -> Size of name String
         *   [FF..] -> Name String
         * }
         * BODY:
         * {
         *   [FF..] -> Serialized member
         * }
         */
        internal UInt64 SerializeWithInfo(Stream stream,
                                          SerializationInfo info,
                                          __Header header)
        {
#nullable disable
            using MemoryStream body = new();

            foreach (MemberState data in info)
            {
                HeaderItem item = new()
                {
                    Position = body.Position
                };
                if (this._strategies.ContainsKey(data.MemberType))
                {
                    this.SerializeWithStrategy(item,
                                               body,
                                               data);
                    header.Items.Add(item);
                    continue;
                }
                if (data.MemberType.GetInterfaces()
                                  .Any(i => i.GetGenericTypeDefinition() == typeof(ISerializable<>)))
                {
                    this.SerializeThroughInterface(item,
                                                   body,
                                                   data);
                    header.Items.Add(item);
                    continue;
                }
                // No strategy available, try more abstract, that is System.Object
                if (this.GetType() == typeof(ByteSerializer))
                {
                    // Object is already the most abstract level, so we failed
                    throw new InvalidOperationException();
                }
                this.SerializeAsObject(item,
                                       body,
                                       data);
                header.Items.Add(item);
            }

            body.Position = 0;

            using MemoryStream head = header.AsMemory();

            UInt64 total = 2UL * sizeof(UInt64) + (UInt64)head.Length + (UInt64)body.Length;
            stream.Write(BitConverter.GetBytes(total));
            stream.Write(BitConverter.GetBytes(head.Length));
            stream.Write(BitConverter.GetBytes(body.Length));
            head.CopyTo(stream);
            body.CopyTo(stream);

            total += sizeof(UInt64);

            return total;
#nullable enable
        }

        internal UInt64 SerializeInternal(Stream stream,
                                          Object? graph)
        {
#nullable disable
            if (graph is null)
            {
                stream.Write(BitConverter.GetBytes(2UL * sizeof(UInt64)));
                stream.Write(BitConverter.GetBytes(0UL));
                stream.Write(BitConverter.GetBytes(0UL));
                return 3 * sizeof(UInt64);
            }

            __Header header = new(graph.GetType());
            SerializationInfo info = SerializationInfo.Create(graph);
            return this.SerializeWithInfo(stream,
                                          info,
                                          header);
#nullable enable
        }
        internal UInt64 SerializeInternal(Stream stream,
                                          ISerializable? graph)
        {
#nullable disable
            if (graph is null)
            {
                stream.Write(BitConverter.GetBytes(2UL * sizeof(UInt64)));
                stream.Write(BitConverter.GetBytes(0UL));
                stream.Write(BitConverter.GetBytes(0UL));
                return 3 * sizeof(UInt64);
            }

            __Header header = new(graph.GetType());
            SerializationInfo info = SerializationInfo.Create(graph);
            return this.SerializeWithInfo(stream,
                                          info,
                                          header);
#nullable enable
        }

        internal SerializationInfo DeserializeInternal(Stream stream,
                                                       out UInt64 read)
        {
#pragma warning disable
            read = 0;

            // SIZES
            UInt64 tempRead = 0;
            Int64 sizeofObject = ReadInt64(stream, out tempRead);
            read += tempRead;

            Int64 sizeofHead = ReadInt64(stream, out tempRead);
            read += tempRead;

            Int64 sizeofBody = ReadInt64(stream, out tempRead);
            read += tempRead;

            // NULL
            if (sizeofObject == 2L * sizeof(Int64) &&
                sizeofHead == 0L &&
                sizeofBody == 0L)
            {
                return SerializationInfo.CreateNull();
            }

            // HEAD
            __Header header = __Header.FromStream(stream,
                                                  sizeofHead,
                                                  out tempRead);
            read += tempRead;
            Type? type = Type.GetType(header.Typename);
            if (type is null)
            {
                throw new FormatException();
            }
            Int64 bodyStart = 3 * sizeof(Int64) + (Int64)sizeofHead;
            SerializationInfo result = SerializationInfo.Create(type);

            for (Int32 i = 0; i < header.MemberCount; i++)
            {
                type = Type.GetType(header.Items[i].Typename);
                if (type is null)
                {
                    throw new FormatException();
                }

                Object? member;
                if (this._strategies.ContainsKey(type))
                {
                    member = this.DeserializeWithStrategy(stream,
                                                          type,
                                                          bodyStart,
                                                          header.Items[i]);
                    result.Add(header.Items[i].Name,
                               member);
                    read += Convert.ToUInt64(header.Items[i].Length);
                    continue;
                }
                if (type.GetInterfaces()
                        .Any(i => i.GetGenericTypeDefinition() == typeof(ISerializable<>)))
                {
                    member = this.DeserializeThroughInterface(stream,
                                                              type,
                                                              bodyStart,
                                                              header.Items[i],
                                                              out tempRead);
                    result.Add(header.Items[i].Name,
                               member);
                    read += tempRead;
                    continue;
                }
                // No strategy available, try more abstract, that is System.Object
                member = this.DeserializeAsObject(stream,
                                                  type,
                                                  bodyStart,
                                                  header.Items[i],
                                                  out tempRead);
                result.Add(header.Items[i].Name,
                           member);
                read += tempRead;
            }

            return result;
#pragma warning restore
        }

        private protected readonly Dictionary<Type, ISerializationStrategy<Byte[]>> _strategies = new()
        {
            { typeof(Boolean),  SerializationStrategies.Boolean.Default },
            { typeof(Byte),     SerializationStrategies.Byte.Default },
            { typeof(Char),     SerializationStrategies.Char.Default },
            { typeof(Double),   SerializationStrategies.Double.Default },
            { typeof(Int16),    SerializationStrategies.Int16.Default },
            { typeof(Int32),    SerializationStrategies.Int32.Default },
            { typeof(Int64),    SerializationStrategies.Int64.Default },
            { typeof(IntPtr),   SerializationStrategies.IntPtr.Default },
            { typeof(SByte),    SerializationStrategies.SByte.Default },
            { typeof(Single),   SerializationStrategies.Single.Default },
            { typeof(UInt16),   SerializationStrategies.UInt16.Default },
            { typeof(UInt32),   SerializationStrategies.UInt32.Default },
            { typeof(UInt64),   SerializationStrategies.UInt64.Default },
            { typeof(UIntPtr),  SerializationStrategies.UIntPtr.Default },
            { typeof(DateTime), SerializationStrategies.DateTime.Default },
            { typeof(Guid),     SerializationStrategies.Guid.Default },
            { typeof(Half),     SerializationStrategies.Half.Default },
            { typeof(String),   SerializationStrategies.String.Default }
        };


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private protected const String STREAM_DOES_NOT_SUPPORT_READING = "The specified stream does not support reading.";
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private protected const String STREAM_DOES_NOT_SUPPORT_WRITING = "The specified stream does not support writing.";
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private protected const String STREAM_DOES_NOT_SUPPORT_SEEKING = "The specified stream does not support seeking.";
    }

    // Private
    partial class SharedByteSerializer
    {
        private void SerializeWithStrategy(HeaderItem item,
                                           Stream body,
                                           MemberState data)
        {
#nullable disable
            Byte[] bytes = this._strategies[data.MemberType].Serialize(data.Value);

            item.Length = bytes.Length;
            item.Typename = data.MemberType.AssemblyQualifiedName;
            item.Name = data.Name;
            body.Write(bytes);
#nullable enable
        }

        private void SerializeThroughInterface(HeaderItem item,
                                               Stream body,
                                               MemberState data)
        {
#nullable disable
            Type dataTypeSerializer = typeof(ByteSerializer<>).MakeGenericType(data.MemberType);
            ConstructorInfo ctor = dataTypeSerializer.GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance,
                                                                     new Type[] { typeof(IReadOnlyDictionary<Type, ISerializationStrategy<Byte[]>>) });
            Object serializer = ctor.Invoke(new Object[] { this._strategies });
            MethodInfo method = dataTypeSerializer.GetMethod(nameof(this.SerializeInternal),
                                                             BindingFlags.Instance | BindingFlags.NonPublic,
                                                             new Type[] { typeof(Stream), typeof(ISerializable) });
            using MemoryStream temp = new();
            method.Invoke(serializer,
                          new Object[] { temp, data.Value });

            item.Length = temp.Length;
            item.Typename = data.MemberType.AssemblyQualifiedName;
            item.Name = data.Name;
            temp.Position = 0;
            temp.CopyTo(body);
#nullable enable
        }
        private void SerializeAsObject(HeaderItem item,
                                       Stream body,
                                       MemberState data)
        {
#nullable disable
            using MemoryStream temp = new();
            ByteSerializer serializer = new(this._strategies);
            serializer.SerializeInternal(temp,
                                         data.Value);

            item.Length = temp.Length;
            item.Typename = data.MemberType.AssemblyQualifiedName;
            item.Name = data.Name;
            temp.Position = 0;
            temp.CopyTo(body);
#nullable enable
        }

        private Object? DeserializeWithStrategy(Stream stream,
                                                Type type,
                                                Int64 bodyStart,
                                                HeaderItem item)
        {
#nullable disable
            stream.Position = bodyStart + (Int64)item.Position;
            Byte[] bytes = new Byte[item.Length];
            Int64 index = 0;
            while (index < item.Length)
            {
                Int32 b = stream.ReadByte();
                if (b == -1)
                {
                    // Unexpected end
                    throw new IOException();
                }
                bytes[index++] = (Byte)b;
            }
            return this._strategies[type].Deserialize(bytes);
#nullable enable
        }

        private Object? DeserializeThroughInterface(Stream stream,
                                                    Type type,
                                                    Int64 bodyStart,
                                                    HeaderItem item,
                                                    out UInt64 read)
        {
#nullable disable
            stream.Position = bodyStart + (Int64)item.Position;
            using MemoryStream temp = new();
            Int64 index = 0;
            while (index < item.Length)
            {
                Int32 b = stream.ReadByte();
                if (b == -1)
                {
                    // Unexpected end
                    throw new IOException();
                }
                temp.WriteByte((Byte)b);
                index++;
            }
            temp.Position = 0;
            Type dataTypeSerializer = typeof(ByteSerializer<>).MakeGenericType(type);
            ConstructorInfo ctor = dataTypeSerializer.GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance,
                                                                     new Type[] { typeof(IReadOnlyDictionary<Type, ISerializationStrategy<Byte[]>>) });
            Object serializer = ctor.Invoke(new Object[] { this._strategies });
            MethodInfo method = dataTypeSerializer.GetMethod(nameof(this.DeserializeInternal),
                                                             BindingFlags.Instance | BindingFlags.NonPublic,
                                                             new Type[] { typeof(Stream), typeof(UInt64).MakeByRefType() });

            Object[] parameters = new Object[] { temp, 0UL };
            SerializationInfo info = (SerializationInfo)method.Invoke(serializer,
                                                                      parameters);
            read = (UInt64)parameters[1];

            if (info.IsNull)
            {
                return null;
            }

            method = type.GetMethod("ConstructFromSerializationData",
                                    BindingFlags.Static | BindingFlags.Public,
                                    new Type[] { typeof(SerializationInfo) });
            return method.Invoke(null,
                                 new Object[] { info });
#nullable enable
        }

        private Object? DeserializeAsObject(Stream stream,
                                            Type type,
                                            Int64 bodyStart,
                                            HeaderItem item,
                                            out UInt64 read)
        {
#nullable disable
            stream.Position = bodyStart + (Int64)item.Position;
            using MemoryStream temp = new();
            Int64 index = 0;
            while (index < item.Length)
            {
                Int32 b = stream.ReadByte();
                if (b == -1)
                {
                    // Unexpected end
                    throw new IOException();
                }
                temp.WriteByte((Byte)b);
                index++;
            }
            temp.Position = 0;

            ByteSerializer serializer = new(this._strategies);
            
            SerializationInfo info = serializer.DeserializeInternal(temp,
                                                                    out read);
            if (type != info.Type)
            {
                throw new FormatException();
            }

            if (info.IsNull)
            {
                return null;
            }

            ConstructorInfo ctor = type.GetConstructor(Type.EmptyTypes);
            Object result = ctor.Invoke(Array.Empty<Object>());
            foreach (String member in info.Members)
            {
                PropertyInfo property = type.GetProperty(member,
                                                         BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (property is not null)
                {
                    property.SetValue(result, 
                                      info.Get<Object>(member));
                    continue;
                }
                FieldInfo field = type.GetField(member,
                                                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (field is not null)
                {
                    field.SetValue(result,
                                   info.Get<Object>(member));
                    continue;
                }
            }

            return result;
#nullable enable
        }

        private static Int64 ReadInt64(Stream stream,
                                       out UInt64 read)
        {
            Byte[] data = new Byte[sizeof(Int64)];
            stream.Read(data,
                        0,
                        sizeof(Int64));
            read = sizeof(Int64);
            return BitConverter.ToInt64(data);
        }
    }
}

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace Narumikazuchi.Serialization.Bytes
{
    /// <summary>
    /// A serializer for classes that implement <see cref="IByteSerializable"/>.
    /// </summary>
    public sealed partial class ByteSerializer<TType> 
        where TType : class, IByteSerializable
    {
        /// <summary>
        /// Instantiates a new instance of the <see cref="ByteSerializer{TType}"/> class.
        /// </summary>
        /// <exception cref="InvalidOperationException"/>
        public ByteSerializer()
        {
            if (typeof(TType).IsAbstract ||
                typeof(TType).IsInterface)
            {
                throw new InvalidOperationException(String.Format(GENERIC_TYPE_NOT_INSTANTIABLE,
                                                                  typeof(TType).Name));
            }
        }

        /// <summary>
        /// Serializes the specified graph into it's byte representation.
        /// </summary>
        /// <param name="graph">The graph to serialize into it's byte representation.</param>
        /// <returns>The specified graph in it's byte representation</returns>
        /// <exception cref="ArgumentNullException"/>
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.NonPublicMethods)]
        [Pure]
        [return: NotNull]
        public Byte[] Serialize([DisallowNull] TType graph)
        {
            if (graph is null)
            {
                throw new ArgumentNullException(nameof(graph));
            }
            Byte[] data = SerializeInternal(graph);
            return BitConverter.GetBytes(data.Length)
                               .Concat(data)
                               .ToArray();
        }

        /// <summary>
        /// Deserializes the specified bytes starting at the specified offset into an instance of type <typeparamref name="TType"/>.
        /// </summary>
        /// <param name="bytes">The bytes supposed to represent an instance of <typeparamref name="TType"/>.</param>
        /// <returns>The instance represented by the specified bytes starting at the specified offset</returns>
        /// <exception cref="ArgumentNullException"/>
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.NonPublicMethods)]
        [Pure]
        [return: NotNull]
        public TType Deserialize([DisallowNull] Byte[] bytes) =>
            this.Deserialize(bytes, 
                             0, 
                             out UInt32 _);
        /// <summary>
        /// Deserializes the specified bytes starting at the specified offset into an instance of type <typeparamref name="TType"/>.
        /// </summary>
        /// <param name="bytes">The bytes supposed to represent an instance of <typeparamref name="TType"/>.</param>
        /// <param name="offset">The amount of elements in <paramref name="bytes"/> to ignore, starting from the first.</param>
        /// <returns>The instance represented by the specified bytes starting at the specified offset</returns>
        /// <exception cref="ArgumentNullException"/>
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.NonPublicMethods)]
        [Pure]
        [return: NotNull]
        public TType Deserialize([DisallowNull] Byte[] bytes,
                                 in Int32 offset) =>
            this.Deserialize(bytes,
                             offset,
                             out UInt32 _);
        /// <summary>
        /// Deserializes the specified bytes starting at the specified offset into an instance of type <typeparamref name="TType"/>.
        /// </summary>
        /// <param name="bytes">The bytes supposed to represent an instance of <typeparamref name="TType"/>.</param>
        /// <param name="read">The amount of elements read from the <paramref name="bytes"/> parameter.</param>
        /// <returns>The instance represented by the specified bytes starting at the specified offset</returns>
        /// <exception cref="ArgumentNullException"/>
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.NonPublicMethods)]
        [Pure]
        [return: NotNull]
        public TType Deserialize([DisallowNull] Byte[] bytes, 
                                 out UInt32 read) =>
            this.Deserialize(bytes, 
                             0, 
                             out read);
        /// <summary>
        /// Deserializes the specified bytes starting at the specified offset into an instance of type <typeparamref name="TType"/>.
        /// </summary>
        /// <param name="bytes">The bytes supposed to represent an instance of <typeparamref name="TType"/>.</param>
        /// <param name="offset">The amount of elements in <paramref name="bytes"/> to ignore, starting from the first.</param>
        /// <param name="read">The amount of elements read from the <paramref name="bytes"/> parameter.</param>
        /// <returns>The instance represented by the specified bytes starting at the specified offset</returns>
        /// <exception cref="ArgumentNullException"/>
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.NonPublicMethods)]
        [Pure]
        [return: NotNull]
        public TType Deserialize([DisallowNull] Byte[] bytes, 
                                 in Int32 offset, 
                                 out UInt32 read)
        {
            if (bytes is null)
            {
                throw new ArgumentNullException(nameof(bytes));
            }

            Int32 size = BitConverter.ToInt32(bytes, 
                                              offset);
            Byte[] data = bytes.Skip(offset + 4)
                               .Take(size)
                               .ToArray();

            TType result;
            ConstructorInfo? constructor = typeof(TType).GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, 
                                                                        null, 
                                                                        Array.Empty<Type>(), 
                                                                        null);
            if (constructor is not null)
            {
                result = (TType)constructor.Invoke(Array.Empty<Object>());
                read = ByteSerializer<TType>.DeserializeInitializedInternal(result, 
                                                                            data);
            }
            else
            {
                result = (TType)FormatterServices.GetSafeUninitializedObject(typeof(TType));
                read = ByteSerializer<TType>.DeserializeUninitializedInternal(result, 
                                                                              data);
            }
            return result;
        }
    }

    // Non-Public
    partial class ByteSerializer<TType>
    {
        private static Byte[] SerializeInternal(TType graph)
        {
#nullable disable
            Type @base = typeof(TType).BaseType;
            if (@base is not null &&
                @base.GetInterface(nameof(IByteSerializable)) is not null)
            {
                Type baseTypeSerializer = typeof(ByteSerializer<>).MakeGenericType(@base);
                ConstructorInfo ctor = baseTypeSerializer.GetConstructor(Type.EmptyTypes);
                Object serializer = ctor.Invoke(Array.Empty<Object>());
                MethodInfo method = baseTypeSerializer.GetMethod("SerializeInternal", BindingFlags.Static | BindingFlags.NonPublic);
                Byte[] data = (Byte[])method.Invoke(serializer, new Object[] { graph });
                return data.Concat(graph.ToBytes()).ToArray();
            }
            else
            {
                return graph.ToBytes().ToArray();
            }
#nullable enable
        }

        private static UInt32 DeserializeUninitializedInternal(TType newObj, Byte[] bytes)
        {
#nullable disable
            UInt32 offset = 0;
            Type @base = typeof(TType).BaseType;
            if (@base is not null &&
                @base.GetInterface(nameof(IByteSerializable)) is not null)
            {
                Type baseTypeSerializer = typeof(ByteSerializer<>).MakeGenericType(@base);
                ConstructorInfo ctor = baseTypeSerializer.GetConstructor(Type.EmptyTypes);
                Object serializer = ctor.Invoke(Array.Empty<Object>());
                MethodInfo method = baseTypeSerializer.GetMethod("DeserializeUninitializedInternal", BindingFlags.Static | BindingFlags.NonPublic);
                offset = (UInt32)method.Invoke(serializer, new Object[] { newObj, bytes });
            }
            if (offset > 0)
            {
                Byte[] data = bytes.Skip(Convert.ToInt32(offset)).ToArray();
                return newObj.InitializeUninitializedState(data) + offset;
            }
            return newObj.InitializeUninitializedState(bytes);
#nullable enable
        }

        private static UInt32 DeserializeInitializedInternal(TType newObj, Byte[] bytes)
        {
#nullable disable
            UInt32 offset = 0;
            Type @base = typeof(TType).BaseType;
            if (@base is not null &&
                @base.GetInterface(nameof(IByteSerializable)) is not null)
            {
                Type baseTypeSerializer = typeof(ByteSerializer<>).MakeGenericType(@base);
                ConstructorInfo ctor = baseTypeSerializer.GetConstructor(Type.EmptyTypes);
                Object serializer = ctor.Invoke(Array.Empty<Object>());
                MethodInfo method = baseTypeSerializer.GetMethod("DeserializeInitializedInternal", BindingFlags.Static | BindingFlags.NonPublic);
                offset = (UInt32)method.Invoke(serializer, new Object[] { newObj, bytes });
            }
            if (offset > 0)
            {
                Byte[] data = bytes.Skip(Convert.ToInt32(offset)).ToArray();
                return newObj.SetState(data) + offset;
            }
            return newObj.SetState(bytes);
#nullable enable
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String GENERIC_TYPE_NOT_INSTANTIABLE = "The generic type {0} needs to be instantiable to be used as type parameter.";
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String STREAM_DOES_NOT_SUPPORT_READING = "The specified stream does not support reading.";
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String STREAM_DOES_NOT_SUPPORT_WRITING = "The specified stream does not support writing.";
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const String STREAM_DOES_NOT_SUPPORT_SEEKING = "The specified stream does not support seeking.";
    }

    // ISerializer<TType>
    partial class ByteSerializer<TType> : ISerializer<TType>
    {
        /// <summary>
        /// Serializes the specified graph into the specified stream.
        /// </summary>
        /// <param name="stream">The stream to serialize the graph into.</param>
        /// <param name="graph">The graph to serialize.</param>
        /// <returns>The amount of bytes written</returns>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="InvalidOperationException"/>
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.NonPublicMethods)]
        public UInt32 Serialize([DisallowNull] Stream stream, 
                                [DisallowNull] TType graph) =>
            this.Serialize(stream, 
                           graph, 
                           -1, 
                           SerializationFinishAction.None);
        /// <summary>
        /// Serializes the specified graph into the specified stream starting at the specified offset in the stream.
        /// </summary>
        /// <param name="stream">The stream to serialize the graph into.</param>
        /// <param name="graph">The graph to serialize.</param>
        /// <param name="offset">The offset in the stream where to begin writing.</param>
        /// <returns>The amount of bytes written</returns>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="InvalidOperationException"/>
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.NonPublicMethods)]
        public UInt32 Serialize([DisallowNull] Stream stream, 
                                [DisallowNull] TType graph, 
                                in Int64 offset) =>
            this.Serialize(stream, 
                           graph, 
                           offset, 
                           SerializationFinishAction.None);
        /// <summary>
        /// Serializes the specified graph into the specified stream.
        /// </summary>
        /// <param name="stream">The stream to serialize the graph into.</param>
        /// <param name="graph">The graph to serialize.</param>
        /// <param name="actionAfter">The actions to perform after the writing operation has finished.</param>
        /// <returns>The amount of bytes written</returns>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="InvalidOperationException"/>
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.NonPublicMethods)]
        public UInt32 Serialize([DisallowNull] Stream stream, 
                                [DisallowNull] TType graph, 
                                in SerializationFinishAction actionAfter) =>
            this.Serialize(stream, 
                           graph, 
                           -1, 
                           actionAfter);
        /// <summary>
        /// Serializes the specified graph into the specified stream starting at the specified offset in the stream.
        /// </summary>
        /// <param name="stream">The stream to serialize the graph into.</param>
        /// <param name="graph">The graph to serialize.</param>
        /// <param name="offset">The offset in the stream where to begin writing.</param>
        /// <param name="actionAfter">The actions to perform after the writing operation has finished.</param>
        /// <returns>The amount of bytes written</returns>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="InvalidOperationException"/>
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.NonPublicMethods)]
        public UInt32 Serialize([DisallowNull] Stream stream, 
                                [DisallowNull] TType graph, 
                                in Int64 offset, 
                                in SerializationFinishAction actionAfter)
        {
            if (stream is null)
            {
                throw new ArgumentNullException(nameof(stream));
            }
            if (graph is null)
            {
                throw new ArgumentNullException(nameof(graph));
            }
            if (!stream.CanWrite)
            {
                throw new InvalidOperationException(STREAM_DOES_NOT_SUPPORT_WRITING);
            }
            if (offset > -1 &&
                !stream.CanSeek)
            {
                throw new InvalidOperationException(STREAM_DOES_NOT_SUPPORT_SEEKING);
            }

            if (offset > -1)
            {
                stream.Seek(offset, 
                            SeekOrigin.Begin);
            }
            Byte[] data = this.Serialize(graph);
            stream.Write(data, 
                         0, 
                         data.Length);

            if (actionAfter.HasFlag(SerializationFinishAction.FlushStream))
            {
                stream.Flush();
            }
            if (actionAfter.HasFlag(SerializationFinishAction.CloseStream))
            {
                stream.Close();
            }

            return (UInt32)data.Length;
        }

        /// <summary>
        /// Tries to serialize the specified graph into the specified stream.
        /// </summary>
        /// <param name="stream">The stream to serialize the graph into.</param>
        /// <param name="graph">The graph to serialize.</param>
        /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.NonPublicMethods)]
        public Boolean TrySerialize([DisallowNull] Stream stream, 
                                    [DisallowNull] TType graph) =>
            this.TrySerialize(stream, 
                              graph, 
                              -1, 
                              out UInt32 _,
                              SerializationFinishAction.None);
        /// <summary>
        /// Tries to serialize the specified graph into the specified stream starting at the specified offset in the stream.
        /// </summary>
        /// <param name="stream">The stream to serialize the graph into.</param>
        /// <param name="graph">The graph to serialize.</param>
        /// <param name="offset">The offset in the stream where to begin writing.</param>
        /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.NonPublicMethods)]
        public Boolean TrySerialize([DisallowNull] Stream stream, 
                                    [DisallowNull] TType graph, 
                                    in Int64 offset) =>
            this.TrySerialize(stream, 
                              graph, 
                              offset,
                              out UInt32 _,
                              SerializationFinishAction.None);
        /// <summary>
        /// Tries to serialize the specified graph into the specified stream starting at the specified offset in the stream.
        /// </summary>
        /// <param name="stream">The stream to serialize the graph into.</param>
        /// <param name="graph">The graph to serialize.</param>
        /// <param name="written">The amount of bytes written.</param>
        /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.NonPublicMethods)]
        public Boolean TrySerialize([DisallowNull] Stream stream,
                                    [DisallowNull] TType graph,
                                    out UInt32 written) =>
            this.TrySerialize(stream,
                              graph,
                              -1,
                              out written,
                              SerializationFinishAction.None);
        /// <summary>
        /// Tries to serialize the specified graph into the specified stream starting at the specified offset in the stream.
        /// </summary>
        /// <param name="stream">The stream to serialize the graph into.</param>
        /// <param name="graph">The graph to serialize.</param>
        /// <param name="offset">The offset in the stream where to begin writing.</param>
        /// <param name="written">The amount of bytes written.</param>
        /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.NonPublicMethods)]
        public Boolean TrySerialize([DisallowNull] Stream stream,
                                    [DisallowNull] TType graph,
                                    in Int64 offset,
                                    out UInt32 written) =>
            this.TrySerialize(stream,
                              graph,
                              offset,
                              out written,
                              SerializationFinishAction.None);
        /// <summary>
        /// Tries to serialize the specified graph into the specified stream.
        /// </summary>
        /// <param name="stream">The stream to serialize the graph into.</param>
        /// <param name="graph">The graph to serialize.</param>
        /// <param name="actionAfter">The actions to perform after the writing operation has finished.</param>
        /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.NonPublicMethods)]
        public Boolean TrySerialize([DisallowNull] Stream stream, 
                                    [DisallowNull] TType graph, 
                                    in SerializationFinishAction actionAfter) =>
            this.TrySerialize(stream, 
                              graph, 
                              -1,
                              out UInt32 _,
                              actionAfter);
        /// <summary>
        /// Tries to serialize the specified graph into the specified stream.
        /// </summary>
        /// <param name="stream">The stream to serialize the graph into.</param>
        /// <param name="graph">The graph to serialize.</param>
        /// <param name="offset">The offset in the stream where to begin writing.</param>
        /// <param name="actionAfter">The actions to perform after the writing operation has finished.</param>
        /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.NonPublicMethods)]
        public Boolean TrySerialize([DisallowNull] Stream stream,
                                    [DisallowNull] TType graph,
                                    in Int64 offset,
                                    in SerializationFinishAction actionAfter) =>
            this.TrySerialize(stream,
                              graph,
                              offset,
                              out UInt32 _,
                              actionAfter);
        /// <summary>
        /// Tries to serialize the specified graph into the specified stream.
        /// </summary>
        /// <param name="stream">The stream to serialize the graph into.</param>
        /// <param name="graph">The graph to serialize.</param>
        /// <param name="written">The amount of bytes written.</param>
        /// <param name="actionAfter">The actions to perform after the writing operation has finished.</param>
        /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.NonPublicMethods)]
        public Boolean TrySerialize([DisallowNull] Stream stream,
                                    [DisallowNull] TType graph,
                                    out UInt32 written,
                                    in SerializationFinishAction actionAfter) =>
            this.TrySerialize(stream,
                              graph,
                              -1,
                              out written,
                              actionAfter);
        /// <summary>
        /// Tries to serialize the specified graph into the specified stream starting at the specified offset in the stream.
        /// </summary>
        /// <param name="stream">The stream to serialize the graph into.</param>
        /// <param name="graph">The graph to serialize.</param>
        /// <param name="offset">The offset in the stream where to begin writing.</param>
        /// <param name="written">The amount of bytes written.</param>
        /// <param name="actionAfter">The actions to perform after the writing operation has finished.</param>
        /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.NonPublicMethods)]
        public Boolean TrySerialize([DisallowNull] Stream stream, 
                                    [DisallowNull] TType graph, 
                                    in Int64 offset,
                                    out UInt32 written, 
                                    in SerializationFinishAction actionAfter)
        {
            try
            {
                written =  this.Serialize(stream, 
                                          graph, 
                                          offset, 
                                          actionAfter);
                return true;
            }
            catch
            {
                written = 0;
                return false;
            }
        }

        /// <summary>
        /// Deserializes the specified stream into an instance of type <typeparamref name="TType"/>.
        /// </summary>
        /// <param name="stream">The stream to deserialize the graph from.</param>
        /// <returns>The instance represented by the bytes in the specified stream</returns>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="InvalidOperationException"/>
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.NonPublicMethods)]
        [return: NotNull]
        public TType Deserialize([DisallowNull] Stream stream) =>
            this.Deserialize(stream, 
                             -1,
                             out UInt32 _,
                             SerializationFinishAction.None);
        /// <summary>
        /// Deserializes the specified stream starting at the specified offset into an instance of type <typeparamref name="TType"/>.
        /// </summary>
        /// <param name="stream">The stream to deserialize the graph from.</param>
        /// <param name="offset">The offset in the stream where to begin reading.</param>
        /// <returns>The instance represented by the bytes in the specified stream</returns>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="InvalidOperationException"/>
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.NonPublicMethods)]
        [return: NotNull]
        public TType Deserialize([DisallowNull] Stream stream,
                                 in Int64 offset) =>
            this.Deserialize(stream,
                             offset,
                             out UInt32 _,
                             SerializationFinishAction.None);
        /// <summary>
        /// Deserializes the specified stream starting at the specified offset into an instance of type <typeparamref name="TType"/>.
        /// </summary>
        /// <param name="stream">The stream to deserialize the graph from.</param>
        /// <param name="read">The amount of elements read from the <paramref name="stream"/> parameter.</param>
        /// <returns>The instance represented by the bytes in the specified stream</returns>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="InvalidOperationException"/>
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.NonPublicMethods)]
        [return: NotNull]
        public TType Deserialize([DisallowNull] Stream stream,
                                 out UInt32 read) =>
            this.Deserialize(stream,
                             -1,
                             out read,
                             SerializationFinishAction.None);
        /// <summary>
        /// Deserializes the specified stream starting at the specified offset into an instance of type <typeparamref name="TType"/>.
        /// </summary>
        /// <param name="stream">The stream to deserialize the graph from.</param>
        /// <param name="offset">The offset in the stream where to begin reading.</param>
        /// <param name="read">The amount of elements read from the <paramref name="stream"/> parameter.</param>
        /// <returns>The instance represented by the bytes in the specified stream</returns>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="InvalidOperationException"/>
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.NonPublicMethods)]
        [return: NotNull]
        public TType Deserialize([DisallowNull] Stream stream, 
                                 in Int64 offset,
                                 out UInt32 read) =>
            this.Deserialize(stream, 
                             offset,
                             out read,
                             SerializationFinishAction.None);
        /// <summary>
        /// Deserializes the specified stream into an instance of type <typeparamref name="TType"/>.
        /// </summary>
        /// <param name="stream">The stream to deserialize the graph from.</param>
        /// <param name="actionAfter">The actions to perform after the reading operation has finished.</param>
        /// <returns>The instance represented by the bytes in the specified stream</returns>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="InvalidOperationException"/>
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.NonPublicMethods)]
        [return: NotNull]
        public TType Deserialize([DisallowNull] Stream stream, 
                                 in SerializationFinishAction actionAfter) =>
            this.Deserialize(stream, 
                             -1,
                             out UInt32 _,
                             actionAfter);
        /// <summary>
        /// Deserializes the specified stream starting at the specified offset into an instance of type <typeparamref name="TType"/>.
        /// </summary>
        /// <param name="stream">The stream to deserialize the graph from.</param>
        /// <param name="offset">The offset in the stream where to begin reading.</param>
        /// <param name="actionAfter">The actions to perform after the reading operation has finished.</param>
        /// <returns>The instance represented by the bytes in the specified stream</returns>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="InvalidOperationException"/>
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.NonPublicMethods)]
        [return: NotNull]
        public TType Deserialize([DisallowNull] Stream stream,
                                 in Int64 offset,
                                 in SerializationFinishAction actionAfter) =>
            this.Deserialize(stream,
                             offset,
                             out UInt32 _,
                             SerializationFinishAction.None);
        /// <summary>
        /// Deserializes the specified stream starting at the specified offset into an instance of type <typeparamref name="TType"/>.
        /// </summary>
        /// <param name="stream">The stream to deserialize the graph from.</param>
        /// <param name="read">The amount of elements read from the <paramref name="stream"/> parameter.</param>
        /// <param name="actionAfter">The actions to perform after the reading operation has finished.</param>
        /// <returns>The instance represented by the bytes in the specified stream</returns>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="InvalidOperationException"/>
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.NonPublicMethods)]
        [return: NotNull]
        public TType Deserialize([DisallowNull] Stream stream,
                                 out UInt32 read, 
                                 in SerializationFinishAction actionAfter) =>
            this.Deserialize(stream, 
                             -1,
                             out read,
                             SerializationFinishAction.None);
        /// <summary>
        /// Deserializes the specified stream starting at the specified offset into an instance of type <typeparamref name="TType"/>.
        /// </summary>
        /// <param name="stream">The stream to deserialize the graph from.</param>
        /// <param name="offset">The offset in the stream where to begin reading.</param>
        /// <param name="read">The amount of elements read from the <paramref name="stream"/> parameter.</param>
        /// <param name="actionAfter">The actions to perform after the reading operation has finished.</param>
        /// <returns>The instance represented by the bytes in the specified stream</returns>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="InvalidOperationException"/>
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.NonPublicMethods)]
        [return: NotNull]
        public TType Deserialize([DisallowNull] Stream stream, 
                                 in Int64 offset, 
                                 out UInt32 read,
                                 in SerializationFinishAction actionAfter)
        {
            if (stream is null)
            {
                throw new ArgumentNullException(nameof(stream));
            }
            if (!stream.CanRead)
            {
                throw new InvalidOperationException(STREAM_DOES_NOT_SUPPORT_READING);
            }
            if (offset > -1 &&
                !stream.CanSeek)
            {
                throw new InvalidOperationException(STREAM_DOES_NOT_SUPPORT_SEEKING);
            }

            if (offset > -1)
            {
                stream.Seek(offset, 
                            SeekOrigin.Begin);
            }
            Int32 length = (Int32)(stream.Length - offset);
            Byte[] buffer = new Byte[length];
            stream.Read(buffer, 
                        0, 
                        length);
            TType result = this.Deserialize(buffer, 
                                            0,
                                            out read);
            if (actionAfter.HasFlag(SerializationFinishAction.FlushStream))
            {
                stream.Flush();
            }
            if (actionAfter.HasFlag(SerializationFinishAction.CloseStream))
            {
                stream.Close();
            }
            return result;
        }

        /// <summary>
        /// Tries to deserialize the specified stream starting at the specified offset into an instance of type <typeparamref name="TType"/>.
        /// </summary>
        /// <param name="stream">The stream to deserialize the graph from.</param>
        /// <param name="result">The instance represented by the bytes in the specified stream.</param>
        /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.NonPublicMethods)]
        public Boolean TryDeserialize([DisallowNull] Stream stream,
                                      [NotNullWhen(true)] out TType? result) =>
            this.TryDeserialize(stream,
                                -1,
                                out UInt32 _,
                                SerializationFinishAction.None,
                                out result);
        /// <summary>
        /// Tries to deserialize the specified stream starting at the specified offset into an instance of type <typeparamref name="TType"/>.
        /// </summary>
        /// <param name="stream">The stream to deserialize the graph from.</param>
        /// <param name="offset">The offset in the stream where to begin reading.</param>
        /// <param name="result">The instance represented by the bytes in the specified stream.</param>
        /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.NonPublicMethods)]
        public Boolean TryDeserialize([DisallowNull] Stream stream,
                                      in Int64 offset,
                                      [NotNullWhen(true)] out TType? result) =>
            this.TryDeserialize(stream,
                                offset,
                                out UInt32 _,
                                SerializationFinishAction.None,
                                out result);
        /// <summary>
        /// Tries to deserialize the specified stream starting at the specified offset into an instance of type <typeparamref name="TType"/>.
        /// </summary>
        /// <param name="stream">The stream to deserialize the graph from.</param>
        /// <param name="read">The amount of elements read from the <paramref name="stream"/> parameter.</param>
        /// <param name="result">The instance represented by the bytes in the specified stream.</param>
        /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.NonPublicMethods)]
        public Boolean TryDeserialize([DisallowNull] Stream stream,
                                      out UInt32 read,
                                      [NotNullWhen(true)] out TType? result) =>
            this.TryDeserialize(stream,
                                -1,
                                out read,
                                SerializationFinishAction.None,
                                out result);
        /// <summary>
        /// Tries to deserialize the specified stream starting at the specified offset into an instance of type <typeparamref name="TType"/>.
        /// </summary>
        /// <param name="stream">The stream to deserialize the graph from.</param>
        /// <param name="offset">The offset in the stream where to begin reading.</param>
        /// <param name="read">The amount of elements read from the <paramref name="stream"/> parameter.</param>
        /// <param name="result">The instance represented by the bytes in the specified stream.</param>
        /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.NonPublicMethods)]
        public Boolean TryDeserialize([DisallowNull] Stream stream,
                                      in Int64 offset,
                                      out UInt32 read,
                                      [NotNullWhen(true)] out TType? result) =>
            this.TryDeserialize(stream,
                                offset,
                                out read,
                                SerializationFinishAction.None,
                                out result);
        /// <summary>
        /// Tries to deserialize the specified stream starting at the specified offset into an instance of type <typeparamref name="TType"/>.
        /// </summary>
        /// <param name="stream">The stream to deserialize the graph from.</param>
        /// <param name="actionAfter">The actions to perform after the reading operation has finished.</param>
        /// <param name="result">The instance represented by the bytes in the specified stream.</param>
        /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.NonPublicMethods)]
        public Boolean TryDeserialize([DisallowNull] Stream stream,
                                      in SerializationFinishAction actionAfter,
                                      [NotNullWhen(true)] out TType? result) =>
            this.TryDeserialize(stream,
                                -1,
                                out UInt32 _,
                                actionAfter,
                                out result);
        /// <summary>
        /// Tries to deserialize the specified stream starting at the specified offset into an instance of type <typeparamref name="TType"/>.
        /// </summary>
        /// <param name="stream">The stream to deserialize the graph from.</param>
        /// <param name="offset">The offset in the stream where to begin reading.</param>
        /// <param name="actionAfter">The actions to perform after the reading operation has finished.</param>
        /// <param name="result">The instance represented by the bytes in the specified stream.</param>
        /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.NonPublicMethods)]
        public Boolean TryDeserialize([DisallowNull] Stream stream,
                                      in Int64 offset,
                                      in SerializationFinishAction actionAfter,
                                      [NotNullWhen(true)] out TType? result) =>
            this.TryDeserialize(stream,
                                offset,
                                out UInt32 _,
                                actionAfter,
                                out result);
        /// <summary>
        /// Tries to deserialize the specified stream starting at the specified offset into an instance of type <typeparamref name="TType"/>.
        /// </summary>
        /// <param name="stream">The stream to deserialize the graph from.</param>
        /// <param name="read">The amount of elements read from the <paramref name="stream"/> parameter.</param>
        /// <param name="actionAfter">The actions to perform after the reading operation has finished.</param>
        /// <param name="result">The instance represented by the bytes in the specified stream.</param>
        /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.NonPublicMethods)]
        public Boolean TryDeserialize([DisallowNull] Stream stream,
                                      out UInt32 read,
                                      in SerializationFinishAction actionAfter,
                                      [NotNullWhen(true)] out TType? result) =>
            this.TryDeserialize(stream, 
                                -1,
                                out read,
                                actionAfter, 
                                out result);
        /// <summary>
        /// Tries to deserialize the specified stream starting at the specified offset into an instance of type <typeparamref name="TType"/>.
        /// </summary>
        /// <param name="stream">The stream to deserialize the graph from.</param>
        /// <param name="offset">The offset in the stream where to begin reading.</param>
        /// <param name="read">The amount of elements read from the <paramref name="stream"/> parameter.</param>
        /// <param name="actionAfter">The actions to perform after the reading operation has finished.</param>
        /// <param name="result">The instance represented by the bytes in the specified stream.</param>
        /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.NonPublicMethods)]
        public Boolean TryDeserialize([DisallowNull] Stream stream, 
                                      in Int64 offset,
                                      out UInt32 read, 
                                      in SerializationFinishAction actionAfter, 
                                      [NotNullWhen(true)] out TType? result)
        {
            try
            {
                result = this.Deserialize(stream, 
                                          offset, 
                                          out read,
                                          actionAfter);
                return true;
            }
            catch
            {
                read = 0;
                result = null;
                return false;
            }
        }
    }
}

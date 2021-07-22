using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Narumikazuchi.Serialization.Bytes
{
    /// <summary>
    /// A serializer for classes that implement <see cref="IByteSerializable"/>.
    /// </summary>
    public class ByteSerializer<TType> : ISerializer<TType> where TType : class, IByteSerializable
    {
        #region Serialize

        private static Byte[] SerializeInternal(TType graph)
        {
            Type? @base = typeof(TType).BaseType;
            if (@base is not null &&
                @base.GetInterface(nameof(IByteSerializable)) is not null)
            {
#nullable disable
                Type baseTypeSerializer = typeof(ByteSerializer<>).MakeGenericType(@base);
                ConstructorInfo ctor = baseTypeSerializer.GetConstructor(Type.EmptyTypes);
                Object serializer = ctor.Invoke(Array.Empty<Object>());
                MethodInfo method = baseTypeSerializer.GetMethod("SerializeInternal", BindingFlags.Static | BindingFlags.NonPublic);
                Byte[] data = (Byte[])method.Invoke(serializer, new Object[] { graph });
                return data.Concat(graph.ToBytes()).ToArray();
#nullable enable
            }
            else
            {
                return graph.ToBytes().ToArray();
            }
        }

        #region Public Methods

        /// <summary>
        /// Serializes the specified graph into it's byte representation.
        /// </summary>
        /// <param name="graph">The graph to serialize into it's byte representation.</param>
        /// <returns>The specified graph in it's byte representation</returns>
        public Byte[] Serialize(TType graph)
        {
            if (typeof(TType).IsInterface)
            {
                throw new InvalidOperationException("The generic type TType can't be an interface.");
            }

            Byte[] data = SerializeInternal(graph);
            return BitConverter.GetBytes(data.Length).Concat(data).ToArray();
        }

        /// <summary>
        /// Serializes the specified graph into the specified stream.
        /// </summary>
        /// <param name="stream">The stream to serialize the graph into.</param>
        /// <param name="graph">The graph to serialize.</param>
        public void Serialize(Stream stream, TType graph) =>
            this.Serialize(stream, graph, -1, SerializationFinishAction.None);
        /// <summary>
        /// Serializes the specified graph into the specified stream starting at the specified offset in the stream.
        /// </summary>
        /// <param name="stream">The stream to serialize the graph into.</param>
        /// <param name="graph">The graph to serialize.</param>
        /// <param name="offset">The offset in the stream where to begin writing.</param>
        public void Serialize(Stream stream, TType graph, in Int64 offset) =>
            this.Serialize(stream, graph, offset, SerializationFinishAction.None);
        /// <summary>
        /// Serializes the specified graph into the specified stream.
        /// </summary>
        /// <param name="stream">The stream to serialize the graph into.</param>
        /// <param name="graph">The graph to serialize.</param>
        /// <param name="actionAfter">The actions to perform after the writing operation has finished.</param>
        public void Serialize(Stream stream, TType graph, in SerializationFinishAction actionAfter) =>
            this.Serialize(stream, graph, -1, actionAfter);
        /// <summary>
        /// Serializes the specified graph into the specified stream starting at the specified offset in the stream.
        /// </summary>
        /// <param name="stream">The stream to serialize the graph into.</param>
        /// <param name="graph">The graph to serialize.</param>
        /// <param name="offset">The offset in the stream where to begin writing.</param>
        /// <param name="actionAfter">The actions to perform after the writing operation has finished.</param>
        public void Serialize(Stream stream, TType graph, in Int64 offset, in SerializationFinishAction actionAfter)
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
                throw new InvalidOperationException("Stream can't be written to.");
            }
            if (offset > -1 &&
                !stream.CanSeek)
            {
                throw new InvalidOperationException("Stream doesn't support seeking.");
            }

            if (offset > -1)
            {
                stream.Seek(offset, SeekOrigin.Begin);
            }
            Byte[] data = this.Serialize(graph);
            stream.Write(data, 0, data.Length);
            if (actionAfter.HasFlag(SerializationFinishAction.FlushStream))
            {
                stream.Flush();
            }
            if (actionAfter.HasFlag(SerializationFinishAction.CloseStream))
            {
                stream.Close();
            }
        }

        /// <summary>
        /// Tries to serialize the specified graph into the specified stream.
        /// </summary>
        /// <param name="stream">The stream to serialize the graph into.</param>
        /// <param name="graph">The graph to serialize.</param>
        /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
        public Boolean TrySerialize(Stream stream, TType graph) =>
            this.TrySerialize(stream, graph, -1, SerializationFinishAction.None);
        /// <summary>
        /// Tries to serialize the specified graph into the specified stream starting at the specified offset in the stream.
        /// </summary>
        /// <param name="stream">The stream to serialize the graph into.</param>
        /// <param name="graph">The graph to serialize.</param>
        /// <param name="offset">The offset in the stream where to begin writing.</param>
        /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
        public Boolean TrySerialize(Stream stream, TType graph, in Int64 offset) =>
            this.TrySerialize(stream, graph, offset, SerializationFinishAction.None);
        /// <summary>
        /// Tries to serialize the specified graph into the specified stream.
        /// </summary>
        /// <param name="stream">The stream to serialize the graph into.</param>
        /// <param name="graph">The graph to serialize.</param>
        /// <param name="actionAfter">The actions to perform after the writing operation has finished.</param>
        /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
        public Boolean TrySerialize(Stream stream, TType graph, in SerializationFinishAction actionAfter) =>
            this.TrySerialize(stream, graph, -1, actionAfter);
        /// <summary>
        /// Tries to serialize the specified graph into the specified stream starting at the specified offset in the stream.
        /// </summary>
        /// <param name="stream">The stream to serialize the graph into.</param>
        /// <param name="graph">The graph to serialize.</param>
        /// <param name="offset">The offset in the stream where to begin writing.</param>
        /// <param name="actionAfter">The actions to perform after the writing operation has finished.</param>
        /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
        public Boolean TrySerialize(Stream stream, TType graph, in Int64 offset, in SerializationFinishAction actionAfter)
        {
            try
            {
                this.Serialize(stream, graph, offset, actionAfter);
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #endregion

        #region Deserialize

        private static UInt32 DeserializeUninitializedInternal(TType newObj, Byte[] bytes)
        {
            UInt32 offset = 0;
            Type? @base = typeof(TType).BaseType;
            if (@base is not null &&
                @base.GetInterface(nameof(IByteSerializable)) is not null)
            {
#nullable disable
                Type baseTypeSerializer = typeof(ByteSerializer<>).MakeGenericType(@base);
                ConstructorInfo ctor = baseTypeSerializer.GetConstructor(Type.EmptyTypes);
                Object serializer = ctor.Invoke(Array.Empty<Object>());
                MethodInfo method = baseTypeSerializer.GetMethod("DeserializeUninitializedInternal", BindingFlags.Static | BindingFlags.NonPublic);
                offset = (UInt32)method.Invoke(serializer, new Object[] { newObj, bytes });
#nullable enable
            }
            if (offset > 0)
            {
                Byte[] data = bytes.Skip(Convert.ToInt32(offset)).ToArray();
                return newObj.InitializeUninitializedState(data) + offset;
            }
            return newObj.InitializeUninitializedState(bytes);
        }

        private static UInt32 DeserializeInitializedInternal(TType newObj, Byte[] bytes)
        {
            UInt32 offset = 0;
            Type? @base = typeof(TType).BaseType;
            if (@base is not null &&
                @base.GetInterface(nameof(IByteSerializable)) is not null)
            {
#nullable disable
                Type baseTypeSerializer = typeof(ByteSerializer<>).MakeGenericType(@base);
                ConstructorInfo ctor = baseTypeSerializer.GetConstructor(Type.EmptyTypes);
                Object serializer = ctor.Invoke(Array.Empty<Object>());
                MethodInfo method = baseTypeSerializer.GetMethod("DeserializeInitializedInternal", BindingFlags.Static | BindingFlags.NonPublic);
                offset = (UInt32)method.Invoke(serializer, new Object[] { newObj, bytes });
#nullable enable
            }
            if (offset > 0)
            {
                Byte[] data = bytes.Skip(Convert.ToInt32(offset)).ToArray();
                return newObj.SetState(data) + offset;
            }
            return newObj.SetState(bytes);
        }

        #region Public Methods

        /// <summary>
        /// Deserializes the specified bytes starting at the specified offset into an instance of type <typeparamref name="TType"/>.
        /// </summary>
        /// <param name="bytes">The bytes supposed to represent an instance of <typeparamref name="TType"/>.</param>
        /// <param name="offset">The amount of elements in <paramref name="bytes"/> to ignore, starting from the first.</param>
        /// <returns>The instance represented by the specified bytes starting at the specified offset</returns>
        public TType Deserialize(Byte[] bytes, in Int32 offset)
        {
            if (typeof(TType).IsInterface)
            {
                throw new InvalidOperationException("The generic type TType can't be an interface.");
            }

            Int32 size = BitConverter.ToInt32(bytes, offset);
            Byte[] data = bytes.Skip(offset + 4).Take(size).ToArray();

            TType result;
            ConstructorInfo? constructor = typeof(TType).GetConstructor(BindingFlags.Public | BindingFlags.Instance, null, Array.Empty<Type>(), null);
            if (constructor is not null)
            {
                result = (TType)constructor.Invoke(Array.Empty<Object>());
                DeserializeInitializedInternal(result, data);
            }
            else
            {
                result = (TType)System.Runtime.Serialization.FormatterServices.GetSafeUninitializedObject(typeof(TType));
                DeserializeUninitializedInternal(result, data);
            }
            return result;
        }

        /// <summary>
        /// Deserializes the specified stream into an instance of type <typeparamref name="TType"/>.
        /// </summary>
        /// <param name="stream">The stream to deserialize the graph from.</param>
        /// <returns>The instance represented by the bytes in the specified stream</returns>
        public TType Deserialize(Stream stream) =>
            this.Deserialize(stream, -1, SerializationFinishAction.None);
        /// <summary>
        /// Deserializes the specified stream starting at the specified offset into an instance of type <typeparamref name="TType"/>.
        /// </summary>
        /// <param name="stream">The stream to deserialize the graph from.</param>
        /// <param name="offset">The offset in the stream where to begin reading.</param>
        /// <returns>The instance represented by the bytes in the specified stream</returns>
        public TType Deserialize(Stream stream, in Int64 offset) =>
            this.Deserialize(stream, offset, SerializationFinishAction.None);
        /// <summary>
        /// Deserializes the specified stream into an instance of type <typeparamref name="TType"/>.
        /// </summary>
        /// <param name="stream">The stream to deserialize the graph from.</param>
        /// <param name="actionAfter">The actions to perform after the reading operation has finished.</param>
        /// <returns>The instance represented by the bytes in the specified stream</returns>
        public TType Deserialize(Stream stream, in SerializationFinishAction actionAfter) =>
            this.Deserialize(stream, -1, actionAfter);
        /// <summary>
        /// Deserializes the specified stream starting at the specified offset into an instance of type <typeparamref name="TType"/>.
        /// </summary>
        /// <param name="stream">The stream to deserialize the graph from.</param>
        /// <param name="offset">The offset in the stream where to begin reading.</param>
        /// <param name="actionAfter">The actions to perform after the reading operation has finished.</param>
        /// <returns>The instance represented by the bytes in the specified stream</returns>
        public TType Deserialize(Stream stream, in Int64 offset, in SerializationFinishAction actionAfter)
        {
            if (stream is null)
            {
                throw new ArgumentNullException(nameof(stream));
            }
            if (!stream.CanRead)
            {
                throw new InvalidOperationException("Stream can't be read from.");
            }
            if (offset > -1 &&
                !stream.CanSeek)
            {
                throw new InvalidOperationException("Stream doesn't support seeking.");
            }

            if (offset > -1)
            {
                stream.Seek(offset, SeekOrigin.Begin);
            }
            Int32 length = (Int32)(stream.Length - offset);
            Byte[] buffer = new Byte[length];
            stream.Read(buffer, 0, length);
            TType result = this.Deserialize(buffer, 0);
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
        /// Tries to deserialize the specified stream into an instance of type <typeparamref name="TType"/>.
        /// </summary>
        /// <param name="stream">The stream to deserialize the graph from.</param>
        /// <param name="result">The instance represented by the bytes in the specified stream.</param>
        /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
        public Boolean TryDeserialize(Stream stream, out TType? result) =>
            this.TryDeserialize(stream, -1, SerializationFinishAction.None, out result);
        /// <summary>
        /// Tries to deserialize the specified stream starting at the specified offset into an instance of type <typeparamref name="TType"/>.
        /// </summary>
        /// <param name="stream">The stream to deserialize the graph from.</param>
        /// <param name="offset">The offset in the stream where to begin reading.</param>
        /// <param name="result">The instance represented by the bytes in the specified stream.</param>
        /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
        public Boolean TryDeserialize(Stream stream, in Int64 offset, out TType? result) =>
            this.TryDeserialize(stream, offset, SerializationFinishAction.None, out result);
        /// <summary>
        /// Tries to deserialize the specified stream into an instance of type <typeparamref name="TType"/>.
        /// </summary>
        /// <param name="stream">The stream to deserialize the graph from.</param>
        /// <param name="actionAfter">The actions to perform after the reading operation has finished.</param>
        /// <param name="result">The instance represented by the bytes in the specified stream.</param>
        /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
        public Boolean TryDeserialize(Stream stream, in SerializationFinishAction actionAfter, out TType? result) =>
            this.TryDeserialize(stream, -1, actionAfter, out result);
        /// <summary>
        /// Tries to deserialize the specified stream starting at the specified offset into an instance of type <typeparamref name="TType"/>.
        /// </summary>
        /// <param name="stream">The stream to deserialize the graph from.</param>
        /// <param name="offset">The offset in the stream where to begin reading.</param>
        /// <param name="actionAfter">The actions to perform after the reading operation has finished.</param>
        /// <param name="result">The instance represented by the bytes in the specified stream.</param>
        /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
        public Boolean TryDeserialize(Stream stream, in Int64 offset, in SerializationFinishAction actionAfter, out TType? result)
        {
            try
            {
                result = this.Deserialize(stream, offset, actionAfter);
                return true;
            }
            catch
            {
                result = null;
                return false;
            }
        }

        #endregion

        #endregion
    }
}

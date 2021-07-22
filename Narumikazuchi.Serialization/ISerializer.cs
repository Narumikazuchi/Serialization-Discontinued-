namespace Narumikazuchi.Serialization
{
    /// <summary>
    /// Represents a serializer for classes that implement either <see cref="ISerializable"/> directly or indirectly through another interface.
    /// </summary>
    public interface ISerializer<TType> where TType : class, ISerializable
    {
        #region Serialize

        /// <summary>
        /// Serializes the specified graph into the specified stream.
        /// </summary>
        /// <param name="stream">The stream to serialize the graph into.</param>
        /// <param name="graph">The graph to serialize.</param>
        public void Serialize(System.IO.Stream stream, TType graph);
        /// <summary>
        /// Serializes the specified graph into the specified stream starting at the specified offset in the stream.
        /// </summary>
        /// <param name="stream">The stream to serialize the graph into.</param>
        /// <param name="graph">The graph to serialize.</param>
        /// <param name="offset">The offset in the stream where to begin writing.</param>
        public void Serialize(System.IO.Stream stream, TType graph, in System.Int64 offset);
        /// <summary>
        /// Serializes the specified graph into the specified stream.
        /// </summary>
        /// <param name="stream">The stream to serialize the graph into.</param>
        /// <param name="graph">The graph to serialize.</param>
        /// <param name="actionAfter">The actions to perform after the writing operation has finished.</param>
        public void Serialize(System.IO.Stream stream, TType graph, in SerializationFinishAction actionAfter);
        /// <summary>
        /// Serializes the specified graph into the specified stream starting at the specified offset in the stream.
        /// </summary>
        /// <param name="stream">The stream to serialize the graph into.</param>
        /// <param name="graph">The graph to serialize.</param>
        /// <param name="offset">The offset in the stream where to begin writing.</param>
        /// <param name="actionAfter">The actions to perform after the writing operation has finished.</param>
        public void Serialize(System.IO.Stream stream, TType graph, in System.Int64 offset, in SerializationFinishAction actionAfter);

        #endregion

        #region TrySerialize

        /// <summary>
        /// Tries to serialize the specified graph into the specified stream.
        /// </summary>
        /// <param name="stream">The stream to serialize the graph into.</param>
        /// <param name="graph">The graph to serialize.</param>
        /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
        public System.Boolean TrySerialize(System.IO.Stream stream, TType graph);
        /// <summary>
        /// Tries to serialize the specified graph into the specified stream starting at the specified offset in the stream.
        /// </summary>
        /// <param name="stream">The stream to serialize the graph into.</param>
        /// <param name="graph">The graph to serialize.</param>
        /// <param name="offset">The offset in the stream where to begin writing.</param>
        /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
        public System.Boolean TrySerialize(System.IO.Stream stream, TType graph, in System.Int64 offset);
        /// <summary>
        /// Tries to serialize the specified graph into the specified stream.
        /// </summary>
        /// <param name="stream">The stream to serialize the graph into.</param>
        /// <param name="graph">The graph to serialize.</param>
        /// <param name="actionAfter">The actions to perform after the writing operation has finished.</param>
        /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
        public System.Boolean TrySerialize(System.IO.Stream stream, TType graph, in SerializationFinishAction actionAfter);
        /// <summary>
        /// Tries to serialize the specified graph into the specified stream starting at the specified offset in the stream.
        /// </summary>
        /// <param name="stream">The stream to serialize the graph into.</param>
        /// <param name="graph">The graph to serialize.</param>
        /// <param name="offset">The offset in the stream where to begin writing.</param>
        /// <param name="actionAfter">The actions to perform after the writing operation has finished.</param>
        /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
        public System.Boolean TrySerialize(System.IO.Stream stream, TType graph, in System.Int64 offset, in SerializationFinishAction actionAfter);

        #endregion

        #region Deserialize

        /// <summary>
        /// Deserializes the specified stream into an instance of type <typeparamref name="TType"/>.
        /// </summary>
        /// <param name="stream">The stream to deserialize the graph from.</param>
        /// <returns>The instance represented by the bytes in the specified stream</returns>
        public TType Deserialize(System.IO.Stream stream);
        /// <summary>
        /// Deserializes the specified stream starting at the specified offset into an instance of type <typeparamref name="TType"/>.
        /// </summary>
        /// <param name="stream">The stream to deserialize the graph from.</param>
        /// <param name="offset">The offset in the stream where to begin reading.</param>
        /// <returns>The instance represented by the bytes in the specified stream</returns>
        public TType Deserialize(System.IO.Stream stream, in System.Int64 offset);
        /// <summary>
        /// Deserializes the specified stream into an instance of type <typeparamref name="TType"/>.
        /// </summary>
        /// <param name="stream">The stream to deserialize the graph from.</param>
        /// <param name="actionAfter">The actions to perform after the reading operation has finished.</param>
        /// <returns>The instance represented by the bytes in the specified stream</returns>
        public TType Deserialize(System.IO.Stream stream, in SerializationFinishAction actionAfter);
        /// <summary>
        /// Deserializes the specified stream starting at the specified offset into an instance of type <typeparamref name="TType"/>.
        /// </summary>
        /// <param name="stream">The stream to deserialize the graph from.</param>
        /// <param name="offset">The offset in the stream where to begin reading.</param>
        /// <param name="actionAfter">The actions to perform after the reading operation has finished.</param>
        /// <returns>The instance represented by the bytes in the specified stream</returns>
        public TType Deserialize(System.IO.Stream stream, in System.Int64 offset, in SerializationFinishAction actionAfter);

        #endregion

        #region TryDeserialize

        /// <summary>
        /// Tries to deserialize the specified stream into an instance of type <typeparamref name="TType"/>.
        /// </summary>
        /// <param name="stream">The stream to deserialize the graph from.</param>
        /// <param name="result">The instance represented by the bytes in the specified stream.</param>
        /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
        public System.Boolean TryDeserialize(System.IO.Stream stream, out TType? result);
        /// <summary>
        /// Tries to deserialize the specified stream starting at the specified offset into an instance of type <typeparamref name="TType"/>.
        /// </summary>
        /// <param name="stream">The stream to deserialize the graph from.</param>
        /// <param name="offset">The offset in the stream where to begin reading.</param>
        /// <param name="result">The instance represented by the bytes in the specified stream.</param>
        /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
        public System.Boolean TryDeserialize(System.IO.Stream stream, in System.Int64 offset, out TType? result);
        /// <summary>
        /// Tries to deserialize the specified stream into an instance of type <typeparamref name="TType"/>.
        /// </summary>
        /// <param name="stream">The stream to deserialize the graph from.</param>
        /// <param name="actionAfter">The actions to perform after the reading operation has finished.</param>
        /// <param name="result">The instance represented by the bytes in the specified stream.</param>
        /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
        public System.Boolean TryDeserialize(System.IO.Stream stream, in SerializationFinishAction actionAfter, out TType? result);
        /// <summary>
        /// Tries to deserialize the specified stream starting at the specified offset into an instance of type <typeparamref name="TType"/>.
        /// </summary>
        /// <param name="stream">The stream to deserialize the graph from.</param>
        /// <param name="offset">The offset in the stream where to begin reading.</param>
        /// <param name="actionAfter">The actions to perform after the reading operation has finished.</param>
        /// <param name="result">The instance represented by the bytes in the specified stream.</param>
        /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
        public System.Boolean TryDeserialize(System.IO.Stream stream, in System.Int64 offset, in SerializationFinishAction actionAfter, out TType? result);

        #endregion
    }
}
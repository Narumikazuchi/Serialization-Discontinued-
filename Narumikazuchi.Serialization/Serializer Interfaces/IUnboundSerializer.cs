namespace Narumikazuchi.Serialization
{
    /// <summary>
    /// Represents a serializer for classes that you have no access to. 
    /// It allows you to provide delegates to write and read the state instead of relying on an attribute or an interface implementation.
    /// </summary>
    public interface IUnboundSerializer : ISerializer
    {
        /// <summary>
        /// Serializes the specified graph into the specified stream starting at the specified offset in the stream.
        /// </summary>
        /// <param name="stream">The stream to serialize the graph into.</param>
        /// <param name="graph">The graph to serialize.</param>
        /// <param name="getSerializationData">The <see langword="delegate"/> which specifies the members to serialize.</param>
        /// <returns>The amount of bytes written</returns>
        public System.UInt64 Serialize<TAny>([System.Diagnostics.CodeAnalysis.DisallowNull] System.IO.Stream stream,
                                             [System.Diagnostics.CodeAnalysis.DisallowNull] TAny graph,
                                             [System.Diagnostics.CodeAnalysis.DisallowNull] System.Action<TAny, SerializationInfo> getSerializationData);
        /// <summary>
        /// Serializes the specified graph into the specified stream starting at the specified offset in the stream.
        /// </summary>
        /// <param name="stream">The stream to serialize the graph into.</param>
        /// <param name="graph">The graph to serialize.</param>
        /// <param name="offset">The offset in the stream where to begin writing.</param>
        /// <param name="getSerializationData">The <see langword="delegate"/> which specifies the members to serialize.</param>
        /// <returns>The amount of bytes written</returns>
        public System.UInt64 Serialize<TAny>([System.Diagnostics.CodeAnalysis.DisallowNull] System.IO.Stream stream,
                                             [System.Diagnostics.CodeAnalysis.DisallowNull] TAny graph,
                                             [System.Diagnostics.CodeAnalysis.DisallowNull] System.Action<TAny, SerializationInfo> getSerializationData,
                                             in System.Int64 offset);
        /// <summary>
        /// Serializes the specified graph into the specified stream starting at the specified offset in the stream.
        /// </summary>
        /// <param name="stream">The stream to serialize the graph into.</param>
        /// <param name="graph">The graph to serialize.</param>
        /// <param name="getSerializationData">The <see langword="delegate"/> which specifies the members to serialize.</param>
        /// <param name="actionAfter">The actions to perform after the writing operation has finished.</param>
        /// <returns>The amount of bytes written</returns>
        public System.UInt64 Serialize<TAny>([System.Diagnostics.CodeAnalysis.DisallowNull] System.IO.Stream stream,
                                             [System.Diagnostics.CodeAnalysis.DisallowNull] TAny graph,
                                             [System.Diagnostics.CodeAnalysis.DisallowNull] System.Action<TAny, SerializationInfo> getSerializationData,
                                             in SerializationFinishAction actionAfter);
        /// <summary>
        /// Serializes the specified graph into the specified stream starting at the specified offset in the stream.
        /// </summary>
        /// <param name="stream">The stream to serialize the graph into.</param>
        /// <param name="graph">The graph to serialize.</param>
        /// <param name="offset">The offset in the stream where to begin writing.</param>
        /// <param name="getSerializationData">The <see langword="delegate"/> which specifies the members to serialize.</param>
        /// <param name="actionAfter">The actions to perform after the writing operation has finished.</param>
        /// <returns>The amount of bytes written</returns>
        public System.UInt64 Serialize<TAny>([System.Diagnostics.CodeAnalysis.DisallowNull] System.IO.Stream stream,
                                             [System.Diagnostics.CodeAnalysis.DisallowNull] TAny graph,
                                             [System.Diagnostics.CodeAnalysis.DisallowNull] System.Action<TAny, SerializationInfo> getSerializationData,
                                             in System.Int64 offset,
                                             in SerializationFinishAction actionAfter);

        /// <summary>
        /// Serializes the specified graph into the specified stream starting at the specified offset in the stream.
        /// </summary>
        /// <param name="stream">The stream to serialize the graph into.</param>
        /// <param name="graph">The graph to serialize.</param>
        /// <param name="getSerializationData">The <see langword="delegate"/> which specifies the members to serialize.</param>
        /// <returns>The amount of bytes written</returns>
        public System.Boolean TrySerialize<TAny>([System.Diagnostics.CodeAnalysis.DisallowNull] System.IO.Stream stream,
                                                 [System.Diagnostics.CodeAnalysis.DisallowNull] TAny graph,
                                                 [System.Diagnostics.CodeAnalysis.DisallowNull] System.Action<TAny, SerializationInfo> getSerializationData);
        /// <summary>
        /// Serializes the specified graph into the specified stream starting at the specified offset in the stream.
        /// </summary>
        /// <param name="stream">The stream to serialize the graph into.</param>
        /// <param name="graph">The graph to serialize.</param>
        /// <param name="offset">The offset in the stream where to begin writing.</param>
        /// <param name="getSerializationData">The <see langword="delegate"/> which specifies the members to serialize.</param>
        /// <returns>The amount of bytes written</returns>
        public System.Boolean TrySerialize<TAny>([System.Diagnostics.CodeAnalysis.DisallowNull] System.IO.Stream stream,
                                                 [System.Diagnostics.CodeAnalysis.DisallowNull] TAny graph,
                                                 [System.Diagnostics.CodeAnalysis.DisallowNull] System.Action<TAny, SerializationInfo> getSerializationData,
                                                 in System.Int64 offset);
        /// <summary>
        /// Serializes the specified graph into the specified stream starting at the specified offset in the stream.
        /// </summary>
        /// <param name="stream">The stream to serialize the graph into.</param>
        /// <param name="graph">The graph to serialize.</param>
        /// <param name="written">The amount of bytes written.</param>
        /// <param name="getSerializationData">The <see langword="delegate"/> which specifies the members to serialize.</param>
        /// <returns>The amount of bytes written</returns>
        public System.Boolean TrySerialize<TAny>([System.Diagnostics.CodeAnalysis.DisallowNull] System.IO.Stream stream,
                                                 [System.Diagnostics.CodeAnalysis.DisallowNull] TAny graph,
                                                 [System.Diagnostics.CodeAnalysis.DisallowNull] System.Action<TAny, SerializationInfo> getSerializationData,
                                                 out System.UInt64 written);
        /// <summary>
        /// Serializes the specified graph into the specified stream starting at the specified offset in the stream.
        /// </summary>
        /// <param name="stream">The stream to serialize the graph into.</param>
        /// <param name="graph">The graph to serialize.</param>
        /// <param name="offset">The offset in the stream where to begin writing.</param>
        /// <param name="written">The amount of bytes written.</param>
        /// <param name="getSerializationData">The <see langword="delegate"/> which specifies the members to serialize.</param>
        /// <returns>The amount of bytes written</returns>
        public System.Boolean TrySerialize<TAny>([System.Diagnostics.CodeAnalysis.DisallowNull] System.IO.Stream stream,
                                                 [System.Diagnostics.CodeAnalysis.DisallowNull] TAny graph,
                                                 [System.Diagnostics.CodeAnalysis.DisallowNull] System.Action<TAny, SerializationInfo> getSerializationData,
                                                 in System.Int64 offset,
                                                 out System.UInt64 written);
        /// <summary>
        /// Serializes the specified graph into the specified stream starting at the specified offset in the stream.
        /// </summary>
        /// <param name="stream">The stream to serialize the graph into.</param>
        /// <param name="graph">The graph to serialize.</param>
        /// <param name="getSerializationData">The <see langword="delegate"/> which specifies the members to serialize.</param>
        /// <param name="actionAfter">The actions to perform after the writing operation has finished.</param>
        /// <returns>The amount of bytes written</returns>
        public System.Boolean TrySerialize<TAny>([System.Diagnostics.CodeAnalysis.DisallowNull] System.IO.Stream stream,
                                                 [System.Diagnostics.CodeAnalysis.DisallowNull] TAny graph,
                                                 [System.Diagnostics.CodeAnalysis.DisallowNull] System.Action<TAny, SerializationInfo> getSerializationData,
                                                 in SerializationFinishAction actionAfter);
        /// <summary>
        /// Serializes the specified graph into the specified stream starting at the specified offset in the stream.
        /// </summary>
        /// <param name="stream">The stream to serialize the graph into.</param>
        /// <param name="graph">The graph to serialize.</param>
        /// <param name="offset">The offset in the stream where to begin writing.</param>
        /// <param name="getSerializationData">The <see langword="delegate"/> which specifies the members to serialize.</param>
        /// <param name="actionAfter">The actions to perform after the writing operation has finished.</param>
        /// <returns>The amount of bytes written</returns>
        public System.Boolean TrySerialize<TAny>([System.Diagnostics.CodeAnalysis.DisallowNull] System.IO.Stream stream,
                                                 [System.Diagnostics.CodeAnalysis.DisallowNull] TAny graph,
                                                 [System.Diagnostics.CodeAnalysis.DisallowNull] System.Action<TAny, SerializationInfo> getSerializationData,
                                                 in System.Int64 offset,
                                                 in SerializationFinishAction actionAfter);
        /// <summary>
        /// Serializes the specified graph into the specified stream starting at the specified offset in the stream.
        /// </summary>
        /// <param name="stream">The stream to serialize the graph into.</param>
        /// <param name="graph">The graph to serialize.</param>
        /// <param name="written">The amount of bytes written.</param>
        /// <param name="getSerializationData">The <see langword="delegate"/> which specifies the members to serialize.</param>
        /// <param name="actionAfter">The actions to perform after the writing operation has finished.</param>
        /// <returns>The amount of bytes written</returns>
        public System.Boolean TrySerialize<TAny>([System.Diagnostics.CodeAnalysis.DisallowNull] System.IO.Stream stream,
                                                 [System.Diagnostics.CodeAnalysis.DisallowNull] TAny graph,
                                                 [System.Diagnostics.CodeAnalysis.DisallowNull] System.Action<TAny, SerializationInfo> getSerializationData,
                                                 out System.UInt64 written,
                                                 in SerializationFinishAction actionAfter);
        /// <summary>
        /// Serializes the specified graph into the specified stream starting at the specified offset in the stream.
        /// </summary>
        /// <param name="stream">The stream to serialize the graph into.</param>
        /// <param name="graph">The graph to serialize.</param>
        /// <param name="offset">The offset in the stream where to begin writing.</param>
        /// <param name="written">The amount of bytes written.</param>
        /// <param name="getSerializationData">The <see langword="delegate"/> which specifies the members to serialize.</param>
        /// <param name="actionAfter">The actions to perform after the writing operation has finished.</param>
        /// <returns>The amount of bytes written</returns>
        public System.Boolean TrySerialize<TAny>([System.Diagnostics.CodeAnalysis.DisallowNull] System.IO.Stream stream,
                                                 [System.Diagnostics.CodeAnalysis.DisallowNull] TAny graph,
                                                 [System.Diagnostics.CodeAnalysis.DisallowNull] System.Action<TAny, SerializationInfo> getSerializationData,
                                                 in System.Int64 offset,
                                                 out System.UInt64 written,
                                                 in SerializationFinishAction actionAfter);

        /// <summary>
        /// Deserializes the specified stream starting at the specified offset into an instance of type <typeparamref name="TAny"/>.
        /// </summary>
        /// <param name="stream">The stream to deserialize the graph from.</param>
        /// <param name="constructFromSerializationData">The <see langword="delegate"/> which specifies how to deserialize the state into an object.</param>
        /// <returns>The instance represented by the bytes in the specified stream</returns>
        public TAny Deserialize<TAny>([System.Diagnostics.CodeAnalysis.DisallowNull] System.IO.Stream stream,
                                      [System.Diagnostics.CodeAnalysis.DisallowNull] System.Func<SerializationInfo, TAny> constructFromSerializationData);
        /// <summary>
        /// Deserializes the specified stream starting at the specified offset into an instance of type <typeparamref name="TAny"/>.
        /// </summary>
        /// <param name="stream">The stream to deserialize the graph from.</param>
        /// <param name="constructFromSerializationData">The <see langword="delegate"/> which specifies how to deserialize the state into an object.</param>
        /// <param name="offset">The offset in the stream where to begin reading.</param>
        /// <returns>The instance represented by the bytes in the specified stream</returns>
        public TAny Deserialize<TAny>([System.Diagnostics.CodeAnalysis.DisallowNull] System.IO.Stream stream,
                                      [System.Diagnostics.CodeAnalysis.DisallowNull] System.Func<SerializationInfo, TAny> constructFromSerializationData,
                                      in System.Int64 offset);
        /// <summary>
        /// Deserializes the specified stream starting at the specified offset into an instance of type <typeparamref name="TAny"/>.
        /// </summary>
        /// <param name="stream">The stream to deserialize the graph from.</param>
        /// <param name="constructFromSerializationData">The <see langword="delegate"/> which specifies how to deserialize the state into an object.</param>
        /// <param name="read">The amount of elements read from the <paramref name="stream"/> parameter.</param>
        /// <returns>The instance represented by the bytes in the specified stream</returns>
        public TAny Deserialize<TAny>([System.Diagnostics.CodeAnalysis.DisallowNull] System.IO.Stream stream,
                                      [System.Diagnostics.CodeAnalysis.DisallowNull] System.Func<SerializationInfo, TAny> constructFromSerializationData,
                                      out System.UInt64 read);
        /// <summary>
        /// Deserializes the specified stream starting at the specified offset into an instance of type <typeparamref name="TAny"/>.
        /// </summary>
        /// <param name="stream">The stream to deserialize the graph from.</param>
        /// <param name="constructFromSerializationData">The <see langword="delegate"/> which specifies how to deserialize the state into an object.</param>
        /// <param name="offset">The offset in the stream where to begin reading.</param>
        /// <param name="read">The amount of elements read from the <paramref name="stream"/> parameter.</param>
        /// <returns>The instance represented by the bytes in the specified stream</returns>
        public TAny Deserialize<TAny>([System.Diagnostics.CodeAnalysis.DisallowNull] System.IO.Stream stream,
                                      [System.Diagnostics.CodeAnalysis.DisallowNull] System.Func<SerializationInfo, TAny> constructFromSerializationData,
                                      in System.Int64 offset,
                                      out System.UInt64 read);
        /// <summary>
        /// Deserializes the specified stream starting at the specified offset into an instance of type <typeparamref name="TAny"/>.
        /// </summary>
        /// <param name="stream">The stream to deserialize the graph from.</param>
        /// <param name="constructFromSerializationData">The <see langword="delegate"/> which specifies how to deserialize the state into an object.</param>
        /// <param name="actionAfter">The actions to perform after the reading operation has finished.</param>
        /// <returns>The instance represented by the bytes in the specified stream</returns>
        public TAny Deserialize<TAny>([System.Diagnostics.CodeAnalysis.DisallowNull] System.IO.Stream stream,
                                      [System.Diagnostics.CodeAnalysis.DisallowNull] System.Func<SerializationInfo, TAny> constructFromSerializationData,
                                      in SerializationFinishAction actionAfter);
        /// <summary>
        /// Deserializes the specified stream starting at the specified offset into an instance of type <typeparamref name="TAny"/>.
        /// </summary>
        /// <param name="stream">The stream to deserialize the graph from.</param>
        /// <param name="constructFromSerializationData">The <see langword="delegate"/> which specifies how to deserialize the state into an object.</param>
        /// <param name="offset">The offset in the stream where to begin reading.</param>
        /// <param name="actionAfter">The actions to perform after the reading operation has finished.</param>
        /// <returns>The instance represented by the bytes in the specified stream</returns>
        public TAny Deserialize<TAny>([System.Diagnostics.CodeAnalysis.DisallowNull] System.IO.Stream stream,
                                      [System.Diagnostics.CodeAnalysis.DisallowNull] System.Func<SerializationInfo, TAny> constructFromSerializationData,
                                      in System.Int64 offset,
                                      in SerializationFinishAction actionAfter);
        /// <summary>
        /// Deserializes the specified stream starting at the specified offset into an instance of type <typeparamref name="TAny"/>.
        /// </summary>
        /// <param name="stream">The stream to deserialize the graph from.</param>
        /// <param name="constructFromSerializationData">The <see langword="delegate"/> which specifies how to deserialize the state into an object.</param>
        /// <param name="read">The amount of elements read from the <paramref name="stream"/> parameter.</param>
        /// <param name="actionAfter">The actions to perform after the reading operation has finished.</param>
        /// <returns>The instance represented by the bytes in the specified stream</returns>
        public TAny Deserialize<TAny>([System.Diagnostics.CodeAnalysis.DisallowNull] System.IO.Stream stream,
                                      [System.Diagnostics.CodeAnalysis.DisallowNull] System.Func<SerializationInfo, TAny> constructFromSerializationData,
                                      out System.UInt64 read,
                                      in SerializationFinishAction actionAfter);
        /// <summary>
        /// Deserializes the specified stream starting at the specified offset into an instance of type <typeparamref name="TAny"/>.
        /// </summary>
        /// <param name="stream">The stream to deserialize the graph from.</param>
        /// <param name="constructFromSerializationData">The <see langword="delegate"/> which specifies how to deserialize the state into an object.</param>
        /// <param name="offset">The offset in the stream where to begin reading.</param>
        /// <param name="read">The amount of elements read from the <paramref name="stream"/> parameter.</param>
        /// <param name="actionAfter">The actions to perform after the reading operation has finished.</param>
        /// <returns>The instance represented by the bytes in the specified stream</returns>
        public TAny Deserialize<TAny>([System.Diagnostics.CodeAnalysis.DisallowNull] System.IO.Stream stream,
                                      [System.Diagnostics.CodeAnalysis.DisallowNull] System.Func<SerializationInfo, TAny> constructFromSerializationData,
                                      in System.Int64 offset,
                                      out System.UInt64 read,
                                      in SerializationFinishAction actionAfter);

        /// <summary>
        /// Tries to deserialize the specified stream starting at the specified offset into an instance of type <typeparamref name="TAny"/>.
        /// </summary>
        /// <param name="stream">The stream to deserialize the graph from.</param>
        /// <param name="constructFromSerializationData">The <see langword="delegate"/> which specifies how to deserialize the state into an object.</param>
        /// <param name="result">The instance represented by the bytes in the specified stream.</param>
        /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
        public System.Boolean TryDeserialize<TAny>([System.Diagnostics.CodeAnalysis.DisallowNull] System.IO.Stream stream,
                                                   [System.Diagnostics.CodeAnalysis.DisallowNull] System.Func<SerializationInfo, TAny> constructFromSerializationData,
                                                   [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out TAny? result);
        /// <summary>
        /// Tries to deserialize the specified stream starting at the specified offset into an instance of type <typeparamref name="TAny"/>.
        /// </summary>
        /// <param name="stream">The stream to deserialize the graph from.</param>
        /// <param name="constructFromSerializationData">The <see langword="delegate"/> which specifies how to deserialize the state into an object.</param>
        /// <param name="offset">The offset in the stream where to begin reading.</param>
        /// <param name="result">The instance represented by the bytes in the specified stream.</param>
        /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
        public System.Boolean TryDeserialize<TAny>([System.Diagnostics.CodeAnalysis.DisallowNull] System.IO.Stream stream,
                                                   [System.Diagnostics.CodeAnalysis.DisallowNull] System.Func<SerializationInfo, TAny> constructFromSerializationData,
                                                   in System.Int64 offset,
                                                   [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out TAny? result);
        /// <summary>
        /// Tries to deserialize the specified stream starting at the specified offset into an instance of type <typeparamref name="TAny"/>.
        /// </summary>
        /// <param name="stream">The stream to deserialize the graph from.</param>
        /// <param name="constructFromSerializationData">The <see langword="delegate"/> which specifies how to deserialize the state into an object.</param>
        /// <param name="read">The amount of elements read from the <paramref name="stream"/> parameter.</param>
        /// <param name="result">The instance represented by the bytes in the specified stream.</param>
        /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
        public System.Boolean TryDeserialize<TAny>([System.Diagnostics.CodeAnalysis.DisallowNull] System.IO.Stream stream,
                                                   [System.Diagnostics.CodeAnalysis.DisallowNull] System.Func<SerializationInfo, TAny> constructFromSerializationData,
                                                   out System.UInt64 read,
                                                   [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out TAny? result);
        /// <summary>
        /// Tries to deserialize the specified stream starting at the specified offset into an instance of type <typeparamref name="TAny"/>.
        /// </summary>
        /// <param name="stream">The stream to deserialize the graph from.</param>
        /// <param name="constructFromSerializationData">The <see langword="delegate"/> which specifies how to deserialize the state into an object.</param>
        /// <param name="offset">The offset in the stream where to begin reading.</param>
        /// <param name="read">The amount of elements read from the <paramref name="stream"/> parameter.</param>
        /// <param name="result">The instance represented by the bytes in the specified stream.</param>
        /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
        public System.Boolean TryDeserialize<TAny>([System.Diagnostics.CodeAnalysis.DisallowNull] System.IO.Stream stream,
                                                   [System.Diagnostics.CodeAnalysis.DisallowNull] System.Func<SerializationInfo, TAny> constructFromSerializationData,
                                                   in System.Int64 offset,
                                                   out System.UInt64 read,
                                                   [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out TAny? result);
        /// <summary>
        /// Tries to deserialize the specified stream starting at the specified offset into an instance of type <typeparamref name="TAny"/>.
        /// </summary>
        /// <param name="stream">The stream to deserialize the graph from.</param>
        /// <param name="constructFromSerializationData">The <see langword="delegate"/> which specifies how to deserialize the state into an object.</param>
        /// <param name="actionAfter">The actions to perform after the reading operation has finished.</param>
        /// <param name="result">The instance represented by the bytes in the specified stream.</param>
        /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
        public System.Boolean TryDeserialize<TAny>([System.Diagnostics.CodeAnalysis.DisallowNull] System.IO.Stream stream,
                                                   [System.Diagnostics.CodeAnalysis.DisallowNull] System.Func<SerializationInfo, TAny> constructFromSerializationData,
                                                   in SerializationFinishAction actionAfter,
                                                   [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out TAny? result);
        /// <summary>
        /// Tries to deserialize the specified stream starting at the specified offset into an instance of type <typeparamref name="TAny"/>.
        /// </summary>
        /// <param name="stream">The stream to deserialize the graph from.</param>
        /// <param name="constructFromSerializationData">The <see langword="delegate"/> which specifies how to deserialize the state into an object.</param>
        /// <param name="offset">The offset in the stream where to begin reading.</param>
        /// <param name="actionAfter">The actions to perform after the reading operation has finished.</param>
        /// <param name="result">The instance represented by the bytes in the specified stream.</param>
        /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
        public System.Boolean TryDeserialize<TAny>([System.Diagnostics.CodeAnalysis.DisallowNull] System.IO.Stream stream,
                                                   [System.Diagnostics.CodeAnalysis.DisallowNull] System.Func<SerializationInfo, TAny> constructFromSerializationData,
                                                   in System.Int64 offset,
                                                   in SerializationFinishAction actionAfter,
                                                   [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out TAny? result);
        /// <summary>
        /// Tries to deserialize the specified stream starting at the specified offset into an instance of type <typeparamref name="TAny"/>.
        /// </summary>
        /// <param name="stream">The stream to deserialize the graph from.</param>
        /// <param name="constructFromSerializationData">The <see langword="delegate"/> which specifies how to deserialize the state into an object.</param>
        /// <param name="read">The amount of elements read from the <paramref name="stream"/> parameter.</param>
        /// <param name="actionAfter">The actions to perform after the reading operation has finished.</param>
        /// <param name="result">The instance represented by the bytes in the specified stream.</param>
        /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
        public System.Boolean TryDeserialize<TAny>([System.Diagnostics.CodeAnalysis.DisallowNull] System.IO.Stream stream,
                                                   [System.Diagnostics.CodeAnalysis.DisallowNull] System.Func<SerializationInfo, TAny> constructFromSerializationData,
                                                   out System.UInt64 read,
                                                   in SerializationFinishAction actionAfter,
                                                   [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out TAny? result);
        /// <summary>
        /// Tries to deserialize the specified stream starting at the specified offset into an instance of type <typeparamref name="TAny"/>.
        /// </summary>
        /// <param name="stream">The stream to deserialize the graph from.</param>
        /// <param name="constructFromSerializationData">The <see langword="delegate"/> which specifies how to deserialize the state into an object.</param>
        /// <param name="offset">The offset in the stream where to begin reading.</param>
        /// <param name="read">The amount of elements read from the <paramref name="stream"/> parameter.</param>
        /// <param name="actionAfter">The actions to perform after the reading operation has finished.</param>
        /// <param name="result">The instance represented by the bytes in the specified stream.</param>
        /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
        public System.Boolean TryDeserialize<TAny>([System.Diagnostics.CodeAnalysis.DisallowNull] System.IO.Stream stream,
                                                   [System.Diagnostics.CodeAnalysis.DisallowNull] System.Func<SerializationInfo, TAny> constructFromSerializationData,
                                                   in System.Int64 offset,
                                                   out System.UInt64 read,
                                                   in SerializationFinishAction actionAfter,
                                                   [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out TAny? result);
    }
}

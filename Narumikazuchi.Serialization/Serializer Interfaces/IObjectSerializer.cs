namespace Narumikazuchi.Serialization
{
    /// <summary>
    /// Represents a serializer for classes that are marked with the <see cref="CustomSerializableAttribute"/>.
    /// </summary>
    public interface IObjectSerializer : ISerializer
    {
        /// <summary>
        /// Serializes the specified graph into the specified stream starting at the specified offset in the stream.
        /// </summary>
        /// <param name="stream">The stream to serialize the graph into.</param>
        /// <param name="graph">The graph to serialize.</param>
        /// <param name="offset">The offset in the stream where to begin writing.</param>
        /// <returns>The amount of bytes written</returns>
        public System.UInt64 Serialize([System.Diagnostics.CodeAnalysis.DisallowNull] System.IO.Stream stream,
                                       [System.Diagnostics.CodeAnalysis.DisallowNull] System.Object graph,
                                       in System.Int64 offset);
        /// <summary>
        /// Serializes the specified graph into the specified stream.
        /// </summary>
        /// <param name="stream">The stream to serialize the graph into.</param>
        /// <param name="graph">The graph to serialize.</param>
        /// <param name="actionAfter">The actions to perform after the writing operation has finished.</param>
        /// <returns>The amount of bytes written</returns>
        public System.UInt64 Serialize([System.Diagnostics.CodeAnalysis.DisallowNull] System.IO.Stream stream,
                                       [System.Diagnostics.CodeAnalysis.DisallowNull] System.Object graph,
                                       in SerializationFinishAction actionAfter);
        /// <summary>
        /// Serializes the specified graph into the specified stream starting at the specified offset in the stream.
        /// </summary>
        /// <param name="stream">The stream to serialize the graph into.</param>
        /// <param name="graph">The graph to serialize.</param>
        /// <param name="offset">The offset in the stream where to begin writing.</param>
        /// <param name="actionAfter">The actions to perform after the writing operation has finished.</param>
        /// <returns>The amount of bytes written</returns>
        public System.UInt64 Serialize([System.Diagnostics.CodeAnalysis.DisallowNull] System.IO.Stream stream,
                                       [System.Diagnostics.CodeAnalysis.DisallowNull] System.Object graph,
                                       in System.Int64 offset,
                                       in SerializationFinishAction actionAfter);

        /// <summary>
        /// Tries to serialize the specified graph into the specified stream.
        /// </summary>
        /// <param name="stream">The stream to serialize the graph into.</param>
        /// <param name="graph">The graph to serialize.</param>
        /// <param name="offset">The offset in the stream where to begin writing.</param>
        /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
        public System.Boolean TrySerialize([System.Diagnostics.CodeAnalysis.DisallowNull] System.IO.Stream stream,
                                           [System.Diagnostics.CodeAnalysis.DisallowNull] System.Object graph,
                                           in System.Int64 offset);
        /// <summary>
        /// Tries to serialize the specified graph into the specified stream.
        /// </summary>
        /// <param name="stream">The stream to serialize the graph into.</param>
        /// <param name="graph">The graph to serialize.</param>
        /// <param name="written">The amount of bytes written.</param>
        /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
        public System.Boolean TrySerialize([System.Diagnostics.CodeAnalysis.DisallowNull] System.IO.Stream stream,
                                           [System.Diagnostics.CodeAnalysis.DisallowNull] System.Object graph,
                                           out System.UInt64 written);
        /// <summary>
        /// Tries to serialize the specified graph into the specified stream starting at the specified offset in the stream.
        /// </summary>
        /// <param name="stream">The stream to serialize the graph into.</param>
        /// <param name="graph">The graph to serialize.</param>
        /// <param name="offset">The offset in the stream where to begin writing.</param>
        /// <param name="written">The amount of bytes written.</param>
        /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
        public System.Boolean TrySerialize([System.Diagnostics.CodeAnalysis.DisallowNull] System.IO.Stream stream,
                                           [System.Diagnostics.CodeAnalysis.DisallowNull] System.Object graph,
                                           in System.Int64 offset,
                                           out System.UInt64 written);
        /// <summary>
        /// Tries to serialize the specified graph into the specified stream.
        /// </summary>
        /// <param name="stream">The stream to serialize the graph into.</param>
        /// <param name="graph">The graph to serialize.</param>
        /// <param name="actionAfter">The actions to perform after the writing operation has finished.</param>
        /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
        public System.Boolean TrySerialize([System.Diagnostics.CodeAnalysis.DisallowNull] System.IO.Stream stream,
                                           [System.Diagnostics.CodeAnalysis.DisallowNull] System.Object graph,
                                           in SerializationFinishAction actionAfter);
        /// <summary>
        /// Tries to serialize the specified graph into the specified stream.
        /// </summary>
        /// <param name="stream">The stream to serialize the graph into.</param>
        /// <param name="graph">The graph to serialize.</param>
        /// <param name="offset">The offset in the stream where to begin writing.</param>
        /// <param name="actionAfter">The actions to perform after the writing operation has finished.</param>
        /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
        public System.Boolean TrySerialize([System.Diagnostics.CodeAnalysis.DisallowNull] System.IO.Stream stream,
                                           [System.Diagnostics.CodeAnalysis.DisallowNull] System.Object graph,
                                           in System.Int64 offset,
                                           in SerializationFinishAction actionAfter);
        /// <summary>
        /// Tries to serialize the specified graph into the specified stream.
        /// </summary>
        /// <param name="stream">The stream to serialize the graph into.</param>
        /// <param name="graph">The graph to serialize.</param>
        /// <param name="written">The amount of bytes written.</param>
        /// <param name="actionAfter">The actions to perform after the writing operation has finished.</param>
        /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
        public System.Boolean TrySerialize([System.Diagnostics.CodeAnalysis.DisallowNull] System.IO.Stream stream,
                                           [System.Diagnostics.CodeAnalysis.DisallowNull] System.Object graph,
                                           out System.UInt64 written,
                                           in SerializationFinishAction actionAfter);
        /// <summary>
        /// Tries to serialize the specified graph into the specified stream starting at the specified offset in the stream.
        /// </summary>
        /// <param name="stream">The stream to serialize the graph into.</param>
        /// <param name="graph">The graph to serialize.</param>
        /// <param name="offset">The offset in the stream where to begin writing.</param>
        /// <param name="written">The amount of bytes written.</param>
        /// <param name="actionAfter">The actions to perform after the writing operation has finished.</param>
        /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
        public System.Boolean TrySerialize([System.Diagnostics.CodeAnalysis.DisallowNull] System.IO.Stream stream,
                                           [System.Diagnostics.CodeAnalysis.DisallowNull] System.Object graph,
                                           in System.Int64 offset,
                                           out System.UInt64 written,
                                           in SerializationFinishAction actionAfter);

        /// <summary>
        /// Deserializes the specified bytes starting at the specified offset into an instance of it's type.
        /// </summary>
        /// <param name="stream">The stream to deserialize the graph from.</param>
        /// <param name="read">The amount of elements read from the <paramref name="stream"/> parameter.</param>
        /// <returns>The instance represented by the specified bytes starting at the specified offset</returns>
        [return: System.Diagnostics.CodeAnalysis.NotNull]
        public System.Object Deserialize([System.Diagnostics.CodeAnalysis.DisallowNull] System.IO.Stream stream,
                                         out System.UInt64 read);
        /// <summary>
        /// Deserializes the specified stream starting at the specified offset into an instance of it's type.
        /// </summary>
        /// <param name="stream">The stream to deserialize the graph from.</param>
        /// <param name="offset">The offset in the stream where to begin reading.</param>
        /// <returns>The instance represented by the bytes in the specified stream</returns>
        [return: System.Diagnostics.CodeAnalysis.NotNull]
        public System.Object Deserialize([System.Diagnostics.CodeAnalysis.DisallowNull] System.IO.Stream stream,
                                         in System.Int64 offset);
        /// <summary>
        /// Deserializes the specified stream starting at the specified offset into an instance of it's type.
        /// </summary>
        /// <param name="stream">The stream to deserialize the graph from.</param>
        /// <param name="offset">The offset in the stream where to begin reading.</param>
        /// <param name="read">The amount of elements read from the <paramref name="stream"/> parameter.</param>
        /// <returns>The instance represented by the bytes in the specified stream</returns>
        [return: System.Diagnostics.CodeAnalysis.NotNull]
        public System.Object Deserialize([System.Diagnostics.CodeAnalysis.DisallowNull] System.IO.Stream stream,
                                         in System.Int64 offset,
                                         out System.UInt64 read);
        /// <summary>
        /// Deserializes the specified stream into an instance of it's type.
        /// </summary>
        /// <param name="stream">The stream to deserialize the graph from.</param>
        /// <param name="actionAfter">The actions to perform after the reading operation has finished.</param>
        /// <returns>The instance represented by the bytes in the specified stream</returns>
        [return: System.Diagnostics.CodeAnalysis.NotNull]
        public System.Object Deserialize([System.Diagnostics.CodeAnalysis.DisallowNull] System.IO.Stream stream,
                                         in SerializationFinishAction actionAfter);
        /// <summary>
        /// Deserializes the specified stream starting at the specified offset into an instance of it's type.
        /// </summary>
        /// <param name="stream">The stream to deserialize the graph from.</param>
        /// <param name="offset">The offset in the stream where to begin reading.</param>
        /// <param name="actionAfter">The actions to perform after the reading operation has finished.</param>
        /// <returns>The instance represented by the bytes in the specified stream</returns>
        [return: System.Diagnostics.CodeAnalysis.NotNull]
        public System.Object Deserialize([System.Diagnostics.CodeAnalysis.DisallowNull] System.IO.Stream stream,
                                         in System.Int64 offset,
                                         in SerializationFinishAction actionAfter);
        /// <summary>
        /// Deserializes the specified stream into an instance of it's type.
        /// </summary>
        /// <param name="stream">The stream to deserialize the graph from.</param>
        /// <param name="read">The amount of elements read from the <paramref name="stream"/> parameter.</param>
        /// <param name="actionAfter">The actions to perform after the reading operation has finished.</param>
        /// <returns>The instance represented by the bytes in the specified stream</returns>
        [return: System.Diagnostics.CodeAnalysis.NotNull]
        public System.Object Deserialize([System.Diagnostics.CodeAnalysis.DisallowNull] System.IO.Stream stream,
                                         out System.UInt64 read,
                                         in SerializationFinishAction actionAfter);
        /// <summary>
        /// Deserializes the specified stream starting at the specified offset into an instance of it's type.
        /// </summary>
        /// <param name="stream">The stream to deserialize the graph from.</param>
        /// <param name="offset">The offset in the stream where to begin reading.</param>
        /// <param name="read">The amount of elements read from the <paramref name="stream"/> parameter.</param>
        /// <param name="actionAfter">The actions to perform after the reading operation has finished.</param>
        /// <returns>The instance represented by the bytes in the specified stream</returns>
        [return: System.Diagnostics.CodeAnalysis.NotNull]
        public System.Object Deserialize([System.Diagnostics.CodeAnalysis.DisallowNull] System.IO.Stream stream,
                                         in System.Int64 offset,
                                         out System.UInt64 read,
                                         in SerializationFinishAction actionAfter);

        /// <summary>
        /// Tries to deserialize the specified stream starting at the specified offset into an instance of it's type.
        /// </summary>
        /// <param name="stream">The stream to deserialize the graph from.</param>
        /// <param name="offset">The offset in the stream where to begin reading.</param>
        /// <param name="result">The instance represented by the bytes in the specified stream.</param>
        /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
        public System.Boolean TryDeserialize([System.Diagnostics.CodeAnalysis.DisallowNull] System.IO.Stream stream,
                                             in System.Int64 offset,
                                             [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out System.Object? result);
        /// <summary>
        /// Tries to deserialize the specified stream starting at the specified offset into an instance of it's type.
        /// </summary>
        /// <param name="stream">The stream to deserialize the graph from.</param>
        /// <param name="read">The amount of elements read from the <paramref name="stream"/> parameter.</param>
        /// <param name="result">The instance represented by the bytes in the specified stream.</param>
        /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
        public System.Boolean TryDeserialize([System.Diagnostics.CodeAnalysis.DisallowNull] System.IO.Stream stream,
                                             out System.UInt64 read,
                                             [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out System.Object? result);
        /// <summary>
        /// Tries to deserialize the specified stream starting at the specified offset into an instance of it's type.
        /// </summary>
        /// <param name="stream">The stream to deserialize the graph from.</param>
        /// <param name="offset">The offset in the stream where to begin reading.</param>
        /// <param name="read">The amount of elements read from the <paramref name="stream"/> parameter.</param>
        /// <param name="result">The instance represented by the bytes in the specified stream.</param>
        /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
        public System.Boolean TryDeserialize([System.Diagnostics.CodeAnalysis.DisallowNull] System.IO.Stream stream,
                                             in System.Int64 offset,
                                             out System.UInt64 read,
                                             [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out System.Object? result);
        /// <summary>
        /// Tries to deserialize the specified stream into an instance of it's type.
        /// </summary>
        /// <param name="stream">The stream to deserialize the graph from.</param>
        /// <param name="actionAfter">The actions to perform after the reading operation has finished.</param>
        /// <param name="result">The instance represented by the bytes in the specified stream.</param>
        /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
        public System.Boolean TryDeserialize([System.Diagnostics.CodeAnalysis.DisallowNull] System.IO.Stream stream,
                                             in SerializationFinishAction actionAfter,
                                             [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out System.Object? result);
        /// <summary>
        /// Tries to deserialize the specified stream starting at the specified offset into an instance of it's type.
        /// </summary>
        /// <param name="stream">The stream to deserialize the graph from.</param>
        /// <param name="offset">The offset in the stream where to begin reading.</param>
        /// <param name="actionAfter">The actions to perform after the reading operation has finished.</param>
        /// <param name="result">The instance represented by the bytes in the specified stream.</param>
        /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
        public System.Boolean TryDeserialize([System.Diagnostics.CodeAnalysis.DisallowNull] System.IO.Stream stream,
                                             in System.Int64 offset,
                                             in SerializationFinishAction actionAfter,
                                             [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out System.Object? result);
        /// <summary>
        /// Tries to deserialize the specified stream starting at the specified offset into an instance of it's type.
        /// </summary>
        /// <param name="stream">The stream to deserialize the graph from.</param>
        /// <param name="read">The amount of elements read from the <paramref name="stream"/> parameter.</param>
        /// <param name="actionAfter">The actions to perform after the reading operation has finished.</param>
        /// <param name="result">The instance represented by the bytes in the specified stream.</param>
        /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
        public System.Boolean TryDeserialize([System.Diagnostics.CodeAnalysis.DisallowNull] System.IO.Stream stream,
                                             out System.UInt64 read,
                                             in SerializationFinishAction actionAfter,
                                             [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out System.Object? result);
        /// <summary>
        /// Tries to deserialize the specified stream starting at the specified offset into an instance of it's type.
        /// </summary>
        /// <param name="stream">The stream to deserialize the graph from.</param>
        /// <param name="offset">The offset in the stream where to begin reading.</param>
        /// <param name="read">The amount of elements read from the <paramref name="stream"/> parameter.</param>
        /// <param name="actionAfter">The actions to perform after the reading operation has finished.</param>
        /// <param name="result">The instance represented by the bytes in the specified stream.</param>
        /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
        public System.Boolean TryDeserialize([System.Diagnostics.CodeAnalysis.DisallowNull] System.IO.Stream stream,
                                             in System.Int64 offset,
                                             out System.UInt64 read,
                                             in SerializationFinishAction actionAfter,
                                             [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out System.Object? result);
    }
}

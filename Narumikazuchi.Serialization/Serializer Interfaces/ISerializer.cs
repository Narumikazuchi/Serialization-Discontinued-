namespace Narumikazuchi.Serialization
{
    /// <summary>
    /// Represents the basic frame for a serializer.
    /// </summary>
    public interface ISerializer
    {
        /// <summary>
        /// Serializes the specified graph into the specified stream.
        /// </summary>
        /// <param name="stream">The stream to serialize the graph into.</param>
        /// <param name="graph">The graph to serialize.</param>
        /// <returns>The amount of bytes written</returns>
        public System.UInt64 Serialize([System.Diagnostics.CodeAnalysis.DisallowNull] System.IO.Stream stream,
                                       [System.Diagnostics.CodeAnalysis.DisallowNull] System.Object graph);

        /// <summary>
        /// Tries to serialize the specified graph into the specified stream.
        /// </summary>
        /// <param name="stream">The stream to serialize the graph into.</param>
        /// <param name="graph">The graph to serialize.</param>
        /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
        public System.Boolean TrySerialize([System.Diagnostics.CodeAnalysis.DisallowNull] System.IO.Stream stream,
                                           [System.Diagnostics.CodeAnalysis.DisallowNull] System.Object graph);

        /// <summary>
        /// Deserializes the specified stream into an instance of it's type.
        /// </summary>
        /// <param name="stream">The stream to deserialize the graph from.</param>
        /// <returns>The instance represented by the bytes in the specified stream</returns>
        [return: System.Diagnostics.CodeAnalysis.NotNull]
        public System.Object Deserialize([System.Diagnostics.CodeAnalysis.DisallowNull] System.IO.Stream stream);

        /// <summary>
        /// Tries to deserialize the specified stream into an instance of it's type.
        /// </summary>
        /// <param name="stream">The stream to deserialize the graph from.</param>
        /// <param name="result">The instance represented by the bytes in the specified stream.</param>
        /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
        public System.Boolean TryDeserialize([System.Diagnostics.CodeAnalysis.DisallowNull] System.IO.Stream stream,
                                             [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out System.Object? result);

        /// <summary>
        /// Gets the types for which a <see cref="ISerializationStrategy{TReturn}"/> is registered for this serializer.
        /// </summary>
        public System.Collections.Generic.IReadOnlyCollection<System.Type> RegisteredStrategies { get; }
    }
}

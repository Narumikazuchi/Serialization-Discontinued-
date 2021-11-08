namespace Narumikazuchi.Serialization
{
    /// <summary>
    /// Represents an object that can be serialized.
    /// </summary>
    /// <remarks>
    /// If you plan to also deserialize a type your should use <see cref="ISerializable{TSelf}"/> instead, since this interface does not implement a way to deserialize.<para/>
    /// If you write a serializer for types that implement this interface, consider using <see langword="lock"/> on the object during the serialization process to prevent changes from other threads until the state retrieval finishes.
    /// </remarks>
    public interface ISerializable
    {
        /// <summary>
        /// Retrieves the state of this object for serialization.
        /// </summary>
        /// <param name="info">The object, which will hold the information to serialize.</param>
        [System.Diagnostics.Contracts.Pure]
        public abstract void GetSerializationData([System.Diagnostics.CodeAnalysis.DisallowNull] SerializationInfo info);
    }

    /// <summary>
    /// Represents an object that can be serialized and deserialized.
    /// </summary>
    public interface ISerializable<TSelf> : ISerializable
        where TSelf : ISerializable<TSelf>
    {
        /// <summary>
        /// Constructs an object of type <typeparamref name="TSelf"/> from the corresponding serialization data.
        /// </summary>
        /// <param name="info">The object, which holds the deserialized information.</param>
        /// <returns>The object of type <typeparamref name="TSelf"/> containing the state that is stored in the serialization data</returns>
        [return: System.Diagnostics.CodeAnalysis.NotNull]
        public static abstract TSelf ConstructFromSerializationData([System.Diagnostics.CodeAnalysis.DisallowNull] SerializationInfo info);
    }
}

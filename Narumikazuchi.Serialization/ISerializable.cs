namespace Narumikazuchi.Serialization
{
    /// <summary>
    /// Represents an object that can be serializedto a byte representation.
    /// </summary>
    public interface ISerializable
    {
        /// <summary>
        /// Serializes this instance into a <see cref="System.Byte"/>[].
        /// </summary>
        /// <returns>This instance represented in byte form</returns>
        [System.Diagnostics.Contracts.Pure]
        [return: System.Diagnostics.CodeAnalysis.NotNull]
        public abstract System.Byte[] ToBytes();
    }
}

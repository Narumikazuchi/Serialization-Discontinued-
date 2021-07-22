namespace Narumikazuchi.Serialization
{
    /// <summary>
    /// Represents an object that can be serializedto a byte representation.
    /// </summary>
    public interface ISerializable
    {
        #region Serialization

        /// <summary>
        /// Serializes this instance into a <see cref="System.Byte"/>[].
        /// </summary>
        /// <returns>This instance represented in byte form</returns>
        internal protected abstract System.Byte[] ToBytes();

        #endregion
    }
}

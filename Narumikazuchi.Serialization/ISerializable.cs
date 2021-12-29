namespace Narumikazuchi.Serialization;

/// <summary>
/// Represents an object that can be serialized.
/// </summary>
/// <remarks>
/// If you plan to also deserialize a type you should use <see cref="IDeserializable{TSelf}"/> instead, since this interface does not implement a way to deserialize.<para/>
/// If you write a serializer for types that implement this interface, consider using <see langword="lock"/> on the object during the serialization process to prevent changes from other threads until the state retrieval finishes.
/// </remarks>
public interface ISerializable
{
    /// <summary>
    /// Retrieves the state of this object for serialization.
    /// </summary>
    /// <param name="info">The object, which will hold the information to serialize.</param>
    [Pure]
    public void GetSerializationData([DisallowNull] ISerializationInfoAdder info);
}
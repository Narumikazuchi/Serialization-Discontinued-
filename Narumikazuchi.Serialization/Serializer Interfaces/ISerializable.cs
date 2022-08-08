namespace Narumikazuchi.Serialization;

/// <summary>
/// Represents an object that can be serialized.
/// </summary>
public interface ISerializable :
    ISerializationTarget
{
    /// <summary>
    /// Retrieves the state of this object for serialization.
    /// </summary>
    /// <param name="info">The object, which will hold the information to serialize.</param>
    [Pure]
    public void GetSerializationData([DisallowNull] WriteableSerializationInfo info);
}
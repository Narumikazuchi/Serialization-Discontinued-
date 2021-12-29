namespace Narumikazuchi.Serialization;

/// <summary>
/// Represents a serializer and deserializer for the specified type <typeparamref name="TSerializable"/>.
/// </summary>
public interface ISerializerDeserializer<TSerializable> :
    IDeserializer<TSerializable>,
    ISerializer<TSerializable>
{ }
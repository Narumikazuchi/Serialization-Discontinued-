namespace Narumikazuchi.Serialization.Bytes;

/// <summary>
/// Represents the functionality of an <see cref="IDeserializer{TSerializable}"/>,
/// which will deserialize the given array of <see cref="Byte"/> into it's managed object of type <typeparamref name="TSerializable"/>.
/// </summary>
public interface IByteDeserializer<TSerializable> :
    IDeserializer<TSerializable>
{ }
namespace Narumikazuchi.Serialization.Bytes;

public interface IByteSerializerDeserializer<TSerializable> :
    IByteSerializer<TSerializable>,
    IByteDeserializer<TSerializable>
{ }
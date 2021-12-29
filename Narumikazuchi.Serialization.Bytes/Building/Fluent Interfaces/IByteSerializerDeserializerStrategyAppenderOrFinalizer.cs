namespace Narumikazuchi.Serialization.Bytes;

public interface IByteSerializerDeserializerStrategyAppenderOrFinalizer<TSerializable> :
    IByteSerializerDeserializerStrategyAppender<TSerializable>
{
    public IByteSerializerDeserializer<TSerializable> Construct();
}

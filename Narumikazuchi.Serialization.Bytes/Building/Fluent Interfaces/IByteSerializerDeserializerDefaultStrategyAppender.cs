namespace Narumikazuchi.Serialization.Bytes;

public interface IByteSerializerDeserializerDefaultStrategyAppender<TSerializable> :
    IByteSerializerDeserializerStrategyAppender<TSerializable>
{
    public IByteSerializerDeserializerStrategyAppenderOrFinalizer<TSerializable> UseDefaultStrategies();
}

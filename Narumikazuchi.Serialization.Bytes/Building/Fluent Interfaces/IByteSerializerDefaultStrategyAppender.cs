namespace Narumikazuchi.Serialization.Bytes;

public interface IByteSerializerDefaultStrategyAppender<TSerializable> :
    IByteSerializerStrategyAppender<TSerializable>
{
    public IByteSerializerStrategyAppenderOrFinalizer<TSerializable> UseDefaultStrategies();
}

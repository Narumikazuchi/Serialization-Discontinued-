namespace Narumikazuchi.Serialization.Bytes;

public interface IByteDeserializerDefaultStrategyAppender<TSerializable> :
    IByteDeserializerStrategyAppender<TSerializable>
{
    public IByteDeserializerStrategyAppenderOrFinalizer<TSerializable> UseDefaultStrategies();
}
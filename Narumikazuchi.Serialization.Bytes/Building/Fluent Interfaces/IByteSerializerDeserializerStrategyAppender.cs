namespace Narumikazuchi.Serialization.Bytes;

public interface IByteSerializerDeserializerStrategyAppender<TSerializable>
{
    public IByteSerializerDeserializerStrategyAppenderOrFinalizer<TSerializable> UseStrategyForType<TFrom>([DisallowNull] ISerializationStrategy<Byte[], TFrom> strategy);

    public IByteSerializerDeserializerStrategyAppenderOrFinalizer<TSerializable> UseStrategies([DisallowNull] IEnumerable<KeyValuePair<Type, ISerializationStrategy<Byte[]>>> strategies);
}

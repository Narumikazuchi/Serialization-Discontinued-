namespace Narumikazuchi.Serialization.Bytes;

public interface IByteSerializerStrategyAppender<TSerializable>
{
    public IByteSerializerStrategyAppenderOrFinalizer<TSerializable> UseStrategyForType<TFrom>([DisallowNull] ISerializationStrategy<Byte[], TFrom> strategy);

    public IByteSerializerStrategyAppenderOrFinalizer<TSerializable> UseStrategies([DisallowNull] IEnumerable<KeyValuePair<Type, ISerializationStrategy<Byte[]>>> strategies);
}

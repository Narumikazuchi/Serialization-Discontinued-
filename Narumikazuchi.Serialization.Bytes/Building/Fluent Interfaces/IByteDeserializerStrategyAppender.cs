namespace Narumikazuchi.Serialization.Bytes;

public interface IByteDeserializerStrategyAppender<TSerializable>
{
    public IByteDeserializerStrategyAppenderOrFinalizer<TSerializable> UseStrategyForType<TFrom>([DisallowNull] ISerializationStrategy<Byte[], TFrom> strategy);

    public IByteDeserializerStrategyAppenderOrFinalizer<TSerializable> UseStrategies([DisallowNull] IEnumerable<KeyValuePair<Type, ISerializationStrategy<Byte[]>>> strategies);
}

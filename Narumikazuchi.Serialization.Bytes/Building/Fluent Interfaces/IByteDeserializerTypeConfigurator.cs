namespace Narumikazuchi.Serialization.Bytes;

public interface IByteDeserializerTypeConfigurator
{
    public IByteDeserializerDefaultStrategyAppender<TSerializable> ConfigureForForeignType<TSerializable>([DisallowNull] Func<ISerializationInfoGetter, TSerializable?> constructFromSerializationData);
    public IByteDeserializerDefaultStrategyAppender<TSerializable> ConfigureForForeignType<TSerializable>([DisallowNull] ISerializationStrategy<Byte[], TSerializable> strategy);

    public IByteDeserializerDefaultStrategyAppender<TSerializable> ConfigureForOwnedType<TSerializable>()
        where TSerializable : IDeserializable<TSerializable>;
    public IByteDeserializerDefaultStrategyAppender<TSerializable> ConfigureForOwnedType<TSerializable>(ISerializationStrategy<Byte[], TSerializable> strategy);
}

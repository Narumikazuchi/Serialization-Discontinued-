namespace Narumikazuchi.Serialization.Bytes;

public interface IByteSerializerDeserializerTypeConfigurator
{
    public IByteSerializerDeserializerDefaultStrategyAppender<TSerializable> ConfigureForForeignType<TSerializable>([DisallowNull] Action<TSerializable?, ISerializationInfoAdder> getSerializationData,
                                                                                                                    [DisallowNull] Func<ISerializationInfoGetter, TSerializable?> constructFromSerializationData);
    public IByteSerializerDeserializerDefaultStrategyAppender<TSerializable> ConfigureForForeignType<TSerializable>([DisallowNull] ISerializationStrategy<Byte[], TSerializable> strategy);

    public IByteSerializerDeserializerDefaultStrategyAppender<TSerializable> ConfigureForOwnedType<TSerializable>()
        where TSerializable : IDeserializable<TSerializable>;
    public IByteSerializerDeserializerDefaultStrategyAppender<TSerializable> ConfigureForOwnedType<TSerializable>(ISerializationStrategy<Byte[], TSerializable> strategy);
}
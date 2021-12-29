namespace Narumikazuchi.Serialization.Bytes;

public interface IByteSerializerTypeConfigurator
{
    public IByteSerializerDefaultStrategyAppender<TSerializable> ConfigureForForeignType<TSerializable>([DisallowNull] Action<TSerializable?, ISerializationInfoAdder> getSerializationData);
    public IByteSerializerDefaultStrategyAppender<TSerializable> ConfigureForForeignType<TSerializable>([DisallowNull] ISerializationStrategy<Byte[], TSerializable> strategy);

    public IByteSerializerDefaultStrategyAppender<TSerializable> ConfigureForOwnedType<TSerializable>()
        where TSerializable : ISerializable;
    public IByteSerializerDefaultStrategyAppender<TSerializable> ConfigureForOwnedType<TSerializable>(ISerializationStrategy<Byte[], TSerializable> strategy);
}

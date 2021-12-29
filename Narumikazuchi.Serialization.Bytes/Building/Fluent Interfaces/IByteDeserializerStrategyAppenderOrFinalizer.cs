namespace Narumikazuchi.Serialization.Bytes;

public interface IByteDeserializerStrategyAppenderOrFinalizer<TSerializable> :
    IByteDeserializerStrategyAppender<TSerializable>
{
    public IByteDeserializer<TSerializable> Construct();
}

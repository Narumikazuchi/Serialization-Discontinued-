namespace Narumikazuchi.Serialization.Bytes;

public interface IByteSerializerStrategyAppenderOrFinalizer<TSerializable> :
    IByteSerializerStrategyAppender<TSerializable>
{
    public IByteSerializer<TSerializable> Construct();
}

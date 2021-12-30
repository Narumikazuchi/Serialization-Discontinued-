namespace Narumikazuchi.Serialization.Bytes;

/// <summary>
/// Configures a <see cref="IByteSerializerDeserializer{TSerializable}"/> to use specified strategies for serialization.
/// </summary>
public interface IByteSerializerDeserializerDefaultStrategyAppender<TSerializable> :
    IByteSerializerDeserializerStrategyAppender<TSerializable>
{
    /// <summary>
    /// Configures the <see cref="IByteSerializerDeserializer{TSerializable}"/> to use the integrated default strategies for primitive types.
    /// </summary>
    /// <remarks>
    /// Default strategies will not override any prior custom strategies that have been configured. Furthermore every strategy targeting one
    /// of the types that the default strategies target will overwrite the default strategy with the custom one.
    /// </remarks>
    public IByteSerializerDeserializerStrategyAppenderOrFinalizer<TSerializable> UseDefaultStrategies();
}

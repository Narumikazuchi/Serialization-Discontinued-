namespace Narumikazuchi.Serialization.Bytes;

/// <summary>
/// Configures a <see cref="IByteDeserializer{TSerializable}"/> to use specified strategies for deserialization.
/// </summary>
public interface IByteDeserializerDefaultStrategyAppender<TSerializable> :
    IByteDeserializerStrategyAppender<TSerializable>
{
    /// <summary>
    /// Configures the <see cref="IByteDeserializer{TSerializable}"/> to use the integrated default strategies for primitive types.
    /// </summary>
    /// <remarks>
    /// Default strategies will not override any prior custom strategies that have been configured. Furthermore every strategy targeting one
    /// of the types that the default strategies target will overwrite the default strategy with the custom one.
    /// </remarks>
    public IByteDeserializerStrategyAppenderOrFinalizer<TSerializable> UseDefaultStrategies();
}
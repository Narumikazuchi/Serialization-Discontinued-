namespace Narumikazuchi.Serialization.Bytes;

/// <summary>
/// Configures a <see cref="IByteSerializer{TSerializable}"/> to use specified strategies for serialization.
/// </summary>
public interface IByteSerializerDefaultStrategyAppender<TSerializable> :
    IByteSerializerStrategyAppender<TSerializable>
{
    /// <summary>
    /// Configures the <see cref="IByteSerializer{TSerializable}"/> to use the integrated default strategies for primitive types.
    /// </summary>
    /// <remarks>
    /// Default strategies will not override any prior custom strategies that have been configured. Furthermore every strategy targeting one
    /// of the types that the default strategies target will overwrite the default strategy with the custom one.
    /// </remarks>
    public IByteSerializerStrategyAppenderOrFinalizer<TSerializable> UseDefaultStrategies();
}

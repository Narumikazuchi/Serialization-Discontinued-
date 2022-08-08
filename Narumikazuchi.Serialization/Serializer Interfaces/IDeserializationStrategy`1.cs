namespace Narumikazuchi.Serialization;

/// <summary>
/// Represents a strategy on how to serialize an object into an object of type <typeparamref name="TReturn"/>.
/// </summary>
public interface IDeserializationStrategy<TReturn> : 
    ITypeAppliedStrategy
{
    /// <summary>
    /// Deserializes the specified object into it's original type and state.
    /// </summary>
    /// <param name="input">The object to deserialize.</param>
    /// <returns>The represented objects original state.</returns>
    public Object? Deserialize(TReturn input);

    /// <summary>
    /// Gets the priority of this strategy. The strategy with the highest priority for a given type will be applied.
    /// </summary>
    public Int32 Priority { get; }
}
namespace Narumikazuchi.Serialization;

/// <summary>
/// Represents the basic frame for a serializer.
/// </summary>
public interface IUsesSerializationStrategies
{
    /// <summary>
    /// Gets the types for which a <see cref="ISerializationStrategy{TReturn}"/> is registered for this serializer.
    /// </summary>
    public IEnumerable<Type> RegisteredStrategies { get; }
}

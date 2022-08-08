namespace Narumikazuchi.Serialization;

/// <summary>
/// Represents a strategy on how to serialize an object into an object of type <typeparamref name="TReturn"/>.
/// </summary>
public interface ISerializationDeserializationStrategy<TReturn> :
    IDeserializationStrategy<TReturn>,
    ISerializationStrategy<TReturn>
{ }
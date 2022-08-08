namespace Narumikazuchi.Serialization;

/// <summary>
/// Represents a strategy on how to serialize an object of type <typeparamref name="TInput"/> into an object of type <typeparamref name="TReturn"/>.
/// </summary>
public interface ISerializationDeserializationStrategy<TReturn, TInput> :
    IDeserializationStrategy<TReturn, TInput>,
    ISerializationDeserializationStrategy<TReturn>,
    ISerializationStrategy<TReturn, TInput>
{ }
namespace Narumikazuchi.Serialization;

/// <summary>
/// Represents a strategy on how to serialize an object into an object of type <typeparamref name="TReturn"/>.
/// </summary>
public interface ISerializationStrategy<TReturn>
{
    /// <summary>
    /// Serializes the specified object into another object of type <typeparamref name="TReturn"/>, which represents the state of the object at the time of method invocation.
    /// </summary>
    /// <param name="input">The object to serialize.</param>
    /// <returns>An object of type <typeparamref name="TReturn"/> representing the state of the input object at the time of method invocation.</returns>
    public TReturn Serialize(Object? input);
}

/// <summary>
/// Represents a strategy on how to serialize an object of type <typeparamref name="TInput"/> into an object of type <typeparamref name="TReturn"/>.
/// </summary>
public interface ISerializationStrategy<TReturn, TInput> : 
    ISerializationStrategy<TReturn>
{
    /// <summary>
    /// Serializes the specified object of type <typeparamref name="TInput"/> into another object of type <typeparamref name="TReturn"/>, which represents the state of the object at the time of method invocation.
    /// </summary>
    /// <param name="input">The object to serialize.</param>
    /// <returns>An object of type <typeparamref name="TReturn"/> representing the state of the input object at the time of method invocation.</returns>
    public TReturn Serialize(TInput input);
}
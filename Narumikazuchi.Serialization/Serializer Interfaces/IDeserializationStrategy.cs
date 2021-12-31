﻿namespace Narumikazuchi.Serialization;

/// <summary>
/// Represents a strategy on how to serialize an object into an object of type <typeparamref name="TReturn"/>.
/// </summary>
public interface IDeserializationStrategy<TReturn>
{
    /// <summary>
    /// Deserializes the specified object into it's original type and state.
    /// </summary>
    /// <param name="input">The object to deserialize.</param>
    /// <returns>The represented objects original state.</returns>
    public Object? Deserialize(TReturn input);
}

/// <summary>
/// Represents a strategy on how to serialize an object of type <typeparamref name="TInput"/> into an object of type <typeparamref name="TReturn"/>.
/// </summary>
public interface IDeserializationStrategy<TReturn, TInput> :
    IDeserializationStrategy<TReturn>
{
    /// <summary>
    /// Deserializes the specified object into it's original type <typeparamref name="TInput"/> and state.
    /// </summary>
    /// <param name="input">The object to deserialize.</param>
    /// <returns>The represented objects original state.</returns>
    public new TInput Deserialize(TReturn input);
}
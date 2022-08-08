﻿namespace Narumikazuchi.Serialization;

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

    TReturn ISerializationStrategy<TReturn>.Serialize(Object? input)
    {
        if (input is not TInput value)
        {
            throw new InvalidCastException();
        }

        return this.Serialize(value);
    }

    Boolean ITypeAppliedStrategy.CanBeAppliedTo(Type type) =>
        type.IsAssignableTo(typeof(TInput));
}
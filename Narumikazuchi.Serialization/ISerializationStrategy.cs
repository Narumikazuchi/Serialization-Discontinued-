namespace Narumikazuchi.Serialization
{
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
        public TReturn Serialize(System.Object? input);

        /// <summary>
        /// Deserializes the specified object into it's original type and state.
        /// </summary>
        /// <param name="input">The object to deserialize.</param>
        /// <returns>The represented objects original state.</returns>
        public System.Object? Deserialize(TReturn input);
    }

    /// <summary>
    /// Represents a strategy on how to serialize an object of type <typeparamref name="TInput"/> into an object of type <typeparamref name="TReturn"/>.
    /// </summary>
    public interface ISerializationStrategy<TReturn, TInput> : ISerializationStrategy<TReturn>
    {
        /// <summary>
        /// Serializes the specified object of type <typeparamref name="TInput"/> into another object of type <typeparamref name="TReturn"/>, which represents the state of the object at the time of method invocation.
        /// </summary>
        /// <param name="input">The object to serialize.</param>
        /// <returns>An object of type <typeparamref name="TReturn"/> representing the state of the input object at the time of method invocation.</returns>
        public TReturn Serialize(TInput input);

        /// <summary>
        /// Deserializes the specified object into it's original type <typeparamref name="TInput"/> and state.
        /// </summary>
        /// <param name="input">The object to deserialize.</param>
        /// <returns>The represented objects original state.</returns>
        public new TInput Deserialize(TReturn input);
    }
}

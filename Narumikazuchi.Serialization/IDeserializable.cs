namespace Narumikazuchi.Serialization;

/// <summary>
/// Represents an object that can be deserialized.
/// </summary>
public interface IDeserializable<TSelf>
    where TSelf : IDeserializable<TSelf>
{
    /// <summary>
    /// Constructs an object of type <typeparamref name="TSelf"/> from the corresponding serialization data.
    /// </summary>
    /// <param name="info">The object, which holds the deserialized information.</param>
    /// <returns>The object of type <typeparamref name="TSelf"/> containing the state that is stored in the serialization data</returns>
    [return: NotNull]
    public static abstract TSelf ConstructFromSerializationData([DisallowNull] ISerializationInfoGetter info);
}
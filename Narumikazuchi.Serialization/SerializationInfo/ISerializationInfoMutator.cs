namespace Narumikazuchi.Serialization;

/// <summary>
/// Represents the functionality of an <see cref="ISerializationInfo"/> object to be mutated.
/// </summary>
public interface ISerializationInfoMutator :
    ISerializationInfoAdder,
    ISerializationInfoGetter,
    ISerializationInfoSetter
{ }
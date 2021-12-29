namespace Narumikazuchi.Serialization;

/// <summary>
/// Represents the functiontality of an <see cref="ISerializationInfo"/> object to mutate the state of certain members.
/// </summary>
public interface ISerializationInfoSetter :
    ISerializationInfo
{
    /// <summary>
    /// Sets the specified member from the state information.
    /// </summary>
    /// <param name="memberName">The name of the member in the object.</param>
    /// <param name="memberValue">The current value of the member.</param>
    public void Set<TMember>([DisallowNull] String memberName,
                             [AllowNull] TMember? memberValue);
}
namespace Narumikazuchi.Serialization;

/// <summary>
/// Represents the functionality of an <see cref="ISerializationInfo"/> object to retrieve it's member state.
/// </summary>
public interface ISerializationInfoGetter :
    ISerializationInfo
{
    /// <summary>
    /// Gets the specified member from the state information.
    /// </summary>
    /// <param name="memberName">The name of the member in the object.</param>
    /// <returns>The value of the specified member</returns>
    /// <exception cref="KeyNotFoundException"/>
    [return: MaybeNull]
    public TMember? Get<TMember>([DisallowNull] String memberName);
}
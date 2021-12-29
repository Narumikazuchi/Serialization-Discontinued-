namespace Narumikazuchi.Serialization;

/// <summary>
/// Represents the functionality of an <see cref="ISerializationInfo"/> object to add member info to it's state.
/// </summary>
public interface ISerializationInfoAdder :
    ISerializationInfo
{
    /// <summary>
    /// Adds the specified member to the current state information.
    /// </summary>
    /// <param name="memberName">The name of the member in the object.</param>
    /// <param name="memberValue">The current value of the member.</param>
    /// <exception cref="ArgumentException"/>
    public ISerializationInfoAdder Add<TMember>([DisallowNull] String memberName,
                                                  [AllowNull] TMember? memberValue);

    /// <summary>
    /// Finalizes the configuration process and returns the current state information.
    /// </summary>
    /// <returns>The current state information after every member has been added</returns>
    public ISerializationInfo Finalize();
}
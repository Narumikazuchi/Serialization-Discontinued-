namespace Narumikazuchi.Serialization;

/// <summary>
/// Contains the current state information of an object that should be serialized.
/// </summary>
public interface ISerializationInfo :
    IEquatable<ISerializationInfo>,
    IReadOnlyCollection<MemberState>
{
    /// <summary>
    /// Gets if this state represents <see langword="null"/>.
    /// </summary>
    public Boolean IsNull { get; }
    /// <summary>
    /// Gets the members that are stored in this information object.
    /// </summary>
    [NotNull]
    public IEnumerable<String> Members { get; }
    /// <summary>
    /// Gets the type of the serialization target.
    /// </summary>
    [NotNull]
    public Type Type { get; }
}

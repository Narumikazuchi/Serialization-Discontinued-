namespace Narumikazuchi.Serialization;

/// <summary>
/// Represents a <see cref="SerializationInfo"/> whose contents have been sealed.
/// </summary>
public sealed partial class ReadOnlySerializationInfo
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public List<MemberState>.Enumerator GetEnumerator() =>
        this.Members.GetEnumerator();

    /// <summary>
    /// Gets the specified member from the state information.
    /// </summary>
    /// <param name="memberName">The name of the member in the object.</param>
    /// <returns>The value of the specified member</returns>
    /// <exception cref="KeyNotFoundException"/>
    public TMember? GetState<TMember>([DisallowNull] String memberName)
    {
        ArgumentNullException.ThrowIfNull(memberName);

        foreach (MemberState member in this.Members)
        {
            if (member.Name == memberName)
            {
                return member.As<TMember>();
            }
        }

        throw new KeyNotFoundException();
    }

    /// <summary>
    /// Gets the members that are stored in this information object.
    /// </summary>
    [NotNull]
    public ReadOnlyList<String> MemberNames { get; }
}

partial class ReadOnlySerializationInfo : SerializationInfo
{
    internal ReadOnlySerializationInfo(WriteableSerializationInfo writeable) :
        base(type: writeable.Type,
             isNull: writeable.IsNull)
    {
        this.Members.AddRange(writeable.Members);
        this.MemberNames = ReadOnlyList<String>.CreateFrom(writeable.Members.Select(x => x.Name));
    }

    /// <summary>
    /// Gets if this state has been sealed.
    /// </summary>
    public override Boolean IsSealed { get; } = true;
}

// IEnumerable
partial class ReadOnlySerializationInfo : IEnumerable
{
    IEnumerator IEnumerable.GetEnumerator() =>
        this.GetEnumerator();
}

// IEnumerable<T>
partial class ReadOnlySerializationInfo : IEnumerable<MemberState>
{
    IEnumerator<MemberState> IEnumerable<MemberState>.GetEnumerator() =>
        this.GetEnumerator();
}

// IReadOnlyCollection<T>
partial class ReadOnlySerializationInfo : IReadOnlyCollection<MemberState>
{
    /// <inheritdoc/>
    public Int32 Count =>
        this.Members.Count;
}
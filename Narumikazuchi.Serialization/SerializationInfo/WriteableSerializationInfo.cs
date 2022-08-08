namespace Narumikazuchi.Serialization;

/// <summary>
/// Represents a <see cref="SerializationInfo"/> object whose contents can be written to.
/// </summary>
public sealed partial class WriteableSerializationInfo
{
    /// <summary>
    /// Adds the specified member to the current state information.
    /// </summary>
    /// <param name="memberName">The name of the member in the object.</param>
    /// <param name="memberValue">The current value of the member.</param>
    /// <returns>Itself to chain multiple calls together.</returns>
    /// <exception cref="ArgumentException"/>
    public SerializationInfo AddState<TMember>([DisallowNull] String memberName,
                                               [AllowNull] TMember? memberValue)
    {
        ArgumentNullException.ThrowIfNull(memberName);

        if (this.Members.Any(m => m.Name == memberName))
        {
            ArgumentException exception = new(message: "Duplicate member names are not allowed.");
            exception.Data.Add(key: "Name",
                               value: memberName);
            throw exception;
        }

        this.Members.Add(MemberState.CreateFromObject(name: memberName,
                                                      @object: memberValue));

        return this;
    }

    /// <summary>
    /// Seals the current state information effectively finalizing it's contents.
    /// </summary>
    public ReadOnlySerializationInfo Seal() =>
        new(this);

    /// <summary>
    /// Sets the specified member from the state information.
    /// </summary>
    /// <param name="memberName">The name of the member in the object.</param>
    /// <param name="memberValue">The current value of the member.</param>
    /// <returns>Itself to chain mutiple calls together.</returns>
    public SerializationInfo SetState<TMember>([DisallowNull] String memberName,
                                               [AllowNull] TMember? memberValue)
    {
        ArgumentNullException.ThrowIfNull(memberName);

        MemberState? state = this.Members.FirstOrDefault(m => m.Name == memberName);
        if (state is not null)
        {
            state.m_Value = memberValue;
            return this;
        }
        else
        {
            this.Members.Add(MemberState.CreateFromObject(name: memberName,
                                                          @object: memberValue));
            return this;
        }
    }

    /// <inheritdoc/>
    public override Boolean IsSealed { get; } = false;
}

partial class WriteableSerializationInfo : SerializationInfo
{
    internal WriteableSerializationInfo(Type type,
                                        Boolean isNull) :
        base(type: type,
             isNull: isNull)
    { }
}
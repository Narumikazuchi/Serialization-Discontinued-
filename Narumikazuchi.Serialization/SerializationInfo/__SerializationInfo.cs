namespace Narumikazuchi.Serialization;

// Non-Public
internal sealed partial class __SerializationInfo : ISerializationInfoMutator
{
    internal __SerializationInfo(Type type,
                                 Boolean isNull)
    {
        ExceptionHelpers.ThrowIfArgumentNull(type);

        this.Type = type;
        this.IsNull = isNull;
    }

    internal IList<MemberState> InternalMembers { get; } = new List<MemberState>();
}

// IEnumerable
partial class __SerializationInfo : IEnumerable
{
    IEnumerator IEnumerable.GetEnumerator() => 
        this.GetEnumerator();
}

// IEnumerable<T>
partial class __SerializationInfo : IEnumerable<MemberState>
{
    public IEnumerator<MemberState> GetEnumerator()
    {
        Int32 count = this.Count;
        for (Int32 i = 0; 
             i < this.InternalMembers
                     .Count; 
             i++)
        {
            if (count != this.Count)
            {
                throw new InvalidOperationException("Collection changed during enumeration.");
            }
            yield return this.InternalMembers[i];
        }
    }
}

// IEquatable<T>
partial class __SerializationInfo : IEquatable<ISerializationInfo>
{
    public Boolean Equals(ISerializationInfo? other)
    {
        if (other is null)
        {
            return false;
        }
        return this.Type == other.Type;
    }
}

// IReadOnlyCollection<T>
partial class __SerializationInfo : IReadOnlyCollection<MemberState>
{
    public Int32 Count => 
        this.InternalMembers
            .Count;
}

// ISerializationInfo
partial class __SerializationInfo : ISerializationInfo
{
    public Boolean IsNull { get; }

    [NotNull]
    public IEnumerable<String> Members => 
        this.InternalMembers
            .Select(d => d.Name);

    [NotNull]
    public Type Type { get; }
}

// ISerializationInfoAddable
partial class __SerializationInfo : ISerializationInfoAdder
{
    public ISerializationInfoAdder Add<TMember>([DisallowNull] String memberName,
                                                  [AllowNull] TMember? memberValue)
    {
        ExceptionHelpers.ThrowIfNullOrEmpty(memberName);

        if (this.InternalMembers
                .Any(m => m.Name == memberName))
        {
            ArgumentException exception = new(message: "Duplicate member names are not allowed.");
            exception.Data
                     .Add(key: "Name",
                          value: memberName);
            throw exception;
        }

        this.InternalMembers
            .Add(new(name: memberName,
                     value: memberValue,
                     type: typeof(TMember)));

        return this;
    }

    public ISerializationInfo Finalize() => 
        this;
}

// ISerializationInfoGettable
partial class __SerializationInfo : ISerializationInfoGetter
{
    public TMember? Get<TMember>([DisallowNull] String memberName)
    {
        ExceptionHelpers.ThrowIfNullOrEmpty(memberName);

        for (Int32 i = 0; i < this.Count; i++)
        {
            if (this.InternalMembers[i]
                    .Name == memberName)
            {
                return this.InternalMembers[i]
                           .As<TMember>();
            }
        }
        throw new KeyNotFoundException();
    }
}

// ISerializationInfoSettable
partial class __SerializationInfo : ISerializationInfoSetter
{
    public void Set<TMember>([DisallowNull] String memberName,
                             [AllowNull] TMember? memberValue)
    {
        ExceptionHelpers.ThrowIfNullOrEmpty(memberName);

        MemberState? state = this.InternalMembers
                                 .FirstOrDefault(m => m.Name == memberName);
        if (state is not null)
        {
            state._value = memberValue;
            return;
        }

        this.InternalMembers
            .Add(new(name: memberName,
                     value: memberValue,
                     type: typeof(TMember)));
    }
}
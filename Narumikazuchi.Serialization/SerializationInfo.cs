namespace Narumikazuchi.Serialization;

/// <summary>
/// Contains the current state information of an object that should be serialized.
/// </summary>
public sealed partial class SerializationInfo
{
    /// <summary>
    /// Creates a new instance of the <see cref="SerializationInfo"/> class from the information provided by the specified object.
    /// </summary>
    /// <param name="from">The object to serialize.</param>
    /// <param name="write">The <see langword="delegate"/> which provides the state for the object.</param>
    /// <returns>The current state information of the specified object.</returns>
    /// <remarks>
    /// This method is designed for types that are neither marked with the <see cref="CustomSerializableAttribute"/> nor implement the <see cref="ISerializable{TSelf}"/> interface.
    /// </remarks>
    [return: NotNull]
    public static SerializationInfo Create<TAny>([DisallowNull] TAny from,
                                                 [DisallowNull] Action<TAny, SerializationInfo> write)
    {
        ExceptionHelpers.ThrowIfArgumentNull(from);
        ExceptionHelpers.ThrowIfArgumentNull(write);

        SerializationInfo result = new(typeof(TAny),
                                       false);
        write.Invoke(from,
                        result);
        if (!_knownTypes.ContainsKey(typeof(TAny)))
        {
            _knownTypes.Add(typeof(TAny),
                            new __TypeCache(result));
        }
        return result;
    }
    /// <summary>
    /// Creates a new instance of the <see cref="SerializationInfo"/> class from the information provided by the specified object.
    /// </summary>
    /// <param name="from">The object to serialize.</param>
    /// <returns>The current state information of the specified object.</returns>
    /// <remarks>
    /// This method is designed for types that implement the <see cref="ISerializable"/> interface.
    /// </remarks>
    [return: NotNull]
    public static SerializationInfo Create([DisallowNull] ISerializable from)
    {
        ExceptionHelpers.ThrowIfArgumentNull(from);

        SerializationInfo result = new(from.GetType(),
                                       false);
        from.GetSerializationData(result);
        if (!_knownTypes.ContainsKey(from.GetType()))
        {
            _knownTypes.Add(from.GetType(),
                            new __TypeCache(result));
        }
        return result;
    }
    /// <summary>
    /// Creates a new instance of the <see cref="SerializationInfo"/> class for the specified type.
    /// </summary>
    /// <param name="type">The type of the object being deserialized.</param>
    /// <returns>An empty state object to be filled by the serializer.</returns>
    [return: NotNull]
    public static SerializationInfo Create([DisallowNull] Type type)
    {
        ExceptionHelpers.ThrowIfArgumentNull(type);
        SerializationInfo result = new(type, 
                                       false);

        if (!_knownTypes.ContainsKey(type))
        {
            return result;
        }

        foreach (MemberInfo member in _knownTypes[type])
        {
            if (member is PropertyInfo property)
            {
                result._members.Add(new(property.Name,
                                        property.PropertyType));
                continue;
            }
            if (member is FieldInfo field)
            {
                result._members.Add(new(field.Name,
                                        field.FieldType));
                continue;
            }
        }

        return result;
    }
    /// <summary>
    /// Creates a new instance of the <see cref="SerializationInfo"/> class for an object withou a reference.
    /// </summary>
    /// <returns>An empty state object representing the special state of <see langword="null"/>.</returns>
    [return: NotNull]
    public static SerializationInfo CreateNull() => new(typeof(Object),
                                                        true);

    /// <summary>
    /// Adds the specified member to the current state information.
    /// </summary>
    /// <param name="memberName">The name of the member in the object.</param>
    /// <param name="memberValue">The current value of the member.</param>
    /// <exception cref="ArgumentException"/>
    public void Add<TMember>([DisallowNull] String memberName,
                             [MaybeNull] TMember? memberValue)
    {
        ExceptionHelpers.ThrowIfNullOrEmpty(memberName);

        if (this.Members.Contains(memberName))
        {
            // Duplicate member not possible
            throw new ArgumentException($"Duplicate member names are not allowed. (Member name: '{memberName}')");
        }

        this._members.Add(new(memberName,
                                memberValue,
                                typeof(TMember)));
    }

    /// <summary>
    /// Sets the specified member from the state information.
    /// </summary>
    /// <param name="memberName">The name of the member in the object.</param>
    /// <param name="memberValue">The current value of the member.</param>
    public void Set<TMember>([DisallowNull] String memberName,
                             [MaybeNull] TMember? memberValue)
    {
        ExceptionHelpers.ThrowIfNullOrEmpty(memberName);

        MemberState? state = this._members.FirstOrDefault(m => m.Name == memberName);
        if (state is not null)
        {
            state._value = memberValue;
            return;
        }

        this._members.Add(new(memberName,
                              memberValue,
                              typeof(TMember)));
    }

    /// <summary>
    /// Gets the specified member from the state information.
    /// </summary>
    /// <param name="memberName">The name of the member in the object.</param>
    /// <returns>The value of the specified member</returns>
    /// <exception cref="KeyNotFoundException"/>
    public TMember? Get<TMember>([DisallowNull] String memberName)
    {
        ExceptionHelpers.ThrowIfNullOrEmpty(memberName);

        for (Int32 i = 0; i < this.Count; i++)
        {
            if (this._members[i].Name == memberName)
            {
                return this._members[i].As<TMember>();
            }
        }
        throw new KeyNotFoundException();
    }

    /// <summary>
    /// Gets the currently cached types.
    /// </summary>
    [NotNull]
    public static IReadOnlyDictionary<Type, IEnumerable<MemberInfo>> MemberCache => _knownTypes;

    /// <summary>
    /// Gets if this state represents <see langword="null"/>.
    /// </summary>
    public Boolean IsNull { get; }
    /// <summary>
    /// Gets the members that are stored in this information object.
    /// </summary>
    [NotNull]
    public IEnumerable<String> Members => this._members.Select(d => d.Name);
    /// <summary>
    /// Gets the type of the serialization target.
    /// </summary>
    [NotNull]
    public Type Type { get; }
}

// Non-Public
partial class SerializationInfo
{
    private SerializationInfo(Type type,
                              Boolean isNull)
    {
        ExceptionHelpers.ThrowIfArgumentNull(type);

        this.Type = type;
        this.IsNull = isNull;
    }

    private static readonly Dictionary<Type, IEnumerable<MemberInfo>> _knownTypes = new();

    private readonly List<MemberState> _members = new();
}

// IReadOnlyCollection
partial class SerializationInfo : IReadOnlyCollection<MemberState>
{
    /// <inheritdoc/>
    public IEnumerator<MemberState> GetEnumerator()
    {
        Int32 count = this.Count;
        for (Int32 i = 0; i < this._members.Count; i++)
        {
            if (count != this.Count)
            {
                throw new InvalidOperationException("Collection changed during enumeration.");
            }
            yield return this._members[i];
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

    /// <inheritdoc/>
    public Int32 Count => this._members.Count;
}
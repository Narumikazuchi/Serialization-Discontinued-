namespace Narumikazuchi.Serialization;

/// <summary>
/// Contains the current state information of an object that should be serialized.
/// </summary>
public abstract partial class SerializationInfo
{
    /// <inheritdoc/>
    public override Boolean Equals(Object? obj) =>
        obj is SerializationInfo other &&
        this.Equals(other);

    /// <inheritdoc/>
    public override Int32 GetHashCode() =>
        this.Type.GetHashCode();

    /// <summary>
    /// Creates a new <see cref="WriteableSerializationInfo"/> object for the specified type.
    /// </summary>
    /// <remarks>
    /// This method is primarily reserved for the deserialization process and deserializers.
    /// </remarks>
    /// <param name="type">The type of the object being deserialized.</param>
    /// <param name="isNull">Whether the object of the specified type is set to <see langword="null"/>.</param>
    /// <returns>An empty state object to be filled by the deserializer</returns>
    public static WriteableSerializationInfo CreateFromType([DisallowNull] Type type,
                                                            Boolean isNull)
    {
        ArgumentNullException.ThrowIfNull(type);

        return new WriteableSerializationInfo(type: type,
                                              isNull: isNull);
    }

    /// <summary>
    /// Creates a new <see cref="SerializationInfo"/> object from the information provided by the specified object.
    /// </summary>
    /// <param name="from">The object to serialize.</param>
    /// <returns>The current state information of the specified object.</returns>
    /// <remarks>
    /// This method is designed for types that implement the <see cref="ISerializable"/> interface.
    /// </remarks>
    /// <returns>A filled state object representing the specified <typeparamref name="TSerializable"/></returns>
    [return: NotNull]
    public static SerializationInfo CreateFromSerializable<TSerializable>([AllowNull] TSerializable? from)
        where TSerializable : ISerializable
    {
        if (from is null)
        {
            return new WriteableSerializationInfo(type: typeof(TSerializable),
                                                  isNull: true);
        }
        else
        {
            Type type = from.GetType();
            WriteableSerializationInfo write = new(type: type,
                                                   isNull: false);
            from.GetSerializationData(write);
            return write.Seal();
        }
    }
    /// <summary>
    /// Creates a new <see cref="SerializationInfo"/> object from the information provided by the specified object.
    /// </summary>
    /// <param name="from">The object to serialize.</param>
    /// <param name="writer">The <see langword="delegate"/> which provides the state for the object.</param>
    /// <returns>The current state information of the specified object.</returns>
    /// <remarks>
    /// This method is designed for types that don't or can't implement the <see cref="IDeserializable{TSelf}"/> interface.
    /// </remarks>
    /// <returns>A filled state object representing the specified <typeparamref name="TAny"/></returns>
    [return: NotNull]
    public static SerializationInfo CreateFromAny<TAny>([AllowNull] TAny? from,
                                                        [DisallowNull] Action<TAny, WriteableSerializationInfo> writer)
    {
        if (from is null)
        {
            return new WriteableSerializationInfo(type: typeof(TAny),
                                                  isNull: true);
        }
        else
        {
            Type type = from.GetType();
            WriteableSerializationInfo write = new(type: type,
                                                   isNull: false);
            writer.Invoke(from, write);
            return write.Seal();
        }
    }

    /// <summary>
    /// Gets if this state represents <see langword="null"/>.
    /// </summary>
    public Boolean IsNull { get; }

    /// <summary>
    /// Gets if this state has been sealed.
    /// </summary>
    public abstract Boolean IsSealed { get; }

    /// <summary>
    /// Gets the type of the serialization target.
    /// </summary>
    [NotNull]
    public Type Type { get; }
}

// Non-Public
partial class SerializationInfo
{
    internal SerializationInfo(Type type,
                               Boolean isNull)
    {
        ArgumentNullException.ThrowIfNull(type);

        this.Type = type;
        this.IsNull = isNull;
    }

    /// <summary>
    /// Contains the members of this info.
    /// </summary>
    internal protected List<MemberState> Members { get; } = new();
}

// IEquatable<T>
partial class SerializationInfo : IEquatable<SerializationInfo>
{
    /// <inheritdoc/>
    public Boolean Equals(SerializationInfo? other)
    {
        if (other is null)
        {
            return false;
        }
        return this.Type == other.Type;
    }
}
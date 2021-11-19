namespace Narumikazuchi.Serialization;

/// <summary>
/// Provides the bare minimum for a serializer class.
/// </summary>
/// <remarks>
/// The typeparameter <typeparamref name="T"/> corresponds to the result of any <see cref="ISerializationStrategy{TReturn}"/> used by the implementing serializer class.
/// </remarks>
public abstract partial class SerializerBase<T>
{
    /// <summary>
    /// Applies the specified strategy for the specified type, when serializing that type.
    /// </summary>
    /// <param name="forType">The type the strategy aims to serialize and deserialize.</param>
    /// <param name="strategy">The strategy to use during serialization and deserialization.</param>
    /// <exception cref="ArgumentNullException" />
    public void ApplyStrategy([DisallowNull] Type forType,
                              [DisallowNull] ISerializationStrategy<T> strategy)
    {
        ExceptionHelpers.ThrowIfArgumentNull(forType);
        ExceptionHelpers.ThrowIfArgumentNull(strategy);

        if (this._strategies.ContainsKey(forType))
        {
            this._strategies[forType] = strategy;
            return;
        }
        this._strategies.Add(forType,
                             strategy);
    }
}

// Non-Public
partial class SerializerBase<T>
{
    /// <summary>
    /// Instantiates a new instance of the <see cref="SerializerBase{T}"/> class.
    /// </summary>
    protected SerializerBase()
    { }
    /// <summary>
    /// Instantiates a new instance of the <see cref="SerializerBase{T}"/> class with the specified strategies.
    /// </summary>
    protected SerializerBase([DisallowNull] IReadOnlyDictionary<Type, ISerializationStrategy<T>> strategies)
    {
        ExceptionHelpers.ThrowIfArgumentNull(strategies);
        this._strategies = new(strategies);
    }

    /// <summary>
    /// Tries to create an object from the specified serialization data.
    /// This method is designed for classes that are marked with the <see cref="CustomSerializableAttribute"/>.
    /// </summary>
    /// <param name="info">The serialization data for the object to create.</param>
    /// <returns>The object specified in the serialization data</returns>
    /// <exception cref="MissingMemberException"/>
    [return: NotNull]
    protected static Object CreateObject([DisallowNull] SerializationInfo info)
    {
        ExceptionHelpers.ThrowIfArgumentNull(info);
#nullable disable
        ConstructorInfo ctor = info.Type.GetConstructor(Type.EmptyTypes);
        Object result = ctor.Invoke(Array.Empty<Object>());
        foreach (String member in info.Members)
        {
            PropertyInfo property = info.Type.GetProperty(member,
                                                          BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (property is not null)
            {
                property.SetValue(result,
                                  info.Get<Object>(member));
                continue;
            }
            FieldInfo field = info.Type.GetField(member,
                                                 BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (field is not null)
            {
                field.SetValue(result,
                               info.Get<Object>(member));
                continue;
            }
            // Member not found?
            throw new MissingMemberException();
        }

        return result;
#nullable enable
    }

    /// <summary>
    /// Contains the serialization strategies for the various types to serialize.
    /// </summary>
    protected readonly Dictionary<Type, ISerializationStrategy<T>> _strategies = new();

#pragma warning disable
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    protected const String STREAM_DOES_NOT_SUPPORT_READING = "The specified stream does not support reading.";
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    protected const String STREAM_DOES_NOT_SUPPORT_WRITING = "The specified stream does not support writing.";
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    protected const String STREAM_DOES_NOT_SUPPORT_SEEKING = "The specified stream does not support seeking.";
#pragma warning restore
}

// ISerializer
partial class SerializerBase<T> : ISerializer
{
    /// <inheritdoc/>
    [NotNull]
    public IReadOnlyCollection<Type> RegisteredStrategies => this._strategies.Keys;
}
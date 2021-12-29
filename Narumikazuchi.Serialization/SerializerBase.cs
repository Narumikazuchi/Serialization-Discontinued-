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
        this._strategies.Add(key: forType,
                             value: strategy);
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
    protected SerializerBase([DisallowNull] IEnumerable<KeyValuePair<Type, ISerializationStrategy<T>>> strategies)
    {
        ExceptionHelpers.ThrowIfArgumentNull(strategies);
        this._strategies = new Dictionary<Type, ISerializationStrategy<T>>(collection: strategies);
    }
    /// <summary>
    /// Instantiates a new instance of the <see cref="SerializerBase{T}"/> class with the specified strategies.
    /// </summary>
    protected SerializerBase([DisallowNull] IEnumerable<(Type, ISerializationStrategy<T>)> strategies)
    {
        ExceptionHelpers.ThrowIfArgumentNull(strategies);
        this._strategies = new Dictionary<Type, ISerializationStrategy<T>>();
        foreach ((Type, ISerializationStrategy<T>) tuple in strategies)
        {
            this._strategies
                .Add(key: tuple.Item1,
                     value: tuple.Item2);
        }
    }
    /// <summary>
    /// Instantiates a new instance of the <see cref="SerializerBase{T}"/> class with the specified strategies.
    /// </summary>
    protected SerializerBase([DisallowNull] IEnumerable<Tuple<Type, ISerializationStrategy<T>>> strategies)
    {
        ExceptionHelpers.ThrowIfArgumentNull(strategies);
        this._strategies = new Dictionary<Type, ISerializationStrategy<T>>();
        foreach (Tuple<Type, ISerializationStrategy<T>> tuple in strategies)
        {
            this._strategies
                .Add(key: tuple.Item1,
                     value: tuple.Item2);
        }
    }

    /// <summary>
    /// Tries to create an object from the specified serialization data.
    /// </summary>
    /// <param name="info">The serialization data for the object to create.</param>
    /// <returns>The object specified in the serialization data</returns>
    /// <exception cref="MissingMemberException"/>
    [return: NotNull]
    protected static Object CreateObject([DisallowNull] ISerializationInfoGetter info)
    {
        ExceptionHelpers.ThrowIfArgumentNull(info);
        ConstructorInfo? ctor = info.Type
                                    .GetConstructor(types: Type.EmptyTypes);
        if (ctor is null)
        {
            throw new MissingMemberException(className: info.Type
                                                            .FullName,
                                             memberName: "ctor");
        }

        Object result = ctor.Invoke(Array.Empty<Object>());
        foreach (String member in info.Members)
        {
            PropertyInfo? property = info.Type
                                         .GetProperty(name: member,
                                                      bindingAttr: BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (property is not null)
            {
                property.SetValue(obj: result,
                                  value: info.Get<Object>(member));
                continue;
            }
            FieldInfo? field = info.Type
                                   .GetField(name: member,
                                             bindingAttr: BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (field is not null)
            {
                field.SetValue(obj: result,
                               value: info.Get<Object>(member));
                continue;
            }

            throw new MissingMemberException(className: info.Type.FullName,
                                             memberName: member);
        }

        return result;
    }

    /// <summary>
    /// Contains the serialization strategies for the various types to serialize.
    /// </summary>
    protected readonly IDictionary<Type, ISerializationStrategy<T>> _strategies = new Dictionary<Type, ISerializationStrategy<T>>();

#pragma warning disable IDE1006
#pragma warning disable CS1591
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
    public IReadOnlyCollection<Type> RegisteredStrategies => 
        (IReadOnlyCollection<Type>)this._strategies
                                       .Keys;
}
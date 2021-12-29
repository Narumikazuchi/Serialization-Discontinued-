﻿namespace Narumikazuchi.Serialization;

/// <summary>
/// Build an <see cref="ISerializationInfo"/> object to store the state information of an object for serialization.
/// </summary>
public static partial class SerializationInfoBuilder
{
    /// <summary>
    /// Creates a new <see cref="ISerializationInfo"/> object for the specified type.
    /// </summary>
    /// <remarks>
    /// This method is primarily reserved for the deserialization process and deserializers.
    /// </remarks>
    /// <param name="type">The type of the object being deserialized.</param>
    /// <param name="isNull">Whether the object of the specified type is set to <see langword="null"/>.</param>
    /// <returns>An empty state object to be filled by the deserializer</returns>
    [return: NotNull]
    public static ISerializationInfoMutator CreateFrom([DisallowNull] Type type,
                                                       Boolean isNull)
    {
        ExceptionHelpers.ThrowIfArgumentNull(type);

        __SerializationInfo result = new(type: type,
                                         isNull: isNull);
        if (!_knownTypes.ContainsKey(type))
        {
            return result;
        }

        foreach (MemberInfo member in _knownTypes[type])
        {
            if (member is PropertyInfo property)
            {
                result.InternalMembers
                      .Add(new(name: property.Name,
                               type: property.PropertyType));
                continue;
            }
            if (member is FieldInfo field)
            {
                result.InternalMembers
                      .Add(new(name: field.Name,
                               type: field.FieldType));
                continue;
            }
        }

        return result;
    }
    /// <summary>
    /// Creates a new <see cref="ISerializationInfo"/> object from the information provided by the specified object.
    /// </summary>
    /// <param name="from">The object to serialize.</param>
    /// <returns>The current state information of the specified object.</returns>
    /// <remarks>
    /// This method is designed for types that implement the <see cref="ISerializable"/> interface.
    /// </remarks>
    /// <returns>A filled state object representing the specified <typeparamref name="TSerializable"/></returns>
    [return: NotNull]
    public static ISerializationInfoAdder CreateFrom<TSerializable>([AllowNull] TSerializable? from)
        where TSerializable : ISerializable
    {
        if (from is null)
        {
            return new __SerializationInfo(type: typeof(TSerializable),
                                           isNull: from is null);
        }

        Type type = from.GetType();
        __SerializationInfo result = new(type: type,
                                         isNull: from is null);

        from!.GetSerializationData(result);
        if (!_knownTypes.ContainsKey(type))
        {
            _knownTypes.Add(key: type,
                            value: new __TypeCache(result));
        }
        return result;
    }
    /// <summary>
    /// Creates a new <see cref="ISerializationInfo"/> object from the information provided by the specified object.
    /// </summary>
    /// <param name="from">The object to serialize.</param>
    /// <param name="write">The <see langword="delegate"/> which provides the state for the object.</param>
    /// <returns>The current state information of the specified object.</returns>
    /// <remarks>
    /// This method is designed for types that don't or can't implement the <see cref="ISerializable{TSelf}"/> interface.
    /// </remarks>
    /// <returns>A filled state object representing the specified <typeparamref name="TAny"/></returns>
    [return: NotNull]
    public static ISerializationInfoAdder CreateFrom<TAny>([AllowNull] TAny? from,
                                                           [DisallowNull] Action<TAny, ISerializationInfoAdder> write)
    {
        ExceptionHelpers.ThrowIfArgumentNull(write);

        if (from is null)
        {
            return new __SerializationInfo(type: typeof(TAny),
                                           isNull: true);
        }

        Type type = from.GetType();
        __SerializationInfo result = new(type: type,
                                         isNull: false);

        write.Invoke(arg1: from,
                     arg2: result);
        if (!_knownTypes.ContainsKey(type))
        {
            _knownTypes.Add(key: type,
                            value: new __TypeCache(result));
        }
        return result;
    }
}

partial class SerializationInfoBuilder
{
    private static readonly IDictionary<Type, IEnumerable<MemberInfo>> _knownTypes = new Dictionary<Type, IEnumerable<MemberInfo>>();
}
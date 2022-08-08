namespace Narumikazuchi.Serialization;

/// <summary>
/// Contains the information on a specific member of an object.
/// </summary>
public sealed partial class MemberState
{
    /// <summary>
    /// Creates a new <see cref="MemberState"/> from the supplied object <typeparamref name="TSource"/> and a corresponding name.
    /// </summary>
    /// <param name="name">The name of the member.</param>
    /// <param name="object">The current state of the member.</param>
    /// <returns>A new <see cref="MemberState"/> object.</returns>
    public static MemberState CreateFromObject<TSource>(String name,
                                                        TSource? @object)
    {
        ArgumentNullException.ThrowIfNull(name);

        if (@object is null)
        {
            return new(name: name,
                       value: @object,
                       type: typeof(TSource));
        }
        else
        {
            return new(name: name,
                       value: @object,
                       type: @object.GetType());
        }
    }
    /// <summary>
    /// Creates a new <see cref="MemberState"/> from the supplied object <typeparamref name="TSource"/> and a corresponding name.
    /// </summary>
    /// <param name="name">The name of the member.</param>
    /// <param name="object">The current state of the member.</param>
    /// <param name="type">The type of the member.</param>
    /// <returns>A new <see cref="MemberState"/> object.</returns>
    public static MemberState CreateFromObject<TSource>(String name,
                                                        TSource? @object,
                                                        Type type)
    {
        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(type);

        return new(name: name,
                   value: @object,
                   type: type);
    }

    /// <summary>
    /// Returns the value of this member as the specified type <typeparamref name="TReturn"/>.
    /// </summary>
    /// <returns>The value of this member as the specified type <typeparamref name="TReturn"/></returns>
    public TReturn? As<TReturn>() =>
        (TReturn?)this.Value;

    /// <summary>
    /// Gets the name of this member in the object.
    /// </summary>
    public String Name { get; }

    /// <summary>
    /// Gets the value of this member in the object.
    /// </summary>
    public Object? Value =>
        m_Value;

    /// <summary>
    /// Gets the member type.
    /// </summary>
    public Type MemberType { get; }
}

// Non-Public
partial class MemberState
{
    private MemberState(String name,
                        Object? value,
                        Type type)
    {
        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(type);

        this.Name = name;
        m_Value = value;
        this.MemberType = type;
    }

    internal Object? m_Value = null;
}
namespace Narumikazuchi.Serialization;

/// <summary>
/// Contains the information on a specific member of an object.
/// </summary>
public sealed partial class MemberState
{
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
    public Object? Value => this._value;
    /// <summary>
    /// Gets the member type.
    /// </summary>
    public Type MemberType { get; }
}

// Non-Public
partial class MemberState
{
    internal MemberState(String name,
                         Object? value,
                         Type type)
    {
        ExceptionHelpers.ThrowIfNullOrEmpty(name);
        ExceptionHelpers.ThrowIfArgumentNull(type);

        this.Name = name;
        this._value = value;
        this.MemberType = type;
    }
    internal MemberState(String name,
                         Type type)
    {
        ExceptionHelpers.ThrowIfNullOrEmpty(name);
        ExceptionHelpers.ThrowIfArgumentNull(type);

        this.Name = name;
        this.MemberType = type;
    }

    internal Object? _value = null;
}
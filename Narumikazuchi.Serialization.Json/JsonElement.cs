namespace Narumikazuchi.Serialization.Json;

/// <summary>
/// Represents an element in a JSON-Object.
/// </summary>
public abstract partial class JsonElement
{
#pragma warning disable
    public static explicit operator Boolean(JsonElement json)
    {
        if (json is not __JsonElement<Boolean> actual)
        {
            throw new InvalidCastException();
        }
        return actual.Value;
    }
    public static explicit operator Double(JsonElement json)
    {
        if (json is not __JsonElement<Double> actual)
        {
            throw new InvalidCastException();
        }
        return actual.Value;
    }
    public static explicit operator Int64(JsonElement json)
    {
        if (json is not __JsonElement<Int64> actual)
        {
            throw new InvalidCastException();
        }
        return actual.Value;
    }
    public static explicit operator String(JsonElement json)
    {
        if (json is not __JsonElement<String> actual)
        {
            throw new InvalidCastException();
        }
        return actual.Value;
    }
    public static explicit operator UInt64(JsonElement json)
    {
        if (json is not __JsonElement<UInt64> actual)
        {
            throw new InvalidCastException();
        }
        return actual.Value;
    }
#pragma warning restore

    /// <summary>
    /// Gets whether this element is an array.
    /// </summary>
    public Boolean IsArray => this is JsonArray;
    /// <summary>
    /// Gets whether this element is a boolean value.
    /// </summary>
    public Boolean IsBoolean => this is IConvertible<Boolean>;
    /// <summary>
    /// Gets whether this element is a floating point value.
    /// </summary>
    public Boolean IsDouble => this is IConvertible<Double>;
    /// <summary>
    /// Gets whether this element is a signed integer value.
    /// </summary>
    public Boolean IsInt64 => this is IConvertible<Int64>;
    /// <summary>
    /// Gets whether this element is a string value.
    /// </summary>
    public Boolean IsString => this is IConvertible<String>;
    /// <summary>
    /// Gets whether this element is an unsigned integer value.
    /// </summary>
    public Boolean IsUInt64 => this is IConvertible<UInt64>;
    /// <summary>
    /// Gets whether this element is an object.
    /// </summary>
    public Boolean IsObject => this is JsonObject;
}
namespace Narumikazuchi.Serialization;

/// <summary>
/// Represents an object that can be (de-)serialized.
/// </summary>
/// <remarks>
/// Don't use this as a standalone. This interface is intended as a base for <see cref="ISerializable"/> and <see cref="IDeserializable{TSelf}"/>.
/// </remarks>
public interface ISerializationTarget
{
    /// <summary>
    /// Registers all types that will be serialized and their associated names.
    /// </summary>
    /// <param name="register">The object in which you register all members and their associated types.</param>
    [Pure]
    public static abstract void RegisterSerializedTypes(MemberRegister register);
}
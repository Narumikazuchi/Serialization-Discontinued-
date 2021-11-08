namespace Narumikazuchi.Serialization
{
    /// <summary>
    /// Marks a type as serializable for <see cref="IDeclaredSerializer"/>.
    /// </summary>
    /// <remarks>
    /// All properties and fields that are not marked with the <see cref="NotSerializedAttribute"/> will get serialized. The only exception to this are indexers, as they are as of right now to complex a task to tackle.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false)]
    public sealed class CustomSerializableAttribute : Attribute
    { }
}

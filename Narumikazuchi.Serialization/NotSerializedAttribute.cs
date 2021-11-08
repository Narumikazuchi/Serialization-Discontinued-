namespace Narumikazuchi.Serialization
{
    /// <summary>
    /// Indicates that this member shall not be serialized.
    /// </summary>
    /// <remarks>
    /// Indexers are currently not supported and won't be serialized either way.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public sealed class NotSerializedAttribute : Attribute
    { }
}

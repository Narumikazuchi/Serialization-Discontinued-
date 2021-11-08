namespace Narumikazuchi.Serialization
{
    internal sealed class __TypeCache
    {
        public __TypeCache(Type type)
        {
            ExceptionHelpers.ThrowIfNull(type);
            this.Properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                                  .Where(p => p.GetIndexParameters().Length == 0 &&
                                              !AttributeResolver.HasAttribute<NotSerializedAttribute>(p))
                                  .ToList();
            this.Fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                              .Where(f => !AttributeResolver.HasAttribute<NotSerializedAttribute>(f))
                              .ToList();
        }

        public IReadOnlyCollection<PropertyInfo> Properties { get; }
        public IReadOnlyCollection<FieldInfo> Fields { get; }
    }
}

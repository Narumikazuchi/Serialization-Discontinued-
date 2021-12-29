namespace Narumikazuchi.Serialization;

internal sealed partial class __TypeCache : 
    IEnumerable<MemberInfo>
{
    public __TypeCache(ISerializationInfo info)
    {
        ExceptionHelpers.ThrowIfArgumentNull(info);
        ICollection<MemberInfo> items = new Collection<MemberInfo>();
        foreach (MemberState state in info)
        {
            PropertyInfo? property = info.Type
                                         .GetProperty(name: state.Name,
                                                      bindingAttr: BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (property is not null)
            {
                items.Add(property);
                continue;
            }

            FieldInfo? field = info.Type
                                   .GetField(name: state.Name,
                                             bindingAttr: BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (field is not null)
            {
                items.Add(field);
                continue;
            }
        }
        this._items = items;
    }
}

// Non-Public
partial class __TypeCache : IEnumerable<MemberInfo>
{
    private readonly IEnumerable<MemberInfo> _items;
}

// IEnumerable
partial class __TypeCache : IEnumerable
{
    IEnumerator IEnumerable.GetEnumerator() =>
        this._items
            .GetEnumerator();
}

// IEnumerable<T>
partial class __TypeCache : IEnumerable<MemberInfo>
{
    IEnumerator<MemberInfo> IEnumerable<MemberInfo>.GetEnumerator() =>
        this._items
            .GetEnumerator();
}
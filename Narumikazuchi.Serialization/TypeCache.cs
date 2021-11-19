namespace Narumikazuchi.Serialization;

internal sealed class __TypeCache : IEnumerable<MemberInfo>
{
    public __TypeCache(SerializationInfo info)
    {
        ExceptionHelpers.ThrowIfArgumentNull(info);
        List<MemberInfo> items = new();
        foreach (MemberState state in info)
        {
            PropertyInfo? property = info.Type.GetProperty(state.Name,
                                                           BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (property is not null)
            {
                items.Add(property);
                continue;
            }

            FieldInfo? field = info.Type.GetField(state.Name,
                                                  BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (field is not null)
            {
                items.Add(field);
                continue;
            }
        }
        this._items = items;
    }

    public IEnumerator<MemberInfo> GetEnumerator() => 
        this._items.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() =>
        this._items.GetEnumerator();

    private readonly IEnumerable<MemberInfo> _items;
}
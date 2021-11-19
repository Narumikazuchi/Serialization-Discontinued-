namespace Narumikazuchi.Serialization.Json;

/// <summary>
/// Represents an array in a JSON-String or JSON-Object.
/// </summary>
public sealed partial class JsonArray
{
    /// <summary>
    /// Gets the index of the first occurence of the specified element.
    /// </summary>
    /// <param name="element">The element to look for in the array.</param>
    /// <returns>The index of the element in the array or -1 if the element is not contained within the array</returns>
    public Int32 IndexOf(JsonElement? element) =>
        this._items.IndexOf(element);

    /// <inheritdoc/>
    public override String ToString()
    {
        StringBuilder builder = new();
        foreach (JsonElement? element in this._items)
        {
            if (builder.Length > 0)
            {
                builder.Append(", ");
            }
            if (element is not null)
            {
                builder.Append(element.ToString());
                continue;
            }
            builder.Append("null");
        }
        return $"[ {builder} ]";
    }

    /// <summary>
    /// Gets the element at the specified index in the array.
    /// </summary>
    /// <param name="index">The index of the element to get.</param>
    /// <returns>The element at the specified index</returns>
    /// <exception cref="IndexOutOfRangeException"/>
    [MaybeNull]
    public JsonElement? this[Int32 index]
    {
        get
        {
            if (index < 0 ||
                index >= this.Count)
            {
                throw new IndexOutOfRangeException();
            }
            return this._items[index];
        }
    }
}

partial class JsonArray : JsonElement
{
    internal void Add(JsonElement? element) => 
        this._items.Add(element);

    private readonly List<JsonElement?> _items = new();
}

partial class JsonArray : IReadOnlyList<JsonElement?>
{
    /// <inheritdoc/>
    public IEnumerator<JsonElement?> GetEnumerator()
    {
        for (Int32 i = 0; i < this.Count; i++)
        {
            yield return this._items[i];
        }
        yield break;
    }
    IEnumerator IEnumerable.GetEnumerator() => 
        this.GetEnumerator();

    /// <inheritdoc/>
    public Int32 Count => this._items.Count;
}
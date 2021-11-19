namespace Narumikazuchi.Serialization.Json;

internal sealed partial class __JsonElement<TElement> : JsonElement
{
    public __JsonElement(TElement value) =>
        this.Value = value;

    public override String? ToString()
    {
        if (typeof(TElement) == typeof(String))
        {
            return String.Format(CultureInfo.InvariantCulture,
                                 "\"{0}\"",
                                 this.Value);
        }
        return String.Format(CultureInfo.InvariantCulture,
                             "{0}",
                             this.Value);
    }

    public TElement Value { get; }
}

partial class __JsonElement<TElement> : IConvertible<TElement>
{
#pragma warning disable CS8768
    TElement IConvertible<TElement>.ToType(IFormatProvider? provider) =>
        this.Value;
}
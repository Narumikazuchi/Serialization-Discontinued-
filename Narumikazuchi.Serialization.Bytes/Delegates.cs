namespace Narumikazuchi.Serialization.Bytes
{
    public delegate Byte[] SerializationStrategy(Object toSerialize);

    public delegate Object DeserializationStrategy(Byte[] raw);
}

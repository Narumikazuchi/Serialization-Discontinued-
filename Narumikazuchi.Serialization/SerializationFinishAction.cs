namespace Narumikazuchi.Serialization;

/// <summary>
/// Represents the actions a serializer takes after finishing a serialization operation.
/// </summary>
[Flags]
public enum SerializationFinishAction
{
#pragma warning disable
    None = 0x0,
    CloseStream = 0x1,
    FlushStream = 0x2,
    DisposeStream = 0x4,
    MoveToBeginning = 0x8
#pragma warning restore
}
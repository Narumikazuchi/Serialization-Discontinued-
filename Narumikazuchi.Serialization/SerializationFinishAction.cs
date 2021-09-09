﻿namespace Narumikazuchi.Serialization
{
    /// <summary>
    /// Represents the actions a serializer takes after finishing a serialization operation.
    /// </summary>
    [System.Flags]
    public enum SerializationFinishAction
    {
#pragma warning disable
        None = 0x0,
        CloseStream = 0x1,
        FlushStream = 0x2
#pragma warning restore
    }
}
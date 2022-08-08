namespace Narumikazuchi.Serialization;

/// <summary>
/// Extends every <see cref="IDeserializer{TSerializable}"/> with less complex serialization methods.
/// </summary>
public static class DeserializerExtensions
{
    /// <summary>
    /// Deserializes the specified stream starting at the specified offset into an instance of type <typeparamref name="TSerializable"/>.
    /// </summary>
    /// <param name="deserializer">The deserializer that will perform the serialization.</param>
    /// <param name="stream">The stream to deserialize the graph from.</param>
    /// <returns>The instance represented by the bytes in the specified stream</returns>
    [Obsolete("Only use this when absolutetly necessary. Use the more modern IWriteableStream interfaced overload if possible.", false)]
    [return: MaybeNull]
    public static TSerializable? Deserialize<TSerializable>(this IDeserializer<TSerializable> deserializer,
                                                            [DisallowNull] Stream stream) =>
        deserializer.Deserialize(stream: stream,
                                 offset: 0,
                                 read: out _,
                                 actionAfter: SerializationFinishAction.CloseStream | SerializationFinishAction.DisposeStream);
    /// <summary>
    /// Deserializes the specified stream starting at the specified offset into an instance of type <typeparamref name="TSerializable"/>.
    /// </summary>
    /// <param name="deserializer">The deserializer that will perform the serialization.</param>
    /// <param name="stream">The stream to deserialize the graph from.</param>
    /// <param name="offset">The offset in the stream where to begin reading.</param>
    /// <returns>The instance represented by the bytes in the specified stream</returns>
    [Obsolete("Only use this when absolutetly necessary. Use the more modern IWriteableStream interfaced overload if possible.", false)]
    [return: MaybeNull]
    public static TSerializable? Deserialize<TSerializable>(this IDeserializer<TSerializable> deserializer,
                                                            [DisallowNull] Stream stream,
                                                            Int64 offset) =>
        deserializer.Deserialize(stream: stream,
                                 offset: offset,
                                 read: out _,
                                 actionAfter: SerializationFinishAction.CloseStream | SerializationFinishAction.DisposeStream);
    /// <summary>
    /// Deserializes the specified stream starting at the specified offset into an instance of type <typeparamref name="TSerializable"/>.
    /// </summary>
    /// <param name="deserializer">The deserializer that will perform the serialization.</param>
    /// <param name="stream">The stream to deserialize the graph from.</param>
    /// <param name="read">The amount of elements read from the <paramref name="stream"/> parameter.</param>
    /// <returns>The instance represented by the bytes in the specified stream</returns>
    [Obsolete("Only use this when absolutetly necessary. Use the more modern IWriteableStream interfaced overload if possible.", false)]
    [return: MaybeNull]
    public static TSerializable? Deserialize<TSerializable>(this IDeserializer<TSerializable> deserializer,
                                                            [DisallowNull] Stream stream,
                                                            out UInt64 read) =>
        deserializer.Deserialize(stream: stream,
                                 offset: 0,
                                 read: out read,
                                 actionAfter: SerializationFinishAction.CloseStream | SerializationFinishAction.DisposeStream);
    /// <summary>
    /// Deserializes the specified stream starting at the specified offset into an instance of type <typeparamref name="TSerializable"/>.
    /// </summary>
    /// <param name="deserializer">The deserializer that will perform the serialization.</param>
    /// <param name="stream">The stream to deserialize the graph from.</param>
    /// <param name="offset">The offset in the stream where to begin reading.</param>
    /// <param name="read">The amount of elements read from the <paramref name="stream"/> parameter.</param>
    /// <returns>The instance represented by the bytes in the specified stream</returns>
    [Obsolete("Only use this when absolutetly necessary. Use the more modern IWriteableStream interfaced overload if possible.", false)]
    [return: MaybeNull]
    public static TSerializable? Deserialize<TSerializable>(this IDeserializer<TSerializable> deserializer,
                                                            [DisallowNull] Stream stream,
                                                            Int64 offset,
                                                            out UInt64 read) =>
        deserializer.Deserialize(stream: stream,
                                 offset: offset,
                                 read: out read,
                                 actionAfter: SerializationFinishAction.CloseStream | SerializationFinishAction.DisposeStream);
    /// <summary>
    /// Deserializes the specified stream starting at the specified offset into an instance of type <typeparamref name="TSerializable"/>.
    /// </summary>
    /// <param name="deserializer">The deserializer that will perform the serialization.</param>
    /// <param name="stream">The stream to deserialize the graph from.</param>
    /// <param name="actionAfter">The actions to perform after the reading operation has finished.</param>
    /// <returns>The instance represented by the bytes in the specified stream</returns>
    [Obsolete("Only use this when absolutetly necessary. Use the more modern IWriteableStream interfaced overload if possible.", false)]
    [return: MaybeNull]
    public static TSerializable? Deserialize<TSerializable>(this IDeserializer<TSerializable> deserializer,
                                                            [DisallowNull] Stream stream,
                                                            SerializationFinishAction actionAfter) =>
        deserializer.Deserialize(stream: stream,
                                 offset: 0,
                                 read: out _,
                                 actionAfter: actionAfter);
    /// <summary>
    /// Deserializes the specified stream starting at the specified offset into an instance of type <typeparamref name="TSerializable"/>.
    /// </summary>
    /// <param name="deserializer">The deserializer that will perform the serialization.</param>
    /// <param name="stream">The stream to deserialize the graph from.</param>
    /// <param name="offset">The offset in the stream where to begin reading.</param>
    /// <param name="actionAfter">The actions to perform after the reading operation has finished.</param>
    /// <returns>The instance represented by the bytes in the specified stream</returns>
    [Obsolete("Only use this when absolutetly necessary. Use the more modern IWriteableStream interfaced overload if possible.", false)]
    [return: MaybeNull]
    public static TSerializable? Deserialize<TSerializable>(this IDeserializer<TSerializable> deserializer,
                                                            [DisallowNull] Stream stream,
                                                            Int64 offset,
                                                            SerializationFinishAction actionAfter) =>
        deserializer.Deserialize(stream: stream,
                                 offset: offset,
                                 read: out _,
                                 actionAfter: actionAfter);
    /// <summary>
    /// Deserializes the specified stream starting at the specified offset into an instance of type <typeparamref name="TSerializable"/>.
    /// </summary>
    /// <param name="deserializer">The deserializer that will perform the serialization.</param>
    /// <param name="stream">The stream to deserialize the graph from.</param>
    /// <param name="read">The amount of elements read from the <paramref name="stream"/> parameter.</param>
    /// <param name="actionAfter">The actions to perform after the reading operation has finished.</param>
    /// <returns>The instance represented by the bytes in the specified stream</returns>
    [Obsolete("Only use this when absolutetly necessary. Use the more modern IWriteableStream interfaced overload if possible.", false)]
    [return: MaybeNull]
    public static TSerializable? Deserialize<TSerializable>(this IDeserializer<TSerializable> deserializer,
                                                            [DisallowNull] Stream stream,
                                                            out UInt64 read,
                                                            SerializationFinishAction actionAfter) =>
        deserializer.Deserialize(stream: stream,
                                 offset: 0,
                                 read: out read,
                                 actionAfter: actionAfter);
    /// <summary>
    /// Deserializes the specified stream starting at the specified offset into an instance of type <typeparamref name="TSerializable"/>.
    /// </summary>
    /// <param name="deserializer">The deserializer that will perform the serialization.</param>
    /// <param name="stream">The stream to deserialize the graph from.</param>
    /// <returns>The instance represented by the bytes in the specified stream</returns>
    [return: MaybeNull]
    public static TSerializable? Deserialize<TSerializable, TStream>(this IDeserializer<TSerializable> deserializer,
                                                                     [DisallowNull] TStream stream)
        where TStream : IReadableStream =>
            deserializer.Deserialize(stream: stream,
                                     offset: 0,
                                     read: out _,
                                     actionAfter: SerializationFinishAction.CloseStream | SerializationFinishAction.DisposeStream);
    /// <summary>
    /// Deserializes the specified stream starting at the specified offset into an instance of type <typeparamref name="TSerializable"/>.
    /// </summary>
    /// <param name="deserializer">The deserializer that will perform the serialization.</param>
    /// <param name="stream">The stream to deserialize the graph from.</param>
    /// <param name="offset">The offset in the stream where to begin reading.</param>
    /// <returns>The instance represented by the bytes in the specified stream</returns>
    [return: MaybeNull]
    public static TSerializable? Deserialize<TSerializable, TStream>(this IDeserializer<TSerializable> deserializer,
                                                                     [DisallowNull] TStream stream,
                                                                     Int64 offset)
        where TStream : IReadableStream =>
            deserializer.Deserialize(stream: stream,
                                     offset: offset,
                                     read: out _,
                                     actionAfter: SerializationFinishAction.CloseStream | SerializationFinishAction.DisposeStream);
    /// <summary>
    /// Deserializes the specified stream starting at the specified offset into an instance of type <typeparamref name="TSerializable"/>.
    /// </summary>
    /// <param name="deserializer">The deserializer that will perform the serialization.</param>
    /// <param name="stream">The stream to deserialize the graph from.</param>
    /// <param name="read">The amount of elements read from the <paramref name="stream"/> parameter.</param>
    /// <returns>The instance represented by the bytes in the specified stream</returns>
    [return: MaybeNull]
    public static TSerializable? Deserialize<TSerializable, TStream>(this IDeserializer<TSerializable> deserializer,
                                                                     [DisallowNull] TStream stream,
                                                                     out UInt64 read)
        where TStream : IReadableStream =>
            deserializer.Deserialize(stream: stream,
                                     offset: 0,
                                     read: out read,
                                     actionAfter: SerializationFinishAction.CloseStream | SerializationFinishAction.DisposeStream);
    /// <summary>
    /// Deserializes the specified stream starting at the specified offset into an instance of type <typeparamref name="TSerializable"/>.
    /// </summary>
    /// <param name="deserializer">The deserializer that will perform the serialization.</param>
    /// <param name="stream">The stream to deserialize the graph from.</param>
    /// <param name="offset">The offset in the stream where to begin reading.</param>
    /// <param name="read">The amount of elements read from the <paramref name="stream"/> parameter.</param>
    /// <returns>The instance represented by the bytes in the specified stream</returns>
    [return: MaybeNull]
    public static TSerializable? Deserialize<TSerializable, TStream>(this IDeserializer<TSerializable> deserializer,
                                                                     [DisallowNull] TStream stream,
                                                                     Int64 offset,
                                                                     out UInt64 read)
        where TStream : IReadableStream =>
            deserializer.Deserialize(stream: stream,
                                     offset: offset,
                                     read: out read,
                                     actionAfter: SerializationFinishAction.CloseStream | SerializationFinishAction.DisposeStream);
    /// <summary>
    /// Deserializes the specified stream starting at the specified offset into an instance of type <typeparamref name="TSerializable"/>.
    /// </summary>
    /// <param name="deserializer">The deserializer that will perform the serialization.</param>
    /// <param name="stream">The stream to deserialize the graph from.</param>
    /// <param name="actionAfter">The actions to perform after the reading operation has finished.</param>
    /// <returns>The instance represented by the bytes in the specified stream</returns>
    [return: MaybeNull]
    public static TSerializable? Deserialize<TSerializable, TStream>(this IDeserializer<TSerializable> deserializer,
                                                                     [DisallowNull] TStream stream,
                                                                     SerializationFinishAction actionAfter)
        where TStream : IReadableStream =>
            deserializer.Deserialize(stream: stream,
                                     offset: 0,
                                     read: out _,
                                     actionAfter: actionAfter);
    /// <summary>
    /// Deserializes the specified stream starting at the specified offset into an instance of type <typeparamref name="TSerializable"/>.
    /// </summary>
    /// <param name="deserializer">The deserializer that will perform the serialization.</param>
    /// <param name="stream">The stream to deserialize the graph from.</param>
    /// <param name="offset">The offset in the stream where to begin reading.</param>
    /// <param name="actionAfter">The actions to perform after the reading operation has finished.</param>
    /// <returns>The instance represented by the bytes in the specified stream</returns>
    [return: MaybeNull]
    public static TSerializable? Deserialize<TSerializable, TStream>(this IDeserializer<TSerializable> deserializer,
                                                                     [DisallowNull] TStream stream,
                                                                     Int64 offset,
                                                                     SerializationFinishAction actionAfter)
        where TStream : IReadableStream =>
            deserializer.Deserialize(stream: stream,
                                     offset: offset,
                                     read: out _,
                                     actionAfter: actionAfter);
    /// <summary>
    /// Deserializes the specified stream starting at the specified offset into an instance of type <typeparamref name="TSerializable"/>.
    /// </summary>
    /// <param name="deserializer">The deserializer that will perform the serialization.</param>
    /// <param name="stream">The stream to deserialize the graph from.</param>
    /// <param name="read">The amount of elements read from the <paramref name="stream"/> parameter.</param>
    /// <param name="actionAfter">The actions to perform after the reading operation has finished.</param>
    /// <returns>The instance represented by the bytes in the specified stream</returns>
    [return: MaybeNull]
    public static TSerializable? Deserialize<TSerializable, TStream>(this IDeserializer<TSerializable> deserializer,
                                                                     [DisallowNull] TStream stream,
                                                                     out UInt64 read,
                                                                     SerializationFinishAction actionAfter)
        where TStream : IReadableStream =>
            deserializer.Deserialize(stream: stream,
                                     offset: 0,
                                     read: out read,
                                     actionAfter: actionAfter);

    /// <summary>
    /// Tries to deserialize the specified stream starting at the specified offset into an instance of type <typeparamref name="TSerializable"/>.
    /// </summary>
    /// <param name="deserializer">The deserializer that will perform the serialization.</param>
    /// <param name="stream">The stream to deserialize the graph from.</param>
    /// <param name="result">The instance represented by the bytes in the specified stream.</param>
    /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
    [Obsolete("Only use this when absolutetly necessary. Use the more modern IWriteableStream interfaced overload if possible.", false)]
    public static Boolean TryDeserialize<TSerializable>(this IDeserializer<TSerializable> deserializer,
                                                        [DisallowNull] Stream stream,
                                                        [AllowNull] out TSerializable? result) =>
        deserializer.TryDeserialize(stream: stream,
                                    offset: 0,
                                    read: out _,
                                    actionAfter: SerializationFinishAction.CloseStream | SerializationFinishAction.DisposeStream,
                                    result: out result);
    /// <summary>
    /// Tries to deserialize the specified stream starting at the specified offset into an instance of type <typeparamref name="TSerializable"/>.
    /// </summary>
    /// <param name="deserializer">The deserializer that will perform the serialization.</param>
    /// <param name="stream">The stream to deserialize the graph from.</param>
    /// <param name="offset">The offset in the stream where to begin reading.</param>
    /// <param name="result">The instance represented by the bytes in the specified stream.</param>
    /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
    [Obsolete("Only use this when absolutetly necessary. Use the more modern IWriteableStream interfaced overload if possible.", false)]
    public static Boolean TryDeserialize<TSerializable>(this IDeserializer<TSerializable> deserializer,
                                                        [DisallowNull] Stream stream,
                                                        Int64 offset,
                                                        [AllowNull] out TSerializable? result) =>
        deserializer.TryDeserialize(stream: stream,
                                    offset: offset,
                                    read: out _,
                                    actionAfter: SerializationFinishAction.CloseStream | SerializationFinishAction.DisposeStream,
                                    result: out result);
    /// <summary>
    /// Tries to deserialize the specified stream starting at the specified offset into an instance of type <typeparamref name="TSerializable"/>.
    /// </summary>
    /// <param name="deserializer">The deserializer that will perform the serialization.</param>
    /// <param name="stream">The stream to deserialize the graph from.</param>
    /// <param name="read">The amount of elements read from the <paramref name="stream"/> parameter.</param>
    /// <param name="result">The instance represented by the bytes in the specified stream.</param>
    /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
    [Obsolete("Only use this when absolutetly necessary. Use the more modern IWriteableStream interfaced overload if possible.", false)]
    public static Boolean TryDeserialize<TSerializable>(this IDeserializer<TSerializable> deserializer,
                                                        [DisallowNull] Stream stream,
                                                        out UInt64 read,
                                                        [AllowNull] out TSerializable? result) =>
        deserializer.TryDeserialize(stream: stream,
                                    offset: 0,
                                    read: out read,
                                    actionAfter: SerializationFinishAction.CloseStream | SerializationFinishAction.DisposeStream,
                                    result: out result);
    /// <summary>
    /// Tries to deserialize the specified stream starting at the specified offset into an instance of type <typeparamref name="TSerializable"/>.
    /// </summary>
    /// <param name="deserializer">The deserializer that will perform the serialization.</param>
    /// <param name="stream">The stream to deserialize the graph from.</param>
    /// <param name="offset">The offset in the stream where to begin reading.</param>
    /// <param name="read">The amount of elements read from the <paramref name="stream"/> parameter.</param>
    /// <param name="result">The instance represented by the bytes in the specified stream.</param>
    /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
    [Obsolete("Only use this when absolutetly necessary. Use the more modern IWriteableStream interfaced overload if possible.", false)]
    public static Boolean TryDeserialize<TSerializable>(this IDeserializer<TSerializable> deserializer,
                                                        [DisallowNull] Stream stream,
                                                        Int64 offset,
                                                        out UInt64 read,
                                                        [AllowNull] out TSerializable? result) =>
        deserializer.TryDeserialize(stream: stream,
                                    offset: offset,
                                    read: out read,
                                    actionAfter: SerializationFinishAction.CloseStream | SerializationFinishAction.DisposeStream,
                                    result: out result);
    /// <summary>
    /// Tries to deserialize the specified stream starting at the specified offset into an instance of type <typeparamref name="TSerializable"/>.
    /// </summary>
    /// <param name="deserializer">The deserializer that will perform the serialization.</param>
    /// <param name="stream">The stream to deserialize the graph from.</param>
    /// <param name="actionAfter">The actions to perform after the reading operation has finished.</param>
    /// <param name="result">The instance represented by the bytes in the specified stream.</param>
    /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
    [Obsolete("Only use this when absolutetly necessary. Use the more modern IWriteableStream interfaced overload if possible.", false)]
    public static Boolean TryDeserialize<TSerializable>(this IDeserializer<TSerializable> deserializer,
                                                        [DisallowNull] Stream stream,
                                                        SerializationFinishAction actionAfter,
                                                        [AllowNull] out TSerializable? result) =>
        deserializer.TryDeserialize(stream: stream,
                                    offset: 0,
                                    read: out _,
                                    actionAfter: actionAfter,
                                    result: out result);
    /// <summary>
    /// Tries to deserialize the specified stream starting at the specified offset into an instance of type <typeparamref name="TSerializable"/>.
    /// </summary>
    /// <param name="deserializer">The deserializer that will perform the serialization.</param>
    /// <param name="stream">The stream to deserialize the graph from.</param>
    /// <param name="offset">The offset in the stream where to begin reading.</param>
    /// <param name="actionAfter">The actions to perform after the reading operation has finished.</param>
    /// <param name="result">The instance represented by the bytes in the specified stream.</param>
    /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
    [Obsolete("Only use this when absolutetly necessary. Use the more modern IWriteableStream interfaced overload if possible.", false)]
    public static Boolean TryDeserialize<TSerializable>(this IDeserializer<TSerializable> deserializer,
                                                        [DisallowNull] Stream stream,
                                                        Int64 offset,
                                                        SerializationFinishAction actionAfter,
                                                        [AllowNull] out TSerializable? result) =>
        deserializer.TryDeserialize(stream: stream,
                                    offset: offset,
                                    read: out _,
                                    actionAfter: actionAfter,
                                    result: out result);
    /// <summary>
    /// Tries to deserialize the specified stream starting at the specified offset into an instance of type <typeparamref name="TSerializable"/>.
    /// </summary>
    /// <param name="deserializer">The deserializer that will perform the serialization.</param>
    /// <param name="stream">The stream to deserialize the graph from.</param>
    /// <param name="read">The amount of elements read from the <paramref name="stream"/> parameter.</param>
    /// <param name="actionAfter">The actions to perform after the reading operation has finished.</param>
    /// <param name="result">The instance represented by the bytes in the specified stream.</param>
    /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
    [Obsolete("Only use this when absolutetly necessary. Use the more modern IWriteableStream interfaced overload if possible.", false)]
    public static Boolean TryDeserialize<TSerializable>(this IDeserializer<TSerializable> deserializer,
                                                        [DisallowNull] Stream stream,
                                                        out UInt64 read,
                                                        SerializationFinishAction actionAfter,
                                                        [AllowNull] out TSerializable? result) =>
        deserializer.TryDeserialize(stream: stream,
                                    offset: 0,
                                    read: out read,
                                    actionAfter: actionAfter,
                                    result: out result);
    /// <summary>
    /// Tries to deserialize the specified stream starting at the specified offset into an instance of type <typeparamref name="TSerializable"/>.
    /// </summary>
    /// <param name="deserializer">The deserializer that will perform the serialization.</param>
    /// <param name="stream">The stream to deserialize the graph from.</param>
    /// <param name="result">The instance represented by the bytes in the specified stream.</param>
    /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
    public static Boolean TryDeserialize<TSerializable, TStream>(this IDeserializer<TSerializable> deserializer,
                                                                 [DisallowNull] TStream stream,
                                                                 [AllowNull] out TSerializable? result)
        where TStream : IReadableStream =>
            deserializer.TryDeserialize(stream: stream,
                                        offset: 0,
                                        read: out _,
                                        actionAfter: SerializationFinishAction.CloseStream | SerializationFinishAction.DisposeStream,
                                        result: out result);
    /// <summary>
    /// Tries to deserialize the specified stream starting at the specified offset into an instance of type <typeparamref name="TSerializable"/>.
    /// </summary>
    /// <param name="deserializer">The deserializer that will perform the serialization.</param>
    /// <param name="stream">The stream to deserialize the graph from.</param>
    /// <param name="offset">The offset in the stream where to begin reading.</param>
    /// <param name="result">The instance represented by the bytes in the specified stream.</param>
    /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
    public static Boolean TryDeserialize<TSerializable, TStream>(this IDeserializer<TSerializable> deserializer,
                                                                 [DisallowNull] TStream stream,
                                                                 Int64 offset,
                                                                 [AllowNull] out TSerializable? result)
        where TStream : IReadableStream =>
            deserializer.TryDeserialize(stream: stream,
                                        offset: offset,
                                        read: out _,
                                        actionAfter: SerializationFinishAction.CloseStream | SerializationFinishAction.DisposeStream,
                                        result: out result);
    /// <summary>
    /// Tries to deserialize the specified stream starting at the specified offset into an instance of type <typeparamref name="TSerializable"/>.
    /// </summary>
    /// <param name="deserializer">The deserializer that will perform the serialization.</param>
    /// <param name="stream">The stream to deserialize the graph from.</param>
    /// <param name="read">The amount of elements read from the <paramref name="stream"/> parameter.</param>
    /// <param name="result">The instance represented by the bytes in the specified stream.</param>
    /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
    public static Boolean TryDeserialize<TSerializable, TStream>(this IDeserializer<TSerializable> deserializer,
                                                                 [DisallowNull] TStream stream,
                                                                 out UInt64 read,
                                                                 [AllowNull] out TSerializable? result)
        where TStream : IReadableStream =>
            deserializer.TryDeserialize(stream: stream,
                                        offset: 0,
                                        read: out read,
                                        actionAfter: SerializationFinishAction.CloseStream | SerializationFinishAction.DisposeStream,
                                        result: out result);
    /// <summary>
    /// Tries to deserialize the specified stream starting at the specified offset into an instance of type <typeparamref name="TSerializable"/>.
    /// </summary>
    /// <param name="deserializer">The deserializer that will perform the serialization.</param>
    /// <param name="stream">The stream to deserialize the graph from.</param>
    /// <param name="offset">The offset in the stream where to begin reading.</param>
    /// <param name="read">The amount of elements read from the <paramref name="stream"/> parameter.</param>
    /// <param name="result">The instance represented by the bytes in the specified stream.</param>
    /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
    public static Boolean TryDeserialize<TSerializable, TStream>(this IDeserializer<TSerializable> deserializer,
                                                                 [DisallowNull] TStream stream,
                                                                 Int64 offset,
                                                                 out UInt64 read,
                                                                 [AllowNull] out TSerializable? result)
        where TStream : IReadableStream =>
            deserializer.TryDeserialize(stream: stream,
                                        offset: offset,
                                        read: out read,
                                        actionAfter: SerializationFinishAction.CloseStream | SerializationFinishAction.DisposeStream,
                                        result: out result);
    /// <summary>
    /// Tries to deserialize the specified stream starting at the specified offset into an instance of type <typeparamref name="TSerializable"/>.
    /// </summary>
    /// <param name="deserializer">The deserializer that will perform the serialization.</param>
    /// <param name="stream">The stream to deserialize the graph from.</param>
    /// <param name="actionAfter">The actions to perform after the reading operation has finished.</param>
    /// <param name="result">The instance represented by the bytes in the specified stream.</param>
    /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
    public static Boolean TryDeserialize<TSerializable, TStream>(this IDeserializer<TSerializable> deserializer,
                                                                 [DisallowNull] TStream stream,
                                                                 SerializationFinishAction actionAfter,
                                                                 [AllowNull] out TSerializable? result)
        where TStream : IReadableStream =>
            deserializer.TryDeserialize(stream: stream,
                                        offset: 0,
                                        read: out _,
                                        actionAfter: actionAfter,
                                        result: out result);
    /// <summary>
    /// Tries to deserialize the specified stream starting at the specified offset into an instance of type <typeparamref name="TSerializable"/>.
    /// </summary>
    /// <param name="deserializer">The deserializer that will perform the serialization.</param>
    /// <param name="stream">The stream to deserialize the graph from.</param>
    /// <param name="offset">The offset in the stream where to begin reading.</param>
    /// <param name="actionAfter">The actions to perform after the reading operation has finished.</param>
    /// <param name="result">The instance represented by the bytes in the specified stream.</param>
    /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
    public static Boolean TryDeserialize<TSerializable, TStream>(this IDeserializer<TSerializable> deserializer,
                                                                 [DisallowNull] TStream stream,
                                                                 Int64 offset,
                                                                 SerializationFinishAction actionAfter,
                                                                 [AllowNull] out TSerializable? result)
        where TStream : IReadableStream =>
            deserializer.TryDeserialize(stream: stream,
                                        offset: offset,
                                        read: out _,
                                        actionAfter: actionAfter,
                                        result: out result);
    /// <summary>
    /// Tries to deserialize the specified stream starting at the specified offset into an instance of type <typeparamref name="TSerializable"/>.
    /// </summary>
    /// <param name="deserializer">The deserializer that will perform the serialization.</param>
    /// <param name="stream">The stream to deserialize the graph from.</param>
    /// <param name="read">The amount of elements read from the <paramref name="stream"/> parameter.</param>
    /// <param name="actionAfter">The actions to perform after the reading operation has finished.</param>
    /// <param name="result">The instance represented by the bytes in the specified stream.</param>
    /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
    public static Boolean TryDeserialize<TSerializable, TStream>(this IDeserializer<TSerializable> deserializer,
                                                                 [DisallowNull] TStream stream,
                                                                 out UInt64 read,
                                                                 SerializationFinishAction actionAfter,
                                                                 [AllowNull] out TSerializable? result)
        where TStream : IReadableStream =>
            deserializer.TryDeserialize(stream: stream,
                                        offset: 0,
                                        read: out read,
                                        actionAfter: actionAfter,
                                        result: out result);
}

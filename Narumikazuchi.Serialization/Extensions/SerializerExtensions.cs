namespace Narumikazuchi.Serialization;

/// <summary>
/// Extends every <see cref="ISerializer{TSerializable}"/> with less complex serialization methods.
/// </summary>
public static class SerializerExtensions
{
    /// <summary>
    /// Serializes the specified graph into the specified stream starting at the specified offset in the stream.
    /// </summary>
    /// <param name="serializer">The serializer that will perform the serialization.</param>
    /// <param name="stream">The stream to serialize the graph into.</param>
    /// <param name="graph">The graph to serialize.</param>
    /// <returns>The amount of bytes written</returns>
    [Obsolete("Only use this when absolutetly necessary. Use the more modern IWriteableStream interface overload if possible.", false)]
    public static UInt64 Serialize<TSerializable>(this ISerializer<TSerializable> serializer,
                                                  [DisallowNull] Stream stream,
                                                  [AllowNull] TSerializable? graph) =>
        serializer.Serialize(stream: stream,
                             offset: 0,
                             actionAfter: SerializationFinishAction.CloseStream | SerializationFinishAction.DisposeStream,
                             graph: graph);
    /// <summary>
    /// Serializes the specified graph into the specified stream starting at the specified offset in the stream.
    /// </summary>
    /// <param name="serializer">The serializer that will perform the serialization.</param>
    /// <param name="stream">The stream to serialize the graph into.</param>
    /// <param name="graph">The graph to serialize.</param>
    /// <param name="offset">The offset in the stream where to begin writing.</param>
    /// <returns>The amount of bytes written</returns>
    [Obsolete("Only use this when absolutetly necessary. Use the more modern IWriteableStream interface overload if possible.", false)]
    public static UInt64 Serialize<TSerializable>(this ISerializer<TSerializable> serializer,
                                                  [DisallowNull] Stream stream,
                                                  [AllowNull] TSerializable? graph,
                                                  Int64 offset) =>
        serializer.Serialize(stream: stream,
                             offset: offset,
                             actionAfter: SerializationFinishAction.CloseStream | SerializationFinishAction.DisposeStream,
                             graph: graph);
    /// <summary>
    /// Serializes the specified graph into the specified stream starting at the specified offset in the stream.
    /// </summary>
    /// <param name="serializer">The serializer that will perform the serialization.</param>
    /// <param name="stream">The stream to serialize the graph into.</param>
    /// <param name="graph">The graph to serialize.</param>
    /// <param name="actionAfter">The actions to perform after the writing operation has finished.</param>
    /// <returns>The amount of bytes written</returns>
    [Obsolete("Only use this when absolutetly necessary. Use the more modern IWriteableStream interface overload if possible.", false)]
    public static UInt64 Serialize<TSerializable>(this ISerializer<TSerializable> serializer,
                                                  [DisallowNull] Stream stream,
                                                  [AllowNull] TSerializable? graph,
                                                  SerializationFinishAction actionAfter) =>
        serializer.Serialize(stream: stream,
                             offset: 0,
                             actionAfter: actionAfter,
                             graph: graph);
    /// <summary>
    /// Serializes the specified graph into the specified stream starting at the specified offset in the stream.
    /// </summary>
    /// <param name="serializer">The serializer that will perform the serialization.</param>
    /// <param name="stream">The stream to serialize the graph into.</param>
    /// <param name="graph">The graph to serialize.</param>
    /// <returns>The amount of bytes written</returns>
    public static UInt64 Serialize<TSerializable, TStream>(this ISerializer<TSerializable> serializer,
                                                           [DisallowNull] TStream stream,
                                                           [AllowNull] TSerializable? graph)
        where TStream : IWriteableStream =>
            serializer.Serialize(stream: stream,
                                 offset: 0,
                                 actionAfter: SerializationFinishAction.CloseStream | SerializationFinishAction.DisposeStream,
                                 graph: graph);
    /// <summary>
    /// Serializes the specified graph into the specified stream starting at the specified offset in the stream.
    /// </summary>
    /// <param name="serializer">The serializer that will perform the serialization.</param>
    /// <param name="stream">The stream to serialize the graph into.</param>
    /// <param name="graph">The graph to serialize.</param>
    /// <param name="offset">The offset in the stream where to begin writing.</param>
    /// <returns>The amount of bytes written</returns>
    public static UInt64 Serialize<TSerializable, TStream>(this ISerializer<TSerializable> serializer,
                                                           [DisallowNull] TStream stream,
                                                           [AllowNull] TSerializable? graph,
                                                           Int64 offset)
        where TStream : IWriteableStream =>
            serializer.Serialize(stream: stream,
                                 offset: offset,
                                 actionAfter: SerializationFinishAction.CloseStream | SerializationFinishAction.DisposeStream,
                                 graph: graph);
    /// <summary>
    /// Serializes the specified graph into the specified stream starting at the specified offset in the stream.
    /// </summary>
    /// <param name="serializer">The serializer that will perform the serialization.</param>
    /// <param name="stream">The stream to serialize the graph into.</param>
    /// <param name="graph">The graph to serialize.</param>
    /// <param name="actionAfter">The actions to perform after the writing operation has finished.</param>
    /// <returns>The amount of bytes written</returns>
    public static UInt64 Serialize<TSerializable, TStream>(this ISerializer<TSerializable> serializer,
                                                           [DisallowNull] TStream stream,
                                                           [AllowNull] TSerializable? graph,
                                                           SerializationFinishAction actionAfter)
        where TStream : IWriteableStream =>
            serializer.Serialize(stream: stream,
                                 offset: 0,
                                 actionAfter: actionAfter,
                                 graph: graph);

    /// <summary>
    /// Serializes the specified graph into the specified stream starting at the specified offset in the stream.
    /// </summary>
    /// <param name="serializer">The serializer that will perform the serialization.</param>
    /// <param name="stream">The stream to serialize the graph into.</param>
    /// <param name="graph">The graph to serialize.</param>
    /// <returns>The amount of bytes written</returns>
    [Obsolete("Only use this when absolutetly necessary. Use the more modern IWriteableStream interface overload if possible.", false)]
    public static Boolean TrySerialize<TSerializable>(this ISerializer<TSerializable> serializer,
                                                      [DisallowNull] Stream stream,
                                                      [AllowNull] TSerializable? graph) =>
        serializer.TrySerialize(stream: stream,
                                offset: 0,
                                written: out _,
                                actionAfter: SerializationFinishAction.CloseStream | SerializationFinishAction.DisposeStream,
                                graph: graph);
    /// <summary>
    /// Serializes the specified graph into the specified stream starting at the specified offset in the stream.
    /// </summary>
    /// <param name="serializer">The serializer that will perform the serialization.</param>
    /// <param name="stream">The stream to serialize the graph into.</param>
    /// <param name="graph">The graph to serialize.</param>
    /// <param name="offset">The offset in the stream where to begin writing.</param>
    /// <returns>The amount of bytes written</returns>
    [Obsolete("Only use this when absolutetly necessary. Use the more modern IWriteableStream interface overload if possible.", false)]
    public static Boolean TrySerialize<TSerializable>(this ISerializer<TSerializable> serializer,
                                                      [DisallowNull] Stream stream,
                                                      [AllowNull] TSerializable? graph,
                                                      Int64 offset) =>
        serializer.TrySerialize(stream: stream,
                                offset: offset,
                                written: out _,
                                actionAfter: SerializationFinishAction.CloseStream | SerializationFinishAction.DisposeStream,
                                graph: graph);
    /// <summary>
    /// Serializes the specified graph into the specified stream starting at the specified offset in the stream.
    /// </summary>
    /// <param name="serializer">The serializer that will perform the serialization.</param>
    /// <param name="stream">The stream to serialize the graph into.</param>
    /// <param name="graph">The graph to serialize.</param>
    /// <param name="written">The amount of bytes written.</param>
    /// <returns>The amount of bytes written</returns>
    [Obsolete("Only use this when absolutetly necessary. Use the more modern IWriteableStream interface overload if possible.", false)]
    public static Boolean TrySerialize<TSerializable>(this ISerializer<TSerializable> serializer,
                                                      [DisallowNull] Stream stream,
                                                      [AllowNull] TSerializable? graph,
                                                      out UInt64 written) =>
        serializer.TrySerialize(stream: stream,
                                offset: 0,
                                written: out written,
                                actionAfter: SerializationFinishAction.CloseStream | SerializationFinishAction.DisposeStream,
                                graph: graph);
    /// <summary>
    /// Serializes the specified graph into the specified stream starting at the specified offset in the stream.
    /// </summary>
    /// <param name="serializer">The serializer that will perform the serialization.</param>
    /// <param name="stream">The stream to serialize the graph into.</param>
    /// <param name="graph">The graph to serialize.</param>
    /// <param name="offset">The offset in the stream where to begin writing.</param>
    /// <param name="written">The amount of bytes written.</param>
    /// <returns>The amount of bytes written</returns>
    [Obsolete("Only use this when absolutetly necessary. Use the more modern IWriteableStream interface overload if possible.", false)]
    public static Boolean TrySerialize<TSerializable>(this ISerializer<TSerializable> serializer,
                                                      [DisallowNull] Stream stream,
                                                      [AllowNull] TSerializable graph,
                                                      Int64 offset,
                                                      out UInt64 written) =>
        serializer.TrySerialize(stream: stream,
                                offset: offset,
                                written: out written,
                                actionAfter: SerializationFinishAction.CloseStream | SerializationFinishAction.DisposeStream,
                                graph: graph);
    /// <summary>
    /// Serializes the specified graph into the specified stream starting at the specified offset in the stream.
    /// </summary>
    /// <param name="serializer">The serializer that will perform the serialization.</param>
    /// <param name="stream">The stream to serialize the graph into.</param>
    /// <param name="graph">The graph to serialize.</param>
    /// <param name="actionAfter">The actions to perform after the writing operation has finished.</param>
    /// <returns>The amount of bytes written</returns>
    [Obsolete("Only use this when absolutetly necessary. Use the more modern IWriteableStream interface overload if possible.", false)]
    public static Boolean TrySerialize<TSerializable>(this ISerializer<TSerializable> serializer,
                                                      [DisallowNull] Stream stream,
                                                      [AllowNull] TSerializable? graph,
                                                      SerializationFinishAction actionAfter) =>
        serializer.TrySerialize(stream: stream,
                                offset: 0,
                                written: out _,
                                actionAfter: actionAfter,
                                graph: graph);
    /// <summary>
    /// Serializes the specified graph into the specified stream starting at the specified offset in the stream.
    /// </summary>
    /// <param name="serializer">The serializer that will perform the serialization.</param>
    /// <param name="stream">The stream to serialize the graph into.</param>
    /// <param name="graph">The graph to serialize.</param>
    /// <param name="offset">The offset in the stream where to begin writing.</param>
    /// <param name="actionAfter">The actions to perform after the writing operation has finished.</param>
    /// <returns>The amount of bytes written</returns>
    [Obsolete("Only use this when absolutetly necessary. Use the more modern IWriteableStream interface overload if possible.", false)]
    public static Boolean TrySerialize<TSerializable>(this ISerializer<TSerializable> serializer,
                                                      [DisallowNull] Stream stream,
                                                      [AllowNull] TSerializable? graph,
                                                      Int64 offset,
                                                      SerializationFinishAction actionAfter) =>
        serializer.TrySerialize(stream: stream,
                                offset: offset,
                                written: out _,
                                actionAfter: actionAfter,
                                graph: graph);
    /// <summary>
    /// Serializes the specified graph into the specified stream starting at the specified offset in the stream.
    /// </summary>
    /// <param name="serializer">The serializer that will perform the serialization.</param>
    /// <param name="stream">The stream to serialize the graph into.</param>
    /// <param name="graph">The graph to serialize.</param>
    /// <param name="written">The amount of bytes written.</param>
    /// <param name="actionAfter">The actions to perform after the writing operation has finished.</param>
    /// <returns>The amount of bytes written</returns>
    [Obsolete("Only use this when absolutetly necessary. Use the more modern IWriteableStream interface overload if possible.", false)]
    public static Boolean TrySerialize<TSerializable>(this ISerializer<TSerializable> serializer,
                                                      [DisallowNull] Stream stream,
                                                      [AllowNull] TSerializable? graph,
                                                      out UInt64 written,
                                                      SerializationFinishAction actionAfter) =>
        serializer.TrySerialize(stream: stream,
                                offset: 0,
                                written: out written,
                                actionAfter: actionAfter,
                                graph: graph);
    /// <summary>
    /// Serializes the specified graph into the specified stream starting at the specified offset in the stream.
    /// </summary>
    /// <param name="serializer">The serializer that will perform the serialization.</param>
    /// <param name="stream">The stream to serialize the graph into.</param>
    /// <param name="graph">The graph to serialize.</param>
    /// <returns>The amount of bytes written</returns>
    public static Boolean TrySerialize<TSerializable, TStream>(this ISerializer<TSerializable> serializer,
                                                               [DisallowNull] TStream stream,
                                                               [AllowNull] TSerializable? graph)
        where TStream : IWriteableStream =>
            serializer.TrySerialize(stream: stream,
                                    offset: 0,
                                    written: out _,
                                    actionAfter: SerializationFinishAction.CloseStream | SerializationFinishAction.DisposeStream,
                                    graph: graph);
    /// <summary>
    /// Serializes the specified graph into the specified stream starting at the specified offset in the stream.
    /// </summary>
    /// <param name="serializer">The serializer that will perform the serialization.</param>
    /// <param name="stream">The stream to serialize the graph into.</param>
    /// <param name="graph">The graph to serialize.</param>
    /// <param name="offset">The offset in the stream where to begin writing.</param>
    /// <returns>The amount of bytes written</returns>
    public static Boolean TrySerialize<TSerializable, TStream>(this ISerializer<TSerializable> serializer,
                                                               [DisallowNull] TStream stream,
                                                               [AllowNull] TSerializable? graph,
                                                               Int64 offset)
        where TStream : IWriteableStream =>
            serializer.TrySerialize(stream: stream,
                                    offset: offset,
                                    written: out _,
                                    actionAfter: SerializationFinishAction.CloseStream | SerializationFinishAction.DisposeStream,
                                    graph: graph);
    /// <summary>
    /// Serializes the specified graph into the specified stream starting at the specified offset in the stream.
    /// </summary>
    /// <param name="serializer">The serializer that will perform the serialization.</param>
    /// <param name="stream">The stream to serialize the graph into.</param>
    /// <param name="graph">The graph to serialize.</param>
    /// <param name="written">The amount of bytes written.</param>
    /// <returns>The amount of bytes written</returns>
    public static Boolean TrySerialize<TSerializable, TStream>(this ISerializer<TSerializable> serializer,
                                                               [DisallowNull] TStream stream,
                                                               [AllowNull] TSerializable? graph,
                                                               out UInt64 written)
        where TStream : IWriteableStream =>
            serializer.TrySerialize(stream: stream,
                                    offset: 0,
                                    written: out written,
                                    actionAfter: SerializationFinishAction.CloseStream | SerializationFinishAction.DisposeStream,
                                    graph: graph);
    /// <summary>
    /// Serializes the specified graph into the specified stream starting at the specified offset in the stream.
    /// </summary>
    /// <param name="serializer">The serializer that will perform the serialization.</param>
    /// <param name="stream">The stream to serialize the graph into.</param>
    /// <param name="graph">The graph to serialize.</param>
    /// <param name="offset">The offset in the stream where to begin writing.</param>
    /// <param name="written">The amount of bytes written.</param>
    /// <returns>The amount of bytes written</returns>
    public static Boolean TrySerialize<TSerializable, TStream>(this ISerializer<TSerializable> serializer,
                                                               [DisallowNull] TStream stream,
                                                               [AllowNull] TSerializable graph,
                                                               Int64 offset,
                                                               out UInt64 written)
        where TStream : IWriteableStream =>
            serializer.TrySerialize(stream: stream,
                                    offset: offset,
                                    written: out written,
                                    actionAfter: SerializationFinishAction.CloseStream | SerializationFinishAction.DisposeStream,
                                    graph: graph);
    /// <summary>
    /// Serializes the specified graph into the specified stream starting at the specified offset in the stream.
    /// </summary>
    /// <param name="serializer">The serializer that will perform the serialization.</param>
    /// <param name="stream">The stream to serialize the graph into.</param>
    /// <param name="graph">The graph to serialize.</param>
    /// <param name="actionAfter">The actions to perform after the writing operation has finished.</param>
    /// <returns>The amount of bytes written</returns>
    public static Boolean TrySerialize<TSerializable, TStream>(this ISerializer<TSerializable> serializer,
                                                               [DisallowNull] TStream stream,
                                                               [AllowNull] TSerializable? graph,
                                                               SerializationFinishAction actionAfter)
        where TStream : IWriteableStream =>
            serializer.TrySerialize(stream: stream,
                                    offset: 0,
                                    written: out _,
                                    actionAfter: actionAfter,
                                    graph: graph);
    /// <summary>
    /// Serializes the specified graph into the specified stream starting at the specified offset in the stream.
    /// </summary>
    /// <param name="serializer">The serializer that will perform the serialization.</param>
    /// <param name="stream">The stream to serialize the graph into.</param>
    /// <param name="graph">The graph to serialize.</param>
    /// <param name="offset">The offset in the stream where to begin writing.</param>
    /// <param name="actionAfter">The actions to perform after the writing operation has finished.</param>
    /// <returns>The amount of bytes written</returns>
    public static Boolean TrySerialize<TSerializable, TStream>(this ISerializer<TSerializable> serializer,
                                                               [DisallowNull] TStream stream,
                                                               [AllowNull] TSerializable? graph,
                                                               Int64 offset,
                                                               SerializationFinishAction actionAfter)
        where TStream : IWriteableStream =>
            serializer.TrySerialize(stream: stream,
                                    offset: offset,
                                    written: out _,
                                    actionAfter: actionAfter,
                                    graph: graph);
    /// <summary>
    /// Serializes the specified graph into the specified stream starting at the specified offset in the stream.
    /// </summary>
    /// <param name="serializer">The serializer that will perform the serialization.</param>
    /// <param name="stream">The stream to serialize the graph into.</param>
    /// <param name="graph">The graph to serialize.</param>
    /// <param name="written">The amount of bytes written.</param>
    /// <param name="actionAfter">The actions to perform after the writing operation has finished.</param>
    /// <returns>The amount of bytes written</returns>
    public static Boolean TrySerialize<TSerializable, TStream>(this ISerializer<TSerializable> serializer,
                                                               [DisallowNull] TStream stream,
                                                               [AllowNull] TSerializable? graph,
                                                               out UInt64 written,
                                                               SerializationFinishAction actionAfter)
        where TStream : IWriteableStream =>
            serializer.TrySerialize(stream: stream,
                                    offset: 0,
                                    written: out written,
                                    actionAfter: actionAfter,
                                    graph: graph);
}
namespace Narumikazuchi.Serialization;

/// <summary>
/// Represents a serializer for the specified type <typeparamref name="TSerializable"/>.
/// </summary>
public interface ISerializer<TSerializable> : 
    IUsesSerializationStrategies
{
    /// <summary>
    /// Serializes the specified graph into the specified stream starting at the specified offset in the stream.
    /// </summary>
    /// <param name="stream">The stream to serialize the graph into.</param>
    /// <param name="graph">The graph to serialize.</param>
    /// <returns>The amount of bytes written</returns>
    public UInt64 Serialize([DisallowNull] Stream stream,
                            [AllowNull] TSerializable? graph) =>
        this.Serialize(stream: stream,
                       offset: 0,
                       actionAfter: SerializationFinishAction.CloseStream | SerializationFinishAction.DisposeStream,
                       graph: graph);
    /// <summary>
    /// Serializes the specified graph into the specified stream starting at the specified offset in the stream.
    /// </summary>
    /// <param name="stream">The stream to serialize the graph into.</param>
    /// <param name="graph">The graph to serialize.</param>
    /// <param name="offset">The offset in the stream where to begin writing.</param>
    /// <returns>The amount of bytes written</returns>
    public UInt64 Serialize([DisallowNull] Stream stream,
                            [AllowNull] TSerializable? graph,
                            Int64 offset) =>
        this.Serialize(stream: stream,
                       offset: offset,
                       actionAfter: SerializationFinishAction.CloseStream | SerializationFinishAction.DisposeStream,
                       graph: graph);
    /// <summary>
    /// Serializes the specified graph into the specified stream starting at the specified offset in the stream.
    /// </summary>
    /// <param name="stream">The stream to serialize the graph into.</param>
    /// <param name="graph">The graph to serialize.</param>
    /// <param name="actionAfter">The actions to perform after the writing operation has finished.</param>
    /// <returns>The amount of bytes written</returns>
    public UInt64 Serialize([DisallowNull] Stream stream,
                            [AllowNull] TSerializable? graph,
                            SerializationFinishAction actionAfter) =>
        this.Serialize(stream: stream,
                       offset: 0,
                       actionAfter: actionAfter,
                       graph: graph);
    /// <summary>
    /// Serializes the specified graph into the specified stream starting at the specified offset in the stream.
    /// </summary>
    /// <param name="stream">The stream to serialize the graph into.</param>
    /// <param name="graph">The graph to serialize.</param>
    /// <param name="offset">The offset in the stream where to begin writing.</param>
    /// <param name="actionAfter">The actions to perform after the writing operation has finished.</param>
    /// <returns>The amount of bytes written</returns>
    public UInt64 Serialize([DisallowNull] Stream stream,
                            [AllowNull] TSerializable? graph,
                            Int64 offset,
                            SerializationFinishAction actionAfter);

    /// <summary>
    /// Serializes the specified graph into the specified stream starting at the specified offset in the stream.
    /// </summary>
    /// <param name="stream">The stream to serialize the graph into.</param>
    /// <param name="graph">The graph to serialize.</param>
    /// <returns>The amount of bytes written</returns>
    public Boolean TrySerialize([DisallowNull] Stream stream,
                                [AllowNull] TSerializable? graph) =>
        this.TrySerialize(stream: stream,
                          offset: 0,
                          written: out _,
                          actionAfter: SerializationFinishAction.CloseStream | SerializationFinishAction.DisposeStream,
                          graph: graph);
    /// <summary>
    /// Serializes the specified graph into the specified stream starting at the specified offset in the stream.
    /// </summary>
    /// <param name="stream">The stream to serialize the graph into.</param>
    /// <param name="graph">The graph to serialize.</param>
    /// <param name="offset">The offset in the stream where to begin writing.</param>
    /// <returns>The amount of bytes written</returns>
    public Boolean TrySerialize([DisallowNull] Stream stream,
                                [AllowNull] TSerializable? graph,
                                Int64 offset) =>
        this.TrySerialize(stream: stream,
                          offset: offset,
                          written: out _,
                          actionAfter: SerializationFinishAction.CloseStream | SerializationFinishAction.DisposeStream,
                          graph: graph);
    /// <summary>
    /// Serializes the specified graph into the specified stream starting at the specified offset in the stream.
    /// </summary>
    /// <param name="stream">The stream to serialize the graph into.</param>
    /// <param name="graph">The graph to serialize.</param>
    /// <param name="written">The amount of bytes written.</param>
    /// <returns>The amount of bytes written</returns>
    public Boolean TrySerialize([DisallowNull] Stream stream,
                                [AllowNull] TSerializable? graph,
                                out UInt64 written) =>
        this.TrySerialize(stream: stream,
                          offset: 0,
                          written: out written,
                          actionAfter: SerializationFinishAction.CloseStream | SerializationFinishAction.DisposeStream,
                          graph: graph);
    /// <summary>
    /// Serializes the specified graph into the specified stream starting at the specified offset in the stream.
    /// </summary>
    /// <param name="stream">The stream to serialize the graph into.</param>
    /// <param name="graph">The graph to serialize.</param>
    /// <param name="offset">The offset in the stream where to begin writing.</param>
    /// <param name="written">The amount of bytes written.</param>
    /// <returns>The amount of bytes written</returns>
    public Boolean TrySerialize([DisallowNull] Stream stream,
                                [AllowNull] TSerializable graph,
                                Int64 offset,
                                out UInt64 written) =>
        this.TrySerialize(stream: stream,
                          offset: offset,
                          written: out written,
                          actionAfter: SerializationFinishAction.CloseStream | SerializationFinishAction.DisposeStream,
                          graph: graph);
    /// <summary>
    /// Serializes the specified graph into the specified stream starting at the specified offset in the stream.
    /// </summary>
    /// <param name="stream">The stream to serialize the graph into.</param>
    /// <param name="graph">The graph to serialize.</param>
    /// <param name="actionAfter">The actions to perform after the writing operation has finished.</param>
    /// <returns>The amount of bytes written</returns>
    public Boolean TrySerialize([DisallowNull] Stream stream,
                                [AllowNull] TSerializable? graph,
                                SerializationFinishAction actionAfter) =>
        this.TrySerialize(stream: stream,
                          offset: 0,
                          written: out _,
                          actionAfter: actionAfter,
                          graph: graph);
    /// <summary>
    /// Serializes the specified graph into the specified stream starting at the specified offset in the stream.
    /// </summary>
    /// <param name="stream">The stream to serialize the graph into.</param>
    /// <param name="graph">The graph to serialize.</param>
    /// <param name="offset">The offset in the stream where to begin writing.</param>
    /// <param name="actionAfter">The actions to perform after the writing operation has finished.</param>
    /// <returns>The amount of bytes written</returns>
    public Boolean TrySerialize([DisallowNull] Stream stream,
                                [AllowNull] TSerializable? graph,
                                Int64 offset,
                                SerializationFinishAction actionAfter) =>
        this.TrySerialize(stream: stream,
                          offset: offset,
                          written: out _,
                          actionAfter: actionAfter,
                          graph: graph);
    /// <summary>
    /// Serializes the specified graph into the specified stream starting at the specified offset in the stream.
    /// </summary>
    /// <param name="stream">The stream to serialize the graph into.</param>
    /// <param name="graph">The graph to serialize.</param>
    /// <param name="written">The amount of bytes written.</param>
    /// <param name="actionAfter">The actions to perform after the writing operation has finished.</param>
    /// <returns>The amount of bytes written</returns>
    public Boolean TrySerialize([DisallowNull] Stream stream,
                                [AllowNull] TSerializable? graph,
                                out UInt64 written,
                                SerializationFinishAction actionAfter) =>
        this.TrySerialize(stream: stream,
                          offset: 0,
                          written: out written,
                          actionAfter: actionAfter,
                          graph: graph);
    /// <summary>
    /// Serializes the specified graph into the specified stream starting at the specified offset in the stream.
    /// </summary>
    /// <param name="stream">The stream to serialize the graph into.</param>
    /// <param name="graph">The graph to serialize.</param>
    /// <param name="offset">The offset in the stream where to begin writing.</param>
    /// <param name="written">The amount of bytes written.</param>
    /// <param name="actionAfter">The actions to perform after the writing operation has finished.</param>
    /// <returns>The amount of bytes written</returns>
    public Boolean TrySerialize([DisallowNull] Stream stream,
                                [AllowNull] TSerializable? graph,
                                Int64 offset,
                                out UInt64 written,
                                SerializationFinishAction actionAfter);
}
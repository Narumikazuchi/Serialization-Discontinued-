namespace Narumikazuchi.Serialization;

/// <summary>
/// Represents a serializer for classes that implement the <see cref="ISerializable"/> interface.
/// </summary>
public interface IDeclaredSerializer : ISerializer
{
    /// <summary>
    /// Serializes the specified graph into the specified stream.
    /// </summary>
    /// <param name="stream">The stream to serialize the graph into.</param>
    /// <param name="graph">The graph to serialize.</param>
    /// <returns>The amount of bytes written</returns>
    public UInt64 Serialize([DisallowNull] Stream stream,
                            [DisallowNull] ISerializable graph);
    /// <summary>
    /// Serializes the specified graph into the specified stream starting at the specified offset in the stream.
    /// </summary>
    /// <param name="stream">The stream to serialize the graph into.</param>
    /// <param name="graph">The graph to serialize.</param>
    /// <param name="offset">The offset in the stream where to begin writing.</param>
    /// <returns>The amount of bytes written</returns>
    public UInt64 Serialize([DisallowNull] Stream stream,
                            [DisallowNull] ISerializable graph,
                            in Int64 offset);
    /// <summary>
    /// Serializes the specified graph into the specified stream.
    /// </summary>
    /// <param name="stream">The stream to serialize the graph into.</param>
    /// <param name="graph">The graph to serialize.</param>
    /// <param name="actionAfter">The actions to perform after the writing operation has finished.</param>
    /// <returns>The amount of bytes written</returns>
    public UInt64 Serialize([DisallowNull] Stream stream,
                            [DisallowNull] ISerializable graph,
                            in SerializationFinishAction actionAfter);
    /// <summary>
    /// Serializes the specified graph into the specified stream starting at the specified offset in the stream.
    /// </summary>
    /// <param name="stream">The stream to serialize the graph into.</param>
    /// <param name="graph">The graph to serialize.</param>
    /// <param name="offset">The offset in the stream where to begin writing.</param>
    /// <param name="actionAfter">The actions to perform after the writing operation has finished.</param>
    /// <returns>The amount of bytes written</returns>
    public UInt64 Serialize([DisallowNull] Stream stream,
                            [DisallowNull] ISerializable graph,
                            in Int64 offset,
                            in SerializationFinishAction actionAfter);

    /// <summary>
    /// Tries to serialize the specified graph into the specified stream.
    /// </summary>
    /// <param name="stream">The stream to serialize the graph into.</param>
    /// <param name="graph">The graph to serialize.</param>
    /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
    public Boolean TrySerialize([DisallowNull] Stream stream,
                                [DisallowNull] ISerializable graph);
    /// <summary>
    /// Tries to serialize the specified graph into the specified stream.
    /// </summary>
    /// <param name="stream">The stream to serialize the graph into.</param>
    /// <param name="graph">The graph to serialize.</param>
    /// <param name="offset">The offset in the stream where to begin writing.</param>
    /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
    public Boolean TrySerialize([DisallowNull] Stream stream,
                                [DisallowNull] ISerializable graph,
                                in Int64 offset);
    /// <summary>
    /// Tries to serialize the specified graph into the specified stream.
    /// </summary>
    /// <param name="stream">The stream to serialize the graph into.</param>
    /// <param name="graph">The graph to serialize.</param>
    /// <param name="written">The amount of bytes written.</param>
    /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
    public Boolean TrySerialize([DisallowNull] Stream stream,
                                [DisallowNull] ISerializable graph,
                                out UInt64 written);
    /// <summary>
    /// Tries to serialize the specified graph into the specified stream starting at the specified offset in the stream.
    /// </summary>
    /// <param name="stream">The stream to serialize the graph into.</param>
    /// <param name="graph">The graph to serialize.</param>
    /// <param name="offset">The offset in the stream where to begin writing.</param>
    /// <param name="written">The amount of bytes written.</param>
    /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
    public Boolean TrySerialize([DisallowNull] Stream stream,
                                [DisallowNull] ISerializable graph,
                                in Int64 offset,
                                out UInt64 written);
    /// <summary>
    /// Tries to serialize the specified graph into the specified stream.
    /// </summary>
    /// <param name="stream">The stream to serialize the graph into.</param>
    /// <param name="graph">The graph to serialize.</param>
    /// <param name="actionAfter">The actions to perform after the writing operation has finished.</param>
    /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
    public Boolean TrySerialize([DisallowNull] Stream stream,
                                [DisallowNull] ISerializable graph,
                                in SerializationFinishAction actionAfter);
    /// <summary>
    /// Tries to serialize the specified graph into the specified stream.
    /// </summary>
    /// <param name="stream">The stream to serialize the graph into.</param>
    /// <param name="graph">The graph to serialize.</param>
    /// <param name="offset">The offset in the stream where to begin writing.</param>
    /// <param name="actionAfter">The actions to perform after the writing operation has finished.</param>
    /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
    public Boolean TrySerialize([DisallowNull] Stream stream,
                                [DisallowNull] ISerializable graph,
                                in Int64 offset,
                                in SerializationFinishAction actionAfter);
    /// <summary>
    /// Tries to serialize the specified graph into the specified stream.
    /// </summary>
    /// <param name="stream">The stream to serialize the graph into.</param>
    /// <param name="graph">The graph to serialize.</param>
    /// <param name="written">The amount of bytes written.</param>
    /// <param name="actionAfter">The actions to perform after the writing operation has finished.</param>
    /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
    public Boolean TrySerialize([DisallowNull] Stream stream,
                                [DisallowNull] ISerializable graph,
                                out UInt64 written,
                                in SerializationFinishAction actionAfter);
    /// <summary>
    /// Tries to serialize the specified graph into the specified stream starting at the specified offset in the stream.
    /// </summary>
    /// <param name="stream">The stream to serialize the graph into.</param>
    /// <param name="graph">The graph to serialize.</param>
    /// <param name="offset">The offset in the stream where to begin writing.</param>
    /// <param name="written">The amount of bytes written.</param>
    /// <param name="actionAfter">The actions to perform after the writing operation has finished.</param>
    /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
    public Boolean TrySerialize([DisallowNull] Stream stream,
                                [DisallowNull] ISerializable graph,
                                in Int64 offset,
                                out UInt64 written,
                                in SerializationFinishAction actionAfter);
}
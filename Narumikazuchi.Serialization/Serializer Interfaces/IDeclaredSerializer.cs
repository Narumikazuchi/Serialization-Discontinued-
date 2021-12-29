namespace Narumikazuchi.Serialization;

/// <summary>
/// Represents a serializer for classes that implement the <see cref="ISerializable"/> interface.
/// </summary>
public interface IDeclaredSerializer : 
    ISerializer
{
    /// <summary>
    /// Serializes the specified graph into the specified stream.
    /// </summary>
    /// <param name="stream">The stream to serialize the graph into.</param>
    /// <param name="graph">The graph to serialize.</param>
    /// <returns>The amount of bytes written</returns>
    public UInt64 Serialize<TSerializable>([DisallowNull] Stream stream,
                                           [AllowNull] TSerializable? graph)
        where TSerializable : ISerializable;
    /// <summary>
    /// Serializes the specified graph into the specified stream starting at the specified offset in the stream.
    /// </summary>
    /// <param name="stream">The stream to serialize the graph into.</param>
    /// <param name="graph">The graph to serialize.</param>
    /// <param name="offset">The offset in the stream where to begin writing.</param>
    /// <returns>The amount of bytes written</returns>
    public UInt64 Serialize<TSerializable>([DisallowNull] Stream stream,
                                           [AllowNull] TSerializable? graph,
                                           in Int64 offset)
        where TSerializable : ISerializable;
    /// <summary>
    /// Serializes the specified graph into the specified stream.
    /// </summary>
    /// <param name="stream">The stream to serialize the graph into.</param>
    /// <param name="graph">The graph to serialize.</param>
    /// <param name="actionAfter">The actions to perform after the writing operation has finished.</param>
    /// <returns>The amount of bytes written</returns>
    public UInt64 Serialize<TSerializable>([DisallowNull] Stream stream,
                                           [AllowNull] TSerializable? graph,
                                           in SerializationFinishAction actionAfter)
        where TSerializable : ISerializable;
    /// <summary>
    /// Serializes the specified graph into the specified stream starting at the specified offset in the stream.
    /// </summary>
    /// <param name="stream">The stream to serialize the graph into.</param>
    /// <param name="graph">The graph to serialize.</param>
    /// <param name="offset">The offset in the stream where to begin writing.</param>
    /// <param name="actionAfter">The actions to perform after the writing operation has finished.</param>
    /// <returns>The amount of bytes written</returns>
    public UInt64 Serialize<TSerializable>([DisallowNull] Stream stream,
                                           [AllowNull] TSerializable? graph,
                                           in Int64 offset,
                                           in SerializationFinishAction actionAfter)
        where TSerializable : ISerializable;

    /// <summary>
    /// Tries to serialize the specified graph into the specified stream.
    /// </summary>
    /// <param name="stream">The stream to serialize the graph into.</param>
    /// <param name="graph">The graph to serialize.</param>
    /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
    public Boolean TrySerialize<TSerializable>([DisallowNull] Stream stream,
                                               [AllowNull] TSerializable? graph)
        where TSerializable : ISerializable;
    /// <summary>
    /// Tries to serialize the specified graph into the specified stream.
    /// </summary>
    /// <param name="stream">The stream to serialize the graph into.</param>
    /// <param name="graph">The graph to serialize.</param>
    /// <param name="offset">The offset in the stream where to begin writing.</param>
    /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
    public Boolean TrySerialize<TSerializable>([DisallowNull] Stream stream,
                                               [AllowNull] TSerializable? graph,
                                               in Int64 offset)
        where TSerializable : ISerializable;
    /// <summary>
    /// Tries to serialize the specified graph into the specified stream.
    /// </summary>
    /// <param name="stream">The stream to serialize the graph into.</param>
    /// <param name="graph">The graph to serialize.</param>
    /// <param name="written">The amount of bytes written.</param>
    /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
    public Boolean TrySerialize<TSerializable>([DisallowNull] Stream stream,
                                               [AllowNull] TSerializable? graph,
                                               out UInt64 written)
        where TSerializable : ISerializable;
    /// <summary>
    /// Tries to serialize the specified graph into the specified stream starting at the specified offset in the stream.
    /// </summary>
    /// <param name="stream">The stream to serialize the graph into.</param>
    /// <param name="graph">The graph to serialize.</param>
    /// <param name="offset">The offset in the stream where to begin writing.</param>
    /// <param name="written">The amount of bytes written.</param>
    /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
    public Boolean TrySerialize<TSerializable>([DisallowNull] Stream stream,
                                               [AllowNull] TSerializable? graph,
                                               in Int64 offset,
                                               out UInt64 written)
        where TSerializable : ISerializable;
    /// <summary>
    /// Tries to serialize the specified graph into the specified stream.
    /// </summary>
    /// <param name="stream">The stream to serialize the graph into.</param>
    /// <param name="graph">The graph to serialize.</param>
    /// <param name="actionAfter">The actions to perform after the writing operation has finished.</param>
    /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
    public Boolean TrySerialize<TSerializable>([DisallowNull] Stream stream,
                                               [AllowNull] TSerializable? graph,
                                               in SerializationFinishAction actionAfter)
        where TSerializable : ISerializable;
    /// <summary>
    /// Tries to serialize the specified graph into the specified stream.
    /// </summary>
    /// <param name="stream">The stream to serialize the graph into.</param>
    /// <param name="graph">The graph to serialize.</param>
    /// <param name="offset">The offset in the stream where to begin writing.</param>
    /// <param name="actionAfter">The actions to perform after the writing operation has finished.</param>
    /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
    public Boolean TrySerialize<TSerializable>([DisallowNull] Stream stream,
                                               [AllowNull] TSerializable? graph,
                                               in Int64 offset,
                                               in SerializationFinishAction actionAfter)
        where TSerializable : ISerializable;
    /// <summary>
    /// Tries to serialize the specified graph into the specified stream.
    /// </summary>
    /// <param name="stream">The stream to serialize the graph into.</param>
    /// <param name="graph">The graph to serialize.</param>
    /// <param name="written">The amount of bytes written.</param>
    /// <param name="actionAfter">The actions to perform after the writing operation has finished.</param>
    /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
    public Boolean TrySerialize<TSerializable>([DisallowNull] Stream stream,
                                               [AllowNull] TSerializable? graph,
                                               out UInt64 written,
                                               in SerializationFinishAction actionAfter)
        where TSerializable : ISerializable;
    /// <summary>
    /// Tries to serialize the specified graph into the specified stream starting at the specified offset in the stream.
    /// </summary>
    /// <param name="stream">The stream to serialize the graph into.</param>
    /// <param name="graph">The graph to serialize.</param>
    /// <param name="offset">The offset in the stream where to begin writing.</param>
    /// <param name="written">The amount of bytes written.</param>
    /// <param name="actionAfter">The actions to perform after the writing operation has finished.</param>
    /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
    public Boolean TrySerialize<TSerializable>([DisallowNull] Stream stream,
                                               [AllowNull] TSerializable? graph,
                                               in Int64 offset,
                                               out UInt64 written,
                                               in SerializationFinishAction actionAfter)
        where TSerializable : ISerializable;
}
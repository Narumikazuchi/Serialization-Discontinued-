namespace Narumikazuchi.Serialization;

/// <summary>
/// Represents a serializer for classes that you have no access to. 
/// It allows you to provide delegates to write and read the state instead of relying on an interface implementation.
/// </summary>
public interface IUnboundSerializer : 
    ISerializer
{
    /// <summary>
    /// Serializes the specified graph into the specified stream starting at the specified offset in the stream.
    /// </summary>
    /// <param name="stream">The stream to serialize the graph into.</param>
    /// <param name="graph">The graph to serialize.</param>
    /// <param name="getSerializationData">The <see langword="delegate"/> which specifies the members to serialize.</param>
    /// <returns>The amount of bytes written</returns>
    public UInt64 Serialize<TAny>([DisallowNull] Stream stream,
                                  [AllowNull] TAny? graph,
                                  [DisallowNull] Action<TAny?, ISerializationInfoAdder> getSerializationData);
    /// <summary>
    /// Serializes the specified graph into the specified stream starting at the specified offset in the stream.
    /// </summary>
    /// <param name="stream">The stream to serialize the graph into.</param>
    /// <param name="graph">The graph to serialize.</param>
    /// <param name="offset">The offset in the stream where to begin writing.</param>
    /// <param name="getSerializationData">The <see langword="delegate"/> which specifies the members to serialize.</param>
    /// <returns>The amount of bytes written</returns>
    public UInt64 Serialize<TAny>([DisallowNull] Stream stream,
                                  [AllowNull] TAny? graph,
                                  [DisallowNull] Action<TAny?, ISerializationInfoAdder> getSerializationData,
                                  in Int64 offset);
    /// <summary>
    /// Serializes the specified graph into the specified stream starting at the specified offset in the stream.
    /// </summary>
    /// <param name="stream">The stream to serialize the graph into.</param>
    /// <param name="graph">The graph to serialize.</param>
    /// <param name="getSerializationData">The <see langword="delegate"/> which specifies the members to serialize.</param>
    /// <param name="actionAfter">The actions to perform after the writing operation has finished.</param>
    /// <returns>The amount of bytes written</returns>
    public UInt64 Serialize<TAny>([DisallowNull] Stream stream,
                                  [AllowNull] TAny? graph,
                                  [DisallowNull] Action<TAny?, ISerializationInfoAdder> getSerializationData,
                                  in SerializationFinishAction actionAfter);
    /// <summary>
    /// Serializes the specified graph into the specified stream starting at the specified offset in the stream.
    /// </summary>
    /// <param name="stream">The stream to serialize the graph into.</param>
    /// <param name="graph">The graph to serialize.</param>
    /// <param name="offset">The offset in the stream where to begin writing.</param>
    /// <param name="getSerializationData">The <see langword="delegate"/> which specifies the members to serialize.</param>
    /// <param name="actionAfter">The actions to perform after the writing operation has finished.</param>
    /// <returns>The amount of bytes written</returns>
    public UInt64 Serialize<TAny>([DisallowNull] Stream stream,
                                  [AllowNull] TAny? graph,
                                  [DisallowNull] Action<TAny?, ISerializationInfoAdder> getSerializationData,
                                  in Int64 offset,
                                  in SerializationFinishAction actionAfter);

    /// <summary>
    /// Serializes the specified graph into the specified stream starting at the specified offset in the stream.
    /// </summary>
    /// <param name="stream">The stream to serialize the graph into.</param>
    /// <param name="graph">The graph to serialize.</param>
    /// <param name="getSerializationData">The <see langword="delegate"/> which specifies the members to serialize.</param>
    /// <returns>The amount of bytes written</returns>
    public Boolean TrySerialize<TAny>([DisallowNull] Stream stream,
                                      [AllowNull] TAny? graph,
                                      [DisallowNull] Action<TAny?, ISerializationInfoAdder> getSerializationData);
    /// <summary>
    /// Serializes the specified graph into the specified stream starting at the specified offset in the stream.
    /// </summary>
    /// <param name="stream">The stream to serialize the graph into.</param>
    /// <param name="graph">The graph to serialize.</param>
    /// <param name="offset">The offset in the stream where to begin writing.</param>
    /// <param name="getSerializationData">The <see langword="delegate"/> which specifies the members to serialize.</param>
    /// <returns>The amount of bytes written</returns>
    public Boolean TrySerialize<TAny>([DisallowNull] Stream stream,
                                      [AllowNull] TAny? graph,
                                      [DisallowNull] Action<TAny?, ISerializationInfoAdder> getSerializationData,
                                      in Int64 offset);
    /// <summary>
    /// Serializes the specified graph into the specified stream starting at the specified offset in the stream.
    /// </summary>
    /// <param name="stream">The stream to serialize the graph into.</param>
    /// <param name="graph">The graph to serialize.</param>
    /// <param name="written">The amount of bytes written.</param>
    /// <param name="getSerializationData">The <see langword="delegate"/> which specifies the members to serialize.</param>
    /// <returns>The amount of bytes written</returns>
    public Boolean TrySerialize<TAny>([DisallowNull] Stream stream,
                                      [AllowNull] TAny? graph,
                                      [DisallowNull] Action<TAny?, ISerializationInfoAdder> getSerializationData,
                                      out UInt64 written);
    /// <summary>
    /// Serializes the specified graph into the specified stream starting at the specified offset in the stream.
    /// </summary>
    /// <param name="stream">The stream to serialize the graph into.</param>
    /// <param name="graph">The graph to serialize.</param>
    /// <param name="offset">The offset in the stream where to begin writing.</param>
    /// <param name="written">The amount of bytes written.</param>
    /// <param name="getSerializationData">The <see langword="delegate"/> which specifies the members to serialize.</param>
    /// <returns>The amount of bytes written</returns>
    public Boolean TrySerialize<TAny>([DisallowNull] Stream stream,
                                      [AllowNull] TAny graph,
                                      [DisallowNull] Action<TAny?, ISerializationInfoAdder> getSerializationData,
                                      in Int64 offset,
                                      out UInt64 written);
    /// <summary>
    /// Serializes the specified graph into the specified stream starting at the specified offset in the stream.
    /// </summary>
    /// <param name="stream">The stream to serialize the graph into.</param>
    /// <param name="graph">The graph to serialize.</param>
    /// <param name="getSerializationData">The <see langword="delegate"/> which specifies the members to serialize.</param>
    /// <param name="actionAfter">The actions to perform after the writing operation has finished.</param>
    /// <returns>The amount of bytes written</returns>
    public Boolean TrySerialize<TAny>([DisallowNull] Stream stream,
                                      [AllowNull] TAny? graph,
                                      [DisallowNull] Action<TAny?, ISerializationInfoAdder> getSerializationData,
                                      in SerializationFinishAction actionAfter);
    /// <summary>
    /// Serializes the specified graph into the specified stream starting at the specified offset in the stream.
    /// </summary>
    /// <param name="stream">The stream to serialize the graph into.</param>
    /// <param name="graph">The graph to serialize.</param>
    /// <param name="offset">The offset in the stream where to begin writing.</param>
    /// <param name="getSerializationData">The <see langword="delegate"/> which specifies the members to serialize.</param>
    /// <param name="actionAfter">The actions to perform after the writing operation has finished.</param>
    /// <returns>The amount of bytes written</returns>
    public Boolean TrySerialize<TAny>([DisallowNull] Stream stream,
                                      [AllowNull] TAny? graph,
                                      [DisallowNull] Action<TAny?, ISerializationInfoAdder> getSerializationData,
                                      in Int64 offset,
                                      in SerializationFinishAction actionAfter);
    /// <summary>
    /// Serializes the specified graph into the specified stream starting at the specified offset in the stream.
    /// </summary>
    /// <param name="stream">The stream to serialize the graph into.</param>
    /// <param name="graph">The graph to serialize.</param>
    /// <param name="written">The amount of bytes written.</param>
    /// <param name="getSerializationData">The <see langword="delegate"/> which specifies the members to serialize.</param>
    /// <param name="actionAfter">The actions to perform after the writing operation has finished.</param>
    /// <returns>The amount of bytes written</returns>
    public Boolean TrySerialize<TAny>([DisallowNull] Stream stream,
                                      [AllowNull] TAny? graph,
                                      [DisallowNull] Action<TAny?, ISerializationInfoAdder> getSerializationData,
                                      out UInt64 written,
                                      in SerializationFinishAction actionAfter);
    /// <summary>
    /// Serializes the specified graph into the specified stream starting at the specified offset in the stream.
    /// </summary>
    /// <param name="stream">The stream to serialize the graph into.</param>
    /// <param name="graph">The graph to serialize.</param>
    /// <param name="offset">The offset in the stream where to begin writing.</param>
    /// <param name="written">The amount of bytes written.</param>
    /// <param name="getSerializationData">The <see langword="delegate"/> which specifies the members to serialize.</param>
    /// <param name="actionAfter">The actions to perform after the writing operation has finished.</param>
    /// <returns>The amount of bytes written</returns>
    public Boolean TrySerialize<TAny>([DisallowNull] Stream stream,
                                      [AllowNull] TAny? graph,
                                      [DisallowNull] Action<TAny?, ISerializationInfoAdder> getSerializationData,
                                      in Int64 offset,
                                      out UInt64 written,
                                      in SerializationFinishAction actionAfter);

    /// <summary>
    /// Deserializes the specified stream starting at the specified offset into an instance of type <typeparamref name="TAny"/>.
    /// </summary>
    /// <param name="stream">The stream to deserialize the graph from.</param>
    /// <param name="constructFromSerializationData">The <see langword="delegate"/> which specifies how to deserialize the state into an object.</param>
    /// <returns>The instance represented by the bytes in the specified stream</returns>
    [return: MaybeNull]
    public TAny? Deserialize<TAny>([DisallowNull] Stream stream,
                                   [DisallowNull] Func<ISerializationInfoGetter, TAny?> constructFromSerializationData);
    /// <summary>
    /// Deserializes the specified stream starting at the specified offset into an instance of type <typeparamref name="TAny"/>.
    /// </summary>
    /// <param name="stream">The stream to deserialize the graph from.</param>
    /// <param name="constructFromSerializationData">The <see langword="delegate"/> which specifies how to deserialize the state into an object.</param>
    /// <param name="offset">The offset in the stream where to begin reading.</param>
    /// <returns>The instance represented by the bytes in the specified stream</returns>
    [return: MaybeNull]
    public TAny? Deserialize<TAny>([DisallowNull] Stream stream,
                                   [DisallowNull] Func<ISerializationInfoGetter, TAny?> constructFromSerializationData,
                                   in Int64 offset);
    /// <summary>
    /// Deserializes the specified stream starting at the specified offset into an instance of type <typeparamref name="TAny"/>.
    /// </summary>
    /// <param name="stream">The stream to deserialize the graph from.</param>
    /// <param name="constructFromSerializationData">The <see langword="delegate"/> which specifies how to deserialize the state into an object.</param>
    /// <param name="read">The amount of elements read from the <paramref name="stream"/> parameter.</param>
    /// <returns>The instance represented by the bytes in the specified stream</returns>
    [return: MaybeNull]
    public TAny? Deserialize<TAny>([DisallowNull] Stream stream,
                                   [DisallowNull] Func<ISerializationInfoGetter, TAny?> constructFromSerializationData,
                                   out UInt64 read);
    /// <summary>
    /// Deserializes the specified stream starting at the specified offset into an instance of type <typeparamref name="TAny"/>.
    /// </summary>
    /// <param name="stream">The stream to deserialize the graph from.</param>
    /// <param name="constructFromSerializationData">The <see langword="delegate"/> which specifies how to deserialize the state into an object.</param>
    /// <param name="offset">The offset in the stream where to begin reading.</param>
    /// <param name="read">The amount of elements read from the <paramref name="stream"/> parameter.</param>
    /// <returns>The instance represented by the bytes in the specified stream</returns>
    [return: MaybeNull]
    public TAny? Deserialize<TAny>([DisallowNull] Stream stream,
                                   [DisallowNull] Func<ISerializationInfoGetter, TAny?> constructFromSerializationData,
                                   in Int64 offset,
                                   out UInt64 read);
    /// <summary>
    /// Deserializes the specified stream starting at the specified offset into an instance of type <typeparamref name="TAny"/>.
    /// </summary>
    /// <param name="stream">The stream to deserialize the graph from.</param>
    /// <param name="constructFromSerializationData">The <see langword="delegate"/> which specifies how to deserialize the state into an object.</param>
    /// <param name="actionAfter">The actions to perform after the reading operation has finished.</param>
    /// <returns>The instance represented by the bytes in the specified stream</returns>
    [return: MaybeNull]
    public TAny? Deserialize<TAny>([DisallowNull] Stream stream,
                                   [DisallowNull] Func<ISerializationInfoGetter, TAny?> constructFromSerializationData,
                                   in SerializationFinishAction actionAfter);
    /// <summary>
    /// Deserializes the specified stream starting at the specified offset into an instance of type <typeparamref name="TAny"/>.
    /// </summary>
    /// <param name="stream">The stream to deserialize the graph from.</param>
    /// <param name="constructFromSerializationData">The <see langword="delegate"/> which specifies how to deserialize the state into an object.</param>
    /// <param name="offset">The offset in the stream where to begin reading.</param>
    /// <param name="actionAfter">The actions to perform after the reading operation has finished.</param>
    /// <returns>The instance represented by the bytes in the specified stream</returns>
    [return: MaybeNull]
    public TAny? Deserialize<TAny>([DisallowNull] Stream stream,
                                   [DisallowNull] Func<ISerializationInfoGetter, TAny?> constructFromSerializationData,
                                   in Int64 offset,
                                   in SerializationFinishAction actionAfter);
    /// <summary>
    /// Deserializes the specified stream starting at the specified offset into an instance of type <typeparamref name="TAny"/>.
    /// </summary>
    /// <param name="stream">The stream to deserialize the graph from.</param>
    /// <param name="constructFromSerializationData">The <see langword="delegate"/> which specifies how to deserialize the state into an object.</param>
    /// <param name="read">The amount of elements read from the <paramref name="stream"/> parameter.</param>
    /// <param name="actionAfter">The actions to perform after the reading operation has finished.</param>
    /// <returns>The instance represented by the bytes in the specified stream</returns>
    [return: MaybeNull]
    public TAny? Deserialize<TAny>([DisallowNull] Stream stream,
                                   [DisallowNull] Func<ISerializationInfoGetter, TAny?> constructFromSerializationData,
                                   out UInt64 read,
                                   in SerializationFinishAction actionAfter);
    /// <summary>
    /// Deserializes the specified stream starting at the specified offset into an instance of type <typeparamref name="TAny"/>.
    /// </summary>
    /// <param name="stream">The stream to deserialize the graph from.</param>
    /// <param name="constructFromSerializationData">The <see langword="delegate"/> which specifies how to deserialize the state into an object.</param>
    /// <param name="offset">The offset in the stream where to begin reading.</param>
    /// <param name="read">The amount of elements read from the <paramref name="stream"/> parameter.</param>
    /// <param name="actionAfter">The actions to perform after the reading operation has finished.</param>
    /// <returns>The instance represented by the bytes in the specified stream</returns>
    [return: MaybeNull]
    public TAny? Deserialize<TAny>([DisallowNull] Stream stream,
                                   [DisallowNull] Func<ISerializationInfoGetter, TAny?> constructFromSerializationData,
                                   in Int64 offset,
                                   out UInt64 read,
                                   in SerializationFinishAction actionAfter);

    /// <summary>
    /// Tries to deserialize the specified stream starting at the specified offset into an instance of type <typeparamref name="TAny"/>.
    /// </summary>
    /// <param name="stream">The stream to deserialize the graph from.</param>
    /// <param name="constructFromSerializationData">The <see langword="delegate"/> which specifies how to deserialize the state into an object.</param>
    /// <param name="result">The instance represented by the bytes in the specified stream.</param>
    /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
    public Boolean TryDeserialize<TAny>([DisallowNull] Stream stream,
                                        [DisallowNull] Func<ISerializationInfoGetter, TAny?> constructFromSerializationData,
                                        [AllowNull] out TAny? result);
    /// <summary>
    /// Tries to deserialize the specified stream starting at the specified offset into an instance of type <typeparamref name="TAny"/>.
    /// </summary>
    /// <param name="stream">The stream to deserialize the graph from.</param>
    /// <param name="constructFromSerializationData">The <see langword="delegate"/> which specifies how to deserialize the state into an object.</param>
    /// <param name="offset">The offset in the stream where to begin reading.</param>
    /// <param name="result">The instance represented by the bytes in the specified stream.</param>
    /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
    public Boolean TryDeserialize<TAny>([DisallowNull] Stream stream,
                                        [DisallowNull] Func<ISerializationInfoGetter, TAny?> constructFromSerializationData,
                                        in Int64 offset,
                                        [AllowNull] out TAny? result);
    /// <summary>
    /// Tries to deserialize the specified stream starting at the specified offset into an instance of type <typeparamref name="TAny"/>.
    /// </summary>
    /// <param name="stream">The stream to deserialize the graph from.</param>
    /// <param name="constructFromSerializationData">The <see langword="delegate"/> which specifies how to deserialize the state into an object.</param>
    /// <param name="read">The amount of elements read from the <paramref name="stream"/> parameter.</param>
    /// <param name="result">The instance represented by the bytes in the specified stream.</param>
    /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
    public Boolean TryDeserialize<TAny>([DisallowNull] Stream stream,
                                        [DisallowNull] Func<ISerializationInfoGetter, TAny?> constructFromSerializationData,
                                        out UInt64 read,
                                        [AllowNull] out TAny? result);
    /// <summary>
    /// Tries to deserialize the specified stream starting at the specified offset into an instance of type <typeparamref name="TAny"/>.
    /// </summary>
    /// <param name="stream">The stream to deserialize the graph from.</param>
    /// <param name="constructFromSerializationData">The <see langword="delegate"/> which specifies how to deserialize the state into an object.</param>
    /// <param name="offset">The offset in the stream where to begin reading.</param>
    /// <param name="read">The amount of elements read from the <paramref name="stream"/> parameter.</param>
    /// <param name="result">The instance represented by the bytes in the specified stream.</param>
    /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
    public Boolean TryDeserialize<TAny>([DisallowNull] Stream stream,
                                        [DisallowNull] Func<ISerializationInfoGetter, TAny?> constructFromSerializationData,
                                        in Int64 offset,
                                        out UInt64 read,
                                        [AllowNull] out TAny? result);
    /// <summary>
    /// Tries to deserialize the specified stream starting at the specified offset into an instance of type <typeparamref name="TAny"/>.
    /// </summary>
    /// <param name="stream">The stream to deserialize the graph from.</param>
    /// <param name="constructFromSerializationData">The <see langword="delegate"/> which specifies how to deserialize the state into an object.</param>
    /// <param name="actionAfter">The actions to perform after the reading operation has finished.</param>
    /// <param name="result">The instance represented by the bytes in the specified stream.</param>
    /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
    public Boolean TryDeserialize<TAny>([DisallowNull] Stream stream,
                                        [DisallowNull] Func<ISerializationInfoGetter, TAny?> constructFromSerializationData,
                                        in SerializationFinishAction actionAfter,
                                        [AllowNull] out TAny? result);
    /// <summary>
    /// Tries to deserialize the specified stream starting at the specified offset into an instance of type <typeparamref name="TAny"/>.
    /// </summary>
    /// <param name="stream">The stream to deserialize the graph from.</param>
    /// <param name="constructFromSerializationData">The <see langword="delegate"/> which specifies how to deserialize the state into an object.</param>
    /// <param name="offset">The offset in the stream where to begin reading.</param>
    /// <param name="actionAfter">The actions to perform after the reading operation has finished.</param>
    /// <param name="result">The instance represented by the bytes in the specified stream.</param>
    /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
    public Boolean TryDeserialize<TAny>([DisallowNull] Stream stream,
                                        [DisallowNull] Func<ISerializationInfoGetter, TAny?> constructFromSerializationData,
                                        in Int64 offset,
                                        in SerializationFinishAction actionAfter,
                                        [AllowNull] out TAny? result);
    /// <summary>
    /// Tries to deserialize the specified stream starting at the specified offset into an instance of type <typeparamref name="TAny"/>.
    /// </summary>
    /// <param name="stream">The stream to deserialize the graph from.</param>
    /// <param name="constructFromSerializationData">The <see langword="delegate"/> which specifies how to deserialize the state into an object.</param>
    /// <param name="read">The amount of elements read from the <paramref name="stream"/> parameter.</param>
    /// <param name="actionAfter">The actions to perform after the reading operation has finished.</param>
    /// <param name="result">The instance represented by the bytes in the specified stream.</param>
    /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
    public Boolean TryDeserialize<TAny>([DisallowNull] Stream stream,
                                        [DisallowNull] Func<ISerializationInfoGetter, TAny?> constructFromSerializationData,
                                        out UInt64 read,
                                        in SerializationFinishAction actionAfter,
                                        [AllowNull] out TAny? result);
    /// <summary>
    /// Tries to deserialize the specified stream starting at the specified offset into an instance of type <typeparamref name="TAny"/>.
    /// </summary>
    /// <param name="stream">The stream to deserialize the graph from.</param>
    /// <param name="constructFromSerializationData">The <see langword="delegate"/> which specifies how to deserialize the state into an object.</param>
    /// <param name="offset">The offset in the stream where to begin reading.</param>
    /// <param name="read">The amount of elements read from the <paramref name="stream"/> parameter.</param>
    /// <param name="actionAfter">The actions to perform after the reading operation has finished.</param>
    /// <param name="result">The instance represented by the bytes in the specified stream.</param>
    /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
    public Boolean TryDeserialize<TAny>([DisallowNull] Stream stream,
                                        [DisallowNull] Func<ISerializationInfoGetter, TAny?> constructFromSerializationData,
                                        in Int64 offset,
                                        out UInt64 read,
                                        in SerializationFinishAction actionAfter,
                                        [AllowNull] out TAny? result);
}
namespace Narumikazuchi.Serialization;

/// <summary>
/// Represents a deserializer for the specified type <typeparamref name="TSerializable"/>.
/// </summary>
public interface IDeserializer<TSerializable> :
    IUsesSerializationStrategies
{
    /// <summary>
    /// Deserializes the specified stream starting at the specified offset into an instance of type <typeparamref name="TSerializable"/>.
    /// </summary>
    /// <param name="stream">The stream to deserialize the graph from.</param>
    /// <returns>The instance represented by the bytes in the specified stream</returns>
    [return: MaybeNull]
    public TSerializable? Deserialize([DisallowNull] Stream stream);
    /// <summary>
    /// Deserializes the specified stream starting at the specified offset into an instance of type <typeparamref name="TSerializable"/>.
    /// </summary>
    /// <param name="stream">The stream to deserialize the graph from.</param>
    /// <param name="offset">The offset in the stream where to begin reading.</param>
    /// <returns>The instance represented by the bytes in the specified stream</returns>
    [return: MaybeNull]
    public TSerializable? Deserialize([DisallowNull] Stream stream,
                                      in Int64 offset);
    /// <summary>
    /// Deserializes the specified stream starting at the specified offset into an instance of type <typeparamref name="TSerializable"/>.
    /// </summary>
    /// <param name="stream">The stream to deserialize the graph from.</param>
    /// <param name="read">The amount of elements read from the <paramref name="stream"/> parameter.</param>
    /// <returns>The instance represented by the bytes in the specified stream</returns>
    [return: MaybeNull]
    public TSerializable? Deserialize([DisallowNull] Stream stream,
                                      out UInt64 read);
    /// <summary>
    /// Deserializes the specified stream starting at the specified offset into an instance of type <typeparamref name="TSerializable"/>.
    /// </summary>
    /// <param name="stream">The stream to deserialize the graph from.</param>
    /// <param name="offset">The offset in the stream where to begin reading.</param>
    /// <param name="read">The amount of elements read from the <paramref name="stream"/> parameter.</param>
    /// <returns>The instance represented by the bytes in the specified stream</returns>
    [return: MaybeNull]
    public TSerializable? Deserialize([DisallowNull] Stream stream,
                                      in Int64 offset,
                                      out UInt64 read);
    /// <summary>
    /// Deserializes the specified stream starting at the specified offset into an instance of type <typeparamref name="TSerializable"/>.
    /// </summary>
    /// <param name="stream">The stream to deserialize the graph from.</param>
    /// <param name="actionAfter">The actions to perform after the reading operation has finished.</param>
    /// <returns>The instance represented by the bytes in the specified stream</returns>
    [return: MaybeNull]
    public TSerializable? Deserialize([DisallowNull] Stream stream,
                                      in SerializationFinishAction actionAfter);
    /// <summary>
    /// Deserializes the specified stream starting at the specified offset into an instance of type <typeparamref name="TSerializable"/>.
    /// </summary>
    /// <param name="stream">The stream to deserialize the graph from.</param>
    /// <param name="offset">The offset in the stream where to begin reading.</param>
    /// <param name="actionAfter">The actions to perform after the reading operation has finished.</param>
    /// <returns>The instance represented by the bytes in the specified stream</returns>
    [return: MaybeNull]
    public TSerializable? Deserialize([DisallowNull] Stream stream,
                                      in Int64 offset,
                                      in SerializationFinishAction actionAfter);
    /// <summary>
    /// Deserializes the specified stream starting at the specified offset into an instance of type <typeparamref name="TSerializable"/>.
    /// </summary>
    /// <param name="stream">The stream to deserialize the graph from.</param>
    /// <param name="read">The amount of elements read from the <paramref name="stream"/> parameter.</param>
    /// <param name="actionAfter">The actions to perform after the reading operation has finished.</param>
    /// <returns>The instance represented by the bytes in the specified stream</returns>
    [return: MaybeNull]
    public TSerializable? Deserialize([DisallowNull] Stream stream,
                                      out UInt64 read,
                                      in SerializationFinishAction actionAfter);
    /// <summary>
    /// Deserializes the specified stream starting at the specified offset into an instance of type <typeparamref name="TSerializable"/>.
    /// </summary>
    /// <param name="stream">The stream to deserialize the graph from.</param>
    /// <param name="offset">The offset in the stream where to begin reading.</param>
    /// <param name="read">The amount of elements read from the <paramref name="stream"/> parameter.</param>
    /// <param name="actionAfter">The actions to perform after the reading operation has finished.</param>
    /// <returns>The instance represented by the bytes in the specified stream</returns>
    [return: MaybeNull]
    public TSerializable? Deserialize([DisallowNull] Stream stream,
                                      in Int64 offset,
                                      out UInt64 read,
                                      in SerializationFinishAction actionAfter);

    /// <summary>
    /// Tries to deserialize the specified stream starting at the specified offset into an instance of type <typeparamref name="TSerializable"/>.
    /// </summary>
    /// <param name="stream">The stream to deserialize the graph from.</param>
    /// <param name="result">The instance represented by the bytes in the specified stream.</param>
    /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
    public Boolean TryDeserialize([DisallowNull] Stream stream,
                                  [AllowNull] out TSerializable? result);
    /// <summary>
    /// Tries to deserialize the specified stream starting at the specified offset into an instance of type <typeparamref name="TSerializable"/>.
    /// </summary>
    /// <param name="stream">The stream to deserialize the graph from.</param>
    /// <param name="offset">The offset in the stream where to begin reading.</param>
    /// <param name="result">The instance represented by the bytes in the specified stream.</param>
    /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
    public Boolean TryDeserialize([DisallowNull] Stream stream,
                                  in Int64 offset,
                                  [AllowNull] out TSerializable? result);
    /// <summary>
    /// Tries to deserialize the specified stream starting at the specified offset into an instance of type <typeparamref name="TSerializable"/>.
    /// </summary>
    /// <param name="stream">The stream to deserialize the graph from.</param>
    /// <param name="read">The amount of elements read from the <paramref name="stream"/> parameter.</param>
    /// <param name="result">The instance represented by the bytes in the specified stream.</param>
    /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
    public Boolean TryDeserialize([DisallowNull] Stream stream,
                                  out UInt64 read,
                                  [AllowNull] out TSerializable? result);
    /// <summary>
    /// Tries to deserialize the specified stream starting at the specified offset into an instance of type <typeparamref name="TSerializable"/>.
    /// </summary>
    /// <param name="stream">The stream to deserialize the graph from.</param>
    /// <param name="offset">The offset in the stream where to begin reading.</param>
    /// <param name="read">The amount of elements read from the <paramref name="stream"/> parameter.</param>
    /// <param name="result">The instance represented by the bytes in the specified stream.</param>
    /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
    public Boolean TryDeserialize([DisallowNull] Stream stream,
                                  in Int64 offset,
                                  out UInt64 read,
                                  [AllowNull] out TSerializable? result);
    /// <summary>
    /// Tries to deserialize the specified stream starting at the specified offset into an instance of type <typeparamref name="TSerializable"/>.
    /// </summary>
    /// <param name="stream">The stream to deserialize the graph from.</param>
    /// <param name="actionAfter">The actions to perform after the reading operation has finished.</param>
    /// <param name="result">The instance represented by the bytes in the specified stream.</param>
    /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
    public Boolean TryDeserialize([DisallowNull] Stream stream,
                                  in SerializationFinishAction actionAfter,
                                  [AllowNull] out TSerializable? result);
    /// <summary>
    /// Tries to deserialize the specified stream starting at the specified offset into an instance of type <typeparamref name="TSerializable"/>.
    /// </summary>
    /// <param name="stream">The stream to deserialize the graph from.</param>
    /// <param name="offset">The offset in the stream where to begin reading.</param>
    /// <param name="actionAfter">The actions to perform after the reading operation has finished.</param>
    /// <param name="result">The instance represented by the bytes in the specified stream.</param>
    /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
    public Boolean TryDeserialize([DisallowNull] Stream stream,
                                  in Int64 offset,
                                  in SerializationFinishAction actionAfter,
                                  [AllowNull] out TSerializable? result);
    /// <summary>
    /// Tries to deserialize the specified stream starting at the specified offset into an instance of type <typeparamref name="TSerializable"/>.
    /// </summary>
    /// <param name="stream">The stream to deserialize the graph from.</param>
    /// <param name="read">The amount of elements read from the <paramref name="stream"/> parameter.</param>
    /// <param name="actionAfter">The actions to perform after the reading operation has finished.</param>
    /// <param name="result">The instance represented by the bytes in the specified stream.</param>
    /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
    public Boolean TryDeserialize([DisallowNull] Stream stream,
                                  out UInt64 read,
                                  in SerializationFinishAction actionAfter,
                                  [AllowNull] out TSerializable? result);
    /// <summary>
    /// Tries to deserialize the specified stream starting at the specified offset into an instance of type <typeparamref name="TSerializable"/>.
    /// </summary>
    /// <param name="stream">The stream to deserialize the graph from.</param>
    /// <param name="offset">The offset in the stream where to begin reading.</param>
    /// <param name="read">The amount of elements read from the <paramref name="stream"/> parameter.</param>
    /// <param name="actionAfter">The actions to perform after the reading operation has finished.</param>
    /// <param name="result">The instance represented by the bytes in the specified stream.</param>
    /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
    public Boolean TryDeserialize([DisallowNull] Stream stream,
                                  in Int64 offset,
                                  out UInt64 read,
                                  in SerializationFinishAction actionAfter,
                                  [AllowNull] out TSerializable? result);
}

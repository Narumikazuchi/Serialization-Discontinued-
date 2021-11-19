namespace Narumikazuchi.Serialization.Bytes;

/// <summary>
/// A serializer for classes that implement <see cref="ISerializable{TSelf}"/>.
/// </summary>
public sealed partial class ByteSerializer<TSerializable> : SharedByteSerializer
    where TSerializable : ISerializable<TSerializable>
{
    /// <summary>
    /// Instantiates a new instance of the <see cref="ByteSerializer{TSerializable}"/> class.
    /// </summary>
    /// <exception cref="InvalidOperationException"/>
    public ByteSerializer() :
        base()
    { }
    /// <summary>
    /// Instantiates a new instance of the <see cref="ByteSerializer{TSerializable}"/> class.
    /// </summary>
    /// <exception cref="InvalidOperationException"/>
    public ByteSerializer([DisallowNull] IReadOnlyDictionary<Type, ISerializationStrategy<Byte[]>> strategies) :
        base(strategies)
    { }
}

// IBothWaySerializer<TSerializable>
partial class ByteSerializer<TSerializable> : IBothWaySerializer<TSerializable>
{
    /// <summary>
    /// Serializes the specified graph into the specified stream.
    /// </summary>
    /// <param name="stream">The stream to serialize the graph into.</param>
    /// <param name="graph">The graph to serialize.</param>
    /// <returns>The amount of bytes written</returns>
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="InvalidOperationException"/>
    public UInt64 Serialize([DisallowNull] Stream stream, 
                            [DisallowNull] TSerializable graph) =>
        this.Serialize(stream, 
                       graph, 
                       -1, 
                       SerializationFinishAction.None);
    /// <summary>
    /// Serializes the specified graph into the specified stream starting at the specified offset in the stream.
    /// </summary>
    /// <param name="stream">The stream to serialize the graph into.</param>
    /// <param name="graph">The graph to serialize.</param>
    /// <param name="offset">The offset in the stream where to begin writing.</param>
    /// <returns>The amount of bytes written</returns>
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="InvalidOperationException"/>
    public UInt64 Serialize([DisallowNull] Stream stream, 
                            [DisallowNull] TSerializable graph, 
                            in Int64 offset) =>
        this.Serialize(stream, 
                       graph, 
                       offset, 
                       SerializationFinishAction.None);
    /// <summary>
    /// Serializes the specified graph into the specified stream.
    /// </summary>
    /// <param name="stream">The stream to serialize the graph into.</param>
    /// <param name="graph">The graph to serialize.</param>
    /// <param name="actionAfter">The actions to perform after the writing operation has finished.</param>
    /// <returns>The amount of bytes written</returns>
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="InvalidOperationException"/>
    public UInt64 Serialize([DisallowNull] Stream stream, 
                            [DisallowNull] TSerializable graph, 
                            in SerializationFinishAction actionAfter) =>
        this.Serialize(stream, 
                       graph, 
                       -1, 
                       actionAfter);
    /// <summary>
    /// Serializes the specified graph into the specified stream starting at the specified offset in the stream.
    /// </summary>
    /// <param name="stream">The stream to serialize the graph into.</param>
    /// <param name="graph">The graph to serialize.</param>
    /// <param name="offset">The offset in the stream where to begin writing.</param>
    /// <param name="actionAfter">The actions to perform after the writing operation has finished.</param>
    /// <returns>The amount of bytes written</returns>
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="InvalidOperationException"/>
    public UInt64 Serialize([DisallowNull] Stream stream,
                            [DisallowNull] TSerializable graph,
                            in Int64 offset,
                            in SerializationFinishAction actionAfter) =>
        this.Serialize(stream,
                       (ISerializable)graph,
                       offset,
                       actionAfter);

    /// <summary>
    /// Tries to serialize the specified graph into the specified stream.
    /// </summary>
    /// <param name="stream">The stream to serialize the graph into.</param>
    /// <param name="graph">The graph to serialize.</param>
    /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
    public Boolean TrySerialize([DisallowNull] Stream stream, 
                                [DisallowNull] TSerializable graph) =>
        this.TrySerialize(stream, 
                          graph, 
                          -1, 
                          out UInt64 _,
                          SerializationFinishAction.None);
    /// <summary>
    /// Tries to serialize the specified graph into the specified stream starting at the specified offset in the stream.
    /// </summary>
    /// <param name="stream">The stream to serialize the graph into.</param>
    /// <param name="graph">The graph to serialize.</param>
    /// <param name="offset">The offset in the stream where to begin writing.</param>
    /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
    public Boolean TrySerialize([DisallowNull] Stream stream, 
                                [DisallowNull] TSerializable graph, 
                                in Int64 offset) =>
        this.TrySerialize(stream, 
                          graph, 
                          offset,
                          out UInt64 _,
                          SerializationFinishAction.None);
    /// <summary>
    /// Tries to serialize the specified graph into the specified stream starting at the specified offset in the stream.
    /// </summary>
    /// <param name="stream">The stream to serialize the graph into.</param>
    /// <param name="graph">The graph to serialize.</param>
    /// <param name="written">The amount of bytes written.</param>
    /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
    public Boolean TrySerialize([DisallowNull] Stream stream,
                                [DisallowNull] TSerializable graph,
                                out UInt64 written) =>
        this.TrySerialize(stream,
                          graph,
                          -1,
                          out written,
                          SerializationFinishAction.None);
    /// <summary>
    /// Tries to serialize the specified graph into the specified stream starting at the specified offset in the stream.
    /// </summary>
    /// <param name="stream">The stream to serialize the graph into.</param>
    /// <param name="graph">The graph to serialize.</param>
    /// <param name="offset">The offset in the stream where to begin writing.</param>
    /// <param name="written">The amount of bytes written.</param>
    /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
    public Boolean TrySerialize([DisallowNull] Stream stream,
                                [DisallowNull] TSerializable graph,
                                in Int64 offset,
                                out UInt64 written) =>
        this.TrySerialize(stream,
                          graph,
                          offset,
                          out written,
                          SerializationFinishAction.None);
    /// <summary>
    /// Tries to serialize the specified graph into the specified stream.
    /// </summary>
    /// <param name="stream">The stream to serialize the graph into.</param>
    /// <param name="graph">The graph to serialize.</param>
    /// <param name="actionAfter">The actions to perform after the writing operation has finished.</param>
    /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
    public Boolean TrySerialize([DisallowNull] Stream stream, 
                                [DisallowNull] TSerializable graph, 
                                in SerializationFinishAction actionAfter) =>
        this.TrySerialize(stream, 
                          graph, 
                          -1,
                          out UInt64 _,
                          actionAfter);
    /// <summary>
    /// Tries to serialize the specified graph into the specified stream.
    /// </summary>
    /// <param name="stream">The stream to serialize the graph into.</param>
    /// <param name="graph">The graph to serialize.</param>
    /// <param name="offset">The offset in the stream where to begin writing.</param>
    /// <param name="actionAfter">The actions to perform after the writing operation has finished.</param>
    /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
    public Boolean TrySerialize([DisallowNull] Stream stream,
                                [DisallowNull] TSerializable graph,
                                in Int64 offset,
                                in SerializationFinishAction actionAfter) =>
        this.TrySerialize(stream,
                          graph,
                          offset,
                          out UInt64 _,
                          actionAfter);
    /// <summary>
    /// Tries to serialize the specified graph into the specified stream.
    /// </summary>
    /// <param name="stream">The stream to serialize the graph into.</param>
    /// <param name="graph">The graph to serialize.</param>
    /// <param name="written">The amount of bytes written.</param>
    /// <param name="actionAfter">The actions to perform after the writing operation has finished.</param>
    /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
    public Boolean TrySerialize([DisallowNull] Stream stream,
                                [DisallowNull] TSerializable graph,
                                out UInt64 written,
                                in SerializationFinishAction actionAfter) =>
        this.TrySerialize(stream,
                          graph,
                          -1,
                          out written,
                          actionAfter);
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
                                [DisallowNull] TSerializable graph,
                                in Int64 offset,
                                out UInt64 written,
                                in SerializationFinishAction actionAfter) =>
        this.TrySerialize(stream,
                          (ISerializable)graph,
                          offset,
                          out written,
                          actionAfter);

    /// <summary>
    /// Deserializes the specified stream into an instance of type <typeparamref name="TSerializable"/>.
    /// </summary>
    /// <param name="stream">The stream to deserialize the graph from.</param>
    /// <returns>The instance represented by the bytes in the specified stream</returns>
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="InvalidOperationException"/>
    [return: NotNull]
    public TSerializable Deserialize([DisallowNull] Stream stream) =>
        this.Deserialize(stream, 
                         -1,
                         out UInt64 _,
                         SerializationFinishAction.None);
    /// <summary>
    /// Deserializes the specified stream starting at the specified offset into an instance of type <typeparamref name="TSerializable"/>.
    /// </summary>
    /// <param name="stream">The stream to deserialize the graph from.</param>
    /// <param name="offset">The offset in the stream where to begin reading.</param>
    /// <returns>The instance represented by the bytes in the specified stream</returns>
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="InvalidOperationException"/>
    [return: NotNull]
    public TSerializable Deserialize([DisallowNull] Stream stream,
                                     in Int64 offset) =>
        this.Deserialize(stream,
                         offset,
                         out UInt64 _,
                         SerializationFinishAction.None);
    /// <summary>
    /// Deserializes the specified stream starting at the specified offset into an instance of type <typeparamref name="TSerializable"/>.
    /// </summary>
    /// <param name="stream">The stream to deserialize the graph from.</param>
    /// <param name="read">The amount of elements read from the <paramref name="stream"/> parameter.</param>
    /// <returns>The instance represented by the bytes in the specified stream</returns>
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="InvalidOperationException"/>
    [return: NotNull]
    public TSerializable Deserialize([DisallowNull] Stream stream,
                                     out UInt64 read) =>
        this.Deserialize(stream,
                         -1,
                         out read,
                         SerializationFinishAction.None);
    /// <summary>
    /// Deserializes the specified stream starting at the specified offset into an instance of type <typeparamref name="TSerializable"/>.
    /// </summary>
    /// <param name="stream">The stream to deserialize the graph from.</param>
    /// <param name="offset">The offset in the stream where to begin reading.</param>
    /// <param name="read">The amount of elements read from the <paramref name="stream"/> parameter.</param>
    /// <returns>The instance represented by the bytes in the specified stream</returns>
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="InvalidOperationException"/>
    [return: NotNull]
    public TSerializable Deserialize([DisallowNull] Stream stream, 
                                     in Int64 offset,
                                     out UInt64 read) =>
        this.Deserialize(stream, 
                         offset,
                         out read,
                         SerializationFinishAction.None);
    /// <summary>
    /// Deserializes the specified stream into an instance of type <typeparamref name="TSerializable"/>.
    /// </summary>
    /// <param name="stream">The stream to deserialize the graph from.</param>
    /// <param name="actionAfter">The actions to perform after the reading operation has finished.</param>
    /// <returns>The instance represented by the bytes in the specified stream</returns>
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="InvalidOperationException"/>
    [return: NotNull]
    public TSerializable Deserialize([DisallowNull] Stream stream, 
                                     in SerializationFinishAction actionAfter) =>
        this.Deserialize(stream, 
                         -1,
                         out UInt64 _,
                         actionAfter);
    /// <summary>
    /// Deserializes the specified stream starting at the specified offset into an instance of type <typeparamref name="TSerializable"/>.
    /// </summary>
    /// <param name="stream">The stream to deserialize the graph from.</param>
    /// <param name="offset">The offset in the stream where to begin reading.</param>
    /// <param name="actionAfter">The actions to perform after the reading operation has finished.</param>
    /// <returns>The instance represented by the bytes in the specified stream</returns>
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="InvalidOperationException"/>
    [return: NotNull]
    public TSerializable Deserialize([DisallowNull] Stream stream,
                                     in Int64 offset,
                                     in SerializationFinishAction actionAfter) =>
        this.Deserialize(stream,
                         offset,
                         out UInt64 _,
                         actionAfter);
    /// <summary>
    /// Deserializes the specified stream starting at the specified offset into an instance of type <typeparamref name="TSerializable"/>.
    /// </summary>
    /// <param name="stream">The stream to deserialize the graph from.</param>
    /// <param name="read">The amount of elements read from the <paramref name="stream"/> parameter.</param>
    /// <param name="actionAfter">The actions to perform after the reading operation has finished.</param>
    /// <returns>The instance represented by the bytes in the specified stream</returns>
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="InvalidOperationException"/>
    [return: NotNull]
    public TSerializable Deserialize([DisallowNull] Stream stream,
                                     out UInt64 read, 
                                     in SerializationFinishAction actionAfter) =>
        this.Deserialize(stream, 
                         -1,
                         out read,
                         actionAfter);
    /// <summary>
    /// Deserializes the specified stream starting at the specified offset into an instance of type <typeparamref name="TSerializable"/>.
    /// </summary>
    /// <param name="stream">The stream to deserialize the graph from.</param>
    /// <param name="offset">The offset in the stream where to begin reading.</param>
    /// <param name="read">The amount of elements read from the <paramref name="stream"/> parameter.</param>
    /// <param name="actionAfter">The actions to perform after the reading operation has finished.</param>
    /// <returns>The instance represented by the bytes in the specified stream</returns>
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="InvalidOperationException"/>
    [return: NotNull]
    public TSerializable Deserialize([DisallowNull] Stream stream, 
                                     in Int64 offset, 
                                     out UInt64 read,
                                     in SerializationFinishAction actionAfter)
    {
        if (stream is null)
        {
            throw new ArgumentNullException(nameof(stream));
        }
        if (!stream.CanRead)
        {
            throw new InvalidOperationException(STREAM_DOES_NOT_SUPPORT_READING);
        }
        if (offset > -1 &&
            !stream.CanSeek)
        {
            throw new InvalidOperationException(STREAM_DOES_NOT_SUPPORT_SEEKING);
        }

        if (offset > -1)
        {
            stream.Seek(offset, 
                        SeekOrigin.Begin);
        }

        SerializationInfo info = this.DeserializeInternal(stream,
                                                          out read);
        TSerializable result = TSerializable.ConstructFromSerializationData(info);

        if (actionAfter.HasFlag(SerializationFinishAction.FlushStream))
        {
            stream.Flush();
        }
        if (actionAfter.HasFlag(SerializationFinishAction.CloseStream))
        {
            stream.Close();
        }
        return result;
    }

    /// <summary>
    /// Tries to deserialize the specified stream starting at the specified offset into an instance of type <typeparamref name="TSerializable"/>.
    /// </summary>
    /// <param name="stream">The stream to deserialize the graph from.</param>
    /// <param name="result">The instance represented by the bytes in the specified stream.</param>
    /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
    public Boolean TryDeserialize([DisallowNull] Stream stream,
                                  [NotNullWhen(true)] out TSerializable? result) =>
        this.TryDeserialize(stream,
                            -1,
                            out UInt64 _,
                            SerializationFinishAction.None,
                            out result);
    /// <summary>
    /// Tries to deserialize the specified stream starting at the specified offset into an instance of type <typeparamref name="TSerializable"/>.
    /// </summary>
    /// <param name="stream">The stream to deserialize the graph from.</param>
    /// <param name="offset">The offset in the stream where to begin reading.</param>
    /// <param name="result">The instance represented by the bytes in the specified stream.</param>
    /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
    public Boolean TryDeserialize([DisallowNull] Stream stream,
                                  in Int64 offset,
                                  [NotNullWhen(true)] out TSerializable? result) =>
        this.TryDeserialize(stream,
                            offset,
                            out UInt64 _,
                            SerializationFinishAction.None,
                            out result);
    /// <summary>
    /// Tries to deserialize the specified stream starting at the specified offset into an instance of type <typeparamref name="TSerializable"/>.
    /// </summary>
    /// <param name="stream">The stream to deserialize the graph from.</param>
    /// <param name="read">The amount of elements read from the <paramref name="stream"/> parameter.</param>
    /// <param name="result">The instance represented by the bytes in the specified stream.</param>
    /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
    public Boolean TryDeserialize([DisallowNull] Stream stream,
                                  out UInt64 read,
                                  [NotNullWhen(true)] out TSerializable? result) =>
        this.TryDeserialize(stream,
                            -1,
                            out read,
                            SerializationFinishAction.None,
                            out result);
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
                                  [NotNullWhen(true)] out TSerializable? result) =>
        this.TryDeserialize(stream,
                            offset,
                            out read,
                            SerializationFinishAction.None,
                            out result);
    /// <summary>
    /// Tries to deserialize the specified stream starting at the specified offset into an instance of type <typeparamref name="TSerializable"/>.
    /// </summary>
    /// <param name="stream">The stream to deserialize the graph from.</param>
    /// <param name="actionAfter">The actions to perform after the reading operation has finished.</param>
    /// <param name="result">The instance represented by the bytes in the specified stream.</param>
    /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
    public Boolean TryDeserialize([DisallowNull] Stream stream,
                                  in SerializationFinishAction actionAfter,
                                  [NotNullWhen(true)] out TSerializable? result) =>
        this.TryDeserialize(stream,
                            -1,
                            out UInt64 _,
                            actionAfter,
                            out result);
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
                                  [NotNullWhen(true)] out TSerializable? result) =>
        this.TryDeserialize(stream,
                            offset,
                            out UInt64 _,
                            actionAfter,
                            out result);
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
                                  [NotNullWhen(true)] out TSerializable? result) =>
        this.TryDeserialize(stream, 
                            -1,
                            out read,
                            actionAfter, 
                            out result);
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
                                  [NotNullWhen(true)] out TSerializable? result)
    {
        if (stream is null ||
            !stream.CanRead ||
            (offset > -1 &&
            !stream.CanSeek))
        {
            read = 0;
            result = default;
            return false;
        }

        if (offset > -1)
        {
            stream.Seek(offset,
                        SeekOrigin.Begin);
        }

        SerializationInfo info = this.DeserializeInternal(stream,
                                                          out read);
        result = TSerializable.ConstructFromSerializationData(info);

        if (actionAfter.HasFlag(SerializationFinishAction.FlushStream))
        {
            stream.Flush();
        }
        if (actionAfter.HasFlag(SerializationFinishAction.CloseStream))
        {
            stream.Close();
        }
        return true;
    }
}
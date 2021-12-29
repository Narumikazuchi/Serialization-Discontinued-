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
        base(__SerializationStrategies.Integrated)
    { }
    /// <summary>
    /// Instantiates a new instance of the <see cref="ByteSerializer{TSerializable}"/> class.
    /// </summary>
    /// <exception cref="InvalidOperationException"/>
    public ByteSerializer([DisallowNull] IEnumerable<KeyValuePair<Type, ISerializationStrategy<Byte[]>>> strategies) :
        base(strategies: strategies)
    { }
    /// <summary>
    /// Instantiates a new instance of the <see cref="ByteSerializer{TSerializable}"/> class.
    /// </summary>
    /// <exception cref="InvalidOperationException"/>
    public ByteSerializer([DisallowNull] IEnumerable<(Type, ISerializationStrategy<Byte[]>)> strategies) :
        base(strategies: strategies)
    { }
    /// <summary>
    /// Instantiates a new instance of the <see cref="ByteSerializer{TSerializable}"/> class.
    /// </summary>
    /// <exception cref="InvalidOperationException"/>
    public ByteSerializer([DisallowNull] IEnumerable<Tuple<Type, ISerializationStrategy<Byte[]>>> strategies) :
        base(strategies: strategies)
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
                            [AllowNull] TSerializable? graph) =>
        base.Serialize(stream: stream, 
                       graph: graph, 
                       offset: -1, 
                       actionAfter: SerializationFinishAction.None);
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
                            [AllowNull] TSerializable? graph, 
                            in Int64 offset) =>
        base.Serialize(stream: stream, 
                       graph: graph, 
                       offset: offset, 
                       actionAfter: SerializationFinishAction.None);
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
                            [AllowNull] TSerializable? graph, 
                            in SerializationFinishAction actionAfter) =>
        base.Serialize(stream: stream, 
                       graph: graph, 
                       offset: -1, 
                       actionAfter: actionAfter);
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
                            [AllowNull] TSerializable? graph,
                            in Int64 offset,
                            in SerializationFinishAction actionAfter) =>
        base.Serialize(stream: stream,
                       graph: graph,
                       offset: offset,
                       actionAfter: actionAfter);

    /// <summary>
    /// Tries to serialize the specified graph into the specified stream.
    /// </summary>
    /// <param name="stream">The stream to serialize the graph into.</param>
    /// <param name="graph">The graph to serialize.</param>
    /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
    public Boolean TrySerialize([DisallowNull] Stream stream, 
                                [AllowNull] TSerializable? graph) =>
        base.TrySerialize(stream: stream, 
                          graph: graph, 
                          offset: -1, 
                          written: out UInt64 _,
                          actionAfter: SerializationFinishAction.None);
    /// <summary>
    /// Tries to serialize the specified graph into the specified stream starting at the specified offset in the stream.
    /// </summary>
    /// <param name="stream">The stream to serialize the graph into.</param>
    /// <param name="graph">The graph to serialize.</param>
    /// <param name="offset">The offset in the stream where to begin writing.</param>
    /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
    public Boolean TrySerialize([DisallowNull] Stream stream, 
                                [AllowNull] TSerializable? graph, 
                                in Int64 offset) =>
        base.TrySerialize(stream: stream, 
                          graph: graph, 
                          offset: offset,
                          written: out UInt64 _,
                          actionAfter: SerializationFinishAction.None);
    /// <summary>
    /// Tries to serialize the specified graph into the specified stream starting at the specified offset in the stream.
    /// </summary>
    /// <param name="stream">The stream to serialize the graph into.</param>
    /// <param name="graph">The graph to serialize.</param>
    /// <param name="written">The amount of bytes written.</param>
    /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
    public Boolean TrySerialize([DisallowNull] Stream stream,
                                [AllowNull] TSerializable? graph,
                                out UInt64 written) =>
        base.TrySerialize(stream: stream,
                          graph: graph,
                          offset: -1,
                          written: out written,
                          actionAfter: SerializationFinishAction.None);
    /// <summary>
    /// Tries to serialize the specified graph into the specified stream starting at the specified offset in the stream.
    /// </summary>
    /// <param name="stream">The stream to serialize the graph into.</param>
    /// <param name="graph">The graph to serialize.</param>
    /// <param name="offset">The offset in the stream where to begin writing.</param>
    /// <param name="written">The amount of bytes written.</param>
    /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
    public Boolean TrySerialize([DisallowNull] Stream stream,
                                [AllowNull] TSerializable? graph,
                                in Int64 offset,
                                out UInt64 written) =>
        base.TrySerialize(stream: stream,
                          graph: graph,
                          offset: offset,
                          written: out written,
                          actionAfter: SerializationFinishAction.None);
    /// <summary>
    /// Tries to serialize the specified graph into the specified stream.
    /// </summary>
    /// <param name="stream">The stream to serialize the graph into.</param>
    /// <param name="graph">The graph to serialize.</param>
    /// <param name="actionAfter">The actions to perform after the writing operation has finished.</param>
    /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
    public Boolean TrySerialize([DisallowNull] Stream stream, 
                                [AllowNull] TSerializable? graph, 
                                in SerializationFinishAction actionAfter) =>
        base.TrySerialize(stream: stream, 
                          graph: graph, 
                          offset: -1,
                          written: out UInt64 _,
                          actionAfter: actionAfter);
    /// <summary>
    /// Tries to serialize the specified graph into the specified stream.
    /// </summary>
    /// <param name="stream">The stream to serialize the graph into.</param>
    /// <param name="graph">The graph to serialize.</param>
    /// <param name="offset">The offset in the stream where to begin writing.</param>
    /// <param name="actionAfter">The actions to perform after the writing operation has finished.</param>
    /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
    public Boolean TrySerialize([DisallowNull] Stream stream,
                                [AllowNull] TSerializable? graph,
                                in Int64 offset,
                                in SerializationFinishAction actionAfter) =>
        base.TrySerialize(stream: stream,
                          graph: graph,
                          offset: offset,
                          written: out UInt64 _,
                          actionAfter: actionAfter);
    /// <summary>
    /// Tries to serialize the specified graph into the specified stream.
    /// </summary>
    /// <param name="stream">The stream to serialize the graph into.</param>
    /// <param name="graph">The graph to serialize.</param>
    /// <param name="written">The amount of bytes written.</param>
    /// <param name="actionAfter">The actions to perform after the writing operation has finished.</param>
    /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
    public Boolean TrySerialize([DisallowNull] Stream stream,
                                [AllowNull] TSerializable? graph,
                                out UInt64 written,
                                in SerializationFinishAction actionAfter) =>
        base.TrySerialize(stream: stream,
                          graph: graph,
                          offset: -1,
                          written: out written,
                          actionAfter: actionAfter);
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
                                [AllowNull] TSerializable? graph,
                                in Int64 offset,
                                out UInt64 written,
                                in SerializationFinishAction actionAfter) =>
        base.TrySerialize(stream: stream,
                          graph: graph,
                          offset: offset,
                          written: out written,
                          actionAfter: actionAfter);

    /// <summary>
    /// Deserializes the specified stream into an instance of type <typeparamref name="TSerializable"/>.
    /// </summary>
    /// <param name="stream">The stream to deserialize the graph from.</param>
    /// <returns>The instance represented by the bytes in the specified stream</returns>
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="InvalidOperationException"/>
    [return: MaybeNull]
    public TSerializable? Deserialize([DisallowNull] Stream stream) =>
        this.Deserialize(stream: stream, 
                         offset: -1,
                         read: out UInt64 _,
                         actionAfter: SerializationFinishAction.None);
    /// <summary>
    /// Deserializes the specified stream starting at the specified offset into an instance of type <typeparamref name="TSerializable"/>.
    /// </summary>
    /// <param name="stream">The stream to deserialize the graph from.</param>
    /// <param name="offset">The offset in the stream where to begin reading.</param>
    /// <returns>The instance represented by the bytes in the specified stream</returns>
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="InvalidOperationException"/>
    [return: MaybeNull]
    public TSerializable? Deserialize([DisallowNull] Stream stream,
                                      in Int64 offset) =>
        this.Deserialize(stream: stream,
                         offset: offset,
                         read: out UInt64 _,
                         actionAfter: SerializationFinishAction.None);
    /// <summary>
    /// Deserializes the specified stream starting at the specified offset into an instance of type <typeparamref name="TSerializable"/>.
    /// </summary>
    /// <param name="stream">The stream to deserialize the graph from.</param>
    /// <param name="read">The amount of elements read from the <paramref name="stream"/> parameter.</param>
    /// <returns>The instance represented by the bytes in the specified stream</returns>
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="InvalidOperationException"/>
    [return: MaybeNull]
    public TSerializable? Deserialize([DisallowNull] Stream stream,
                                      out UInt64 read) =>
        this.Deserialize(stream: stream,
                         offset: -1,
                         read: out read,
                         actionAfter: SerializationFinishAction.None);
    /// <summary>
    /// Deserializes the specified stream starting at the specified offset into an instance of type <typeparamref name="TSerializable"/>.
    /// </summary>
    /// <param name="stream">The stream to deserialize the graph from.</param>
    /// <param name="offset">The offset in the stream where to begin reading.</param>
    /// <param name="read">The amount of elements read from the <paramref name="stream"/> parameter.</param>
    /// <returns>The instance represented by the bytes in the specified stream</returns>
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="InvalidOperationException"/>
    [return: MaybeNull]
    public TSerializable? Deserialize([DisallowNull] Stream stream, 
                                      in Int64 offset,
                                      out UInt64 read) =>
        this.Deserialize(stream: stream, 
                         offset: offset,
                         read: out read,
                         actionAfter: SerializationFinishAction.None);
    /// <summary>
    /// Deserializes the specified stream into an instance of type <typeparamref name="TSerializable"/>.
    /// </summary>
    /// <param name="stream">The stream to deserialize the graph from.</param>
    /// <param name="actionAfter">The actions to perform after the reading operation has finished.</param>
    /// <returns>The instance represented by the bytes in the specified stream</returns>
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="InvalidOperationException"/>
    [return: MaybeNull]
    public TSerializable? Deserialize([DisallowNull] Stream stream, 
                                      in SerializationFinishAction actionAfter) =>
        this.Deserialize(stream: stream, 
                         offset: -1,
                         read: out UInt64 _,
                         actionAfter: actionAfter);
    /// <summary>
    /// Deserializes the specified stream starting at the specified offset into an instance of type <typeparamref name="TSerializable"/>.
    /// </summary>
    /// <param name="stream">The stream to deserialize the graph from.</param>
    /// <param name="offset">The offset in the stream where to begin reading.</param>
    /// <param name="actionAfter">The actions to perform after the reading operation has finished.</param>
    /// <returns>The instance represented by the bytes in the specified stream</returns>
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="InvalidOperationException"/>
    [return: MaybeNull]
    public TSerializable? Deserialize([DisallowNull] Stream stream,
                                      in Int64 offset,
                                      in SerializationFinishAction actionAfter) =>
        this.Deserialize(stream: stream,
                         offset: offset,
                         read: out UInt64 _,
                         actionAfter: actionAfter);
    /// <summary>
    /// Deserializes the specified stream starting at the specified offset into an instance of type <typeparamref name="TSerializable"/>.
    /// </summary>
    /// <param name="stream">The stream to deserialize the graph from.</param>
    /// <param name="read">The amount of elements read from the <paramref name="stream"/> parameter.</param>
    /// <param name="actionAfter">The actions to perform after the reading operation has finished.</param>
    /// <returns>The instance represented by the bytes in the specified stream</returns>
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="InvalidOperationException"/>
    [return: MaybeNull]
    public TSerializable? Deserialize([DisallowNull] Stream stream,
                                      out UInt64 read, 
                                      in SerializationFinishAction actionAfter) =>
        this.Deserialize(stream: stream, 
                         offset: -1,
                         read: out read,
                         actionAfter: actionAfter);
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
    [return: MaybeNull]
    public TSerializable? Deserialize([DisallowNull] Stream stream, 
                                      in Int64 offset, 
                                      out UInt64 read,
                                      in SerializationFinishAction actionAfter)
    {
        ExceptionHelpers.ThrowIfArgumentNull(stream);
        if (!stream.CanRead)
        {
            throw new InvalidOperationException(message: STREAM_DOES_NOT_SUPPORT_READING);
        }
        if (offset > -1 &&
            !stream.CanSeek)
        {
            throw new InvalidOperationException(message: STREAM_DOES_NOT_SUPPORT_SEEKING);
        }

        if (offset > -1)
        {
            stream.Seek(offset: offset, 
                        origin: SeekOrigin.Begin);
        }

        ISerializationInfoGetter info = this.DeserializeInternal(stream: stream,
                                                                 read: out read);
        TSerializable? result = TSerializable.ConstructFromSerializationData(info);

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
                                  [AllowNull] out TSerializable? result) =>
        this.TryDeserialize(stream: stream,
                            offset: -1,
                            read: out UInt64 _,
                            actionAfter: SerializationFinishAction.None,
                            result: out result);
    /// <summary>
    /// Tries to deserialize the specified stream starting at the specified offset into an instance of type <typeparamref name="TSerializable"/>.
    /// </summary>
    /// <param name="stream">The stream to deserialize the graph from.</param>
    /// <param name="offset">The offset in the stream where to begin reading.</param>
    /// <param name="result">The instance represented by the bytes in the specified stream.</param>
    /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
    public Boolean TryDeserialize([DisallowNull] Stream stream,
                                  in Int64 offset,
                                  [AllowNull] out TSerializable? result) =>
        this.TryDeserialize(stream: stream,
                            offset: offset,
                            read: out UInt64 _,
                            actionAfter: SerializationFinishAction.None,
                            result: out result);
    /// <summary>
    /// Tries to deserialize the specified stream starting at the specified offset into an instance of type <typeparamref name="TSerializable"/>.
    /// </summary>
    /// <param name="stream">The stream to deserialize the graph from.</param>
    /// <param name="read">The amount of elements read from the <paramref name="stream"/> parameter.</param>
    /// <param name="result">The instance represented by the bytes in the specified stream.</param>
    /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
    public Boolean TryDeserialize([DisallowNull] Stream stream,
                                  out UInt64 read,
                                  [AllowNull] out TSerializable? result) =>
        this.TryDeserialize(stream: stream,
                            offset: -1,
                            read: out read,
                            actionAfter: SerializationFinishAction.None,
                            result: out result);
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
                                  [AllowNull] out TSerializable? result) =>
        this.TryDeserialize(stream: stream,
                            offset: offset,
                            read: out read,
                            actionAfter: SerializationFinishAction.None,
                            result: out result);
    /// <summary>
    /// Tries to deserialize the specified stream starting at the specified offset into an instance of type <typeparamref name="TSerializable"/>.
    /// </summary>
    /// <param name="stream">The stream to deserialize the graph from.</param>
    /// <param name="actionAfter">The actions to perform after the reading operation has finished.</param>
    /// <param name="result">The instance represented by the bytes in the specified stream.</param>
    /// <returns><see langword="true"/> if the serialization succeeded; else, <see langword="false"/></returns>
    public Boolean TryDeserialize([DisallowNull] Stream stream,
                                  in SerializationFinishAction actionAfter,
                                  [AllowNull] out TSerializable? result) =>
        this.TryDeserialize(stream: stream,
                            offset: -1,
                            read: out UInt64 _,
                            actionAfter: actionAfter,
                            result: out result);
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
                                  [AllowNull] out TSerializable? result) =>
        this.TryDeserialize(stream: stream,
                            offset: offset,
                            read: out UInt64 _,
                            actionAfter: actionAfter,
                            result: out result);
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
                                  [AllowNull] out TSerializable? result) =>
        this.TryDeserialize(stream: stream, 
                            offset: -1,
                            read: out read,
                            actionAfter: actionAfter, 
                            result: out result);
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
                                  [AllowNull] out TSerializable? result)
    {
        if (stream is null ||
            !stream.CanRead ||
            offset > -1 &&
            !stream.CanSeek)
        {
            read = 0;
            result = default;
            return false;
        }

        if (offset > -1)
        {
            stream.Seek(offset: offset,
                        origin: SeekOrigin.Begin);
        }

        ISerializationInfoGetter info = this.DeserializeInternal(stream: stream,
                                                                 read: out read);
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
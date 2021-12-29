namespace Narumikazuchi.Serialization.Bytes;

/// <summary>
/// Represents an <see cref="ISerializer"/> for objects that are provided with a custom state-reader/writer delegate.
/// </summary>
public sealed partial class ByteSerializer : SharedByteSerializer
{
    /// <summary>
    /// Instantiates a new instance of the <see cref="ByteSerializer"/> class.
    /// </summary>
    public ByteSerializer() :
        base(__SerializationStrategies.Integrated)
    { }
    /// <summary>
    /// Instantiates a new instance of the <see cref="ByteSerializer"/> class.
    /// </summary>
    public ByteSerializer([DisallowNull] IEnumerable<KeyValuePair<Type, ISerializationStrategy<Byte[]>>> strategies) :
        base(strategies: strategies)
    { }
    /// <summary>
    /// Instantiates a new instance of the <see cref="ByteSerializer"/> class.
    /// </summary>
    public ByteSerializer([DisallowNull] IEnumerable<(Type, ISerializationStrategy<Byte[]>)> strategies) :
        base(strategies: strategies)
    { }
    /// <summary>
    /// Instantiates a new instance of the <see cref="ByteSerializer"/> class.
    /// </summary>
    public ByteSerializer([DisallowNull] IEnumerable<Tuple<Type, ISerializationStrategy<Byte[]>>> strategies) :
        base(strategies: strategies)
    { }
}

// IUnboundSerializer
partial class ByteSerializer : IUnboundSerializer
{
    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="InvalidOperationException"/>
    public UInt64 Serialize<TAny>([DisallowNull] Stream stream,
                                  [AllowNull] TAny? graph,
                                  [DisallowNull] Action<TAny?, ISerializationInfoAdder> getSerializationData) =>
        this.Serialize(stream: stream,
                       graph: graph,
                       getSerializationData: getSerializationData,
                       offset: -1,
                       actionAfter: SerializationFinishAction.None);
    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="InvalidOperationException"/>
    public UInt64 Serialize<TAny>([DisallowNull] Stream stream,
                                  [AllowNull] TAny? graph,
                                  [DisallowNull] Action<TAny?, ISerializationInfoAdder> getSerializationData,
                                  in Int64 offset) =>
        this.Serialize(stream: stream,
                       graph: graph,
                       getSerializationData: getSerializationData,
                       offset: offset,
                       actionAfter: SerializationFinishAction.None);
    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="InvalidOperationException"/>
    public UInt64 Serialize<TAny>([DisallowNull] Stream stream,
                                  [AllowNull] TAny? graph,
                                  [DisallowNull] Action<TAny?, ISerializationInfoAdder> getSerializationData,
                                  in SerializationFinishAction actionAfter) =>
        this.Serialize(stream: stream,
                       graph: graph,
                       getSerializationData: getSerializationData,
                       offset: -1,
                       actionAfter: actionAfter);
    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="InvalidOperationException"/>
    public UInt64 Serialize<TAny>([DisallowNull] Stream stream,
                                  [AllowNull] TAny? graph,
                                  [DisallowNull] Action<TAny?, ISerializationInfoAdder> getSerializationData,
                                  in Int64 offset,
                                  in SerializationFinishAction actionAfter)
    {
        ExceptionHelpers.ThrowIfArgumentNull(stream);
        ExceptionHelpers.ThrowIfArgumentNull(graph);
        ExceptionHelpers.ThrowIfArgumentNull(getSerializationData);
        if (!stream.CanWrite)
        {
            throw new InvalidOperationException(message: STREAM_DOES_NOT_SUPPORT_WRITING);
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

        ISerializationInfoAdder info = SerializationInfoBuilder.CreateFrom(from: graph,
                                                                           write: getSerializationData);
        UInt64 result = this.SerializeWithInfo(stream: stream,
                                               info: info);

        if (actionAfter.HasFlag(SerializationFinishAction.MoveToBeginning))
        {
            stream.Position = 0;
        }
        if (actionAfter.HasFlag(SerializationFinishAction.FlushStream))
        {
            stream.Flush();
        }
        if (actionAfter.HasFlag(SerializationFinishAction.CloseStream))
        {
            stream.Close();
        }
        if (actionAfter.HasFlag(SerializationFinishAction.DisposeStream))
        {
            stream.Dispose();
        }

        return result;
    }

    /// <inheritdoc/>
    public Boolean TrySerialize<TAny>([DisallowNull] Stream stream,
                                      [AllowNull] TAny? graph,
                                      [DisallowNull] Action<TAny?, ISerializationInfoAdder> getSerializationData) =>
        this.TrySerialize(stream: stream,
                          graph: graph,
                          getSerializationData : getSerializationData,
                          offset: -1,
                          written: out UInt64 _,
                          actionAfter: SerializationFinishAction.None);
    /// <inheritdoc/>
    public Boolean TrySerialize<TAny>([DisallowNull] Stream stream,
                                      [AllowNull] TAny? graph,
                                      [DisallowNull] Action<TAny?, ISerializationInfoAdder> getSerializationData,
                                      in Int64 offset) =>
        this.TrySerialize(stream: stream,
                          graph: graph,
                          getSerializationData: getSerializationData,
                          offset: offset,
                          written: out UInt64 _,
                          actionAfter: SerializationFinishAction.None);
    /// <inheritdoc/>
    public Boolean TrySerialize<TAny>([DisallowNull] Stream stream,
                                      [AllowNull] TAny? graph,
                                      [DisallowNull] Action<TAny?, ISerializationInfoAdder> getSerializationData,
                                      out UInt64 written) =>
        this.TrySerialize(stream: stream,
                          graph: graph,
                          getSerializationData: getSerializationData,
                          offset: -1,
                          written: out written,
                          actionAfter: SerializationFinishAction.None);
    /// <inheritdoc/>
    public Boolean TrySerialize<TAny>([DisallowNull] Stream stream,
                                      [AllowNull] TAny? graph,
                                      [DisallowNull] Action<TAny?, ISerializationInfoAdder> getSerializationData,
                                      in Int64 offset,
                                      out UInt64 written) =>
        this.TrySerialize(stream: stream,
                          graph: graph,
                          getSerializationData: getSerializationData,
                          offset: offset,
                          written: out written,
                          actionAfter: SerializationFinishAction.None);
    /// <inheritdoc/>
    public Boolean TrySerialize<TAny>([DisallowNull] Stream stream,
                                      [AllowNull] TAny? graph,
                                      [DisallowNull] Action<TAny?, ISerializationInfoAdder> getSerializationData,
                                      in SerializationFinishAction actionAfter) =>
        this.TrySerialize(stream: stream,
                          graph: graph,
                          getSerializationData: getSerializationData,
                          offset: -1,
                          written: out UInt64 _,
                          actionAfter: actionAfter);
    /// <inheritdoc/>
    public Boolean TrySerialize<TAny>([DisallowNull] Stream stream,
                                      [AllowNull] TAny? graph,
                                      [DisallowNull] Action<TAny?, ISerializationInfoAdder> getSerializationData,
                                      in Int64 offset,
                                      in SerializationFinishAction actionAfter) =>
        this.TrySerialize(stream: stream,
                          graph: graph,
                          getSerializationData: getSerializationData,
                          offset: offset,
                          written: out UInt64 _,
                          actionAfter: actionAfter);
    /// <inheritdoc/>
    public Boolean TrySerialize<TAny>([DisallowNull] Stream stream,
                                      [AllowNull] TAny? graph,
                                      [DisallowNull] Action<TAny?, ISerializationInfoAdder> getSerializationData,
                                      out UInt64 written,
                                      in SerializationFinishAction actionAfter) =>
        this.TrySerialize(stream: stream,
                          graph: graph,
                          getSerializationData: getSerializationData,
                          offset: -1,
                          written: out written,
                          actionAfter: actionAfter);
    /// <inheritdoc/>
    public Boolean TrySerialize<TAny>([DisallowNull] Stream stream,
                                      [AllowNull] TAny? graph,
                                      [DisallowNull] Action<TAny?, ISerializationInfoAdder> getSerializationData,
                                      in Int64 offset,
                                      out UInt64 written,
                                      in SerializationFinishAction actionAfter)
    {
        if (stream is null ||
            graph is null ||
            getSerializationData is null ||
            !stream.CanWrite ||
            offset > -1 &&
            !stream.CanSeek)
        {
            written = 0;
            return false;
        }

        if (offset > -1)
        {
            stream.Seek(offset: offset,
                        origin: SeekOrigin.Begin);
        }

        ISerializationInfoAdder info = SerializationInfoBuilder.CreateFrom(from: graph,
                                                                           write: getSerializationData);
        written = this.SerializeWithInfo(stream: stream,
                                         info: info);

        if (actionAfter.HasFlag(SerializationFinishAction.MoveToBeginning))
        {
            stream.Position = 0;
        }
        if (actionAfter.HasFlag(SerializationFinishAction.FlushStream))
        {
            stream.Flush();
        }
        if (actionAfter.HasFlag(SerializationFinishAction.CloseStream))
        {
            stream.Close();
        }
        if (actionAfter.HasFlag(SerializationFinishAction.DisposeStream))
        {
            stream.Dispose();
        }

        return true;
    }

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="InvalidOperationException"/>
    [return: MaybeNull]
    public TAny? Deserialize<TAny>([DisallowNull] Stream stream,
                                   [DisallowNull] Func<ISerializationInfoGetter, TAny?> constructFromSerializationData) =>
        this.Deserialize(stream: stream,
                         constructFromSerializationData: constructFromSerializationData,
                         offset: -1,
                         read: out UInt64 _,
                         actionAfter: SerializationFinishAction.None);
    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="InvalidOperationException"/>
    [return: MaybeNull]
    public TAny? Deserialize<TAny>([DisallowNull] Stream stream,
                                   [DisallowNull] Func<ISerializationInfoGetter, TAny?> constructFromSerializationData,
                                   in Int64 offset) =>
        this.Deserialize(stream: stream,
                         constructFromSerializationData: constructFromSerializationData,
                         offset: offset,
                         read: out UInt64 _,
                         actionAfter: SerializationFinishAction.None);
    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="InvalidOperationException"/>
    [return: MaybeNull]
    public TAny? Deserialize<TAny>([DisallowNull] Stream stream,
                                   [DisallowNull] Func<ISerializationInfoGetter, TAny?> constructFromSerializationData,
                                   out UInt64 read) =>
        this.Deserialize(stream: stream,
                         constructFromSerializationData: constructFromSerializationData,
                         offset: -1,
                         read: out read,
                         actionAfter: SerializationFinishAction.None);
    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="InvalidOperationException"/>
    [return: MaybeNull]
    public TAny? Deserialize<TAny>([DisallowNull] Stream stream,
                                   [DisallowNull] Func<ISerializationInfoGetter, TAny?> constructFromSerializationData,
                                   in Int64 offset,
                                   out UInt64 read) =>
        this.Deserialize(stream: stream,
                         constructFromSerializationData: constructFromSerializationData,
                         offset: offset,
                         read: out read,
                         actionAfter: SerializationFinishAction.None);
    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="InvalidOperationException"/>
    [return: MaybeNull]
    public TAny? Deserialize<TAny>([DisallowNull] Stream stream,
                                   [DisallowNull] Func<ISerializationInfoGetter, TAny?> constructFromSerializationData,
                                   in SerializationFinishAction actionAfter) =>
        this.Deserialize(stream: stream,
                         constructFromSerializationData: constructFromSerializationData,
                         offset: -1,
                         read: out UInt64 _,
                         actionAfter: actionAfter);
    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="InvalidOperationException"/>
    [return: MaybeNull]
    public TAny? Deserialize<TAny>([DisallowNull] Stream stream,
                                   [DisallowNull] Func<ISerializationInfoGetter, TAny?> constructFromSerializationData,
                                   in Int64 offset,
                                   in SerializationFinishAction actionAfter) =>
        this.Deserialize(stream: stream,
                         constructFromSerializationData: constructFromSerializationData,
                         offset: offset,
                         read: out UInt64 _,
                         actionAfter: actionAfter);
    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="InvalidOperationException"/>
    [return: MaybeNull]
    public TAny? Deserialize<TAny>([DisallowNull] Stream stream,
                                   [DisallowNull] Func<ISerializationInfoGetter, TAny?> constructFromSerializationData,
                                   out UInt64 read,
                                   in SerializationFinishAction actionAfter) =>
        this.Deserialize(stream: stream,
                         constructFromSerializationData: constructFromSerializationData,
                         offset: -1,
                         read: out read,
                         actionAfter: actionAfter);
    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="InvalidOperationException"/>
    [return: MaybeNull]
    public TAny? Deserialize<TAny>([DisallowNull] Stream stream,
                                   [DisallowNull] Func<ISerializationInfoGetter, TAny?> constructFromSerializationData,
                                   in Int64 offset,
                                   out UInt64 read,
                                   in SerializationFinishAction actionAfter)
    {
        ExceptionHelpers.ThrowIfArgumentNull(stream);
        ExceptionHelpers.ThrowIfArgumentNull(constructFromSerializationData);
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
        TAny? result = constructFromSerializationData.Invoke(info);

        if (actionAfter.HasFlag(SerializationFinishAction.MoveToBeginning))
        {
            stream.Position = 0;
        }
        if (actionAfter.HasFlag(SerializationFinishAction.FlushStream))
        {
            stream.Flush();
        }
        if (actionAfter.HasFlag(SerializationFinishAction.CloseStream))
        {
            stream.Close();
        }
        if (actionAfter.HasFlag(SerializationFinishAction.DisposeStream))
        {
            stream.Dispose();
        }

        return result;
    }

    /// <inheritdoc/>
    public Boolean TryDeserialize<TAny>([DisallowNull] Stream stream,
                                        [DisallowNull] Func<ISerializationInfoGetter, TAny?> constructFromSerializationData,
                                        [AllowNull] out TAny? result) =>
        this.TryDeserialize(stream: stream,
                            constructFromSerializationData: constructFromSerializationData,
                            offset: -1,
                            read: out UInt64 _,
                            actionAfter: SerializationFinishAction.None,
                            result: out result);
    /// <inheritdoc/>
    public Boolean TryDeserialize<TAny>([DisallowNull] Stream stream,
                                        [DisallowNull] Func<ISerializationInfoGetter, TAny?> constructFromSerializationData,
                                        in Int64 offset,
                                        [AllowNull] out TAny? result) =>
        this.TryDeserialize(stream: stream,
                            constructFromSerializationData: constructFromSerializationData,
                            offset: offset,
                            read: out UInt64 _,
                            actionAfter: SerializationFinishAction.None,
                            result: out result);
    /// <inheritdoc/>
    public Boolean TryDeserialize<TAny>([DisallowNull] Stream stream,
                                        [DisallowNull] Func<ISerializationInfoGetter, TAny?> constructFromSerializationData,
                                        out UInt64 read,
                                        [AllowNull] out TAny? result) =>
        this.TryDeserialize(stream: stream,
                            constructFromSerializationData: constructFromSerializationData,
                            offset: -1,
                            read: out read,
                            actionAfter: SerializationFinishAction.None,
                            result: out result);
    /// <inheritdoc/>
    public Boolean TryDeserialize<TAny>([DisallowNull] Stream stream,
                                        [DisallowNull] Func<ISerializationInfoGetter, TAny?> constructFromSerializationData,
                                        in Int64 offset,
                                        out UInt64 read,
                                        [AllowNull] out TAny? result) =>
        this.TryDeserialize(stream: stream,
                            constructFromSerializationData: constructFromSerializationData,
                            offset: offset,
                            read: out read,
                            actionAfter: SerializationFinishAction.None,
                            result: out result);
    /// <inheritdoc/>
    public Boolean TryDeserialize<TAny>([DisallowNull] Stream stream,
                                        [DisallowNull] Func<ISerializationInfoGetter, TAny?> constructFromSerializationData,
                                        in SerializationFinishAction actionAfter,
                                        [AllowNull] out TAny? result) =>
        this.TryDeserialize(stream: stream,
                            constructFromSerializationData: constructFromSerializationData,
                            offset: -1,
                            read: out UInt64 _,
                            actionAfter: actionAfter,
                            result: out result);
    /// <inheritdoc/>
    public Boolean TryDeserialize<TAny>([DisallowNull] Stream stream,
                                        [DisallowNull] Func<ISerializationInfoGetter, TAny?> constructFromSerializationData,
                                        in Int64 offset,
                                        in SerializationFinishAction actionAfter,
                                        [AllowNull] out TAny? result) =>
        this.TryDeserialize(stream: stream,
                            constructFromSerializationData: constructFromSerializationData,
                            offset: offset,
                            read: out UInt64 _,
                            actionAfter: actionAfter,
                            result: out result);
    /// <inheritdoc/>
    public Boolean TryDeserialize<TAny>([DisallowNull] Stream stream,
                                        [DisallowNull] Func<ISerializationInfoGetter, TAny?> constructFromSerializationData,
                                        out UInt64 read,
                                        in SerializationFinishAction actionAfter,
                                        [AllowNull] out TAny? result) =>
        this.TryDeserialize(stream: stream,
                            constructFromSerializationData: constructFromSerializationData,
                            offset: -1,
                            read: out read,
                            actionAfter: actionAfter,
                            result: out result);
    /// <inheritdoc/>
    public Boolean TryDeserialize<TAny>([DisallowNull] Stream stream,
                                        [DisallowNull] Func<ISerializationInfoGetter, TAny?> constructFromSerializationData,
                                        in Int64 offset,
                                        out UInt64 read,
                                        in SerializationFinishAction actionAfter,
                                        [AllowNull] out TAny? result)
    {
        if (stream is null ||
            constructFromSerializationData is null ||
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
        result = constructFromSerializationData.Invoke(info);

        if (actionAfter.HasFlag(SerializationFinishAction.MoveToBeginning))
        {
            stream.Position = 0;
        }
        if (actionAfter.HasFlag(SerializationFinishAction.FlushStream))
        {
            stream.Flush();
        }
        if (actionAfter.HasFlag(SerializationFinishAction.CloseStream))
        {
            stream.Close();
        }
        if (actionAfter.HasFlag(SerializationFinishAction.DisposeStream))
        {
            stream.Dispose();
        }

        return read > 0;
    }
}
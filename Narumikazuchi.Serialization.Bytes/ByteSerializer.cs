namespace Narumikazuchi.Serialization.Bytes;

/// <summary>
/// Represents an <see cref="ISerializer"/> for objects that are marked with the <see cref="CustomSerializableAttribute"/> or are provided with a custom state-reader/writer.
/// </summary>
public sealed partial class ByteSerializer : SharedByteSerializer
{
    /// <summary>
    /// Instantiates a new instance of the <see cref="ByteSerializer"/> class.
    /// </summary>
    public ByteSerializer() :
        base()
    { }
    /// <summary>
    /// Instantiates a new instance of the <see cref="ByteSerializer"/> class.
    /// </summary>
    public ByteSerializer([DisallowNull] IReadOnlyDictionary<Type, ISerializationStrategy<Byte[]>> strategies) :
        base(strategies)
    { }
}

// IUnboundSerializer
partial class ByteSerializer : IUnboundSerializer
{
    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="InvalidOperationException"/>
    public UInt64 Serialize<TAny>([DisallowNull] Stream stream,
                                  [DisallowNull] TAny graph,
                                  [DisallowNull] Action<TAny, SerializationInfo> getSerializationData) =>
        this.Serialize(stream,
                       graph,
                       getSerializationData,
                       -1,
                       SerializationFinishAction.None);
    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="InvalidOperationException"/>
    public UInt64 Serialize<TAny>([DisallowNull] Stream stream,
                                  [DisallowNull] TAny graph,
                                  [DisallowNull] Action<TAny, SerializationInfo> getSerializationData,
                                  in Int64 offset) =>
        this.Serialize(stream,
                       graph,
                       getSerializationData,
                       offset,
                       SerializationFinishAction.None);
    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="InvalidOperationException"/>
    public UInt64 Serialize<TAny>([DisallowNull] Stream stream,
                                  [DisallowNull] TAny graph,
                                  [DisallowNull] Action<TAny, SerializationInfo> getSerializationData,
                                  in SerializationFinishAction actionAfter) =>
        this.Serialize(stream,
                       graph,
                       getSerializationData,
                       -1,
                       actionAfter);
    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="InvalidOperationException"/>
    public UInt64 Serialize<TAny>([DisallowNull] Stream stream,
                                  [DisallowNull] TAny graph,
                                  [DisallowNull] Action<TAny, SerializationInfo> getSerializationData,
                                  in Int64 offset,
                                  in SerializationFinishAction actionAfter)
    {
        ExceptionHelpers.ThrowIfArgumentNull(stream);
        ExceptionHelpers.ThrowIfArgumentNull(graph);
        ExceptionHelpers.ThrowIfArgumentNull(getSerializationData);
        if (!stream.CanWrite)
        {
            throw new InvalidOperationException(STREAM_DOES_NOT_SUPPORT_WRITING);
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

#nullable disable
        __Header header = new(graph.GetType());
        SerializationInfo info = SerializationInfo.Create(graph,
                                                          getSerializationData);
        UInt64 result = this.SerializeWithInfo(stream,
                                               info,
                                               header);
#nullable enable

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
                                      [DisallowNull] TAny graph,
                                      [DisallowNull] Action<TAny, SerializationInfo> getSerializationData) =>
        this.TrySerialize(stream,
                          graph,
                          getSerializationData,
                          -1,
                          out UInt64 _,
                          SerializationFinishAction.None);
    /// <inheritdoc/>
    public Boolean TrySerialize<TAny>([DisallowNull] Stream stream,
                                      [DisallowNull] TAny graph,
                                      [DisallowNull] Action<TAny, SerializationInfo> getSerializationData,
                                      in Int64 offset) =>
        this.TrySerialize(stream,
                          graph,
                          getSerializationData,
                          offset,
                          out UInt64 _,
                          SerializationFinishAction.None);
    /// <inheritdoc/>
    public Boolean TrySerialize<TAny>([DisallowNull] Stream stream,
                                      [DisallowNull] TAny graph,
                                      [DisallowNull] Action<TAny, SerializationInfo> getSerializationData,
                                      out UInt64 written) =>
        this.TrySerialize(stream,
                          graph,
                          getSerializationData,
                          -1,
                          out written,
                          SerializationFinishAction.None);
    /// <inheritdoc/>
    public Boolean TrySerialize<TAny>([DisallowNull] Stream stream,
                                      [DisallowNull] TAny graph,
                                      [DisallowNull] Action<TAny, SerializationInfo> getSerializationData,
                                      in Int64 offset,
                                      out UInt64 written) =>
        this.TrySerialize(stream,
                          graph,
                          getSerializationData,
                          offset,
                          out written,
                          SerializationFinishAction.None);
    /// <inheritdoc/>
    public Boolean TrySerialize<TAny>([DisallowNull] Stream stream,
                                      [DisallowNull] TAny graph,
                                      [DisallowNull] Action<TAny, SerializationInfo> getSerializationData,
                                      in SerializationFinishAction actionAfter) =>
        this.TrySerialize(stream,
                          graph,
                          getSerializationData,
                          -1,
                          out UInt64 _,
                          actionAfter);
    /// <inheritdoc/>
    public Boolean TrySerialize<TAny>([DisallowNull] Stream stream,
                                      [DisallowNull] TAny graph,
                                      [DisallowNull] Action<TAny, SerializationInfo> getSerializationData,
                                      in Int64 offset,
                                      in SerializationFinishAction actionAfter) =>
        this.TrySerialize(stream,
                          graph,
                          getSerializationData,
                          offset,
                          out UInt64 _,
                          actionAfter);
    /// <inheritdoc/>
    public Boolean TrySerialize<TAny>([DisallowNull] Stream stream,
                                      [DisallowNull] TAny graph,
                                      [DisallowNull] Action<TAny, SerializationInfo> getSerializationData,
                                      out UInt64 written,
                                      in SerializationFinishAction actionAfter) =>
        this.TrySerialize(stream,
                          graph,
                          getSerializationData,
                          -1,
                          out written,
                          actionAfter);
    /// <inheritdoc/>
    public Boolean TrySerialize<TAny>([DisallowNull] Stream stream,
                                      [DisallowNull] TAny graph,
                                      [DisallowNull] Action<TAny, SerializationInfo> getSerializationData,
                                      in Int64 offset,
                                      out UInt64 written,
                                      in SerializationFinishAction actionAfter)
    {
        if (stream is null ||
            graph is null ||
            getSerializationData is null ||
            !stream.CanWrite ||
            (offset > -1 &&
            !stream.CanSeek))
        {
            written = 0;
            return false;
        }

        if (offset > -1)
        {
            stream.Seek(offset,
                        SeekOrigin.Begin);
        }

#nullable disable
        __Header header = new(graph.GetType());
        SerializationInfo info = SerializationInfo.Create(graph,
                                                          getSerializationData);
        written = this.SerializeWithInfo(stream,
                                         info,
                                         header);
#nullable enable

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
    [return: NotNull]
    public TAny Deserialize<TAny>([DisallowNull] Stream stream,
                                  [DisallowNull] Func<SerializationInfo, TAny> constructFromSerializationData) =>
        this.Deserialize(stream,
                         constructFromSerializationData,
                         -1,
                         out UInt64 _,
                         SerializationFinishAction.None);
    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="InvalidOperationException"/>
    [return: NotNull]
    public TAny Deserialize<TAny>([DisallowNull] Stream stream,
                                  [DisallowNull] Func<SerializationInfo, TAny> constructFromSerializationData,
                                  in Int64 offset) =>
        this.Deserialize(stream,
                         constructFromSerializationData,
                         offset,
                         out UInt64 _,
                         SerializationFinishAction.None);
    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="InvalidOperationException"/>
    [return: NotNull]
    public TAny Deserialize<TAny>([DisallowNull] Stream stream,
                                  [DisallowNull] Func<SerializationInfo, TAny> constructFromSerializationData,
                                  out UInt64 read) =>
        this.Deserialize(stream,
                         constructFromSerializationData,
                         -1,
                         out read,
                         SerializationFinishAction.None);
    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="InvalidOperationException"/>
    [return: NotNull]
    public TAny Deserialize<TAny>([DisallowNull] Stream stream,
                                  [DisallowNull] Func<SerializationInfo, TAny> constructFromSerializationData,
                                  in Int64 offset,
                                  out UInt64 read) =>
        this.Deserialize(stream,
                         constructFromSerializationData,
                         offset,
                         out read,
                         SerializationFinishAction.None);
    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="InvalidOperationException"/>
    [return: NotNull]
    public TAny Deserialize<TAny>([DisallowNull] Stream stream,
                                  [DisallowNull] Func<SerializationInfo, TAny> constructFromSerializationData,
                                  in SerializationFinishAction actionAfter) =>
        this.Deserialize(stream,
                         constructFromSerializationData,
                         -1,
                         out UInt64 _,
                         actionAfter);
    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="InvalidOperationException"/>
    [return: NotNull]
    public TAny Deserialize<TAny>([DisallowNull] Stream stream,
                                  [DisallowNull] Func<SerializationInfo, TAny> constructFromSerializationData,
                                  in Int64 offset,
                                  in SerializationFinishAction actionAfter) =>
        this.Deserialize(stream,
                         constructFromSerializationData,
                         offset,
                         out UInt64 _,
                         actionAfter);
    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="InvalidOperationException"/>
    [return: NotNull]
    public TAny Deserialize<TAny>([DisallowNull] Stream stream,
                                  [DisallowNull] Func<SerializationInfo, TAny> constructFromSerializationData,
                                  out UInt64 read,
                                  in SerializationFinishAction actionAfter) =>
        this.Deserialize(stream,
                         constructFromSerializationData,
                         -1,
                         out read,
                         actionAfter);
    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="InvalidOperationException"/>
    [return: NotNull]
    public TAny Deserialize<TAny>([DisallowNull] Stream stream,
                                  [DisallowNull] Func<SerializationInfo, TAny> constructFromSerializationData,
                                  in Int64 offset,
                                  out UInt64 read,
                                  in SerializationFinishAction actionAfter)
    {
        ExceptionHelpers.ThrowIfArgumentNull(stream);
        ExceptionHelpers.ThrowIfArgumentNull(constructFromSerializationData);
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
        TAny result = constructFromSerializationData.Invoke(info);
        if (result is null)
        {
            throw new InvalidOperationException("Object construction delegate is not allowed to return null.");
        }

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

#pragma warning disable CS8767
    /// <inheritdoc/>
    public Boolean TryDeserialize<TAny>([DisallowNull] Stream stream,
                                        [DisallowNull] Func<SerializationInfo, TAny> constructFromSerializationData,
                                        [NotNullWhen(true)] out TAny? result) =>
        this.TryDeserialize(stream,
                            constructFromSerializationData,
                            -1,
                            out UInt64 _,
                            SerializationFinishAction.None,
                            out result);
    /// <inheritdoc/>
    public Boolean TryDeserialize<TAny>([DisallowNull] Stream stream,
                                        [DisallowNull] Func<SerializationInfo, TAny> constructFromSerializationData,
                                        in Int64 offset,
                                        [NotNullWhen(true)] out TAny? result) =>
        this.TryDeserialize(stream,
                            constructFromSerializationData,
                            offset,
                            out UInt64 _,
                            SerializationFinishAction.None,
                            out result);
    /// <inheritdoc/>
    public Boolean TryDeserialize<TAny>([DisallowNull] Stream stream,
                                        [DisallowNull] Func<SerializationInfo, TAny> constructFromSerializationData,
                                        out UInt64 read,
                                        [NotNullWhen(true)] out TAny? result) =>
        this.TryDeserialize(stream,
                            constructFromSerializationData,
                            -1,
                            out read,
                            SerializationFinishAction.None,
                            out result);
    /// <inheritdoc/>
    public Boolean TryDeserialize<TAny>([DisallowNull] Stream stream,
                                        [DisallowNull] Func<SerializationInfo, TAny> constructFromSerializationData,
                                        in Int64 offset,
                                        out UInt64 read,
                                        [NotNullWhen(true)] out TAny? result) =>
        this.TryDeserialize(stream,
                            constructFromSerializationData,
                            offset,
                            out read,
                            SerializationFinishAction.None,
                            out result);
    /// <inheritdoc/>
    public Boolean TryDeserialize<TAny>([DisallowNull] Stream stream,
                                        [DisallowNull] Func<SerializationInfo, TAny> constructFromSerializationData,
                                        in SerializationFinishAction actionAfter,
                                        [NotNullWhen(true)] out TAny? result) =>
        this.TryDeserialize(stream,
                            constructFromSerializationData,
                            -1,
                            out UInt64 _,
                            actionAfter,
                            out result);
    /// <inheritdoc/>
    public Boolean TryDeserialize<TAny>([DisallowNull] Stream stream,
                                        [DisallowNull] Func<SerializationInfo, TAny> constructFromSerializationData,
                                        in Int64 offset,
                                        in SerializationFinishAction actionAfter,
                                        [NotNullWhen(true)] out TAny? result) =>
        this.TryDeserialize(stream,
                            constructFromSerializationData,
                            offset,
                            out UInt64 _,
                            actionAfter,
                            out result);
    /// <inheritdoc/>
    public Boolean TryDeserialize<TAny>([DisallowNull] Stream stream,
                                        [DisallowNull] Func<SerializationInfo, TAny> constructFromSerializationData,
                                        out UInt64 read,
                                        in SerializationFinishAction actionAfter,
                                        [NotNullWhen(true)] out TAny? result) =>
        this.TryDeserialize(stream,
                            constructFromSerializationData,
                            -1,
                            out read,
                            actionAfter,
                            out result);
    /// <inheritdoc/>
    public Boolean TryDeserialize<TAny>([DisallowNull] Stream stream,
                                        [DisallowNull] Func<SerializationInfo, TAny> constructFromSerializationData,
                                        in Int64 offset,
                                        out UInt64 read,
                                        in SerializationFinishAction actionAfter,
                                        [NotNullWhen(true)] out TAny? result)
    {
        if (stream is null ||
            constructFromSerializationData is null ||
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

        return result is not null;
    }
}
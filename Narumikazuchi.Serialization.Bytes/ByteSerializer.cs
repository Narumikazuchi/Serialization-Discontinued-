namespace Narumikazuchi.Serialization.Bytes
{
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
        public ByteSerializer(IReadOnlyDictionary<Type, ISerializationStrategy<Byte[]>> strategies) :
            base(strategies)
        { }
    }

    // IDeclaredSerializer
    partial class ByteSerializer : IDeclaredSerializer
    {
        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="InvalidOperationException"/>
        public UInt64 Serialize([DisallowNull] Stream stream, 
                                [DisallowNull] ISerializable graph) =>
            this.Serialize(stream,
                           graph,
                           -1,
                           SerializationFinishAction.None);
        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="InvalidOperationException"/>
        public UInt64 Serialize([DisallowNull] Stream stream, 
                                [DisallowNull] ISerializable graph, 
                                in Int64 offset) =>
            this.Serialize(stream,
                           graph,
                           offset,
                           SerializationFinishAction.None);
        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="InvalidOperationException"/>
        public UInt64 Serialize([DisallowNull] Stream stream, 
                                [DisallowNull] ISerializable graph, 
                                in SerializationFinishAction actionAfter) =>
            this.Serialize(stream,
                           graph,
                           -1,
                           actionAfter);
        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="InvalidOperationException"/>
        public UInt64 Serialize([DisallowNull] Stream stream, 
                                [DisallowNull] ISerializable graph, 
                                in Int64 offset, 
                                in SerializationFinishAction actionAfter)
        {
            ExceptionHelpers.ThrowIfNull(stream);
            ExceptionHelpers.ThrowIfNull(graph);
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

            UInt64 result = this.SerializeInternal(stream,
                                                   graph);

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
        public Boolean TrySerialize([DisallowNull] Stream stream,
                                    [DisallowNull] ISerializable graph) =>
            this.TrySerialize(stream,
                              graph,
                              -1,
                              out UInt64 _,
                              SerializationFinishAction.None);
        /// <inheritdoc/>
        public Boolean TrySerialize([DisallowNull] Stream stream, 
                                    [DisallowNull] ISerializable graph, 
                                    in Int64 offset) =>
            this.TrySerialize(stream,
                              graph,
                              offset,
                              out UInt64 _,
                              SerializationFinishAction.None);
        /// <inheritdoc/>
        public Boolean TrySerialize([DisallowNull] Stream stream, 
                                    [DisallowNull] ISerializable graph, 
                                    out UInt64 written) =>
            this.TrySerialize(stream,
                              graph,
                              -1,
                              out written,
                              SerializationFinishAction.None);
        /// <inheritdoc/>
        public Boolean TrySerialize([DisallowNull] Stream stream, 
                                    [DisallowNull] ISerializable graph, 
                                    in Int64 offset, 
                                    out UInt64 written) =>
            this.TrySerialize(stream,
                              graph,
                              offset,
                              out written,
                              SerializationFinishAction.None);
        /// <inheritdoc/>
        public Boolean TrySerialize([DisallowNull] Stream stream, 
                                    [DisallowNull] ISerializable graph, 
                                    in SerializationFinishAction actionAfter) =>
            this.TrySerialize(stream,
                              graph,
                              -1,
                              out UInt64 _,
                              actionAfter);
        /// <inheritdoc/>
        public Boolean TrySerialize([DisallowNull] Stream stream, 
                                    [DisallowNull] ISerializable graph, 
                                    in Int64 offset, 
                                    in SerializationFinishAction actionAfter) =>
            this.TrySerialize(stream,
                              graph,
                              offset,
                              out UInt64 _,
                              actionAfter);
        /// <inheritdoc/>
        public Boolean TrySerialize([DisallowNull] Stream stream, 
                                    [DisallowNull] ISerializable graph, 
                                    out UInt64 written, 
                                    in SerializationFinishAction actionAfter) =>
            this.TrySerialize(stream,
                              graph,
                              -1,
                              out written,
                              actionAfter);
        /// <inheritdoc/>
        public Boolean TrySerialize([DisallowNull] Stream stream, 
                                    [DisallowNull] ISerializable graph, 
                                    in Int64 offset, 
                                    out UInt64 written, 
                                    in SerializationFinishAction actionAfter)
        {
            if (stream is null ||
                graph is null ||
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
            written = this.SerializeInternal(stream,
                                             graph);

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
    }

    // IObjectSerializer
    partial class ByteSerializer : IObjectSerializer
    {
        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="InvalidOperationException"/>
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.NonPublicMethods)]
        public UInt64 Serialize([DisallowNull] Stream stream,
                                [DisallowNull] Object graph) =>
            this.Serialize(stream,
                           graph,
                           -1,
                           SerializationFinishAction.None);
        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="InvalidOperationException"/>
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.NonPublicMethods)]
        public UInt64 Serialize([DisallowNull] Stream stream,
                                [DisallowNull] Object graph,
                                in Int64 offset) =>
            this.Serialize(stream,
                           graph,
                           offset,
                           SerializationFinishAction.None);
        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="InvalidOperationException"/>
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.NonPublicMethods)]
        public UInt64 Serialize([DisallowNull] Stream stream,
                                [DisallowNull] Object graph,
                                in SerializationFinishAction actionAfter) =>
            this.Serialize(stream,
                           graph,
                           -1,
                           actionAfter);
        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="InvalidOperationException"/>
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.NonPublicMethods)]
        public UInt64 Serialize([DisallowNull] Stream stream,
                                [DisallowNull] Object graph,
                                in Int64 offset,
                                in SerializationFinishAction actionAfter)
        {
            ExceptionHelpers.ThrowIfNull(stream);
            ExceptionHelpers.ThrowIfNull(graph);
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
            UInt64 result = this.SerializeInternal(stream,
                                                   graph);

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
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.NonPublicMethods)]
        public Boolean TrySerialize([DisallowNull] Stream stream,
                                    [DisallowNull] Object graph) =>
            this.TrySerialize(stream,
                              graph,
                              -1,
                              out UInt64 _,
                              SerializationFinishAction.None);
        /// <inheritdoc/>
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.NonPublicMethods)]
        public Boolean TrySerialize([DisallowNull] Stream stream,
                                    [DisallowNull] Object graph,
                                    in Int64 offset) =>
            this.TrySerialize(stream,
                              graph,
                              offset,
                              out UInt64 _,
                              SerializationFinishAction.None);
        /// <inheritdoc/>
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.NonPublicMethods)]
        public Boolean TrySerialize([DisallowNull] Stream stream,
                                    [DisallowNull] Object graph,
                                    out UInt64 written) =>
            this.TrySerialize(stream,
                              graph,
                              -1,
                              out written,
                              SerializationFinishAction.None);
        /// <inheritdoc/>
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.NonPublicMethods)]
        public Boolean TrySerialize([DisallowNull] Stream stream,
                                    [DisallowNull] Object graph,
                                    in Int64 offset,
                                    out UInt64 written) =>
            this.TrySerialize(stream,
                              graph,
                              offset,
                              out written,
                              SerializationFinishAction.None);
        /// <inheritdoc/>
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.NonPublicMethods)]
        public Boolean TrySerialize([DisallowNull] Stream stream,
                                    [DisallowNull] Object graph,
                                    in SerializationFinishAction actionAfter) =>
            this.TrySerialize(stream,
                              graph,
                              -1,
                              out UInt64 _,
                              actionAfter);
        /// <inheritdoc/>
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.NonPublicMethods)]
        public Boolean TrySerialize([DisallowNull] Stream stream,
                                    [DisallowNull] Object graph,
                                    in Int64 offset,
                                    in SerializationFinishAction actionAfter) =>
            this.TrySerialize(stream,
                              graph,
                              offset,
                              out UInt64 _,
                              actionAfter);
        /// <inheritdoc/>
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.NonPublicMethods)]
        public Boolean TrySerialize([DisallowNull] Stream stream,
                                    [DisallowNull] Object graph,
                                    out UInt64 written,
                                    in SerializationFinishAction actionAfter) =>
            this.TrySerialize(stream,
                              graph,
                              -1,
                              out written,
                              actionAfter);
        /// <inheritdoc/>
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.NonPublicMethods)]
        public Boolean TrySerialize([DisallowNull] Stream stream,
                                    [DisallowNull] Object graph,
                                    in Int64 offset,
                                    out UInt64 written,
                                    in SerializationFinishAction actionAfter)
        {
            if (stream is null ||
                graph is null ||
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
            written = this.SerializeInternal(stream,
                                             graph);

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
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.NonPublicMethods)]
        [return: NotNull]
        public Object Deserialize([DisallowNull] Stream stream) =>
            this.Deserialize(stream,
                             -1,
                             out UInt64 _,
                             SerializationFinishAction.None);
        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="InvalidOperationException"/>
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.NonPublicMethods)]
        [return: NotNull]
        public Object Deserialize([DisallowNull] Stream stream,
                                  in Int64 offset) =>
            this.Deserialize(stream,
                             offset,
                             out UInt64 _,
                             SerializationFinishAction.None);
        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="InvalidOperationException"/>
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.NonPublicMethods)]
        [return: NotNull]
        public Object Deserialize([DisallowNull] Stream stream,
                                  out UInt64 read) =>
            this.Deserialize(stream,
                             -1,
                             out read,
                             SerializationFinishAction.None);
        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="InvalidOperationException"/>
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.NonPublicMethods)]
        [return: NotNull]
        public Object Deserialize([DisallowNull] Stream stream,
                                  in Int64 offset,
                                  out UInt64 read) =>
            this.Deserialize(stream,
                             offset,
                             out read,
                             SerializationFinishAction.None);
        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="InvalidOperationException"/>
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.NonPublicMethods)]
        [return: NotNull]
        public Object Deserialize([DisallowNull] Stream stream,
                                  in SerializationFinishAction actionAfter) =>
            this.Deserialize(stream,
                             -1,
                             out UInt64 _,
                             actionAfter);
        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="InvalidOperationException"/>
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.NonPublicMethods)]
        [return: NotNull]
        public Object Deserialize([DisallowNull] Stream stream,
                                  in Int64 offset,
                                  in SerializationFinishAction actionAfter) =>
            this.Deserialize(stream,
                             offset,
                             out UInt64 _,
                             actionAfter);
        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="InvalidOperationException"/>
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.NonPublicMethods)]
        [return: NotNull]
        public Object Deserialize([DisallowNull] Stream stream,
                                  out UInt64 read,
                                  in SerializationFinishAction actionAfter) =>
            this.Deserialize(stream,
                             -1,
                             out read,
                             actionAfter);
        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="InvalidOperationException"/>
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.NonPublicMethods)]
        [return: NotNull]
        public Object Deserialize([DisallowNull] Stream stream,
                                  in Int64 offset,
                                  out UInt64 read,
                                  in SerializationFinishAction actionAfter)
        {
            ExceptionHelpers.ThrowIfNull(stream);
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
            Object result = CreateObject(info);

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
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.NonPublicMethods)]
        public Boolean TryDeserialize([DisallowNull] Stream stream,
                                      [NotNullWhen(true)] out Object? result) =>
            this.TryDeserialize(stream,
                                -1,
                                out UInt64 _,
                                SerializationFinishAction.None,
                                out result);
        /// <inheritdoc/>
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.NonPublicMethods)]
        public Boolean TryDeserialize([DisallowNull] Stream stream,
                                      in Int64 offset,
                                      [NotNullWhen(true)] out Object? result) =>
            this.TryDeserialize(stream,
                                offset,
                                out UInt64 _,
                                SerializationFinishAction.None,
                                out result);
        /// <inheritdoc/>
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.NonPublicMethods)]
        public Boolean TryDeserialize([DisallowNull] Stream stream,
                                      out UInt64 read,
                                      [NotNullWhen(true)] out Object? result) =>
            this.TryDeserialize(stream,
                                -1,
                                out read,
                                SerializationFinishAction.None,
                                out result);
        /// <inheritdoc/>
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.NonPublicMethods)]
        public Boolean TryDeserialize([DisallowNull] Stream stream,
                                      in Int64 offset,
                                      out UInt64 read,
                                      [NotNullWhen(true)] out Object? result) =>
            this.TryDeserialize(stream,
                                offset,
                                out read,
                                SerializationFinishAction.None,
                                out result);
        /// <inheritdoc/>
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.NonPublicMethods)]
        public Boolean TryDeserialize([DisallowNull] Stream stream,
                                      in SerializationFinishAction actionAfter,
                                      [NotNullWhen(true)] out Object? result) =>
            this.TryDeserialize(stream,
                                -1,
                                out UInt64 _,
                                actionAfter,
                                out result);
        /// <inheritdoc/>
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.NonPublicMethods)]
        public Boolean TryDeserialize([DisallowNull] Stream stream,
                                      in Int64 offset,
                                      in SerializationFinishAction actionAfter,
                                      [NotNullWhen(true)] out Object? result) =>
            this.TryDeserialize(stream,
                                offset,
                                out UInt64 _,
                                actionAfter,
                                out result);
        /// <inheritdoc/>
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.NonPublicMethods)]
        public Boolean TryDeserialize([DisallowNull] Stream stream,
                                      out UInt64 read,
                                      in SerializationFinishAction actionAfter,
                                      [NotNullWhen(true)] out Object? result) =>
            this.TryDeserialize(stream,
                                -1,
                                out read,
                                actionAfter,
                                out result);
        /// <inheritdoc/>
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.NonPublicMethods)]
        public Boolean TryDeserialize([DisallowNull] Stream stream,
                                      in Int64 offset,
                                      out UInt64 read,
                                      in SerializationFinishAction actionAfter,
                                      [NotNullWhen(true)] out Object? result)
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
            result = CreateObject(info);

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
    }

    // IUnboundSerializer
    partial class ByteSerializer : IUnboundSerializer
    {
        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="InvalidOperationException"/>
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.NonPublicMethods)]
        public UInt64 Serialize<TAny>(Stream stream,
                                      TAny graph,
                                      Action<TAny, SerializationInfo> getSerializationData) =>
            this.Serialize(stream,
                           graph,
                           getSerializationData,
                           -1,
                           SerializationFinishAction.None);
        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="InvalidOperationException"/>
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.NonPublicMethods)]
        public UInt64 Serialize<TAny>(Stream stream,
                                      TAny graph,
                                      Action<TAny, SerializationInfo> getSerializationData,
                                      in Int64 offset) =>
            this.Serialize(stream,
                           graph,
                           getSerializationData,
                           offset,
                           SerializationFinishAction.None);
        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="InvalidOperationException"/>
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.NonPublicMethods)]
        public UInt64 Serialize<TAny>(Stream stream,
                                      TAny graph,
                                      Action<TAny, SerializationInfo> getSerializationData,
                                      in SerializationFinishAction actionAfter) =>
            this.Serialize(stream,
                           graph,
                           getSerializationData,
                           -1,
                           actionAfter);
        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="InvalidOperationException"/>
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.NonPublicMethods)]
        public UInt64 Serialize<TAny>(Stream stream,
                                      TAny graph,
                                      Action<TAny, SerializationInfo> getSerializationData,
                                      in Int64 offset,
                                      in SerializationFinishAction actionAfter)
        {
            ExceptionHelpers.ThrowIfNull(stream);
            ExceptionHelpers.ThrowIfNull(graph);
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
        public Boolean TrySerialize<TAny>(Stream stream,
                                          TAny graph,
                                          Action<TAny, SerializationInfo> getSerializationData) =>
            this.TrySerialize(stream,
                              graph,
                              getSerializationData,
                              -1,
                              out UInt64 _,
                              SerializationFinishAction.None);
        /// <inheritdoc/>
        public Boolean TrySerialize<TAny>(Stream stream,
                                          TAny graph,
                                          Action<TAny, SerializationInfo> getSerializationData,
                                          in Int64 offset) =>
            this.TrySerialize(stream,
                              graph,
                              getSerializationData,
                              offset,
                              out UInt64 _,
                              SerializationFinishAction.None);
        /// <inheritdoc/>
        public Boolean TrySerialize<TAny>(Stream stream,
                                          TAny graph,
                                          Action<TAny, SerializationInfo> getSerializationData,
                                          out UInt64 written) =>
            this.TrySerialize(stream,
                              graph,
                              getSerializationData,
                              -1,
                              out written,
                              SerializationFinishAction.None);
        /// <inheritdoc/>
        public Boolean TrySerialize<TAny>(Stream stream,
                                          TAny graph,
                                          Action<TAny, SerializationInfo> getSerializationData,
                                          in Int64 offset,
                                          out UInt64 written) =>
            this.TrySerialize(stream,
                              graph,
                              getSerializationData,
                              offset,
                              out written,
                              SerializationFinishAction.None);
        /// <inheritdoc/>
        public Boolean TrySerialize<TAny>(Stream stream,
                                          TAny graph,
                                          Action<TAny, SerializationInfo> getSerializationData,
                                          in SerializationFinishAction actionAfter) =>
            this.TrySerialize(stream,
                              graph,
                              getSerializationData,
                              -1,
                              out UInt64 _,
                              actionAfter);
        /// <inheritdoc/>
        public Boolean TrySerialize<TAny>(Stream stream,
                                          TAny graph,
                                          Action<TAny, SerializationInfo> getSerializationData,
                                          in Int64 offset,
                                          in SerializationFinishAction actionAfter) =>
            this.TrySerialize(stream,
                              graph,
                              getSerializationData,
                              offset,
                              out UInt64 _,
                              actionAfter);
        /// <inheritdoc/>
        public Boolean TrySerialize<TAny>(Stream stream,
                                          TAny graph,
                                          Action<TAny, SerializationInfo> getSerializationData,
                                          out UInt64 written,
                                          in SerializationFinishAction actionAfter) =>
            this.TrySerialize(stream,
                              graph,
                              getSerializationData,
                              -1,
                              out written,
                              actionAfter);
        /// <inheritdoc/>
        public Boolean TrySerialize<TAny>(Stream stream,
                                          TAny graph,
                                          Action<TAny, SerializationInfo> getSerializationData,
                                          in Int64 offset,
                                          out UInt64 written,
                                          in SerializationFinishAction actionAfter)
        {
            if (stream is null ||
                graph is null ||
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
        public TAny Deserialize<TAny>(Stream stream,
                                      Func<SerializationInfo, TAny> constructFromSerializationData) =>
            this.Deserialize(stream,
                             constructFromSerializationData,
                             -1,
                             out UInt64 _,
                             SerializationFinishAction.None);
        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="InvalidOperationException"/>
        public TAny Deserialize<TAny>(Stream stream,
                                      Func<SerializationInfo, TAny> constructFromSerializationData,
                                      in Int64 offset) =>
            this.Deserialize(stream,
                             constructFromSerializationData,
                             offset,
                             out UInt64 _,
                             SerializationFinishAction.None);
        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="InvalidOperationException"/>
        public TAny Deserialize<TAny>(Stream stream,
                                      Func<SerializationInfo, TAny> constructFromSerializationData,
                                      out UInt64 read) =>
            this.Deserialize(stream,
                             constructFromSerializationData,
                             -1,
                             out read,
                             SerializationFinishAction.None);
        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="InvalidOperationException"/>
        public TAny Deserialize<TAny>(Stream stream,
                                      Func<SerializationInfo, TAny> constructFromSerializationData,
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
        public TAny Deserialize<TAny>(Stream stream,
                                      Func<SerializationInfo, TAny> constructFromSerializationData,
                                      in SerializationFinishAction actionAfter) =>
            this.Deserialize(stream,
                             constructFromSerializationData,
                             -1,
                             out UInt64 _,
                             actionAfter);
        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="InvalidOperationException"/>
        public TAny Deserialize<TAny>(Stream stream,
                                      Func<SerializationInfo, TAny> constructFromSerializationData,
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
        public TAny Deserialize<TAny>(Stream stream,
                                      Func<SerializationInfo, TAny> constructFromSerializationData,
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
        public TAny Deserialize<TAny>(Stream stream,
                                      Func<SerializationInfo, TAny> constructFromSerializationData,
                                      in Int64 offset,
                                      out UInt64 read,
                                      in SerializationFinishAction actionAfter)
        {
            ExceptionHelpers.ThrowIfNull(stream);
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
        public Boolean TryDeserialize<TAny>(Stream stream,
                                            Func<SerializationInfo, TAny> constructFromSerializationData,
                                            out TAny? result) =>
            this.TryDeserialize(stream,
                                constructFromSerializationData,
                                -1,
                                out UInt64 _,
                                SerializationFinishAction.None,
                                out result);
        /// <inheritdoc/>
        public Boolean TryDeserialize<TAny>(Stream stream,
                                            Func<SerializationInfo, TAny> constructFromSerializationData,
                                            in Int64 offset,
                                            out TAny? result) =>
            this.TryDeserialize(stream,
                                constructFromSerializationData,
                                offset,
                                out UInt64 _,
                                SerializationFinishAction.None,
                                out result);
        /// <inheritdoc/>
        public Boolean TryDeserialize<TAny>(Stream stream,
                                            Func<SerializationInfo, TAny> constructFromSerializationData,
                                            out UInt64 read,
                                            out TAny? result) =>
            this.TryDeserialize(stream,
                                constructFromSerializationData,
                                -1,
                                out read,
                                SerializationFinishAction.None,
                                out result);
        /// <inheritdoc/>
        public Boolean TryDeserialize<TAny>(Stream stream,
                                            Func<SerializationInfo, TAny> constructFromSerializationData,
                                            in Int64 offset,
                                            out UInt64 read,
                                            out TAny? result) =>
            this.TryDeserialize(stream,
                                constructFromSerializationData,
                                offset,
                                out read,
                                SerializationFinishAction.None,
                                out result);
        /// <inheritdoc/>
        public Boolean TryDeserialize<TAny>(Stream stream,
                                            Func<SerializationInfo, TAny> constructFromSerializationData,
                                            in SerializationFinishAction actionAfter,
                                            out TAny? result) =>
            this.TryDeserialize(stream,
                                constructFromSerializationData,
                                -1,
                                out UInt64 _,
                                actionAfter,
                                out result);
        /// <inheritdoc/>
        public Boolean TryDeserialize<TAny>(Stream stream,
                                            Func<SerializationInfo, TAny> constructFromSerializationData,
                                            in Int64 offset,
                                            in SerializationFinishAction actionAfter,
                                            out TAny? result) =>
            this.TryDeserialize(stream,
                                constructFromSerializationData,
                                offset,
                                out UInt64 _,
                                actionAfter,
                                out result);
        /// <inheritdoc/>
        public Boolean TryDeserialize<TAny>(Stream stream,
                                            Func<SerializationInfo, TAny> constructFromSerializationData,
                                            out UInt64 read,
                                            in SerializationFinishAction actionAfter,
                                            out TAny? result) =>
            this.TryDeserialize(stream,
                                constructFromSerializationData,
                                -1,
                                out read,
                                actionAfter,
                                out result);
        /// <inheritdoc/>
        public Boolean TryDeserialize<TAny>(Stream stream,
                                            Func<SerializationInfo, TAny> constructFromSerializationData,
                                            in Int64 offset,
                                            out UInt64 read,
                                            in SerializationFinishAction actionAfter,
                                            out TAny? result)
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

            return true;
        }
    }
}

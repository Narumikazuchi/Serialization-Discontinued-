namespace Narumikazuchi.Serialization.Bytes;

/// <summary>
/// Base class for the 2 different byte serializer classes, which can't be inherited.
/// </summary>
[Browsable(false)]
[EditorBrowsable(EditorBrowsableState.Never)]
public abstract partial class SharedByteSerializer : IDeclaredSerializer
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
        ExceptionHelpers.ThrowIfArgumentNull(stream);
        ExceptionHelpers.ThrowIfArgumentNull(graph);
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

// Non-Public
partial class SharedByteSerializer : SerializerBase<Byte[]>
{
    private protected SharedByteSerializer() :
        base(__SerializationStrategies.Integrated)
    { }

    private protected SharedByteSerializer(IReadOnlyDictionary<Type, ISerializationStrategy<Byte[]>> strategies) :
        base(strategies)
    { }

    /**
        * FF FF FF FF FF FF FF FF -> Number of bytes for the entire object
        * FF FF FF FF FF FF FF FF -> Number of bytes for the HEAD
        * FF FF FF FF FF FF FF FF -> Number of bytes for the BODY
        * HEAD:
        * FF FF FF FF -> Count of serialized members
        * {
        *   FF FF FF FF FF FF FF FF -> Position/Index in the BODY
        *   FF FF FF FF FF FF FF FF -> Number of bytes in the BODY
        *   FF FF FF FF -> Size of typename String
        *   [FF..] -> Typename String
        *   FF FF FF FF -> Size of name String
        *   [FF..] -> Name String
        * }
        * BODY:
        * {
        *   [FF..] -> Serialized member
        * }
        */
    internal UInt64 SerializeWithInfo(Stream stream,
                                      SerializationInfo info,
                                      __Header header)
    {
#nullable disable
        using MemoryStream body = new();

        foreach (MemberState member in info)
        {
            __HeaderItem item = new()
            {
                Position = body.Position
            };
            if (this._strategies.ContainsKey(member.MemberType))
            {
                this.SerializeWithStrategy(item,
                                           body,
                                           member);
                header.Items.Add(item);
                continue;
            }
            if (member.MemberType.GetInterfaces()
                                 .Any(i => i == typeof(ISerializable)))
            {
                this.SerializeThroughInterface(item,
                                               body,
                                               member);
                header.Items.Add(item);
                continue;
            }
            throw new InvalidOperationException("Type couldn't be serialized. (Are you missing a serialization strategy? Consider implementing the ISerializable interface, if it's a custom class.)");
        }

        body.Position = 0;

        using MemoryStream head = header.AsMemory();

        UInt64 total = 2UL * sizeof(UInt64) + (UInt64)head.Length + (UInt64)body.Length;
        stream.Write(BitConverter.GetBytes(total));
        stream.Write(BitConverter.GetBytes(head.Length));
        stream.Write(BitConverter.GetBytes(body.Length));
        head.CopyTo(stream);
        body.CopyTo(stream);

        total += sizeof(UInt64);

        return total;
#nullable enable
    }

    internal UInt64 SerializeInternal(Stream stream,
                                      ISerializable? graph)
    {
#nullable disable
        if (graph is null)
        {
            stream.Write(BitConverter.GetBytes(2UL * sizeof(UInt64)));
            stream.Write(BitConverter.GetBytes(0UL));
            stream.Write(BitConverter.GetBytes(0UL));
            return 3 * sizeof(UInt64);
        }

        lock (graph)
        {
            __Header header = new(graph.GetType());
            SerializationInfo info = SerializationInfo.Create(graph);
            return this.SerializeWithInfo(stream,
                                          info,
                                          header);
        }
#nullable enable
    }

    internal SerializationInfo DeserializeInternal(Stream stream,
                                                   out UInt64 read)
    {
#pragma warning disable
        read = 0;

        // SIZES
        UInt64 tempRead = 0;
        Int64 sizeofObject = ReadInt64(stream,
                                       out tempRead);
        read += tempRead;

        Int64 sizeofHead = ReadInt64(stream,
                                     out tempRead);
        read += tempRead;

        Int64 sizeofBody = ReadInt64(stream,
                                     out tempRead);
        read += tempRead;

        // NULL
        if (sizeofObject == 2L * sizeof(Int64) &&
            sizeofHead == 0L &&
            sizeofBody == 0L)
        {
            return SerializationInfo.CreateNull();
        }

        // HEAD
        __Header header = __Header.FromStream(stream,
                                              sizeofHead,
                                              out tempRead);
        read += tempRead;
        Type? type = Type.GetType(header.Typename);
        if (type is null)
        {
            throw new FormatException();
        }
        Int64 bodyStart = 3 * sizeof(Int64) + (Int64)sizeofHead;
        SerializationInfo result = SerializationInfo.Create(type);

        for (Int32 i = 0; i < header.MemberCount; i++)
        {
            type = Type.GetType(header.Items[i].Typename);
            if (type is null)
            {
                throw new FormatException();
            }

            Object? member;
            if (this._strategies.ContainsKey(type))
            {
                member = this.DeserializeWithStrategy(stream,
                                                      type,
                                                      bodyStart,
                                                      header.Items[i]);
                result.Set(header.Items[i].Name,
                           member);
                read += Convert.ToUInt64(header.Items[i].Length);
                continue;
            }
            if (type.GetInterfaces()
                    .Any(i => i.GetGenericTypeDefinition() == typeof(ISerializable<>)))
            {
                member = this.DeserializeThroughInterface(stream,
                                                          type,
                                                          bodyStart,
                                                          header.Items[i],
                                                          out tempRead);
                result.Set(header.Items[i].Name,
                           member);
                read += tempRead;
                continue;
            }
            throw new InvalidOperationException("Couldn't deserialize type. (Are you missing a serialization strategy? Consider implementing the ISerializable´1 interface, if it's a custom class.)");
        }

        return result;
#pragma warning restore
    }

    private void SerializeWithStrategy(__HeaderItem item,
                                       Stream body,
                                       MemberState data)
    {
#nullable disable
        Byte[] bytes = this._strategies[data.MemberType].Serialize(data.Value);

        item.Length = bytes.Length;
        item.Typename = data.MemberType.AssemblyQualifiedName;
        item.Name = data.Name;
        body.Write(bytes);
#nullable enable
    }

    private void SerializeThroughInterface(__HeaderItem item,
                                           Stream body,
                                           MemberState data)
    {
#nullable disable
        Type dataTypeSerializer = typeof(ByteSerializer<>).MakeGenericType(data.MemberType);
        ConstructorInfo ctor = dataTypeSerializer.GetConstructor(BindingFlags.Public | BindingFlags.Instance,
                                                                 new Type[] { typeof(IReadOnlyDictionary<Type, ISerializationStrategy<Byte[]>>) });
        Object serializer = ctor.Invoke(new Object[] { this._strategies });
        MethodInfo method = dataTypeSerializer.GetMethod(nameof(this.SerializeInternal),
                                                         BindingFlags.Instance | BindingFlags.NonPublic,
                                                         new Type[] { typeof(Stream), typeof(ISerializable) });
        using MemoryStream temp = new();
        method.Invoke(serializer,
                      new Object[] { temp, data.Value });

        item.Length = temp.Length;
        item.Typename = data.MemberType.AssemblyQualifiedName;
        item.Name = data.Name;
        temp.Position = 0;
        temp.CopyTo(body);
#nullable enable
    }

    private Object? DeserializeWithStrategy(Stream stream,
                                            Type type,
                                            Int64 bodyStart,
                                            __HeaderItem item)
    {
#nullable disable
        stream.Position = bodyStart + (Int64)item.Position;
        Byte[] bytes = new Byte[item.Length];
        Int64 index = 0;
        while (index < item.Length)
        {
            Int32 b = stream.ReadByte();
            if (b == -1)
            {
                // Unexpected end
                throw new IOException();
            }
            bytes[index++] = (Byte)b;
        }
        return this._strategies[type].Deserialize(bytes);
#nullable enable
    }

    private Object? DeserializeThroughInterface(Stream stream,
                                                Type type,
                                                Int64 bodyStart,
                                                __HeaderItem item,
                                                out UInt64 read)
    {
#nullable disable
        stream.Position = bodyStart + (Int64)item.Position;
        using MemoryStream temp = new();
        Int64 index = 0;
        while (index < item.Length)
        {
            Int32 b = stream.ReadByte();
            if (b == -1)
            {
                // Unexpected end
                throw new IOException();
            }
            temp.WriteByte((Byte)b);
            index++;
        }
        temp.Position = 0;
        Type dataTypeSerializer = typeof(ByteSerializer<>).MakeGenericType(type);
        ConstructorInfo ctor = dataTypeSerializer.GetConstructor(BindingFlags.Public | BindingFlags.Instance,
                                                                 new Type[] { typeof(IReadOnlyDictionary<Type, ISerializationStrategy<Byte[]>>) });
        Object serializer = ctor.Invoke(new Object[] { this._strategies });
        MethodInfo method = dataTypeSerializer.GetMethod(nameof(this.DeserializeInternal),
                                                         BindingFlags.Instance | BindingFlags.NonPublic,
                                                         new Type[] { typeof(Stream), typeof(UInt64).MakeByRefType() });

        Object[] parameters = new Object[] { temp, 0UL };
        SerializationInfo info = (SerializationInfo)method.Invoke(serializer,
                                                                   parameters);
        read = (UInt64)parameters[1];

        if (info.IsNull)
        {
            return null;
        }

        method = type.GetMethod("ConstructFromSerializationData",
                                BindingFlags.Static | BindingFlags.Public,
                                new Type[] { typeof(SerializationInfo) });
        return method.Invoke(null,
                             new Object[] { info });
#nullable enable
    }

    private static Int64 ReadInt64(Stream stream,
                                   out UInt64 read)
    {
        Byte[] data = new Byte[sizeof(Int64)];
        stream.Read(data,
                    0,
                    sizeof(Int64));
        read = sizeof(Int64);
        return BitConverter.ToInt64(data);
    }
}
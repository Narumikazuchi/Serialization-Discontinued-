namespace Narumikazuchi.Serialization.Bytes;

/// <summary>
/// Base class for the 2 different byte serializer classes, which can't be inherited.
/// </summary>
[Browsable(false)]
[EditorBrowsable(EditorBrowsableState.Never)]
public abstract partial class SharedByteSerializer
{ }

// Non-Public
partial class SharedByteSerializer : SerializerBase<Byte[]>
{
    private protected SharedByteSerializer(IEnumerable<KeyValuePair<Type, ISerializationStrategy<Byte[]>>> strategies) :
        base(strategies: strategies)
    { }
    private protected SharedByteSerializer(IEnumerable<(Type, ISerializationStrategy<Byte[]>)> strategies) :
        base(strategies: strategies)
    { }
    private protected SharedByteSerializer(IEnumerable<Tuple<Type, ISerializationStrategy<Byte[]>>> strategies) :
        base(strategies: strategies)
    { }

    /**
        * FF FF FF FF FF FF FF FF -> Number of bytes for the entire object
        * FF FF FF FF FF FF FF FF -> Number of bytes for the HEAD
        * FF FF FF FF FF FF FF FF -> Number of bytes for the BODY
        * HEAD:
        * FF -> Is Object Null (Boolean)
        * FF FF FF FF -> Size of typename String
        * [FF..] -> Typename String
        * FF FF FF FF -> Count of serialized members
        * {
        *   FF -> Is Object Null (Boolean)
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
                                      ISerializationInfo info)
    {
        __Header header = new(info);
        using MemoryStream body = new();

        foreach (MemberState member in info)
        {
            __HeaderItem item = new()
            {
                Position = body.Position
            };
            if (this._strategies.ContainsKey(member.MemberType))
            {
                this.SerializeWithStrategy(item: item,
                                           body: body,
                                           data: member);
                header.Items
                      .Add(item);
                continue;
            }
            if (member.MemberType
                      .GetInterfaces()
                      .Any(i => i == typeof(ISerializable)))
            {
                this.SerializeThroughInterface(item: item,
                                               body: body,
                                               data: member);
                header.Items
                      .Add(item);
                continue;
            }
            throw new InvalidOperationException(message: "Type couldn't be serialized. (Are you missing a serialization strategy? Consider implementing the ISerializable interface, if it's a custom class.)");
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
    }

    internal UInt64 SerializeInternal<TSerializable>(Stream stream,
                                                     TSerializable? graph)
        where TSerializable : ISerializable
    {
        ISerializationInfoAdder info;
        if (graph is not null)
        {
            lock (graph)
            {
                info = SerializationInfoBuilder.CreateFrom(from: graph);
                return this.SerializeWithInfo(stream: stream,
                                              info: info);
            }
        }

        info = SerializationInfoBuilder.CreateFrom(from: graph);
        return this.SerializeWithInfo(stream: stream,
                                      info: info);
    }

    internal ISerializationInfoGetter DeserializeInternal(Stream stream,
                                                          out UInt64 read)
    {
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

        // HEAD
        __Header header = __Header.FromStream(source: stream,
                                              size: sizeofHead,
                                              read: out tempRead);
        read += tempRead;

        Type? type = Type.GetType(typeName: header.Typename);
        if (type is null)
        {
            throw new FormatException();
        }

        // NULL
        if (header.IsNull)
        {
            return SerializationInfoBuilder.CreateFrom(type: type,
                                                       isNull: true);
        }

        Int64 bodyStart = 3 * sizeof(UInt64) + sizeofHead;
        ISerializationInfoMutator result = SerializationInfoBuilder.CreateFrom(type: type, 
                                                                               isNull: false);

        for (Int32 i = 0; i < header.MemberCount; i++)
        {
            type = Type.GetType(typeName: header.Items[i]
                                                .Typename);
            if (type is null)
            {
                throw new FormatException();
            }

            Object? member;
            if (this._strategies
                    .ContainsKey(type))
            {
                member = this.DeserializeWithStrategy(stream: stream,
                                                      type: type,
                                                      bodyStart: bodyStart,
                                                      item: header.Items[i]);
                result.Set(memberName: header.Items[i]
                                             .Name,
                           memberValue: member);
                read += Convert.ToUInt64(header.Items[i]
                                               .Length);
                continue;
            }
            if (type.GetInterfaces()
                    .Any(i => i.GetGenericTypeDefinition() == typeof(ISerializable<>)))
            {
                member = this.DeserializeThroughInterface(stream: stream,
                                                          type: type,
                                                          bodyStart: bodyStart,
                                                          item: header.Items[i],
                                                          read: out tempRead);
                result.Set(memberName: header.Items[i]
                                             .Name,
                           memberValue: member);
                read += tempRead;
                continue;
            }
            throw new InvalidOperationException(message: "Couldn't deserialize type. (Are you missing a serialization strategy? Consider implementing the ISerializable´1 interface, if it's a custom class.)");
        }

        return result;
    }

    private void SerializeWithStrategy(__HeaderItem item,
                                       Stream body,
                                       MemberState data)
    {
        item.Typename = data.MemberType
                            .AssemblyQualifiedName!;
        item.Name = data.Name;

        if (data.Value is null)
        {
            item.Length = 0;
            item.IsNull = true;
            body.Write(Array.Empty<Byte>());
            return;
        }
        Byte[] bytes = this._strategies[data.MemberType]
                            .Serialize(data.Value);

        item.Length = bytes.Length;
        body.Write(bytes);
    }

    private void SerializeThroughInterface(__HeaderItem item,
                                           Stream body,
                                           MemberState data)
    {
        MethodInfo method = typeof(SharedByteSerializer).GetMethod(name: nameof(this.SerializeInternal),
                                                                   bindingAttr: BindingFlags.Instance | BindingFlags.NonPublic)!
                                                        .MakeGenericMethod(data.MemberType);
        using MemoryStream temp = new();
        method.Invoke(obj: this,
                      parameters: new Object?[] { temp, data.Value });

        item.Length = temp.Length;
        item.Typename = data.MemberType
                            .AssemblyQualifiedName!;
        item.Name = data.Name;
        temp.Position = 0;
        temp.CopyTo(body);
    }

    private Object? DeserializeWithStrategy(Stream stream,
                                            Type type,
                                            Int64 bodyStart,
                                            __HeaderItem item)
    {
        stream.Position = bodyStart + item.Position;
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
        return this._strategies[type]
                   .Deserialize(bytes);
    }

    private Object? DeserializeThroughInterface(Stream stream,
                                                Type type,
                                                Int64 bodyStart,
                                                __HeaderItem item,
                                                out UInt64 read)
    {
        stream.Position = bodyStart + item.Position;
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
        ConstructorInfo ctor = dataTypeSerializer.GetConstructor(bindingAttr: BindingFlags.Public | BindingFlags.Instance,
                                                                 types: new Type[] { typeof(IEnumerable<KeyValuePair<Type, ISerializationStrategy<Byte[]>>>) })!;
        Object serializer = ctor.Invoke(new Object[] { this._strategies });
        MethodInfo method = dataTypeSerializer.GetMethod(name: nameof(this.DeserializeInternal),
                                                         bindingAttr: BindingFlags.Instance | BindingFlags.NonPublic,
                                                         types: new Type[] { typeof(Stream), typeof(UInt64).MakeByRefType() })!;

        Object[] parameters = new Object[] { temp, 0UL };
        ISerializationInfoMutator info = (ISerializationInfoMutator)method.Invoke(obj: serializer,
                                                                                  parameters: parameters)!;
        read = (UInt64)parameters[1];

        if (info.IsNull)
        {
            return null;
        }

        method = type.GetMethod(name: "ConstructFromSerializationData",
                                bindingAttr: BindingFlags.Static | BindingFlags.Public,
                                types: new Type[] { typeof(ISerializationInfoGetter) })!;
        return method.Invoke(obj: null,
                             parameters: new Object[] { info });
    }

    private static Int64 ReadInt64(Stream stream,
                                   out UInt64 read)
    {
        Byte[] data = new Byte[sizeof(Int64)];
        stream.Read(buffer: data,
                    offset: 0,
                    count: sizeof(Int64));
        read = sizeof(Int64);
        return BitConverter.ToInt64(value: data);
    }
}

// IDeclaredSerializer
partial class SharedByteSerializer : IDeclaredSerializer
{
    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="InvalidOperationException"/>
    public UInt64 Serialize<TSerializable>([DisallowNull] Stream stream,
                                           [AllowNull] TSerializable? graph)
        where TSerializable : ISerializable =>
            this.Serialize(stream: stream,
                           graph: graph,
                           offset: -1,
                           actionAfter: SerializationFinishAction.None);
    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="InvalidOperationException"/>
    public UInt64 Serialize<TSerializable>([DisallowNull] Stream stream,
                                           [AllowNull] TSerializable? graph,
                                           in Int64 offset)
        where TSerializable : ISerializable =>
            this.Serialize(stream: stream,
                           graph: graph,
                           offset: offset,
                           actionAfter: SerializationFinishAction.None);
    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="InvalidOperationException"/>
    public UInt64 Serialize<TSerializable>([DisallowNull] Stream stream,
                                           [AllowNull] TSerializable? graph,
                                           in SerializationFinishAction actionAfter)
        where TSerializable : ISerializable =>
            this.Serialize(stream: stream,
                           graph: graph,
                           offset: -1,
                           actionAfter: actionAfter);
    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="InvalidOperationException"/>
    public UInt64 Serialize<TSerializable>([DisallowNull] Stream stream,
                                           [AllowNull] TSerializable? graph,
                                           in Int64 offset,
                                           in SerializationFinishAction actionAfter)
        where TSerializable : ISerializable
    {
        ExceptionHelpers.ThrowIfArgumentNull(stream);
        ExceptionHelpers.ThrowIfArgumentNull(graph);
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
        UInt64 result = this.SerializeInternal(stream: stream,
                                               graph: graph);

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
    public Boolean TrySerialize<TSerializable>([DisallowNull] Stream stream,
                                               [AllowNull] TSerializable? graph)
        where TSerializable : ISerializable =>
            this.TrySerialize(stream: stream,
                              graph: graph,
                              offset: -1,
                              written: out UInt64 _,
                              actionAfter: SerializationFinishAction.None);
    /// <inheritdoc/>
    public Boolean TrySerialize<TSerializable>([DisallowNull] Stream stream,
                                               [AllowNull] TSerializable? graph,
                                               in Int64 offset)
        where TSerializable : ISerializable =>
            this.TrySerialize(stream: stream,
                              graph: graph,
                              offset: offset,
                              written: out UInt64 _,
                              actionAfter: SerializationFinishAction.None);
    /// <inheritdoc/>
    public Boolean TrySerialize<TSerializable>([DisallowNull] Stream stream,
                                               [AllowNull] TSerializable? graph,
                                               out UInt64 written)
        where TSerializable : ISerializable =>
            this.TrySerialize(stream: stream,
                              graph: graph,
                              offset: -1,
                              written: out written,
                              actionAfter: SerializationFinishAction.None);
    /// <inheritdoc/>
    public Boolean TrySerialize<TSerializable>([DisallowNull] Stream stream,
                                               [AllowNull] TSerializable? graph,
                                               in Int64 offset,
                                               out UInt64 written)
        where TSerializable : ISerializable =>
            this.TrySerialize(stream: stream,
                              graph: graph,
                              offset: offset,
                              written: out written,
                              actionAfter: SerializationFinishAction.None);
    /// <inheritdoc/>
    public Boolean TrySerialize<TSerializable>([DisallowNull] Stream stream,
                                               [AllowNull] TSerializable? graph,
                                               in SerializationFinishAction actionAfter)
        where TSerializable : ISerializable =>
            this.TrySerialize(stream: stream,
                              graph: graph,
                              offset: -1,
                              written: out UInt64 _,
                              actionAfter: actionAfter);
    /// <inheritdoc/>
    public Boolean TrySerialize<TSerializable>([DisallowNull] Stream stream,
                                               [AllowNull] TSerializable? graph,
                                               in Int64 offset,
                                               in SerializationFinishAction actionAfter)
        where TSerializable : ISerializable =>
            this.TrySerialize(stream: stream,
                              graph: graph,
                              offset: offset,
                              written: out UInt64 _,
                              actionAfter: actionAfter);
    /// <inheritdoc/>
    public Boolean TrySerialize<TSerializable>([DisallowNull] Stream stream,
                                               [AllowNull] TSerializable? graph,
                                               out UInt64 written,
                                               in SerializationFinishAction actionAfter)
        where TSerializable : ISerializable =>
            this.TrySerialize(stream: stream,
                              graph: graph,
                              offset: -1,
                              written: out written,
                              actionAfter: actionAfter);
    /// <inheritdoc/>
    public Boolean TrySerialize<TSerializable>([DisallowNull] Stream stream,
                                               [AllowNull] TSerializable? graph,
                                               in Int64 offset,
                                               out UInt64 written,
                                               in SerializationFinishAction actionAfter)
        where TSerializable : ISerializable
    {
        if (stream is null ||
            graph is null ||
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
        written = this.SerializeInternal(stream: stream,
                                         graph: graph);

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
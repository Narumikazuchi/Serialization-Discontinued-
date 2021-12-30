namespace Narumikazuchi.Serialization.Bytes;

internal sealed partial class __ByteSerializer<TSerializable>
{
    public __ByteSerializer(__BuildInformation<TSerializable> info)
    {
        ExceptionHelpers.ThrowIfArgumentNull(info);

        foreach (KeyValuePair<Type, ISerializationStrategy<Byte[]>> strategy in info.Strategies)
        {
            this._strategies
                .Add(item: strategy);
        }
        this._dataGetter = info.DataGetter;
        this._dataSetter = info.DataSetter;
    }
}

// Non-Public
partial class __ByteSerializer<TSerializable> : IByteSerializerDeserializer<TSerializable>
{
    private UInt64 SerializeInternal(Stream stream,
                                     TSerializable? graph)
    {
        ISerializationInfoAdder info;
        if (this._dataGetter is not null)
        {
            info = CreateSerializationInfo.From(from: graph,
                                                write: this._dataGetter);
            return this.SerializeWithInfo(stream: stream,
                                          info: info);
        }

        if (graph is null)
        {
            info = CreateSerializationInfo.From(type: typeof(TSerializable),
                                                isNull: true);
            return this.SerializeWithInfo(stream: stream,
                                          info: info);
        }

        Type type = graph.GetType();
        if (graph is not ISerializable serializable)
        {
            foreach (KeyValuePair<Type, ISerializationStrategy<Byte[]>> kv in this._strategies)
            {
                if (kv.Key == type)
                {
                    return this.SerializeWithStrategy(stream: stream,
                                                      graph: graph,
                                                      type: type,
                                                      strategy: kv.Value);
                }
            }
            InvalidOperationException exception = new(message: COULD_NOT_SERIALIZE_TYPE);
            exception.Data
                     .Add(key: "Typename",
                          value: type.FullName);
            throw exception;
        }
        info = CreateSerializationInfo.From(from: serializable);
        return this.SerializeWithInfo(stream: stream,
                                        info: info);
    }

    private UInt64 SerializeWithInfo(Stream stream,
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
            if (this._strategies
                    .ContainsKey(member.MemberType))
            {
                this.SerializeWithStrategy(stream: body,
                                           item: item,
                                           data: member);
                header.Items
                      .Add(item);
                continue;
            }
            if (member.MemberType
                      .GetInterfaces()
                      .Any(i => i == typeof(ISerializable)))
            {
                if (member.Value is ISerializable serializable)
                {
                    this.SerializeThroughInterface(stream: body,
                                                   item: item,
                                                   data: member,
                                                   value: serializable);
                    header.Items
                          .Add(item);
                    continue;
                }
                else if (member.Value is null)
                {
                    item.Length = 0;
                    item.Typename = member.MemberType
                                          .AssemblyQualifiedName!;
                    item.Name = member.Name;
                    item.IsNull = true;
                    header.Items
                          .Add(item);
                    continue;
                }
            }

            InvalidOperationException exception = new(message: COULD_NOT_SERIALIZE_TYPE);
            exception.Data
                     .Add(key: "Typename",
                          value: member.MemberType
                                       .FullName);
            throw exception;
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

    private UInt64 SerializeWithStrategy(Stream stream,
                                         TSerializable? graph,
                                         Type type,
                                         ISerializationStrategy<Byte[]> strategy)
    {

        __Header header = new(type: type,
                              value: graph);
        using MemoryStream head = header.AsMemory();

        using MemoryStream body = new();
        body.Write(buffer: strategy.Serialize(graph));
        body.Position = 0;

        UInt64 total = 2UL * sizeof(UInt64) + (UInt64)head.Length + (UInt64)body.Length;
        stream.Write(BitConverter.GetBytes(total));
        stream.Write(BitConverter.GetBytes(head.Length));
        stream.Write(BitConverter.GetBytes(body.Length));
        head.CopyTo(stream);
        body.CopyTo(stream);

        total += sizeof(UInt64);

        return total;
    }

    private void SerializeWithStrategy(Stream stream,
                                       __HeaderItem item,
                                       MemberState data)
    {
        item.Typename = data.MemberType
                            .AssemblyQualifiedName!;
        item.Name = data.Name;

        if (data.Value is null)
        {
            item.Length = 0;
            item.IsNull = true;
            return;
        }

        Byte[] bytes = this._strategies[data.MemberType]
                            .Serialize(data.Value);

        item.Length = bytes.Length;
        stream.Write(bytes);
    }

    private void SerializeThroughInterface(Stream stream,
                                           __HeaderItem item,
                                           MemberState data,
                                           ISerializable value)
    {
        IByteSerializer<ISerializable> serializer;

        if (__Cache.CreatedOwnedSerializers
                   .ContainsKey(data.MemberType))
        {
            serializer = __Cache.CreatedOwnedSerializers[data.MemberType][0];
        }
        else
        {
            serializer = CreateByteSerializer
                        .ForSerialization()
                        .ConfigureForOwnedType<ISerializable>()
                        .UseStrategies(this._strategies)
                        .Construct();
            __Cache.CreatedOwnedSerializers
                   .Add(key: data.MemberType,
                        value: new List<IByteSerializer<ISerializable>>
                        {
                            serializer
                        });
        }

        using MemoryStream temp = new();
        serializer.Serialize(stream: temp, 
                             graph: value);
        item.Length = temp.Length;
        item.Typename = data.MemberType
                            .AssemblyQualifiedName!;
        item.Name = data.Name;
        temp.Position = 0;
        temp.CopyTo(stream);
    }

    private TSerializable? DeserializeInternal(Stream stream,
                                               out UInt64 read)
    {
        read = 0;

        // Segment sizes
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

        // Read header
        __Header header = __Header.FromStream(source: stream,
                                              size: sizeofHead,
                                              read: out tempRead);
        read += tempRead;

        // Serialized type
        Type? type = Type.GetType(typeName: header.Typename);
        if (type is null ||
            !type.IsAssignableTo(typeof(TSerializable)))
        {
            throw new FormatException();
        }

        // Is null
        if (header.IsNull)
        {
            return (TSerializable?)(Object?)null;
        }

        Int64 bodyStart = 3 * sizeof(UInt64) + sizeofHead;

        // Strategy-approach
        if (this._strategies
                .ContainsKey(type))
        {
            Byte[] body = new Byte[sizeofBody];
            stream.Read(buffer: body,
                        offset: 0,
                        count: (Int32)sizeofBody);
            return (TSerializable?)this._strategies[type]
                                       .Deserialize(body);
        }

        // No strategy means we do it ourselves
        ISerializationInfoMutator info = CreateSerializationInfo.From(type: type,
                                                                      isNull: false);

        InvalidOperationException exception;

        for (Int32 i = 0; i < header.MemberCount; i++)
        {
            if (this.DeserializeMember(stream: stream,
                                       item: header.Items[i],
                                       info: info,
                                       bodyStart: bodyStart,
                                       read: ref read))
            {
                continue;
            }

            exception = new(message: COULD_NOT_DESERIALIZE_TYPE);
            exception.Data
                     .Add(key: "Typename",
                          value: header.Items[i]
                                       .Typename);
            throw exception;
        }

        // Do we have a setter?
        if (this._dataSetter is not null)
        {
            return this._dataSetter.Invoke(info);
        }

        // Do we implement the interface?
        if (type.GetInterfaces()
                .Any(i => i.GetGenericTypeDefinition() == typeof(IDeserializable<>)))
        {
            MethodInfo method = this.GetType()
                                    .GetMethod(name: nameof(CreateObject),
                                               bindingAttr: BindingFlags.Static | BindingFlags.NonPublic)!
                                    .MakeGenericMethod(type);
            return (TSerializable?)method.Invoke(obj: this,
                                                 parameters: new Object[] { info });
        }

        exception = new(message: COULD_NOT_DESERIALIZE_TYPE);
        exception.Data
                 .Add(key: "Typename",
                      value: type.FullName);
        throw exception;
    }

    private Boolean DeserializeMember(Stream stream,
                                      __HeaderItem item,
                                      ISerializationInfoMutator info,
                                      Int64 bodyStart,
                                      ref UInt64 read)
    {
        Type? type = Type.GetType(typeName: item.Typename);
        if (type is null)
        {
            throw new FormatException();
        }

        Object? member;
        UInt64 tempRead = 0;

        // Length = 0 means the member was null during serialization
        if (item.Length == 0)
        {
            info.SetState(memberName: item.Name,
                          memberValue: (Object?)null);
            return true;
        }

        // Attempting through known strategies
        if (this._strategies
                .ContainsKey(type))
        {
            member = this.DeserializeWithStrategy(stream: stream,
                                                  type: type,
                                                  bodyStart: bodyStart,
                                                  item: item);
            info.SetState(memberName: item.Name,
                          memberValue: member);
            read += Convert.ToUInt64(item.Length);
            return true;
        }

        // Attempting through implemented interface
        if (type.GetInterfaces()
                .Any(i => i.GetGenericTypeDefinition() == typeof(IDeserializable<>)))
        {
            MethodInfo method;
            if (_usedInterfaceDeserializations.ContainsKey(type))
            {
                method = _usedInterfaceDeserializations[type];
            }
            else
            {
                method = this.GetType()
                             .GetMethod(name: nameof(DeserializeThroughInterface),
                                        bindingAttr: BindingFlags.Instance | BindingFlags.NonPublic,
                                        types: new Type[] { typeof(Stream), typeof(Int64), typeof(__HeaderItem), typeof(UInt64).MakeByRefType() })!
                             .MakeGenericMethod(type);
                _usedInterfaceDeserializations.Add(key: type,
                                                   value: method);
            }

            Object?[] parameters = new Object?[] { stream, bodyStart, item, 0UL };
            member = method.Invoke(obj: this, 
                                   parameters: parameters);
            tempRead = (UInt64)parameters[3]!;

            info.SetState(memberName: item.Name,
                          memberValue: member);
            read += tempRead;
            return true;
        }
        return false;
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

    private TResult? DeserializeThroughInterface<TResult>(Stream stream,
                                                          Int64 bodyStart,
                                                          __HeaderItem item,
                                                          out UInt64 read)
        where TResult : IDeserializable<TResult>
    {
        IByteDeserializer<TResult> deserializer;

        if (__Cache.CreatedDeserializers
                   .ContainsKey(typeof(TResult)))
        {
            deserializer = (IByteDeserializer<TResult>)__Cache.CreatedDeserializers[typeof(TResult)][0];
        }
        else
        {
            deserializer = CreateByteSerializer
                          .ForDeserialization()
                          .ConfigureForOwnedType<TResult>()
                          .UseStrategies(this._strategies)
                          .Construct();
        }

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

        return deserializer.Deserialize(stream: temp, 
                                        read: out read);
    }

    private static TResult CreateObject<TResult>(ISerializationInfoGetter info)
        where TResult : IDeserializable<TResult> =>
            TResult.ConstructFromSerializationData(info);

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

    internal Action<TSerializable, ISerializationInfoAdder>? DataGetter =>
        this._dataGetter;

    internal Func<ISerializationInfoGetter, TSerializable>? DataSetter =>
        this._dataSetter;

    internal IDictionary<Type, ISerializationStrategy<Byte[]>> Strategies =>
        this._strategies;

    private static readonly IDictionary<Type, MethodInfo> _usedInterfaceDeserializations = new Dictionary<Type, MethodInfo>();
    private readonly IDictionary<Type, ISerializationStrategy<Byte[]>> _strategies = new Dictionary<Type, ISerializationStrategy<Byte[]>>();
    private readonly Action<TSerializable, ISerializationInfoAdder>? _dataGetter;
    private readonly Func<ISerializationInfoGetter, TSerializable>? _dataSetter;

#pragma warning disable IDE1006
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private const String STREAM_DOES_NOT_SUPPORT_READING = "The specified stream does not support reading.";
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private const String STREAM_DOES_NOT_SUPPORT_WRITING = "The specified stream does not support writing.";
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private const String STREAM_DOES_NOT_SUPPORT_SEEKING = "The specified stream does not support seeking.";
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private const String COULD_NOT_SERIALIZE_TYPE = "Couldn't serialize type. (Are you missing a serialization strategy? Consider implementing the ISerializable´1 interface, if it's a type you own.)";
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private const String COULD_NOT_DESERIALIZE_TYPE = "Couldn't deserialize type. (Are you missing a serialization strategy? Consider implementing the ISerializable´1 interface, if it's a type you own.)";
#pragma warning restore
}

// IByteDeserializer<T>
partial class __ByteSerializer<TSerializable> : IByteDeserializer<TSerializable>
{
    [return: MaybeNull]
    public TSerializable? Deserialize([DisallowNull] Stream stream) =>
        this.Deserialize(stream: stream,
                         offset: -1,
                         read: out UInt64 _,
                         actionAfter: SerializationFinishAction.None);
    [return: MaybeNull]
    public TSerializable? Deserialize([DisallowNull] Stream stream, 
                                      in Int64 offset) =>
        this.Deserialize(stream: stream,
                         offset: offset,
                         read: out UInt64 _,
                         actionAfter: SerializationFinishAction.None);
    [return: MaybeNull]
    public TSerializable? Deserialize([DisallowNull] Stream stream, 
                                      out UInt64 read) =>
        this.Deserialize(stream: stream,
                         offset: -1,
                         read: out read,
                         actionAfter: SerializationFinishAction.None);
    [return: MaybeNull]
    public TSerializable? Deserialize([DisallowNull] Stream stream, 
                                      in Int64 offset, 
                                      out UInt64 read) =>
        this.Deserialize(stream: stream,
                         offset: offset,
                         read: out read,
                         actionAfter: SerializationFinishAction.None);
    [return: MaybeNull]
    public TSerializable? Deserialize([DisallowNull] Stream stream, 
                                      in SerializationFinishAction actionAfter) =>
        this.Deserialize(stream: stream,
                         offset: -1,
                         read: out UInt64 _,
                         actionAfter: actionAfter);
    [return: MaybeNull]
    public TSerializable? Deserialize([DisallowNull] Stream stream, 
                                      in Int64 offset, 
                                      in SerializationFinishAction actionAfter) =>
        this.Deserialize(stream: stream,
                         offset: offset,
                         read: out UInt64 _,
                         actionAfter: actionAfter);
    [return: MaybeNull]
    public TSerializable? Deserialize([DisallowNull] Stream stream, 
                                      out UInt64 read, 
                                      in SerializationFinishAction actionAfter) =>
        this.Deserialize(stream: stream,
                         offset: -1,
                         read: out read,
                         actionAfter: actionAfter);
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

        TSerializable? result = this.DeserializeInternal(stream: stream,
                                                         read: out read);

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

    public Boolean TryDeserialize([DisallowNull] Stream stream, 
                                  [AllowNull] out TSerializable? result) =>
        this.TryDeserialize(stream: stream,
                            offset: -1,
                            read: out UInt64 _,
                            actionAfter: SerializationFinishAction.None,
                            result: out result);
    public Boolean TryDeserialize([DisallowNull] Stream stream, 
                                  in Int64 offset, 
                                  [AllowNull] out TSerializable? result) =>
        this.TryDeserialize(stream: stream,
                            offset: offset,
                            read: out UInt64 _,
                            actionAfter: SerializationFinishAction.None,
                            result: out result);
    public Boolean TryDeserialize([DisallowNull] Stream stream, 
                                  out UInt64 read, 
                                  [AllowNull] out TSerializable? result) =>
        this.TryDeserialize(stream: stream,
                            offset: -1,
                            read: out read,
                            actionAfter: SerializationFinishAction.None,
                            result: out result);
    public Boolean TryDeserialize([DisallowNull] Stream stream, 
                                  in Int64 offset, 
                                  out UInt64 read, 
                                  [AllowNull] out TSerializable? result) =>
        this.TryDeserialize(stream: stream,
                            offset: offset,
                            read: out read,
                            actionAfter: SerializationFinishAction.None,
                            result: out result);
    public Boolean TryDeserialize([DisallowNull] Stream stream, 
                                  in SerializationFinishAction actionAfter, 
                                  [AllowNull] out TSerializable? result) =>
        this.TryDeserialize(stream: stream,
                            offset: -1,
                            read: out UInt64 _,
                            actionAfter: actionAfter,
                            result: out result);
    public Boolean TryDeserialize([DisallowNull] Stream stream, 
                                  in Int64 offset, 
                                  in SerializationFinishAction actionAfter, 
                                  [AllowNull] out TSerializable? result) =>
        this.TryDeserialize(stream: stream,
                            offset: offset,
                            read: out UInt64 _,
                            actionAfter: actionAfter,
                            result: out result);
    public Boolean TryDeserialize([DisallowNull] Stream stream, 
                                  out UInt64 read, 
                                  in SerializationFinishAction actionAfter, 
                                  [AllowNull] out TSerializable? result) =>
        this.TryDeserialize(stream: stream,
                            offset: -1,
                            read: out read,
                            actionAfter: actionAfter,
                            result: out result);
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

        result = this.DeserializeInternal(stream: stream,
                                          read: out read);

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

// IByteSerializer<T>
partial class __ByteSerializer<TSerializable> : IByteSerializer<TSerializable>
{
    public UInt64 Serialize([DisallowNull] Stream stream, 
                            [AllowNull] TSerializable? graph) =>
            this.Serialize(stream: stream,
                           graph: graph,
                           offset: -1,
                           actionAfter: SerializationFinishAction.None);
    public UInt64 Serialize([DisallowNull] Stream stream, 
                            [AllowNull] TSerializable? graph, 
                            in Int64 offset) =>
            this.Serialize(stream: stream,
                           graph: graph,
                           offset: offset,
                           actionAfter: SerializationFinishAction.None);
    public UInt64 Serialize([DisallowNull] Stream stream, 
                            [AllowNull] TSerializable? graph, 
                            in SerializationFinishAction actionAfter) =>
            this.Serialize(stream: stream,
                           graph: graph,
                           offset: -1,
                           actionAfter: actionAfter);
    public UInt64 Serialize([DisallowNull] Stream stream, 
                            [AllowNull] TSerializable? graph, 
                            in Int64 offset, 
                            in SerializationFinishAction actionAfter)
    {
        ExceptionHelpers.ThrowIfArgumentNull(stream);
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

    public Boolean TrySerialize([DisallowNull] Stream stream, 
                                [AllowNull] TSerializable? graph) =>
            this.TrySerialize(stream: stream,
                              graph: graph,
                              offset: -1,
                              written: out UInt64 _,
                              actionAfter: SerializationFinishAction.None);
    public Boolean TrySerialize([DisallowNull] Stream stream, 
                                [AllowNull] TSerializable? graph, 
                                in Int64 offset) =>
            this.TrySerialize(stream: stream,
                              graph: graph,
                              offset: offset,
                              written: out UInt64 _,
                              actionAfter: SerializationFinishAction.None);
    public Boolean TrySerialize([DisallowNull] Stream stream, 
                                [AllowNull] TSerializable? graph, 
                                out UInt64 written) =>
            this.TrySerialize(stream: stream,
                              graph: graph,
                              offset: -1,
                              written: out written,
                              actionAfter: SerializationFinishAction.None);
    public Boolean TrySerialize([DisallowNull] Stream stream, 
                                [AllowNull] TSerializable graph, 
                                in Int64 offset, 
                                out UInt64 written) =>
            this.TrySerialize(stream: stream,
                              graph: graph,
                              offset: offset,
                              written: out written,
                              actionAfter: SerializationFinishAction.None);
    public Boolean TrySerialize([DisallowNull] Stream stream, 
                                [AllowNull] TSerializable? graph, 
                                in SerializationFinishAction actionAfter) =>
            this.TrySerialize(stream: stream,
                              graph: graph,
                              offset: -1,
                              written: out UInt64 _,
                              actionAfter: actionAfter);
    public Boolean TrySerialize([DisallowNull] Stream stream, 
                                [AllowNull] TSerializable? graph, 
                                in Int64 offset, 
                                in SerializationFinishAction actionAfter) =>
            this.TrySerialize(stream: stream,
                              graph: graph,
                              offset: offset,
                              written: out UInt64 _,
                              actionAfter: actionAfter);
    public Boolean TrySerialize([DisallowNull] Stream stream, 
                                [AllowNull] TSerializable? graph, 
                                out UInt64 written, 
                                in SerializationFinishAction actionAfter) =>
            this.TrySerialize(stream: stream,
                              graph: graph,
                              offset: -1,
                              written: out written,
                              actionAfter: actionAfter);
    public Boolean TrySerialize([DisallowNull] Stream stream, 
                                [AllowNull] TSerializable? graph, 
                                in Int64 offset, 
                                out UInt64 written, 
                                in SerializationFinishAction actionAfter)
    {
        if (stream is null ||
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

// IUsesSerializationStrategies
partial class __ByteSerializer<TSerializable> : IUsesSerializationStrategies
{
    public IEnumerable<Type> RegisteredStrategies =>
        this._strategies
            .Keys;
}
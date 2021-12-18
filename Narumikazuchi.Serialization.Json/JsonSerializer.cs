namespace Narumikazuchi.Serialization.Json;

/// <summary>
/// A serializer for classes that implement <see cref="ISerializable{TSelf}"/>.
/// </summary>
public sealed partial class JsonSerializer<TSerializable> : SerializerBase<JsonElement>
    where TSerializable : ISerializable<TSerializable>
{
    /// <summary>
    /// Instantiates a new instance of the <see cref="JsonSerializer{TSerializable}"/> class.
    /// </summary>
    /// <exception cref="InvalidOperationException"/>
    public JsonSerializer() :
        base(__SerializationStrategies.Integrated)
    { }
    /// <summary>
    /// Instantiates a new instance of the <see cref="JsonSerializer{TSerializable}"/> class.
    /// </summary>
    /// <exception cref="InvalidOperationException"/>
    public JsonSerializer([DisallowNull] IReadOnlyDictionary<Type, ISerializationStrategy<JsonElement>> strategies) :
        base(strategies)
    { }
}

// Non-Public
partial class JsonSerializer<TSerializable>
{
    private UInt64 SerializeInternal(Stream stream,
                                     ISerializable? graph)
    {
#pragma warning disable
        Byte[] data;
        if (graph is null)
        {
            data = Encoding.UTF8.GetBytes("null");
            stream.Write(data);
            return (UInt64)data.Length;
        }

        JsonObject jsonObject = this.SerializeAsObject(graph);
        String json = jsonObject.ToString();
        Int64 start = stream.Position;
        using StreamWriter writer = new(stream, 
                                        Encoding.UTF8);
        writer.Write(json);
        return (UInt64)(stream.Position - start);
#pragma warning restore
    }

    private SerializationInfo DeserializeInternal(Stream stream,
                                                  out UInt64 read)
    {
#pragma warning disable
        using StreamReader reader = new(stream);
        read = 0;
        Int32 depth = 0;
        String? line;
        StringBuilder json = new();
        while ((line = reader.ReadLine()) != null)
        {
            Boolean cancel = false;
            for (Int32 i = 0; i < line.Length; i++)
            {
                json.Append(line[i]);
                read += (UInt64)Encoding.UTF8.GetBytes(line[i].ToString())
                                             .Length;
                if (line[i] == '{')
                {
                    depth++;
                    continue;
                }
                if (line[i] == '}')
                {
                    depth--;
                }
                if (depth == 0)
                {
                    cancel = true;
                    if (i < line.Length - 1)
                    {
                        stream.Position -= Encoding.UTF8.GetBytes(line[(i + 1)..])
                                                        .Length;
                    }
                    break;
                }
            }
            if (cancel)
            {
                break;
            }
        }

        JsonObject jsonObject = JsonObject.FromJsonString(json.ToString());
        if (!jsonObject.HasMember(TYPENAME))
        {
            // Needs typename to create object
            throw new FormatException();
        }
        Type? type = Type.GetType((String)jsonObject[TYPENAME]);
        if (type is null)
        {
            throw new InvalidOperationException();
        }
        SerializationInfo result = SerializationInfo.Create(type);
        this.DeserializeInformation(jsonObject,
                                    result);
        return result;
#pragma warning restore
    }

    private JsonObject? SerializeAsObject(ISerializable? graph)
    {
        if (graph is null)
        {
            return null;
        }

        SerializationInfo info = SerializationInfo.Create(graph);
        return this.SerializeWithInfo(info);
    }

    private JsonObject SerializeWithInfo(SerializationInfo info)
    {
#pragma warning disable
        JsonObject result = new();
        result.Add(TYPENAME,
                   new __JsonElement<String?>(info.Type.AssemblyQualifiedName));
        foreach (MemberState member in info)
        {
            Boolean serialized = false;
            foreach (Type key in this.RegisteredStrategies)
            {
                if (member.MemberType.IsAssignableTo(key))
                {
                    result.Add(member.Name,
                               this._strategies[key].Serialize(member.Value));
                    serialized = true;
                    break;
                }
            }
            if (serialized)
            {
                continue;
            }
            if (member.MemberType.IsAssignableTo(typeof(ISerializable)))
            {
                result.Add(member.Name,
                           this.SerializeAsObject((ISerializable)member.Value));
                continue;
            }
            // No strategy available, try more abstract, that is System.Object
            throw new NotSupportedException();
        }
        return result;
#pragma warning restore
    }

    private Object? DeserializeThroughInterface(JsonObject jsonObject,
                                                Type type)
    {
#nullable disable
        String json = jsonObject.ToString();
        Byte[] data = Encoding.UTF8.GetBytes(json);
        using MemoryStream temp = new(data);

        Type dataTypeSerializer = typeof(JsonSerializer<>).MakeGenericType(type);
        ConstructorInfo ctor = dataTypeSerializer.GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance,
                                                                 new Type[] { typeof(IReadOnlyDictionary<Type, ISerializationStrategy<Byte[]>>) });
        Object serializer = ctor.Invoke(new Object[] { this._strategies });
        MethodInfo method = dataTypeSerializer.GetMethod(nameof(this.DeserializeInternal),
                                                         BindingFlags.Instance | BindingFlags.NonPublic,
                                                         new Type[] { typeof(Stream), typeof(UInt64).MakeByRefType() });

        Object[] parameters = new Object[] { temp, 0UL };
        SerializationInfo info = (SerializationInfo)method.Invoke(serializer,
                                                                  parameters);

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

    private IEnumerable TransformJsonArray(JsonArray jsonArray)
    {
        List<Object?> elements = new();
        foreach (JsonElement? element in jsonArray)
        {
            if (element is null)
            {
                elements.Add(null);
                continue;
            }
            if (element.IsArray)
            {
                elements.Add(this.TransformJsonArray((JsonArray)element));
                continue;
            }
            if (element.IsBoolean)
            {
                elements.Add((Boolean)element);
                continue;
            }
            if (element.IsDouble)
            {
                elements.Add((Double)element);
                continue;
            }
            if (element.IsInt64)
            {
                elements.Add((Int64)element);
                continue;
            }
            if (element.IsObject)
            {
                elements.Add(this.CreateObjectFromJson((JsonObject)element));
                continue;
            }
            if (element.IsString)
            {
                elements.Add((String)element);
                continue;
            }
            if (element.IsUInt64)
            {
                elements.Add((UInt64)element);
                continue;
            }
        }
        return elements;
    }

    private void DeserializeInformation(JsonElement jsonElement,
                                        SerializationInfo info,
                                        MemberState state)
    {
        if (jsonElement.IsArray)
        {
            if (this._strategies.ContainsKey(state.MemberType))
            {
                info.Set(state.Name,
                         this._strategies[state.MemberType].Deserialize(jsonElement));
                return;
            }
            info.Set(state.Name,
                     this.TransformJsonArray((JsonArray)jsonElement));
            return;
        }
        if (jsonElement.IsBoolean)
        {
            if (this._strategies.ContainsKey(state.MemberType))
            {
                info.Set(state.Name,
                         this._strategies[state.MemberType].Deserialize(jsonElement));
                return;
            }
            info.Set(state.Name,
                     (Boolean)jsonElement);
            return;
        }
        if (jsonElement.IsDouble)
        {
            if (this._strategies.ContainsKey(state.MemberType))
            {
                info.Set(state.Name,
                         this._strategies[state.MemberType].Deserialize(jsonElement));
                return;
            }
            info.Set(state.Name,
                     (Double)jsonElement);
            return;
        }
        if (jsonElement.IsInt64)
        {
            if (this._strategies.ContainsKey(state.MemberType))
            {
                info.Set(state.Name,
                         this._strategies[state.MemberType].Deserialize(jsonElement));
                return;
            }
            info.Set(state.Name,
                     (Int64)jsonElement);
            return;
        }
        if (jsonElement.IsObject)
        {
            if (this._strategies.ContainsKey(state.MemberType))
            {
                info.Set(state.Name,
                         this._strategies[state.MemberType].Deserialize(jsonElement));
                return;
            }
            info.Set(state.Name,
                     this.CreateObjectFromJson((JsonObject)jsonElement));
            return;
        }
        if (jsonElement.IsString)
        {
            if (this._strategies.ContainsKey(state.MemberType))
            {
                info.Set(state.Name,
                         this._strategies[state.MemberType].Deserialize(jsonElement));
                return;
            }
            info.Set(state.Name,
                     (String)jsonElement);
            return;
        }
        if (jsonElement.IsUInt64)
        {
            if (this._strategies.ContainsKey(state.MemberType))
            {
                info.Set(state.Name,
                         this._strategies[state.MemberType].Deserialize(jsonElement));
                return;
            }
            info.Set(state.Name,
                     (UInt64)jsonElement);
            return;
        }
    }
    private void DeserializeInformation(JsonObject jsonObject,
                                        SerializationInfo info)
    {
        foreach (MemberState state in info)
        {
            if (!jsonObject.HasMember(state.Name))
            {
                throw new MissingMemberException();
            }
            JsonElement? element = jsonObject[state.Name];
            if (element is null)
            {
                info.Set(state.Name,
                         (Object?)null);
                continue;
            }
            if (element is JsonArray memberArray)
            {
                info.Set(state.Name,
                         this.TransformJsonArray(memberArray));
                continue;
            }
            if (element is JsonObject memberObject)
            {
                info.Set(state.Name,
                         this.CreateObjectFromJson(memberObject));
                continue;
            }
            this.DeserializeInformation(element,
                                        info,
                                        state);
        }
        return;
    }

    private Object? CreateObjectFromJson(JsonObject jsonObject)
    {
#pragma warning disable
        if (!jsonObject.HasMember(TYPENAME))
        {
            // Needs typename to create object
            throw new FormatException();
        }
        Type? type = Type.GetType((String)jsonObject[TYPENAME]);
        if (type is null)
        {
            throw new InvalidOperationException();
        }
        if (this._strategies.ContainsKey(type))
        {
            return this._strategies[type].Deserialize(jsonObject);
        }
        if (type.GetInterfaces()
                .Any(i => i.GetGenericTypeDefinition() == typeof(ISerializable<>)))
        {
            return this.DeserializeThroughInterface(jsonObject,
                                                    type);
        }
        SerializationInfo info = SerializationInfo.Create(type);
        this.DeserializeInformation(jsonObject,
                                    info);
        return CreateObject(info);
#pragma warning restore
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private const String TYPENAME = "JsonSerializer:Typename";
}

// IBothWaySerializer<TSerializable>
partial class JsonSerializer<TSerializable> : IBothWaySerializer<TSerializable>
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
        return result is not null;
    }
}

// IDeclaredSerializer
partial class JsonSerializer<TSerializable> : IDeclaredSerializer
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
namespace Narumikazuchi.Serialization.Bytes;

internal sealed partial class __BuildInformation<TSerializable>
{
    public IDictionary<Type, ISerializationStrategy<Byte[]>> Strategies =>
        this._strategies;

    public Action<TSerializable, ISerializationInfoAdder>? DataGetter
    {
        get => this._dataGetter;
        set => this._dataGetter = value;
    }

    public Func<ISerializationInfoGetter, TSerializable>? DataSetter
    {
        get => this._dataSetter;
        set => this._dataSetter = value;
    }
}

// Non-Public
partial class __BuildInformation<TSerializable>
{
    private void UseDefaultStrategies()
    {
        foreach (KeyValuePair<Type, ISerializationStrategy<Byte[]>> strategy in __SerializationStrategies.Integrated)
        {
            if (this._strategies
                    .ContainsKey(strategy.Key))
            {
                continue;
            }
            this._strategies
                .Add(item: strategy);
        }
    }

    private void UseStrategies(IEnumerable<KeyValuePair<Type, ISerializationStrategy<Byte[]>>> strategies)
    {
        ExceptionHelpers.ThrowIfArgumentNull(strategies);
        foreach (KeyValuePair<Type, ISerializationStrategy<Byte[]>> strategy in strategies)
        {
            if (this._strategies
                    .ContainsKey(strategy.Key))
            {
                this._strategies[strategy.Key] = strategy.Value;
                continue;
            }
            this._strategies
                .Add(item: strategy);
        }
    }

    private void UseStrategy<TFrom>(ISerializationStrategy<Byte[], TFrom> strategy)
    {
        ExceptionHelpers.ThrowIfArgumentNull(strategy);
        if (this._strategies
                .ContainsKey(typeof(TFrom)))
        {
            this._strategies[typeof(TFrom)] = strategy;
            return;
        }
        this._strategies
            .Add(key: typeof(TFrom),
                 value: strategy);
    }

    private readonly IDictionary<Type, ISerializationStrategy<Byte[]>> _strategies = new Dictionary<Type, ISerializationStrategy<Byte[]>>();
    private Action<TSerializable, ISerializationInfoAdder>? _dataGetter = null;
    private Func<ISerializationInfoGetter, TSerializable>? _dataSetter = null;
}

// IByteDeserializerDefaultStrategyAppender<T>
partial class __BuildInformation<TSerializable> : IByteDeserializerDefaultStrategyAppender<TSerializable>
{
    IByteDeserializerStrategyAppenderOrFinalizer<TSerializable> IByteDeserializerDefaultStrategyAppender<TSerializable>.UseDefaultStrategies()
    {
        this.UseDefaultStrategies();
        return this;
    }
}

// IByteDeserializerStrategyAppender<T>
partial class __BuildInformation<TSerializable> : IByteDeserializerStrategyAppender<TSerializable>
{
    IByteDeserializerStrategyAppenderOrFinalizer<TSerializable> IByteDeserializerStrategyAppender<TSerializable>.UseStrategies(IEnumerable<KeyValuePair<Type, ISerializationStrategy<Byte[]>>> strategies)
    {
        this.UseStrategies(strategies);
        return this;
    }

    IByteDeserializerStrategyAppenderOrFinalizer<TSerializable> IByteDeserializerStrategyAppender<TSerializable>.UseStrategyForType<TFrom>(ISerializationStrategy<Byte[], TFrom> strategy)
    {
        this.UseStrategy(strategy);
        return this;
    }
}

// IByteDeserializerStrategyAppenderOrFinalizer<T>
partial class __BuildInformation<TSerializable> : IByteDeserializerStrategyAppenderOrFinalizer<TSerializable>
{
    IByteDeserializer<TSerializable> IByteDeserializerStrategyAppenderOrFinalizer<TSerializable>.Construct()
    {
        __ByteSerializer<TSerializable> deserializer;
        if (__Cache.CreatedDeserializers
                   .ContainsKey(typeof(TSerializable)))
        {
            IList<Object> deserializers = __Cache.CreatedDeserializers[typeof(TSerializable)];
            for (Int32 i = 0; i < deserializers.Count; i++)
            {
                deserializer = (__ByteSerializer<TSerializable>)deserializers[i];
                if (this._dataGetter != deserializer.DataGetter)
                {
                    continue;
                }
                if (this._dataSetter != deserializer.DataSetter)
                {
                    continue;
                }
                Boolean next = false;
                foreach (KeyValuePair<Type, ISerializationStrategy<Byte[]>> kv in this._strategies)
                {
                    if (!deserializer.Strategies
                                     .Contains(kv))
                    {
                        next = true;
                        break;
                    }
                }
                if (next)
                {
                    continue;
                }

                return deserializer;
            }
        }
        else
        {
            __Cache.CreatedDeserializers
                   .Add(key: typeof(TSerializable),
                        value: new List<Object>());
        }

        deserializer = new __ByteSerializer<TSerializable>(this);
        __Cache.CreatedDeserializers[typeof(TSerializable)]
               .Add(deserializer);
        return deserializer;
    }
}

// IByteSerializerDefaultStrategyAppender<T>
partial class __BuildInformation<TSerializable> : IByteSerializerDefaultStrategyAppender<TSerializable>
{
    IByteSerializerStrategyAppenderOrFinalizer<TSerializable> IByteSerializerDefaultStrategyAppender<TSerializable>.UseDefaultStrategies()
    {
        this.UseDefaultStrategies();
        return this;
    }
}

// IByteSerializerStrategyAppender<T>
partial class __BuildInformation<TSerializable> : IByteSerializerStrategyAppender<TSerializable>
{
    IByteSerializerStrategyAppenderOrFinalizer<TSerializable> IByteSerializerStrategyAppender<TSerializable>.UseStrategies(IEnumerable<KeyValuePair<Type, ISerializationStrategy<Byte[]>>> strategies)
    {
        this.UseStrategies(strategies);
        return this;
    }

    IByteSerializerStrategyAppenderOrFinalizer<TSerializable> IByteSerializerStrategyAppender<TSerializable>.UseStrategyForType<TFrom>(ISerializationStrategy<Byte[], TFrom> strategy)
    {
        this.UseStrategy(strategy);
        return this;
    }
}

// IByteSerializerStrategyAppenderOrFinalizer<T>
partial class __BuildInformation<TSerializable> : IByteSerializerStrategyAppenderOrFinalizer<TSerializable>
{
    IByteSerializer<TSerializable> IByteSerializerStrategyAppenderOrFinalizer<TSerializable>.Construct()
    {
        __ByteSerializer<TSerializable> serializer;
        if (__Cache.CreatedSerializers
                   .ContainsKey(typeof(TSerializable)))
        {
            IList<Object> deserializers = __Cache.CreatedSerializers[typeof(TSerializable)];
            for (Int32 i = 0; i < deserializers.Count; i++)
            {
                serializer = (__ByteSerializer<TSerializable>)deserializers[i];
                if (this._dataGetter != serializer.DataGetter)
                {
                    continue;
                }
                if (this._dataSetter != serializer.DataSetter)
                {
                    continue;
                }
                Boolean next = false;
                foreach (KeyValuePair<Type, ISerializationStrategy<Byte[]>> kv in this._strategies)
                {
                    if (!serializer.Strategies
                                   .Contains(kv))
                    {
                        next = true;
                        break;
                    }
                }
                if (next)
                {
                    continue;
                }

                return serializer;
            }
        }
        else
        {
            __Cache.CreatedSerializers
                   .Add(key: typeof(TSerializable),
                        value: new List<Object>());
        }

        serializer = new __ByteSerializer<TSerializable>(this);
        __Cache.CreatedSerializers[typeof(TSerializable)]
               .Add(serializer);
        return serializer;
    }
}

// IByteSerializerDeserializerDefaultStrategyAppender<T>
partial class __BuildInformation<TSerializable> : IByteSerializerDeserializerDefaultStrategyAppender<TSerializable>
{
    IByteSerializerDeserializerStrategyAppenderOrFinalizer<TSerializable> IByteSerializerDeserializerDefaultStrategyAppender<TSerializable>.UseDefaultStrategies()
    {
        this.UseDefaultStrategies();
        return this;
    }
}

// IByteSerializerDeserializerStrategyAppender<T>
partial class __BuildInformation<TSerializable> : IByteSerializerDeserializerStrategyAppender<TSerializable>
{
    IByteSerializerDeserializerStrategyAppenderOrFinalizer<TSerializable> IByteSerializerDeserializerStrategyAppender<TSerializable>.UseStrategies(IEnumerable<KeyValuePair<Type, ISerializationStrategy<Byte[]>>> strategies)
    {
        this.UseStrategies(strategies);
        return this;
    }

    IByteSerializerDeserializerStrategyAppenderOrFinalizer<TSerializable> IByteSerializerDeserializerStrategyAppender<TSerializable>.UseStrategyForType<TFrom>(ISerializationStrategy<Byte[], TFrom> strategy)
    {
        this.UseStrategy(strategy);
        return this;
    }
}

// IByteSerializerDeserializerStrategyAppenderOrFinalizer<T>
partial class __BuildInformation<TSerializable> : IByteSerializerDeserializerStrategyAppenderOrFinalizer<TSerializable>
{
    IByteSerializerDeserializer<TSerializable> IByteSerializerDeserializerStrategyAppenderOrFinalizer<TSerializable>.Construct()
    {
        __ByteSerializer<TSerializable> serializer;
        if (__Cache.CreatedSerializers
                   .ContainsKey(typeof(TSerializable)))
        {
            IList<Object> deserializers = __Cache.CreatedSerializers[typeof(TSerializable)];
            for (Int32 i = 0; i < deserializers.Count; i++)
            {
                serializer = (__ByteSerializer<TSerializable>)deserializers[i];
                if (this._dataGetter != serializer.DataGetter)
                {
                    continue;
                }
                if (this._dataSetter != serializer.DataSetter)
                {
                    continue;
                }
                Boolean next = false;
                foreach (KeyValuePair<Type, ISerializationStrategy<Byte[]>> kv in this._strategies)
                {
                    if (!serializer.Strategies
                                   .Contains(kv))
                    {
                        next = true;
                        break;
                    }
                }
                if (next)
                {
                    continue;
                }

                if (__Cache.CreatedDeserializers
                           .ContainsKey(typeof(TSerializable)))
                {
                    __Cache.CreatedDeserializers[typeof(TSerializable)]
                           .Add(serializer);
                }
                else
                {
                    __Cache.CreatedDeserializers
                           .Add(key: typeof(TSerializable),
                                value: new List<Object>
                                    {
                                        serializer
                                    });
                }
                return serializer;
            }
        }
        else
        {
            __Cache.CreatedSerializers
                   .Add(key: typeof(TSerializable),
                        value: new List<Object>());
        }

        if (__Cache.CreatedDeserializers
                   .ContainsKey(typeof(TSerializable)))
        {
            IList<Object> deserializers = __Cache.CreatedDeserializers[typeof(TSerializable)];
            for (Int32 i = 0; i < deserializers.Count; i++)
            {
                serializer = (__ByteSerializer<TSerializable>)deserializers[i];
                if (this._dataGetter != serializer.DataGetter)
                {
                    continue;
                }
                if (this._dataSetter != serializer.DataSetter)
                {
                    continue;
                }
                Boolean next = false;
                foreach (KeyValuePair<Type, ISerializationStrategy<Byte[]>> kv in this._strategies)
                {
                    if (!serializer.Strategies
                                   .Contains(kv))
                    {
                        next = true;
                        break;
                    }
                }
                if (next)
                {
                    continue;
                }

                if (__Cache.CreatedSerializers
                           .ContainsKey(typeof(TSerializable)))
                {
                    __Cache.CreatedSerializers[typeof(TSerializable)]
                           .Add(serializer);
                }
                else
                {
                    __Cache.CreatedSerializers
                           .Add(key: typeof(TSerializable),
                                value: new List<Object>
                                    {
                                        serializer
                                    });
                }
                return serializer;
            }
        }
        else
        {
            __Cache.CreatedDeserializers
                   .Add(key: typeof(TSerializable),
                        value: new List<Object>());
        }

        serializer = new __ByteSerializer<TSerializable>(this);
        __Cache.CreatedDeserializers[typeof(TSerializable)]
               .Add(serializer);
        __Cache.CreatedSerializers[typeof(TSerializable)]
               .Add(serializer);
        return serializer;
    }
}
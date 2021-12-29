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
    IByteDeserializer<TSerializable> IByteDeserializerStrategyAppenderOrFinalizer<TSerializable>.Construct() =>
        new __ByteSerializer<TSerializable>(this);
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
    IByteSerializer<TSerializable> IByteSerializerStrategyAppenderOrFinalizer<TSerializable>.Construct() =>
        new __ByteSerializer<TSerializable>(this);
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
    IByteSerializerDeserializer<TSerializable> IByteSerializerDeserializerStrategyAppenderOrFinalizer<TSerializable>.Construct() =>
        new __ByteSerializer<TSerializable>(this);
}
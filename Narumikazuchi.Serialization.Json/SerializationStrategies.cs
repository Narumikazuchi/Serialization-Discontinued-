namespace Narumikazuchi.Serialization.Json;

internal static class __SerializationStrategies
{
    public static Dictionary<Type, ISerializationStrategy<JsonElement>> Integrated { get; } = new()
    {
        { typeof(System.Boolean),   Boolean.Default },
        { typeof(System.Byte),      Byte.Default },
        { typeof(System.Char),      Char.Default },
        { typeof(System.Double),    Double.Default },
        { typeof(System.Int16),     Int16.Default },
        { typeof(System.Int32),     Int32.Default },
        { typeof(System.Int64),     Int64.Default },
        { typeof(System.IntPtr),    IntPtr.Default },
        { typeof(System.SByte),     SByte.Default },
        { typeof(System.Single),    Single.Default },
        { typeof(System.UInt16),    UInt16.Default },
        { typeof(System.UInt32),    UInt32.Default },
        { typeof(System.UInt64),    UInt64.Default },
        { typeof(System.UIntPtr),   UIntPtr.Default },
        { typeof(System.DateTime),  DateTime.Default },
        { typeof(System.Guid),      Guid.Default },
        { typeof(System.Half),      Half.Default },
        { typeof(System.String),    String.Default }
    };

#pragma warning disable CS8605
    public readonly struct Boolean : ISerializationStrategy<JsonElement, System.Boolean>
    {
        public JsonElement Serialize(System.Boolean input) =>
            new __JsonElement<System.Boolean>(input);

        public System.Boolean Deserialize(JsonElement input) =>
            (System.Boolean)input;

        JsonElement ISerializationStrategy<JsonElement>.Serialize(Object? input) =>
            this.Serialize((System.Boolean)input);

        Object? ISerializationStrategy<JsonElement>.Deserialize(JsonElement input) =>
            this.Deserialize(input);

        public static ref Boolean Default => ref _default;

        private static Boolean _default;
    }

    public readonly struct Byte : ISerializationStrategy<JsonElement, System.Byte>
    {
        public JsonElement Serialize(System.Byte input) =>
            UInt64.Default.Serialize(input);

        public System.Byte Deserialize(JsonElement input) =>
            Convert.ToByte((System.UInt64)input);

        JsonElement ISerializationStrategy<JsonElement>.Serialize(Object? input) =>
            this.Serialize((System.Byte)input);

        Object? ISerializationStrategy<JsonElement>.Deserialize(JsonElement input) =>
            this.Deserialize(input);

        public static ref Byte Default => ref _default;

        private static Byte _default;
    }

    public readonly struct Char : ISerializationStrategy<JsonElement, System.Char>
    {
        public JsonElement Serialize(System.Char input) =>
            String.Default.Serialize(input.ToString());

        public System.Char Deserialize(JsonElement input) =>
            ((System.String)input)[0];

        JsonElement ISerializationStrategy<JsonElement>.Serialize(Object? input) =>
            this.Serialize((System.Char)input);

        Object? ISerializationStrategy<JsonElement>.Deserialize(JsonElement input) =>
            this.Deserialize(input);

        public static ref Char Default => ref _default;

        private static Char _default;
    }

    public readonly struct Double : ISerializationStrategy<JsonElement, System.Double>
    {
        public JsonElement Serialize(System.Double input) =>
            new __JsonElement<System.Double>(input);

        public System.Double Deserialize(JsonElement input) =>
            (System.Double)input;

        JsonElement ISerializationStrategy<JsonElement>.Serialize(Object? input) =>
            this.Serialize((System.Double)input);

        Object? ISerializationStrategy<JsonElement>.Deserialize(JsonElement input) =>
            this.Deserialize(input);

        public static ref Double Default => ref _default;

        private static Double _default;
    }

    public readonly struct Int16 : ISerializationStrategy<JsonElement, System.Int16>
    {
        public JsonElement Serialize(System.Int16 input) =>
            Int64.Default.Serialize(input);

        public System.Int16 Deserialize(JsonElement input) =>
            Convert.ToInt16((System.Int64)input);

        JsonElement ISerializationStrategy<JsonElement>.Serialize(Object? input) =>
            this.Serialize((System.Int16)input);

        Object? ISerializationStrategy<JsonElement>.Deserialize(JsonElement input) =>
            this.Deserialize(input);

        public static ref Int16 Default => ref _default;

        private static Int16 _default;
    }

    public readonly struct Int32 : ISerializationStrategy<JsonElement, System.Int32>
    {
        public JsonElement Serialize(System.Int32 input) =>
            Int64.Default.Serialize(input);

        public System.Int32 Deserialize(JsonElement input) =>
            Convert.ToInt32((System.Int64)input);

        JsonElement ISerializationStrategy<JsonElement>.Serialize(Object? input) =>
            this.Serialize((System.Int32)input);

        Object? ISerializationStrategy<JsonElement>.Deserialize(JsonElement input) =>
            this.Deserialize(input);

        public static ref Int32 Default => ref _default;

        private static Int32 _default;
    }

    public readonly struct Int64 : ISerializationStrategy<JsonElement, System.Int64>
    {
        public JsonElement Serialize(System.Int64 input) =>
            new __JsonElement<System.Int64>(input);

        public System.Int64 Deserialize(JsonElement input) =>
            (System.Int64)input;

        JsonElement ISerializationStrategy<JsonElement>.Serialize(Object? input) =>
            this.Serialize((System.Int64)input);

        Object? ISerializationStrategy<JsonElement>.Deserialize(JsonElement input) =>
            this.Deserialize(input);

        public static ref Int64 Default => ref _default;

        private static Int64 _default;
    }

    public readonly struct IntPtr : ISerializationStrategy<JsonElement, System.IntPtr>
    {
        public JsonElement Serialize(System.IntPtr input) =>
            Int64.Default.Serialize(input.ToInt64());

        public System.IntPtr Deserialize(JsonElement input) =>
            (System.IntPtr)(System.Int64)input;

        JsonElement ISerializationStrategy<JsonElement>.Serialize(Object? input) =>
            this.Serialize((System.IntPtr)input);

        Object? ISerializationStrategy<JsonElement>.Deserialize(JsonElement input) =>
            this.Deserialize(input);

        public static ref IntPtr Default => ref _default;

        private static IntPtr _default;
    }

    public readonly struct SByte : ISerializationStrategy<JsonElement, System.SByte>
    {
        public JsonElement Serialize(System.SByte input) =>
            Int64.Default.Serialize(input);

        public System.SByte Deserialize(JsonElement input) =>
            (System.SByte)(System.Int64)input;

        JsonElement ISerializationStrategy<JsonElement>.Serialize(Object? input) =>
            this.Serialize((System.SByte)input);

        Object? ISerializationStrategy<JsonElement>.Deserialize(JsonElement input) =>
            this.Deserialize(input);

        public static ref SByte Default => ref _default;

        private static SByte _default;
    }

    public readonly struct Single : ISerializationStrategy<JsonElement, System.Single>
    {
        public JsonElement Serialize(System.Single input) =>
            Double.Default.Serialize(input);

        public System.Single Deserialize(JsonElement input) =>
            Convert.ToSingle((System.Double)input);

        JsonElement ISerializationStrategy<JsonElement>.Serialize(Object? input) =>
            this.Serialize((System.Single)input);

        Object? ISerializationStrategy<JsonElement>.Deserialize(JsonElement input) =>
            this.Deserialize(input);

        public static ref Single Default => ref _default;

        private static Single _default;
    }

    public readonly struct UInt16 : ISerializationStrategy<JsonElement, System.UInt16>
    {
        public JsonElement Serialize(System.UInt16 input) =>
            UInt64.Default.Serialize(input);

        public System.UInt16 Deserialize(JsonElement input) =>
            Convert.ToUInt16((System.UInt64)input);

        JsonElement ISerializationStrategy<JsonElement>.Serialize(Object? input) =>
            this.Serialize((System.UInt16)input);

        Object? ISerializationStrategy<JsonElement>.Deserialize(JsonElement input) =>
            this.Deserialize(input);

        public static ref UInt16 Default => ref _default;

        private static UInt16 _default;
    }

    public readonly struct UInt32 : ISerializationStrategy<JsonElement, System.UInt32>
    {
        public JsonElement Serialize(System.UInt32 input) =>
            UInt64.Default.Serialize(input);

        public System.UInt32 Deserialize(JsonElement input) =>
            Convert.ToUInt16((System.UInt64)input);

        JsonElement ISerializationStrategy<JsonElement>.Serialize(Object? input) =>
            this.Serialize((System.UInt32)input);

        Object? ISerializationStrategy<JsonElement>.Deserialize(JsonElement input) =>
            this.Deserialize(input);

        public static ref UInt32 Default => ref _default;

        private static UInt32 _default;
    }

    public readonly struct UInt64 : ISerializationStrategy<JsonElement, System.UInt64>
    {
        public JsonElement Serialize(System.UInt64 input) =>
            new __JsonElement<System.UInt64>(input);

        public System.UInt64 Deserialize(JsonElement input) =>
            (System.UInt64)input;

        JsonElement ISerializationStrategy<JsonElement>.Serialize(Object? input) =>
            this.Serialize((System.UInt64)input);

        Object? ISerializationStrategy<JsonElement>.Deserialize(JsonElement input) =>
            this.Deserialize(input);

        public static ref UInt64 Default => ref _default;

        private static UInt64 _default;
    }

    public readonly struct UIntPtr : ISerializationStrategy<JsonElement, System.UIntPtr>
    {
        public JsonElement Serialize(System.UIntPtr input) =>
            UInt64.Default.Serialize(input.ToUInt64());

        public System.UIntPtr Deserialize(JsonElement input) =>
            (System.UIntPtr)(System.UInt64)input;

        JsonElement ISerializationStrategy<JsonElement>.Serialize(Object? input) =>
            this.Serialize((System.UIntPtr)input);

        Object? ISerializationStrategy<JsonElement>.Deserialize(JsonElement input) =>
            this.Deserialize(input);

        public static ref UIntPtr Default => ref _default;

        private static UIntPtr _default;
    }

    public readonly struct DateTime : ISerializationStrategy<JsonElement, System.DateTime>
    {
        public JsonElement Serialize(System.DateTime input) =>
            Int64.Default.Serialize(input.Ticks);

        public System.DateTime Deserialize(JsonElement input) =>
            new((System.Int64)input);

        JsonElement ISerializationStrategy<JsonElement>.Serialize(Object? input) =>
            this.Serialize((System.DateTime)input);

        Object? ISerializationStrategy<JsonElement>.Deserialize(JsonElement input) =>
            this.Deserialize(input);

        public static ref DateTime Default => ref _default;

        private static DateTime _default;
    }

    public readonly struct Guid : ISerializationStrategy<JsonElement, System.Guid>
    {
        public JsonElement Serialize(System.Guid input) =>
            String.Default.Serialize(input.ToString());

        public System.Guid Deserialize(JsonElement input) =>
            System.Guid.Parse((System.String)input);

        JsonElement ISerializationStrategy<JsonElement>.Serialize(Object? input) =>
            this.Serialize((System.Guid)input);

        Object? ISerializationStrategy<JsonElement>.Deserialize(JsonElement input) =>
            this.Deserialize(input);

        public static ref Guid Default => ref _default;

        private static Guid _default;
    }

    public readonly struct Half : ISerializationStrategy<JsonElement, System.Half>
    {
        public JsonElement Serialize(System.Half input) =>
            Double.Default.Serialize((System.Double)input);

        public System.Half Deserialize(JsonElement input) =>
            (System.Half)(System.Double)input;

        JsonElement ISerializationStrategy<JsonElement>.Serialize(Object? input) =>
            this.Serialize((System.Half)input);

        Object? ISerializationStrategy<JsonElement>.Deserialize(JsonElement input) =>
            this.Deserialize(input);

        public static ref Half Default => ref _default;

        private static Half _default;
    }

    public readonly struct String : ISerializationStrategy<JsonElement, System.String?>
    {
        public JsonElement Serialize(System.String? input)
        {
            if (input is null)
            {
                return null;
            }
            return new __JsonElement<System.String>(input);
        }

        public System.String? Deserialize(JsonElement input)
        {
            if (input is null)
            {
                return null;
            }
            return (System.String?)input;
        }


        JsonElement ISerializationStrategy<JsonElement>.Serialize(Object? input) =>
            this.Serialize((System.String?)input);

        Object? ISerializationStrategy<JsonElement>.Deserialize(JsonElement input) =>
            this.Deserialize(input);

        public static ref String Default => ref _default;

        private static String _default;
    }
}
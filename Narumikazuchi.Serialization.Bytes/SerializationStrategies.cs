namespace Narumikazuchi.Serialization.Bytes
{
    internal static class __SerializationStrategies
    {
#pragma warning disable CS8605
        public readonly struct Boolean : ISerializationStrategy<System.Byte[], System.Boolean>
        {
            public System.Byte[] Serialize(System.Boolean input) =>
                BitConverter.GetBytes(input);

            public System.Boolean Deserialize(System.Byte[] input) =>
                BitConverter.ToBoolean(input);

            System.Byte[] ISerializationStrategy<System.Byte[]>.Serialize(Object? input) =>
                this.Serialize((System.Boolean)input);

            Object? ISerializationStrategy<System.Byte[]>.Deserialize(System.Byte[] input) =>
                this.Deserialize(input);

            public static ref Boolean Default => ref _default;

            private static Boolean _default;
        }

        public readonly struct Byte : ISerializationStrategy<System.Byte[], System.Byte>
        {
            public System.Byte[] Serialize(System.Byte input) =>
                new System.Byte[] { input };

            public System.Byte Deserialize(System.Byte[] input) =>
                input[0];

            System.Byte[] ISerializationStrategy<System.Byte[]>.Serialize(Object? input) =>
                this.Serialize((System.Byte)input);

            Object? ISerializationStrategy<System.Byte[]>.Deserialize(System.Byte[] input) =>
                this.Deserialize(input);

            public static ref Byte Default => ref _default;

            private static Byte _default;
        }

        public readonly struct Char : ISerializationStrategy<System.Byte[], System.Char>
        {
            public System.Byte[] Serialize(System.Char input) =>
                BitConverter.GetBytes(input);

            public System.Char Deserialize(System.Byte[] input) =>
                BitConverter.ToChar(input);

            System.Byte[] ISerializationStrategy<System.Byte[]>.Serialize(Object? input) =>
                this.Serialize((System.Char)input);

            Object? ISerializationStrategy<System.Byte[]>.Deserialize(System.Byte[] input) =>
                this.Deserialize(input);

            public static ref Char Default => ref _default;

            private static Char _default;
        }

        public readonly struct Double : ISerializationStrategy<System.Byte[], System.Double>
        {
            public System.Byte[] Serialize(System.Double input) =>
                BitConverter.GetBytes(input);

            public System.Double Deserialize(System.Byte[] input) =>
                BitConverter.ToDouble(input);

            System.Byte[] ISerializationStrategy<System.Byte[]>.Serialize(Object? input) =>
                this.Serialize((System.Double)input);

            Object? ISerializationStrategy<System.Byte[]>.Deserialize(System.Byte[] input) =>
                this.Deserialize(input);

            public static ref Double Default => ref _default;

            private static Double _default;
        }

        public readonly struct Int16 : ISerializationStrategy<System.Byte[], System.Int16>
        {
            public System.Byte[] Serialize(System.Int16 input) =>
                BitConverter.GetBytes(input);

            public System.Int16 Deserialize(System.Byte[] input) =>
                BitConverter.ToInt16(input);

            System.Byte[] ISerializationStrategy<System.Byte[]>.Serialize(Object? input) =>
                this.Serialize((System.Int16)input);

            Object? ISerializationStrategy<System.Byte[]>.Deserialize(System.Byte[] input) =>
                this.Deserialize(input);

            public static ref Int16 Default => ref _default;

            private static Int16 _default;
        }

        public readonly struct Int32 : ISerializationStrategy<System.Byte[], System.Int32>
        {
            public System.Byte[] Serialize(System.Int32 input) =>
                BitConverter.GetBytes(input);

            public System.Int32 Deserialize(System.Byte[] input) =>
                BitConverter.ToInt32(input);

            System.Byte[] ISerializationStrategy<System.Byte[]>.Serialize(Object? input) =>
                this.Serialize((System.Int32)input);

            Object? ISerializationStrategy<System.Byte[]>.Deserialize(System.Byte[] input) =>
                this.Deserialize(input);

            public static ref Int32 Default => ref _default;

            private static Int32 _default;
        }

        public readonly struct Int64 : ISerializationStrategy<System.Byte[], System.Int64>
        {
            public System.Byte[] Serialize(System.Int64 input) =>
                BitConverter.GetBytes(input);

            public System.Int64 Deserialize(System.Byte[] input) =>
                BitConverter.ToInt64(input);

            System.Byte[] ISerializationStrategy<System.Byte[]>.Serialize(Object? input) =>
                this.Serialize((System.Int64)input);

            Object? ISerializationStrategy<System.Byte[]>.Deserialize(System.Byte[] input) =>
                this.Deserialize(input);

            public static ref Int64 Default => ref _default;

            private static Int64 _default;
        }

        public readonly struct IntPtr : ISerializationStrategy<System.Byte[], System.IntPtr>
        {
            public System.Byte[] Serialize(System.IntPtr input) =>
                BitConverter.GetBytes(input.ToInt64());

            public System.IntPtr Deserialize(System.Byte[] input) =>
                (System.IntPtr)BitConverter.ToInt64(input);

            System.Byte[] ISerializationStrategy<System.Byte[]>.Serialize(Object? input) =>
                this.Serialize((System.IntPtr)input);

            Object? ISerializationStrategy<System.Byte[]>.Deserialize(System.Byte[] input) =>
                this.Deserialize(input);

            public static ref IntPtr Default => ref _default;

            private static IntPtr _default;
        }

        public readonly struct SByte : ISerializationStrategy<System.Byte[], System.SByte>
        {
            public System.Byte[] Serialize(System.SByte input) =>
                new System.Byte[] { (System.Byte)input };

            public System.SByte Deserialize(System.Byte[] input) =>
                (System.SByte)input[0];

            System.Byte[] ISerializationStrategy<System.Byte[]>.Serialize(Object? input) =>
                this.Serialize((System.SByte)input);

            Object? ISerializationStrategy<System.Byte[]>.Deserialize(System.Byte[] input) =>
                this.Deserialize(input);

            public static ref SByte Default => ref _default;

            private static SByte _default;
        }

        public readonly struct Single : ISerializationStrategy<System.Byte[], System.Single>
        {
            public System.Byte[] Serialize(System.Single input) =>
                BitConverter.GetBytes(input);

            public System.Single Deserialize(System.Byte[] input) =>
                BitConverter.ToSingle(input);

            System.Byte[] ISerializationStrategy<System.Byte[]>.Serialize(Object? input) =>
                this.Serialize((System.Single)input);

            Object? ISerializationStrategy<System.Byte[]>.Deserialize(System.Byte[] input) =>
                this.Deserialize(input);

            public static ref Single Default => ref _default;

            private static Single _default;
        }

        public readonly struct UInt16 : ISerializationStrategy<System.Byte[], System.UInt16>
        {
            public System.Byte[] Serialize(System.UInt16 input) =>
                BitConverter.GetBytes(input);

            public System.UInt16 Deserialize(System.Byte[] input) =>
                BitConverter.ToUInt16(input);

            System.Byte[] ISerializationStrategy<System.Byte[]>.Serialize(Object? input) =>
                this.Serialize((System.UInt16)input);

            Object? ISerializationStrategy<System.Byte[]>.Deserialize(System.Byte[] input) =>
                this.Deserialize(input);

            public static ref UInt16 Default => ref _default;

            private static UInt16 _default;
        }

        public readonly struct UInt32 : ISerializationStrategy<System.Byte[], System.UInt32>
        {
            public System.Byte[] Serialize(System.UInt32 input) =>
                BitConverter.GetBytes(input);

            public System.UInt32 Deserialize(System.Byte[] input) =>
                BitConverter.ToUInt32(input);

            System.Byte[] ISerializationStrategy<System.Byte[]>.Serialize(Object? input) =>
                this.Serialize((System.UInt32)input);

            Object? ISerializationStrategy<System.Byte[]>.Deserialize(System.Byte[] input) =>
                this.Deserialize(input);

            public static ref UInt32 Default => ref _default;

            private static UInt32 _default;
        }

        public readonly struct UInt64 : ISerializationStrategy<System.Byte[], System.UInt64>
        {
            public System.Byte[] Serialize(System.UInt64 input) =>
                BitConverter.GetBytes(input);

            public System.UInt64 Deserialize(System.Byte[] input) =>
                BitConverter.ToUInt64(input);

            System.Byte[] ISerializationStrategy<System.Byte[]>.Serialize(Object? input) =>
                this.Serialize((System.UInt64)input);

            Object? ISerializationStrategy<System.Byte[]>.Deserialize(System.Byte[] input) =>
                this.Deserialize(input);

            public static ref UInt64 Default => ref _default;

            private static UInt64 _default;
        }

        public readonly struct UIntPtr : ISerializationStrategy<System.Byte[], System.UIntPtr>
        {
            public System.Byte[] Serialize(System.UIntPtr input) =>
                BitConverter.GetBytes(input.ToUInt64());

            public System.UIntPtr Deserialize(System.Byte[] input) =>
                (System.UIntPtr)BitConverter.ToUInt64(input);

            System.Byte[] ISerializationStrategy<System.Byte[]>.Serialize(Object? input) =>
                this.Serialize((System.UIntPtr)input);

            Object? ISerializationStrategy<System.Byte[]>.Deserialize(System.Byte[] input) =>
                this.Deserialize(input);

            public static ref UIntPtr Default => ref _default;

            private static UIntPtr _default;
        }

        public readonly struct DateTime : ISerializationStrategy<System.Byte[], System.DateTime>
        {
            public System.Byte[] Serialize(System.DateTime input) => 
                BitConverter.GetBytes(input.Ticks);

            public System.DateTime Deserialize(System.Byte[] input)
            {
                System.Int64 ticks = BitConverter.ToInt64(input, 0);
                return new(ticks);
            }

            System.Byte[] ISerializationStrategy<System.Byte[]>.Serialize(Object? input) =>
                this.Serialize((System.DateTime)input);

            Object? ISerializationStrategy<System.Byte[]>.Deserialize(System.Byte[] input) =>
                this.Deserialize(input);

            public static ref DateTime Default => ref _default;

            private static DateTime _default;
        }

        public readonly struct Guid : ISerializationStrategy<System.Byte[], System.Guid>
        {
            public System.Byte[] Serialize(System.Guid input) =>
                input.ToByteArray();

            public System.Guid Deserialize(System.Byte[] input) =>
                new(input);

            System.Byte[] ISerializationStrategy<System.Byte[]>.Serialize(Object? input) =>
                this.Serialize((System.Guid)input);

            Object? ISerializationStrategy<System.Byte[]>.Deserialize(System.Byte[] input) =>
                this.Deserialize(input);

            public static ref Guid Default => ref _default;

            private static Guid _default;
        }

        public readonly struct Half : ISerializationStrategy<System.Byte[], System.Half>
        {
            public System.Byte[] Serialize(System.Half input) =>
                BitConverter.GetBytes(input);

            public System.Half Deserialize(System.Byte[] input) =>
                BitConverter.ToHalf(input);

            System.Byte[] ISerializationStrategy<System.Byte[]>.Serialize(Object? input) =>
                this.Serialize((System.Half)input);

            Object? ISerializationStrategy<System.Byte[]>.Deserialize(System.Byte[] input) =>
                this.Deserialize(input);

            public static ref Half Default => ref _default;

            private static Half _default;
        }

        public readonly struct String : ISerializationStrategy<System.Byte[], System.String?>
        {
            public System.Byte[] Serialize(System.String? input)
            {
                if (input is null)
                {
                    return new System.Byte[] { 0x00, 0x00, 0x00, 0x00 };
                }

                System.Byte[] data = Encoding.UTF8.GetBytes(input);
                System.Byte[] size = BitConverter.GetBytes(data.Length);
                return size.Concat(data)
                           .ToArray();
            }

            public System.String? Deserialize(System.Byte[] input)
            {
                System.Int32 size = BitConverter.ToInt32(input, 0);
                if (size == 0)
                {
                    return System.String.Empty;
                }
                return Encoding.UTF8.GetString(input, 4, size);
            }

            System.Byte[] ISerializationStrategy<System.Byte[]>.Serialize(Object? input) =>
                this.Serialize((System.String?)input);

            Object? ISerializationStrategy<System.Byte[]>.Deserialize(System.Byte[] input) =>
                this.Deserialize(input);

            public static ref String Default => ref _default;

            private static String _default;
        }
    }
}

namespace Narumikazuchi.Serialization.Bytes
{
    internal class __Header
    {
        private __Header()
        { }
        public __Header(Type type)
        {
            ExceptionHelpers.ThrowIfNull(type);
            ExceptionHelpers.ThrowIfNullOrEmpty(type.AssemblyQualifiedName,
                                                nameof(type));

#pragma warning disable
            this.Typename = type.AssemblyQualifiedName;
#pragma warning restore
        }

        public static __Header FromStream(Stream source,
                                        Int64 size,
                                        out UInt64 read)
        {
            __Header result = new();

            Byte[] data = new Byte[size];
            Int64 index = 0;
            while (index < size)
            {
                Int32 b = source.ReadByte();
                if (b == -1)
                {
                    // Unexpected end
                    throw new IOException();
                }
                data[index++] = (Byte)b;
            }
            read = Convert.ToUInt64(size);

            Int32 offset = 0;
            Int32 stringLength = BitConverter.ToInt32(data,
                                                      offset);
            offset += sizeof(Int32);
            String typename = Encoding.UTF8.GetString(data,
                                                      offset,
                                                      stringLength);
            offset += stringLength;
            result.Typename = typename;

            Int32 count = BitConverter.ToInt32(data, 
                                               offset);
            offset += sizeof(Int32);

            for (Int32 i = 0; i < count; i++)
            {
                __HeaderItem item = new();
                item.Position = BitConverter.ToInt64(data,
                                                      offset);
                offset += sizeof(Int64);
                item.Length = BitConverter.ToInt64(data,
                                                    offset);
                offset += sizeof(Int64);
                stringLength = BitConverter.ToInt32(data,
                                                    offset);
                offset += sizeof(Int32);
                item.Typename = Encoding.UTF8.GetString(data, 
                                                        offset, 
                                                        stringLength);
                offset += stringLength;
                stringLength = BitConverter.ToInt32(data,
                                                    offset);
                offset += sizeof(Int32);
                item.Name = Encoding.UTF8.GetString(data,
                                                    offset,
                                                    stringLength);
                offset += stringLength;
                result.Items.Add(item);
            }

            return result;
        }

        public MemoryStream AsMemory()
        {
            MemoryStream result = new();

            result.Write(BitConverter.GetBytes(this.TypenameGlyphs));
            result.Write(this._typenameRaw);
            result.Write(BitConverter.GetBytes(this.MemberCount));
            for (Int32 i = 0; i < this.MemberCount; i++)
            {
                using MemoryStream item = this.Items[i].AsMemory();
                item.CopyTo(result);
            }

            result.Position = 0;
            return result;
        }

        public Int32 TypenameGlyphs => this._typenameRaw.Length;
        public String Typename
        {
            get => this._typename;
            set
            {
                if (String.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentNullException(nameof(value));
                }
                this._typename = value;
                this._typenameRaw = Encoding.UTF8.GetBytes(value);
            }
        }
        public Int32 MemberCount => this.Items.Count;
        public Int64 Size
        {
            get
            {
                using MemoryStream temp = this.AsMemory();
                return temp.Length;
            }
        }
        public List<__HeaderItem> Items { get; } = new List<__HeaderItem>();

        private String _typename = String.Empty;
        private Byte[] _typenameRaw = Array.Empty<Byte>();
    }
}

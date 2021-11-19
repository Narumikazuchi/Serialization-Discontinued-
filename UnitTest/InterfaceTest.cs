using Narumikazuchi.Serialization;
using System;

namespace UnitTest
{
    public partial struct InterfaceTest
    {
        public InterfaceTest()
        { }
        public InterfaceTest(Guid guid,
                             String name,
                             String description,
                             Int32 count,
                             Single rate,
                             Half small)
        {
            this.Guid = guid;
            this.Name = name;
            this.Description = description;
            this.Count = count;
            this.Rate = rate;
            this.Small = small;
        }

        public String Name { get; set; } = "Foo";
        public String Description { get; set; } = "Lorem ipsum colorem";
        public Guid Guid { get; set; } = Guid.NewGuid();
        public Int32 Count { get; set; } = 128;
        public Single Rate { get; set; } = 0.96445f;
        public Half Small { get; set; } = (Half)0.45554f;
    }

    partial struct InterfaceTest : ISerializable<InterfaceTest>
    {
        public static InterfaceTest ConstructFromSerializationData(SerializationInfo info)
        {
            return new(info.Get<Guid>(nameof(Guid)),
                       info.Get<String>(nameof(Name)),
                       info.Get<String>(nameof(Description)),
                       info.Get<Int32>(nameof(Count)),
                       info.Get<Single>(nameof(Rate)),
                       info.Get<Half>(nameof(Small)));
        }

        public void GetSerializationData(SerializationInfo info)
        {
            info.Add(nameof(this.Guid),
                     this.Guid);
            info.Add(nameof(this.Name),
                     this.Name);
            info.Add(nameof(this.Description),
                     this.Description);
            info.Add(nameof(this.Count),
                     this.Count);
            info.Add(nameof(this.Rate),
                     this.Rate);
            info.Add(nameof(this.Small),
                     this.Small);
        }
    }

    partial struct InterfaceTest : IEquatable<InterfaceTest>
    {
        public Boolean Equals(InterfaceTest other) =>
            this.Guid == other.Guid &&
            this.Name == other.Name &&
            this.Description == other.Description &&
            this.Count == other.Count &&
            this.Rate == other.Rate &&
            this.Small == other.Small;
    }
}

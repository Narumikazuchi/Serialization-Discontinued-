using System;

namespace UnitTest
{
    public sealed class UnmarkedTest
    {
        public String Name { get; set; } = "Foo";
        public String Description { get; set; } = "Lorem ipsum colorem";
        public Guid Guid { get; set; } = Guid.NewGuid();
        public Int32 Count { get; set; } = 128;
        public Single Rate { get; set; } = 0.96445f;
        public Half Small { get; set; } = (Half)0.45554f;
    }
}

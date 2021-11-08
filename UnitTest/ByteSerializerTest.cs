using Narumikazuchi.Serialization.Bytes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Narumikazuchi.Serialization;
using System;
using System.IO;
using System.Diagnostics.CodeAnalysis;

namespace UnitTest
{
    [TestClass]
    public partial class ByteSerializerTest
    {
        [TestMethod]
        public void SerializationTest()
        {
            using MemoryStream stream = new();
            ByteSerializer serializer = new();
            serializer.Serialize(stream, new Test());
            Assert.IsTrue(stream.Length > 0);
        }

        [TestMethod]
        public void GenericSerializationTest()
        {
            using MemoryStream stream = new();
            ByteSerializer<Test> serializer = new();
            serializer.Serialize(stream, new());
            Assert.IsTrue(stream.Length > 0);
        }

        [TestMethod]
        public void SerializationAfterActionTest()
        {
            using MemoryStream stream = new();
            ByteSerializer<Test> serializer = new();
            serializer.Serialize(stream, new(), SerializationFinishAction.CloseStream);
            Assert.ThrowsException<ObjectDisposedException>(() => stream.WriteByte(0x00));
        }

        [TestMethod]
        public void SerializationWrittenTest()
        {
            using MemoryStream stream = new();
            ByteSerializer serializer = new();
            UInt64 written = serializer.Serialize(stream, new Test());
            _instance.WriteLine(written.ToString());
        }

        [TestMethod]
        public void GenericSerializationWrittenTest()
        {
            using MemoryStream stream = new();
            ByteSerializer<Test> serializer = new();
            UInt64 written = serializer.Serialize(stream, new());
            _instance.WriteLine(written.ToString());
        }

        [TestMethod]
        public void DeserializationTest()
        {
            NonTest original = new();
            Test wow = new(Guid.NewGuid(),
                           "Hellcat",
                           "Excellion",
                           null);
            original.Child = wow;
            using MemoryStream stream = new();
            ByteSerializer serializer = new();
            serializer.Serialize(stream, original);

            stream.Seek(0, SeekOrigin.Begin);

            NonTest obj = (NonTest)serializer.Deserialize(stream, out UInt64 read);
            Assert.IsNotNull(obj);
            Assert.IsTrue(original.Equals(obj));
        }

        [TestMethod]
        public void GenericDeserializationTest()
        {
            Test original = new();
            Test wow = new(Guid.NewGuid(),
                           "Hellcat",
                           "Excellion",
                           null);
            original.Child = wow;
            using MemoryStream stream = new();
            ByteSerializer<Test> serializer = new();
            serializer.Serialize(stream, original);

            stream.Seek(0, SeekOrigin.Begin);

            Test obj = serializer.Deserialize(stream, out UInt64 read);
            Assert.IsNotNull(obj);
            Assert.IsTrue(original.Equals(obj));
        }

        [TestMethod]
        public void DeserializationAfterActionTest()
        {
            Test original = new();
            using MemoryStream stream = new();
            ByteSerializer<Test> serializer = new();
            serializer.Serialize(stream, original);

            stream.Seek(0, SeekOrigin.Begin);

            Test obj = serializer.Deserialize(stream, out UInt64 read, SerializationFinishAction.CloseStream);
            Assert.IsNotNull(obj);
            Assert.ThrowsException<ObjectDisposedException>(() => stream.WriteByte(0x00));
        }

        [TestMethod]
        public void DeserializationReadTest()
        {
            NonTest original = new();
            using MemoryStream stream = new();
            ByteSerializer serializer = new();
            serializer.Serialize(stream, original);

            stream.Seek(0, SeekOrigin.Begin);

            NonTest obj = (NonTest)serializer.Deserialize(stream, out UInt64 read);
            Assert.IsNotNull(obj);
            _instance.WriteLine(read.ToString());
        }

        [TestMethod]
        public void GenericDeserializationReadTest()
        {
            Test original = new();
            using MemoryStream stream = new();
            ByteSerializer<Test> serializer = new();
            serializer.Serialize(stream, original);

            stream.Seek(0, SeekOrigin.Begin);

            Test obj = serializer.Deserialize(stream, out UInt64 read);
            Assert.IsNotNull(obj);
            _instance.WriteLine(read.ToString());
        }

        public TestContext TestContext
        {
            get => this._instance;
            set => this._instance = value;
        }

        private TestContext _instance;
    }

    partial class ByteSerializerTest
    {
        [CustomSerializable]
        public class NonTest : IEquatable<NonTest>
        {
            public Boolean Equals(NonTest? other)
            {
                return other is not null &&
                       this.Name == other.Name &&
                       this.Guid == other.Guid;
            }

            public String Name { get; set; } = "Foo";
            [NotSerialized]
            public String Description { get; set; } = "Lorem ipsum colorem";
            public Guid Guid { get; set; } = Guid.NewGuid();
            [NotSerialized]
            public Test? Child { get; set; } = null;
        }

        public class Test : IEquatable<Test>, ISerializable<Test>
        {
            public Test()
            { }
            public Test(Guid guid,
                        String name,
                        String description,
                        Test? child)
            {
                this.Guid = guid;
                this.Name = name;
                this.Description = description;
                this.Child = child;
            }

            [return: NotNull]
            public static Test ConstructFromSerializationData([DisallowNull] SerializationInfo info)
            {
                return new(info.Get<Guid>(nameof(Guid)),
                           info.Get<String>(nameof(Name)),
                           info.Get<String>(nameof(Description)),
                           info.Get<Test>(nameof(Child)));
            }

            [return: NotNull]
            public void GetSerializationData([DisallowNull] SerializationInfo info)
            {
                info.Add(nameof(this.Guid),
                         this.Guid);
                info.Add(nameof(this.Name),
                         this.Name);
                info.Add(nameof(this.Description),
                         this.Description);
                info.Add(nameof(this.Child),
                         this.Child);
            }

            public Boolean Equals(Test? other)
            {
                if (other is null)
                {
                    return false;
                }
                if (this.Child is null &&
                    other.Child is null)
                {
                    return this.Name == other.Name &&
                           this.Description == other.Description &&
                           this.Guid == other.Guid;
                }
                if (this.Child is not null)
                {
                    return this.Child.Equals(other.Child) &&
                           this.Name == other.Name &&
                           this.Description == other.Description &&
                           this.Guid == other.Guid;
                }
                else if (other.Child is not null)
                {
                    return other.Child.Equals(this.Child) &&
                           this.Name == other.Name &&
                           this.Description == other.Description &&
                           this.Guid == other.Guid;
                }
                return false;
            }

            public String Name { get; set; } = "Foo";
            public String Description { get; set; } = "Lorem ipsum colorem";
            public Guid Guid { get; set; } = Guid.NewGuid();
            public Test? Child { get; set; } = null;
        }
    }
}

using Narumikazuchi.Serialization.Bytes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Narumikazuchi.Serialization;
using System;
using System.IO;

namespace UnitTest
{
    [TestClass]
    public class ByteSerializerTest
    {
        [TestMethod]
        public void SerializationTest()
        {
            using MemoryStream stream = new();
            ByteSerializer serializer = new();
            serializer.Serialize(stream, new AttributeTest());
            Assert.IsTrue(stream.Length > 0);
        }

        [TestMethod]
        public void GenericSerializationTest()
        {
            using MemoryStream stream = new();
            ByteSerializer<InterfaceTest> serializer = new();
            serializer.Serialize(stream, new());
            Assert.IsTrue(stream.Length > 0);
        }

        [TestMethod]
        public void UnfitSerializationTest()
        {
            using MemoryStream stream = new();
            ByteSerializer serializer = new();
            Assert.ThrowsException<NotSupportedException>(() => serializer.Serialize(stream, new UnmarkedTest()));
        }

        [TestMethod]
        public void SerializationAfterActionTest()
        {
            using MemoryStream stream = new();
            ByteSerializer<InterfaceTest> serializer = new();
            serializer.Serialize(stream, new(), SerializationFinishAction.CloseStream);
            Assert.ThrowsException<ObjectDisposedException>(() => stream.WriteByte(0x00));
        }

        [TestMethod]
        public void SerializationWrittenTest()
        {
            using MemoryStream stream = new();
            ByteSerializer serializer = new();
            UInt64 written = serializer.Serialize(stream, new InterfaceTest());
            _instance.WriteLine(written.ToString());
        }

        [TestMethod]
        public void GenericSerializationWrittenTest()
        {
            using MemoryStream stream = new();
            ByteSerializer<InterfaceTest> serializer = new();
            UInt64 written = serializer.Serialize(stream, new());
            _instance.WriteLine(written.ToString());
        }

        [TestMethod]
        public void DeserializationTest()
        {
            AttributeTest original = new();
            using MemoryStream stream = new();
            ByteSerializer serializer = new();
            serializer.Serialize(stream, original);

            stream.Seek(0, SeekOrigin.Begin);

            AttributeTest obj = (AttributeTest)serializer.Deserialize(stream, out UInt64 read);
            Assert.IsNotNull(obj);
            Assert.IsTrue(original.Equals(obj));
        }

        [TestMethod]
        public void GenericDeserializationTest()
        {
            InterfaceTest original = new();
            using MemoryStream stream = new();
            ByteSerializer<InterfaceTest> serializer = new();
            serializer.Serialize(stream, original);

            stream.Seek(0, SeekOrigin.Begin);

            InterfaceTest obj = serializer.Deserialize(stream, out UInt64 read);
            Assert.IsNotNull(obj);
            Assert.IsTrue(original.Equals(obj));
        }

        [TestMethod]
        public void DeserializationAfterActionTest()
        {
            InterfaceTest original = new();
            using MemoryStream stream = new();
            ByteSerializer<InterfaceTest> serializer = new();
            serializer.Serialize(stream, original);

            stream.Seek(0, SeekOrigin.Begin);

            InterfaceTest obj = serializer.Deserialize(stream, out UInt64 read, SerializationFinishAction.CloseStream);
            Assert.IsNotNull(obj);
            Assert.ThrowsException<ObjectDisposedException>(() => stream.WriteByte(0x00));
        }

        [TestMethod]
        public void DeserializationReadTest()
        {
            AttributeTest original = new();
            using MemoryStream stream = new();
            ByteSerializer serializer = new();
            serializer.Serialize(stream, original);

            stream.Seek(0, SeekOrigin.Begin);

            AttributeTest obj = (AttributeTest)serializer.Deserialize(stream, out UInt64 read);
            Assert.IsNotNull(obj);
            _instance.WriteLine(read.ToString());
        }

        [TestMethod]
        public void GenericDeserializationReadTest()
        {
            InterfaceTest original = new();
            using MemoryStream stream = new();
            ByteSerializer<InterfaceTest> serializer = new();
            serializer.Serialize(stream, original);

            stream.Seek(0, SeekOrigin.Begin);

            InterfaceTest obj = serializer.Deserialize(stream, out UInt64 read);
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
}

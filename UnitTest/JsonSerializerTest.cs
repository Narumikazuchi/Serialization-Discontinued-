using Narumikazuchi.Serialization.Json;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Narumikazuchi.Serialization;
using System;
using System.IO;

namespace UnitTest
{
    [TestClass]
    public class JsonSerializerTest
    {
        [TestMethod]
        public void SerializationTest()
        {
            using MemoryStream stream = new();
            JsonSerializer<InterfaceTest> serializer = new();
            serializer.Serialize(stream, new InterfaceTest());
            Assert.IsTrue(stream.Length > 0);
        }

        [TestMethod]
        public void SerializationAfterActionTest()
        {
            using MemoryStream stream = new();
            JsonSerializer<InterfaceTest> serializer = new();
            serializer.Serialize(stream, new(), SerializationFinishAction.CloseStream);
            Assert.ThrowsException<ObjectDisposedException>(() => stream.WriteByte(0x00));
        }

        [TestMethod]
        public void SerializationWrittenTest()
        {
            using MemoryStream stream = new();
            JsonSerializer<InterfaceTest> serializer = new();
            UInt64 written = serializer.Serialize(stream, new InterfaceTest());
            _instance.WriteLine(written.ToString());
        }

        [TestMethod]
        public void DeserializationTest()
        {
            InterfaceTest original = new();
            using MemoryStream stream = new();
            JsonSerializer<InterfaceTest> serializer = new();
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
            JsonSerializer<InterfaceTest> serializer = new();
            serializer.Serialize(stream, original);

            stream.Seek(0, SeekOrigin.Begin);

            InterfaceTest obj = serializer.Deserialize(stream, out UInt64 read, SerializationFinishAction.CloseStream);
            Assert.IsNotNull(obj);
            Assert.ThrowsException<ObjectDisposedException>(() => stream.WriteByte(0x00));
        }

        [TestMethod]
        public void DeserializationReadTest()
        {
            InterfaceTest original = new();
            using MemoryStream stream = new();
            JsonSerializer<InterfaceTest> serializer = new();
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

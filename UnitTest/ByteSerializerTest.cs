using Narumikazuchi.Serialization.Bytes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Narumikazuchi.Serialization;
using System;
using System.IO;
using System.Linq;

namespace UnitTest
{
    [TestClass]
    public class ByteSerializerTest
    {
        [TestMethod]
        public void SerializationTest()
        {
            using MemoryStream stream = new();
            ByteSerializer<Serializable> serializer = new();
            serializer.Serialize(stream, new());
            Assert.IsTrue(stream.Length > 0);
        }

        [TestMethod]
        public void DeserializationTest()
        {
            Serializable original = new();
            using MemoryStream stream = new();
            ByteSerializer<Serializable> serializer = new();
            serializer.Serialize(stream, original);

            stream.Seek(0, SeekOrigin.Begin);

            Serializable obj = serializer.Deserialize(stream);
            Assert.IsNotNull(obj);
            Assert.AreEqual(original._bytes.Length, obj._bytes.Length);
            Assert.IsTrue(obj._bytes.SequenceEqual(original._bytes));
        }

        [TestMethod]
        public void AbstractTest()
        {
            ByteSerializer<AbstractSerializable> serializer = new();
            Assert.IsNotNull(serializer);
        }
    }

    public class Serializable : IByteSerializable
    {
        UInt32 IByteSerializable.InitializeUninitializedState(Byte[] bytes)
        {
            this._bytes = new Byte[bytes.Length];
            Array.Copy(bytes, this._bytes, bytes.Length);
            return (UInt32)bytes.Length;
        }
        UInt32 IByteSerializable.SetState(Byte[] bytes)
        {
            this._bytes = new Byte[bytes.Length];
            Array.Copy(bytes, this._bytes, bytes.Length);
            return (UInt32)bytes.Length;
        }
        Byte[] ISerializable.ToBytes() => _bytes;

        public Byte[] _bytes = new Byte[] { 0xFF, 0x23, 0x69, 0xEA, 0x11, 0x5A, 0xC1, 0x0B };
    }

    public abstract class AbstractSerializable : IByteSerializable
    {
        UInt32 IByteSerializable.InitializeUninitializedState(Byte[] bytes)
        {
            this._bytes = new Byte[bytes.Length];
            Array.Copy(bytes, this._bytes, bytes.Length);
            return (UInt32)bytes.Length;
        }
        UInt32 IByteSerializable.SetState(Byte[] bytes)
        {
            this._bytes = new Byte[bytes.Length];
            Array.Copy(bytes, this._bytes, bytes.Length);
            return (UInt32)bytes.Length;
        }
        Byte[] ISerializable.ToBytes() => _bytes;

        public Byte[] _bytes = new Byte[] { 0xFF, 0x23, 0x69, 0xEA, 0x11, 0x5A, 0xC1, 0x0B };
    }
}

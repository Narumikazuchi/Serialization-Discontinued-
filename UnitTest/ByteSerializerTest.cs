using Narumikazuchi.Serialization.Bytes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Narumikazuchi.Serialization;
using System;
using System.IO;
using System.Linq;

namespace UnitTest
{
    [TestClass]
    public partial class ByteSerializerTest
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
        public void SerializationAfterActionTest()
        {
            using MemoryStream stream = new();
            ByteSerializer<Serializable> serializer = new();
            serializer.Serialize(stream, new(), SerializationFinishAction.CloseStream);
            Assert.ThrowsException<ObjectDisposedException>(() => stream.WriteByte(0x00));
        }

        [TestMethod]
        public void SerializationWrittenTest()
        {
            using MemoryStream stream = new();
            ByteSerializer<Serializable> serializer = new();
            UInt32 written = serializer.Serialize(stream, new());
            Assert.AreEqual((UInt32)12, written);
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
        public void DeserializationAfterActionTest()
        {
            Serializable original = new();
            using MemoryStream stream = new();
            ByteSerializer<Serializable> serializer = new();
            serializer.Serialize(stream, original);

            stream.Seek(0, SeekOrigin.Begin);

            Serializable obj = serializer.Deserialize(stream, SerializationFinishAction.CloseStream);
            Assert.IsNotNull(obj);
            Assert.AreEqual(original._bytes.Length, obj._bytes.Length);
            Assert.IsTrue(obj._bytes.SequenceEqual(original._bytes));
            Assert.ThrowsException<ObjectDisposedException>(() => stream.WriteByte(0x00));
        }

        [TestMethod]
        public void DeserializationReadTest()
        {
            Serializable original = new();
            using MemoryStream stream = new();
            ByteSerializer<Serializable> serializer = new();
            serializer.Serialize(stream, original);

            stream.Seek(0, SeekOrigin.Begin);

            Serializable obj = serializer.Deserialize(stream, out UInt32 read);
            Assert.IsNotNull(obj);
            Assert.AreEqual(original._bytes.Length, obj._bytes.Length);
            Assert.IsTrue(obj._bytes.SequenceEqual(original._bytes));
            Assert.AreEqual((UInt32)8, read);
        }

        [TestMethod]
        public void AbstractTest()
        {
            Assert.ThrowsException<InvalidOperationException>(() => _ = new ByteSerializer<AbstractSerializable>());
        }
    }

    partial class ByteSerializerTest
    {
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
}

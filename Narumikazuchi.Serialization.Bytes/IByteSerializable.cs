namespace Narumikazuchi.Serialization.Bytes
{
    /// <summary>
    /// Represents an object which can be serialized with the <see cref="ByteSerializer{T}"/> class.
    /// </summary>
    public interface IByteSerializable : ISerializable
    {
        /// <summary>
        /// Initializes this instance from it's byte representation.
        /// </summary>
        /// <param name="bytes">The byte representation of this instance.</param>
        /// <returns>The amount of bytes this method consumed from the <paramref name="bytes"/> parameter.</returns>
        /// <remarks>
        /// This method will only be used when no default constructor exists in the type definition.
        /// Keep in mind that this instance will be uninitialized, meaning default values for fields and properties won't be set
        /// and therefore need to be set within this method. <para/>
        /// When inheriting from another type you also need to initialize all default values of the parent type and their parent types. <para/>
        /// If the inherited type implements <see cref="IByteSerializable"/> as well, it's implementation of 
        /// <see cref="InitializeUninitializedState(System.Byte[])"/> will be called before the implementation of this type.
        /// </remarks>
        public System.UInt32 InitializeUninitializedState(System.Byte[] bytes);

        /// <summary>
        /// Sets the state of this instance from it's byte representation.
        /// </summary>
        /// <param name="bytes">The byte representation of this instance.</param>
        /// <returns>The amount of bytes this method consumed from the <paramref name="bytes"/> parameter.</returns>
        /// <remarks>
        /// This method will be called after the call to the default constructor of this type.<para/>
        /// If inheriting from another type and the inherited type implements <see cref="IByteSerializable"/> as well, it's implementation of 
        /// <see cref="SetState(System.Byte[])"/> will be called before the implementation of this type.
        /// </remarks>
        public System.UInt32 SetState(System.Byte[] bytes);
    }
}

namespace ByteBuffer.Abstractions
{
    using System;
    using System.Text;

    public interface IBinaryReader
    {
        /// <summary>
        ///     Reads a <see cref="byte"/> sequence.
        /// </summary>
        /// <param name="count">the number of bytes to read</param>
        /// <returns>the <see cref="byte"/> sequence</returns>
        byte[] Read(int count);

        /// <summary>
        ///     Reads a <see cref="bool"/> value.
        /// </summary>
        /// <returns>the value read</returns>
        bool ReadBoolean();

        /// <summary>
        ///     Reads a <see cref="byte"/>.
        /// </summary>
        /// <returns>the value read</returns>
        byte ReadByte();

        /// <summary>
        ///     Reads a <see cref="byte"/> sequence and writes it to the specified <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">the buffer</param>
        /// <param name="count">the number of bytes to read</param>
        void ReadBytes(byte[] buffer, int count);

        /// <summary>
        ///     Reads a <see cref="byte"/> sequence and writes it to the specified <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">the buffer</param>
        void ReadBytes(byte[] buffer);

        /// <summary>
        ///     Reads a <see cref="byte"/> sequence and writes it to the specified <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">the buffer</param>
        void ReadBytes(ArraySegment<byte> buffer);

        /// <summary>
        ///     Reads a <see cref="byte"/> sequence and writes it to the specified <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">the buffer</param>
        /// <param name="offset">the buffer write offset</param>
        /// <param name="count">the number of bytes to read</param>
        void ReadBytes(byte[] buffer, int offset, int count);

        /// <summary>
        ///     Reads a <see cref="double"/> value.
        /// </summary>
        /// <returns>the value read</returns>
        double ReadDouble();

        /// <summary>
        ///     Reads a <see cref="float"/> value.
        /// </summary>
        /// <returns>the value read</returns>
        float ReadFloat();

        /// <summary>
        ///     Reads a <see cref="Guid"/> value.
        /// </summary>
        /// <returns>the value read</returns>
        Guid ReadGuid();

        /// <summary>
        ///     Reads a <see cref="int"/> value.
        /// </summary>
        /// <returns>the value read</returns>
        int ReadInt();

        /// <summary>
        ///     Reads a <see cref="long"/> value.
        /// </summary>
        /// <returns>the value read</returns>
        long ReadLong();

        /// <summary>
        ///     Reads a <see cref="sbyte"/> value.
        /// </summary>
        /// <returns>the value read</returns>
        sbyte ReadSByte();

        /// <summary>
        ///     Reads a <see cref="short"/> value.
        /// </summary>
        /// <returns>the value read</returns>
        short ReadShort();

        /// <summary>
        ///     Reads an UTF-8 encoded, length-prefixed <see cref="string"/>.
        /// </summary>
        /// <returns>the string read</returns>
        string ReadString();

        /// <summary>
        ///     Reads a length-prefixed <see cref="string"/> using the specified <paramref name="encoding"/>.
        /// </summary>
        /// <param name="encoding">the encoding to use</param>
        /// <returns>the string read</returns>
        string ReadString(Encoding encoding);

        /// <summary>
        ///     Reads a <see cref="uint"/> value.
        /// </summary>
        /// <returns>the value read</returns>
        uint ReadUInt();

        /// <summary>
        ///     Reads a <see cref="ulong"/> value.
        /// </summary>
        /// <returns>the value read</returns>
        ulong ReadULong();

        /// <summary>
        ///     Reads a <see cref="ushort"/> value.
        /// </summary>
        /// <returns>the value read</returns>
        ushort ReadUShort();
    }
}
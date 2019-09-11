namespace BufferIO.IO
{
    using System;
    using System.Buffers;
    using System.IO;
    using System.Text;
    using Util;

    /// <summary>
    ///     Wrapper class for a big-endian binary reader that reads from an underlying stream.
    /// </summary>
    public class BigEndianReader : IDisposable
    {
        /// <summary>
        ///     The internal read buffer.
        /// </summary>
        /// <seealso cref="FillReadBuffer(int)"/>
        protected readonly byte[] _readBuffer;

        private readonly bool _leaveOpen;

        /// <summary>
        ///     Initializes a new instance of the <see cref="BigEndianReader"/> class.
        /// </summary>
        /// <param name="baseStream">the base stream to write to / read from</param>
        /// <param name="leaveOpen">
        ///     a value indicating whether the specified <paramref name="baseStream"/> should be left
        ///     open when the <see cref="BigEndianReader"/> is closed.
        /// </param>
        public BigEndianReader(Stream baseStream, bool leaveOpen = false)
        {
            BaseStream = baseStream ?? throw new ArgumentNullException(nameof(baseStream));
            _leaveOpen = leaveOpen;
            _readBuffer = new byte[8];
        }

        /// <summary>
        ///     Gets the base stream to write to / read from.
        /// </summary>
        public Stream BaseStream { get; }

        /// <summary>
        ///     Disposes the <see cref="BaseStream"/> if specified in constructor (leaveOpen = <see langword="false"/>).
        /// </summary>
        public void Dispose()
        {
            // check whether to dispose the base stream
            if (!_leaveOpen)
            {
                BaseStream.Dispose();
            }
        }

        /// <summary>
        ///     Reads a <see cref="byte"/> sequence.
        /// </summary>
        /// <param name="count">the number of bytes to read</param>
        /// <returns>the <see cref="byte"/> sequence</returns>
        public byte[] Read(int count)
        {
            var buffer = new byte[count];
            ReadBytes(buffer);
            return buffer;
        }

        /// <summary>
        ///     Reads a <see cref="bool"/> value.
        /// </summary>
        /// <returns>the value read</returns>
        public bool ReadBoolean() => ReadByte() != 0;

        /// <summary>
        ///     Reads a <see cref="byte"/>.
        /// </summary>
        /// <returns>the value read</returns>
        public byte ReadByte()
        {
            var value = BaseStream.ReadByte();

            if (value < 0)
            {
                throw new EndOfStreamException();
            }

            return (byte)value;
        }

        /// <summary>
        ///     Reads a <see cref="byte"/> sequence and writes it to the specified <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">the buffer</param>
        /// <param name="count">the number of bytes to read</param>
        public void ReadBytes(byte[] buffer, int count) => ReadBytes(buffer, offset: 0, count);

        /// <summary>
        ///     Reads a <see cref="byte"/> sequence and writes it to the specified <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">the buffer</param>
        public void ReadBytes(byte[] buffer) => ReadBytes(buffer, offset: 0, buffer.Length);

        /// <summary>
        ///     Reads a <see cref="byte"/> sequence and writes it to the specified <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">the buffer</param>
        public void ReadBytes(ArraySegment<byte> buffer) => ReadBytes(buffer.Array, buffer.Offset, buffer.Count);

        /// <summary>
        ///     Reads a <see cref="byte"/> sequence and writes it to the specified <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">the buffer</param>
        /// <param name="offset">the buffer write offset</param>
        /// <param name="count">the number of bytes to read</param>
        public void ReadBytes(byte[] buffer, int offset, int count)
        {
            if (BaseStream.Read(buffer, offset, count) < 0)
            {
                throw new EndOfStreamException();
            }
        }

        /// <summary>
        ///     Reads a <see cref="double"/> value.
        /// </summary>
        /// <returns>the value read</returns>
        public double ReadDouble()
        {
            FillReadBuffer(sizeof(double));
            return BigEndian.ToDouble(_readBuffer);
        }

        /// <summary>
        ///     Reads a <see cref="float"/> value.
        /// </summary>
        /// <returns>the value read</returns>
        public float ReadFloat()
        {
            FillReadBuffer(sizeof(float));
            return BigEndian.ToFloat(_readBuffer);
        }

        /// <summary>
        ///     Reads a <see cref="Guid"/> value.
        /// </summary>
        /// <returns>the value read</returns>
        public Guid ReadGuid() => new Guid(Read(16));

        /// <summary>
        ///     Reads a <see cref="int"/> value.
        /// </summary>
        /// <returns>the value read</returns>
        public int ReadInt()
        {
            FillReadBuffer(sizeof(int));
            return BigEndian.ToInt32(_readBuffer);
        }

        /// <summary>
        ///     Reads a <see cref="long"/> value.
        /// </summary>
        /// <returns>the value read</returns>
        public long ReadLong()
        {
            FillReadBuffer(sizeof(long));
            return BigEndian.ToInt64(_readBuffer);
        }

        /// <summary>
        ///     Reads a <see cref="sbyte"/> value.
        /// </summary>
        /// <returns>the value read</returns>
        public sbyte ReadSByte() => (sbyte)ReadByte();

        /// <summary>
        ///     Reads a <see cref="short"/> value.
        /// </summary>
        /// <returns>the value read</returns>
        public short ReadShort()
        {
            FillReadBuffer(sizeof(short));
            return BigEndian.ToInt16(_readBuffer);
        }

        /// <summary>
        ///     Reads an UTF-8 encoded, length-prefixed <see cref="string"/>.
        /// </summary>
        /// <returns>the string read</returns>
        public string ReadString() => ReadString(Encoding.UTF8);

        /// <summary>
        ///     Reads a length-prefixed <see cref="string"/> using the specified <paramref name="encoding"/>.
        /// </summary>
        /// <param name="encoding">the encoding to use</param>
        /// <returns>the string read</returns>
        public string ReadString(Encoding encoding)
        {
            // read the length prefix (a 2-byte long ushort indicating the number of bytes the string has)
            var byteCount = ReadUShort();

            // rent a buffer that can hold the string
            var pooledBuffer = ArrayPool<byte>.Shared.Rent(byteCount);

            // ensure the pooled buffer is returned to the pool even if an exception is thrown
            try
            {
                // read data
                ReadBytes(pooledBuffer, byteCount);

                // decode string
                return encoding.GetString(pooledBuffer, index: 0, byteCount);
            }
            finally
            {
                // release / return the buffer to the array pool
                ArrayPool<byte>.Shared.Return(pooledBuffer);
            }
        }

        /// <summary>
        ///     Reads a <see cref="uint"/> value.
        /// </summary>
        /// <returns>the value read</returns>
        public uint ReadUInt()
        {
            FillReadBuffer(sizeof(uint));
            return BigEndian.ToUInt32(_readBuffer);
        }

        /// <summary>
        ///     Reads a <see cref="ulong"/> value.
        /// </summary>
        /// <returns>the value read</returns>
        public ulong ReadULong()
        {
            FillReadBuffer(sizeof(ulong));
            return BigEndian.ToUInt64(_readBuffer);
        }

        /// <summary>
        ///     Reads an UTF-8 encoded <see cref="string"/>.
        /// </summary>
        /// <param name="encodedLength">the encoded length of the string</param>
        /// <returns>the string read</returns>
        public string ReadUnprefixedString(int encodedLength)
            => ReadUnprefixedString(encodedLength, Encoding.UTF8);

        /// <summary>
        ///     Reads a <see cref="string"/> using the specified <paramref name="encoding"/>.
        /// </summary>
        /// <param name="encodedLength">the encoded length of the string</param>
        /// <param name="encoding">the encoding to use</param>
        /// <returns>the string read</returns>
        public string ReadUnprefixedString(int encodedLength, Encoding encoding)
        {
            // rent a buffer that can hold the string
            var pooledBuffer = ArrayPool<byte>.Shared.Rent(encodedLength);

            // ensure the pooled buffer is returned to the pool even if an exception is thrown
            try
            {
                // read data
                ReadBytes(pooledBuffer, encodedLength);

                // decode string
                return encoding.GetString(pooledBuffer, index: 0, encodedLength);
            }
            finally
            {
                // release / return the buffer to the array pool
                ArrayPool<byte>.Shared.Return(pooledBuffer);
            }
        }

        /// <summary>
        ///     Reads a <see cref="ushort"/> value.
        /// </summary>
        /// <returns>the value read</returns>
        public ushort ReadUShort()
        {
            FillReadBuffer(sizeof(ushort));
            return BigEndian.ToUInt16(_readBuffer);
        }

        /// <summary>
        ///     Fills the read buffer with the specified number of bytes ( <paramref name="count"/>).
        /// </summary>
        /// <param name="count">the number of bytes to fill the read buffer with</param>
        protected void FillReadBuffer(int count)
        {
            if (BaseStream.Read(_readBuffer, offset: 0, count) < count)
            {
                throw new EndOfStreamException();
            }
        }
    }
}
namespace BufferIO.IO
{
    using System;
    using System.Buffers;
    using System.IO;
    using System.Text;
    using Util;

    /// <summary>
    ///     Wrapper class for a big-endian binary writer that writes to an underlying stream.
    /// </summary>
    public class BinaryWriter : IDisposable
    {
        /// <summary>
        ///     The maximum encoded byte size of string.
        /// </summary>
        public const int MaximumStringByteSize = 0xFFFF;

        /// <summary>
        ///     The internal write buffer.
        /// </summary>
        /// <seealso cref="FlushWriteBuffer(int)"/>
        protected readonly byte[] _writeBuffer;

        private readonly bool _leaveOpen;

        /// <summary>
        ///     Initializes a new instance of the <see cref="BinaryWriter"/> class.
        /// </summary>
        /// <param name="baseStream">the base stream to write to / read from</param>
        /// <param name="leaveOpen">
        ///     a value indicating whether the specified <paramref name="baseStream"/> should be left
        ///     open when the <see cref="BinaryWriter"/> is closed.
        /// </param>
        public BinaryWriter(Stream baseStream, bool leaveOpen = false)
        {
            BaseStream = baseStream ?? throw new ArgumentNullException(nameof(baseStream));
            _leaveOpen = leaveOpen;

            _writeBuffer = new byte[8];
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
        ///     Writes the specified <paramref name="value"/> to the buffer.
        /// </summary>
        /// <param name="value">the value to write</param>
        public void Write(bool value) => Write((byte)(value ? 1 : 0));

        /// <summary>
        ///     Writes the specified <paramref name="value"/> to the buffer.
        /// </summary>
        /// <param name="value">the value to write</param>
        public void Write(byte value) => BaseStream.WriteByte(value);

        /// <summary>
        ///     Writes the specified <paramref name="buffer"/> to the internal buffer.
        /// </summary>
        /// <param name="buffer">the buffer</param>
        /// <exception cref="ArgumentNullException">
        ///     thrown if the specified <paramref name="buffer"/> is <see langword="null"/>.
        /// </exception>
        public void Write(byte[] buffer) => BaseStream.Write(buffer, 0, count: buffer.Length);

        /// <summary>
        ///     Writes the specified <paramref name="buffer"/> to the internal buffer.
        /// </summary>
        /// <param name="buffer">the buffer</param>
        public void Write(ArraySegment<byte> buffer) => BaseStream.Write(buffer.Array, buffer.Offset, buffer.Count);

        /// <summary>
        ///     Writes the specified <paramref name="buffer"/> to the internal buffer.
        /// </summary>
        /// <param name="buffer">the buffer</param>
        /// <param name="offset">the buffer read offset</param>
        /// <param name="count">the number of bytes to write</param>
        /// <exception cref="ArgumentNullException">
        ///     thrown if the specified <paramref name="buffer"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     thrown if the specified <paramref name="offset"/> is negative.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     thrown if the specified <paramref name="count"/> is negative.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     thrown if the specified <paramref name="buffer"/> is too small for the specified <paramref name="count"/>.
        /// </exception>
        public void Write(byte[] buffer, int offset, int count) => BaseStream.Write(buffer, offset, count);

        /// <summary>
        ///     Writes the specified <paramref name="value"/> to the buffer.
        /// </summary>
        /// <param name="value">the value to write</param>
        public void Write(int value)
        {
            BigEndian.GetBytes(_writeBuffer, value);
            FlushWriteBuffer(sizeof(int));
        }

        /// <summary>
        ///     Writes the specified <paramref name="value"/> to the buffer.
        /// </summary>
        /// <param name="value">the value to write</param>
        public void Write(uint value)
        {
            BigEndian.GetBytes(_writeBuffer, value);
            FlushWriteBuffer(sizeof(uint));
        }

        /// <summary>
        ///     Writes the specified <paramref name="value"/> to the buffer.
        /// </summary>
        /// <param name="value">the value to write</param>
        public void Write(ushort value)
        {
            BigEndian.GetBytes(_writeBuffer, value);
            FlushWriteBuffer(sizeof(ushort));
        }

        /// <summary>
        ///     Writes the specified <paramref name="value"/> to the buffer.
        /// </summary>
        /// <param name="value">the value to write</param>
        public void Write(float value)
        {
            BigEndian.GetBytes(_writeBuffer, value);
            FlushWriteBuffer(sizeof(float));
        }

        /// <summary>
        ///     Writes the specified <paramref name="value"/> to the buffer.
        /// </summary>
        /// <param name="value">the value to write</param>
        public void Write(double value)
        {
            BigEndian.GetBytes(_writeBuffer, value);
            FlushWriteBuffer(sizeof(double));
        }

        /// <summary>
        ///     Writes the specified <paramref name="value"/> to the buffer.
        /// </summary>
        /// <param name="value">the value to write</param>
        public void Write(short value)
        {
            BigEndian.GetBytes(_writeBuffer, value);
            FlushWriteBuffer(sizeof(short));
        }

        /// <summary>
        ///     Writes the specified <paramref name="value"/> to the buffer.
        /// </summary>
        /// <param name="value">the value to write</param>
        public void Write(sbyte value) => Write((byte)value);

        /// <summary>
        ///     Writes the specified <paramref name="value"/> to the buffer.
        /// </summary>
        /// <param name="value">the value to write</param>
        public void Write(long value)
        {
            BigEndian.GetBytes(_writeBuffer, value);
            FlushWriteBuffer(sizeof(long));
        }

        /// <summary>
        ///     Writes the specified <paramref name="value"/> to the buffer.
        /// </summary>
        /// <param name="value">the value to write</param>
        public void Write(ulong value)
        {
            BigEndian.GetBytes(_writeBuffer, value);
            FlushWriteBuffer(sizeof(ulong));
        }

        /// <summary>
        ///     Writes the specified <paramref name="value"/> to the buffer.
        /// </summary>
        /// <param name="value">the value to write</param>
        public void Write(Guid value) => Write(value.ToByteArray());

        /// <summary>
        ///     Writes a string encoded in UTF-8 prefixed with a 2-byte <see cref="ushort"/> length
        ///     prefix to the buffer.
        /// </summary>
        /// <param name="value">the value to write</param>
        /// <param name="charCount">the number of characters</param>
        /// <returns>the number of total bytes written (including 2-byte length prefix)</returns>
        public int Write(string value, int charCount) => Write(value, charCount, Encoding.UTF8);

        /// <summary>
        ///     Writes a string encoded in UTF-8 prefixed with a 2-byte <see cref="ushort"/> length
        ///     prefix to the buffer.
        /// </summary>
        /// <param name="value">the value to write</param>
        /// <returns>the number of total bytes written (including 2-byte length prefix)</returns>
        /// <exception cref="ArgumentException">
        ///     thrown if the specified <paramref name="value"/> overflows the maximum encoded byte
        ///     length ( <c>0xFFFF</c>)
        /// </exception>
        public int Write(string value) => Write(value, Encoding.UTF8);

        /// <summary>
        ///     Writes a string encoded in UTF-8 prefixed with a 2-byte <see cref="ushort"/> length
        ///     prefix to the buffer.
        /// </summary>
        /// <param name="value">the value to write</param>
        /// <param name="charIndex">the character index</param>
        /// <param name="charCount">the number of characters</param>
        /// <returns>the number of total bytes written (including 2-byte length prefix)</returns>
        /// <exception cref="ArgumentException">
        ///     thrown if the specified <paramref name="value"/> overflows the maximum encoded byte
        ///     length ( <c>0xFFFF</c>)
        /// </exception>
        public int Write(string value, int charIndex, int charCount) => Write(value, charIndex, charCount, Encoding.UTF8);

        /// <summary>
        ///     Writes a string encoded in the specified <paramref name="encoding"/> prefixed with a
        ///     2-byte <see cref="ushort"/> length prefix to the buffer.
        /// </summary>
        /// <param name="value">the value to write</param>
        /// <param name="charCount">the number of characters</param>
        /// <param name="encoding">the encoding to use</param>
        /// <returns>the number of total bytes written (including 2-byte length prefix)</returns>
        /// <exception cref="ArgumentException">
        ///     thrown if the specified <paramref name="value"/> overflows the maximum encoded byte
        ///     length ( <c>0xFFFF</c>)
        /// </exception>
        public int Write(string value, int charCount, Encoding encoding) => Write(value, charIndex: 0, charCount, encoding);

        /// <summary>
        ///     Writes a string encoded in the specified <paramref name="encoding"/> prefixed with a
        ///     2-byte <see cref="ushort"/> length prefix to the buffer.
        /// </summary>
        /// <param name="value">the value to write</param>
        /// <param name="encoding">the encoding to use</param>
        /// <returns>the number of total bytes written (including 2-byte length prefix)</returns>
        /// <exception cref="ArgumentException">
        ///     thrown if the specified <paramref name="value"/> overflows the maximum encoded byte
        ///     length ( <c>0xFFFF</c>)
        /// </exception>
        public int Write(string value, Encoding encoding) => Write(value, charIndex: 0, charCount: value.Length, encoding);

        /// <summary>
        ///     Writes a string encoded in the specified <paramref name="encoding"/> prefixed with a
        ///     2-byte <see cref="ushort"/> length prefix to the buffer.
        /// </summary>
        /// <param name="value">the value to write</param>
        /// <param name="charIndex">the character index</param>
        /// <param name="charCount">the number of characters</param>
        /// <param name="encoding">the encoding to use</param>
        /// <returns>the number of total bytes written (including 2-byte length prefix)</returns>
        /// <exception cref="ArgumentException">
        ///     thrown if the specified <paramref name="value"/> overflows the maximum encoded byte
        ///     length ( <c>0xFFFF</c>)
        /// </exception>
        public int Write(string value, int charIndex, int charCount, Encoding encoding)
        {
            // rent a buffer that can hold the string and the 2-byte ushort length prefix
            var pooledBuffer = ArrayPool<byte>.Shared.Rent(encoding.GetMaxByteCount(charCount) + 2);

            // ensure the pooled buffer is returned to the pool even if an exception is thrown
            try
            {
                // encode the string starting at buffer offset 2
                var length = encoding.GetBytes(value, charIndex, charCount, pooledBuffer, byteIndex: 2);

                // ensure the length does not overflow
                if (length > MaximumStringByteSize)
                {
                    throw new ArgumentException($"The specified string overflows the maximum " +
                        $"encoded byte length ({MaximumStringByteSize}, 0xFFFF)", nameof(value));
                }

                // encode length prefix
                pooledBuffer[0] = (byte)(length >> 8);
                pooledBuffer[1] = (byte)(length & 0xFF);

                // write buffer
                Write(pooledBuffer, offset: 0, length + 2);

                return length + 2;
            }
            finally
            {
                // release / return the buffer to the array pool
                ArrayPool<byte>.Shared.Return(pooledBuffer);
            }
        }

        /// <summary>
        ///     Flushes the write buffer.
        /// </summary>
        /// <param name="count">the number of bytes to flush</param>
        protected void FlushWriteBuffer(int count)
            => BaseStream.Write(_writeBuffer, offset: 0, count);
    }
}
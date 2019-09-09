namespace ByteBuffer.IO
{
    using System;
    using System.IO;
    using System.Text;
    using Abstractions;

    public class BinaryStream : IBinaryWriter, IBinaryReader, IDisposable
    {
        /// <summary>
        ///     The maximum byte size for the highest writable / readable value.
        /// </summary>
        public const int MaximumValueSize = 8;

        protected readonly byte[] _readBuffer;
        protected readonly byte[] _writeBuffer;
        private readonly bool _leaveOpen;

        /// <summary>
        ///     Initializes a new instance of the <see cref="BinaryStream"/> class.
        /// </summary>
        /// <param name="baseStream">the base stream to write to / read from</param>
        /// <param name="leaveOpen">
        ///     a value indicating whether the specified <paramref name="baseStream"/> should be left
        ///     open when the <see cref="BinaryStream"/> is closed.
        /// </param>
        public BinaryStream(Stream baseStream, bool leaveOpen = false)
        {
            BaseStream = baseStream ?? throw new ArgumentNullException(nameof(baseStream));
            _leaveOpen = leaveOpen;

            _writeBuffer = new byte[MaximumValueSize];
            _readBuffer = new byte[MaximumValueSize];
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
            // TODO
            throw new NotImplementedException();
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
        ///     Reads a <see cref="ushort"/> value.
        /// </summary>
        /// <returns>the value read</returns>
        public ushort ReadUShort()
        {
            FillReadBuffer(sizeof(ushort));
            return BigEndian.ToUInt16(_readBuffer);
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
        /// <exception cref="InvalidOperationException">
        ///     thrown if the buffer is read-only ( <see cref="IsReadOnly"/>)
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     thrown if the buffer is not expandable ( <see cref="IsExpandable"/>)
        /// </exception>
        public void Write(byte[] buffer) => BaseStream.Write(buffer, 0, count: buffer.Length);

        /// <summary>
        ///     Writes the specified <paramref name="buffer"/> to the internal buffer.
        /// </summary>
        /// <param name="buffer">the buffer</param>
        /// <param name="offset">the buffer read offset</param>
        /// <param name="count">the number of bytes to write</param>
        /// <exception cref="InvalidOperationException">
        ///     thrown if the buffer is read-only ( <see cref="IsReadOnly"/>)
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     thrown if the buffer is not expandable ( <see cref="IsExpandable"/>)
        /// </exception>
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
        /// <exception cref="InvalidOperationException">
        ///     thrown if the buffer is read-only ( <see cref="IsReadOnly"/>)
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     thrown if the buffer is not expandable ( <see cref="IsExpandable"/>)
        /// </exception>
        public void Write(byte[] buffer, int offset, int count) => BaseStream.Write(buffer, offset, count);

        /// <summary>
        ///     Writes the specified <paramref name="value"/> to the buffer.
        /// </summary>
        /// <param name="value">the value to write</param>
        /// <exception cref="InvalidOperationException">
        ///     thrown if the buffer is read-only ( <see cref="IsReadOnly"/>)
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     thrown if the buffer is not expandable ( <see cref="IsExpandable"/>)
        /// </exception>
        public void Write(int value)
        {
            BigEndian.GetBytes(_writeBuffer, value);
            FlushWriteBuffer(sizeof(int));
        }

        /// <summary>
        ///     Writes the specified <paramref name="value"/> to the buffer.
        /// </summary>
        /// <param name="value">the value to write</param>
        /// <exception cref="InvalidOperationException">
        ///     thrown if the buffer is read-only ( <see cref="IsReadOnly"/>)
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     thrown if the buffer is not expandable ( <see cref="IsExpandable"/>)
        /// </exception>
        public void Write(uint value)
        {
            BigEndian.GetBytes(_writeBuffer, value);
            FlushWriteBuffer(sizeof(uint));
        }

        /// <summary>
        ///     Writes the specified <paramref name="value"/> to the buffer.
        /// </summary>
        /// <param name="value">the value to write</param>
        /// <exception cref="InvalidOperationException">
        ///     thrown if the buffer is read-only ( <see cref="IsReadOnly"/>)
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     thrown if the buffer is not expandable ( <see cref="IsExpandable"/>)
        /// </exception>
        public void Write(ushort value)
        {
            BigEndian.GetBytes(_writeBuffer, value);
            FlushWriteBuffer(sizeof(ushort));
        }

        /// <summary>
        ///     Writes the specified <paramref name="value"/> to the buffer.
        /// </summary>
        /// <param name="value">the value to write</param>
        /// <exception cref="InvalidOperationException">
        ///     thrown if the buffer is read-only ( <see cref="IsReadOnly"/>)
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     thrown if the buffer is not expandable ( <see cref="IsExpandable"/>)
        /// </exception>
        public void Write(float value)
        {
            BigEndian.GetBytes(_writeBuffer, value);
            FlushWriteBuffer(sizeof(float));
        }

        /// <summary>
        ///     Writes the specified <paramref name="value"/> to the buffer.
        /// </summary>
        /// <param name="value">the value to write</param>
        /// <exception cref="InvalidOperationException">
        ///     thrown if the buffer is read-only ( <see cref="IsReadOnly"/>)
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     thrown if the buffer is not expandable ( <see cref="IsExpandable"/>)
        /// </exception>
        public void Write(double value)
        {
            BigEndian.GetBytes(_writeBuffer, value);
            FlushWriteBuffer(sizeof(double));
        }

        /// <summary>
        ///     Writes the specified <paramref name="value"/> to the buffer.
        /// </summary>
        /// <param name="value">the value to write</param>
        /// <exception cref="InvalidOperationException">
        ///     thrown if the buffer is read-only ( <see cref="IsReadOnly"/>)
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     thrown if the buffer is not expandable ( <see cref="IsExpandable"/>)
        /// </exception>
        public void Write(short value)
        {
            BigEndian.GetBytes(_writeBuffer, value);
            FlushWriteBuffer(sizeof(short));
        }

        /// <summary>
        ///     Writes the specified <paramref name="value"/> to the buffer.
        /// </summary>
        /// <param name="value">the value to write</param>
        /// <exception cref="InvalidOperationException">
        ///     thrown if the buffer is read-only ( <see cref="IsReadOnly"/>)
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     thrown if the buffer is not expandable ( <see cref="IsExpandable"/>)
        /// </exception>
        public void Write(sbyte value) => Write((byte)value);

        /// <summary>
        ///     Writes the specified <paramref name="value"/> to the buffer.
        /// </summary>
        /// <param name="value">the value to write</param>
        /// <exception cref="InvalidOperationException">
        ///     thrown if the buffer is read-only ( <see cref="IsReadOnly"/>)
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     thrown if the buffer is not expandable ( <see cref="IsExpandable"/>)
        /// </exception>
        public void Write(long value)
        {
            BigEndian.GetBytes(_writeBuffer, value);
            FlushWriteBuffer(sizeof(long));
        }

        /// <summary>
        ///     Writes the specified <paramref name="value"/> to the buffer.
        /// </summary>
        /// <param name="value">the value to write</param>
        /// <exception cref="InvalidOperationException">
        ///     thrown if the buffer is read-only ( <see cref="IsReadOnly"/>)
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     thrown if the buffer is not expandable ( <see cref="IsExpandable"/>)
        /// </exception>
        public void Write(ulong value)
        {
            BigEndian.GetBytes(_writeBuffer, value);
            FlushWriteBuffer(sizeof(ulong));
        }

        /// <summary>
        ///     Writes the specified <paramref name="value"/> to the buffer.
        /// </summary>
        /// <param name="value">the value to write</param>
        /// <exception cref="InvalidOperationException">
        ///     thrown if the buffer is read-only ( <see cref="IsReadOnly"/>)
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     thrown if the buffer is not expandable ( <see cref="IsExpandable"/>)
        /// </exception>
        public void Write(Guid value) => Write(value.ToByteArray());

        /// <summary>
        ///     Writes a string encoded in UTF-8 prefixed with a 2-byte <see cref="ushort"/> length
        ///     prefix to the buffer.
        /// </summary>
        /// <param name="value">the value to write</param>
        /// <param name="charCount">the number of characters</param>
        /// <returns>the number of total bytes written (including 2-byte length prefix)</returns>
        /// <exception cref="InvalidOperationException">
        ///     thrown if the buffer is read-only ( <see cref="IsReadOnly"/>)
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     thrown if the buffer is not expandable ( <see cref="IsExpandable"/>)
        /// </exception>
        public int Write(string value, int charCount) => Write(value, charCount, Encoding.UTF8);

        /// <summary>
        ///     Writes a string encoded in UTF-8 prefixed with a 2-byte <see cref="ushort"/> length
        ///     prefix to the buffer.
        /// </summary>
        /// <param name="value">the value to write</param>
        /// <returns>the number of total bytes written (including 2-byte length prefix)</returns>
        /// <exception cref="InvalidOperationException">
        ///     thrown if the buffer is read-only ( <see cref="IsReadOnly"/>)
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     thrown if the buffer is not expandable ( <see cref="IsExpandable"/>)
        /// </exception>
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
        /// <exception cref="InvalidOperationException">
        ///     thrown if the buffer is read-only ( <see cref="IsReadOnly"/>)
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     thrown if the buffer is not expandable ( <see cref="IsExpandable"/>)
        /// </exception>
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
        /// <exception cref="InvalidOperationException">
        ///     thrown if the buffer is read-only ( <see cref="IsReadOnly"/>)
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     thrown if the buffer is not expandable ( <see cref="IsExpandable"/>)
        /// </exception>
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
        /// <exception cref="InvalidOperationException">
        ///     thrown if the buffer is read-only ( <see cref="IsReadOnly"/>)
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     thrown if the buffer is not expandable ( <see cref="IsExpandable"/>)
        /// </exception>
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
        /// <exception cref="InvalidOperationException">
        ///     thrown if the buffer is read-only ( <see cref="IsReadOnly"/>)
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     thrown if the buffer is not expandable ( <see cref="IsExpandable"/>)
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     thrown if the specified <paramref name="value"/> overflows the maximum encoded byte
        ///     length ( <c>0xFFFF</c>)
        /// </exception>
        public int Write(string value, int charIndex, int charCount, Encoding encoding)
        {
            // TODO
            throw new NotImplementedException();
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

        /// <summary>
        ///     Flushes the write buffer.
        /// </summary>
        /// <param name="count">the number of bytes to flush</param>
        protected void FlushWriteBuffer(int count)
            => BaseStream.Write(_writeBuffer, offset: 0, count);
    }
}
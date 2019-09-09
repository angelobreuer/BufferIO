namespace ByteBuffer
{
    using System;
    using System.Buffers;
    using System.IO;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    public unsafe class ByteBuffer : IBuffer, IDisposable
    {
        /// <summary>
        ///     Gets the default initial capacity.
        /// </summary>
        public const int DefaultInitialCapacity = 256;

        /// <summary>
        ///     An empty byte buffer.
        /// </summary>
        private static readonly byte[] _empty = new byte[0];

        /// <summary>
        ///     The general buffer write offset (not the writing position).
        /// </summary>
        private readonly int _origin;

        /// <summary>
        ///     The array pool the buffer was rent from.
        /// </summary>
        private readonly ArrayPool<byte> _pool;

        /// <summary>
        ///     The local buffer storing the data.
        /// </summary>
        private byte[] _buffer;

        /// <summary>
        ///     The number of bytes allocated for the buffer.
        /// </summary>
        private int _capacity;

        /// <summary>
        ///     The current read / write position in the <see cref="_buffer"/>.
        /// </summary>
        private int _cursor;

        /// <summary>
        ///     A value indicating whether the buffer was disposed.
        /// </summary>
        private bool _disposed;

        /// <summary>
        ///     The buffer length.
        /// </summary>
        private int _length;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ByteBuffer"/> class.
        /// </summary>
        /// <param name="buffer">the buffer</param>
        /// <param name="offset">the buffer offset</param>
        /// <param name="count">the buffer byte count allocated for the buffer</param>
        /// <param name="writable">a value indicating whether the buffer should support writing</param>
        /// <param name="exposable">
        ///     a value indicating whether the specified <paramref name="buffer"/> should be exposable
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     thrown if the specified <paramref name="buffer"/> is <see langword="null"/>.
        /// </exception>
        public ByteBuffer(byte[] buffer, int offset, int count, bool writable = true, bool exposable = true)
        {
            _buffer = buffer ?? throw new ArgumentNullException(nameof(buffer));
            _origin = offset;
            _cursor = offset;
            _length = count;
            _capacity = count;

            IsExposable = exposable;
            IsReadOnly = !writable;
            IsExpandable = false;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ByteBuffer"/> class.
        /// </summary>
        /// <param name="buffer">the buffer</param>
        /// <param name="count">the buffer byte count allocated for the buffer</param>
        /// <param name="writable">a value indicating whether the buffer should support writing</param>
        /// <param name="exposable">
        ///     a value indicating whether the specified <paramref name="buffer"/> should be exposable
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     thrown if the specified <paramref name="buffer"/> is <see langword="null"/>.
        /// </exception>
        public ByteBuffer(byte[] buffer, int count, bool writable = true, bool exposable = true)
            : this(buffer, offset: 0, count, writable, exposable)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ByteBuffer"/> class.
        /// </summary>
        /// <param name="buffer">the buffer</param>
        /// <param name="writable">a value indicating whether the buffer should support writing</param>
        /// <param name="exposable">
        ///     a value indicating whether the specified <paramref name="buffer"/> should be exposable
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     thrown if the specified <paramref name="buffer"/> is <see langword="null"/>.
        /// </exception>
        public ByteBuffer(byte[] buffer, bool writable = true, bool exposable = true)
            : this(buffer, offset: 0, buffer.Length, writable, exposable)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ByteBuffer"/> class.
        /// </summary>
        /// <param name="buffer">the buffer</param>
        /// <param name="writable">a value indicating whether the buffer should support writing</param>
        /// <param name="exposable">
        ///     a value indicating whether the specified <paramref name="buffer"/> should be exposable
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     thrown if the specified <paramref name="buffer"/> is <see langword="null"/>.
        /// </exception>
        public ByteBuffer(ArraySegment<byte> buffer, bool writable = true, bool exposable = true)
            : this(buffer.Array, buffer.Offset, buffer.Count, writable, exposable)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ByteBuffer"/> class with a pooled buffer backend.
        /// </summary>
        /// <param name="arrayPool">
        ///     the byte array pool from which the <see cref="ByteBuffer"/> should be pooled from
        /// </param>
        /// <param name="initialCapacity">the initial buffer capacity</param>
        /// <param name="expandable">a value indicating whether the buffer should be expandable</param>
        /// <param name="writable">a value indicating whether the buffer should support writing</param>
        /// <param name="exposable">
        ///     a value indicating whether the specified <paramref name="buffer"/> should be exposable
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     thrown if the specified <paramref name="initialCapacity"/> is negative.
        /// </exception>
        public ByteBuffer(ArrayPool<byte> arrayPool, int initialCapacity = DefaultInitialCapacity,
            bool expandable = true, bool writable = true, bool exposable = true)
        {
            if (initialCapacity < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(initialCapacity), initialCapacity,
                    "The specified initial buffer capacity can not be null.");
            }

            _pool = arrayPool;
            _buffer = arrayPool.Rent(initialCapacity);
            _capacity = _buffer.Length;

            IsExpandable = expandable;
            IsReadOnly = !writable;
            IsExposable = exposable;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ByteBuffer"/> class with a pooled buffer backend.
        /// </summary>
        /// <param name="initialCapacity">the initial buffer capacity</param>
        /// <param name="expandable">a value indicating whether the buffer should be expandable</param>
        /// <param name="writable">a value indicating whether the buffer should support writing</param>
        /// <param name="exposable">
        ///     a value indicating whether the specified <paramref name="buffer"/> should be exposable
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     thrown if the specified <paramref name="initialCapacity"/> is negative.
        /// </exception>
        public ByteBuffer(int initialCapacity = DefaultInitialCapacity,
            bool expandable = true, bool writable = true, bool exposable = true)
            : this(DefaultBufferPool, initialCapacity, expandable, writable, exposable)
        {
        }

        /// <summary>
        ///     Gets the default array pool for pooled <see cref="ByteBuffer"/> instances.
        /// </summary>
        public static ArrayPool<byte> DefaultBufferPool => ArrayPool<byte>.Shared;

        /// <summary>
        ///     Gets or sets the number of allocated internal buffer bytes.
        /// </summary>
        public int Capacity
        {
            get => _capacity;

            set
            {
                // check if the capacity has not changed
                if (_capacity == value)
                {
                    // the capacity did remain the same
                    return;
                }

                // ensure that the capacity is not negative
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), value,
                        "The specified capacity can not be negative.");
                }

                // ensure the buffer is expandable
                EnsureExpandable();

                // check if the buffer is shrinking
                if (value < _capacity)
                {
                    // clear shrink-ed bytes
                    Array.Clear(_buffer, value, _capacity);

                    // shrink length and position
                    _cursor = Math.Min(value, _cursor);
                    Length = value;
                }
                else
                {
                    // rent a new buffer with the capacity
                    var newBuffer = _pool.Rent(value);

                    // copy bytes
                    Buffer.BlockCopy(_buffer, srcOffset: 0, newBuffer, dstOffset: 0, count: _buffer.Length);

                    // return old buffer to array pool
                    _pool.Return(_buffer);

                    // set the new buffer
                    _buffer = newBuffer;
                }

                // set new capacity
                _capacity = value;
            }
        }

        /// <summary>
        ///     Gets the buffer endianness.
        /// </summary>
        public Endianness Endianness { get; } = Endianness.BigEndian;

        /// <summary>
        ///     Gets a value indicating whether the buffer is empty.
        /// </summary>
        public bool IsEmpty => Length == 0;

        /// <summary>
        ///     Gets a value indicating whether the buffer is expandable.
        /// </summary>
        public bool IsExpandable { get; }

        /// <summary>
        ///     Gets a value indicating whether the internal buffer is exposable.
        /// </summary>
        public bool IsExposable { get; }

        /// <summary>
        ///     Gets a value indicating whether the buffer was pooled from an array pool.
        /// </summary>
        public bool IsPooled => _pool != null;

        /// <summary>
        ///     Gets a value indicating whether the buffer is read-only.
        /// </summary>
        public bool IsReadOnly { get; }

        /// <summary>
        ///     Gets the current length of the buffer.
        /// </summary>
        public int Length
        {
            get => _length;

            set
            {
                // ensure length is not negative.
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), value,
                        "The specified length can not be negative.");
                }

                // check if the length is beyond the buffer capacity, then increase buffer size
                if (value > Capacity)
                {
                    // increase capacity
                    Capacity = value;
                }

                _length = value;
            }
        }

        /// <summary>
        ///     Gets or sets the current buffer cursor position.
        /// </summary>
        public int Position
        {
            get => _cursor - _origin;

            set
            {
                // ensure position is not negative.
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), value,
                        "The specified position can not be negative");
                }

                // ensure position is less than capacity
                if (value >= Capacity)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), value,
                        "The specified position can not be beyond the buffer capacity.");
                }

                _cursor = value + _origin;
            }
        }

        /// <summary>
        ///     Gets the amount of bytes remaining until the buffer is full.
        /// </summary>
        public int Remaining => Length - Position;

        /// <summary>
        ///     Gets or sets the <see cref="byte"/> at the specified zero-based <paramref name="index"/>.
        /// </summary>
        /// <param name="index">the zero-based absolute <see cref="byte"/> index</param>
        /// <returns>the <see cref="byte"/> at the specified zero-based <paramref name="index"/></returns>
        public byte this[int index]
        {
            get
            {
                // ensure the index is in range
                if ((uint)index >= (uint)_buffer.Length)
                {
                    // The index was out of range
                    throw new IndexOutOfRangeException();
                }

                // return byte at index
                return _buffer[index];
            }

            set
            {
                // ensure the index is in range
                if ((uint)index >= (uint)Length)
                {
                    // The index was out of range
                    throw new IndexOutOfRangeException();
                }

                // set byte at index
                _buffer[index] = value;
            }
        }

        /// <summary>
        ///     Validates the specified buffer specification ( <paramref name="offset"/> and
        ///     <paramref name="count"/> in relation with the specified <paramref name="buffer"/>).
        /// </summary>
        /// <param name="buffer">the buffer</param>
        /// <param name="offset">the byte offset</param>
        /// <param name="count">the number of bytes (relative to <paramref name="offset"/>)</param>
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
        public static void ValidateBuffer(byte[] buffer, int offset, int count)
        {
            if (buffer is null)
            {
                throw new ArgumentNullException(nameof(buffer),
                    "The specified buffer can not be null.");
            }

            if (offset < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(offset), offset,
                    "The specified offset can not be negative.");
            }

            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count), count,
                    "The specified count can not be negative.");
            }

            if (buffer.Length - offset < count)
            {
                throw new ArgumentOutOfRangeException(nameof(count), count,
                    "The specified buffer is too small for the specified count.");
            }
        }

        /// <summary>
        ///     Creates a memory stream from the buffer.
        /// </summary>
        /// <returns>the memory stream</returns>
        public MemoryStream AsMemoryStream()
            => new MemoryStream(_buffer, _origin, Length, !IsReadOnly);

        /// <summary>
        ///     Clears the buffer.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        ///     thrown if the buffer is read-only ( <see cref="IsReadOnly"/>)
        /// </exception>
        public void Clear()
        {
            EnsureWritable();
            Capacity = 0;
        }

        /// <summary>
        ///     Copies the internal buffer to the specified <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">the stream to copy the buffer to</param>
        /// <param name="full">
        ///     a value indicating whether the full buffer should be copied (
        ///     <see langword="false"/>, from start to end); or the buffer from the current cursor to
        ///     the end ( <see langword="true"/>).
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     thrown if the specified <paramref name="stream"/> is <see langword="null"/>.
        /// </exception>
        public void CopyTo(Stream stream, bool full = true)
        {
            if (stream is null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            stream.Write(_buffer, full ? _origin : _cursor, _length);
        }

        /// <summary>
        ///     Copies the internal buffer to the specified <paramref name="stream"/> asynchronously.
        /// </summary>
        /// <param name="stream">the stream to copy the buffer to</param>
        /// <param name="full">
        ///     a value indicating whether the full buffer should be copied (
        ///     <see langword="false"/>, from start to end); or the buffer from the current cursor to
        ///     the end ( <see langword="true"/>).
        /// </param>
        /// <param name="cancellationToken">
        ///     a cancellation token used to propagate notification that the asynchronous operation
        ///     should be canceled.
        /// </param>
        /// <returns>a task that represents the asynchronous operation</returns>
        /// <exception cref="ArgumentNullException">
        ///     thrown if the specified <paramref name="stream"/> is <see langword="null"/>.
        /// </exception>
        public Task CopyToAsync(Stream stream, bool full = true, CancellationToken cancellationToken = default)
        {
            if (stream is null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            return stream.WriteAsync(_buffer, full ? _origin : _cursor, _length, cancellationToken);
        }

        /// <summary>
        ///     Disposes the <see cref="ByteBuffer"/> instance and releases the buffer if it was rent
        ///     from an array pool ( <see cref="IsPooled"/>).
        /// </summary>
        public virtual void Dispose()
        {
            if (_disposed)
            {
                // instance already disposed
                return;
            }

            // set disposed flag
            _disposed = true;

            // check if the buffer was rent from the array pool
            if (IsPooled)
            {
                // return the buffer to the array pool
                _pool.Return(_buffer);
            }
        }

        /// <summary>
        ///     Gets the internal buffer.
        /// </summary>
        /// <returns>the internal buffer</returns>
        /// <exception cref="UnauthorizedAccessException">
        ///     thrown if the buffer is not exposable ( <see cref="IsExposable"/>).
        /// </exception>
        public ArraySegment<byte> GetBuffer()
        {
            // ensure buffer is exposable
            if (!IsExposable)
            {
                throw new UnauthorizedAccessException("The internal buffer is not exposable.");
            }

            // create array segment
            return new ArraySegment<byte>(_buffer, _origin, count: Remaining);
        }

        /// <summary>
        ///     Reads a <see cref="byte"/> sequence.
        /// </summary>
        /// <param name="count">the number of bytes to read</param>
        /// <returns>the <see cref="byte"/> sequence</returns>
        /// <exception cref="InvalidOperationException">thrown if the buffer is too small.</exception>
        public byte[] Read(int count)
        {
            var buffer = new byte[count];
            ReadBytes(buffer, count);
            return buffer;
        }

        /// <summary>
        ///     Reads a <see cref="bool"/> value from the buffer.
        /// </summary>
        /// <returns>the value read</returns>
        /// <exception cref="InvalidOperationException">thrown if the buffer is too small.</exception>
        public bool ReadBoolean() => ReadByte() != 0;

        /// <summary>
        ///     Reads a <see cref="byte"/> value from the buffer.
        /// </summary>
        /// <returns>the value read</returns>
        /// <exception cref="InvalidOperationException">thrown if the buffer is too small.</exception>
        public byte ReadByte()
        {
            EnsureRemaining(1);
            IncreasePosition(1);

            return _buffer[_cursor - 1];
        }

        /// <summary>
        ///     Reads a <see cref="byte"/> sequence and writes it to the specified <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">the buffer</param>
        /// <param name="count">the number of bytes to read</param>
        /// <exception cref="InvalidOperationException">thrown if the buffer is too small.</exception>
        public void ReadBytes(byte[] buffer, int count)
            => ReadBytes(buffer, offset: 0, count);

        /// <summary>
        ///     Reads a <see cref="byte"/> sequence and writes it to the specified <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">the buffer</param>
        /// <exception cref="InvalidOperationException">thrown if the buffer is too small.</exception>
        public void ReadBytes(byte[] buffer)
            => ReadBytes(buffer, offset: 0, buffer.Length);

        /// <summary>
        ///     Reads a <see cref="byte"/> sequence and writes it to the specified <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">the buffer</param>
        /// <exception cref="InvalidOperationException">thrown if the buffer is too small.</exception>
        public void ReadBytes(ArraySegment<byte> buffer)
            => ReadBytes(buffer.Array, buffer.Offset, buffer.Count);

        /// <summary>
        ///     Reads a <see cref="byte"/> sequence and writes it to the specified <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">the buffer</param>
        /// <param name="offset">the buffer write offset</param>
        /// <param name="count">the number of bytes to read</param>
        /// <exception cref="InvalidOperationException">thrown if the buffer is too small.</exception>
        public void ReadBytes(byte[] buffer, int offset, int count)
        {
            EnsureRemaining(count);
            Buffer.BlockCopy(_buffer, _cursor, buffer, offset, count);
            IncreasePosition(count);
        }

        /// <summary>
        ///     Reads a <see cref="double"/> value from the buffer.
        /// </summary>
        /// <returns>the value read</returns>
        /// <exception cref="InvalidOperationException">thrown if the buffer is too small.</exception>
        public double ReadDouble()
        {
            var buffer = Read(sizeof(double));

            // check if the buffer must be swapped to match endianness
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(buffer);
            }

            return BitConverter.ToDouble(buffer, startIndex: 0);
        }

        /// <summary>
        ///     Reads a <see cref="float"/> value from the buffer.
        /// </summary>
        /// <returns>the value read</returns>
        /// <exception cref="InvalidOperationException">thrown if the buffer is too small.</exception>
        public float ReadFloat()
        {
            var buffer = Read(sizeof(float));

            // check if the buffer must be swapped to match endianness
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(buffer);
            }

            return BitConverter.ToSingle(buffer, startIndex: 0);
        }

        /// <summary>
        ///     Reads a <see cref="Guid"/> value from the buffer.
        /// </summary>
        /// <returns>the value read</returns>
        /// <exception cref="InvalidOperationException">thrown if the buffer is too small.</exception>
        public Guid ReadGuid() => new Guid(Read(16));

        /// <summary>
        ///     Reads an <see cref="int"/> value from the buffer.
        /// </summary>
        /// <returns>the value read</returns>
        /// <exception cref="InvalidOperationException">thrown if the buffer is too small.</exception>
        public int ReadInt()
        {
            // ensure enough bytes are remaining to read the value
            EnsureRemaining(sizeof(int));

            // fix the buffer in the memory
            fixed (byte* ptr = &_buffer[_cursor])
            {
                // decode value from buffer
                return BigEndian.ToInt32(ptr);
            }
        }

        /// <summary>
        ///     Reads a <see cref="long"/> value from the buffer.
        /// </summary>
        /// <returns>the value read</returns>
        /// <exception cref="InvalidOperationException">thrown if the buffer is too small.</exception>
        public long ReadLong()
        {
            // ensure enough bytes are remaining to read the value
            EnsureRemaining(sizeof(long));

            // fix the buffer in the memory
            fixed (byte* ptr = &_buffer[_cursor])
            {
                // decode value from buffer
                return BigEndian.ToInt64(ptr);
            }
        }

        /// <summary>
        ///     Reads a <see cref="sbyte"/> value from the buffer.
        /// </summary>
        /// <returns>the value read</returns>
        /// <exception cref="InvalidOperationException">thrown if the buffer is too small.</exception>
        public sbyte ReadSByte() => (sbyte)ReadByte();

        /// <summary>
        ///     Reads a <see cref="short"/> value from the buffer.
        /// </summary>
        /// <returns>the value read</returns>
        /// <exception cref="InvalidOperationException">thrown if the buffer is too small.</exception>
        public short ReadShort()
        {
            // ensure enough bytes are remaining to read the value
            EnsureRemaining(sizeof(short));

            // fix the buffer in the memory
            fixed (byte* ptr = &_buffer[_cursor])
            {
                // decode value from buffer
                return BigEndian.ToInt16(ptr);
            }
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
        ///     Reads an <see cref="uint"/> value from the buffer.
        /// </summary>
        /// <returns>the value read</returns>
        /// <exception cref="InvalidOperationException">thrown if the buffer is too small.</exception>
        public uint ReadUInt()
        {
            // ensure enough bytes are remaining to read the value
            EnsureRemaining(sizeof(uint));

            // fix the buffer in the memory
            fixed (byte* ptr = &_buffer[_cursor])
            {
                // decode value from buffer
                return BigEndian.ToUInt32(ptr);
            }
        }

        /// <summary>
        ///     Reads an <see cref="ulong"/> value from the buffer.
        /// </summary>
        /// <returns>the value read</returns>
        /// <exception cref="InvalidOperationException">thrown if the buffer is too small.</exception>
        public ulong ReadULong()
        {
            // ensure enough bytes are remaining to read the value
            EnsureRemaining(sizeof(ulong));

            // fix the buffer in the memory
            fixed (byte* ptr = &_buffer[_cursor])
            {
                // decode value from buffer
                return BigEndian.ToUInt64(ptr);
            }
        }

        /// <summary>
        ///     Reads an <see cref="ushort"/> value from the buffer.
        /// </summary>
        /// <returns>the value read</returns>
        /// <exception cref="InvalidOperationException">thrown if the buffer is too small.</exception>
        public ushort ReadUShort()
        {
            // ensure enough bytes are remaining to read the value
            EnsureRemaining(sizeof(ushort));

            // fix the buffer in the memory
            fixed (byte* ptr = &_buffer[_cursor])
            {
                // decode value from buffer
                return BigEndian.ToUInt16(ptr);
            }
        }

        /// <summary>
        ///     Resets the cursor position.
        /// </summary>
        public void Reset() => _cursor = _origin;

        /// <summary>
        ///     Creates an array of the buffer data.
        /// </summary>
        /// <returns>an array of the buffer data</returns>
        public byte[] ToArray()
        {
            if (IsEmpty)
            {
                // return empty buffer
                return _empty;
            }

            // copy data
            var buffer = new byte[Length];
            Buffer.BlockCopy(_buffer, _origin, buffer, 0, Length);
            return buffer;
        }

        /// <summary>
        ///     Trims the internal buffer to the number of bytes used.
        /// </summary>
        public void Trim() => Capacity = Length;

        /// <summary>
        ///     Tries to get the internal buffer.
        /// </summary>
        /// <param name="buffer">the internal buffer</param>
        /// <returns>a value indicating whether the buffer could be get</returns>
        public bool TryGetBuffer(out ArraySegment<byte> buffer)
        {
            // ensure the buffer is exposable
            if (!IsExposable)
            {
                // buffer is not exposable
                buffer = default;
                return false;
            }

            // create array segment
            buffer = new ArraySegment<byte>(_buffer, _origin, count: Remaining);
            return true;
        }

        /// <summary>
        ///     Writes the specified <paramref name="value"/> to the buffer.
        /// </summary>
        /// <param name="value">the value</param>
        /// <exception cref="InvalidOperationException">
        ///     thrown if the buffer is read-only ( <see cref="IsReadOnly"/>)
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     thrown if the buffer is not expandable ( <see cref="IsExpandable"/>)
        /// </exception>
        public void Write(byte value) => Write(new[] { value }, 0, 1);

        /// <summary>
        ///     Writes the specified <paramref name="buffer"/> to the internal buffer.
        /// </summary>
        /// <param name="buffer">the buffer</param>
        /// <param name="count">the number of bytes to write</param>
        /// <exception cref="ArgumentNullException">
        ///     thrown if the specified <paramref name="buffer"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     thrown if the specified <paramref name="count"/> is negative.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     thrown if the buffer is read-only ( <see cref="IsReadOnly"/>)
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     thrown if the buffer is not expandable ( <see cref="IsExpandable"/>)
        /// </exception>
        public void Write(byte[] buffer, int count) => Write(buffer, offset: 0, count);

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
        public void Write(byte[] buffer) => Write(buffer, offset: 0, buffer.Length);

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
        public void Write(ArraySegment<byte> buffer) => Write(buffer.Array, buffer.Offset, buffer.Count);

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
        public void Write(byte[] buffer, int offset, int count)
        {
            ValidateBuffer(buffer, offset, count);

            // ensure enough capacity is remaining
            EnsureCapacity(Position + count);

            // copy data
            Buffer.BlockCopy(buffer, offset, _buffer, _cursor, count);
            IncreasePosition(count);
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
        public void Write(int value)
        {
            var position = EmulateWrite(sizeof(int));

            // fix buffer in memory
            fixed (byte* ptr = _buffer)
            {
                // encode bytes
                BigEndian.GetBytes(&ptr[position], value);
            }
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
            var position = EmulateWrite(sizeof(uint));

            // fix buffer in memory
            fixed (byte* ptr = _buffer)
            {
                // encode bytes
                BigEndian.GetBytes(&ptr[position], value);
            }
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
            var position = EmulateWrite(sizeof(ushort));

            // fix buffer in memory
            fixed (byte* ptr = _buffer)
            {
                // encode bytes
                BigEndian.GetBytes(&ptr[position], value);
            }
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
            var position = EmulateWrite(sizeof(float));

            // fix buffer in memory
            fixed (byte* ptr = &_buffer[position])
            {
                // encode bytes
                BigEndian.GetBytes(ptr, value);
            }
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
            var position = EmulateWrite(sizeof(double));

            // fix buffer in memory
            fixed (byte* ptr = &_buffer[position])
            {
                // encode bytes
                BigEndian.GetBytes(ptr, value);
            }
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
        public void Write(bool value)
        {
            var position = EmulateWrite(sizeof(bool));

            // fix buffer in memory
            fixed (byte* ptr = &_buffer[position])
            {
                // encode bytes
                BigEndian.GetBytes(ptr, value);
            }
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
            var position = EmulateWrite(sizeof(short));

            // fix buffer in memory
            fixed (byte* ptr = _buffer)
            {
                // encode bytes
                BigEndian.GetBytes(&ptr[position], value);
            }
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
        public void Write(sbyte value)
        {
            var position = EmulateWrite(sizeof(sbyte));

            // fix buffer in memory
            fixed (byte* ptr = &_buffer[position])
            {
                // encode bytes
                BigEndian.GetBytes(ptr, value);
            }
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
        public void Write(long value)
        {
            var position = EmulateWrite(sizeof(long));

            // fix buffer in memory
            fixed (byte* ptr = _buffer)
            {
                // encode bytes
                BigEndian.GetBytes(&ptr[position], value);
            }
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
            var position = EmulateWrite(sizeof(ulong));

            // fix buffer in memory
            fixed (byte* ptr = &_buffer[position])
            {
                // encode bytes
                BigEndian.GetBytes(ptr, value);
            }
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
        public int Write(string value, int charIndex, int charCount)
            => Write(value, charIndex, charCount, Encoding.UTF8);

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
        public int Write(string value, int charCount, Encoding encoding)
            => Write(value, charIndex: 0, charCount, encoding);

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
        public int Write(string value, Encoding encoding)
            => Write(value, charIndex: 0, value.Length, encoding);

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
            // rent a buffer that can hold the string and the 2-byte ushort length prefix
            var pooledBuffer = ArrayPool<byte>.Shared.Rent(encoding.GetMaxByteCount(charCount) + 2);

            // ensure the pooled buffer is returned to the pool even if an exception is thrown
            try
            {
                // encode the string starting at buffer offset 2
                var length = encoding.GetBytes(value, charIndex, charCount, pooledBuffer, byteIndex: 2);

                // ensure the length does not overflow
                if (length > 0xFFFF)
                {
                    throw new ArgumentException("The specified string overflows the maximum encoded byte length (0xFFFF).", nameof(value));
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

        protected void EnsureCapacity(int count)
        {
            EnsureWritable();

            // check if the required byte count is already satisfied
            if (Capacity >= count)
            {
                // there are enough bytes remaining
                return;
            }

            // we have to increase the capacity (in 256 byte chunks)
            Capacity += (((_capacity + count) >> 8) + 1) << 8;
        }

        /// <summary>
        ///     Ensures that the buffer is expandable.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        ///     thrown if the buffer is not expandable ( <see cref="IsExpandable"/>)
        /// </exception>
        protected void EnsureExpandable()
        {
            if (!IsExpandable || !IsPooled)
            {
                throw new InvalidOperationException("The buffer is not expandable.");
            }
        }

        /// <summary>
        ///     Ensures that the instance was not disposed.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        ///     thrown if the <see cref="ByteBuffer"/> instance is disposed.
        /// </exception>
        protected void EnsureNotDisposed()
        {
            // check if the instance was already disposed
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(ByteBuffer));
            }
        }

        protected void EnsureRemaining(int count)
        {
            // TODO
        }

        /// <summary>
        ///     Ensures that the buffer is writable.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        ///     thrown if the buffer is read-only ( <see cref="IsReadOnly"/>)
        /// </exception>
        protected void EnsureWritable()
        {
            if (IsReadOnly)
            {
                throw new InvalidOperationException("The buffer is read-only.");
            }
        }

        private int EmulateWrite(int count)
        {
            // store current cursor position
            var cursorPosition = _cursor;

            // ensure enough capacity is remaining
            EnsureCapacity(Position + count);

            // increase position
            IncreasePosition(count);

            return cursorPosition;
        }

        private void IncreasePosition(int count)
        {
            _cursor += count;
            _length = Math.Max(_cursor - _origin, _length);
        }
    }
}
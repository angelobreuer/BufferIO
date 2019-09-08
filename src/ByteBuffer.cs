namespace ByteBuffer
{
    using System;
    using System.IO;

    public class ByteBuffer
    {
        /// <summary>
        ///     Gets the default initial capacity.
        /// </summary>
        public const int DefaultInitialCapacity = 4;

        /// <summary>
        ///     An empty byte buffer.
        /// </summary>
        private static readonly byte[] _empty = new byte[0];

        /// <summary>
        ///     The general buffer write offset (not the writing position).
        /// </summary>
        private readonly int _origin;

        /// <summary>
        ///     The local buffer storing the data.
        /// </summary>
        private byte[] _buffer;

        /// <summary>
        ///     The number of bytes allocated for the buffer.
        /// </summary>
        private int _capacity;

        /// <summary>
        ///     The current cursor position.
        /// </summary>
        private int _position;

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

            Length = count;
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
        ///     Initializes a new instance of the <see cref="ByteBuffer"/> class.
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
        {
            if (initialCapacity < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(initialCapacity), initialCapacity,
                    "The specified initial buffer capacity can not be null.");
            }

            _buffer = new byte[initialCapacity];
            _capacity = initialCapacity;

            IsExpandable = expandable;
            IsReadOnly = !writable;
            IsExposable = exposable;
        }

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
                    _position = Math.Min(value, _position);
                    Length = value;
                }
                else
                {
                    // the buffer is expanding
                    var newBuffer = new byte[value];

                    // copy bytes
                    Buffer.BlockCopy(_buffer, srcOffset: 0, newBuffer, dstOffset: 0, count: _buffer.Length);

                    // set the new buffer
                    _buffer = newBuffer;
                }

                // set new capacity
                _capacity = value;
            }
        }

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
        ///     Gets a value indicating whether the buffer is read-only.
        /// </summary>
        public bool IsReadOnly { get; }

        /// <summary>
        ///     Gets the current length of the buffer.
        /// </summary>
        public int Length { get; private set; }

        /// <summary>
        ///     Gets or sets the current buffer cursor position.
        /// </summary>
        public int Position
        {
            get => _position;

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

                _position = value;
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
        public void Write(byte value) => Write(new[] { value }, 0, 1);

        /// <summary>
        ///     Writes the specified <paramref name="buffer"/> to the internal buffer.
        /// </summary>
        /// <param name="buffer">the buffer</param>
        /// <param name="count">the number of bytes to write</param>
        public void Write(byte[] buffer, int count) => Write(buffer, offset: 0, count);

        /// <summary>
        ///     Writes the specified <paramref name="buffer"/> to the internal buffer.
        /// </summary>
        /// <param name="buffer">the buffer</param>
        public void Write(byte[] buffer) => Write(buffer, offset: 0, buffer.Length);

        /// <summary>
        ///     Writes the specified <paramref name="buffer"/> to the internal buffer.
        /// </summary>
        /// <param name="buffer">the buffer</param>
        /// <param name="offset">the buffer read offset</param>
        /// <param name="count">the number of bytes to write</param>
        public void Write(byte[] buffer, int offset, int count)
        {
            // null-check buffer
            if (buffer is null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            // emulate the write
            var writeOrigin = EmulateWrite(count);

            // copy data
            Buffer.BlockCopy(buffer, offset, _buffer, writeOrigin, count);
        }

        /// <summary>
        ///     Ensures that the buffer is expandable.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        ///     thrown if the buffer is not expandable ( <see cref="IsExpandable"/>)
        /// </exception>
        protected void EnsureExpandable()
        {
            if (!IsExpandable)
            {
                throw new InvalidOperationException("The buffer is not expandable.");
            }
        }

        protected void EnsureRemaining(int count)
        {
            EnsureWritable();

            // check if the required byte count is already satisfied
            if (Remaining >= count)
            {
                // there are enough bytes remaining
                return;
            }

            // we have to increase the capacity
            Capacity += Math.Max(256, count);
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
            // ensure enough capacity is remaining
            EnsureRemaining(count);

            var origin = _position;

            // copy data
            Length += count;
            _position += count;

            return origin;
        }
    }
}
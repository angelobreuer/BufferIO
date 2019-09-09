namespace ByteBuffer.Util
{
    using System;
    using System.Runtime.CompilerServices;

    /// <summary>
    ///     An utility class used for the conversion between byte buffers and value types.
    /// </summary>
    public static unsafe class BigEndian
    {
        /// <summary>
        ///     Encodes the specified <paramref name="value"/> to a <see cref="byte"/> array.
        /// </summary>
        /// <param name="value">the value to encode</param>
        /// <returns>the <see cref="byte"/> array</returns>
        public static byte[] GetBytes(int value)
        {
            // create buffer that can hold the value
            var buffer = new byte[sizeof(int)];

            // fix buffer in memory
            fixed (byte* ptr = buffer)
            {
                // encode value
                GetBytes(ptr, value);
            }

            return buffer;
        }

        /// <summary>
        ///     Encodes the specified <paramref name="value"/> to a <see cref="byte"/> array.
        /// </summary>
        /// <param name="value">the value to encode</param>
        /// <returns>the <see cref="byte"/> array</returns>
        public static byte[] GetBytes(uint value)
        {
            // create buffer that can hold the value
            var buffer = new byte[sizeof(uint)];

            // fix buffer in memory
            fixed (byte* ptr = buffer)
            {
                // encode value
                GetBytes(ptr, value);
            }

            return buffer;
        }

        /// <summary>
        ///     Encodes the specified <paramref name="value"/> to a <see cref="byte"/> array.
        /// </summary>
        /// <param name="value">the value to encode</param>
        /// <returns>the <see cref="byte"/> array</returns>
        public static byte[] GetBytes(ushort value)
        {
            // create buffer that can hold the value
            var buffer = new byte[sizeof(ushort)];

            // fix buffer in memory
            fixed (byte* ptr = buffer)
            {
                // encode value
                GetBytes(ptr, value);
            }

            return buffer;
        }

        /// <summary>
        ///     Encodes the specified <paramref name="value"/> to a <see cref="byte"/> array.
        /// </summary>
        /// <param name="value">the value to encode</param>
        /// <returns>the <see cref="byte"/> array</returns>
        public static byte[] GetBytes(short value)
        {
            // create buffer that can hold the value
            var buffer = new byte[sizeof(short)];

            // fix buffer in memory
            fixed (byte* ptr = buffer)
            {
                // encode value
                GetBytes(ptr, value);
            }

            return buffer;
        }

        /// <summary>
        ///     Encodes the specified <paramref name="value"/> to a <see cref="byte"/> array.
        /// </summary>
        /// <param name="value">the value to encode</param>
        /// <returns>the <see cref="byte"/> array</returns>
        public static byte[] GetBytes(long value)
        {
            // create buffer that can hold the value
            var buffer = new byte[sizeof(long)];

            // fix buffer in memory
            fixed (byte* ptr = buffer)
            {
                // encode value
                GetBytes(ptr, value);
            }

            return buffer;
        }

        /// <summary>
        ///     Encodes the specified <paramref name="value"/> to a <see cref="byte"/> array.
        /// </summary>
        /// <param name="value">the value to encode</param>
        /// <returns>the <see cref="byte"/> array</returns>
        public static byte[] GetBytes(ulong value)
        {
            // create buffer that can hold the value
            var buffer = new byte[sizeof(ulong)];

            // fix buffer in memory
            fixed (byte* ptr = buffer)
            {
                // encode value
                GetBytes(ptr, value);
            }

            return buffer;
        }

        /// <summary>
        ///     Encodes the specified <paramref name="value"/> to a <see cref="byte"/> array.
        /// </summary>
        /// <param name="value">the value to encode</param>
        /// <returns>the <see cref="byte"/> array</returns>
        public static byte[] GetBytes(decimal value)
        {
            // create buffer that can hold the value
            var buffer = new byte[sizeof(decimal)];

            // fix buffer in memory
            fixed (byte* ptr = buffer)
            {
                // encode value
                GetBytes(ptr, value);
            }

            return buffer;
        }

        /// <summary>
        ///     Encodes the specified <paramref name="value"/> to a <see cref="byte"/> array.
        /// </summary>
        /// <param name="value">the value to encode</param>
        /// <returns>the <see cref="byte"/> array</returns>
        public static byte[] GetBytes(double value)
        {
            // create buffer that can hold the value
            var buffer = new byte[sizeof(double)];

            // fix buffer in memory
            fixed (byte* ptr = buffer)
            {
                // encode value
                GetBytes(ptr, value);
            }

            return buffer;
        }

        /// <summary>
        ///     Encodes the specified <paramref name="value"/> to a <see cref="byte"/> array.
        /// </summary>
        /// <param name="value">the value to encode</param>
        /// <returns>the <see cref="byte"/> array</returns>
        public static byte[] GetBytes(float value)
        {
            // create buffer that can hold the value
            var buffer = new byte[sizeof(float)];

            // fix buffer in memory
            fixed (byte* ptr = buffer)
            {
                // encode value
                GetBytes(ptr, value);
            }

            return buffer;
        }

        /// <summary>
        ///     Encodes the specified <paramref name="value"/> to the specified
        ///     <paramref name="buffer"/> at the specified zero-based <paramref name="byteOffset"/>.
        /// </summary>
        /// <param name="buffer">the buffer to encode the value to</param>
        /// <param name="value">the value</param>
        /// <param name="byteOffset">the byte offset</param>
        /// <returns>the buffer position (for the next value)</returns>
        /// <exception cref="ArgumentNullException">
        ///     thrown if the specified <paramref name="buffer"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     thrown if the specified <paramref name="byteOffset"/> is negative.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     thrown if the specified <paramref name="buffer"/> is too small for the value.
        /// </exception>
        public static int GetBytes(byte[] buffer, int value, int byteOffset = 0)
        {
            ByteBuffer.ValidateBuffer(buffer, byteOffset, sizeof(int));

            fixed (byte* ptr = buffer)
            {
                GetBytes(&ptr[byteOffset], value);
            }

            return byteOffset + sizeof(int);
        }

        /// <summary>
        ///     Encodes the specified <paramref name="value"/> to the specified
        ///     <paramref name="buffer"/> at the specified zero-based <paramref name="byteOffset"/>.
        /// </summary>
        /// <param name="buffer">the buffer to encode the value to</param>
        /// <param name="value">the value</param>
        /// <param name="byteOffset">the byte offset</param>
        /// <returns>the buffer position (for the next value)</returns>
        /// <exception cref="ArgumentNullException">
        ///     thrown if the specified <paramref name="buffer"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     thrown if the specified <paramref name="byteOffset"/> is negative.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     thrown if the specified <paramref name="buffer"/> is too small for the value.
        /// </exception>
        public static int GetBytes(byte[] buffer, uint value, int byteOffset = 0)
        {
            ByteBuffer.ValidateBuffer(buffer, byteOffset, sizeof(uint));

            fixed (byte* ptr = buffer)
            {
                GetBytes(&ptr[byteOffset], value);
            }

            return byteOffset + sizeof(uint);
        }

        /// <summary>
        ///     Encodes the specified <paramref name="value"/> to the specified
        ///     <paramref name="buffer"/> at the specified zero-based <paramref name="byteOffset"/>.
        /// </summary>
        /// <param name="buffer">the buffer to encode the value to</param>
        /// <param name="value">the value</param>
        /// <param name="byteOffset">the byte offset</param>
        /// <returns>the buffer position (for the next value)</returns>
        /// <exception cref="ArgumentNullException">
        ///     thrown if the specified <paramref name="buffer"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     thrown if the specified <paramref name="byteOffset"/> is negative.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     thrown if the specified <paramref name="buffer"/> is too small for the value.
        /// </exception>
        public static int GetBytes(byte[] buffer, short value, int byteOffset = 0)
        {
            ByteBuffer.ValidateBuffer(buffer, byteOffset, sizeof(short));

            fixed (byte* ptr = buffer)
            {
                GetBytes(&ptr[byteOffset], value);
            }

            return byteOffset + sizeof(short);
        }

        /// <summary>
        ///     Encodes the specified <paramref name="value"/> to the specified
        ///     <paramref name="buffer"/> at the specified zero-based <paramref name="byteOffset"/>.
        /// </summary>
        /// <param name="buffer">the buffer to encode the value to</param>
        /// <param name="value">the value</param>
        /// <param name="byteOffset">the byte offset</param>
        /// <returns>the buffer position (for the next value)</returns>
        /// <exception cref="ArgumentNullException">
        ///     thrown if the specified <paramref name="buffer"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     thrown if the specified <paramref name="byteOffset"/> is negative.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     thrown if the specified <paramref name="buffer"/> is too small for the value.
        /// </exception>
        public static int GetBytes(byte[] buffer, ushort value, int byteOffset = 0)
        {
            ByteBuffer.ValidateBuffer(buffer, byteOffset, sizeof(ushort));

            fixed (byte* ptr = buffer)
            {
                GetBytes(&ptr[byteOffset], value);
            }

            return byteOffset + sizeof(ushort);
        }

        /// <summary>
        ///     Encodes the specified <paramref name="value"/> to the specified
        ///     <paramref name="buffer"/> at the specified zero-based <paramref name="byteOffset"/>.
        /// </summary>
        /// <param name="buffer">the buffer to encode the value to</param>
        /// <param name="value">the value</param>
        /// <param name="byteOffset">the byte offset</param>
        /// <returns>the buffer position (for the next value)</returns>
        /// <exception cref="ArgumentNullException">
        ///     thrown if the specified <paramref name="buffer"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     thrown if the specified <paramref name="byteOffset"/> is negative.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     thrown if the specified <paramref name="buffer"/> is too small for the value.
        /// </exception>
        public static int GetBytes(byte[] buffer, long value, int byteOffset = 0)
        {
            ByteBuffer.ValidateBuffer(buffer, byteOffset, sizeof(long));

            fixed (byte* ptr = buffer)
            {
                GetBytes(&ptr[byteOffset], value);
            }

            return byteOffset + sizeof(long);
        }

        /// <summary>
        ///     Encodes the specified <paramref name="value"/> to the specified
        ///     <paramref name="buffer"/> at the specified zero-based <paramref name="byteOffset"/>.
        /// </summary>
        /// <param name="buffer">the buffer to encode the value to</param>
        /// <param name="value">the value</param>
        /// <param name="byteOffset">the byte offset</param>
        /// <returns>the buffer position (for the next value)</returns>
        /// <exception cref="ArgumentNullException">
        ///     thrown if the specified <paramref name="buffer"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     thrown if the specified <paramref name="byteOffset"/> is negative.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     thrown if the specified <paramref name="buffer"/> is too small for the value.
        /// </exception>
        public static int GetBytes(byte[] buffer, ulong value, int byteOffset = 0)
        {
            ByteBuffer.ValidateBuffer(buffer, byteOffset, sizeof(ulong));

            fixed (byte* ptr = buffer)
            {
                GetBytes(&ptr[byteOffset], value);
            }

            return byteOffset + sizeof(ulong);
        }

        /// <summary>
        ///     Encodes the specified <paramref name="value"/> to the specified
        ///     <paramref name="buffer"/> at the specified zero-based <paramref name="byteOffset"/>.
        /// </summary>
        /// <param name="buffer">the buffer to encode the value to</param>
        /// <param name="value">the value</param>
        /// <param name="byteOffset">the byte offset</param>
        /// <returns>the buffer position (for the next value)</returns>
        /// <exception cref="ArgumentNullException">
        ///     thrown if the specified <paramref name="buffer"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     thrown if the specified <paramref name="byteOffset"/> is negative.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     thrown if the specified <paramref name="buffer"/> is too small for the value.
        /// </exception>
        public static int GetBytes(byte[] buffer, sbyte value, int byteOffset = 0)
        {
            ByteBuffer.ValidateBuffer(buffer, byteOffset, sizeof(sbyte));

            fixed (byte* ptr = buffer)
            {
                GetBytes(&ptr[byteOffset], value);
            }

            return byteOffset + sizeof(sbyte);
        }

        /// <summary>
        ///     Encodes the specified <paramref name="value"/> to the specified
        ///     <paramref name="buffer"/> at the specified zero-based <paramref name="byteOffset"/>.
        /// </summary>
        /// <param name="buffer">the buffer to encode the value to</param>
        /// <param name="value">the value</param>
        /// <param name="byteOffset">the byte offset</param>
        /// <returns>the buffer position (for the next value)</returns>
        /// <exception cref="ArgumentNullException">
        ///     thrown if the specified <paramref name="buffer"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     thrown if the specified <paramref name="byteOffset"/> is negative.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     thrown if the specified <paramref name="buffer"/> is too small for the value.
        /// </exception>
        public static int GetBytes(byte[] buffer, byte value, int byteOffset = 0)
        {
            ByteBuffer.ValidateBuffer(buffer, byteOffset, sizeof(byte));

            fixed (byte* ptr = buffer)
            {
                GetBytes(&ptr[byteOffset], value);
            }

            return byteOffset + sizeof(byte);
        }

        /// <summary>
        ///     Encodes the specified <paramref name="value"/> to the specified
        ///     <paramref name="buffer"/> at the specified zero-based <paramref name="byteOffset"/>.
        /// </summary>
        /// <param name="buffer">the buffer to encode the value to</param>
        /// <param name="value">the value</param>
        /// <param name="byteOffset">the byte offset</param>
        /// <returns>the buffer position (for the next value)</returns>
        /// <exception cref="ArgumentNullException">
        ///     thrown if the specified <paramref name="buffer"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     thrown if the specified <paramref name="byteOffset"/> is negative.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     thrown if the specified <paramref name="buffer"/> is too small for the value.
        /// </exception>
        public static int GetBytes(byte[] buffer, bool value, int byteOffset = 0)
        {
            ByteBuffer.ValidateBuffer(buffer, byteOffset, sizeof(bool));

            fixed (byte* ptr = buffer)
            {
                GetBytes(&ptr[byteOffset], value);
            }

            return byteOffset + sizeof(bool);
        }

        /// <summary>
        ///     Encodes the specified <paramref name="value"/> to the specified
        ///     <paramref name="buffer"/> at the specified zero-based <paramref name="byteOffset"/>.
        /// </summary>
        /// <param name="buffer">the buffer to encode the value to</param>
        /// <param name="value">the value</param>
        /// <param name="byteOffset">the byte offset</param>
        /// <returns>the buffer position (for the next value)</returns>
        /// <exception cref="ArgumentNullException">
        ///     thrown if the specified <paramref name="buffer"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     thrown if the specified <paramref name="byteOffset"/> is negative.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     thrown if the specified <paramref name="buffer"/> is too small for the value.
        /// </exception>
        public static int GetBytes(byte[] buffer, float value, int byteOffset = 0)
        {
            ByteBuffer.ValidateBuffer(buffer, byteOffset, sizeof(float));

            fixed (byte* ptr = buffer)
            {
                GetBytes(&ptr[byteOffset], value);
            }

            return byteOffset + sizeof(float);
        }

        /// <summary>
        ///     Encodes the specified <paramref name="value"/> to the specified
        ///     <paramref name="buffer"/> at the specified zero-based <paramref name="byteOffset"/>.
        /// </summary>
        /// <param name="buffer">the buffer to encode the value to</param>
        /// <param name="value">the value</param>
        /// <param name="byteOffset">the byte offset</param>
        /// <returns>the buffer position (for the next value)</returns>
        /// <exception cref="ArgumentNullException">
        ///     thrown if the specified <paramref name="buffer"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     thrown if the specified <paramref name="byteOffset"/> is negative.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     thrown if the specified <paramref name="buffer"/> is too small for the value.
        /// </exception>
        public static int GetBytes(byte[] buffer, double value, int byteOffset = 0)
        {
            ByteBuffer.ValidateBuffer(buffer, byteOffset, sizeof(double));

            fixed (byte* ptr = buffer)
            {
                GetBytes(&ptr[byteOffset], value);
            }

            return byteOffset + sizeof(double);
        }

        /// <summary>
        ///     Encodes the specified <paramref name="value"/> to the specified
        ///     <paramref name="buffer"/> at the specified zero-based <paramref name="byteOffset"/>.
        /// </summary>
        /// <param name="buffer">the buffer to encode the value to</param>
        /// <param name="value">the value</param>
        /// <param name="byteOffset">the byte offset</param>
        /// <returns>the buffer position (for the next value)</returns>
        /// <exception cref="ArgumentNullException">
        ///     thrown if the specified <paramref name="buffer"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     thrown if the specified <paramref name="byteOffset"/> is negative.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     thrown if the specified <paramref name="buffer"/> is too small for the value.
        /// </exception>
        public static int GetBytes(byte[] buffer, decimal value, int byteOffset = 0)
        {
            ByteBuffer.ValidateBuffer(buffer, byteOffset, sizeof(decimal));

            fixed (byte* ptr = buffer)
            {
                GetBytes(&ptr[byteOffset], value);
            }

            return byteOffset + sizeof(decimal);
        }

        /// <summary>
        ///     Copies the specified <paramref name="value"/> to the specified <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">
        ///     a pointer pointing to the memory address of the first byte element of the buffer
        /// </param>
        /// <param name="value">the value to copy</param>
        /// <returns>
        ///     a pointer pointing to the memory address of the next free byte of the specified <paramref name="buffer"/>
        /// </returns>
        public static byte* GetBytes(byte* buffer, int value)
        {
            *buffer++ = (byte)(value >> 24);
            *buffer++ = (byte)(value >> 16);
            *buffer++ = (byte)(value >> 8);
            *buffer++ = (byte)value;

            return buffer;
        }

        /// <summary>
        ///     Copies the specified <paramref name="value"/> to the specified <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">
        ///     a pointer pointing to the memory address of the first byte element of the buffer
        /// </param>
        /// <param name="value">the value to copy</param>
        /// <returns>
        ///     a pointer pointing to the memory address of the next free byte of the specified <paramref name="buffer"/>
        /// </returns>
        public static byte* GetBytes(byte* buffer, uint value)
        {
            *buffer++ = (byte)(value >> 24);
            *buffer++ = (byte)(value >> 16);
            *buffer++ = (byte)(value >> 8);
            *buffer++ = (byte)value;

            return buffer;
        }

        /// <summary>
        ///     Copies the specified <paramref name="value"/> to the specified <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">
        ///     a pointer pointing to the memory address of the first byte element of the buffer
        /// </param>
        /// <param name="value">the value to copy</param>
        /// <returns>
        ///     a pointer pointing to the memory address of the next free byte of the specified <paramref name="buffer"/>
        /// </returns>
        public static byte* GetBytes(byte* buffer, bool value)
        {
            *buffer++ = (byte)(value ? 1 : 0);

            return buffer;
        }

        /// <summary>
        ///     Copies the specified <paramref name="value"/> to the specified <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">
        ///     a pointer pointing to the memory address of the first byte element of the buffer
        /// </param>
        /// <param name="value">the value to copy</param>
        /// <returns>
        ///     a pointer pointing to the memory address of the next free byte of the specified <paramref name="buffer"/>
        /// </returns>
        public static byte* GetBytes(byte* buffer, byte value)
        {
            *buffer++ = value;

            return buffer;
        }

        /// <summary>
        ///     Copies the specified <paramref name="value"/> to the specified <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">
        ///     a pointer pointing to the memory address of the first byte element of the buffer
        /// </param>
        /// <param name="value">the value to copy</param>
        /// <returns>
        ///     a pointer pointing to the memory address of the next free byte of the specified <paramref name="buffer"/>
        /// </returns>
        public static byte* GetBytes(byte* buffer, sbyte value)
        {
            *buffer++ = (byte)value;

            return buffer;
        }

        /// <summary>
        ///     Copies the specified <paramref name="value"/> to the specified <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">
        ///     a pointer pointing to the memory address of the first byte element of the buffer
        /// </param>
        /// <param name="value">the value to copy</param>
        /// <returns>
        ///     a pointer pointing to the memory address of the next free byte of the specified <paramref name="buffer"/>
        /// </returns>
        public static byte* GetBytes(byte* buffer, short value)
        {
            *buffer++ = (byte)(value >> 8);
            *buffer++ = (byte)value;

            return buffer;
        }

        /// <summary>
        ///     Copies the specified <paramref name="value"/> to the specified <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">
        ///     a pointer pointing to the memory address of the first byte element of the buffer
        /// </param>
        /// <param name="value">the value to copy</param>
        /// <returns>
        ///     a pointer pointing to the memory address of the next free byte of the specified <paramref name="buffer"/>
        /// </returns>
        public static byte* GetBytes(byte* buffer, ushort value)
        {
            *buffer++ = (byte)(value >> 8);
            *buffer++ = (byte)value;

            return buffer;
        }

        /// <summary>
        ///     Copies the specified <paramref name="value"/> to the specified <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">
        ///     a pointer pointing to the memory address of the first byte element of the buffer
        /// </param>
        /// <param name="value">the value to copy</param>
        /// <returns>
        ///     a pointer pointing to the memory address of the next free byte of the specified <paramref name="buffer"/>
        /// </returns>
        public static byte* GetBytes(byte* buffer, double value)
            => GetBytes(buffer, *(long*)&value);

        /// <summary>
        ///     Copies the specified <paramref name="value"/> to the specified <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">
        ///     a pointer pointing to the memory address of the first byte element of the buffer
        /// </param>
        /// <param name="value">the value to copy</param>
        /// <returns>
        ///     a pointer pointing to the memory address of the next free byte of the specified <paramref name="buffer"/>
        /// </returns>
        public static byte* GetBytes(byte* buffer, float value)
            => GetBytes(buffer, *(int*)&value);

        /// <summary>
        ///     Copies the specified <paramref name="value"/> to the specified <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">
        ///     a pointer pointing to the memory address of the first byte element of the buffer
        /// </param>
        /// <param name="value">the value to copy</param>
        /// <returns>
        ///     a pointer pointing to the memory address of the next free byte of the specified <paramref name="buffer"/>
        /// </returns>
        public static byte* GetBytes(byte* buffer, decimal value)
        {
            // the decimal parts (flags, hi, low, mid)
            var parts = decimal.GetBits(value);

            // encode decimal
            buffer = GetBytes(buffer, parts[0]);
            buffer = GetBytes(buffer, parts[1]);
            buffer = GetBytes(buffer, parts[2]);
            return GetBytes(buffer, parts[3]);
        }

        /// <summary>
        ///     Copies the specified <paramref name="value"/> to the specified <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">
        ///     a pointer pointing to the memory address of the first byte element of the buffer
        /// </param>
        /// <param name="value">the value to copy</param>
        /// <returns>
        ///     a pointer pointing to the memory address of the next free byte of the specified <paramref name="buffer"/>
        /// </returns>
        public static byte* GetBytes(byte* buffer, long value)
        {
            *buffer++ = (byte)(value >> 56);
            *buffer++ = (byte)(value >> 48);
            *buffer++ = (byte)(value >> 40);
            *buffer++ = (byte)(value >> 32);
            *buffer++ = (byte)(value >> 24);
            *buffer++ = (byte)(value >> 16);
            *buffer++ = (byte)(value >> 8);
            *buffer++ = (byte)value;

            return buffer;
        }

        /// <summary>
        ///     Copies the specified <paramref name="value"/> to the specified <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">
        ///     a pointer pointing to the memory address of the first byte element of the buffer
        /// </param>
        /// <param name="value">the value to copy</param>
        /// <returns>
        ///     a pointer pointing to the memory address of the next free byte of the specified <paramref name="buffer"/>
        /// </returns>
        public static byte* GetBytes(byte* buffer, ulong value)
        {
            *buffer++ = (byte)(value >> 56);
            *buffer++ = (byte)(value >> 48);
            *buffer++ = (byte)(value >> 40);
            *buffer++ = (byte)(value >> 32);
            *buffer++ = (byte)(value >> 24);
            *buffer++ = (byte)(value >> 16);
            *buffer++ = (byte)(value >> 8);
            *buffer++ = (byte)value;

            return buffer;
        }

        /// <summary>
        ///     Converts the <paramref name="buffer"/> to an <see cref="double"/> value.
        /// </summary>
        /// <param name="buffer">a pointer pointing to the memory address of the buffer</param>
        /// <param name="offset">the zero-based buffer offset</param>
        /// <returns>the <see cref="double"/> value</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double ToDouble(byte* buffer, int offset) => ToDouble(&buffer[offset]);

        /// <summary>
        ///     Converts the <paramref name="buffer"/> to an <see cref="double"/> value.
        /// </summary>
        /// <param name="buffer">the buffer</param>
        /// <param name="offset">the zero-based buffer offset</param>
        /// <returns>the <see cref="double"/> value</returns>
        /// <exception cref="InvalidOperationException">
        ///     thrown if the specified buffer is too small in relation with the specified byte <paramref name="offset"/>
        /// </exception>
        public static double ToDouble(byte[] buffer, int offset = 0)
        {
            // range check offset
            if (buffer.Length < offset + sizeof(double))
            {
                throw new InvalidOperationException("The specified buffer is too small.");
            }

            // fix buffer in memory
            fixed (byte* ptr = &buffer[offset])
            {
                // convert value
                return ToDouble(ptr);
            }
        }

        /// <summary>
        ///     Converts the specified <paramref name="buffer"/> to a <see cref="short"/> value.
        /// </summary>
        /// <param name="buffer">a pointer pointing to the memory address of the buffer</param>
        /// <returns>the <see cref="double"/> value</returns>
        public static double ToDouble(byte* buffer)
        {
            // read binary double representation (long)
            var value = ToInt64(buffer);

            // reinterpret value
            return *(double*)&value;
        }

        /// <summary>
        ///     Converts the <paramref name="buffer"/> to an <see cref="float"/> value.
        /// </summary>
        /// <param name="buffer">the buffer</param>
        /// <param name="offset">the zero-based buffer offset</param>
        /// <returns>the <see cref="float"/> value</returns>
        /// <exception cref="InvalidOperationException">
        ///     thrown if the specified buffer is too small in relation with the specified byte <paramref name="offset"/>
        /// </exception>
        public static float ToFloat(byte[] buffer, int offset = 0)
        {
            // range check offset
            if (buffer.Length < offset + sizeof(float))
            {
                throw new InvalidOperationException("The specified buffer is too small.");
            }

            // fix buffer in memory
            fixed (byte* ptr = &buffer[offset])
            {
                // convert value
                return ToFloat(ptr);
            }
        }

        /// <summary>
        ///     Converts the <paramref name="buffer"/> to an <see cref="float"/> value.
        /// </summary>
        /// <param name="buffer">a pointer pointing to the memory address of the buffer</param>
        /// <param name="offset">the zero-based buffer offset</param>
        /// <returns>the <see cref="float"/> value</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float ToFloat(byte* buffer, int offset) => ToFloat(&buffer[offset]);

        /// <summary>
        ///     Converts the specified <paramref name="buffer"/> to a <see cref="float"/> value.
        /// </summary>
        /// <param name="buffer">a pointer pointing to the memory address of the buffer</param>
        /// <returns>the <see cref="float"/> value</returns>
        public static float ToFloat(byte* buffer)
        {
            // read binary float representation (int)
            var value = ToInt32(buffer);

            // reinterpret value
            return *(float*)&value;
        }

        /// <summary>
        ///     Converts the <paramref name="buffer"/> to an <see cref="short"/> value.
        /// </summary>
        /// <param name="buffer">the buffer</param>
        /// <param name="offset">the zero-based buffer offset</param>
        /// <returns>the <see cref="short"/> value</returns>
        /// <exception cref="InvalidOperationException">
        ///     thrown if the specified buffer is too small in relation with the specified byte <paramref name="offset"/>
        /// </exception>
        public static short ToInt16(byte[] buffer, int offset = 0)
        {
            // range check offset
            if (buffer.Length < offset + sizeof(short))
            {
                throw new InvalidOperationException("The specified buffer is too small.");
            }

            // fix buffer in memory
            fixed (byte* ptr = &buffer[offset])
            {
                // convert value
                return ToInt16(ptr);
            }
        }

        /// <summary>
        ///     Converts the <paramref name="buffer"/> to an <see cref="short"/> value.
        /// </summary>
        /// <param name="buffer">a pointer pointing to the memory address of the buffer</param>
        /// <param name="offset">the zero-based buffer offset</param>
        /// <returns>the <see cref="short"/> value</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static short ToInt16(byte* buffer, int offset) => ToInt16(&buffer[offset]);

        /// <summary>
        ///     Converts the specified <paramref name="buffer"/> to a <see cref="short"/> value.
        /// </summary>
        /// <param name="buffer">a pointer pointing to the memory address of the buffer</param>
        /// <returns>the <see cref="short"/> value</returns>
        public static short ToInt16(byte* buffer)
            => (short)((buffer[0] << 8) | buffer[1]);

        /// <summary>
        ///     Converts the <paramref name="buffer"/> to an <see cref="int"/> value.
        /// </summary>
        /// <param name="buffer">the buffer</param>
        /// <param name="offset">the zero-based buffer offset</param>
        /// <returns>the <see cref="int"/> value</returns>
        /// <exception cref="InvalidOperationException">
        ///     thrown if the specified buffer is too small in relation with the specified byte <paramref name="offset"/>
        /// </exception>
        public static int ToInt32(byte[] buffer, int offset = 0)
        {
            // range check offset
            if (buffer.Length < offset + sizeof(int))
            {
                throw new InvalidOperationException("The specified buffer is too small.");
            }

            // fix buffer in memory
            fixed (byte* ptr = &buffer[offset])
            {
                // convert value
                return ToInt32(ptr);
            }
        }

        /// <summary>
        ///     Converts the <paramref name="buffer"/> to an <see cref="int"/> value.
        /// </summary>
        /// <param name="buffer">a pointer pointing to the memory address of the buffer</param>
        /// <param name="offset">the zero-based buffer offset</param>
        /// <returns>the <see cref="int"/> value</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ToInt32(byte* buffer, int offset) => ToInt32(&buffer[offset]);

        /// <summary>
        ///     Converts the specified <paramref name="buffer"/> to a <see cref="int"/> value.
        /// </summary>
        /// <param name="buffer">a pointer pointing to the memory address of the buffer</param>
        /// <returns>the <see cref="int"/> value</returns>
        public static int ToInt32(byte* buffer)
            => (buffer[0] << 24) | (buffer[1] << 16)
             | (buffer[2] << 8) | buffer[3];

        /// <summary>
        ///     Converts the <paramref name="buffer"/> to an <see cref="long"/> value.
        /// </summary>
        /// <param name="buffer">the buffer</param>
        /// <param name="offset">the zero-based buffer offset</param>
        /// <returns>the <see cref="long"/> value</returns>
        /// <exception cref="InvalidOperationException">
        ///     thrown if the specified buffer is too small in relation with the specified byte <paramref name="offset"/>
        /// </exception>
        public static long ToInt64(byte[] buffer, int offset = 0)
        {
            // range check offset
            if (buffer.Length < offset + sizeof(long))
            {
                throw new InvalidOperationException("The specified buffer is too small.");
            }

            // fix buffer in memory
            fixed (byte* ptr = &buffer[offset])
            {
                // convert value
                return ToInt64(ptr);
            }
        }

        /// <summary>
        ///     Converts the <paramref name="buffer"/> to an <see cref="long"/> value.
        /// </summary>
        /// <param name="buffer">a pointer pointing to the memory address of the buffer</param>
        /// <param name="offset">the zero-based buffer offset</param>
        /// <returns>the <see cref="long"/> value</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long ToInt64(byte* buffer, int offset) => ToInt64(&buffer[offset]);

        /// <summary>
        ///     Converts the specified <paramref name="buffer"/> to a <see cref="long"/> value.
        /// </summary>
        /// <param name="buffer">a pointer pointing to the memory address of the buffer</param>
        /// <returns>the <see cref="long"/> value</returns>
        public static long ToInt64(byte* buffer)
        {
            var value = (long)((buffer[0] << 24) | (buffer[1] << 16) | (buffer[2] << 8) | buffer[3]);
            return (value << 32) | (uint)((buffer[4] << 24) | (buffer[5] << 16) | (buffer[6] << 8) | buffer[7]);
        }

        /// <summary>
        ///     Converts the <paramref name="buffer"/> to an <see cref="ushort"/> value.
        /// </summary>
        /// <param name="buffer">the buffer</param>
        /// <param name="offset">the zero-based buffer offset</param>
        /// <returns>the <see cref="ushort"/> value</returns>
        /// <exception cref="InvalidOperationException">
        ///     thrown if the specified buffer is too small in relation with the specified byte <paramref name="offset"/>
        /// </exception>
        public static ushort ToUInt16(byte[] buffer, int offset = 0)
        {
            // range check offset
            if (buffer.Length < offset + sizeof(ushort))
            {
                throw new InvalidOperationException("The specified buffer is too small.");
            }

            // fix buffer in memory
            fixed (byte* ptr = &buffer[offset])
            {
                // convert value
                return ToUInt16(ptr);
            }
        }

        /// <summary>
        ///     Converts the <paramref name="buffer"/> to an <see cref="ushort"/> value.
        /// </summary>
        /// <param name="buffer">a pointer pointing to the memory address of the buffer</param>
        /// <param name="offset">the zero-based buffer offset</param>
        /// <returns>the <see cref="ushort"/> value</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort ToUInt16(byte* buffer, int offset) => ToUInt16(&buffer[offset]);

        /// <summary>
        ///     Converts the specified <paramref name="buffer"/> to a <see cref="ushort"/> value.
        /// </summary>
        /// <param name="buffer">a pointer pointing to the memory address of the buffer</param>
        /// <returns>the <see cref="ushort"/> value</returns>
        public static ushort ToUInt16(byte* buffer)
            => (ushort)((buffer[0] << 8) | buffer[1]);

        /// <summary>
        ///     Converts the <paramref name="buffer"/> to an <see cref="uint"/> value.
        /// </summary>
        /// <param name="buffer">the buffer</param>
        /// <param name="offset">the zero-based buffer offset</param>
        /// <returns>the <see cref="uint"/> value</returns>
        /// <exception cref="InvalidOperationException">
        ///     thrown if the specified buffer is too small in relation with the specified byte <paramref name="offset"/>
        /// </exception>
        public static uint ToUInt32(byte[] buffer, int offset = 0)
        {
            // range check offset
            if (buffer.Length < offset + sizeof(uint))
            {
                throw new InvalidOperationException("The specified buffer is too small.");
            }

            // fix buffer in memory
            fixed (byte* ptr = &buffer[offset])
            {
                // convert value
                return ToUInt32(ptr);
            }
        }

        /// <summary>
        ///     Converts the <paramref name="buffer"/> to an <see cref="uint"/> value.
        /// </summary>
        /// <param name="buffer">a pointer pointing to the memory address of the buffer</param>
        /// <param name="offset">the zero-based buffer offset</param>
        /// <returns>the <see cref="uint"/> value</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint ToUInt32(byte* buffer, int offset) => ToUInt32(&buffer[offset]);

        /// <summary>
        ///     Converts the specified <paramref name="buffer"/> to a <see cref="uint"/> value.
        /// </summary>
        /// <param name="buffer">a pointer pointing to the memory address of the buffer</param>
        /// <returns>the <see cref="uint"/> value</returns>
        public static uint ToUInt32(byte* buffer)
            => (uint)((buffer[0] << 24) | (buffer[1] << 16)
             | (buffer[2] << 8) | buffer[3]);

        /// <summary>
        ///     Converts the <paramref name="buffer"/> to an <see cref="ulong"/> value.
        /// </summary>
        /// <param name="buffer">the buffer</param>
        /// <param name="offset">the zero-based buffer offset</param>
        /// <returns>the <see cref="ulong"/> value</returns>
        /// <exception cref="InvalidOperationException">
        ///     thrown if the specified buffer is too small in relation with the specified byte <paramref name="offset"/>
        /// </exception>
        public static ulong ToUInt64(byte[] buffer, int offset = 0)
        {
            // range check offset
            if (buffer.Length < offset + sizeof(ulong))
            {
                throw new InvalidOperationException("The specified buffer is too small.");
            }

            // fix buffer in memory
            fixed (byte* ptr = &buffer[offset])
            {
                // convert value
                return ToUInt64(ptr);
            }
        }

        /// <summary>
        ///     Converts the <paramref name="buffer"/> to an <see cref="ulong"/> value.
        /// </summary>
        /// <param name="buffer">a pointer pointing to the memory address of the buffer</param>
        /// <param name="offset">the zero-based buffer offset</param>
        /// <returns>the <see cref="ulong"/> value</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong ToUInt64(byte* buffer, int offset) => ToUInt64(&buffer[offset]);

        /// <summary>
        ///     Converts the specified <paramref name="buffer"/> to a <see cref="ulong"/> value.
        /// </summary>
        /// <param name="buffer">a pointer pointing to the memory address of the buffer</param>
        /// <returns>the <see cref="ulong"/> value</returns>
        public static ulong ToUInt64(byte* buffer)
        {
            var value = (ulong)((buffer[0] << 24) | (buffer[1] << 16) | (buffer[2] << 8) | buffer[3]);
            return (value << 32) | (uint)((buffer[4] << 24) | (buffer[5] << 16) | (buffer[6] << 8) | buffer[7]);
        }
    }
}
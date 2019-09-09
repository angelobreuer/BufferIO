namespace ByteBuffer.Abstractions
{
    using System;
    using System.Text;

    public interface IBinaryWriter
    {
        /// <summary>
        ///     Writes the specified <paramref name="value"/> to the buffer.
        /// </summary>
        /// <param name="value">the value to write</param>
        void Write(bool value);

        /// <summary>
        ///     Writes the specified <paramref name="value"/> to the buffer.
        /// </summary>
        /// <param name="value">the value to write</param>
        void Write(byte value);

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
        void Write(byte[] buffer);

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
        void Write(ArraySegment<byte> buffer);

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
        void Write(byte[] buffer, int offset, int count);

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
        void Write(int value);

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
        void Write(uint value);

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
        void Write(ushort value);

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
        void Write(float value);

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
        void Write(double value);

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
        void Write(short value);

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
        void Write(sbyte value);

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
        void Write(long value);

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
        void Write(ulong value);

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
        void Write(Guid value);

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
        int Write(string value, int charCount);

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
        int Write(string value);

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
        int Write(string value, int charIndex, int charCount);

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
        int Write(string value, int charCount, Encoding encoding);

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
        int Write(string value, Encoding encoding);

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
        int Write(string value, int charIndex, int charCount, Encoding encoding);
    }
}
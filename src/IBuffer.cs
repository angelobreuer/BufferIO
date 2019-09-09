namespace ByteBuffer
{
    using System;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    ///     Interface for byte buffers.
    /// </summary>
    public interface IBuffer : IDisposable
    {
        /// <summary>
        ///     Gets or sets the number of allocated internal buffer bytes.
        /// </summary>
        int Capacity { get; set; }

        /// <summary>
        ///     Gets the buffer endianness.
        /// </summary>
        Endianness Endianness { get; }

        /// <summary>
        ///     Gets a value indicating whether the buffer is empty.
        /// </summary>
        bool IsEmpty { get; }

        /// <summary>
        ///     Gets a value indicating whether the buffer is expandable.
        /// </summary>
        bool IsExpandable { get; }

        /// <summary>
        ///     Gets a value indicating whether the internal buffer is exposable.
        /// </summary>
        bool IsExposable { get; }

        /// <summary>
        ///     Gets a value indicating whether the buffer was pooled from an array pool.
        /// </summary>
        bool IsPooled { get; }

        /// <summary>
        ///     Gets a value indicating whether the buffer is read-only.
        /// </summary>
        bool IsReadOnly { get; }

        /// <summary>
        ///     Gets or sets the current length of the buffer.
        /// </summary>
        int Length { get; set; }

        /// <summary>
        ///     Gets or sets the current buffer cursor position.
        /// </summary>
        int Position { get; set; }

        /// <summary>
        ///     Gets the amount of bytes remaining until the buffer is full.
        /// </summary>
        int Remaining { get; }

        /// <summary>
        ///     Gets or sets the <see cref="byte"/> at the specified zero-based <paramref name="index"/>.
        /// </summary>
        /// <param name="index">the zero-based absolute <see cref="byte"/> index</param>
        /// <returns>the <see cref="byte"/> at the specified zero-based <paramref name="index"/></returns>
        byte this[int index] { get; set; }

        /// <summary>
        ///     Creates a memory stream from the buffer.
        /// </summary>
        /// <returns>the memory stream</returns>
        MemoryStream AsMemoryStream();

        /// <summary>
        ///     Clears the buffer.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        ///     thrown if the buffer is read-only ( <see cref="IsReadOnly"/>)
        /// </exception>
        void Clear();

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
        void CopyTo(Stream stream, bool full = true);

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
        Task CopyToAsync(Stream stream, bool full = true, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Gets the internal buffer.
        /// </summary>
        /// <returns>the internal buffer</returns>
        /// <exception cref="UnauthorizedAccessException">
        ///     thrown if the buffer is not exposable ( <see cref="IsExposable"/>).
        /// </exception>
        ArraySegment<byte> GetBuffer();

        /// <summary>
        ///     Resets the cursor position.
        /// </summary>
        void Reset();

        /// <summary>
        ///     Creates an array of the buffer data.
        /// </summary>
        /// <returns>an array of the buffer data</returns>
        byte[] ToArray();

        /// <summary>
        ///     Trims the internal buffer to the number of bytes used.
        /// </summary>
        void Trim();

        /// <summary>
        ///     Tries to get the internal buffer.
        /// </summary>
        /// <param name="buffer">the internal buffer</param>
        /// <returns>a value indicating whether the buffer could be get</returns>
        bool TryGetBuffer(out ArraySegment<byte> buffer);
    }
}
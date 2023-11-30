namespace BufferIO.IO;

using System;
using System.IO;

/// <summary>
///     The <see cref="Stream"/> wrapper for writing to / reading from a <see cref="ByteBuffer"/>.
/// </summary>
internal sealed class ByteBufferStream : Stream
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ByteBufferStream"/> class.
    /// </summary>
    /// <param name="buffer"></param>
    public ByteBufferStream(ByteBuffer buffer)
        => Buffer = buffer ?? throw new ArgumentNullException(nameof(buffer));

    /// <summary>
    ///     Gets the underlying buffer.
    /// </summary>
    public ByteBuffer Buffer { get; }

    /// <summary>
    ///     Gets a value indicating whether the stream supports reading.
    /// </summary>
    public override bool CanRead => true;

    /// <summary>
    ///     Gets a value indicating whether the stream supports seeking.
    /// </summary>
    public override bool CanSeek => true;

    /// <summary>
    ///     Gets a value indicating whether the stream supports writing.
    /// </summary>
    public override bool CanWrite => Buffer.IsReadOnly;

    /// <summary>
    ///     Gets the current length of the underlying buffer.
    /// </summary>
    public override long Length => Buffer.Length;

    /// <summary>
    ///     Gets or sets the writing / reading position of the underlying buffer.
    /// </summary>
    public override long Position
    {
        get => Buffer.Position;
        set => Buffer.Position = (int)value;
    }

    /// <summary>
    ///     Flushes the stream.
    /// </summary>
    public override void Flush()
    {
    }

    /// <summary>
    ///     Reads data from the underlying <see cref="Buffer"/>.
    /// </summary>
    /// <param name="buffer">the buffer</param>
    /// <param name="offset">the offset</param>
    /// <param name="count">the number of bytes to read</param>
    /// <returns>the number of bytes read</returns>
    public override int Read(byte[] buffer, int offset, int count)
    {
        count = Math.Min(Buffer.Remaining, count);
        Buffer.ReadBytes(buffer, offset, count);
        return count;
    }

    /// <summary>
    ///     Seeks a position in the stream.
    /// </summary>
    /// <param name="offset">the seek offset</param>
    /// <param name="origin">the seek origin</param>
    /// <returns>the new position in the stream</returns>
    public override long Seek(long offset, SeekOrigin origin)
    {
        var position = FindPosition(offset, origin);

        position = Math.Max(position, 0);
        position = Math.Min(position, Buffer.Length);

        Buffer.Position = (int)position;

        return position;
    }

    /// <summary>
    ///     Sets the stream length to the specified <paramref name="value"/>.
    /// </summary>
    /// <param name="value">the target stream length</param>
    public override void SetLength(long value)
    {
        if (value < 0 || value > int.MaxValue)
        {
            throw new ArgumentOutOfRangeException(nameof(value), value,
                "The specified stream length is not applicable for a ByteBuffer.");
        }

        Buffer.Length = (int)value;
    }

    /// <summary>
    ///     Writes the specified <paramref name="buffer"/> to the underlying <see cref="Buffer"/>.
    /// </summary>
    /// <param name="buffer">the buffer to read from</param>
    /// <param name="offset">the offset</param>
    /// <param name="count">the number of bytes to write</param>
    public override void Write(byte[] buffer, int offset, int count)
        => Buffer.Write(buffer, offset, count);

    private long FindPosition(long offset, SeekOrigin origin)
    {
        switch (origin)
        {
            case SeekOrigin.Begin:
                return offset;

            case SeekOrigin.Current:
                return Buffer.Position + offset;

            case SeekOrigin.End:
                return Buffer.Length - offset;

            default:
                throw new ArgumentOutOfRangeException(nameof(origin), origin,
                    "Unsupported or undefined seek origin specified.");
        }
    }
}
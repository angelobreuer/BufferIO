namespace ByteBuffer
{
    using System;
    using System.Runtime.CompilerServices;

    /// <summary>
    ///     An utility class that helps with swapping endianness.
    /// </summary>
    public static class EndianConverter
    {
        /// <summary>
        ///     Gets the system-endianness.
        /// </summary>
        public static Endianness SystemEndianness { get; }
            = BitConverter.IsLittleEndian ? Endianness.LittleEndian : Endianness.BigEndian;

        /// <summary>
        ///     Determines whether a byte-swap is needed to preserve the specified byte-order ( <paramref name="endianness"/>).
        /// </summary>
        /// <param name="sourceEndianness">the source endianness / the current buffer endianness</param>
        /// <param name="endianness">the target endianness / the endianness to convert to</param>
        /// <returns>
        ///     a value indicating whether a byte-swap is needed to preserve the specified byte-order
        ///     ( <paramref name="endianness"/>)
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ShouldSwap(Endianness sourceEndianness, Endianness endianness)
            => sourceEndianness != endianness;

        /// <summary>
        ///     Determines whether a byte-swap is needed to preserve the specified byte-order ( <paramref name="endianness"/>).
        /// </summary>
        /// <remarks>This method assumes that the source buffer is in system-endianness byte-order.</remarks>
        /// <param name="endianness">the target endianness / the endianness to convert to</param>
        /// <returns>
        ///     a value indicating whether a byte-swap is needed to preserve the specified byte-order
        ///     ( <paramref name="endianness"/>)
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ShouldSwap(Endianness endianness)
            => SystemEndianness != endianness;

        /// <summary>
        ///     Swaps the endianness of the specified <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">the buffer to swap the bytes</param>
        /// <param name="offset">the buffer swap offset</param>
        /// <param name="count">the buffer swap count</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SwapEndianness(byte[] buffer, int offset, int count)
            => Array.Reverse(buffer, offset, count);

        /// <summary>
        ///     Swaps the endianness of the specified <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">the buffer to swap the bytes</param>
        /// <param name="count">the buffer swap count</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SwapEndianness(byte[] buffer, int count)
            => Array.Reverse(buffer, index: 0, count);

        /// <summary>
        ///     Swaps the endianness of the specified <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">the buffer to swap the bytes</param>
        /// <param name="count">the buffer swap count</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SwapEndianness(byte[] buffer) => Array.Reverse(buffer);
    }
}
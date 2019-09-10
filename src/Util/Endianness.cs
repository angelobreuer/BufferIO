namespace BufferIO.Util
{
    /// <summary>
    ///     A set of the supported endianness (see: https://en.wikipedia.org/wiki/Endianness for more
    ///     details about Endianness).
    /// </summary>
    public enum Endianness
    {
        /// <summary>
        ///     Denotes that the byte-order is little-endian.
        /// </summary>
        LittleEndian,

        /// <summary>
        ///     Denotes that the byte-order is big-endian.
        /// </summary>
        BigEndian
    }
}
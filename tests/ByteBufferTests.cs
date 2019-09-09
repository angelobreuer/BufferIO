namespace ByteBuffer.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;
    using Xunit;

    public sealed class ByteBufferTests
    {
        /// <summary>
        ///     A set of <see cref="Guid"/> instances used for testing.
        /// </summary>
        public static IEnumerable<object[]> TestGuids
        {
            get
            {
                yield return new object[] { new Guid() };
                yield return new object[] { Guid.NewGuid() };
                yield return new object[] { Guid.NewGuid() };
                yield return new object[] { Guid.NewGuid() };
            }
        }

        /// <summary>
        ///     Gets the byte buffer being tested.
        /// </summary>
        public ByteBuffer Buffer { get; } = new ByteBuffer();

        public static IEnumerable<object[]> GenerateRandomData(int cycles)
            => GenerateRandomData(minimumCount: 100, maximumCount: 500, cycles);

        public static IEnumerable<object[]> GenerateRandomData(int minimumCount, int maximumCount, int cycles)
        {
            var random = new Random();

            for (var cycle = 0; cycle < cycles; cycle++)
            {
                var length = random.Next(minimumCount, maximumCount);
                var buffer = new byte[length];
                random.NextBytes(buffer);
                yield return new[] { buffer };
            }
        }

        public static IEnumerable<object> GenerateSingleUnionData()
        {
            var random = new Random();

            var buffer = new byte[random.Next(400)];
            random.NextBytes(buffer);

            yield return (byte)random.Next(byte.MinValue, byte.MaxValue); // byte
            yield return (sbyte)random.Next(sbyte.MinValue, sbyte.MaxValue); // sbyte
            yield return (short)random.Next(short.MinValue, short.MaxValue); // short
            yield return (ushort)random.Next(ushort.MinValue, ushort.MaxValue); // ushort
            yield return random.Next(); // int
            yield return (uint)random.Next(); // uint
            yield return (long)random.Next(); // long
            yield return (ulong)random.Next(); // ulong
            yield return (float)random.NextDouble(); // float
            yield return random.NextDouble(); // double
            yield return Guid.NewGuid(); // guid
            yield return buffer; // byte[]
            yield return Encoding.UTF8.GetString(buffer); // string
        }

        public static IEnumerable<object[]> GenerateUnionData(int cycles)
                    => Enumerable.Range(0, cycles).Select(_ => GenerateSingleUnionData().ToArray());

        /// <summary>
        ///     Tests clearing the buffer.
        /// </summary>
        [Fact]
        public void TestClear()
        {
            // assert buffer is empty
            Assert.Empty(Buffer.ToArray());
            Assert.True(Buffer.IsEmpty);
            Assert.Equal(0, Buffer.Length);

            // write some data
            Buffer.Write(1);

            // assert buffer is not empty
            Assert.NotEmpty(Buffer.ToArray());
            Assert.False(Buffer.IsEmpty);
            Assert.NotEqual(0, Buffer.Length);

            // clear buffer
            Buffer.Clear();

            // assert buffer is empty
            Assert.Empty(Buffer.ToArray());
            Assert.True(Buffer.IsEmpty);
            Assert.Equal(0, Buffer.Length);
        }

        /// <summary>
        ///     Tests writing / reading a <see cref="double"/> to / from the buffer.
        /// </summary>
        /// <param name="value">the value</param>
        [Theory]
        [InlineData(.77515475415d), InlineData(5451152151d)]
        [InlineData(515144.55451515151d), InlineData(5151511.585445514454d)]
        [InlineData(double.MinValue), InlineData(double.MaxValue)]
        public void TestDouble(double value)
        {
            // write
            Buffer.Write(value);
            Buffer.Reset();

            // read
            Assert.Equal(value, Buffer.ReadDouble());
        }

        /// <summary>
        ///     Tests writing / reading a <see cref="float"/> to / from the buffer.
        /// </summary>
        /// <param name="value">the value</param>
        [Theory]
        [InlineData(.775154f), InlineData(545615156f)]
        [InlineData(5151.515151f), InlineData(511.585445f)]
        [InlineData(float.MinValue), InlineData(float.MaxValue)]
        public void TestFloat(float value)
        {
            // write
            Buffer.Write(value);
            Buffer.Reset();

            // read
            Assert.Equal(value, Buffer.ReadFloat());
        }

        /// <summary>
        ///     Tests writing / reading of a <see cref="Guid"/> to / from the buffer.
        /// </summary>
        /// <param name="guid">the value</param>
        [Theory]
        [MemberData(nameof(TestGuids))]
        public void TestGuid(Guid guid)
        {
            // write
            Buffer.Write(guid);
            Buffer.Reset();

            // read
            Assert.Equal(guid, Buffer.ReadGuid());
        }

        /// <summary>
        ///     Tests writing / reading an <see cref="int"/> to / from the buffer.
        /// </summary>
        /// <param name="value">the value</param>
        [Theory]
        [InlineData(1654551), InlineData(456456564)]
        [InlineData(-415151), InlineData(-949754164)]
        [InlineData(int.MinValue), InlineData(int.MaxValue)]
        public void TestInt(int value)
        {
            // write
            Buffer.Write(value);
            Buffer.Reset();

            // read
            Assert.Equal(value, Buffer.ReadInt());
        }

        /// <summary>
        ///     Tests writing / reading a <see cref="long"/> to / from the buffer.
        /// </summary>
        /// <param name="value">the value</param>
        [Theory]
        [InlineData(-45848115841), InlineData(-91548548464)]
        [InlineData(1645154544451), InlineData(4565156156184)]
        [InlineData(long.MinValue), InlineData(long.MaxValue)]
        public void TestLong(long value)
        {
            // write
            Buffer.Write(value);
            Buffer.Reset();

            // read
            Assert.Equal(value, Buffer.ReadLong());
        }

        [Theory]
        [MemberData(nameof(GenerateRandomData), 8)]
        public void TestOffsetWrite(byte[] buffer)
        {
            // create the byte buffer with a custom backend buffer
            var myBuffer = new byte[buffer.Length * 2];

            // the expected buffer
            var expectedBuffer = new byte[buffer.Length * 2];
            Array.Copy(buffer, 0, expectedBuffer, buffer.Length, buffer.Length);

            using (var byteBuffer = new ByteBuffer(myBuffer, buffer.Length, buffer.Length) { Length = 0 })
            {
                // fill data
                byteBuffer.Write(buffer);

                // ensure buffer matches
                Assert.Equal(buffer, byteBuffer.ToArray());
                Assert.Equal(expectedBuffer, myBuffer);
            }
        }

        [Theory]
        [MemberData(nameof(GenerateRandomData), 8)]
        public void TestRandomAccess(byte[] buffer)
        {
            const int cycles = 500;
            var random = new Random();

            // write the data to the buffer
            Buffer.Write(buffer);

            // run random access cycles
            for (var cycle = 0; cycle < cycles; cycle++)
            {
                var position = random.Next(Buffer.Length);

                Assert.Equal(buffer[position], Buffer[position]);
            }
        }

        /// <summary>
        ///     Tests reading a <see cref="double"/> value.
        /// </summary>
        /// <param name="value">the value</param>
        [Theory]
        [InlineData(.77515475415d), InlineData(5451152151d)]
        [InlineData(515144.55451515151d), InlineData(5151511.585445514454d)]
        [InlineData(double.MinValue), InlineData(double.MaxValue)]
        public void TestReadDouble(double value)
        {
            // write the buffer
            Buffer.Write(GetEquivalent(value, BitConverter.GetBytes));
            Buffer.Reset();

            // ensure the buffers are equal
            Assert.Equal(value, Buffer.ReadDouble());
        }

        /// <summary>
        ///     Tests reading a <see cref="float"/> value.
        /// </summary>
        /// <param name="value">the value</param>
        [Theory]
        [InlineData(.775154f), InlineData(545615156f)]
        [InlineData(5151.515151f), InlineData(511.585445f)]
        [InlineData(float.MinValue), InlineData(float.MaxValue)]
        public void TestReadFloat(float value)
        {
            // write the buffer
            Buffer.Write(GetEquivalent(value, BitConverter.GetBytes));
            Buffer.Reset();

            // ensure the buffers are equal
            Assert.Equal(value, Buffer.ReadFloat());
        }

        /// <summary>
        ///     Tests reading a <see cref="Guid"/> from the buffer.
        /// </summary>
        /// <param name="guid">the value</param>
        [Theory]
        [MemberData(nameof(TestGuids))]
        public void TestReadGuid(Guid guid)
        {
            // write guid data
            Buffer.Write(guid.ToByteArray());
            Buffer.Reset();

            // ensure the guid is read correctly
            Assert.Equal(guid, Buffer.ReadGuid());
        }

        /// <summary>
        ///     Tests reading an <see cref="int"/> value.
        /// </summary>
        /// <param name="value">the value</param>
        [Theory]
        [InlineData(1654551), InlineData(456456564)]
        [InlineData(-415151), InlineData(-949754164)]
        [InlineData(int.MinValue), InlineData(int.MaxValue)]
        public void TestReadInt(int value)
        {
            // write the buffer
            Buffer.Write(GetEquivalent(value, BitConverter.GetBytes));
            Buffer.Reset();

            // ensure the buffers are equal
            Assert.Equal(value, Buffer.ReadInt());
        }

        /// <summary>
        ///     Tests reading a <see cref="long"/> value.
        /// </summary>
        /// <param name="value">the value</param>
        [Theory]
        [InlineData(-45848115841), InlineData(-91548548464)]
        [InlineData(1645154544451), InlineData(4565156156184)]
        [InlineData(long.MinValue), InlineData(long.MaxValue)]
        public void TestReadLong(long value)
        {
            // write the buffer
            Buffer.Write(GetEquivalent(value, BitConverter.GetBytes));
            Buffer.Reset();

            // ensure the buffers are equal
            Assert.Equal(value, Buffer.ReadLong());
        }

        /// <summary>
        ///     Tests reading a <see cref="short"/> value.
        /// </summary>
        /// <param name="value">the value</param>
        [Theory]
        [InlineData(1651), InlineData(4564)]
        [InlineData(-451), InlineData(-9164)]
        [InlineData(short.MinValue), InlineData(short.MaxValue)]
        public void TestReadShort(short value)
        {
            // write the buffer
            Buffer.Write(GetEquivalent(value, BitConverter.GetBytes));
            Buffer.Reset();

            // ensure the buffers are equal
            Assert.Equal(value, Buffer.ReadShort());
        }

        /// <summary>
        ///     Tests reading a <see cref="string"/> from the buffer.
        /// </summary>
        /// <param name="value">the value to test</param>
        [Theory]
        [InlineData(""), InlineData("\0")]
        [InlineData("Hello World!"), InlineData("\t\u6161\u4474")]
        public void TestReadString(string value)
        {
            // write
            Buffer.Write((ushort)Encoding.UTF8.GetByteCount(value));
            Buffer.Write(Encoding.UTF8.GetBytes(value));
            Buffer.Reset();

            // read
            Assert.Equal(value, Buffer.ReadString());
        }

        /// <summary>
        ///     Tests reading an <see cref="uint"/> value.
        /// </summary>
        /// <param name="value">the value</param>
        [Theory]
        [InlineData(1654551), InlineData(456456564)]
        [InlineData(415151), InlineData(949754164)]
        [InlineData(uint.MinValue), InlineData(uint.MaxValue)]
        public void TestReadUInt(uint value)
        {
            // write the buffer
            Buffer.Write(GetEquivalent(value, BitConverter.GetBytes));
            Buffer.Reset();

            // ensure the buffers are equal
            Assert.Equal(value, Buffer.ReadUInt());
        }

        /// <summary>
        ///     Tests reading a <see cref="ulong"/> value.
        /// </summary>
        /// <param name="value">the value</param>
        [Theory]
        [InlineData(45848115841), InlineData(91548548464)]
        [InlineData(1645154544451), InlineData(4565156156184)]
        [InlineData(ulong.MinValue), InlineData(ulong.MaxValue)]
        public void TestReadULong(ulong value)
        {
            // write the buffer
            Buffer.Write(GetEquivalent(value, BitConverter.GetBytes));
            Buffer.Reset();

            // ensure the buffers are equal
            Assert.Equal(value, Buffer.ReadULong());
        }

        [Theory]
        [MemberData(nameof(GenerateRandomData), 10)]
        public unsafe void TestReadUnsafe(byte[] buffer)
        {
            // write the data
            Buffer.Write(buffer);
            Buffer.Reset();

            // allocate unsafe buffer on stack
            var buf = stackalloc byte[buffer.Length];

            // read to buffer
            Buffer.ReadBytes(buf, buffer.Length);

            // create managed buffer
            var data = new byte[buffer.Length];
            Marshal.Copy(new IntPtr(buf), data, 0, data.Length);

            // verify
            Assert.Equal(buffer, data);
        }

        /// <summary>
        ///     Tests reading an <see cref="ushort"/> value.
        /// </summary>
        /// <param name="value">the value</param>
        [Theory]
        [InlineData(1651), InlineData(4564)]
        [InlineData(45451), InlineData(17164)]
        [InlineData(ushort.MinValue), InlineData(ushort.MaxValue)]
        public void TestReadUShort(ushort value)
        {
            // write the buffer
            Buffer.Write(GetEquivalent(value, BitConverter.GetBytes));
            Buffer.Reset();

            // ensure the buffers are equal
            Assert.Equal(value, Buffer.ReadUShort());
        }

        /// <summary>
        ///     Tests writing / reading a <see cref="short"/> to / from the buffer.
        /// </summary>
        /// <param name="value">the value</param>
        [Theory]
        [InlineData(1651), InlineData(4564)]
        [InlineData(-451), InlineData(-9164)]
        [InlineData(short.MinValue), InlineData(short.MaxValue)]
        public void TestShort(short value)
        {
            // write
            Buffer.Write(value);
            Buffer.Reset();

            // read
            Assert.Equal(value, Buffer.ReadShort());
        }

        [Theory]
        [MemberData(nameof(GenerateRandomData), 8)]
        public void TestSingleWrite(byte[] buffer)
        {
            // write test data
            Array.ForEach(buffer, Buffer.Write);

            // ensure matching
            Assert.Equal(buffer, Buffer.ToArray());
        }

        /// <summary>
        ///     Tests writing / reading a <see cref="string"/> to / from the buffer.
        /// </summary>
        /// <param name="value">the value to test</param>
        [Theory]
        [InlineData(""), InlineData("\0")]
        [InlineData("Hello World!"), InlineData("\t\u6161\u4474")]
        public void TestString(string value)
        {
            // write
            Buffer.Write(value);
            Buffer.Reset();

            // read
            Assert.Equal(value, Buffer.ReadString());
        }

        /// <summary>
        ///     Tests writing / reading an <see cref="uint"/> to / from the buffer.
        /// </summary>
        /// <param name="value">the value</param>
        [Theory]
        [InlineData(1654551), InlineData(456456564)]
        [InlineData(415151), InlineData(949754164)]
        [InlineData(uint.MinValue), InlineData(uint.MaxValue)]
        public void TestUInt(uint value)
        {
            // write
            Buffer.Write(value);
            Buffer.Reset();

            // read
            Assert.Equal(value, Buffer.ReadUInt());
        }

        /// <summary>
        ///     Tests writing / reading a <see cref="ulong"/> to / from the buffer.
        /// </summary>
        /// <param name="value">the value</param>
        [Theory]
        [InlineData(45848115841), InlineData(91548548464)]
        [InlineData(1645154544451), InlineData(4565156156184)]
        [InlineData(ulong.MinValue), InlineData(ulong.MaxValue)]
        public void TestULong(ulong value)
        {
            // write
            Buffer.Write(value);
            Buffer.Reset();

            // read
            Assert.Equal(value, Buffer.ReadULong());
        }

        /// <summary>
        ///     Tests writing / reading an <see cref="ushort"/> to / from the buffer.
        /// </summary>
        /// <param name="value">the value</param>
        [Theory]
        [InlineData(1651), InlineData(4564)]
        [InlineData(45451), InlineData(17164)]
        [InlineData(ushort.MinValue), InlineData(ushort.MaxValue)]
        public void TestUShort(ushort value)
        {
            // write
            Buffer.Write(value);
            Buffer.Reset();

            // read
            Assert.Equal(value, Buffer.ReadUShort());
        }

        [Theory]
        [MemberData(nameof(GenerateRandomData), 8)]
        public void TestWriteBytes(byte[] buffer)
        {
            // write data
            Buffer.Write(buffer, 0, buffer.Length);

            // assertions
            Assert.Equal(buffer.Length, Buffer.Position);
            Assert.Equal(buffer.Length, Buffer.Length);
            Assert.True(Buffer.Capacity >= buffer.Length);
            Assert.Equal(buffer, Buffer.ToArray());
        }

        /// <summary>
        ///     Tests writing a <see cref="double"/> to the buffer.
        /// </summary>
        /// <param name="value">the value</param>
        [Theory]
        [InlineData(.77515475415d), InlineData(5451152151d)]
        [InlineData(515144.55451515151d), InlineData(5151511.585445514454d)]
        [InlineData(double.MinValue), InlineData(double.MaxValue)]
        public void TestWriteDouble(double value)
        {
            // write the value
            Buffer.Write(value);
            Buffer.Reset();

            // ensure the buffers are equal
            Assert.Equal(GetEquivalent(value, BitConverter.GetBytes), Buffer.ToArray());
        }

        /// <summary>
        ///     Tests writing a <see cref="float"/> to the buffer.
        /// </summary>
        /// <param name="value">the value</param>
        [Theory]
        [InlineData(.775154f), InlineData(545615156f)]
        [InlineData(5151.515151f), InlineData(511.585445f)]
        [InlineData(float.MinValue), InlineData(float.MaxValue)]
        public void TestWriteFloat(float value)
        {
            // write the value
            Buffer.Write(value);
            Buffer.Reset();

            // ensure the buffers are equal
            Assert.Equal(GetEquivalent(value, BitConverter.GetBytes), Buffer.ToArray());
        }

        /// <summary>
        ///     Tests writing a <see cref="Guid"/> to the buffer.
        /// </summary>
        /// <param name="guid">the guid to test</param>
        [Theory]
        [MemberData(nameof(TestGuids))]
        public void TestWriteGuid(Guid guid)
        {
            // write the guid
            Buffer.Write(guid);
            Buffer.Reset();

            // ensure the guid is in the buffer
            Assert.Equal(guid.ToByteArray(), Buffer.ToArray());
        }

        /// <summary>
        ///     Tests writing an <see cref="int"/> to the buffer.
        /// </summary>
        /// <param name="value">the value</param>
        [Theory]
        [InlineData(1654551), InlineData(456456564)]
        [InlineData(-415151), InlineData(-949754164)]
        [InlineData(int.MinValue), InlineData(int.MaxValue)]
        public void TestWriteInt(int value)
        {
            // write the value
            Buffer.Write(value);
            Buffer.Reset();

            // ensure the buffers are equal
            Assert.Equal(GetEquivalent(value, BitConverter.GetBytes), Buffer.ToArray());
        }

        /// <summary>
        ///     Tests writing a <see cref="long"/> to the buffer.
        /// </summary>
        /// <param name="value">the value</param>
        [Theory]
        [InlineData(-45848115841), InlineData(-91548548464)]
        [InlineData(1645154544451), InlineData(4565156156184)]
        [InlineData(long.MinValue), InlineData(long.MaxValue)]
        public void TestWriteLong(long value)
        {
            // write the value
            Buffer.Write(value);
            Buffer.Reset();

            // ensure the buffers are equal
            Assert.Equal(GetEquivalent(value, BitConverter.GetBytes), Buffer.ToArray());
        }

        /// <summary>
        ///     Tests writing a <see cref="short"/> to the buffer.
        /// </summary>
        /// <param name="value">the value</param>
        [Theory]
        [InlineData(1651), InlineData(4564)]
        [InlineData(-451), InlineData(-9164)]
        [InlineData(short.MinValue), InlineData(short.MaxValue)]
        public void TestWriteShort(short value)
        {
            // write the value
            Buffer.Write(value);
            Buffer.Reset();

            // ensure the buffers are equal
            Assert.Equal(GetEquivalent(value, BitConverter.GetBytes), Buffer.ToArray());
        }

        /// <summary>
        ///     Tests writing a <see cref="string"/> to the buffer.
        /// </summary>
        /// <param name="value">the value to test</param>
        [Theory]
        [InlineData(""), InlineData("\0")]
        [InlineData("Hello World!"), InlineData("\t\u6161\u4474")]
        public void TestWriteString(string value)
        {
            // write
            Buffer.Write(value);
            Buffer.Reset();

            // read
            var length = Buffer.ReadUShort();
            var data = Buffer.Read(length);

            // assertions
            Assert.Equal(Encoding.UTF8.GetByteCount(value), length);
            Assert.Equal(data, Encoding.UTF8.GetBytes(value));
            Assert.Equal(value, Encoding.UTF8.GetString(data));
        }

        /// <summary>
        ///     Tests writing an <see cref="int"/> to the buffer.
        /// </summary>
        /// <param name="value">the value</param>
        [Theory]
        [InlineData(1654551), InlineData(456456564)]
        [InlineData(415151), InlineData(949754164)]
        [InlineData(uint.MinValue), InlineData(uint.MaxValue)]
        public void TestWriteUInt(uint value)
        {
            // write the value
            Buffer.Write(value);
            Buffer.Reset();

            // ensure the buffers are equal
            Assert.Equal(GetEquivalent(value, BitConverter.GetBytes), Buffer.ToArray());
        }

        /// <summary>
        ///     Tests writing a <see cref="ulong"/> to the buffer.
        /// </summary>
        /// <param name="value">the value</param>
        [Theory]
        [InlineData(45848115841), InlineData(91548548464)]
        [InlineData(1645154544451), InlineData(4565156156184)]
        [InlineData(ulong.MinValue), InlineData(ulong.MaxValue)]
        public void TestWriteULong(ulong value)
        {
            // write the value
            Buffer.Write(value);
            Buffer.Reset();

            // ensure the buffers are equal
            Assert.Equal(GetEquivalent(value, BitConverter.GetBytes), Buffer.ToArray());
        }

        [Theory]
        [MemberData(nameof(GenerateRandomData), 10)]
        public unsafe void TestWriteUnsafe(byte[] buffer)
        {
            // fix the test data buffer in memory
            fixed (byte* ptr = buffer)
            {
                // write data
                Buffer.Write(ptr, buffer.Length);
                Buffer.Reset();
            }

            // read data
            var data = Buffer.Read(buffer.Length);
            Assert.Equal(buffer, data);
        }

        /// <summary>
        ///     Tests writing an <see cref="ushort"/> to the buffer.
        /// </summary>
        /// <param name="value">the value</param>
        [Theory]
        [InlineData(1651), InlineData(4564)]
        [InlineData(45451), InlineData(17164)]
        [InlineData(ushort.MinValue), InlineData(ushort.MaxValue)]
        public void TestWriteUShort(ushort value)
        {
            // write the value
            Buffer.Write(value);
            Buffer.Reset();

            // ensure the buffers are equal
            Assert.Equal(GetEquivalent(value, BitConverter.GetBytes), Buffer.ToArray());
        }

        [Theory]
        [MemberData(nameof(GenerateUnionData), 20)]
        public void UnionTest(byte a, sbyte b, short c, ushort d, int e, uint f, long g, ulong h, float i, double j, Guid k, byte[] l, string m)
        {
            // write
            Buffer.Write(a);
            Buffer.Write(b);
            Buffer.Write(c);
            Buffer.Write(d);
            Buffer.Write(e);
            Buffer.Write(f);
            Buffer.Write(g);
            Buffer.Write(h);
            Buffer.Write(i);
            Buffer.Write(j);
            Buffer.Write(k);
            Buffer.Write(l);
            Buffer.Write(m);
            Buffer.Reset();

            // read and verify
            Assert.Equal(Buffer.ReadByte(), a);
            Assert.Equal(Buffer.ReadSByte(), b);
            Assert.Equal(Buffer.ReadShort(), c);
            Assert.Equal(Buffer.ReadUShort(), d);
            Assert.Equal(Buffer.ReadInt(), e);
            Assert.Equal(Buffer.ReadUInt(), f);
            Assert.Equal(Buffer.ReadLong(), g);
            Assert.Equal(Buffer.ReadULong(), h);
            Assert.Equal(Buffer.ReadFloat(), i);
            Assert.Equal(Buffer.ReadDouble(), j);
            Assert.Equal(Buffer.ReadGuid(), k);
            Assert.Equal(Buffer.Read(l.Length), l);
            Assert.Equal(Buffer.ReadString(), m);
        }

        private static byte[] GetEquivalent<TValue>(TValue value, Func<TValue, byte[]> converter, bool swapEndianness = true)
        {
            // get value equivalent (in system-endian)
            var buffer = converter(value);

            // check if the current system endian is little-endian
            if (BitConverter.IsLittleEndian && swapEndianness)
            {
                // swap bytes (the ByteBuffer is big-endian)
                Array.Reverse(buffer);
            }

            return buffer;
        }
    }
}
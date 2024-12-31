using System.Buffers;
using System.Buffers.Binary;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace UtilLib.Span
{
	public ref struct SpanReader
	{
		public int Remaining => data.Length - idx;

		ReadOnlySpan<byte> data;
		int idx;
		bool invertEndianess;

		public SpanReader(ReadOnlySpan<byte> data, bool isLittleEndian = false)
		{
			this.data = data;
			this.invertEndianess = isLittleEndian != BitConverter.IsLittleEndian; // If system and data endianess is mismatched invert endianess
		}

		public ulong ReadUInt64()
			=> invertEndianess ? BinaryPrimitives.ReverseEndianness(Read<ulong>()) : Read<ulong>();

		public long ReadInt64()
			=> invertEndianess ? BinaryPrimitives.ReverseEndianness(Read<long>()) : Read<long>();

		public uint ReadUInt32()
			=> invertEndianess ? BinaryPrimitives.ReverseEndianness(Read<uint>()) : Read<uint>();

		public int ReadInt32()
			=> invertEndianess ? BinaryPrimitives.ReverseEndianness(Read<int>()) : Read<int>();

		public ushort ReadUInt16()
			=> invertEndianess ? BinaryPrimitives.ReverseEndianness(Read<ushort>()) : Read<ushort>();

		public short ReadInt16()
			=> invertEndianess ? BinaryPrimitives.ReverseEndianness(Read<short>()) : Read<short>();

		public Half ReadHalf()
			=> Read<Half>();

		public float ReadFloat()
			=> Read<float>();

		public double ReadDouble()
			=> Read<double>();

		public byte ReadByte()
			=> Read<byte>();

		public char ReadChar()
			=> Read<char>();

		public sbyte ReadSByte()
			=> Read<sbyte>();

		public int Read(scoped Span<byte> buffer)
		{
			if (buffer.Length > Remaining)
			{
				data.Slice(idx).TryCopyTo(buffer);
				idx += Remaining;
				return Remaining;
			}
			else
			{
				data.Slice(idx, buffer.Length).TryCopyTo(buffer);
				idx += buffer.Length;
				return buffer.Length;
			}
		}

		unsafe T Read<T>() where T : unmanaged
		{
			idx += sizeof(T);
			return MemoryMarshal.AsRef<T>(data.Slice(idx - sizeof(T), sizeof(T)));
		}

		public static implicit operator SpanReader(ReadOnlySpan<byte> span) => new(span);
	}
}

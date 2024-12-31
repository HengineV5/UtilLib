using System.Buffers.Binary;
using System.Runtime.InteropServices;

namespace UtilLib.Span
{
	public ref struct SpanWriter
	{
		public int Remaining => data.Length - idx;

		Span<byte> data;
		int idx = 0;
		bool invertEndianess;

		public SpanWriter(Span<byte> data, bool isLittleEndian = false)
		{
			this.data = data;
			this.invertEndianess = isLittleEndian != BitConverter.IsLittleEndian; // If system and data endianess is mismatched invert endianess
		}

		public void WriteUInt64(ulong value)
			=> Write(invertEndianess ? BinaryPrimitives.ReverseEndianness(value) : value);

		public void WriteInt64(long value)
			=> Write(invertEndianess ? BinaryPrimitives.ReverseEndianness(value) : value);

		public void WriteUInt32(uint value)
			=> Write(invertEndianess ? BinaryPrimitives.ReverseEndianness(value) : value);

		public void WriteInt32(int value)
			=> Write(invertEndianess ? BinaryPrimitives.ReverseEndianness(value) : value);

		public void WriteUInt16(ushort value)
			=> Write(invertEndianess ? BinaryPrimitives.ReverseEndianness(value) : value);

		public void WriteInt16(short value)
			=> Write(invertEndianess ? BinaryPrimitives.ReverseEndianness(value) : value);

		public void WriteHalf(Half value)
			=> Write(in value);

		public void ReadFloat(float value)
			=> Write(in value);

		public void ReadDouble(double value)
			=> Write(in value);

		public void ReadByte(byte value)
			=> Write(in value);

		public void ReadChar(char value)
			=> Write(in value);

		public void ReadSByte(sbyte value)
			=> Write(in value);

		public int Write(scoped ReadOnlySpan<byte> buffer)
		{
			if (buffer.Length > Remaining)
			{
				buffer.Slice(0, Remaining).TryCopyTo(data.Slice(idx));

				idx += Remaining;
				return Remaining;
			}
			else
			{
				buffer.TryCopyTo(data.Slice(idx, buffer.Length));

				idx += buffer.Length;
				return buffer.Length;
			}
		}

		unsafe void Write<T>(ref readonly T item) where T : unmanaged
		{
			idx += sizeof(T);
			MemoryMarshal.Write(data, in item);
		}

		public static implicit operator SpanWriter(Span<byte> span) => new(span);
	}
}

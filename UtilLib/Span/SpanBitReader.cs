namespace UtilLib.Span
{
	public ref struct SpanBitReader
	{
		public int Remaining => data.Length * 8 - idx;

		ReadOnlySpan<byte> data;
		int idx = 0;

		public SpanBitReader(ReadOnlySpan<byte> data)
		{
			this.data = data;	
		}

		public bool ReadBit()
		{
			return (data[idx / 8] & (128 >> (idx++ & 7))) != 0;
		}

		public int ReadBits(int bits)
		{
			int value = 0;
			for (int i = 0; i < bits; i++)
			{
				//value |= ((ReadBit() ? 1 : 0) << (7 - i));
				value = value * 2 + (ReadBit() ? 1 : 0);
			}

			return value;
		}

		/*
		public byte ReadBits(int bits)
		{
			int offset = idx % 8;
			int remaining = 8 - offset;

			idx += bits;

			int byteIdx = idx >> 3;

			if (remaining > bits)
				return (byte)((data[byteIdx] & (((1 << bits) - 1) << offset)) >> offset);
			else if (remaining == bits)
				return (byte)((data[byteIdx - 1] & (((1 << bits) - 1) << offset)) >> offset);

			byte first = (byte)(((1 << remaining) - 1) << offset);
			byte last = (byte)((1 << (bits - remaining)) - 1);

			byte b = (byte)((data[byteIdx - 1] & first) >> offset);
			b |= (byte)((data[byteIdx] & last) << remaining);

			return b;
		}
		*/

		public static implicit operator SpanBitReader(Span<byte> span) => new(span);
		public static implicit operator SpanBitReader(ReadOnlySpan<byte> span) => new(span);
	}
}

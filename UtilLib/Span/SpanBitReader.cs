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
			return (data[idx / 8] & (1 << (idx++ & 7))) != 0;
		}

		public static implicit operator SpanBitReader(Span<byte> span) => new(span);
		public static implicit operator SpanBitReader(ReadOnlySpan<byte> span) => new(span);
	}
}

namespace UtilLib.Span
{
	public ref struct SpanRingBuffer<T>
	{
		public int Length => span.Length;

		public ref T this[int idx] => ref span[(idx + span.Length) % span.Length];

		Span<T> span;

		public SpanRingBuffer(Span<T> span)
		{
			this.span = span;
		}

		public void Clear()
		{
			span.Clear();
		}

		public static implicit operator SpanRingBuffer<T>(Span<T> span) => new(span);
	}

	public readonly ref struct ReadOnlySpanRingBuffer<T>
	{
		public int Length => span.Length;

		public ref readonly T this[int idx] => ref span[(idx + span.Length) % span.Length];

		readonly ReadOnlySpan<T> span;

		public ReadOnlySpanRingBuffer(ReadOnlySpan<T> span)
		{
			this.span = span;
		}

		public static implicit operator ReadOnlySpanRingBuffer<T>(ReadOnlySpan<T> span) => new(span);
	}
}

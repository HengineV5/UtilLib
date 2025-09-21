namespace UtilLib.Span
{
	public ref struct SegmentedSpan<T>
    {
        public Span<T> this[int idx] => span.Slice(segments[idx], segments[idx + 1] - segments[idx]);

        public readonly int Length => segments.Length;

        Span<T> span;
        ReadOnlySpan<int> segments;

        public SegmentedSpan(Span<T> span, ReadOnlySpan<int> segments)
		{
			this.span = span;
			this.segments = segments;
		}

		public SegmentedSpan<T> Slice(int start, int length) => new(span.Slice(segments[start], segments[start + length] - segments[start]), segments.Slice(start, length));

        public void Clear() => span.Clear();
    }

	public ref struct SegmentedReadOnlySpan<T>
    {
        public ReadOnlySpan<T> this[int idx] => span.Slice(segments[idx], segments[idx + 1] - segments[idx]);

        public readonly int Length => segments.Length;

        ReadOnlySpan<T> span;
        ReadOnlySpan<int> segments;

        public SegmentedReadOnlySpan(ReadOnlySpan<T> span, ReadOnlySpan<int> segments)
		{
			this.span = span;
			this.segments = segments;
		}

		public SegmentedReadOnlySpan<T> Slice(int start, int length) => new(span.Slice(segments[start], segments[start + length] - segments[start]), segments.Slice(start, length));
    }
}

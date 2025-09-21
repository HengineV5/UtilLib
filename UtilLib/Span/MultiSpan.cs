namespace UtilLib.Span
{
	public ref struct MultiSpan<T>
	{
        public Span<T> this[int idx] => span.Slice(step * idx, step);

        public readonly int Length => span.Length / step;

        Span<T> span;
		int step;

        public MultiSpan(Span<T> span, int step)
		{
			this.span = span;
			this.step = step;
        }

		public MultiSpan<T> Slice(int start, int length) => new(span.Slice(start * step, length * step), step);

		public void Clear() => span.Clear();
    }

	public ref struct MultiReadOnlySpan<T>
	{
        public ReadOnlySpan<T> this[int idx] => span.Slice(step * idx, step);

        public readonly int Length => span.Length / step;

        ReadOnlySpan<T> span;
		int step;

        public MultiReadOnlySpan(ReadOnlySpan<T> span, int step)
		{
			this.span = span;
			this.step = step;
        }

		public MultiReadOnlySpan<T> Slice(int start, int length) => new(span.Slice(start * step, length * step), step);
    }
}

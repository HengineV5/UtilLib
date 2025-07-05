using System;
using System.Runtime.CompilerServices;

namespace UtilLib.Span
{
	public ref struct SpanList<T>
	{
		public ref T this[int idx] => ref span[idx];

		public readonly int Count => idx;

		public readonly bool IsFull => idx >= span.Length;

		Span<T> span;
		int idx = 0;

		public SpanList(Span<T> span)
		{
			this.span = span;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Add(scoped ReadOnlySpan<T> span)
		{
			span.CopyTo(this.span.Slice(idx));
			idx += span.Length;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Add(in T item)
		{
			this.span[idx] = item;
			idx++;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<T> Reserve(int count)
		{
			int start = idx;
			idx += count;
			return span.Slice(start, count);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Remove(int idx)
		{
			span.Slice(idx + 1).TryCopyTo(span.Slice(idx));
			this.idx--;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Swap(int i1, int i2)
		{
			if (i1 >= Count || i2 >= Count)
				throw new IndexOutOfRangeException();

			(span[i2], span[i1]) = (span[i1], span[i2]);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Sort(Comparison<T> comparison)
		{
			span.Slice(0, Count).Sort(comparison);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Clear()
		{
			idx = 0;
			span.Clear();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Reverse()
			=> span.Slice(0, Count).Reverse();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ReadOnlySpan<T> AsSpan()
			=> span.Slice(0, Count);

		public static implicit operator SpanList<T>(Span<T> span) => new(span);
	}
}

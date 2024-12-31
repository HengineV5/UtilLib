using System.Runtime.CompilerServices;

namespace UtilLib.Span
{
	unsafe ref struct SpanStack
	{
		Span<byte> pool;
		int idx = 0;

		public SpanStack(Span<byte> pool)
		{
			this.pool = pool;
		}

		public ref T AllocSingle<T>() where T : unmanaged
		{
			idx += sizeof(T);
			return ref Unsafe.As<byte, T>(ref pool[idx - sizeof(T)]);
		}

		public Span<T> Alloc<T>(int count) where T : unmanaged
		{
			idx += count * sizeof(T);
			return new Span<T>(Unsafe.AsPointer(ref pool[idx - count * sizeof(T)]), count);
		}

		public static implicit operator SpanStack(Span<byte> span) => new(span);
	}
}

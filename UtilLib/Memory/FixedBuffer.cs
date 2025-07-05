using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace UtilLib.Memory
{
	[InlineArray(2)]
	public struct FixedBuffer2<T>
	{
		private T _element0;

		[EditorBrowsable(EditorBrowsableState.Never)]
		public ref T GetPinnableReference()
		{
			return ref Unsafe.AsRef(ref _element0);
		}
	}

	[InlineArray(4)]
	public struct FixedBuffer4<T>
	{
		private T _element0;

		[EditorBrowsable(EditorBrowsableState.Never)]
		public ref T GetPinnableReference()
		{
			return ref Unsafe.AsRef(ref _element0);
		}
	}

	[InlineArray(8)]
	public struct FixedBuffer8<T>
	{
		private T _element0;

		[EditorBrowsable(EditorBrowsableState.Never)]
		public ref T GetPinnableReference()
		{
			return ref Unsafe.AsRef(ref _element0);
		}
	}

	[InlineArray(16)]
	public struct FixedBuffer16<T>
	{
		private T _element0;

		[EditorBrowsable(EditorBrowsableState.Never)]
		public ref T GetPinnableReference()
		{
			return ref Unsafe.AsRef(ref _element0);
		}
	}

	[InlineArray(32)]
	public struct FixedBuffer32<T>
	{
		private T _element0;

		[EditorBrowsable(EditorBrowsableState.Never)]
		public ref T GetPinnableReference()
		{
			return ref Unsafe.AsRef(ref _element0);
		}
	}

	[InlineArray(64)]
	public struct FixedBuffer64<T>
	{
		private T _element0;

		[EditorBrowsable(EditorBrowsableState.Never)]
		public ref T GetPinnableReference()
		{
			return ref Unsafe.AsRef(ref _element0);
		}
	}

	public static class FixedBufferExtensions
	{
		public static Span<T> AsSpan<T>(this FixedBuffer2<T> buffer) where T : unmanaged
		{
			return MemoryMarshal.CreateSpan(ref buffer.GetPinnableReference(), 2);
		}

		public static ReadOnlySpan<T> AsReadOnlySpan<T>(this FixedBuffer2<T> buffer) where T : unmanaged
		{
			return MemoryMarshal.CreateReadOnlySpan(ref buffer.GetPinnableReference(), 2);
		}

		public static Span<T> AsSpan<T>(this FixedBuffer4<T> buffer) where T : unmanaged
		{
			return MemoryMarshal.CreateSpan(ref buffer.GetPinnableReference(), 4);
		}

		public static ReadOnlySpan<T> AsReadOnlySpan<T>(this FixedBuffer4<T> buffer) where T : unmanaged
		{
			return MemoryMarshal.CreateReadOnlySpan(ref buffer.GetPinnableReference(), 4);
		}

		public static Span<T> AsSpan<T>(this FixedBuffer8<T> buffer) where T : unmanaged
		{
			return MemoryMarshal.CreateSpan(ref buffer.GetPinnableReference(), 8);
		}

		public static ReadOnlySpan<T> AsReadOnlySpan<T>(this FixedBuffer8<T> buffer) where T : unmanaged
		{
			return MemoryMarshal.CreateReadOnlySpan(ref buffer.GetPinnableReference(), 8);
		}

		public static Span<T> AsSpan<T>(this FixedBuffer16<T> buffer) where T : unmanaged
		{
			return MemoryMarshal.CreateSpan(ref buffer.GetPinnableReference(), 16);
		}

		public static ReadOnlySpan<T> AsReadOnlySpan<T>(this FixedBuffer16<T> buffer) where T : unmanaged
		{
			return MemoryMarshal.CreateReadOnlySpan(ref buffer.GetPinnableReference(), 16);
		}

		public static Span<T> AsSpan<T>(this FixedBuffer32<T> buffer) where T : unmanaged
		{
			return MemoryMarshal.CreateSpan(ref buffer.GetPinnableReference(), 32);
		}

		public static ReadOnlySpan<T> AsReadOnlySpan<T>(this FixedBuffer32<T> buffer) where T : unmanaged
		{
			return MemoryMarshal.CreateReadOnlySpan(ref buffer.GetPinnableReference(), 32);
		}

		public static Span<T> AsSpan<T>(this FixedBuffer64<T> buffer) where T : unmanaged
		{
			return MemoryMarshal.CreateSpan(ref buffer.GetPinnableReference(), 64);
		}

		public static ReadOnlySpan<T> AsReadOnlySpan<T>(this FixedBuffer64<T> buffer) where T : unmanaged
		{
			return MemoryMarshal.CreateReadOnlySpan(ref buffer.GetPinnableReference(), 64);
		}
	}
}

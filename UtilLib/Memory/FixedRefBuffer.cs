using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace UtilLib.Memory
{
	[InlineArray(2)]
	public ref struct FixedRefBuffer2<T> where T : allows ref struct
	{
		public const int Length = 2;

		private T _element0;

		[EditorBrowsable(EditorBrowsableState.Never)]
		public ref T GetPinnableReference()
		{
			return ref Unsafe.AsRef(ref _element0);
		}

		[UnscopedRef]
		public ref T this[int index]
		{
			get => ref Unsafe.Add(ref _element0, index);
		}

		[UnscopedRef]
		public ref T Get(int index)
		{
			return ref Unsafe.Add(ref _element0, index);
		}
	}

	[InlineArray(4)]
	public ref struct FixedRefBuffer4<T> where T : allows ref struct
	{
		public const int Length = 4;

		private T _element0;

		[EditorBrowsable(EditorBrowsableState.Never)]
		public ref T GetPinnableReference()
		{
			return ref Unsafe.AsRef(ref _element0);
		}

		[UnscopedRef]
		public ref T this[int index]
		{
			get => ref Unsafe.Add(ref _element0, index);
		}

		[UnscopedRef]
		public ref T Get(int index)
		{
			return ref Unsafe.Add(ref _element0, index);
		}
	}

	[InlineArray(8)]
	public ref struct FixedRefBuffer8<T> where T : allows ref struct
	{
		public const int Length = 8;
		private T _element0;
		[EditorBrowsable(EditorBrowsableState.Never)]
		public ref T GetPinnableReference()
		{
			return ref Unsafe.AsRef(ref _element0);
		}

		[UnscopedRef]
		public ref T this[int index]
		{
			get => ref Unsafe.Add(ref _element0, index);
		}

		[UnscopedRef]
		public ref T Get(int index)
		{
			return ref Unsafe.Add(ref _element0, index);
		}
	}

	[InlineArray(16)]
	public ref struct FixedRefBuffer16<T> where T : allows ref struct
	{
		public const int Length = 16;
		private T _element0;
		[EditorBrowsable(EditorBrowsableState.Never)]
		public ref T GetPinnableReference()
		{
			return ref Unsafe.AsRef(ref _element0);
		}

		[UnscopedRef]
		public ref T this[int index]
		{
			get => ref Unsafe.Add(ref _element0, index);
		}

		[UnscopedRef]
		public ref T Get(int index)
		{
			return ref Unsafe.Add(ref _element0, index);
		}
	}

	[InlineArray(32)]
	public ref struct FixedRefBuffer32<T> where T : allows ref struct
	{
		public const int Length = 32;
		private T _element0;
		[EditorBrowsable(EditorBrowsableState.Never)]
		public ref T GetPinnableReference()
		{
			return ref Unsafe.AsRef(ref _element0);
		}

		[UnscopedRef]
		public ref T Get(int index)
		{
			return ref Unsafe.Add(ref _element0, index);
		}
	}

	[InlineArray(64)]
	public ref struct FixedRefBuffer64<T> where T : allows ref struct
	{
		public const int Length = 64;
		private T _element0;
		[EditorBrowsable(EditorBrowsableState.Never)]
		public ref T GetPinnableReference()
		{
			return ref Unsafe.AsRef(ref _element0);
		}

		[UnscopedRef]
		public ref T this[int index]
		{
			get => ref Unsafe.Add(ref _element0, index);
		}

		[UnscopedRef]
		public ref T Get(int index)
		{
			return ref Unsafe.Add(ref _element0, index);
		}
	}
}

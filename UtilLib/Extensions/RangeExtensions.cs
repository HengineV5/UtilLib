using System.Runtime.CompilerServices;

namespace UtilLib.Extensions
{
	public static class RangeExtensions
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int Length(this Range range)
		{
			return range.End.Value - range.Start.Value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Shift(this ref Range range, int shiftAmount)
		{
			range = new(range.Start.Value + shiftAmount, range.End.Value + shiftAmount);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Elongate(this ref Range range, int amount)
		{
			range = new(range.Start, range.End.Value + amount);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int At(this Range range, int amount)
		{
			return range.Start.Value + amount;
		}
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Contains(this Range range, Range other)
		{
			return range.Start.Value <= other.Start.Value && range.End.Value >= other.End.Value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Overlaps(this Range range, Range other)
		{
			return range.Start.Value < other.End.Value && other.Start.Value < range.End.Value;
		}
	}
}

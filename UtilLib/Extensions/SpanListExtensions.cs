using System.Runtime.CompilerServices;
using UtilLib.Span;

namespace UtilLib.Extensions
{
	public static class SpanListExtensions
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void RemoveElement<T>(this ref SpanList<T> list, ref readonly T item) where T : IEquatable<T>
		{
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].Equals(item))
				{
					list.Remove(i);
					return;
				}
			}
		}
	}
}

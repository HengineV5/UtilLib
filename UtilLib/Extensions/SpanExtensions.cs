using System;
using System.Collections.Generic;
using System.Text;

namespace UtilLib.Extensions
{
	public delegate void SpanAction<T>(ref T item);

	public static class SpanExtensions
	{
		public static void ForEach<T>(this ref Span<T> span, SpanAction<T> action)
		{
			for (int i = 0; i < span.Length; i++)
			{
				action(ref span[i]);
			}
		}
	}
}

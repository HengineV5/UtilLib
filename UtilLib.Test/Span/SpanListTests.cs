using UtilLib.Span;

namespace UtilLib.Test
{
	public class SpanListTests
	{
		[Test]
		public void SpanListTest()
		{
			SpanList<int> ints = stackalloc int[4];

			ints.Add(1);
			ints.Add(3);

			Assert.That(ints[0] == 1);
			Assert.That(ints[1] == 3);
			Assert.That(ints.Count == 2);
			Assert.That(ints.IsFull == false);
			Assert.That(ints.AsSpan().Length == ints.Count);

			ints.Add(2);
			ints.Add(4);

			ints.Swap(0, 3);

			Assert.That(ints[0] == 4);
			Assert.That(ints[3] == 1);
			Assert.That(ints.Count == 4);
			Assert.That(ints.IsFull == true);
			Assert.That(ints.AsSpan().Length == ints.Count);

			ints.Reverse();
			Assert.That(ints[3] == 4);
			Assert.That(ints[0] == 1);

			Assert.That(ints[2] == 3);
			ints.Remove(2);
			Assert.That(ints[2] == 4);

			ints.Clear();

			Assert.That(ints.Count == 0);
			Assert.That(ints.IsFull == false);

			var span = ints.Reserve(4);
			span[0] = 6;

			Assert.That(ints[0] == 6);
			Assert.That(ints.Count == 4);
			Assert.That(ints.IsFull == true);
		}
	}
}

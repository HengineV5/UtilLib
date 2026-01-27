using UtilLib.Span;
using UtilLib.Extensions;

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
	public class SpanHierarchyTests
	{
		[Test]
		public void SpanHierarchyTest()
		{
			SpanHierarchy<int> hierarchy = new(stackalloc int[10], stackalloc SpanHierarchy<int>.Node[10], 0);

			int rootIndex = hierarchy.SetRoot(1);
			Assert.That(rootIndex == 0);
			Assert.That(hierarchy.GetChildren(rootIndex).Length() == 0);

			int child1Index = hierarchy.CreateChild(rootIndex, 2);
			Assert.That(child1Index == 1);
			Assert.That(hierarchy.GetChildren(rootIndex).Length() == 1);
			Assert.That(hierarchy.GetChildrenValues(rootIndex)[0] == 2);
			Assert.That(hierarchy.GetParent(child1Index) == 0);

			int child2Index = hierarchy.CreateChild(rootIndex, 3);
			Assert.That(child2Index == 2);
			Assert.That(hierarchy.GetChildren(rootIndex).Length() == 2);
			Assert.That(hierarchy.GetChildrenValues(rootIndex)[0] == 2);
			Assert.That(hierarchy.GetChildrenValues(rootIndex)[1] == 3);
			Assert.That(hierarchy.GetParent(child2Index) == 0);

			int grandChildIndex = hierarchy.CreateChild(child1Index, 4);
			Assert.That(grandChildIndex == 3);
			Assert.That(hierarchy.GetChildren(child1Index).Length() == 1);
			Assert.That(hierarchy.GetChildrenValues(child1Index)[0] == 4);
			Assert.That(hierarchy.GetParent(grandChildIndex) == 1);

			int child3Index = hierarchy.CreateChild(rootIndex, 5);
			Assert.That(child3Index == 3);
			Assert.That(hierarchy.GetChildren(rootIndex).Length() == 3);
			Assert.That(hierarchy.GetChildrenValues(rootIndex)[0] == 2);
			Assert.That(hierarchy.GetChildrenValues(rootIndex)[1] == 3);
			Assert.That(hierarchy.GetChildrenValues(rootIndex)[2] == 5);
			Assert.That(hierarchy.GetParent(child3Index) == 0);

			// Repeat to ensure child 1 grandchildren is retained
			Assert.That(hierarchy.GetChildren(child1Index).Length() == 1);
			Assert.That(hierarchy.GetChildrenValues(child1Index)[0] == 4);
			Assert.That(hierarchy.GetParent(grandChildIndex + 1) == 1);

			int grandChild2Index = hierarchy.CreateChild(child1Index, 6);
			Assert.That(grandChild2Index == 5);
			Assert.That(hierarchy.GetChildren(child1Index).Length() == 2);
			Assert.That(hierarchy.GetChildrenValues(child1Index)[0] == 4);
			Assert.That(hierarchy.GetChildrenValues(child1Index)[1] == 6);
			Assert.That(hierarchy.GetParent(grandChild2Index) == 1);

			int grandChild3Index = hierarchy.CreateChild(child2Index, 7);
			Assert.That(grandChild3Index == 6);
			Assert.That(hierarchy.GetChildren(child2Index).Length() == 1);
			Assert.That(hierarchy.GetChildrenValues(child2Index)[0] == 7);
			Assert.That(hierarchy.GetParent(grandChild3Index) == 2);
		}

		[Test]
		public void SpanHierarchyDeleteTest()
		{
			SpanHierarchy<int> hierarchy = new(stackalloc int[10], stackalloc SpanHierarchy<int>.Node[10], 0);

			int rootIndex = hierarchy.SetRoot(1);
			int child1Index = hierarchy.CreateChild(rootIndex, 2);
			int child2Index = hierarchy.CreateChild(rootIndex, 3);
			int grandChildIndex = hierarchy.CreateChild(child1Index, 4);
			int child3Index = hierarchy.CreateChild(rootIndex, 5);
			int grandChild2Index = hierarchy.CreateChild(child1Index, 6);
			int grandChild3Index = hierarchy.CreateChild(child2Index, 7);

			hierarchy.DeleteNode(child1Index);

			Assert.That(hierarchy.GetChildren(rootIndex).Length() == 2);
			Assert.That(hierarchy.GetChildrenValues(rootIndex)[0] == 3);
			Assert.That(hierarchy.GetChildrenValues(rootIndex)[1] == 5);
			Assert.That(hierarchy.GetParent(child2Index - 1) == 0);
			Assert.That(hierarchy.GetChildren(child2Index - 1).Length() == 1);

			hierarchy.DeleteNode(grandChild3Index - 3);

			Assert.That(hierarchy.GetChildren(child2Index - 1).Length() == 0);

			hierarchy.DeleteNode(rootIndex);
			Assert.That(hierarchy.GetChildren(0).Length() == 0);
		}

		[Test]
		public void SpanHierarchyNavigationTest()
		{
			SpanHierarchy<int> hierarchy = new(stackalloc int[10], stackalloc SpanHierarchy<int>.Node[10], 0);

			int rootIndex = hierarchy.SetRoot(1);
			int child1Index = hierarchy.CreateChild(rootIndex, 2);
			int child2Index = hierarchy.CreateChild(rootIndex, 3);
			int grandChildIndex = hierarchy.CreateChild(child1Index, 4);
			int child3Index = hierarchy.CreateChild(rootIndex, 5);
			int grandChild2Index = hierarchy.CreateChild(child1Index, 6);
			int grandChild3Index = hierarchy.CreateChild(child2Index, 7);

			var rootChildren = hierarchy.GetChildren(rootIndex);
			for (int i = rootChildren.Start.Value; i < rootChildren.End.Value; i++)
			{
				Assert.That(hierarchy.GetParent(i) == rootIndex);

				var childChildren = hierarchy.GetChildren(i);
				for (int j = childChildren.Start.Value; j < childChildren.End.Value; j++)
				{
					Assert.That(hierarchy.GetParent(j) == i);
				}
			}

			Assert.That(hierarchy.GetNodeData(hierarchy.GetChildren(hierarchy.GetChildren(rootIndex).At(0)).At(1)) == 6);
		}
	}
}

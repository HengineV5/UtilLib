using UtilLib.Extensions;
using Node = UtilLib.Span.SpanHierarchy.Node;

namespace UtilLib.Span
{
	public struct SpanHierarchy
	{
		public struct Node
		{
			public int parentIndex;
			public Range children;
		}
	}

	public ref struct SpanHierarchy<T>
	{
		Span<T> data;
		Span<Node> hierarchy;
		ref int count;

		public SpanHierarchy(Span<T> data, Span<Node> nodes, ref int count)
		{
			if (data.Length != nodes.Length)
				throw new ArgumentException("Data and nodes must have the same length");

			this.data = data;
			this.hierarchy = nodes;
			this.count = ref count;
		}

		public int SetRoot(in T value)
		{
			hierarchy[0] = new Node
			{
				parentIndex = -1,
				children = new(0, 0)
			};

			data[0] = value;
			count++;

			return 0;
		}

		public int GetRoot()
		{
			return 0;
		}

		public int CreateChild(int parentIndex, in T value)
		{
			ref var parentNode = ref hierarchy[parentIndex];

			int childIndex;
			if (parentNode.children.Length() == 0)
			{
				childIndex = count;
				parentNode.children = new(childIndex, childIndex);
			}
			else
			{
				childIndex = parentNode.children.End.Value;
				ShiftNodes(childIndex, 1);
			}

			hierarchy[childIndex] = new Node
			{
				parentIndex = parentIndex,
				children = new(parentIndex, parentIndex)
			};

			data[childIndex] = value;
			parentNode.children.Elongate(1);

			count++;

			return childIndex;
		}

		public int CreateOrphan(in T value)
		{
			int nodeIndex = count;
			hierarchy[nodeIndex] = new Node
			{
				parentIndex = -1,
				children = new(nodeIndex, nodeIndex)
			};
			data[nodeIndex] = value;
			count++;
			return nodeIndex;
		}

		public void DeleteNode(int nodeIndex)
		{
			ref var node = ref hierarchy[nodeIndex];

			for (int i = 0; i < node.children.Length(); i++)
			{
				ref var childNode = ref hierarchy[node.children.Start.Value];
				childNode.parentIndex = -1;

				DeleteNode(node.children.Start.Value);
			}

			int parentIndex = node.parentIndex;
			if (parentIndex != -1)
			{
				ref var parentNode = ref hierarchy[parentIndex];

				parentNode.children.Elongate(-1);
			}

			ShiftNodes(nodeIndex + 1, -1);

			count--;
		}

		public ref T GetNodeData(int nodeIndex)
		{
			return ref data[nodeIndex];
		}

		public Range GetChildren(int parentIndex)
		{
			return hierarchy[parentIndex].children;
		}

		public Range GetChildrenAndSelf(int parentIndex)
		{
			ref var parentNode = ref hierarchy[parentIndex];
			return new Range(parentIndex, parentNode.children.End.Value);
		}

		public Span<T> GetChildrenValues(int parentIndex)
		{
			ref var parentNode = ref hierarchy[parentIndex];
			return data.Slice(parentNode.children.Start.Value, parentNode.children.Length());
		}

		public int GetParent(int childIndex)
		{
			return hierarchy[childIndex].parentIndex;
		}

		void ShiftNodes(int startIndex, int shiftAmount)
		{
			for (int i = 0; i < startIndex; i++)
			{
				if (hierarchy[i].children.Start.Value >= startIndex)
					hierarchy[i].children.Shift(shiftAmount);

				if (hierarchy[i].parentIndex >= startIndex)
					hierarchy[i].parentIndex += shiftAmount;
			}

			for (int i = startIndex; i < hierarchy.Length; i++)
			{
				if (hierarchy[i].parentIndex >= startIndex)
					hierarchy[i].parentIndex += shiftAmount;
			}

			hierarchy.Slice(startIndex, hierarchy.Length - startIndex - int.Max(shiftAmount, 0)).CopyTo(hierarchy.Slice(startIndex + shiftAmount));
			data.Slice(startIndex, data.Length - startIndex - int.Max(shiftAmount, 0)).CopyTo(data.Slice(startIndex + shiftAmount));
		}
	}
}

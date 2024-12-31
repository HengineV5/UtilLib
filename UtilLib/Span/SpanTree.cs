using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace UtilLib.Span
{
	public ref struct SpanTree<T>
	{
		public ref readonly Node this[int idx] => ref nodes[idx];

		public int Nodes => idx;

		public struct Node
		{
			public T value;
			public int right;
			public int left;

			public Node()
			{
				value = default;
				right = -1;
				left = -1;
			}
		}

		Span<Node> nodes;
		int idx = 0;
		int root = 0;

		public SpanTree(Span<Node> nodes)
		{
			this.nodes = nodes;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ref Node CreateNode(out int idx)
		{
			nodes[this.idx] = new();

			idx = this.idx;
			return ref nodes[this.idx++];
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ref Node CreateNode()
		{
			nodes[this.idx] = new();

			return ref nodes[this.idx++];
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ref Node GetNode(int idx)
		{
			return ref nodes[idx];
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GetRoot()
		{
			return root;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetRoot(int idx)
		{
			this.root = idx;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void InsertRight(int idx, int right)
		{
			nodes[right].right = nodes[idx].right;
			nodes[idx].right = right;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void InsertLeft(int idx, int left)
		{
			nodes[left].left = nodes[idx].left;
			nodes[idx].left = left;
		}

		public static implicit operator SpanTree<T>(Span<Node> span) => new(span);
	}

	public static class SpanTreeExtensions
	{
		public static void PrintTreeValues<T>(this ref SpanTree<T> tree)
		{
			Console.Write('|');

			for (int i = 0; i < tree.Nodes; i++)
			{
				Console.BackgroundColor = GetBackgroundColor(i);
				Console.ForegroundColor = GetForegroundColor(i);
				Console.Write($"{i}");
			}

			Console.BackgroundColor = ConsoleColor.Black;
			Console.ForegroundColor = ConsoleColor.White;
			Console.Write('|');
			Console.WriteLine();
			Console.Write('|');

			for (int i = 0; i < tree.Nodes; i++)
			{
				Console.BackgroundColor = GetBackgroundColor(i);
				Console.ForegroundColor = GetForegroundColor(i);
				Console.Write($"{tree[i].value}");
			}

			Console.BackgroundColor = ConsoleColor.Black;
			Console.ForegroundColor = ConsoleColor.White;
			Console.Write('|');
			Console.WriteLine();
		}

		public static void PrintTree<T>(this ref SpanTree<T> tree)
		{
			var root = tree.GetRoot();
			List<int> visited = new();

			PrintTree(ref tree, root, x => false, x => $"I: {x.Item1} L: {x.Item2.left} R: {x.Item2.right} V: {x.Item2.value}", 0, visited);
		}

		public static void PrintTree<T>(this ref SpanTree<T> tree, Func<SpanTree<T>.Node, bool> nodeFilter, Func<(int, SpanTree<T>.Node), string> nodeFormat)
		{
			var root = tree.GetRoot();
			List<int> visited = new();

			PrintTree(ref tree, root, nodeFilter, nodeFormat, 0, visited);
		}

		static void PrintTree<T>(this ref SpanTree<T> tree, int node, Func<SpanTree<T>.Node, bool> nodeFilter, Func<(int ,SpanTree<T>.Node), string> nodeFormat, int indent, List<int> visited)
		{
			if (visited.Contains(node))
				throw new Exception("Tree contains loop");

			visited.Add(node);

			ref var nodeRef = ref tree.GetNode(node);

			if (!nodeFilter(nodeRef))
			{
				var line = $"{new string('│', int.Max(indent - 1, 0))}{(indent == 0 ? "" : "├")}{nodeFormat((node, nodeRef))}";
				Console.WriteLine(line);
			}

			if (nodeRef.right != -1)
				PrintTree(ref tree, nodeRef.right, nodeFilter, nodeFormat, indent + 1, visited);

			if (nodeRef.left != -1)
				PrintTree(ref tree, nodeRef.left, nodeFilter, nodeFormat, indent + 1, visited);
		}

		static ConsoleColor GetForegroundColor(int idx)
		{
			switch (idx % 2)
			{
				case 0:
					return ConsoleColor.White;
				case 1:
					return ConsoleColor.Black;
				default:
					return ConsoleColor.Black;
			}
		}

		static ConsoleColor GetBackgroundColor(int idx)
		{
			switch (idx % 2)
			{
				case 0:
					return ConsoleColor.Black;
				case 1:
					return ConsoleColor.White;
				default:
					return ConsoleColor.Black;
			}
		}
	}
}

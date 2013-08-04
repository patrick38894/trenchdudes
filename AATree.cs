using System;

public class AATree<TKey, TValue> where TKey : IComparable<TKey> {
	private class Node
	{
		// node internal data
		internal int level;
		internal Node left;
		internal Node right;

		// user data
		internal TKey key;
		internal TValue value;

		// constuctor for the sentinel node
		internal Node() {
			this.level = 0;
			this.left = this;
			this.right = this;
		}

		// constuctor for regular nodes (that all start life as leaf nodes)
		internal Node(TKey key, TValue value, Node sentinel) {
			this.level = 1;
			this.left = sentinel;
			this.right = sentinel;
			this.key = key;
			this.value = value;
		}
	}

	Node root;
	Node sentinel;
	Node deleted;

	public AATree() {
		root = sentinel = new Node();
		deleted = null;
	}

	private void Skew(ref Node node) {
		if (node.level == node.left.level) {
			// rotate right
			Node left = node.left;
			node.left = left.right;
			left.right = node;
			node = left;
		}
	}

	private void Split(ref Node node) {
		if (node.right.right.level == node.level) {
			// rotate left
			Node right = node.right;
			node.right = right.left;
			right.left = node;
			node = right;
			node.level++;
		}
	}

	private bool Insert(ref Node node, TKey key, TValue value) {
		if (node == sentinel) {
			node = new Node(key, value, sentinel);
			return true;
		}

		int compare = key.CompareTo(node.key);
		if (compare < 0) {
			if (!Insert(ref node.left, key, value))
				return false;
		}
		else if (compare > 0) {
			if (!Insert(ref node.right, key, value))
				return false;
		}
		else
			return false;

		Skew(ref node);
		Split(ref node);

		return true;
	}

	private bool Delete(ref Node node, TKey key) {
		if (node == sentinel)
			return (deleted != null);

		int compare = key.CompareTo(node.key);
		if (compare < 0) {
			if (!Delete(ref node.left, key))
				return false;
		}
		else {
			if (compare == 0)
				deleted = node;
			if (!Delete(ref node.right, key))
				return false;
		}

		if (deleted != null) {
			deleted.key = node.key;
			deleted.value = node.value;
			deleted = null;
			node = node.right;
		}
		else if (node.left.level < node.level - 1 || node.right.level < node.level - 1) {
			--node.level;
			if (node.right.level > node.level)
				node.right.level = node.level;
			Skew(ref node);
			Skew(ref node.right);
			Skew(ref node.right.right);
			Split(ref node);
			Split(ref node.right);
		}

		return true;
	}

	private Node Search(Node node, TKey key) {
		if (node == sentinel)
			return null;

		int compare = key.CompareTo(node.key);
		if (compare < 0)
			return Search(node.left, key);
		else if (compare > 0)
			return Search(node.right, key);
		else
			return node;
	}

	public bool Add(TKey key, TValue value) {
		return Insert(ref root, key, value);
	}

	public bool Remove(TKey key) {
		return Delete(ref root, key);
	}
	
	private TKey pop(Node n, ref TValue val) {
		if (n == sentinel) {
			val = default(TValue);
			return default(TKey);
		}
		if (n.left == sentinel) {
			val = n.value;
			return n.key;
		}
		return pop (n.left, ref val);
	}
	
	public TValue pop() {
		TValue val = default(TValue);
		TKey k = pop(root, ref val);
		if (k.CompareTo(default(TKey)) != 0)
			Delete(ref root, k);
		return val;
	}

	public TValue this[TKey key] {
		get {
			Node node = Search(root, key);
			return node == null ? default(TValue) : node.value;
		}
		set {
			Node node = Search(root, key);
			if (node == null)
				Add(key, value);
			else
				node.value = value;
		}
	}
}

/*
class Program
{
	static void Test(int[] values)
	{
		AATree<int, int> tree = new AATree<int, int>();
		for (int i = 0; i < values.Length; i++)
		{
			if (!tree.Add(values[i], (i + 1)))
			{
				Console.WriteLine("Failed to insert {0}", values[i]);
			}
		}
		for (int i = 0; i < values.Length; i++)
		{
			for (int j = 0; j < i; j++)
			{
				if (tree[values[j]] != 0)
				{
					Console.WriteLine("Found deleted key {0}", values[j]);
				}
			}
			for (int j = i; j < values.Length; j++)
			{
				if (tree[values[j]] != (j + 1))
				{
					Console.WriteLine("Could not find key {0}", values[j]);
				}
			}
			if (!tree.Remove(values[i]))
			{
				Console.WriteLine("Failed to delete {0}", values[i]);
			}
		}
	}

	static void Main(string[] args)
	{
		Test(new int[] { 1 });

		Test(new int[] { 1, 2 });
		Test(new int[] { 2, 1 });

		Test(new int[] { 1, 2, 3 });
		Test(new int[] { 2, 1, 3 });
		Test(new int[] { 1, 3, 2 });
		Test(new int[] { 2, 3, 1 });
		Test(new int[] { 3, 1, 2 });
		Test(new int[] { 3, 2, 1 });

		Test(new int[] { 1, 2, 3, 4 });
		Test(new int[] { 2, 1, 3, 4 });
		Test(new int[] { 1, 3, 2, 4 });
		Test(new int[] { 2, 3, 1, 4 });
		Test(new int[] { 3, 1, 2, 4 });
		Test(new int[] { 3, 2, 1, 4 });
		Test(new int[] { 1, 2, 4, 3 });
		Test(new int[] { 2, 1, 4, 3 });
		Test(new int[] { 1, 3, 4, 2 });
		Test(new int[] { 2, 3, 4, 1 });
		Test(new int[] { 3, 1, 4, 2 });
		Test(new int[] { 3, 2, 4, 1 });
		Test(new int[] { 1, 4, 2, 3 });
		Test(new int[] { 2, 4, 1, 3 });
		Test(new int[] { 1, 4, 3, 2 });
		Test(new int[] { 2, 4, 3, 1 });
		Test(new int[] { 3, 4, 1, 2 });
		Test(new int[] { 3, 4, 2, 1 });
		Test(new int[] { 4, 1, 2, 3 });
		Test(new int[] { 4, 2, 1, 3 });
		Test(new int[] { 4, 1, 3, 2 });
		Test(new int[] { 4, 2, 3, 1 });
		Test(new int[] { 4, 3, 1, 2 });
		Test(new int[] { 4, 3, 2, 1 });

		for (int count = 0; count < 1000; count++)
		{
			int[] a = new int[100];
			Random random = new Random();
			for (int i = 0; i < a.Length; i++)
			{
				int r;
				bool dup;
				do
				{
					dup = false;
					r = random.Next();
					for (int j = 0; j < i; j++)
					{
						if (a[j] == r)
						{
							dup = true;
							break;
						}
					}
				}
				while (dup);
				a[i] = r;
			}
			Test(a);
		}
	}
}*/

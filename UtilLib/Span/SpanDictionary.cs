namespace UtilLib.Span
{
	public ref struct SpanDictionary<TKey, TValue>
		where TKey : IEquatable<TKey>
	{
		public ref TValue this[TKey idx]
		{
			get
			{
				for (int i = 0; i < keys.Length; i++)
				{
					if (keys[i].Equals(idx))
					{
						return ref values[i];
					}
				}

				throw new KeyNotFoundException();
			}
		}

		Span<TKey> keys;
		Span<TValue> values;
		int idx = 0;

		public SpanDictionary(Span<TKey> keys, Span<TValue> values)
		{
			if (keys.Length != values.Length)
				throw new ArgumentException("Keys and values must have the same length.");

			this.keys = keys;
			this.values = values;
		}

		public bool ContainsKey(TKey key)
		{
			for (int i = 0; i < keys.Length; i++)
			{
				if (keys[i].Equals(key))
					return true;
			}

			return false;
		}

		public bool TryAdd(TKey key, TValue value)
		{
			if (idx >= keys.Length)
				return false;

			if (ContainsKey(key))
				return false;

			keys[idx] = key;
			values[idx] = value;
			idx++;

			return true;
		}

		public bool TryGetValue(TKey key, ref TValue value)
		{
			if (!ContainsKey(key))
				return false;

			value = this[key];
			return true;
		}
	}
}

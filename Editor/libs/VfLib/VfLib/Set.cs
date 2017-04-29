using System;
using System.Collections.Generic;
using System.Text;

namespace vflibcs
{
	class Set<T> : ICollection<T>
	{
		#region Private Variables
		Dictionary<T, int> _dict = new Dictionary<T, int>();
		#endregion

		#region ICollection<T> Members

		public void Add(T item)
		{
			_dict.Add(item, 0);
		}

		public void Clear()
		{
			_dict.Clear();
		}

		public bool Contains(T item)
		{
			return _dict.ContainsKey(item);
		}

		public void CopyTo(T[] array, int arrayIndex)
		{
			_dict.Keys.CopyTo(array, arrayIndex);
		}

		public int Count
		{
			get { return _dict.Count; }
		}

		public bool IsReadOnly
		{
			get { return false; }
		}

		public bool Remove(T item)
		{
			return _dict.Remove(item);
		}

		#endregion

		#region IEnumerable<T> Members

		public IEnumerator<T> GetEnumerator()
		{
			return _dict.Keys.GetEnumerator();
		}

		#endregion

		#region IEnumerable Members

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return _dict.Keys.GetEnumerator();
		}

		#endregion
	}
}

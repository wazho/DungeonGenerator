using System;
using System.Collections.Generic;
using System.Text;

namespace vflibcs
{
	class SortedListNoKey<T>
	{
		#region Private Variables
		SortedList<T, byte> _srtlst = new SortedList<T, byte>();
		#endregion

		#region Accessors
		public void Add(T item)
		{
			_srtlst.Add(item, 0);
		}

		public void CopyTo(T[] arT)
		{
			_srtlst.Keys.CopyTo(arT, 0);
		}

		public int Count
		{
			get { return _srtlst.Count; }
		}

		public T this[int i]
		{
			get { return _srtlst.Keys[i]; }
		}

		public void Delete(T item)
		{
			_srtlst.Remove(item);
		}
		#endregion
	}
}

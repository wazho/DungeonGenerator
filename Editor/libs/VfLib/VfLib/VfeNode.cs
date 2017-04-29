using System;
using System.Collections.Generic;
using System.Text;

namespace vflibcs
{
	class VfeNode : IEquatable<VfeNode>
	{
		#region Private Variables
		internal int _inodFrom;
		internal int _inodTo;
		internal object _objAttr;
		#endregion

		#region Constructor
		internal VfeNode(int inodFrom, int inodTo, object objAttr)
		{
			_inodFrom = inodFrom;
			_inodTo = inodTo;
			_objAttr = objAttr;
		}
		#endregion

		#region Hashing
		public override int GetHashCode()
		{
			int iTest = _inodTo.GetHashCode();
			return ((_inodFrom << 16) + _inodTo).GetHashCode();
		}

		public bool Equals(VfeNode other)
		{
			return (other != null) ? (other._inodFrom.Equals(_inodFrom) && other._inodTo.Equals(_inodTo)) : false;
		}
		#endregion
	}
}

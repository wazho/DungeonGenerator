using System;
using System.Collections.Generic;
using System.Text;

namespace vflibcs
{
	class Match
	{
		#region Private Variables
		int _inod1;
		int _inod2;
		#endregion

		#region Properties
		internal int Inod1
		{
			get { return _inod1; }
			set { _inod1 = value; }
		}

		internal int Inod2
		{
			get { return _inod2; }
		}
		#endregion

		#region Constructor
		public Match(int inod1, int inod2)
		{
			_inod1 = inod1;
			_inod2 = inod2;
		}
		#endregion
	}
}

using System;
using System.Collections.Generic;
using System.Text;

namespace vflibcs
{
	class CmpNodeDegrees : IComparer<int>
	{
		#region Private Variables
		IGraphLoader _loader;
		#endregion

		#region Constructor
		internal CmpNodeDegrees(IGraphLoader loader)
		{
			_loader = loader;
		}
		#endregion

		#region Permutation
		internal int[] Permutation
		{
			get
			{
				int[] mpInodInodPermuted = new int[_loader.NodeCount];
				IComparer<int> icmpDegree = new CmpNodeDegrees(_loader);

				for (int i = 0; i < _loader.NodeCount; i++)
				{
					mpInodInodPermuted[i] = i;
				}

				Array.Sort(mpInodInodPermuted, icmpDegree);
				return mpInodInodPermuted;
			}
		}
		#endregion

		#region IComparer<vfnNode> Members
		public int Compare(int x, int y)
		{
			if (x == y)
			{
				return 0;
			}
			int nidX = _loader.IdFromPos(x);
			int nidY = _loader.IdFromPos(y);
			int xDegree = _loader.InEdgeCount(nidX) + _loader.OutEdgeCount(nidX);
			int yDegree = _loader.InEdgeCount(nidY) + _loader.OutEdgeCount(nidY);
			return xDegree < yDegree ? 1 : -1;
		}
		#endregion
	}
}

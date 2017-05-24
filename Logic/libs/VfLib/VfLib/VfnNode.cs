using System;
using System.Collections.Generic;
using System.Text;

namespace vflibcs
{
	[Flags]
	enum Groups
	{
		ContainedInMapping = 1,		// Contained in the mapping
		FromMapping = 2,			// Outside the mapping but pointed to from the mapping
		ToMapping = 4,				// Outside the mapping but points to a node in the mapping
		Disconnected = 8			// Outside the mapping with no links to mapped nodes
	}

	class VfnNode
	{
		#region Private Variables
		VfeNode[] _arvfeEdgeOut;
		VfeNode[] _arvfeEdgeIn;
		object _objAttr;
		Groups _grps = Groups.Disconnected;
		#endregion

		#region Constructor
		internal VfnNode(IGraphLoader loader, int inodGraph, Dictionary<VfeNode, VfeNode> dctEdge, int[] mpInodGraphInodVf)
		{
			int nid = loader.IdFromPos(inodGraph);
			_objAttr = loader.GetNodeAttr(nid);
			_arvfeEdgeOut = new VfeNode[loader.OutEdgeCount(nid)];
			_arvfeEdgeIn = new VfeNode[loader.InEdgeCount(nid)];
			MakeEdges(loader, nid, dctEdge, mpInodGraphInodVf);
		}
		#endregion

		#region Properties

		internal Groups Grps
		{
			get { return _grps; }
			set { _grps = value; }
		}


		internal object Attr
		{
			get
			{
				return _objAttr;
			}
		}

		internal int InDegree
		{
			get
			{
				return _arvfeEdgeIn.Length;
			}
		}

		internal int OutDegree
		{
			get
			{
				return _arvfeEdgeOut.Length;
			}
		}

		internal List<int> OutNeighbors
		{
			get
			{
				List<int> lstOut = new List<int>(_arvfeEdgeOut.Length);
				foreach (VfeNode vfe in _arvfeEdgeOut)
				{
					lstOut.Add(vfe._inodTo);
				}
				return lstOut;
			}
		}
		internal List<int> InNeighbors
		{
			get
			{
				List<int> lstIn = new List<int>(_arvfeEdgeIn.Length);
				foreach (VfeNode vfe in _arvfeEdgeIn)
				{
					lstIn.Add(vfe._inodFrom);
				}
				return lstIn;
			}
		}

		internal bool FInMapping
		{
			get { return _grps == Groups.ContainedInMapping; }
		}
		#endregion

		#region Edge Makers
		private void MakeEdges(IGraphLoader loader, int nid, Dictionary<VfeNode, VfeNode> dctEdge, int[] mpInodGraphInodVf)
		{
			int inodGraph = loader.PosFromId(nid);
			int inodVf = mpInodGraphInodVf[inodGraph];
			VfeNode vfeKey = new VfeNode(0, 0, null);

			vfeKey._inodFrom = inodVf;
			MakeOutEdges(loader, nid, dctEdge, mpInodGraphInodVf, ref vfeKey);
			vfeKey._inodTo = inodVf;
			MakeInEdges(loader, nid, dctEdge, mpInodGraphInodVf, ref vfeKey);
		}

		private void MakeOutEdges(IGraphLoader loader, int nid, Dictionary<VfeNode, VfeNode> dctEdge, int[] mpInodGraphInodVf, ref VfeNode vfeKey)
		{
			object attr;
			for (int i = 0; i < loader.OutEdgeCount(nid); i++)
			{
				vfeKey._inodTo = mpInodGraphInodVf[loader.PosFromId(loader.GetOutEdge(nid, i, out attr))];

				if (!dctEdge.ContainsKey(vfeKey))
				{
					_arvfeEdgeOut[i] = dctEdge[vfeKey] = new VfeNode(vfeKey._inodFrom, vfeKey._inodTo, attr);
					vfeKey = new VfeNode(vfeKey._inodFrom, vfeKey._inodTo, null);
				}
				else
				{
					_arvfeEdgeOut[i] = dctEdge[vfeKey];
				}
			}
		}

		private void MakeInEdges(IGraphLoader loader, int nid, Dictionary<VfeNode, VfeNode> dctEdge, int[] mpInodGraphInodVf, ref VfeNode vfeKey)
		{
			object attr;
			for (int i = 0; i < loader.InEdgeCount(nid); i++)
			{
				vfeKey._inodFrom = mpInodGraphInodVf[loader.PosFromId(loader.GetInEdge(nid, i, out attr))];

				if (!dctEdge.ContainsKey(vfeKey))
				{
					_arvfeEdgeIn[i] = dctEdge[vfeKey] = new VfeNode(vfeKey._inodFrom, vfeKey._inodTo, attr);
					vfeKey = new VfeNode(vfeKey._inodFrom, vfeKey._inodTo, null);
				}
				else
				{
					_arvfeEdgeIn[i] = dctEdge[vfeKey];
				}
			}
		}
		#endregion
	}
}

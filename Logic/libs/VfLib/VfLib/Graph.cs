using System;
using System.Collections.Generic;
using System.Text;
#if NUNIT
using NUnit.Framework;
#endif

namespace vflibcs
{
	public class Graph : IGraphLoader
	{
		#region Private variables
		internal SortedList<int, nNode> _nodes = new SortedList<int, nNode>(); // Sorted by node id's
		internal const int _nidIllegal = -1;
		#endregion

		#region Private structs
		internal class eNode
		{
			public int _idFrom = _nidIllegal;
			public int _idTo = _nidIllegal;
			public object _attr = null;
		}

		internal class nNode
		{
			public int _id = _nidIllegal;
			public object _attr = null;
			public SortedList<int, eNode> _edgesFrom = new SortedList<int, eNode>();	// Key is named id of "to" node
			public List<eNode> _edgesTo = new List<eNode>();

			public eNode FindOutEdge(int nidTo)
			{
				try
				{
					return _edgesFrom[nidTo];
				}
				catch (Exception)
				{
					VfException.Error("Inconsistent Data");
				}
				return null;
			}
		}
		#endregion

		#region Properties
		public int NodeCount
		{
			get
			{
				return (int)_nodes.Count;
			}
		}
		#endregion

		#region Constructors
		public Graph()
		{
		}

		static public void Shuffle<T>(T[] arT, Random rnd, out T[] arRet)
		{
			arRet = new T[arT.Length];
			arT.CopyTo(arRet, 0);
			Shuffle<T>(arRet, rnd);
		}

		static public void Shuffle<T>(T[] arT, Random rnd)
		{
			int iSwap;
			T THold;

			for (int i = 0; i < arT.Length - 1; i++)
			{
				iSwap = rnd.Next(arT.Length - i) + i;
				THold = arT[i];
				arT[i] = arT[iSwap];
				arT[iSwap] = THold;
			}
		}

		internal Graph IsomorphicShuffling(Random rnd)
		{
			Graph graph = new Graph();
			int[] ariShuffle = new int[NodeCount];

			for (int i = 0; i < NodeCount; i++)
			{
				ariShuffle[i] = i;
			}
			Shuffle<int>(ariShuffle, rnd);

			graph.InsertNodes(NodeCount);

			for (int inod = 0; inod < NodeCount; inod++)
            {
				int inodShuffled = ariShuffle[inod];
				nNode nod = _nodes[inodShuffled];

				foreach (eNode end in nod._edgesFrom.Values)
				{
					graph.InsertEdge(ariShuffle[PosFromId(end._idFrom)], ariShuffle[PosFromId(end._idTo)]);
				}

            }
			return graph;
		}
		#endregion

		#region Accessors
		internal nNode FindNode(int id)
		{
			int i = _nodes.IndexOfKey(id);
			if (i >= 0)
			{
				return _nodes.Values[i];
			}
			VfException.Error("Inconsistent data");
			return null;
		}
		
		public int IdFromPos(int inod)
		{
			return _nodes.Values[inod]._id;
		}

		public int PosFromId(int nid)
		{
			return _nodes.IndexOfKey(nid);
		}

        public object GetNodeAttr(int id)
		{
        	return FindNode(id)._attr;
        }

		public int InEdgeCount(int id)
		{
			return (int)(FindNode(id)._edgesTo.Count);
		}

		public int OutEdgeCount(int id)
		{
			return (int)(FindNode(id)._edgesFrom.Count);
		}

		public int GetInEdge(int idTo, int pos, out object attr)
		{
			eNode end = null;
			try
			{
				end = FindNode(idTo)._edgesTo[pos];
			}
			catch (Exception)
			{
				VfException.Error("Inconsistent data");
			}

			attr = end._attr;
			return end._idFrom;
		}

		public int GetOutEdge(int idFrom, int pos, out object attr)
		{
			eNode end = null;
			try
			{
				end = FindNode(idFrom)._edgesFrom.Values[pos];
			}
			catch (Exception)
			{
				VfException.Error("Inconsistent data");
			}

			attr = end._attr;
			return end._idTo;
		}
		#endregion

		#region Insertion/Deletion
		public int InsertNode(object attr)
		{
			nNode nod = new nNode();
			nod._id = (int)(_nodes.Count);
			nod._attr = attr;
			_nodes.Add(nod._id, nod);
			return nod._id;
		}

		public int InsertNode()
		{
			return InsertNode(null);
		}

		public int InsertNodes(int cnod, object attr)
		{
			int nid = InsertNode(attr);

			for (int i = 0; i < cnod - 1; i++)
			{
				InsertNode(attr);
			}

			return nid;
		}

		public int InsertNodes(int cnod)
		{
			int nid = InsertNode();

			for (int i = 0; i < cnod - 1; i++)
			{
				InsertNode();
			}
			return nid;
		}

		public void InsertEdge(int nidFrom, int nidTo, object attr)
		{
        	eNode end = new eNode();
			nNode nodFrom = FindNode(nidFrom);
			nNode nodTo = FindNode(nidTo);

			end._idFrom = nidFrom;
			end._idTo = nidTo;
			end._attr = attr;
			try
			{
				nodFrom._edgesFrom.Add(nidTo, end);
				nodTo._edgesTo.Add(end);
			}
			catch (Exception)
			{
				VfException.Error("Inconsistent Data");
			}
        }

		public void InsertEdge(int nidFrom, int nidTo)
		{
			InsertEdge(nidFrom, nidTo, null);
		}

		public void DeleteNode(int nid)
		{
			nNode nod = FindNode(nid);
			eNode[] arend = new eNode[nod._edgesFrom.Count + nod._edgesTo.Count];
			nod._edgesFrom.Values.CopyTo(arend, 0);
			nod._edgesTo.CopyTo(arend, nod._edgesFrom.Count);

			foreach (eNode end in arend)
			{
				DeleteEdge(end._idFrom, end._idTo);
			}
			_nodes.Remove(nod._id);
		}

		public void DeleteEdge(int nidFrom, int nidTo)
		{
			nNode nodFrom = FindNode(nidFrom);
			nNode nodTo = FindNode(nidTo);
			eNode end = nodFrom.FindOutEdge(nidTo);

			nodFrom._edgesFrom.Remove(nidTo);
			nodTo._edgesTo.Remove(end);
		}
		#endregion

		#region NUNIT Testing
#if NUNIT
		[TestFixture]
		public class GraphTester
		{
			[Test]
			public void TestConstructor()
			{
				Graph gr = new Graph();
				Assert.IsNotNull(gr);
			}

			[ExpectedException(typeof(VfException))]
			[Test]
			public void TestFindNodeNotFound()
			{
				Graph gr = new Graph();
				nNode nod = gr.FindNode(0);
			}

			[ExpectedException(typeof(VfException))]
			[Test]
			public void TestInsertNode()
			{
				Graph gr = new Graph();
				gr.InsertNode(1);
				nNode nod = gr.FindNode(0);
				Assert.IsNotNull(nod);
				nod = gr.FindNode(1);
				Assert.IsTrue(false);
			}

			[ExpectedException(typeof(VfException))]
			[Test]
			public void TestInsertEdge()
			{
				object attr;
				Graph gr = new Graph();
				int idFrom = gr.InsertNode(0);
				int idTo = gr.InsertNode(1);
				gr.InsertEdge(idFrom, idTo, 100);
				Assert.AreEqual(gr.OutEdgeCount(idFrom), 1);
				Assert.AreEqual(gr.OutEdgeCount(idTo), 0);
				int idEdge = gr.GetOutEdge(idFrom, 0, out attr);
				Assert.AreEqual(100, (int)attr);
				Assert.AreEqual(idTo, idEdge);

				// Try inserting the same edge twice to trigger exception...
				gr.InsertEdge(0, 1, 200);
			}

			[ExpectedException(typeof(VfException))]
			[Test]
			public void TestDeleteEdge()
			{
				Graph gr = new Graph();
				Assert.AreEqual(0, gr.InsertNode());
				Assert.AreEqual(1, gr.InsertNode());
				Assert.AreEqual(2, gr.InsertNode());
				gr.InsertEdge(0, 1);
				gr.InsertEdge(1, 2);
				gr.InsertEdge(2, 0);
				gr.DeleteEdge(1, 2);
				Assert.AreEqual(1, gr.OutEdgeCount(0));
				Assert.AreEqual(0, gr.OutEdgeCount(1));
				Assert.AreEqual(1, gr.OutEdgeCount(2));

				// Trigger the exception - no edge from 1 to 0...
				gr.DeleteEdge(1, 0);
			}

			[ExpectedException(typeof(VfException))]
			[Test]
			public void TestDeleteNode()
			{
				Graph gr = new Graph();
				Assert.AreEqual(0, gr.InsertNode());
				Assert.AreEqual(1, gr.InsertNode());
				Assert.AreEqual(2, gr.InsertNode());
				gr.InsertEdge(0, 1);
				gr.InsertEdge(1, 2);
				gr.InsertEdge(2, 0);
				gr.DeleteNode(0);

				Assert.AreEqual(1, gr.OutEdgeCount(1));
				Assert.AreEqual(0, gr.OutEdgeCount(2));

				// Trigger the exception - shouldn't be a zero node any more...
				gr.FindNode(0);
			}

			[Test]
			public void TestShuffle()
			{
				Random rnd = new Random(10);
				int count = 100;
				int[] ariShuffle = new int[count];

				for (int i = 0; i < count; i++)
				{
					ariShuffle[i] = i;
				}
				Assert.AreEqual(0, ariShuffle[0]);
				Assert.AreEqual(99, ariShuffle[99]);
				Shuffle<int>(ariShuffle, rnd);
				Assert.AreNotEqual(0, ariShuffle[0]);
				Assert.AreNotEqual(99, ariShuffle[99]);

				int iTotal = 0;
				for (int i = 0; i < count; i++)
				{
					iTotal += ariShuffle[i];
				}
				Assert.AreEqual(count * (count - 1) / 2, iTotal);
			}
		}
#endif
		#endregion
	}
}

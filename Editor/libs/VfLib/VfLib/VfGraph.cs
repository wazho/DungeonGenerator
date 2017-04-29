using System;
using System.Collections.Generic;
using System.Text;
#if NUNIT
using NUnit.Framework;
#endif

namespace vflibcs
{
	class VfGraph
	{
		#region Private Variables
		VfnNode[] _arNodes;
		#endregion

		#region Properties
		internal int NodeCount
		{
			get
			{
				return _arNodes.Length;
			}
		}
		#endregion

		#region Accessors
		internal int OutDegree(int inod)
		{
			return _arNodes[inod].OutDegree;
		}

		internal int InDegree(int inod)
		{
			return _arNodes[inod].InDegree;
		}

		internal int TotalDegree(int inod)
		{
			return OutDegree(inod) + InDegree(inod);
		}

		internal List<int> OutNeighbors(int inod)
		{
			return _arNodes[inod].OutNeighbors;
		}

		internal List<int> InNeighbors(int inod)
		{
			return _arNodes[inod].InNeighbors;
		}

		internal Groups GetGroup(int inod)
		{
			return _arNodes[inod].Grps;
		}

		internal void SetGroup(int inod, Groups grps)
		{
			_arNodes[inod].Grps = grps;
		}

		internal object GetAttr(int inod)
		{
			return _arNodes[inod].Attr;
		}
		#endregion

		#region Constructor
		internal static int[] ReversePermutation(int[] perm)
		{
			int[] permOut = new int[perm.Length];
			for (int i = 0; i < perm.Length; i++)
			{
				permOut[i] = Array.IndexOf<int>(perm, i);
			}
			return permOut;
		}

		internal VfGraph(IGraphLoader loader, int[] mpInodVfInodGraph)
		{
			_arNodes = new VfnNode[loader.NodeCount];
			int[] mpInodGraphInodVf = ReversePermutation(mpInodVfInodGraph);
			Dictionary<VfeNode, VfeNode> dctEdge = new Dictionary<VfeNode, VfeNode>();

			for (int inodVf = 0; inodVf < loader.NodeCount; inodVf++)
			{
				_arNodes[inodVf] = new VfnNode(loader, mpInodVfInodGraph[inodVf], dctEdge, mpInodGraphInodVf);
			}
		}
		#endregion

		#region NUNIT Testing
#if NUNIT
		[TestFixture]
		public class VfGraphTester
		{
			VfGraph SetupGraph()
			{
				Graph graph = new Graph();
				Assert.AreEqual(0, graph.InsertNode());
				Assert.AreEqual(1, graph.InsertNode());
				Assert.AreEqual(2, graph.InsertNode());
				Assert.AreEqual(3, graph.InsertNode());
				Assert.AreEqual(4, graph.InsertNode());
				Assert.AreEqual(5, graph.InsertNode());
				graph.InsertEdge(0, 1);
				graph.InsertEdge(1, 2);
				graph.InsertEdge(2, 3);
				graph.InsertEdge(3, 4);
				graph.InsertEdge(4, 5);
				graph.InsertEdge(5, 0);
				graph.DeleteNode(0);
				graph.DeleteNode(1);
				graph.InsertEdge(5, 2);
				graph.InsertEdge(2, 4);

				return new VfGraph(graph, (new CmpNodeDegrees(graph)).Permutation);
			}

			[Test]
			public void TestPermutations()
			{
				Graph graph = new Graph();
				Assert.AreEqual(0, graph.InsertNode());
				Assert.AreEqual(1, graph.InsertNode());
				Assert.AreEqual(2, graph.InsertNode());
				graph.InsertEdge(1, 0);
				graph.InsertEdge(1, 2);
				int[] mpPermutation = (new CmpNodeDegrees(graph)).Permutation;
				VfGraph vfg = new VfGraph(graph, mpPermutation);
				Assert.AreEqual(mpPermutation[1], 0);
				int[] arOut = new int[vfg._arNodes[0].OutNeighbors.Count];
				vfg._arNodes[0].OutNeighbors.CopyTo(arOut, 0);
				int inodNeighbor1 = arOut[0];
				int inodNeighbor2 = arOut[1];
				Assert.IsTrue(inodNeighbor1 == 1 && inodNeighbor2 == 2 || inodNeighbor1 == 2 && inodNeighbor2 == 1);
			}

			[Test]
			public void TestConstructor()
			{
				Assert.IsNotNull(SetupGraph());
			}

			[Test]
			public void TestNodeCount()
			{
				Assert.AreEqual(4, SetupGraph().NodeCount);
			}
		}
#endif
		#endregion
	}
}

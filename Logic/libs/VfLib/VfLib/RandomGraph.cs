using System;
using System.Collections.Generic;
using System.Text;
#if NUNIT
using NUnit.Framework;
#endif

namespace vflibcs
{
#if NUNIT
	class RandomGraph
	{
		#region Private variables
		double _pctEdges;
		Random _rnd;
        #endregion

		#region Constructor
		// Works best for pctEdges < 0.5
		public RandomGraph(double pctEdges, int iSeed)
		{
			_pctEdges = pctEdges;
			if (iSeed > 0)
			{
				_rnd = new Random(iSeed);
			}
			else
			{
				_rnd = new Random();
			}
		}

		public RandomGraph(double pctEdges) : this(pctEdges, -1) { }
		#endregion

		#region Random Graphs
		struct EdgeKey
		{
			public int From;
			public int To;

			public EdgeKey(int FromPrm, int ToPrm)
			{
				From = FromPrm;
				To = ToPrm;
			}
		}

        public Graph GetGraph(int cnod)
		{
			int cEdgesNeeded = (int)(cnod * (cnod - 1) * _pctEdges);
			int cEdgesSoFar = 0;
			Set<EdgeKey> setEdges = new Set<EdgeKey>();
			Graph graph = new Graph();

			graph.InsertNodes(cnod);

			while (cEdgesSoFar < cEdgesNeeded)
			{
				EdgeKey ekey = new EdgeKey(_rnd.Next(cnod), _rnd.Next(cnod));

				if (setEdges.Contains(ekey) || ekey.From == ekey.To)
				{
					continue;
				}
				graph.InsertEdge(ekey.From, ekey.To);
				setEdges.Add(ekey);
				cEdgesSoFar++;
			}
			return graph;
		}

		public void IsomorphicPair(int cnod, out Graph graph1, out Graph graph2)
		{
			graph1 = GetGraph(cnod);
			graph2 = graph1.IsomorphicShuffling(_rnd);
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
				RandomGraph rg = new RandomGraph(0.2, 20);
				Assert.IsNotNull(rg);
			}

			[Test]
			public void TestGetGraph()
			{
				RandomGraph rg = new RandomGraph(0.3);

				Graph graph = rg.GetGraph(30);
				Assert.IsNotNull(graph);
				Assert.AreEqual(30, graph.NodeCount);
			}
		}
#endif
		#endregion
	}
#endif
}

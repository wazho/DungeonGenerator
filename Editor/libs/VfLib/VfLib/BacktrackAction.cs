using System;
using System.Collections.Generic;
using System.Text;
#if NUNIT
using NUnit.Framework;
#endif

namespace vflibcs
{
	enum Action
	{
		deleteMatch,
		groupMove
	}


	class BacktrackAction
	{
		#region Private Variables
		Action _act;
		int _iGraph;
		int _inod;
		Groups _grpRestore;
		#endregion

		#region Constructor
		internal BacktrackAction(Action act, int iGraph, int inod, Groups grpRestore)
		{
			_act = act;
			_iGraph = iGraph;
			_inod = inod;
			_grpRestore = grpRestore;
		}

		internal BacktrackAction(Action act, int iGraph, int inod) : this(act, iGraph, inod, (Groups)0) {}

		#endregion

		#region Backtracking
		internal void Backtrack(VfState vfs)
		{
			switch (_act)
			{
				case Action.deleteMatch:
					vfs.RemoveFromMappingList(_iGraph, _inod);
					break;

				case Action.groupMove:
					vfs.MakeMove(_iGraph, _inod, _grpRestore);
					break;
			}
		}
		#endregion

		#region NUNIT Testing
#if NUNIT
		[TestFixture]
		public class BacktrackActionTester
		{
			VfState VfsTest()
			{
				Graph graph1 = new Graph();
				Assert.AreEqual(0, graph1.InsertNode());
				Assert.AreEqual(1, graph1.InsertNode());
				Assert.AreEqual(2, graph1.InsertNode());
				Assert.AreEqual(3, graph1.InsertNode());
				Assert.AreEqual(4, graph1.InsertNode());
				Assert.AreEqual(5, graph1.InsertNode());
				// Circular graph with "extra" edge at (0,3)
				graph1.InsertEdge(0, 1);
				graph1.InsertEdge(1, 2);
				graph1.InsertEdge(2, 3);
				graph1.InsertEdge(3, 4);
				graph1.InsertEdge(4, 5);
				graph1.InsertEdge(5, 0);
				graph1.InsertEdge(0, 3);

				Graph graph2 = new Graph();
				Assert.AreEqual(0, graph2.InsertNode());
				Assert.AreEqual(1, graph2.InsertNode());
				Assert.AreEqual(2, graph2.InsertNode());
				Assert.AreEqual(3, graph2.InsertNode());
				Assert.AreEqual(4, graph2.InsertNode());
				Assert.AreEqual(5, graph2.InsertNode());
				// Same graph in reverse order with slightly offset "extra" edge at (4,1)
				graph2.InsertEdge(1, 0);
				graph2.InsertEdge(2, 1);
				graph2.InsertEdge(3, 2);
				graph2.InsertEdge(4, 3);
				graph2.InsertEdge(5, 4);
				graph2.InsertEdge(0, 5);
				graph2.InsertEdge(4, 1);

				return new VfState(graph1, graph2);
			}

			[Test]
			public void TestConstructor()
			{
				Assert.IsNotNull(new BacktrackAction(Action.deleteMatch, 1, 0));
				Assert.IsNotNull(new BacktrackAction(Action.groupMove, 1, 0, Groups.FromMapping));
			}
		}
#endif
		#endregion

	}
}

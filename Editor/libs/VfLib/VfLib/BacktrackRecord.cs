using System;
using System.Collections.Generic;
using System.Text;
#if NUNIT
using NUnit.Framework;
#endif

namespace vflibcs
{
	class BacktrackRecord
	{
		#region Private Variables
		List<BacktrackAction> _lstActions = new List<BacktrackAction>();
		#endregion

		#region Actions
		internal void AddAction(BacktrackAction act)
		{
			_lstActions.Insert(0, act);
		}

		internal void SetMatch(int inod1, int inod2, VfState vfs)
		{
			MoveToGroup(1, inod1, Groups.ContainedInMapping, vfs);
			MoveToGroup(2, inod2, Groups.ContainedInMapping, vfs);

			vfs.SetMapping(inod1, inod2);

			// Add actions to undo this act...
			AddAction(new BacktrackAction(Action.deleteMatch, 1, inod1));
			AddAction(new BacktrackAction(Action.deleteMatch, 2, inod2));
		}

		internal void MoveToGroup(int iGraph, int inod, Groups grpNew, VfState vfs)
		{
			VfGraph vfg = iGraph == 1 ? vfs.Vfgr1 : vfs.Vfgr2;
			Groups grpOld = vfg.GetGroup(inod);

			if (grpOld == Groups.FromMapping && grpNew == Groups.ToMapping ||
				grpOld == Groups.ToMapping && grpNew == Groups.FromMapping)
			{
				grpNew = Groups.FromMapping | Groups.ToMapping;
			}
			if (grpOld != (grpOld | grpNew))
			{
				AddAction(new BacktrackAction(Action.groupMove, iGraph, inod, grpOld));
				vfs.MakeMove(iGraph, inod, grpNew);
			}
		}
		#endregion

		#region Backtracking
		internal void Backtrack(VfState vfs)
		{
			foreach (BacktrackAction act in _lstActions)
			{
				act.Backtrack(vfs);
			}
		}
		#endregion

		#region NUNIT Testing
#if NUNIT
		[TestFixture]
		public class BacktrackRecordTester
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
				Assert.IsNotNull(new BacktrackRecord());
			}

			[Test]
			public void TestMatchBacktrack()
			{
				VfState vfs = VfsTest();
				BacktrackRecord btr = new BacktrackRecord();

				btr.SetMatch(0, 1, vfs);
				Groups grp1 = vfs.Vfgr1.GetGroup(0);
				Groups grp2 = vfs.Vfgr2.GetGroup(1);

				Assert.IsTrue((((int)grp1 & (int)Groups.ContainedInMapping)) != 0);
				Assert.IsTrue((((int)grp2 & (int)Groups.ContainedInMapping)) != 0);
				Assert.AreEqual(Groups.ContainedInMapping, vfs.Vfgr1.GetGroup(0));
				Assert.AreEqual(Groups.ContainedInMapping, vfs.Vfgr2.GetGroup(1));
				btr.Backtrack(vfs);
				grp1 = vfs.Vfgr1.GetGroup(0);
				grp2 = vfs.Vfgr2.GetGroup(1);

				Assert.IsFalse((((int)grp1 & (int)Groups.ContainedInMapping)) != 0);
				Assert.IsFalse((((int)grp2 & (int)Groups.ContainedInMapping)) != 0);
				Assert.AreEqual(Groups.Disconnected, vfs.Vfgr1.GetGroup(0));
				Assert.AreEqual(Groups.Disconnected, vfs.Vfgr2.GetGroup(1));
			}
		}
#endif
		#endregion
	}
}

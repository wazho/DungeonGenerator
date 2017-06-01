using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Guid = System.Guid;

namespace MissionGrammarSystem {
	// Setting and getting node for CreVox.
	public static class CreVoxAttach {
		private static CreVoxNode _rootNode;
		// Return root node
		public static CreVoxNode RootNode {
			get { return _rootNode; }
		}
		// Set root node.
		public static void SetCreVoxNodeRoot(MissionGrammarSystem.GraphGrammarNode node) {
			_rootNode = new CreVoxNode();
			_rootNode.SetNode(node);
		}
		// Realtime level generation I 
		public static CreVoxNode GenerateMissionGraph(string xmlPath, int Seed) {
			DungeonLevel.OperateXML.Unserialize.UnserializeFromXml(xmlPath);
			GraphGrammar graph = new GraphGrammar();
			// Rewrite system initialization.
			RewriteSystem.Initial(Seed);
			graph = RewriteSystem.TransformFromGraph();
			var stopWatch = System.Diagnostics.Stopwatch.StartNew();
			// Iterate until finish.
			while (
				(
					// Still exist non-terminal nodes.
					graph.Nodes.Exists(n => n.Terminal == NodeTerminalType.NonTerminal)
					// Have to exhauste all rules that set minimum.
					|| RewriteSystem.Rules.Sum(r => r.QuantityLimitMin) > 0
				)
				// Time limit is 3,000 ms.
				&& stopWatch.ElapsedMilliseconds <= 3000
			) {
				// Rewrite system iteration.
				RewriteSystem.Iterate();
				// Update the current graph.
				graph = RewriteSystem.TransformFromGraph();
			}
			stopWatch.Stop();
			// Setting root node for CreVoxAttach.
			SetCreVoxNodeRoot(graph.Nodes[0]);
			Debug.Log(_rootNode);
			return _rootNode;
		}

	}
	// For output iterated nodes.
	public class CreVoxNode {
		private Guid _symbolID;
		private Guid _alphabetID;
		private List<CreVoxNode> _children;

		public Guid SymbolID {
			get { return _symbolID; }
			set { _symbolID = value; }
		}
		public Guid AlphabetID {
			get { return _alphabetID; }
			set { _alphabetID = value; }
		}
		public List<CreVoxNode> Children {
			get { return _children; }
			set { _children = value; }
		}
		// Recursion copy node value.
		public void SetNode(MissionGrammarSystem.GraphGrammarNode node) {
			_children = new List<CreVoxNode>();
			_alphabetID = node.AlphabetID;
			_symbolID = node.ID;
			foreach (MissionGrammarSystem.GraphGrammarNode n in node.Children) {
				_children.Add(new CreVoxNode());
				_children[_children.Count - 1].SetNode(n);
			}
		}
	}
}
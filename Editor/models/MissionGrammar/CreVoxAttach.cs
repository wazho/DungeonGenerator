using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
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
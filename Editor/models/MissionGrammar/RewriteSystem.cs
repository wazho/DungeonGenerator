using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Guid = System.Guid;

namespace MissionGrammarSystem {
	public static class RewriteSystem {
		// Current root of the mission graph.
		private static Node       _root;
		// The rules that are transformed to the tree structure.
		private static List<Rule> _rules;
		// When click the initial button of generate graph page.
		public static void Initial() {
			// Initial the current graph.
			_root = new Node(Alphabet.StartingNode);
			// [TEST]-----------
			Node _entrance = new Node(Alphabet.Nodes[1]);
			Node _go = new Node(Alphabet.Nodes[2]);
			Node _start = new Node(Alphabet.Nodes[3]);
			Node _explore = new Node(Alphabet.Nodes[4]);
			Node _crossRoad = new Node(Alphabet.Nodes[5]);
			Node _boss = new Node(Alphabet.Nodes[6]);
			Node _gate = new Node(Alphabet.Nodes[7]);
			Node _fork = new Node(Alphabet.Nodes[8]);
			//     ¢z> entrance
			// root -> gate -> explore -> fork -> go -> boss
			//             ¢|> entrance
			_root.Children = new List<Node>() { _entrance, _gate};
			_entrance.Children = new List<Node>();
			_go.Children = new List<Node>();
			_start.Children = new List<Node>();
			_explore.Children = new List<Node>() { _fork, _boss };
			_crossRoad.Children = new List<Node>();
			_boss.Children = new List<Node>() { _explore };
			_gate.Children = new List<Node>() { _explore, _entrance, _boss};
			_fork.Children = new List<Node>() { _boss, _go};
			Debug.Log("Starting node : " + _root.Name);
			//---------------------
			_rules = new List<Rule>();
			// According to current rules of mission grammar, transform them to tree structure.
			TransformRules();
		}

		public static void Iterate() {
			ProgressIteration(_root);
			ClearExplored(_root);
		}
		// Depth-first search.
		private static void ProgressIteration(Node node) {
			// Step 1: Find matchs.
			Rule result = FindMatchs(node);

			if (result != null) {
				Debug.Log("Current node: " + node.Name + " is match the rule : " + result.Name);
			} else {
				Debug.Log("Current node: " + node.Name + " doesn't match any rule.");
			}

			// Has explored this node.
			node.Explored = true;
			// For each children.
			foreach (Node childNode in node.Children) {
				if (childNode.Explored == false) {
					ProgressIteration(childNode);
				}
			}
		}
		private static void ClearExplored(Node node) {
			node.Explored = false;
			foreach (Node childNode in node.Children) {
				if (childNode.Explored) {
					ClearExplored(childNode);
				}
			}
		}
		// According to current rules of mission grammar, transform them to tree structure.
		private static void TransformRules() {
			foreach (var originGroup in MissionGrammar.Groups) {
				foreach (var originRule in originGroup.Rules) {
					//Debug.Log(originRule.Name + "-" + originRule.Description);
					// Declare the rule. Can use 'rule.SourceRoot' and 'rule.ReplacementRoot'.
					Rule rule = new Rule();
					// Transform
					int nodeCount;
					// [Will remove just for test] 
					rule.Name = originRule.Name;

					rule.SourceRoot = TransformGraph(originRule.SourceRule, out nodeCount);
					rule.SourceNodeCount = nodeCount;
					rule.ReplacementRoot = TransformGraph(originRule.ReplacementRule, out nodeCount);
					rule.ReplacementNodeCount = nodeCount;
					_rules.Add(rule);
					// Show the message to proof you code is correct.
					//ProgressIteration(rule.SourceRoot);
					//ProgressIteration(rule.ReplacementRoot);
				}
			}
		}
		// Transform a graph into tree struct. Return root.
		private static Node TransformGraph(GraphGrammar graph, out int nodeCount) {
			// Initialize nodes
			Node[] nodes = new Node[graph.Nodes.Count];
			for (int i = 0; i < graph.Nodes.Count; i++) {
				nodes[i] = new Node(graph.Nodes[i], graph.Nodes[i].Ordering);
			}
			// Set parents and children
			for (int i = 0; i < graph.Nodes.Count; i++) {
				foreach (var childNode in graph.Nodes[i].Children) {
					int index = graph.Nodes.FindIndex(n => n.ID == childNode.ID);
					if (nodes[i].Children == null) {
						nodes[i].Children = new List<Node>();
					}
					nodes[i].Children.Add(nodes[index]);
				}
				foreach (var parentsNode in graph.Nodes[i].Parents) {
					int index = graph.Nodes.FindIndex(n => n.ID == parentsNode.ID);
					if (nodes[i].Parents == null) {
						nodes[i].Parents = new List<Node>();
					}
					nodes[i].Parents.Add(nodes[index]);
				}
			}
			nodeCount = graph.Nodes.Count;
			return nodes.FirstOrDefault<Node>(n => n.Index == 1);
		}

		private static bool[,] _usedIndexTable;
		private static List<Node> matchNodes;
		private static List<Node> exploredNodes;
		private static Rule FindMatchs(Node node) {
			if(matchNodes != null) {
				for (int i = 0; i < matchNodes.Count; i++) {
					matchNodes[i].Index = 0;
				}
			}
			foreach (var rule in _rules) {
				if (rule.SourceRoot.AlphabetID == node.AlphabetID) {
					matchNodes = new List<Node>();
					exploredNodes = new List<Node>();
					_usedIndexTable = new bool[rule.SourceNodeCount + 1, rule.SourceNodeCount + 1];
					node.Index = rule.SourceRoot.Index;
					if (RecursionMatch(node, rule.SourceRoot)) {
						return rule;
					}
					// If not match then clear index.
					for (int i = 0; i < matchNodes.Count; i++) {
						matchNodes[i].Index = 0;
					}
				}
			}
			// Not found.
			return null;
		}
		// Confirm the children are match
		private static bool RecursionMatch(Node node, Node matchNode) {
			exploredNodes.Add(node);
			foreach (Node childMatchNode in matchNode.Children) {
				bool _isMatch = false;
				foreach (Node childNode in node.Children) {
					// If this node index and the rule index have not be used
					if (((childNode.Index == 0 &&
						! _usedIndexTable[matchNode.Index, childMatchNode.Index] ) ||
						childNode.Index == childMatchNode.Index) &&
						childNode.AlphabetID == childMatchNode.AlphabetID) {
						// Record used connection, not node.
						_usedIndexTable[matchNode.Index, childMatchNode.Index] = true;
						childNode.Index = childMatchNode.Index;
						// If the children are also match.
						if (exploredNodes.Exists(x => ReferenceEquals(x, childNode)) || RecursionMatch(childNode, childMatchNode)) {
							_isMatch = true;
							matchNodes.Add(childNode);
							break;
						}
					}
				}
				// If no child is match.
				if (! _isMatch) {
					return false;
				}
			}
			// If rule node have no child, or said this rule is match.
			return true;
		}
		private static void RemoveConnections() {

		}
		private static void ReplaceNodes() {

		}
		private static void AppendNodes() {

		}
		private static void ReAddConnection() {

		}
		private static void RemoveIndex() {

		}

		// This is the minimum unit of exporting mission graph.
		private class Node {
			// GUID for this symbol in alphabet.
			private Guid             _alphabetID;
			// Members.
			private string           _name;
			private int              _index;
			private NodeTerminalType _terminal;
			private List<Node>       _parents;
			private List<Node>       _children;
			private bool             _isExplored; 
			// Constructor.
			public Node() {
				this._alphabetID = Guid.Empty;
				this._name       = string.Empty;
				this._index      = 0;
				this._terminal   = NodeTerminalType.Terminal;
				this._parents    = new List<Node>();
				this._children   = new List<Node>();
				this._isExplored = false;
			}
			public Node(GraphGrammarNode node) : this() {
				this._alphabetID = node.AlphabetID;
				this._name       = node.Name;
				this._terminal   = node.Terminal;
			}
			// Just for eample, maybe will delete soon.
			public Node(GraphGrammarNode node, int index) : this() {
				this._alphabetID = node.AlphabetID;
				this._name       = node.Name;
				this._index      = index;
				this._terminal   = node.Terminal;
			}
			// Name, getter and setter.
			public string Name {
				get { return _name; }
				set { _name = value; }
			}
			// Index, getter and setter.
			public int Index {
				get { return _index; }
				set { _index = value; }
			}
			// Parents, getter and setter.
			public List<Node> Parents {
				get { return _parents; }
				set { _parents = value; }
			}
			// Children, getter and setter.
			public List<Node> Children {
				get { return _children; }
				set { _children = value; }
			}
			// AlphabetID, getter and setter
			public Guid AlphabetID {
				get { return _alphabetID; }
			}
			// Explored, getter and setter.
			public bool Explored {
				get { return _isExplored; }
				set { _isExplored = value; }
			}
			// Explored, getter.
			public Node UnexploredChild {
				get { return _children.FirstOrDefault<Node>(n => n.Explored == false); }
			}
		}
		// This is a pair of source rule and replacement rule.
		private class Rule {
			// [Will remove just for test]
			public string Name;

			private Node _sourceRoot;
			private Node _replacementRoot;
			private int _sourceNodeCount;
			private int _replacementNodeCount;
			// Constructor.
			public Rule() {
				this._sourceRoot      = new Node();
				this._replacementRoot = new Node();
			}
			// Source, getter and setter.
			public Node SourceRoot {
				get { return _sourceRoot; }
				set { _sourceRoot = value; }
			}
			// Replacement, getter and setter.
			public Node ReplacementRoot {
				get { return _replacementRoot; }
				set { _replacementRoot = value; }
			}
			// Source node count, getter and setter.
			public int SourceNodeCount {
				get { return _sourceNodeCount; }
				set { _sourceNodeCount = value; }
			}
			// Source node count, getter and setter.
			public int ReplacementNodeCount {
				get { return _replacementNodeCount; }
				set { _replacementNodeCount = value; }
			}
		}
		private struct CompareNode {
			public Node node;
			public Node matchNode;
			public CompareNode(Node node, Node matchNode) {
				this.node = node;
				this.matchNode = matchNode;
			}
		}
	}
	
}
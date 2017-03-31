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
			//     �z> entrance
			// root -> gate -> explore -> fork -> go
			//             �|> entrance
			_root.Children = new List<Node>() { _entrance, _gate};
			_entrance.Children = new List<Node>();
			_go.Children = new List<Node>();
			_start.Children = new List<Node>();
			_explore.Children = new List<Node>() { _fork};
			_crossRoad.Children = new List<Node>();
			_boss.Children = new List<Node>();
			_gate.Children = new List<Node>() { _explore, _entrance};
			_fork.Children = new List<Node>() { _go};
			_explore.Children.Add(_boss);
			Debug.Log("Starting node : " + _root.Name);
			//---------------------
			_rules = new List<Rule>();
			// According to current rules of mission grammar, transform them to tree structure.
			TransformRules();
		}

		public static void Iterate() {
			Match result = FindMatchs();
			if (result != null) {
				Debug.Log("Match Rule : " + result.rule.Name);
				ProgressIteration(result.root);
			} else {
				Debug.Log("Not found.");
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
		private static void ProgressIteration(Node node) {
			Debug.Log(node.Index + " - " + node.Name);
			// Recursive for children.
			foreach (Node childNode in node.Children) {
				if (childNode.Index > 0)
					ProgressIteration(childNode);
			}
			if (node.Children.Count == 0) {
				Debug.Log("null");
			}
		}

		private static bool[] _usedIndexTable;
		private static List<Node> matchNodes;
		private static Match FindMatchs() {
			Node fakeRoot = new Node();
			fakeRoot.Children = new List<Node>();
			fakeRoot.Children.Add(_root);
			return RecursionFindRoot(fakeRoot);
		}
		// Find root.
		private static Match RecursionFindRoot(Node node) {
			foreach (var childNode in node.Children) {
				foreach (var rule in _rules) {
					if (rule.SourceRoot.AlphabetID == childNode.AlphabetID) {
						matchNodes = new List<Node>();
						_usedIndexTable = new bool[rule.SourceNodeCount + 1];
						if (RecursionMatch(childNode, rule.SourceRoot)) {
							return new Match(childNode, rule);
						}
						// If not match then clear index.
						for(int i=0;i< matchNodes.Count; i++) {
							matchNodes[i].Index = 0;
						}
					}
				}
				// Recursion children node 
				// If find then return it. else continue find other children.
				Match match = RecursionFindRoot(childNode);
				if (match != null)
					return match;
			}
			// Not found.
			return null;
		}
		// Confirm the children are match
		private static bool RecursionMatch(Node node, Node matchNode) {
			_usedIndexTable[matchNode.Index] = true;
			node.Index = matchNode.Index;
			foreach (Node childMatchNode in matchNode.Children) {
				bool _isMatch = false;
				foreach (Node childNode in node.Children) {
					// If this node index and the rule index have not be used
					if (childNode.Index == 0 &&
						! _usedIndexTable[childMatchNode.Index] &&
						childNode.AlphabetID == childMatchNode.AlphabetID) {
						// If the children are also match.
						if (RecursionMatch(childNode, childMatchNode)) {
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
			// Constructor.
			public Node() {
				this._alphabetID = Guid.Empty;
				this._name       = string.Empty;
				this._index      = 0;
				this._terminal   = NodeTerminalType.Terminal;
				this._parents    = new List<Node>();
				this._children   = new List<Node>();
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
		private class Match {
			public Node root;
			public Rule rule;
			public Match(Node root, Rule rule) {
				this.root = root;
				this.rule = rule;
			}
		}
	}
	
}
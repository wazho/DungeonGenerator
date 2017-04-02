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
			_rules = new List<Rule>();
			// According to current rules of mission grammar, transform them to tree structure.
			TransformRules();
			// [TEST] 
			_root = _rules[0].SourceRoot;
			TestClearIndex(_root);
		}
		// When click the iterate button of generate graph page.
		public static void Iterate() {
			ProgressIteration(_root);
			ClearExplored(_root);
		}
		// Export the original structure from tree structure to canvas.
		public static GraphGrammar TransformFromGraph() {
			GraphGrammar graphGrammar = new GraphGrammar();

			// Code here.
			// Parsing from '_root'.

			return graphGrammar;
		}

		// [TEST] 
		private static void TestClearIndex(Node node) {
			node.Index = 0;
			foreach (Node childNode in node.Children) {
				if (childNode.Index > 0) {
					TestClearIndex(childNode);
				}
			}
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

			// Step 2: Remove connections.
			RemoveConnections();
			// Step 3: Replace nodes.
			ReplaceNodes(result);
			AppendNodes(result);
			ShrinkNodes(result);

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

		private static bool[] _usedIndexTable;
		private static List<Node> matchNodes = new List<Node>();
		private static List<Node> exploredNodes = new List<Node>();
		private static Rule FindMatchs(Node node) {
			foreach (var rule in _rules) {
				if (rule.SourceRoot.AlphabetID == node.AlphabetID) {
					// Clear index.
					for (int i = 0; i < matchNodes.Count; i++) {
						matchNodes[i].Index = 0;
					}
					matchNodes.Clear();
					exploredNodes.Clear();
					_usedIndexTable = new bool[rule.SourceNodeCount + 1];
					node.Index = rule.SourceRoot.Index;
					matchNodes.Add(node);
					_usedIndexTable[node.Index] = true;
					if (RecursionMatch(node, rule.SourceRoot)) {
						return rule;
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
					if (childNode.Index == 0 &&
						! _usedIndexTable[childMatchNode.Index]  &&
						childNode.AlphabetID == childMatchNode.AlphabetID) {
						// Record used connection, not node.
						_usedIndexTable[childMatchNode.Index] = true;
						childNode.Index = childMatchNode.Index;
						matchNodes.Add(childNode);
						// If the children are also match.
						if (exploredNodes.Exists(x => ReferenceEquals(x, childNode)) || RecursionMatch(childNode, childMatchNode)) {
							_isMatch = true;
							break;
						}
					}else if (childNode.Index == childMatchNode.Index &&
						_usedIndexTable[childMatchNode.Index]) {
						_isMatch = true;
						break;
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
			foreach(Node node in matchNodes) {
				node.Parents.Clear();
				node.Children.Clear();
				Debug.Log("Index: " + node.Index + "Is Removed");
			}
		}
		private static void ReplaceNodes(Rule replaceRule) {
			foreach (Node node in matchNodes) {
				node.Name = findNode(node.Index + 1,replaceRule.ReplacementRoot).Name;
			}
		}
		// Return node have same index from rule.
		private static Node findNode(int index,Node root) {
			Node temp;
			if (root.Index == index) {
				return root;
			}
			foreach(Node node in root.Children) {
				temp = findNode(index, node);
				if(temp != null) {
					return temp;
				}
			}
			return null;
		}
		private static void AppendNodes(Rule replaceRule) {
			for (;matchNodes.ToArray().Length < replaceRule.ReplacementNodeCount;) {
				matchNodes.Add(findNode(matchNodes.ToArray().Length + 1, replaceRule.ReplacementRoot));
				Debug.Log("Add: " + matchNodes.Last().Name);
			}
		}
		private static void ShrinkNodes(Rule replaceRule) {
			for (; matchNodes.ToArray().Length > replaceRule.ReplacementNodeCount;) {
				if(matchNodes.Remove(matchNodes.Where(e=>e.Index> replaceRule.ReplacementNodeCount).FirstOrDefault())) {
					Debug.Log("Minor: " + matchNodes.Last().Name);
				}
			}
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

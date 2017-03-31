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
			// According to current rules of mission grammar, transform them to tree structure.
			TransformRules();
		}

		public static void Iterate() {
			// This is EXAMPLE.
			Node exampleRoot  = new Node(new GraphGrammarNode("en", "", "", NodeTerminalType.Terminal), 1);
			Node exampleNode2 = new Node(new GraphGrammarNode("x",  "", "", NodeTerminalType.Terminal), 2);
			Node exampleNode3 = new Node(new GraphGrammarNode("CR", "", "", NodeTerminalType.NonTerminal), 3);
			Node exampleNode4 = new Node(new GraphGrammarNode("x",  "", "", NodeTerminalType.Terminal), 4);
			Node exampleNode5 = new Node(new GraphGrammarNode("CR", "", "", NodeTerminalType.NonTerminal), 5);
			Node exampleNode6 = new Node(new GraphGrammarNode("go", "", "", NodeTerminalType.Terminal), 6);

			// 6
			exampleNode6.Parents  = new List<Node>() { exampleNode5 };
			// 5
			exampleNode5.Parents  = new List<Node>() { exampleNode4 };
			// 4
			exampleNode4.Parents  = new List<Node>() { exampleNode3, exampleNode4 };
			exampleNode4.Children = new List<Node>() { exampleNode5, exampleNode6 };
			// 3
			exampleNode3.Parents  = new List<Node>() { exampleNode2 };
			exampleNode3.Children = new List<Node>() { exampleNode4 };
			// 2
			exampleNode2.Parents  = new List<Node>() { exampleRoot };
			exampleNode2.Children = new List<Node>() { exampleNode3, exampleNode4 };
			// 1
			exampleRoot.Children  = new List<Node>() { exampleNode2 };

			ProgressIteration(exampleRoot);
		}

		// According to current rules of mission grammar, transform them to tree structure.
		private static void TransformRules() {
			foreach (var originGroup in MissionGrammar.Groups) {
				foreach (var originRule in originGroup.Rules) {
					Debug.Log(originRule.Name + "-" + originRule.Description);
					// Declare the rule. Can use 'rule.SourceRoot' and 'rule.ReplacementRoot'.
					Rule rule = new Rule();
					// Transform
					rule.SourceRoot      = TransformGraph(originRule.SourceRule);
					rule.ReplacementRoot = TransformGraph(originRule.ReplacementRule);

					// Show the message to proof you code is correct.
					//ProgressIteration(rule.SourceRoot);
					ProgressIteration(rule.ReplacementRoot);
				}
			}
		}
		// Transform a graph into tree struct. Return root.
		private static Node TransformGraph(GraphGrammar graph) {
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
			return nodes.FirstOrDefault<Node>(n => n.Index == 1);
		}
		private static void ProgressIteration(Node node) {
			// If this node is null then throw error. (Current just return void.)
			if (node == null) { return; }
			
			Debug.Log(node.Index + " - " + node.Name);
			// Recursive for children.
			foreach (Node childNode in node.Children) {
				ProgressIteration(childNode);
			}
			if (node.Children.Count == 0) {
				Debug.Log("null");
			}
		}

		private static void RemoveConnections(ref Node rootNode) {
			foreach(Node node in rootNode.Children) {
				// RemoveConnections if node have index.
				if(node.Index != 0) {
					node.Parents.Clear();
					node.Children.Clear();
					}
				}
			ReplaceNodes(ref rootNode);
		}
 
		private static void ReplaceNodes(ref Node rootNode) {
			// [Temporary]****
			Rule testRule = new Rule();
			Node tempNode;
			//****************

			foreach (Node node in rootNode.Children) {
				if (node.Index != 0) {
					tempNode = testRule.SourceRoot.Children.Where(e => e.Index == node.Index).FirstOrDefault();
					if(tempNode != null) {
						node.Clone(tempNode);
					}
				}
			}
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

			// Copy node.
			public void Clone(Node node) {
				this._name       = node.Name;
				this._index      = node.Index;
				this._alphabetID = node.AlphabetID;
				this._terminal   = node.Terminal;
 
				if (node.Children.Count > 0) {
					foreach(Node n in node.Children) {
						this.Children.Add(_root.Children.Where(e => e.Index == n.Index).FirstOrDefault());
					}
				}
 
				if (node.Parents.Count > 0) {
					foreach (Node n in node.Parents) {
						this.Parents.Add(_root.Children.Where(e => e.Index == n.Index).FirstOrDefault());
					}
				}
			}

		}
		// This is a pair of source rule and replacement rule.
		private class Rule {
			private Node _sourceRoot;
			private Node _replacementRoot;
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
		}
	}
}
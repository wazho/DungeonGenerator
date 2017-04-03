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
		// Related nodes are a table that can quickly access nodes that are related with rule.
		private static List<Node> _relatedNodes;
		// The rules that are transformed to the tree structure.
		private static List<Rule> _rules;
		// Get non-terminal nodes.
		private static List<Node> _nonTerminalNodes;
		// When click the initial button of generate graph page.
		public static void Initial() {
			// Initial the current graph.
			_root             = new Node(Alphabet.StartingNode);
			_relatedNodes     = new List<Node>();
			_rules            = new List<Rule>();
			_nonTerminalNodes = new List<Node>();
			// Update _nonTerminalNodes.
			if (_root.Terminal == NodeTerminalType.NonTerminal) {
				_nonTerminalNodes.Add(_root);
			}
			// According to current rules of mission grammar, transform them to tree structure.
			TransformRules();
		}
		// When click the iterate button of generate graph page.
		public static void Iterate() {
			_relatedNodes.Clear();
			// Start interating.
			ProgressIteration();
		}
		// Reference table is used to get the symbol of Alphabet via the AlphabetID.
		private static Dictionary<Guid, GraphGrammarNode> _referenceNodeTable;
		private static Dictionary<Guid, GraphGrammarConnection> _referenceConnectionTable;
		// Mapping table is used to get the GraphGrammarNode via the Node.
		private static Dictionary<Node, GraphGrammarNode> _nodeMappingTable;
		private static Vector2 LEFT_TOP_POSITION = new Vector2(20, 30);
		private static int PADDING = 50;
		private static List<int> CountInLayer = new List<int>();
		// Export the original structure from tree structure to canvas.
		public static GraphGrammar TransformFromGraph() {
			GraphGrammar graphGrammar = new GraphGrammar();
			// Get reference table (For getting the symbol of Alphabet.) 
			_referenceNodeTable       = Alphabet.ReferenceNodeTable;
			_referenceConnectionTable = Alphabet.ReferenceConnectionTable;
			// Initialize mapping table.
			_nodeMappingTable = new Dictionary<Node, GraphGrammarNode>();
			// Add root node.
			_nodeMappingTable[_root] = new GraphGrammarNode(_referenceNodeTable[_root.AlphabetID]) { Position = LEFT_TOP_POSITION };
			graphGrammar.Nodes.Add(_nodeMappingTable[_root]);
			CountInLayer.Clear();
			CountInLayer.Add(0);
			RecursionGraphGrammar(_root, ref graphGrammar, 1);
			ClearExplored(_root);
			return graphGrammar;
		}
		// Add node and connection to graph grammar by dfs.
		// "layer" is used to calculate x position
		private static void RecursionGraphGrammar(Node node, ref GraphGrammar graphGrammar, int layer) {
			if (CountInLayer.Count <= layer) {
				CountInLayer.Add(0);
			}
			// Mark this node.
			node.Explored = true;
			// "index" is used to calculate y position.
			int index = 0;
			foreach (Node childNode in node.Children) {
				// Add connection (Now only use Connections[0], will modify).
				GraphGrammarConnection connection = new GraphGrammarConnection(Alphabet.Connections[0]);
				graphGrammar.Connections.Add(connection);
				// Set starting sticked attribute.
				_nodeMappingTable[node].AddStickiedConnection(connection, "start");
				connection.StartpointStickyOn = _nodeMappingTable[node];
				connection.StartPosition = _nodeMappingTable[node].Position;
				// If mapping table have not contained this Node then add it.
				if (! _nodeMappingTable.ContainsKey(childNode)) {
					// Set position.
					_nodeMappingTable[childNode] = new GraphGrammarNode(_referenceNodeTable[childNode.AlphabetID]) {
						Position = new Vector2(LEFT_TOP_POSITION.x + layer * PADDING,
						                       LEFT_TOP_POSITION.y + (CountInLayer[layer] + index) * PADDING)
					};
					graphGrammar.Nodes.Add(_nodeMappingTable[childNode]);
				}
				// Set ending sticked attribute.
				_nodeMappingTable[childNode].AddStickiedConnection(connection, "end");
				connection.EndpointStickyOn = _nodeMappingTable[childNode];
				connection.EndPosition = _nodeMappingTable[childNode].Position;
				// Check the mark exist.
				if (!childNode.Explored) {
					// Search deeper, so "layer" must increase.
					RecursionGraphGrammar(childNode, ref graphGrammar, layer + 1);
				}
				index++;
			}
			CountInLayer[layer] += index;
		}
		// Depth-first search.
		private static void ProgressIteration() {
			Node[] _nonTerminalNodesClone = _nonTerminalNodes.ToArray();
			foreach (Node node in _nonTerminalNodesClone) {
				// If not be replaced.
				if(node.Terminal == NodeTerminalType.NonTerminal) { 
					// Step 1: Find matchs and set indexes.
					Rule matchedRule = FindMatchs(node);
				
					if (matchedRule != null) {
						Debug.Log("Current node: '" + node.Name + "' is match the rule : " + matchedRule.Name + "  " + node.Index);
						// Step 2: Remove connections.
						RemoveConnections(matchedRule);
						// Step 3: Remove connections from replacement rule.
						ReplaceNodes(matchedRule);
						// Step 4: Append the new nodes from replacement rule.
						AppendNodes(matchedRule);
						// Step 5: Re-add the connections from replacement rule.
						ReAddConnection(matchedRule);
						// Step 6: Remove indexes.
						RemoveIndexes();
					} else {
						Debug.Log("Current node: '" + node.Name + "'.");
					}
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
					// Declare the rule. Can use 'rule.SourceRoot' and 'rule.ReplacementRoot'.
					Rule rule = new Rule();
					int nodeCount;
					// Transform source and replacement.
					rule.Name = originGroup.Name + " - " + originRule.Name;
					rule.SourceNodeTable      = TransformGraph(originRule.SourceRule, out nodeCount);
					rule.SourceRoot           = rule.SourceNodeTable.FirstOrDefault();
					rule.SourceNodeCount      = nodeCount;
					rule.ReplacementNodeTable = TransformGraph(originRule.ReplacementRule, out nodeCount);
					rule.ReplacementRoot      = rule.ReplacementNodeTable.FirstOrDefault();
					rule.ReplacementNodeCount = nodeCount;
					// Insert into the '_rules'.
					_rules.Add(rule);
				}
			}
		}
		// Transform a graph into tree struct. Then return the table.
		private static List<Node> TransformGraph(GraphGrammar graph, out int nodeCount) {
			// Initialize nodes
			Node[] nodes = new Node[graph.Nodes.Count];
			for (int i = 0; i < graph.Nodes.Count; i++) {
				nodes[i] = new Node(graph.Nodes[i], graph.Nodes[i].Ordering);
			}
			// Set parents and children
			for (int i = 0; i < graph.Nodes.Count; i++) {
				foreach (var childNode in graph.Nodes[i].Children) {
					int index = graph.Nodes.FindIndex(n => n.ID == childNode.ID);
					nodes[i].Children.Add(nodes[index]);
				}
				foreach (var parentsNode in graph.Nodes[i].Parents) {
					int index = graph.Nodes.FindIndex(n => n.ID == parentsNode.ID);
					nodes[i].Parents.Add(nodes[index]);
				}
			}
			// Update the node count.
			nodeCount = graph.Nodes.Count;
			// Return the table.
			return nodes.OrderBy(n => n.Index).ToList();
		}

		private static bool[] _usedIndexTable;
		private static List<Node> exploredNodes = new List<Node>();
		private static Rule FindMatchs(Node node) {
			foreach (var rule in _rules) {
				// Compare the root node of rule.
				if (rule.SourceRoot.AlphabetID == node.AlphabetID) {
					// Clear index of all nodes.
					for (int i = 0; i < _relatedNodes.Count; i++) { _relatedNodes[i].Index = 0; }
					_relatedNodes.Clear();
					exploredNodes.Clear();
					_usedIndexTable = new bool[rule.SourceNodeCount + 1];
					node.Index = rule.SourceRoot.Index;
					_relatedNodes.Add(node);
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
						! _usedIndexTable[childMatchNode.Index] &&
						childNode.AlphabetID == childMatchNode.AlphabetID) {
						// Record used connection, not node.
						_usedIndexTable[childMatchNode.Index] = true;
						childNode.Index = childMatchNode.Index;
						_relatedNodes.Add(childNode);
						// If the children are also match.
						if (exploredNodes.Exists(x => ReferenceEquals(x, childNode)) ||
							RecursionMatch(childNode, childMatchNode)) {
							_isMatch = true;
							break;
						}
					} else if (childNode.Index == childMatchNode.Index &&
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
		// Step 2: Remove connections.
		private static void RemoveConnections(Rule matchedRule) {
			foreach (Node node in _relatedNodes) {
				for (int i = 0; i < node.Children.Count; i++) {
					// If this node and its child are in the rule, remove the connective.
					if (node.Children[i].Index != 0) {
						node.Children[i].Parents.Remove(node);
						node.Children.RemoveAt(i);
						i--;
					}
				}
			}
		}
		// Step 3: Remove connections from replacement rule.
		private static void ReplaceNodes(Rule matchedRule) {
			// Replace the node from matched rule.
			foreach (Node node in _relatedNodes) {
				// Remove the replaced node from _nonTerminalNodes.
				if(node.Terminal == NodeTerminalType.NonTerminal) {
					_nonTerminalNodes.Remove(node);
				}
				node.Update(matchedRule.FindReplacementByIndex(node.Index));
				// Update _nonTerminalNodes.
				if (node.Terminal == NodeTerminalType.NonTerminal) {
					_nonTerminalNodes.Add(node);
				}
			}
		}
		// Step 4: Append the new nodes from replacement rule.
		private static void AppendNodes(Rule matchedRule) {
			foreach (Node matchedRuleNode in matchedRule.ReplacementNodeTable) {
				// If index does not exist then add.
				if (! _relatedNodes.Exists(x => x.Index == matchedRuleNode.Index)) {
					Node node = new Node(matchedRuleNode);
					_relatedNodes.Add(node);
					// Update _nonTerminalNodes.
					if (node.Terminal == NodeTerminalType.NonTerminal) {
						_nonTerminalNodes.Add(node);
					}
				}
			}
			// Order by Index.
			_relatedNodes = _relatedNodes.OrderBy(x => x.Index).ToList();
		}
		// Step 5: Re-add the connections from replacement rule.
		private static void ReAddConnection(Rule matchedRule) {
			for (int i = 0; i < _relatedNodes.Count; i++) {
				foreach (Node matchedRuleNode in matchedRule.FindReplacementByIndex(_relatedNodes[i].Index).Children) {
					_relatedNodes[i].Children.Add(_relatedNodes[matchedRuleNode.Index - 1]);
					_relatedNodes[matchedRuleNode.Index - 1].Parents.Add(_relatedNodes[i]);
				}
			}
		}
		// Step 6: Remove indexes.
		private static void RemoveIndexes() {
			foreach (var node in _relatedNodes) {
				node.Index = 0;
			}
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
			public Node(Node node) : this() {
				this._alphabetID = node.AlphabetID;
				this._name       = node.Name;
				this._terminal   = node.Terminal;
				this.Index       = node.Index;
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
			// Terminal, getter and setter.
			public NodeTerminalType Terminal {
				get { return _terminal; }
				set { _terminal = value; }
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
			// Update the node information, only name and terminal.
			public void Update(Node node) {
				_alphabetID = node.AlphabetID;
				_name       = node.Name;
				_terminal   = node.Terminal;
			}
		}
		// This is a pair of source rule and replacement rule.
		private class Rule {
			// [Will remove just for test]
			public string Name;

			private Node       _sourceRoot;
			private Node       _replacementRoot;
			private int        _sourceNodeCount;
			private int        _replacementNodeCount;
			private List<Node> _sourceNodeTable;
			private List<Node> _replacementNodeTable;
			// Constructor.
			public Rule() {
				this._sourceRoot           = new Node();
				this._replacementRoot      = new Node();
				this._sourceNodeTable      = new List<Node>();
				this._replacementNodeTable = new List<Node>();
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
			// SourceNodeTable, getter and setter.
			public List<Node> SourceNodeTable {
				get { return _sourceNodeTable; }
				set { _sourceNodeTable = value; }
			}
			// ReplacementNodeTable, getter and setter.
			public List<Node> ReplacementNodeTable {
				get { return _replacementNodeTable; }
				set { _replacementNodeTable = value; }
			}
			// Find the node from source rule by index.
			public Node FindSourceByIndex(int index) {
				return _sourceNodeTable.Where(n => n.Index == index).FirstOrDefault();
			}
			// Find the node from replacement rule by index.
			public Node FindReplacementByIndex(int index) {
				return _replacementNodeTable.Where(n => n.Index == index).FirstOrDefault();
			}
		}
	}
}
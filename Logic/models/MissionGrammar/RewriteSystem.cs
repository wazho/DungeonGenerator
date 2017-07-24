using UnityEngine;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using Guid = System.Guid;
using Math = System.Math;
// VF Graph Isomorphism Algorithm.
using VFlibcs = vflibcs;

namespace MissionGrammarSystem {
	public static class RewriteSystem {
		// Current root of the mission graph.
		private static Node       _root;
		// Related nodes are a table that can quickly access nodes that are related with rule.
		private static List<Node> _relatedNodes;
		// The rules that are transformed to the tree structure.
		private static List<Rule> _rules;
		// Getter of _rules.
		public static List<Rule> Rules {
			get { return _rules; }
		}

		// When click the initial button of generate graph page.
		public static void Initial(int seed) {
            CleanNodeRelation();
            // Initial the current graph.
            _root         = new Node(Alphabet.StartingNode);
			_relatedNodes = new List<Node>();
			_rules        = new List<Rule>();
			Random.InitState(seed);
			// According to current rules of mission grammar, transform them to tree structure.
			TransformRules();
		}
		// When click the iterate button of generate graph page.
		public static void Iterate() {
			RemoveIndexes();
			// Clear the node table.
			_relatedNodes.Clear();
			// Start interating.
			ProgressIteration(_root);
			ClearExplored();
		}
		// Reference table is used to get the symbol of Alphabet via the AlphabetID.
		private static Dictionary<Guid, GraphGrammarNode> _referenceNodeTable;
		// private static Dictionary<Guid, GraphGrammarConnection> _referenceConnectionTable;
		// Mapping table is used to get the GraphGrammarNode via the Node.
		private static Dictionary<Node, GraphGrammarNode> _nodeMappingTable;
		private static Vector2 LEFT_TOP_POSITION = new Vector2(20, 30);
		private const int PADDING = 50;
		private static List<int> CountInLayer = new List<int>();
		private static Dictionary<Rule, VFlibcs.Graph> _ruleVFgraphTable = new Dictionary<Rule, VFlibcs.Graph>();
		// Record the node that the explored is true.
		private static Stack<Node> _exploredNodeStack = new Stack<Node>();
		// Export the original structure from tree structure to canvas.
		public static GraphGrammar TransformFromGraph() {
			var graphGrammar = new GraphGrammar();
			// Get reference table (For getting the symbol of Alphabet.) 
			_referenceNodeTable       = Alphabet.ReferenceNodeTable;
			// _referenceConnectionTable = Alphabet.ReferenceConnectionTable;
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
				var connection = new GraphGrammarConnection(Alphabet.Connections[0]);
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
		private static void ProgressIteration(Node root) {
			while (true) {
				// Step 1: Find matched rule.
				Rule matchedRule = FindMatchs(TransToVFGraph(root));
				if (matchedRule != null) {
					Debug.Log("Current match rule : " + matchedRule.Name);
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
                    break;
                } else {
					break;
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
		// Overloading remove explored of nodes in stack.
		private static void ClearExplored() {
			while(_exploredNodeStack.Count > 0) {
				_exploredNodeStack.Pop().Explored = false;
			}
		}
		// According to current rules of mission grammar, transform them to tree structure.
		private static void TransformRules() {
			foreach (var originGroup in MissionGrammar.Groups) {
				foreach (var originRule in originGroup.Rules) {
					// If the rule isn't enabled, then skip it.
					if (! originRule.Enable) { continue; }
					// Declare the rule. Can use 'rule.SourceRoot' and 'rule.ReplacementRoot'.
					var rule = new Rule();
					int nodeCount;
					// Transform source and replacement.
					rule.Name = originGroup.Name + " - " + originRule.Name;
					rule.SourceNodeTable      = TransformGraph(originRule.SourceRule, out nodeCount);
					rule.SourceRoot           = rule.SourceNodeTable.FirstOrDefault();
					rule.SourceNodeCount      = nodeCount;
					rule.ReplacementNodeTable = TransformGraph(originRule.ReplacementRule, out nodeCount);
					rule.ReplacementRoot      = rule.ReplacementNodeTable.FirstOrDefault();
					rule.ReplacementNodeCount = nodeCount;
					rule.Weight               = originRule.Weight;
					// -1 means infinite.
					rule.QuantityLimitMin = Math.Max(0, originRule.QuantityLimitMin);
					rule.QuantityLimitMax = (originRule.QuantityLimitMax > 0) ? originRule.QuantityLimitMax : -1;
					// Insert into the '_rules'.
					_rules.Add(rule);
					// Store the dictionary of VF graph.
					_ruleVFgraphTable.Add(rule, TransToVFGraph(rule.SourceRoot));
				}
			}
		}
		// Transform a graph into tree struct. Then return the table.
		private static List<Node> TransformGraph(GraphGrammar graph, out int nodeCount) {
			// Initialize nodes
			var nodes = new Node[graph.Nodes.Count];
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
		private static List<Rule> RandomOrderByWeight() {
			// Declare list to store rules by order.
			var orderRules = new List<Rule>();
			// Declare the clone list that avoid to modify original list.
			var cloneRules = new List<Rule>(_rules);
			// Sum of rule's weight.
			int sum = cloneRules.Sum(r => r.Weight);
			// When cloneRules is empty. The sort finish.
			while (cloneRules.Count > 0) {
				// Select one rule from the filtering result by weight.
				int minBounding = 0;
				int randomNum = Random.Range(1, sum + 1);
				foreach (Rule rule in cloneRules) {
					if (randomNum >= minBounding && randomNum <= minBounding + rule.Weight) {
						// Add rule to list.
						orderRules.Add(rule);
						// Remove rule in original list.
						cloneRules.Remove(rule);
						// Sum also need to decrease.
						sum -= rule.Weight;
						// Found it then break.
						break;
					} else {
						minBounding += rule.Weight;
					}
				}
			}
			return orderRules;
		}
		// Step 1: Find matchs.
		private static Rule FindMatchs(VFlibcs.Graph graphVF) {
			foreach (var rule in RandomOrderByWeight()) {
				if (rule.QuantityLimitMax == 0) { continue; }
				// VfState: Calculate the result of subgraph isomorphic.
				VFlibcs.VfState vfs = new VFlibcs.VfState(graphVF, _ruleVFgraphTable[rule], false, true);
				if (vfs.FMatch()) {
					// Reduce the quantity limit.
					if (rule.QuantityLimitMin > 0) { rule.QuantityLimitMin -= 1; }
					if (rule.QuantityLimitMax > 0) { rule.QuantityLimitMax -= 1; }
					_relatedNodes.Clear();
					for (int i = 0; i < vfs.Mapping2To1.Length; i++) {
						// Set Index.
						Node node  = graphVF.GetNodeAttr(vfs.Mapping2To1[i]) as Node;
						node.Index = (_ruleVFgraphTable[rule].GetNodeAttr(i) as Node).Index;
						_relatedNodes.Add(node);
						// Record this one is used in this iterating.
						node.Explored = true;
						_exploredNodeStack.Push(node);
					}
					return rule;
				}
			}
			return null;
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
				Node replaceNode = matchedRule.FindReplacementByIndex(node.Index);
                if (replaceNode == null){
                    break;
                }
                // If any node then keep origin node.
                if (Alphabet.IsAnyNode(replaceNode.AlphabetID)) {
					continue;
				}
				node.Update(replaceNode);
			}
		}
		// Step 4: Append the new nodes from replacement rule.
		private static void AppendNodes(Rule matchedRule) {
			foreach (Node matchedRuleNode in matchedRule.ReplacementNodeTable) {
				// If index does not exist then add.
				if (! _relatedNodes.Exists(x => x.Index == matchedRuleNode.Index)) {
					var node = new Node(matchedRuleNode);
					_relatedNodes.Add(node);
					// Record this one is used in this iterating.
					node.Explored = true;
					_exploredNodeStack.Push(node);
				}
			}
			// Order by Index.
			_relatedNodes = _relatedNodes.OrderBy(x => x.Index).ToList();
		}
        // Step 5: Re-add the connections from replacement rule.
        private static void ReAddConnection(Rule matchedRule) {
            bool txtSave = false;
            foreach (Node node in matchedRule.SourceNodeTable) {
                WriteNodeRelation(node.Name + " ");
            }
            WriteNodeRelation("=> ");
            for (int i = 0; i < _relatedNodes.Count; i++) {
                //Check if the room can be find
                if (matchedRule.FindReplacementByIndex(_relatedNodes[i].Index) == null) {
                    //find the last room in node
                    for (int j = 0; j < _relatedNodes.Count; j++) {
                        //find the last room in node //Debug.Log("find last node: " + matchedRule.FindReplacementByIndex(_relatedNodes[i - j].Index).Name);
                        if (i < j || matchedRule.FindReplacementByIndex(_relatedNodes[i - j].Index) == null) { continue; }
                        if (!txtSave) {
                            WriteNodeRelation(_relatedNodes[i - j].Name + "\r\n");
                            txtSave = true;
                        }
                        foreach (Node tempNode in _relatedNodes[i].Children) {
                            _relatedNodes[i - j].Children.Add(tempNode);
                            tempNode.Parents.Clear();
                            tempNode.Parents.Add(_relatedNodes[i - j]);

                        }
                    }
                    continue;
                }
                if (!txtSave) {
                    WriteNodeRelation(_relatedNodes[i].Name + "\r\n");
                    txtSave = true;
                }
                foreach (Node matchedRuleNode in matchedRule.FindReplacementByIndex(_relatedNodes[i].Index).Children) {
                    _relatedNodes[i].Children.Add(_relatedNodes[matchedRuleNode.Index - 1]);
                    _relatedNodes[matchedRuleNode.Index - 1].Parents.Add(_relatedNodes[i]);
                }
            }
        }
        // Step 6: Remove indexes.
        private static void RemoveIndexes() {
			foreach (var node in _relatedNodes ?? Enumerable.Empty<Node>()) {
				node.Index = 0;
			}
		}
        //save the node transform relation ino Assets/Resources/CreVox/NodeRelation.txt
        private static void WriteNodeRelation(string output) {
            if (!Directory.Exists("Assets/Resources/CreVox"))
                Directory.CreateDirectory("Assets/Resources/CreVox");
            StreamWriter writer = new StreamWriter("Assets/Resources/CreVox/NodeRelation.txt", append: true);
            writer.Write(output);
            writer.Close();
        }
        //clean the node transform relation
        private static void CleanNodeRelation() {
            if (!Directory.Exists("Assets/Resources/CreVox"))
                Directory.CreateDirectory("Assets/Resources/CreVox");
            StreamWriter writer = new StreamWriter("Assets/Resources/CreVox/NodeRelation.txt");
            writer.Write("");
            writer.Close();
        }
        // This is the minimum unit of exporting mission graph.
        public class Node {
			public Guid             AlphabetID { get; private set; }
			public string           Name       { get; set; }
			public int              Index      { get; set; }
			public NodeTerminalType Terminal   { get; set; }
			public List<Node>       Parents    { get; set; }
			public List<Node>       Children   { get; set; }
			public bool             Explored   { get; set; }

			// Constructor.
			public Node() {
				this.AlphabetID = Guid.Empty;
				this.Name       = string.Empty;
				this.Index      = 0;
				this.Terminal   = NodeTerminalType.Terminal;
				this.Parents    = new List<Node>();
				this.Children   = new List<Node>();
				this.Explored   = false;
			}
			public Node(GraphGrammarNode node) : this() {
				this.AlphabetID = node.AlphabetID;
				this.Name       = node.Name;
				this.Terminal   = node.Terminal;
			}
			public Node(GraphGrammarNode node, int index) : this() {
				this.AlphabetID = node.AlphabetID;
				this.Name       = node.Name;
				this.Index      = index;
				this.Terminal   = node.Terminal;
			}
			public Node(Node node) : this() {
				this.AlphabetID = node.AlphabetID;
				this.Name       = node.Name;
				this.Terminal   = node.Terminal;
				this.Index      = node.Index;
			}
			// Update the node information, only name and terminal.
			public void Update(Node node) {
				AlphabetID = node.AlphabetID;
				Name       = node.Name;
				Terminal   = node.Terminal;
			}
			public Node UnexploredChild {
				get { return Children.FirstOrDefault<Node>(n => ! n.Explored); }
			}
		}
		// This is a pair of source rule and replacement rule.
		public class Rule {
			// [Will remove just for test]
			public string     Name                 { get; set; }
			public Node       SourceRoot           { get; set; }
			public Node       ReplacementRoot      { get; set; }
			public int        SourceNodeCount      { get; set; }
			public int        ReplacementNodeCount { get; set; }
			public List<Node> SourceNodeTable      { get; set; }
			public List<Node> ReplacementNodeTable { get; set; }
			public int        Weight               { get; set; }
			public int        QuantityLimitMin     { get; set; }
			public int        QuantityLimitMax     { get; set; }

			// Constructor.
			public Rule() {
				this.SourceRoot           = new Node();
				this.ReplacementRoot      = new Node();
				this.SourceNodeTable      = new List<Node>();
				this.ReplacementNodeTable = new List<Node>();
			}
			// Find the node from source rule by index.
			public Node FindSourceByIndex(int index) {
				return SourceNodeTable.First(n => n.Index == index);
			}
			// Find the node from replacement rule by index.
			public Node FindReplacementByIndex(int index) {
                foreach (Node temp in ReplacementNodeTable) {
                    if (temp.Index == index) {
                        return temp;
                    }
                }
                return null;
            }
		}
		private static Dictionary<Node, int> nodeDictionary = new Dictionary<Node, int>();
		private static List<string> _usedEdge = new List<string>();
		private static VFlibcs.Graph TransToVFGraph(Node node) {
			nodeDictionary.Clear();
			_usedEdge.Clear();
			VFlibcs.Graph result = new VFlibcs.Graph();
			nodeDictionary.Add(node, result.InsertNode(node));
			InsertGraph(result, node);
			return result;
		}
		// DFS Insert node.
		private static void InsertGraph(VFlibcs.Graph graph, Node node) {
			foreach (Node child in node.Children) {
				if (child == null) { continue; }
				// If the node have not set then set it.
				if (!nodeDictionary.ContainsKey(child)) {
					nodeDictionary.Add(child, graph.InsertNode(child));
				}
				string stringEdge = nodeDictionary[node] + "+" + nodeDictionary[child];
				if (_usedEdge.Exists(x => x == stringEdge)) { continue; }
				// Set edge.
				graph.InsertEdge(nodeDictionary[node], nodeDictionary[child]);
				_usedEdge.Add(stringEdge);
				InsertGraph(graph, child);
			}
			foreach (var parent in node.Parents) {
				if (parent == null) { continue; }
				// If the node have not set then set it.
				if (!nodeDictionary.ContainsKey(parent)) {
					nodeDictionary.Add(parent, graph.InsertNode(parent));
				}
				string stringEdge = nodeDictionary[parent] + "+" + nodeDictionary[node];
				if (_usedEdge.Exists(x => x == stringEdge)) { continue; }
				// Set edge.
				graph.InsertEdge(nodeDictionary[parent], nodeDictionary[node]);
				_usedEdge.Add(stringEdge);
				InsertGraph(graph, parent);
			}
		}
	}
}

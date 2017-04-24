using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using System;

namespace MissionGrammarSystem {
	// Label of the validation system in mission rule.
	public enum ValidationLabel {
		LeftMoreThanRight,
		EmptyLeft,
		HeadHasParent,
		IsolatedNode,
		IsolatedConnection,
		ExactlyDuplicated,
		MultipleRelations,
		CyclicLink,
		OrphanNode
	}

	public class ValidationSystem {
		private static List<ValidationLabel> _error = new List<ValidationLabel>();
		// Validate the rule is legal or not.
		private static Dictionary<ValidationLabel, Func<MissionRule, GraphGrammar, bool>> _validationMethods = new Dictionary<ValidationLabel, Func<MissionRule, GraphGrammar, bool>>() {
			{ ValidationLabel.LeftMoreThanRight,  (MissionRule rule, GraphGrammar graphGrammar) => ValidateLeftMoreThanRight(rule, graphGrammar) },
			{ ValidationLabel.EmptyLeft,          (MissionRule rule, GraphGrammar graphGrammar) => ValidateEmptyLeft(rule, graphGrammar) },
			{ ValidationLabel.HeadHasParent,      (MissionRule rule, GraphGrammar graphGrammar) => ValidateHeadHasParent(rule, graphGrammar) },
			{ ValidationLabel.IsolatedNode,       (MissionRule rule, GraphGrammar graphGrammar) => ValidateIsolatedNode(rule, graphGrammar) },
			{ ValidationLabel.IsolatedConnection, (MissionRule rule, GraphGrammar graphGrammar) => ValidateIsolatedConnection(rule, graphGrammar) },
			{ ValidationLabel.ExactlyDuplicated,  (MissionRule rule, GraphGrammar graphGrammar) => ValidateExactlyDuplicated(rule, graphGrammar) },
			{ ValidationLabel.MultipleRelations,  (MissionRule rule, GraphGrammar graphGrammar) => ValidateMultipleRelations(rule, graphGrammar) },
			{ ValidationLabel.CyclicLink,         (MissionRule rule, GraphGrammar graphGrammar) => ValidateCyclicLink(rule, graphGrammar) },
			{ ValidationLabel.OrphanNode,         (MissionRule rule, GraphGrammar graphGrammar) => ValidateOrphanNode(rule, graphGrammar) },
		};

		// Validate the graph grammar (one of pair of rule).
		public static void Validate(MissionRule rule, GraphGrammar graphGrammar) {
			// Initial the error list.
			_error.Clear();
			// Execute each method.
			foreach (var method in _validationMethods) {
				if (! method.Value.Invoke(rule, graphGrammar)) {
					if (! _error.Contains(method.Key)) {
						_error.Add(method.Key);
					}
				} else {
					_error.Remove(method.Key);
				}
			}
			// Debug.
			Debug.Log(string.Join(", ", _error.Cast<ValidationLabel>().Select(v => v.ToString()).ToArray()));
		}
		// No 1. LeftMoreThanRight.
		private static bool ValidateLeftMoreThanRight(MissionRule rule, GraphGrammar graphGrammar) {
			if(rule.SourceRule.Nodes.Count > rule.ReplacementRule.Nodes.Count) {
				// Nodes of sourceRule are more than nodes of replacementRule.
				return false;
			}
			// Nodes of sourceRule are not more than nodes of replacementRule.
			return true;
		}
		// No 2. EmptyLeft.
		private static bool ValidateEmptyLeft(MissionRule rule, GraphGrammar graphGrammar) {
			if (rule.SourceRule.Nodes.Count == 0) {
				// There is no node in sourceRule.
				return false;
			}
			// There is at least one node in sourceRule.
			return true;
		}
		// No 3. HeadHasParent.
		private static bool ValidateHeadHasParent(MissionRule rule, GraphGrammar graphGrammar) {
			if (graphGrammar.Nodes.Count > 1) {
				foreach (GraphGrammarNode node in graphGrammar.Nodes) {
					if (node.Ordering == 1) {
						if (node.Parents.Count > 0) {
							// Head has parent.
							return false;
						}
					}
				}
			}
			// Head doesn't has parent.
			return true;
		}
		// No 4. IsolatedNode.
		private static bool ValidateIsolatedNode(MissionRule rule, GraphGrammar graphGrammar) {
			if (graphGrammar.Nodes.Count > 1) {
				if (graphGrammar.Connections.Count < graphGrammar.Nodes.Count - 1) {
					return false;
				}
				foreach (GraphGrammarNode node in graphGrammar.Nodes) {
					// Connection.StickOn will remain the last node it sticked on, so use position to inforce validation.
					if (! graphGrammar.Connections.Where(e => (e.StartPosition == node.Position || e.EndPosition == node.Position)).Any()) {
						return false;
					}
				}
			}
			return true;
		}
		// No 5. IsolatedConnection.
		private static bool ValidateIsolatedConnection(MissionRule rule, GraphGrammar graphGrammar) {

			return true;
		}
		// No 6. ExactlyDuplicated.
		private static bool ValidateExactlyDuplicated(MissionRule rule, GraphGrammar graphGrammar) {

			return true;
		}
		// No 7. MultipleRelations.
		private static bool ValidateMultipleRelations(MissionRule rule, GraphGrammar graphGrammar) {
			if (graphGrammar.Connections.Count > 1) {
				foreach (GraphGrammarConnection connection in graphGrammar.Connections) {
					if ((graphGrammar.Connections.Where(e => (e != connection &&
					(e.StartpointStickyOn == connection.StartpointStickyOn && e.EndpointStickyOn == connection.EndpointStickyOn) ||
					(e.StartpointStickyOn == connection.EndpointStickyOn && e.EndpointStickyOn == connection.StartpointStickyOn)))).Any()) {
						return false;
					}
				}
			}
			return true;
		}
		// No 8. CyclicLink.
		private static bool ValidateCyclicLink(MissionRule rule, GraphGrammar graphGrammar) {
			// Store the parents and children to avoid the repeat call method.
			Dictionary<GraphGrammarNode, List<GraphGrammarNode>> parentsTable = new Dictionary<GraphGrammarNode, List<GraphGrammarNode>>();
			Dictionary<GraphGrammarNode, List<GraphGrammarNode>> childrenTable = new Dictionary<GraphGrammarNode, List<GraphGrammarNode>>();
			foreach (var node in graphGrammar.Nodes) {
				parentsTable[node] = node.Parents;
				childrenTable[node] = node.Children;
			}
			// Kahn's Algorithm
			// Array that record the removed edges.
			bool[,] _usedEdge = new bool[graphGrammar.Nodes.Count, graphGrammar.Nodes.Count];
			// Non-indegree queue.
			Queue<GraphGrammarNode> nonIndegree = new Queue<GraphGrammarNode>();
			// Push non-indegree node to queue.
			foreach (var node in graphGrammar.Nodes.FindAll(x => parentsTable[x].Count == 0)) {
				nonIndegree.Enqueue(node);
			}
			// Bfs.
			while (nonIndegree.Count > 0) {
				// Pop.
				GraphGrammarNode popNode = nonIndegree.Dequeue();
				// Remove the edge between this node and children node.
				foreach (var childNode in childrenTable[popNode]) {
					// Remove edge.
					_usedEdge[popNode.Ordering - 1, childNode.Ordering - 1] = true;
					// Check this child if it is non-indegree or not.
					bool hasInput = false;
					foreach (var parentNode in parentsTable[childNode]) {
						if (! _usedEdge[parentNode.Ordering - 1, childNode.Ordering - 1]) {
							hasInput = true;
							break;
						}
					}
					// If it is non-indegree then push it.
					if (! hasInput) {
						nonIndegree.Enqueue(childNode);
					}
				}
			}
			// Return false when any edge exist. It represents that this is a cyclic link.
			foreach (var node in graphGrammar.Nodes) {
				foreach (var childNode in childrenTable[node]) {
					if (! _usedEdge[node.Ordering - 1, childNode.Ordering - 1]) {
						return false;
					}
				}
			}
			// Otherwise, this is not cyclic link.
			return true;
		}
		// No 9. OrphanNode.
		private static bool ValidateOrphanNode(MissionRule rule, GraphGrammar graphGrammar) {
			// If node has no parent, it is an orphan. (Exclude ordering 1)
			return (! graphGrammar.Nodes.Where(n => (n.Ordering != 1 && n.Parents.Count == 0)).Any());
		}
	}
}

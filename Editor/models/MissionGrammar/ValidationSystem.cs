using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
// Locales.
using Languages = LanguageManager;
// TESTCommit11
namespace MissionGrammarSystem {
	// Label of the validation system in mission rule.
	public enum ValidationLabel {
		LeftMoreThanRight,
		EmptyLeft,
		IsolatedNode,
		IsolatedConnection,
		ExactlyDuplicated,
		MultipleRelations,
		CyclicLink,
		OrphanNode,
		OverflowedAnyNode,
		NoError
	}

	public class ValidationSystem {
		private static ValidationLabel _error = new ValidationLabel();
		// Validate the rule is legal or not.
		private static Dictionary<ValidationLabel, Func<MissionRule, GraphGrammar, bool>> _validationMethods = new Dictionary<ValidationLabel, Func<MissionRule, GraphGrammar, bool>>() {
			{ ValidationLabel.LeftMoreThanRight,  (MissionRule rule, GraphGrammar graphGrammar) => ValidateLeftMoreThanRight(rule, graphGrammar) },
			{ ValidationLabel.EmptyLeft,          (MissionRule rule, GraphGrammar graphGrammar) => ValidateEmptyLeft(rule, graphGrammar) },
			{ ValidationLabel.IsolatedNode,       (MissionRule rule, GraphGrammar graphGrammar) => ValidateIsolatedNode(rule, graphGrammar) },
			{ ValidationLabel.IsolatedConnection, (MissionRule rule, GraphGrammar graphGrammar) => ValidateIsolatedConnection(rule, graphGrammar) },
			{ ValidationLabel.ExactlyDuplicated,  (MissionRule rule, GraphGrammar graphGrammar) => ValidateExactlyDuplicated(rule, graphGrammar) },
			{ ValidationLabel.MultipleRelations,  (MissionRule rule, GraphGrammar graphGrammar) => ValidateMultipleRelations(rule, graphGrammar) },
			{ ValidationLabel.CyclicLink,         (MissionRule rule, GraphGrammar graphGrammar) => ValidateCyclicLink(rule, graphGrammar) },
			{ ValidationLabel.OrphanNode,         (MissionRule rule, GraphGrammar graphGrammar) => ValidateOrphanNode(rule, graphGrammar) },
			{ ValidationLabel.OverflowedAnyNode,  (MissionRule rule, GraphGrammar graphGrammar) => ValidateOverflowedAnyNode(rule, graphGrammar) },
		};

		// Validate the graph grammar (one of pair of rule).
		public static KeyValuePair<ValidationLabel, string> Validate(MissionRule rule, GraphGrammar graphGrammar) {
			// Initial the error to none.
			_error = ValidationLabel.NoError;
			// Execute each method.
			foreach (var method in _validationMethods) {
				if (! method.Value.Invoke(rule, graphGrammar)) {
					_error = (method.Key);
					break;
				}
			}
			// Return Error Message.
			return new KeyValuePair<ValidationLabel, string>(_error, SelectErrorType(_error));
		}
		// No 1. LeftMoreThanRight.
		protected static bool ValidateLeftMoreThanRight(MissionRule rule, GraphGrammar graphGrammar) {
			// Are Nodes of sourceRule more than nodes of replacementRule?
			//return rule.SourceRule.Nodes.Count <= rule.ReplacementRule.Nodes.Count ? true : false;
			return false;
		}
		// No 2. EmptyLeft.
		protected static bool ValidateEmptyLeft(MissionRule rule, GraphGrammar graphGrammar) {
			// Is there no node in sourceRule?
			return rule.SourceRule.Nodes.Count != 0 ? true : false;
		}
		// No 3. IsolatedNode.
		private static bool ValidateIsolatedNode(MissionRule rule, GraphGrammar graphGrammar) {
			if (graphGrammar.Nodes.Count > 1) {
				if (graphGrammar.Connections.Count < graphGrammar.Nodes.Count - 1) {
					return false;
				}
				foreach (GraphGrammarNode node in graphGrammar.Nodes) {
					// Connection.StickOn will remain the last node it sticked on, so use position to inforce validation.
					if (! graphGrammar.Connections.Where(c => (c.StartpointStickyOn == node || c.EndpointStickyOn == node)).Any()) {
						return false;
					}
				}
			}
			return true;
		}
		// No 4. IsolatedConnection.
		private static bool ValidateIsolatedConnection(MissionRule rule, GraphGrammar graphGrammar) {
			return !(graphGrammar.Connections.Where(c => c.StartpointStickyOn == null || c.EndpointStickyOn == null).Any());
		}
		// No 5. ExactlyDuplicated.
		private static bool ValidateExactlyDuplicated(MissionRule rule, GraphGrammar graphGrammar) {
			// Check the number of connections & nodes first.
			if (rule.SourceRule.Nodes.Count       != rule.ReplacementRule.Nodes.Count ||
			    rule.SourceRule.Connections.Count != rule.ReplacementRule.Connections.Count) {
				return true;
			}
			// If find any difference in connections, then defined they are not isomorphic.
			foreach (var connectionA in rule.SourceRule.Connections) {
				if (! rule.ReplacementRule.Connections.Exists(connectionB => 
					connectionB.AlphabetID == connectionA.AlphabetID &&
					// Check the ordering they sticky on. If null, expresses zero.
					(connectionB.StartpointStickyOn == null ? 0 : connectionB.StartpointStickyOn.Ordering) == (connectionA.StartpointStickyOn == null ? 0 : connectionA.StartpointStickyOn.Ordering) &&
					(connectionB.EndpointStickyOn   == null ? 0 : connectionB.EndpointStickyOn.Ordering)   == (connectionA.EndpointStickyOn   == null ? 0 : connectionA.EndpointStickyOn.Ordering)
				)) {
					return true;
				}
			}
			// If find any difference in nodes, then defined they are not isomorphic.
			foreach (var nodeA in rule.SourceRule.Nodes) {
				if (! rule.ReplacementRule.Nodes.Exists(nodeB =>
					nodeB.AlphabetID     == nodeA.AlphabetID &&
					nodeB.Ordering       == nodeA.Ordering &&
					nodeB.Children.Count == nodeA.Children.Count &&
					nodeB.Parents.Count  == nodeA.Parents.Count
				)) {
					return true;
				}
			}
			// It's illegal and isomorphic.
			return false;
		}
		// No 6. MultipleRelations.
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
		// No 7. CyclicLink.
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
			foreach (var node in graphGrammar.Nodes.FindAll(x => parentsTable[x].Count == 0 &&
			// children can not be node itself. 
			(!childrenTable[x].Exists(p => p.Ordering == x.Ordering)))) {
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
		// No 8. OrphanNode.
		private static bool ValidateOrphanNode(MissionRule rule, GraphGrammar graphGrammar) {
			// If node has no parent, it is an orphan.
			// Indegree = 0 quantity cannot > 1.
			return (graphGrammar.Nodes.FindAll(n => n.Parents.Count == 0).Count == 1);
		}
		// No 9. OverflowedAnyNode.
		private static bool ValidateOverflowedAnyNode(MissionRule rule, GraphGrammar graphGrammar) {
			// Sort nodes in ordering.
			GraphGrammarNode[] sourceNodes = rule.SourceRule.Nodes.OrderBy(n => n.Ordering).ToArray();
			// if replaceRule have any node that dont match the source ordering.
			return !rule.ReplacementRule.Nodes.Exists(n => (Alphabet.IsAnyNode(n.AlphabetID) &&
			(n.Ordering > sourceNodes.Length ? true : !Alphabet.IsAnyNode(sourceNodes[n.Ordering - 1].AlphabetID))));
		}
		// Return Error message.
		public static string SelectErrorType(ValidationLabel errorLabel) {
			string result = Languages.GetText("MissionGraph-Validation-Succeed");
			switch (errorLabel) {
			case ValidationLabel.LeftMoreThanRight:
				result = Languages.GetText("MissionGraph-Validation-LeftMoreThanRight");
				break;
			case ValidationLabel.EmptyLeft:
				result = Languages.GetText("MissionGraph-Validation-EmptyLeft");
				break;
			case ValidationLabel.IsolatedNode:
				result = Languages.GetText("MissionGraph-Validation-IsolatedNode");
				break;
			case ValidationLabel.IsolatedConnection:
				result = Languages.GetText("MissionGraph-Validation-IsolatedConnection");
				break;
			case ValidationLabel.ExactlyDuplicated:
				result = Languages.GetText("MissionGraph-Validation-ExactlyDuplicated");
				break;
			case ValidationLabel.MultipleRelations:
				result = Languages.GetText("MissionGraph-Validation-MultipleRelations");
				break;
			case ValidationLabel.CyclicLink:
				result = Languages.GetText("MissionGraph-Validation-CyclicLink");
				break;
			case ValidationLabel.OrphanNode:
				result = Languages.GetText("MissionGraph-Validation-OrphanNode");
				break;
			case ValidationLabel.OverflowedAnyNode:
				result = Languages.GetText("MissionGraph-Validation-OverflowedAnyNode");
				break;
			}
			return result;
		}
	}
}

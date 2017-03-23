using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using EditorCanvas = EditorExtend.NodeCanvas;
using EditorStyle  = EditorExtend.Style;

namespace MissionGrammarSystem {
	public static class MissionGrammar {
		// Default mission group.
		private static List<MissionGroup> _groups = new List<MissionGroup>() { new MissionGroup() };

		// Groups, getter.
		public static List<MissionGroup> Groups {
			get { return _groups; }
		}
		// Get the specified mission group by name.
		public static MissionGroup Group(string name) {
			return _groups.Where(s => s.Name == name).FirstOrDefault();
		}
		// Add a mission group.
		public static void AddGroup(MissionGroup group) {
			_groups.Add(group);
			return;
		}
		// Remove the specified mission group.
		public static void RemoveGroup(MissionGroup group) {
			_groups.Remove(group);
			return;
		}
		// Remove the specified mission group by name.
		public static void RemoveGroup(string name) {
			_groups.Remove(_groups.Where(s => s.Name == name).FirstOrDefault());
			return;
		}
		// Same symbols in mission grammar will be updated in the same time when the alphabet updated.
		public static void OnAlphabetUpdated(GraphGrammarSymbol symbol) {
			GraphGrammarNode       referenceNode       = null;
			GraphGrammarConnection referenceConnection = null;
			if (symbol is GraphGrammarNode) {
				referenceNode = (GraphGrammarNode) symbol;
				foreach (var group in _groups) {
					foreach (var rule in group.Rules) {
						foreach (var node in rule.SourceRule.Nodes) {
							if (node.AlphabetID == referenceNode.AlphabetID) {
								node.UpdateSymbolInfo(referenceNode);
							}
						}
						foreach (var node in rule.ReplacementRule.Nodes) {
							if (node.AlphabetID == referenceNode.AlphabetID) {
								node.UpdateSymbolInfo(referenceNode);
							}
						}
					}
				}
			} else if (symbol is GraphGrammarConnection) {
				referenceConnection = (GraphGrammarConnection) symbol;
				foreach (var group in _groups) {
					foreach (var rule in group.Rules) {
						foreach (var connection in rule.SourceRule.Connections) {
							if (connection.AlphabetID == referenceConnection.AlphabetID) {
								connection.UpdateSymbolInfo(referenceConnection);
							}
						}
						foreach (var connection in rule.ReplacementRule.Connections) {
							if (connection.AlphabetID == referenceConnection.AlphabetID) {
								connection.UpdateSymbolInfo(referenceConnection);
							}
						}
					}
				}
			}
		}
	}
}
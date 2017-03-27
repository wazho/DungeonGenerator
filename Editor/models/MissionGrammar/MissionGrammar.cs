using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using EditorCanvas = EditorExtend.NodeCanvas;
using EditorStyle  = EditorExtend.Style;

namespace MissionGrammarSystem {
	public static class MissionGrammar {
		// Default mission group.
		private static List<MissionGroup> _groups = new List<MissionGroup>() { new MissionGroup() };

		// Groups, getter.
		public static List<MissionGroup> Groups {
			get { return _groups; }
			set { _groups = value; }
		}
		// Get the specified mission group by name.
		public static MissionGroup Group(string name) {
			return _groups.Where(s => s.Name == name).FirstOrDefault();
		}
		// Add a mission group by default.
		public static void AddGroup() {
			_groups.Add(new MissionGroup());
			return;
		}
		// Add a mission group by default.
		public static void AddGroup(string name, string description) {
			_groups.Add(new MissionGroup(name, description));
			return;
		}
		// Add a mission group from another group.
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
		// Return a boolean about name of group has never used before.
		public static bool IsGroupNameUsed(string newGroup) {
			return (from usedGroup in MissionGrammar.Groups
				where usedGroup.Name.ToLower() == newGroup.ToLower()
				select usedGroup)
				.Any();
		}
		// Return a boolean about name of rule has never used before.
		public static bool IsRuleNameUsed(string newRule, int groupIndex) {
			return (from usedRule in MissionGrammar.Groups[groupIndex].Rules
				where usedRule.Name.ToLower() == newRule.ToLower()
				select usedRule)
				.Any();
		}
		// Get the default group name.
		public static string GetDefaultGroupName(string[] groupsOptions) {
			// The member of all default name. 
			List<int> members = new List<int>();
			// The new default name.
			string newName = string.Empty;
			// Match the regular expression pattern against the name of groups.
			for (int _index = 0; _index < groupsOptions.Length; _index++) {
				Match match = new Regex(@"^New group (\d+)$", RegexOptions.IgnoreCase)
									.Match(MissionGrammar.Groups[_index].Name);
				while (match.Success) { members.Add(Int32.Parse(match.Groups[1].ToString())); }
			}
			// Sort the member of all the default name.
			members.Sort();
			// Find the empty number in sort.
			for (int i = 0; i < members.Count && newName != string.Empty; i++) {
				if (i + 1 != members[i]) { newName = "New group " + (i + 1); }
			}
			// If there is not the empty number in sort, create new number.
			if (newName == string.Empty) { newName = "New group " + (members.Count + 1); }
			return newName;
		}
		// Get the default rule name.
		public static string GetDefaultRuleName(string[] rulesOptions, int indexOfGroup) {
			// The member of all default name. 
			List<int> members = new List<int>();
			// The new default name.
			string newName = string.Empty;
			// Match the regular expression pattern against the name of rules.
			for (int i = 0; i < rulesOptions.Length; i++) {
				Match match = new Regex(@"^New rule (\d+)$", RegexOptions.IgnoreCase)
									.Match(MissionGrammar.Groups[indexOfGroup].Rules[i].Name);
				if (match.Success) { members.Add(Int32.Parse(match.Groups[1].ToString())); }
			}
			// Sort the member of all the default name.
			members.Sort();
			// Find the empty number in sort.
			for (int i = 0; i < members.Count && newName != string.Empty; i++) {
				if (i + 1 != members[i]) { newName = "New rule " + (i + 1); }
			}
			// If there is not the empty number in sort, create new number.
			if (newName == string.Empty) { newName = "New rule " + (members.Count + 1); }
			return newName;
		}
	}
}
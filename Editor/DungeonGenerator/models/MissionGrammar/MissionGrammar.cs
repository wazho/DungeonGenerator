using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System;
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
		// Add the default group name.
		public static string AddNewDefaultGroup(string[] groupsOptions) {
			string _pattern = @"^New group (\d+)$";
			// The member of all default name. 
			List<int> _member = new List<int>() { };
			// The new default name.
			string _newDefaultName = string.Empty;
			// Match the regular expression pattern against the name of groups.
			for (int _index = 0; _index < groupsOptions.Length; _index++) {
				Match _newGroup = new Regex(_pattern, RegexOptions.IgnoreCase).Match(MissionGrammar.Groups[_index].Name);
				while (_newGroup.Success) {
					Group _idOfGroupNeme = _newGroup.Groups[1];
					_member.Add(Int32.Parse(_idOfGroupNeme.ToString()));
					_newGroup = _newGroup.NextMatch();
				}
			}
			// Sort the member of all the default name.
			_member.Sort();
			// Find the empty number in sort.
			int _id = 0;
			while (_id < _member.Count) {
				if (_id > 0 && _member[_id] != _member[_id - 1] + 1) {
					_newDefaultName = "New group " + (_id + 1);
					break;
				} else if (_id == 0 && _member[_id] != 1) {
					_newDefaultName = "New group 1";
					break;
				}
				_id++;
			}
			// If there are not the empty number in sort, create new number.
			if (_newDefaultName == string.Empty) _newDefaultName = "New group " + (_member.Count + 1);

			return _newDefaultName;
		}
		// Add the default rule name.
		public static string AddNewDefaultRule(string[] rulesOptions, int indexOfGroup) {
			string _pattern = @"^New rule (\d+)$";
			// The member of all default name. 
			List<int> _member = new List<int>() { };
			// The new default name.
			string _newDefaultName = string.Empty;
			// Match the regular expression pattern against the name of rules.
			for (int _index = 0; _index < rulesOptions.Length; _index++) {
				Match _newRule = new Regex(_pattern, RegexOptions.IgnoreCase).Match(MissionGrammar.Groups[indexOfGroup].Rules[_index].Name);
				while (_newRule.Success) {
					Group _idOfRuleNeme = _newRule.Groups[1];
					_member.Add(Int32.Parse(_idOfRuleNeme.ToString()));
					_newRule = _newRule.NextMatch();
				}
			}
			// Sort the member of all the default name.
			_member.Sort();
			// Find the empty number in sort.
			int _id = 0;
			while (_id < _member.Count) {
				if (_id > 0 && _member[_id] != _member[_id - 1] + 1) {
					_newDefaultName = "New rule " + (_id + 1);
					break;
				} else if (_id == 0 && _member[_id] != 1) {
					_newDefaultName = "New rule 1";
					break;
				}
				_id++;
			}
			// If there are not the empty number in sort, create new number.
			if (_newDefaultName == string.Empty) _newDefaultName = "New rule " + (_member.Count + 1);

			return _newDefaultName;
		}
	}
}
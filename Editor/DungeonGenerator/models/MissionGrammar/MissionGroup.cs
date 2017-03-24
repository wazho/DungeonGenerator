using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

/*
	What is *mission group* in this file?

	Mission consists of multi mission rules and basic information.
 */

namespace MissionGrammarSystem {
	public class MissionGroup {
		private string            _name;
		private string            _description;
		private List<MissionRule> _rules;
		private bool			  _isSelected;
		// Constructor - Default.
		public MissionGroup() {
			this._name        = "New group";
			this._description = "Description here.";
			// Default mission rule.
			this._rules       = new List<MissionRule>() { new MissionRule() };
			this._isSelected  = false;
		}
		// Constructor - Pass name and description.
		// Constructor - Pass name and description.
		public MissionGroup(string name, string description) : this() {
			this._name        = name;
			this._description = description;
		}
		// Name, getter and setter.
		public string Name {
			get { return _name; }
			set { _name = value; }
		}
		// Description, getter and setter.
		public string Description {
			get { return _description; }
			set { _description = value; }
		}
		// MissionRules, getter.
		public List<MissionRule> Rules {
			get { return _rules; }
		}
		// Get the specified mission rule by name.
		public MissionRule Rule(string name) {
			return _rules.Where(r => r.Name == name).FirstOrDefault();
		}
		// Add a mission rule by default.
		public void AddRule() {
			_rules.Add(new MissionRule());
			return;
		}
		// Add a mission rule from basic info.
		public void AddRule(string name, string description) {
			_rules.Add(new MissionRule(name, description));
			return;
		}
		// Add a mission rule from another rule.
		public void AddRule(MissionRule rule) {
			_rules.Add(rule);
			return;
		}
		// Remove the specified mission rule.
		public void RemoveRule(MissionRule rule) {
			_rules.Remove(rule);
			return;
		}
		// Remove the specified mission rule by name.
		public void RemoveRule(string name) {
			_rules.Remove(_rules.Where(r => r.Name == name).FirstOrDefault());
			return;
		}
		// Enable, getter and setter.
		public bool Selected {
			get { return _isSelected; }
			set { _isSelected = value; }
		}
	}
}
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

		public MissionGroup() {
			this._name         = "New group";
			this._description  = "Description here.";
			// Default mission rule.
			this._rules        = new List<MissionRule>() { new MissionRule() };
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
		// Add a mission rule.
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
	}
}
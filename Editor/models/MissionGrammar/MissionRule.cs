using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

/*
    What is *mission rule* in this file?

    Mission consists of two graph grammars and basic information.
*/

namespace MissionGrammarSystem {
	public class MissionRule {
		private string       _name;
		private string       _description;
		private GraphGrammar _sourceRule;
		private GraphGrammar _replacementRule;
		private bool         _isEnabled;
		private int          _weight;
		// Constructor - Default.
		public MissionRule() {
			this._name            = "New rule";
			this._description     = "Description here.";
			this._sourceRule      = new GraphGrammar();
			this._replacementRule = new GraphGrammar();
			this._isEnabled       = true;
			this._weight          = 100;
		}
		// Constructor - Pass name and description.
		public MissionRule(string name, string description) : this() {
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
		// SourceRule, getter and setter.
		public GraphGrammar SourceRule {
			get { return _sourceRule; }
			set { _sourceRule = value; }
		}
		// ReplacementRule, getter and setter.
		public GraphGrammar ReplacementRule {
			get { return _replacementRule; }
			set { _replacementRule = value; }
		}
		// Enable, getter and setter.
		public bool Enable {
			get { return _isEnabled; }
			set { _isEnabled = value; }
		}
		// Weight, getter and setter.
		public int Weight {
			get { return _weight; }
			set {
				_weight = value;
				if (_weight < 0) {
					_weight = 0;
				}
			}
		}
	}
}
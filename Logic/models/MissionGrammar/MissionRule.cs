
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
		// [Addition] if error occur disabled button
		private bool         _isValid;
		private int          _weight;
		// Constructor - Default.
		public MissionRule() {
			this._name             = "New rule";
			this._description      = "Description here.";
			this._sourceRule       = new GraphGrammar();
			this._replacementRule  = new GraphGrammar();
			this._isEnabled        = false;
			this._isValid          = false;
			this._weight           = 10;
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
		// Validation, getter and setter.
		public bool Valid {
			get { return _isValid; }
			set { _isValid = value; }
		}
		// Quantity Limit.
		public int QuantityLimitMin { get; set; }
		public int QuantityLimitMax { get; set; }
		// Weight, getter and setter.
		public int Weight {
			get { return _weight; }
			set { _weight = (_weight <= 0) ? 1 : value; }
		}
	}
}
using Guid = System.Guid;

namespace MissionGrammarSystem {
	// Types of the symbol.
	public enum SymbolType {
		None,
		Node,
		Connection,
	}
	// This is the base structure of GraphGrammarNode and GraphGrammarConnection.
	public class GraphGrammarSymbol {
		// GUID for symbol.
		protected Guid       _symbolID;
		// Members.
		protected SymbolType _type;
		protected bool       _isSelect;
		protected int        _ordering;
		// Basic construction.
		protected GraphGrammarSymbol() {
			this._symbolID = Guid.NewGuid();
			this._type     = SymbolType.None;
			this._isSelect = false;
			this._ordering = 0;
		}
		// Return the ID.
		public Guid ID {
			get { return _symbolID; }
			set { _symbolID = value; }
		}
		// Return the symbol type.
		public SymbolType Type {
			get { return _type; }
		}
		// Selected, getter and setter.
		public bool Selected {
			get { return _isSelect; }
			set { _isSelect = value; }
		}
		// Ordering, getter and setter.
		public int Ordering {
			get { return _ordering; }
			set { _ordering = value; }
		}
	}
}
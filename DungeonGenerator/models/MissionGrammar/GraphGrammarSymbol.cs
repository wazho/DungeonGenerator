using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace MissionGrammar {
	// Types of the symbol.
	public enum SymbolType {
		None       = 0,
		Node       = 1,
		Connection = 2,
	}

	public class GraphGrammarSymbol {
		protected SymbolType _type;
		protected bool       _isSelect;
		protected int        _ordering;

		protected GraphGrammarSymbol() {
			this._type     = SymbolType.None;
			this._isSelect = false;
			this._ordering = 0;
		}

		public SymbolType Type {
			get { return _type; }
		}

		public bool Selected {
			get { return _isSelect; }
			set { _isSelect = value; }
		}

		public int Ordering {
			get { return _ordering; }
			set { _ordering = value; }
		}
	}
}
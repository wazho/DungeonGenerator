using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace MissionGrammar {
	// Types of connection.
	public enum ConnectionType {
		WeakRequirement   = 0,
		StrongRequirement = 1,
		Inhibition        = 2
	}
	// Types of connection arrow.
	public enum ConnectionArrowType {
		Normal     = 0,
		Double     = 1,
		WithCircle = 2
	}

	public class GraphGrammarConnection : GraphGrammarSymbol {
		// Values of setting.
		private int   _pointScopeSize  = 11;
		private float _lineThickness   = 5f;
		// Members.
		private bool _isStartSelected;
		private bool _isEndSelected;
		private Rect _startpointScope;
		private Rect _endpointScope;
		private GraphGrammarNode _startpointStickyOn;
		private GraphGrammarNode _endpointStickyOn;

		public GraphGrammarConnection() : base() {
			this._type            = SymbolType.Connection;
			this._startpointScope = new Rect( 10, 10, this._pointScopeSize, this._pointScopeSize);
			this._endpointScope   = new Rect(100, 10, this._pointScopeSize, this._pointScopeSize);
		}
		// .
		public int PointScopeSize {
			get { return _pointScopeSize; }
		}
		// .
		public float LineThickness {
			get { return _lineThickness; }
		}
		// .
		public bool StartSelected {
			get { return _isStartSelected; }
			set { _isStartSelected = value; }
		}
		// .
		public bool EndSelected {
			get { return _isEndSelected; }
			set { _isEndSelected = value; }
		}
		// .
		public Vector2 StartPosition {
			get { return _startpointScope.position; }
			set { _startpointScope.position = value; }
		}
		// .
		public Vector2 EndPosition {
			get { return _endpointScope.position; }
			set { _endpointScope.position = value; }
		}
		// .
		public GraphGrammarNode StartpointStickyOn {
			get { return _startpointStickyOn; }
			set { _startpointStickyOn = value; }
		}
		// .
		public GraphGrammarNode EndpointStickyOn {
			get { return _endpointStickyOn; }
			set { _endpointStickyOn = value; }
		}
		// .
		public bool IsInStartscope(Vector2 pos) {
			return _startpointScope.Contains(pos);
		}
		// .
		public bool IsInEndscope(Vector2 pos) {
			return _endpointScope.Contains(pos);
		}
	}
}
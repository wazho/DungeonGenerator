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
		private string              _name;
		private string              _description;
		private ConnectionType      _requirement;
		private ConnectionArrowType _arrow;
		private Rect                _startpointScope;
		private Rect                _endpointScope;
		private Color               _outlineColor;
		private GraphGrammarNode    _startpointStickyOn;
		private GraphGrammarNode    _endpointStickyOn;
		// Basic construction.
		public GraphGrammarConnection() : base() {
			this._type               = SymbolType.Connection;
			this._name               = "";
			this._description        = "";
			this._requirement        = ConnectionType.WeakRequirement;
			this._arrow              = ConnectionArrowType.Normal;
			this._startpointScope    = new Rect(  0,   0, this._pointScopeSize, this._pointScopeSize);
			this._endpointScope      = new Rect(100, 100, this._pointScopeSize, this._pointScopeSize);
			this._outlineColor       = Color.black;
			this._startpointStickyOn = null;
			this._endpointStickyOn   = null;
		}
		// Clone construction for basic informations.
		public GraphGrammarConnection(GraphGrammarConnection connection) {
			this._type               = SymbolType.Connection;
			this._name               = connection.Name;
			this._description        = connection.Description;
			this._requirement        = connection.Requirement;
			this._arrow              = connection.Arrow;
			this._startpointScope    = connection.StartpointScope;
			this._endpointScope      = connection.EndpointScope;
			this._outlineColor       = connection.OutlineColor;
			this._startpointStickyOn = null;
			this._endpointStickyOn   = null;

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
		// Requirement, getter and setter.
		public ConnectionType Requirement {
			get { return _requirement; }
			set { _requirement = value; }
		}
		// Requirement, getter and setter.
		public ConnectionArrowType Arrow {
			get { return _arrow; }
			set { _arrow = value; }
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
			get { return _startpointStickyOn != null; }
		}
		// .
		public bool EndSelected {
			get { return _endpointStickyOn != null; }
		}
		// .
		public Vector2 StartPosition {
			get { return _startpointScope.center; }
			set { _startpointScope.center = value; }
		}
		// .
		public Vector2 EndPosition {
			get { return _endpointScope.center; }
			set { _endpointScope.center = value; }
		}


		public float StartPositionX {
			get { return _startpointScope.center.x; }
			set { _startpointScope.x = _startpointScope.x - _startpointScope.center.x + value; }
		}
		public float StartPositionY {
			get { return _startpointScope.center.y; }
			set { _startpointScope.y = _startpointScope.y - _startpointScope.center.y + value; }
		}
		public float EndPositionX {
			get { return _endpointScope.center.x; }
			set { _endpointScope.x = _endpointScope.x - _endpointScope.center.x + value; }
		}
		public float EndPositionY {
			get { return _endpointScope.center.y; }
			set { _endpointScope.y = _endpointScope.y - _endpointScope.center.y + value; }
		}


		// Startpoint scope, getter.
		public Rect StartpointScope {
			get { return _startpointScope; }
		}
		// Endpoint scope, getter.
		public Rect EndpointScope {
			get { return _endpointScope; }
		}
		// Outline color, getter and setter.
		public Color OutlineColor {
			get { return _outlineColor; }
			set { _outlineColor = value; }
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
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Guid = System.Guid;

using EditorCanvas = EditorExtend.NodeCanvas;

namespace MissionGrammarSystem {
	// Types of connection.
	public enum ConnectionType {
		WeakRequirement,
		StrongRequirement,
		Inhibition,
	}
	// Types of connection arrow.
	public enum ConnectionArrowType {
		Normal,
		Double,
		WithCircle,
	}

	public class GraphGrammarConnection : GraphGrammarSymbol {
		// Values of setting.
		private int   _pointScopeSize  = 11;
		private float _lineThickness   = 5f;
		// GUID for this symbol in alphabet.
		private Guid                _alphabetID;
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
		private bool                _startSelected;
		private bool                _endSelected;
		// Basic construction.
		public GraphGrammarConnection() : base() {
			this._type               = SymbolType.Connection;
			this._name               = string.Empty;
			this._description        = string.Empty;
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
		// Return the ID.
		public Guid AlphabetID {
			get { return _alphabetID; }
			set { _alphabetID = value; }
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
			get { return _startSelected; }
			set { _startSelected = value; }
		}
		// .
		public bool EndSelected {
			get { return _endSelected; }
			set { _endSelected = value; }
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
		// Update the information form another connection, mostly reference is in Alphabet.
		public void UpdateSymbolInfo(GraphGrammarConnection referenceConnection) {
			_name         = referenceConnection.Name;
			_description  = referenceConnection.Description;
			_outlineColor = referenceConnection.OutlineColor;
			_requirement  = referenceConnection.Requirement;
			_arrow        = referenceConnection.Arrow;
		}
		// Draw the connection on canvas.
		public void Draw() {
			// Draw the main line about connection.
			EditorCanvas.DrawLine(StartPosition, EndPosition, OutlineColor, LineThickness);
			// Head size
			Vector2 dir = (EndPosition - StartPosition).normalized * LineThickness;
			Vector2 orthoptics = new Vector2(-dir.y, dir.x);
			// Arrow cap's points
			Vector3[] arrowHead = new Vector3[3];
			arrowHead[0] = EndPosition - dir * 2 + orthoptics;
			arrowHead[1] = EndPosition - dir * 2 - orthoptics;
			arrowHead[2] = EndPosition; 
			Vector3[] arrowHeadSec = new Vector3[3];
			Vector2 dir2 = dir = dir + dir * 1f;
			arrowHeadSec[0] = EndPosition - dir2 * 2 + orthoptics;
			arrowHeadSec[1] = EndPosition - dir2 * 2 - orthoptics;
			arrowHeadSec[2] = EndPosition - dir2;
			// Draw the arraw part.
			switch (this.Arrow) {
			case ConnectionArrowType.Normal:
				EditorCanvas.DrawTriangle(arrowHead, OutlineColor);
				break;
			case ConnectionArrowType.Double:
				EditorCanvas.DrawTriangle(arrowHead, OutlineColor);
				EditorCanvas.DrawTriangle(arrowHeadSec, OutlineColor);
				break;
			case ConnectionArrowType.WithCircle:
				EditorCanvas.DrawTriangle(arrowHead, OutlineColor);
				EditorCanvas.DrawDisc(EndPosition - dir2 / 2f, LineThickness, OutlineColor);
				EditorCanvas.DrawDisc(EndPosition - dir2 / 2f, LineThickness - 1, Color.white);
				break;
			}
			// Endpoints of conneciton.
			if (Selected) {
				EditorCanvas.DrawQuad(StartpointScope, Color.red);
				EditorCanvas.DrawQuad(EndpointScope, Color.blue);
			}
		}
	}
}
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Guid = System.Guid;

using EditorCanvas = EditorExtend.NodeCanvas;

namespace SpaceGrammarSystem {
	// Types of edge.
	public enum EdgeType {
		WeakRequirement,
		StrongRequirement,
		Inhibition,
	}
	// Types of edge arrow.
	public enum EdgeArrowType {
		Normal,
		Double,
		WithCircle,
	}

	public class SpaceGrammarEdge : SpaceGrammarSymbol {
		// Values of setting.
		private int _pointScopeSize = 11;
		private float _lineThickness = 5f;
		// GUID for this symbol in alphabet.
		private Guid _alphabetID;
		// Members.
		private string _name;
		private string _description;
		private EdgeType _requirement;
		private EdgeArrowType _arrow;
		private Rect _startpointScope;
		private Rect _endpointScope;
		private Color _outlineColor;
		private bool _startSelected;
		private bool _endSelected;
		// Basic construction.
		public SpaceGrammarEdge() : base() {
			this._alphabetID = Guid.NewGuid();
			this._type = SymbolType.Edge;
			this._name = string.Empty;
			this._description = string.Empty;
			this._requirement = EdgeType.WeakRequirement;
			this._arrow = EdgeArrowType.Normal;
			this._startpointScope = new Rect(0, 0, this._pointScopeSize, this._pointScopeSize);
			this._endpointScope = new Rect(100, 100, this._pointScopeSize, this._pointScopeSize);
			this._outlineColor = Color.black;
		}
		// Basic construction.
		public SpaceGrammarEdge(string name, string description, EdgeType requirement, EdgeArrowType arrowType) : this() {
			this._name = name;
			this._description = description;
			this._requirement = requirement;
			this._arrow = arrowType;
		}
		// Clone construction for basic informations.
		public SpaceGrammarEdge(SpaceGrammarEdge edge) {
			// Generate new symbol ID, but use same alphabet ID.
			this._symbolID = Guid.NewGuid();
			this._alphabetID = edge.AlphabetID;
			// Basic information to copy.
			this._type = SymbolType.Edge;
			this._name = edge.Name;
			this._description = edge.Description;
			this._requirement = edge.Requirement;
			this._arrow = edge.Arrow;
			this._startpointScope = edge.StartpointScope;
			this._endpointScope = edge.EndpointScope;
			this._outlineColor = edge.OutlineColor;

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
		public EdgeType Requirement {
			get { return _requirement; }
			set { _requirement = value; }
		}
		// Requirement, getter and setter.
		public EdgeArrowType Arrow {
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
			set { _startpointScope = value; }
		}
		// Endpoint scope, getter.
		public Rect EndpointScope {
			get { return _endpointScope; }
			set { _endpointScope = value; }
		}
		// Outline color, getter and setter.
		public Color OutlineColor {
			get { return _outlineColor; }
			set { _outlineColor = value; }
		}		
		// Update the information form another edge, mostly reference is in Alphabet.
		public void UpdateSymbolInfo(SpaceGrammarEdge referenceEdge) {
			_name = referenceEdge.Name;
			_description = referenceEdge.Description;
			_outlineColor = referenceEdge.OutlineColor;
			_requirement = referenceEdge.Requirement;
			_arrow = referenceEdge.Arrow;
		}
		// Draw the edge on canvas.
		public void Draw() {
			// Draw the main line about edge.
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
				case EdgeArrowType.Normal:
					EditorCanvas.DrawTriangle(arrowHead, OutlineColor);
					break;
				case EdgeArrowType.Double:
					EditorCanvas.DrawTriangle(arrowHead, OutlineColor);
					EditorCanvas.DrawTriangle(arrowHeadSec, OutlineColor);
					break;
				case EdgeArrowType.WithCircle:
					EditorCanvas.DrawTriangle(arrowHeadSec, OutlineColor);
					EditorCanvas.DrawDisc(EndPosition - dir2 / 2f, LineThickness, OutlineColor);
					EditorCanvas.DrawDisc(EndPosition - dir2 / 2f, LineThickness - 1, Color.white);
					break;
			}
		}
	}
}
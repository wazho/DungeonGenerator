using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Guid = System.Guid;

using EditorCanvas = EditorExtend.NodeCanvas;

namespace SpaceGrammarSystem {
	public class SpaceGrammarArea : SpaceGrammarSymbol {
		// Values of setting.
		private const int _thickness = 2;
		// GUID for this symbol in alphabet.
		private Guid _alphabetID;
		// Members.
		private string _name;
		private string _abbreviation;
		private string _description;
		private Rect _outlineScope;
		private Rect _filledScope;
		private Rect _textScope;
		private Color _outlineColor;
		private Color _filledColor;
		private Color _textColor;

		private Vector3[] _vertexPosition;
		// Construction
		public SpaceGrammarArea() : base() {
			this._alphabetID = Guid.NewGuid();
			this._type = SymbolType.Area;
			this._name = string.Empty;
			this._abbreviation = string.Empty;
			this._description = string.Empty;
			// Square vertex
			this.VertexPosition = new Vector3[] {
				new Vector2(0, 0),
				new Vector2(100, 0),
				new Vector2(100, 100),
				new Vector2(0, 100)
			};
			this._textScope = new Rect(0, 0, 45, 34);
			this._outlineColor = Color.black;
			this._filledColor = Color.white;
			this._textColor = Color.black;
		}
		// Basic construction.
		public SpaceGrammarArea(string name, string abbreviation, string description) : this() {
			this._name = name;
			this._abbreviation = abbreviation;
			this._description = description;
		}
		// Clone construction for basic informations.
		public SpaceGrammarArea(SpaceGrammarArea area) {
			// Generate new symbol ID, but use same alphabet ID.
			this._symbolID = Guid.NewGuid();
			this._alphabetID = area.AlphabetID;
			// Basic information to copy.
			this._type = SymbolType.Area;
			this._name = area.Name;
			this._abbreviation = area.Abbreviation;
			this._description = area.Description;
			this._outlineScope = area.OutlineScope;
			this._filledScope = area.FilledScope;
			this._textScope = area.TextScope;
			this._outlineColor = area.OutlineColor;
			this._filledColor = area.FilledColor;
			this._textColor = area.TextColor;
		}
		// Return the ID.
		public Guid AlphabetID {
			get { return _alphabetID; }
			set { _alphabetID = value; }
		}
		// ExpressName, getter.
		public string ExpressName {
			get { return _name + " (" + _abbreviation + ")"; }
		}
		// Name, getter and setter.
		public string Name {
			get { return _name; }
			set { _name = value; }
		}
		// Abbreviation, getter and setter.
		public string Abbreviation {
			get { return _abbreviation; }
			set { _abbreviation = value; }
		}
		// Description, getter and setter.
		public string Description {
			get { return _description; }
			set { _description = value; }
		}
		// Position, getter and setter.
		public Vector2 Position {
			get { return _filledScope.center; }
			set {
				// Update center positions.
				_outlineScope.center = value;
				_filledScope.center = value;
				_textScope.center = value;
			}
		}
		public float PositionX {
			get { return _filledScope.center.x; }
			set {
				// Update center positions.
				_outlineScope.x = _outlineScope.x - _outlineScope.center.x + value;
				_filledScope.x = _filledScope.x - _filledScope.center.x + value;
				_textScope.x = _textScope.x - _textScope.center.x + value;
			}
		}
		public float PositionY {
			get { return _filledScope.center.y; }
			set {
				// Update center positions.
				_outlineScope.y = _outlineScope.y - _outlineScope.center.y + value;
				_filledScope.y = _filledScope.y - _filledScope.center.y + value;
				_textScope.y = _textScope.y - _textScope.center.y + value;
			}
		}
		// Outline scope, getter.
		public Rect OutlineScope {
			get { return _outlineScope; }
			set { _outlineScope = value; }
		}
		// Filled scope, getter.
		public Rect FilledScope {
			get { return _filledScope; }
			set { _filledScope = value; }
		}
		// Text scope, getter.
		public Rect TextScope {
			get { return _textScope; }
			set { _textScope = value; }
		}
		// Outline color, getter and setter.
		public Color OutlineColor {
			get { return _outlineColor; }
			set { _outlineColor = value; }
		}
		// Filled color, getter and setter.
		public Color FilledColor {
			get { return _filledColor; }
			set { _filledColor = value; }
		}
		// Text color, getter and setter.
		public Color TextColor {
			get { return _textColor; }
			set { _textColor = value; }
		}
		// Area points, getter and setter.
		public Vector3[] VertexPosition {
			get { return _vertexPosition; }
			set {
				_vertexPosition = value;
				resizeScope();
			}
		}
		private void resizeScope() {
			Vector2 pos = _filledScope.position;
			Vector2 scopeRange = new Vector2(0, 0);
			foreach(var item in _vertexPosition) {
				if(item.x > scopeRange.x) {
					scopeRange.x = item.x;
				}
				if(item.y > scopeRange.y) {
					scopeRange.y = item.y;
				}
			}
			_filledScope = new Rect(pos.x + _thickness, pos.y + _thickness, scopeRange.x + _thickness, scopeRange.y + _thickness);
			_outlineScope = new Rect(pos.x, pos.y, scopeRange.x + _thickness * 2, scopeRange.y + _thickness * 2);
		}
		// Update the information form another node, mostly reference is in Alphabet.
		public void UpdateSymbolInfo(SpaceGrammarArea referenceArea) {
			_name = referenceArea.Name;
			_abbreviation = referenceArea.Abbreviation;
			_description = referenceArea.Description;
			_outlineColor = referenceArea.OutlineColor;
			_filledColor = referenceArea.FilledColor;
			_textColor = referenceArea.TextColor;
		}
		// Return the position is contained in this symbol or not.
		public bool IsInScope(Vector2 pos) {
			return _filledScope.Contains(pos);
		}
		// Draw the node on canvas.
		public void Draw() {
			EditorCanvas.DrawConvexPolygon(_filledScope.position, _vertexPosition, _filledColor, _outlineColor, _thickness);
		}
	}
}
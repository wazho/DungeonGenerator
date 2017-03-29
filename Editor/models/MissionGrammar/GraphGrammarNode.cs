using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Guid = System.Guid;

using EditorCanvas = EditorExtend.NodeCanvas;

namespace MissionGrammarSystem {
	// Terminal types of node.
	public enum NodeTerminalType {
		NonTerminal,
		Terminal,
	}
	/*
		This structure is based on the GraphGrammarSymbol.

		1. Basic information of symbol:
		    _name, _abbreviation, _description

		2. Exclusive information of node:
		    _terminal
		    _stickiedConnections (Store the stickied connections)

		3. Shape to draw:
		    _outlineScope, _filledScope, _textScope

		4. Color to draw:
		    _outlineColor, _filledColor, _textColor

	 */
	public class GraphGrammarNode : GraphGrammarSymbol {
		// Values of setting.
		private const int _thickness = 2;
		// GUID for this symbol in alphabet.
		private Guid                     _alphabetID;
		// Members.
		private string                   _name;
		private string                   _abbreviation;
		private string                   _description;
		private NodeTerminalType         _terminal;
		private Rect                     _outlineScope;
		private Rect                     _filledScope;
		private Rect                     _textScope;
		private Color                    _outlineColor;
		private Color                    _filledColor;
		private Color                    _textColor;
		private List<StickiedConnection> _stickiedConnections;
		// Construction
		public GraphGrammarNode() : base() {
			this._alphabetID          = Guid.NewGuid();
			this._type                = SymbolType.Node;
			this._name                = string.Empty;
			this._abbreviation        = string.Empty;
			this._description         = string.Empty;
			this._terminal            = NodeTerminalType.Terminal;
			this._outlineScope        = new Rect(0, 0, 39, 39);
			this._filledScope         = new Rect(2, 2, 35, 35);
			this._textScope           = new Rect(0, 0, 45, 34);
			this._outlineColor        = Color.black;
			this._filledColor         = Color.white;
			this._textColor           = Color.black;
			this._stickiedConnections = new List<StickiedConnection>();
		}
		// Basic construction.
		public GraphGrammarNode(NodeTerminalType terminal) : this() {
			this._terminal = terminal;
		}
		// Basic construction.
		public GraphGrammarNode(string name, string abbreviation, string description, NodeTerminalType terminal) : this() {
			this._name         = name;
			this._abbreviation = abbreviation;
			this._description  = description;
			this._terminal     = terminal;
		}
		// Clone construction for basic informations.
		public GraphGrammarNode(GraphGrammarNode node) {
			// Generate new symbol ID, but use same alphabet ID.
			this._symbolID            = Guid.NewGuid();
			this._alphabetID          = node.AlphabetID;
			// Basic information to copy.
			this._type                = SymbolType.Node;
			this._name                = node.Name;
			this._abbreviation        = node.Abbreviation;
			this._description         = node.Description;
			this._terminal            = node.Terminal;
			this._outlineScope        = node.OutlineScope;
			this._filledScope         = node.FilledScope;
			this._textScope           = node.TextScope;
			this._outlineColor        = node.OutlineColor;
			this._filledColor         = node.FilledColor;
			this._textColor           = node.TextColor;
			this._stickiedConnections = new List<StickiedConnection>();
		}
		// Get stickiedConnections. ID array.
		public Guid[] StickiedConnectionsGuid {
			get {
				Guid[] guids = new Guid[_stickiedConnections.Count];
				for (int i = 0; i < _stickiedConnections.Count; i++) {
					guids[i] = _stickiedConnections[i].connection.ID;
				}
				return guids;
			}
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
		// Terminal, getter and setter.
		public NodeTerminalType Terminal {
			get { return _terminal; }
			set { _terminal = value; }
		}
		// Position, getter and setter.
		public Vector2 Position {
			get { return _filledScope.center; }
			set {
				// Update center positions.
				_outlineScope.center = value;
				_filledScope.center  = value;
				_textScope.center    = value;
				// Move the stickied connections together.
				foreach (StickiedConnection stickied in _stickiedConnections) {
					switch (stickied.location) {
					case "start":
						stickied.connection.StartPosition = value;
						break;
					case "end":
						stickied.connection.EndPosition = value;
						break;
					}
				}
			}
		}
		public float PositionX {
			get { return _filledScope.center.x; }
			set {
				// Update center positions.
				_outlineScope.x = _outlineScope.x - _outlineScope.center.x + value;
				_filledScope.x  = _filledScope.x  - _filledScope.center.x  + value;
				_textScope.x    = _textScope.x    - _textScope.center.x    + value;
			}
		}
		public float PositionY {
			get { return _filledScope.center.y; }
			set {
				// Update center positions.
				_outlineScope.y = _outlineScope.y - _outlineScope.center.y + value;
				_filledScope.y  = _filledScope.y  - _filledScope.center.y  + value;
				_textScope.y    = _textScope.y    - _textScope.center.y    + value;
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
		// Parents, getter.
		public List<GraphGrammarNode> Parents {
			get {
				List<GraphGrammarNode> parents = new List<GraphGrammarNode>();
				foreach (StickiedConnection stickiedConnection in _stickiedConnections) {
					if (stickiedConnection.location.Equals("end")) {
						parents.Add(stickiedConnection.connection.StartpointStickyOn);
					}
				}
				return parents;
			}
		}
		// Children, getter.
		public List<GraphGrammarNode> Children {
			get {
				List<GraphGrammarNode> children = new List<GraphGrammarNode>();
				foreach (StickiedConnection stickiedConnection in _stickiedConnections) {
					if (stickiedConnection.location.Equals("start")) {
						children.Add(stickiedConnection.connection.EndpointStickyOn);
					}
				}
				return children;
			}
		}
		// Update the information form another node, mostly reference is in Alphabet.
		public void UpdateSymbolInfo(GraphGrammarNode referenceNode) {
			_terminal     = referenceNode.Terminal;
			_name         = referenceNode.Name;
			_abbreviation = referenceNode.Abbreviation;
			_description  = referenceNode.Description;
			_outlineColor = referenceNode.OutlineColor;
			_filledColor  = referenceNode.FilledColor;
			_textColor    = referenceNode.TextColor;
		}
		// Add a new pair for stickied connection.
		public void AddStickiedConnection(GraphGrammarConnection connection, string location) {
			// If connection is not store here, append this connection.
			if (! _stickiedConnections.Any(e => e.connection == connection)) {
				_stickiedConnections.Add(new StickiedConnection(connection, location));
			}
		}
		// Remove the specific stickied connection.
		public void RemoveStickiedConnection(GraphGrammarConnection connection, string location) {
			// If connection is store here, remove this connection.
			_stickiedConnections.Remove(_stickiedConnections.Find(e => e.connection == connection && e.location == location));
		}
		// Return the position is contained in this symbol or not.
		public bool IsInScope(Vector2 pos) {
			return _filledScope.Contains(pos);
		}
		// Draw the node on canvas.
		public void Draw() {
			switch (Terminal) {
			case NodeTerminalType.NonTerminal:
				// Highlighting.
				if (Selected) {
					EditorCanvas.DrawQuad(new Rect(OutlineScope.x - _thickness, (int) OutlineScope.y - _thickness, OutlineScope.width + _thickness * 2, OutlineScope.height + _thickness * 2), Color.red);
				}
				// Main part of node.
				EditorCanvas.DrawQuad(OutlineScope, OutlineColor);
				EditorCanvas.DrawQuad(FilledScope, FilledColor);
				EditorCanvas.DrawQuad(TextScope, Color.clear, "<" + Ordering + ">\n" + Abbreviation, TextColor);
				break;
			case NodeTerminalType.Terminal:
				// Highlighting.
				if (Selected) {
					EditorCanvas.DrawDisc(Position, OutlineScope.width / 2 + _thickness, Color.red);
				}
				// Main part of node.
				EditorCanvas.DrawDisc(Position, OutlineScope.width / 2, OutlineColor);
				EditorCanvas.DrawDisc(Position, OutlineScope.width / 2 - _thickness, FilledColor);
				EditorCanvas.DrawQuad(TextScope, Color.clear, "<" + Ordering + ">\n" + Abbreviation, TextColor);
				break;
			}
		}
		// Sub-classes.
		private class StickiedConnection {
			public GraphGrammarConnection connection;
			public string                 location;
			// Construction.
			public StickiedConnection(GraphGrammarConnection connection, string location) {
				this.connection = connection;
				this.location   = location;
			}
		}
	}
}
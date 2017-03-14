using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace MissionGrammar {
	// Terminal types of node.
	public enum NodeTerminalType {
		NonTerminal = 0,
		Terminal    = 1
	}

	public class GraphGrammarNode : GraphGrammarSymbol {
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
		// Construction (private).
		private GraphGrammarNode() : base() {
			this._type                = SymbolType.Node;
			this._name                = "";
			this._abbreviation        = "";
			this._description         = "";
			this._terminal            = NodeTerminalType.Terminal;
			this._outlineScope        = new Rect(0, 0, 39, 39);
			this._filledScope         = new Rect(2, 2, 35, 35);
			this._textScope           = new Rect(0, 0, 35, 21);
			this._outlineColor        = Color.black;
			this._filledColor         = Color.white;
			this._textColor           = Color.black;
			this._stickiedConnections = new List<StickiedConnection>();
		}
		// Basic construction.
		public GraphGrammarNode(NodeTerminalType terminal) : this() {
			this._terminal = terminal;
		}
		// Clone construction for basic informations.
		public GraphGrammarNode(GraphGrammarNode node) {
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
		// ExpressName, getter and setter.
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
				// // Move the stickied connections together.
				// foreach (StickiedConnection stickied in _stickiedConnections) {
				// 	switch (stickied.location) {
				// 	case "start":
				// 		stickied.connection.StartPosition = value;
				// 		break;
				// 	case "end":
				// 		stickied.connection.EndPosition = value;
				// 		break;
				// 	}
				// }
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
		// Outline scope, getter and setter.
		public Rect OutlineScope {
			get { return _outlineScope; }
		}
		// Filled scope, getter and setter.
		public Rect FilledScope {
			get { return _filledScope; }
		}
		// Text scope, getter and setter.
		public Rect TextScope {
			get { return _textScope; }
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
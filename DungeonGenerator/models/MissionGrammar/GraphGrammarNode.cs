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
		private NodeTerminalType         _terminal;
		private Rect                     _outlineScope;
		private Rect                     _filledScope;
		private Rect                     _textScope;
		private List<StickiedConnection> _stickiedConnections;
		private Color                    _outlineColor;
		private Color                    _filledColor;
		private Color                    _textColor;
		// Construction (private).
		private GraphGrammarNode() : base() {
			this._type                = SymbolType.Node;
			this._name                = "";
			this._abbreviation        = "";
			this._terminal            = NodeTerminalType.Terminal;
			this._outlineScope        = new Rect(0, 0, 39, 39);
			this._filledScope         = new Rect(2, 2, 35, 35);
			this._textScope           = new Rect(0, 0, 35, 21);
			this._stickiedConnections = new List<StickiedConnection>();
			this._outlineColor        = Color.black;
			this._filledColor         = Color.white;
			this._textColor           = Color.black;
		}
		// Construction.
		public GraphGrammarNode(NodeTerminalType terminal) : this() {
			this._terminal = terminal;
		}
		// Name, getter and setter.
		public string Name {
			get { return _name; }
			set { _name = value; }
		}
		public string Abbreviation {
			get { return _abbreviation; }
			set { _abbreviation = value; }
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
		public Rect OutlineScope {
			get { return _outlineScope; }
		}
		public Rect FilledScope {
			get { return _filledScope; }
		}
		public Rect TextScope {
			get { return _textScope; }
		}
		public Color OutlineColor {
			get { return _outlineColor; }
			set { _outlineColor = value; }
		}
		public Color FilledColor {
			get { return _filledColor; }
			set { _filledColor = value; }
		}
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
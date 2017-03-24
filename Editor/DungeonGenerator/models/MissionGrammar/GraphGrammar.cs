using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using EditorCanvas = EditorExtend.NodeCanvas;

namespace MissionGrammarSystem {
	public class GraphGrammar {
		private List<GraphGrammarNode>       _nodes;
		private List<GraphGrammarConnection> _connections;
		private GraphGrammarSymbol           _selectedSymbol;

		public GraphGrammar() {
			this._nodes          = new List<GraphGrammarNode>();
			this._connections    = new List<GraphGrammarConnection>();
			this._selectedSymbol = null;
		}

		// Nodes, getter and setter.
		public List<GraphGrammarNode> Nodes {
			get { return _nodes; }
			set { _nodes = value; }
		}
		// Connections, getter and setter.
		public List<GraphGrammarConnection> Connections {
			get { return _connections; }
			set { _connections = value; }
		}
		// SelectedSymbol, getter and setter.
		// This is the parent class, it can be assigned by GraphGrammarNode of GraphGrammarConnection.
		public GraphGrammarSymbol SelectedSymbol {
			get { return _selectedSymbol; }
			set { _selectedSymbol = value; }
		}

		// Pass the mouse position.
		// Be careful for one thing. '_nodes' and '_connections' must pre-order by ordering.
		public void TouchedSymbol(Vector2 pos) {
			// Find the 'points of selected connection'.
			foreach (GraphGrammarConnection symbol in _connections.AsEnumerable().Reverse()) {
				// This connection is selected, check its endpoints are in scope or not.
				if (symbol.Selected) {
					// Initial all symbol first.
					RevokeAllSelected();
					// Update the symbol status. If out of selecting, disabled the connection.
					if (symbol.IsInStartscope(pos)) {
						symbol.Selected = true;
						symbol.StartSelected = true;
					} else if (symbol.IsInEndscope(pos)) {
						symbol.Selected = true;
						symbol.EndSelected = true;
					} else {
						break;
					}
					// Update selected symbol.
					_selectedSymbol = symbol;
					return;
				}
			}
			// Find the 'selected nodes'.
			foreach (GraphGrammarNode symbol in _nodes.AsEnumerable().Reverse()) {
				if (symbol.IsInScope(pos)) {
					// Initial all symbol first.
					RevokeAllSelected();
					// Update the symbol status.
					symbol.Selected = true;
					// Update selected symbol.
					_selectedSymbol = symbol;
					return;
				}
			}
			// Find the 'selected connections'.
			foreach (GraphGrammarConnection symbol in _connections.AsEnumerable().Reverse()) {
				float distance = HandleUtility.DistancePointLine((Vector3) pos, (Vector3) symbol.StartPosition, (Vector3) symbol.EndPosition);
				if (distance < symbol.LineThickness) {
					// Initial all symbol first.
					RevokeAllSelected();
					// Update the symbol status.
					symbol.Selected = true;
					// Update selected symbol.
					_selectedSymbol = symbol;
					return;
				}
			}
			// If anything has been found or not.
			RevokeAllSelected();
			_selectedSymbol = null;
		}

		// Points of connection is sticky to the node.
		public void StickyNode(GraphGrammarConnection connection, Vector2 pos, string location) {
			foreach (GraphGrammarNode node in _nodes.AsEnumerable().Reverse()) {
				if (node.IsInScope(pos)) {
					if (string.Equals(location, "start")) {
						connection.StartpointStickyOn = node;
						connection.StartPosition = node.Position;
					} else {
						connection.EndpointStickyOn = node;
						connection.EndPosition = node.Position;
					}
					node.AddStickiedConnection(connection, location);
					return;
				} else {
					node.RemoveStickiedConnection(connection, location);
				}
			}
		}

		// Add a new node.
		public void AddNode() {
			// Revoke all symbols first.
			RevokeAllSelected();
			// Create a new node and update its ordering and selected status.
			GraphGrammarNode node = new GraphGrammarNode(NodeTerminalType.NonTerminal);
			node.Ordering = _nodes.Count + 1;
			node.Selected = true;
			_nodes.Add(node);
			// Update the current node.
			_selectedSymbol = node;
		}
		// Add a new node from another exist node.
		public void AddNode(GraphGrammarNode nodeClone) {
			RevokeAllSelected();
			// Deep copy.
			GraphGrammarNode node = new GraphGrammarNode(nodeClone);
			node.Ordering = _nodes.Count + 1;
			node.Selected = true;
			_nodes.Add(node);
			_selectedSymbol = node;
		}
		// Update symbol appearance.
		public void UpdateSymbol(GraphGrammarSymbol before, GraphGrammarSymbol after) {
			int symbolIndex                   = -1;
			GraphGrammarNode       node       = null;
			GraphGrammarConnection connection = null;
			if (before is GraphGrammarNode) {
				node = (GraphGrammarNode) after;
				symbolIndex = _nodes.FindIndex(x => x.Equals(before));
				_nodes[symbolIndex].Terminal     = node.Terminal;
				_nodes[symbolIndex].Name         = node.Name;
				_nodes[symbolIndex].Abbreviation = node.Abbreviation;
				_nodes[symbolIndex].Description  = node.Description;
				_nodes[symbolIndex].OutlineColor = node.OutlineColor;
				_nodes[symbolIndex].FilledColor  = node.FilledColor;
				_nodes[symbolIndex].TextColor    = node.TextColor;
			} else if (before is GraphGrammarConnection) {
				connection = (GraphGrammarConnection) after;
				symbolIndex = _connections.FindIndex(x => x.Equals(before));
				_connections[symbolIndex].Name         = connection.Name;
				_connections[symbolIndex].Description  = connection.Description;
				_connections[symbolIndex].OutlineColor = connection.OutlineColor;
				_connections[symbolIndex].Requirement  = connection.Requirement;
				_connections[symbolIndex].Arrow        = connection.Arrow;
			}
		}
		// Add a new connection.
		public void AddConnection() {
			// Revoke all symbols first.
			RevokeAllSelected();
			// Create a new connection.
			GraphGrammarConnection connection = new GraphGrammarConnection();
			connection.Selected = true;
			_connections.Add(connection);
			// Update the current connection.
			_selectedSymbol = connection;
		}
		// Add a new connection from another exist connection.
		public void AddConnection(GraphGrammarConnection connectionClone) {
			RevokeAllSelected();
			// Deep copy.
			GraphGrammarConnection connection = new GraphGrammarConnection(connectionClone);
			connection.Selected = true;
			_connections.Add(connection);
			_selectedSymbol = connection;
		}
		// Set all 'seleted' of symbols to false.
		public void RevokeAllSelected() {
			foreach (var symbol in _nodes) {
				symbol.Selected = false;
			}
			foreach (var symbol in _connections) {
				symbol.Selected = symbol.StartSelected = symbol.EndSelected = false;
			}
			_selectedSymbol = null;
			return;
		}
	}
}
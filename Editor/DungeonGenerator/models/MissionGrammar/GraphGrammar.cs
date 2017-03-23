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
			if (before is GraphGrammarNode) {
				GraphGrammarNode node = (GraphGrammarNode) after;
				int symbolIndex = _nodes.FindIndex(x => x.Equals(before));
				Debug.Log(symbolIndex + "/" + _nodes.Count);
				_nodes[symbolIndex].Terminal     = node.Terminal;
				_nodes[symbolIndex].Name         = node.Name;
				_nodes[symbolIndex].Abbreviation = node.Abbreviation;
				_nodes[symbolIndex].Description  = node.Description;
				_nodes[symbolIndex].OutlineColor = node.OutlineColor;
				_nodes[symbolIndex].FilledColor  = node.FilledColor;
				_nodes[symbolIndex].TextColor    = node.TextColor;
			} else if (before is GraphGrammarConnection) {
				GraphGrammarConnection connection = (GraphGrammarConnection) after;
				int symbolIndex = _connections.FindIndex(x => x.Equals(before));
				// [TODO] Will modify
				_connections[symbolIndex].Requirement  = connection.Requirement;
				_connections[symbolIndex].Name         = connection.Name;
				_connections[symbolIndex].Description  = connection.Description;
				_connections[symbolIndex].OutlineColor = connection.OutlineColor;
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
			foreach (GraphGrammarNode symbol in _nodes) {
				symbol.Selected = false;
			}
			foreach (GraphGrammarConnection symbol in _connections) {
				symbol.Selected = symbol.StartSelected = symbol.EndSelected = false;
			}
			_selectedSymbol = null;
			return;
		}
		
		// [Draw on canvas] Draw the node on canvas.
		public static void DrawNode(GraphGrammarNode node) {
			int thickness = 2;

			switch (node.Terminal) {
			case NodeTerminalType.NonTerminal:
				if (node.Selected) {
					EditorCanvas.DrawQuad(new Rect(node.OutlineScope.x - thickness, (int) node.OutlineScope.y - thickness, node.OutlineScope.width + thickness * 2, node.OutlineScope.height + thickness * 2), Color.red);
				}
				EditorCanvas.DrawQuad(node.OutlineScope, node.OutlineColor);
				EditorCanvas.DrawQuad(node.FilledScope, node.FilledColor);
				EditorCanvas.DrawQuad(node.TextScope, Color.clear, node.Abbreviation, node.TextColor);
				break;
			case NodeTerminalType.Terminal:
				if (node.Selected) {
					EditorCanvas.DrawDisc(node.Position, 20 + thickness, Color.red);
				}
				EditorCanvas.DrawDisc(node.Position, 20, node.OutlineColor);
				EditorCanvas.DrawDisc(node.Position, 18, node.FilledColor);
				EditorCanvas.DrawQuad(node.TextScope, Color.clear, node.Abbreviation, node.TextColor);
				break;
			}
		}

		// [Draw on canvas] Draw the connection on canvas.
		public static void DrawConnection(GraphGrammarConnection connection) {
			// Setting of endpoints and line.
			int pBorder      = 2;
			float lThickness = connection.LineThickness;

			// Line between two points.
			EditorCanvas.DrawLine(connection.StartPosition, connection.EndPosition, connection.OutlineColor, lThickness);

			// Head size
			Vector2 dir = (connection.EndPosition - connection.StartPosition).normalized * 5f;
			Vector2 orth = new Vector2(-dir.y, dir.x);
			// Arrow cap's points
			Vector3[] arrowHead = new Vector3[3];
			arrowHead [0] = connection.EndPosition - dir * 2 + orth; 
			arrowHead [1] = connection.EndPosition - dir * 2 - orth; 
			arrowHead [2] = connection.EndPosition; 
			Vector3[] arrowHeadSec = new Vector3[3];
			Vector2 dir2 = dir = dir + dir * 1f;
			arrowHeadSec [0] = connection.EndPosition - dir2 * 2 + orth;
			arrowHeadSec [1] = connection.EndPosition - dir2 * 2 - orth;
			arrowHeadSec [2] = connection.EndPosition - dir2;
			
			switch (connection.Arrow) {
			case ConnectionArrowType.Normal:
				Handles.color = connection.OutlineColor;
				Handles.DrawAAConvexPolygon (arrowHead);	
				break;
			case ConnectionArrowType.Double:
				Handles.color = connection.OutlineColor;
				Handles.DrawAAConvexPolygon (arrowHead);
				Handles.DrawAAConvexPolygon (arrowHeadSec);
				break;
			case ConnectionArrowType.WithCircle:
				Handles.DrawAAConvexPolygon (arrowHeadSec);
				EditorCanvas.DrawDisc(connection.EndPosition - dir2 / 2f, 5f, connection.OutlineColor);
				EditorCanvas.DrawDisc(connection.EndPosition - dir2 / 2f, 4f, Color.white);
				break;
			}

			// Basic square and boarder.
			if (connection.Selected) {
				EditorCanvas.DrawQuad((int) connection.StartpointScope.x - pBorder, (int) connection.StartpointScope.y - pBorder, (int) connection.StartpointScope.width + pBorder*2, (int) connection.StartpointScope.height + pBorder*2, Color.black);
				EditorCanvas.DrawQuad((int) connection.StartpointScope.x, (int) connection.StartpointScope.y, (int) connection.StartpointScope.width, (int) connection.StartpointScope.height, Color.red);
				EditorCanvas.DrawQuad((int) connection.EndpointScope.x - pBorder, (int) connection.EndpointScope.y - pBorder, (int) connection.EndpointScope.width + pBorder * 2, (int) connection.EndpointScope.height + pBorder * 2, Color.black);
				EditorCanvas.DrawQuad((int) connection.EndpointScope.x, (int) connection.EndpointScope.y, (int) connection.EndpointScope.width, (int) connection.EndpointScope.height, Color.blue);
			}
		}
	}
}
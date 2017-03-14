using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using EditorCanvas = EditorExtend.NodeCanvas;

namespace MissionGrammar {
	public class GraphGrammar {
		private List<GraphGrammarNode>       nodes;
		private List<GraphGrammarConnection> connections;
		private GraphGrammarSymbol           selectedSymbol;

		public GraphGrammar() {
			this.nodes          = new List<GraphGrammarNode>();
			this.connections    = new List<GraphGrammarConnection>();
			this.selectedSymbol = null;
		}

		public List<GraphGrammarNode> Nodes {
			get { return this.nodes; }
			set { this.nodes = value; }
		}

		public List<GraphGrammarConnection> Connections {
			get { return this.connections; }
			set { this.connections = value; }
		}

		public GraphGrammarSymbol SelectedSymbol {
			get { return this.selectedSymbol; }
			set { this.selectedSymbol = value; }
		}

		// Pass the mouse position.
		// Be careful for one thing. 'this.nodes' and 'this.connections' must pre-order by ordering.
		public void TouchedSymbol(Vector2 pos) {
			// Find the 'points of selected connection'.
			foreach (GraphGrammarConnection symbol in this.connections.AsEnumerable().Reverse()) {
				// This connection is selected, check its endpoints are in scope or not.
				if (symbol.Selected) {
					// Initial all symbol first.
					this.RevokeAllSelected();
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
					this.selectedSymbol = symbol;
					return;
				}
			}
			// Find the 'selected nodes'.
			foreach (GraphGrammarNode symbol in this.nodes.AsEnumerable().Reverse()) {
				if (symbol.IsInScope(pos)) {
					// Initial all symbol first.
					this.RevokeAllSelected();
					// Update the symbol status.
					symbol.Selected = true;
					// Update selected symbol.
					this.selectedSymbol = symbol;
					return;
				}
			}
			// Find the 'selected connections'.
			foreach (GraphGrammarConnection symbol in this.connections.AsEnumerable().Reverse()) {
				float distance = HandleUtility.DistancePointLine((Vector3) pos, (Vector3) symbol.StartPosition, (Vector3) symbol.EndPosition);
				if (distance < symbol.LineThickness) {
					// Initial all symbol first.
					this.RevokeAllSelected();
					// Update the symbol status.
					symbol.Selected = true;
					// Update selected symbol.
					this.selectedSymbol = symbol;
					return;
				}
			}
			// If anything has been found or not.
			this.RevokeAllSelected();
			this.selectedSymbol = null;
		}

		// Points of connection is sticky to the node.
		public void StickyNode(GraphGrammarConnection connection, Vector2 pos, string location) {
			foreach (GraphGrammarNode node in this.nodes.AsEnumerable().Reverse()) {
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
			this.RevokeAllSelected();
			// Create a new node and update its ordering and selected status.
			GraphGrammarNode node = new GraphGrammarNode(NodeTerminalType.NonTerminal);
			node.Ordering = this.nodes.Count + 1;
			node.Selected = true;
			this.nodes.Add(node);
			// Update the current node.
			this.selectedSymbol = node;
		}
		// Add a new connection.
		public void AddConnection() {
			// Revoke all symbols first.
			this.RevokeAllSelected();
			// Create a new connection.
			GraphGrammarConnection connection = new GraphGrammarConnection();
			connection.Selected = true;
			this.connections.Add(connection);
			// Update the current connection.
			this.selectedSymbol = connection;
		}

		// Set all 'seleted' of symbols to false.
		public void RevokeAllSelected() {
			foreach (GraphGrammarNode symbol in this.nodes) {
				symbol.Selected = false;
			}
			foreach (GraphGrammarConnection symbol in this.connections) {
				symbol.Selected = false;
				symbol.StartSelected = symbol.EndSelected = false;
			}
			return;
		}
		
		// [Draw on canvas] Draw the node on canvas.
		public static void DrawNode(Vector2 pos, bool isTerminal, bool isSelected) {
			int thickness = 2;
			int size = 35;
			// If this node is activated (selected), add the highlight boarder.
			if (isSelected) {
				EditorCanvas.DrawQuad((int) pos[0]-thickness, (int) pos[1]-thickness, size+thickness*4, size+thickness*4, Color.red);
			}
			// Basic square and boarder.
			EditorCanvas.DrawQuad((int) pos[0], (int) pos[1], size+thickness*2, size+thickness*2, Color.black);
			EditorCanvas.DrawQuad((int) pos[0]+thickness, (int) pos[1]+thickness, size, size, isTerminal ? Color.green : Color.yellow);
		}
		// [Draw on canvas] Draw the connection on canvas.
		public static void DrawConnection(GraphGrammarConnection connection) {
			// Setting of endpoints and line.
			int pBorder      = 2;
			int pSize        = connection.PointScopeSize;
			float lThickness = connection.LineThickness;
			Vector2 posA     = connection.StartPosition;
			Vector2 posB     = connection.EndPosition;
			//Vector2 offset   = new Vector2(pSize/2, pSize/2);

			// Line between two points.
			// EditorCanvas.DrawLine(posA + offset, posB + offset, Color.white);
			EditorCanvas.DrawLine(posA, posB, Color.white, lThickness);

			// Basic square and boarder.
			if (connection.Selected) {
				EditorCanvas.DrawQuad((int) posA[0], (int) posA[1], pSize+pBorder*2, pSize+pBorder*2, Color.black);
				EditorCanvas.DrawQuad((int) posA[0]+pBorder, (int) posA[1]+pBorder, pSize, pSize, Color.red);
				EditorCanvas.DrawQuad((int) posB[0], (int) posB[1], pSize+pBorder*2, pSize+pBorder*2, Color.black);
				EditorCanvas.DrawQuad((int) posB[0]+pBorder, (int) posB[1]+pBorder, pSize, pSize, Color.blue);
			}
		}
	}
}
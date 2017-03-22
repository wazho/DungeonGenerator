using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using EditorCanvas = EditorExtend.NodeCanvas;
using EditorStyle  = EditorExtend.Style;

namespace MissionGrammarSystem {
	public static class Alphabet {
		// Default nodes in alphabet.
		private static List<GraphGrammarNode> _nodes = new List<GraphGrammarNode>() {
				new GraphGrammarNode("none",     "none", "System protected.", NodeTerminalType.Terminal),
				new GraphGrammarNode("entrance", "en",   "System protected.", NodeTerminalType.Terminal),
				new GraphGrammarNode("goal",     "go",   "System protected.", NodeTerminalType.Terminal),
			};
		private static List<GraphGrammarConnection> _connections = new List<GraphGrammarConnection>();

		// Node list in alphabet.
		public static List<GraphGrammarNode> Nodes {
			get { return _nodes; }
			set { _nodes = value; }
		}
		// Connection list in alphabet.
		public static List<GraphGrammarConnection> Connections {
			get { return _connections; }
			set { _connections = value; }
		}
		// Return the first selected node.
		public static GraphGrammarNode SelectedNode {
			get { return _nodes.Where(n => n.Selected == true).FirstOrDefault(); }
		}
		// Return the first selected node.
		public static GraphGrammarConnection SelectedConnection {
			get { return _connections.Where(c => c.Selected == true).FirstOrDefault(); }
		}
		// Add a new node.
		public static void AddNode(GraphGrammarNode node) {
			_nodes.Add(node);
		}
		// Add a new connection.
		public static void AddConnection(GraphGrammarConnection connection) {
			_connections.Add(connection);
		}
		// Remove one node.
		public static void RemoveNode(GraphGrammarNode node) {
			_nodes.Remove(node);
		}
		// Remove one connection.
		public static void RemoveConnection(GraphGrammarConnection connection) {
			_connections.Remove(connection);
		}
		// Return a boolean when it's name never be used in alphabet.
		public static bool IsNodeNameUsed(GraphGrammarNode currentNode) {
			if (currentNode == null) { return false; }
			return (from node in Alphabet.Nodes
				where node.Name == currentNode.Name && node != currentNode
				select node)
				.Any();
		}
		// Return a boolean about it's abbreviation never be used in alphabet.
		public static bool IsNodeAbbreviationUsed(GraphGrammarNode currentNode) {
			if (currentNode == null) { return false; }
			return (from node in Alphabet.Nodes
				where node.Abbreviation == currentNode.Abbreviation && node != currentNode
				select node)
				.Any();
		}
		// Remove all nodes.
		public static void ClearAllNodes() {
			// Create a new node.
			_nodes.Clear();
		}
		// Set all 'seleted' of symbols to false.
		public static void RevokeAllSelected() {
			foreach (GraphGrammarNode node in _nodes) {
				node.Selected = false;
			}
			foreach (GraphGrammarConnection connection in _connections) {
				connection.Selected = false;
			}
		}
		// Draw the node on canvas.
		public static void DrawNode(GraphGrammarNode node) {
			switch (node.Terminal) {
			case NodeTerminalType.NonTerminal:
				EditorCanvas.DrawQuad(node.OutlineScope, node.OutlineColor);
				EditorCanvas.DrawQuad(node.FilledScope, node.FilledColor);
				EditorCanvas.DrawQuad(node.TextScope, Color.clear, node.Abbreviation, node.TextColor);
				break;
			case NodeTerminalType.Terminal:
				EditorCanvas.DrawDisc(node.Position, 20, node.OutlineColor);
				EditorCanvas.DrawDisc(node.Position, 18, node.FilledColor);
				EditorCanvas.DrawQuad(node.TextScope, Color.clear, node.Abbreviation, node.TextColor);
				break;
			}
		}
		// Draw the node in the node list.
		public static void DrawNodeInList(GraphGrammarNode node) {
			node.PositionX = 30;
			node.PositionY = 25 + 50 * _nodes.FindIndex(n => n == node);
			// Background color of selectable area.
			EditorCanvas.DrawQuad(new Rect(5, node.PositionY - 23, Screen.width - 8, 46), node.Selected ? new Color(0.75f, 0.75f, 1, 0.75f) : Color.clear);
			// Draw this node.
			DrawNode(node);
		}
		// Draw the connection on canvas.
		// [TODO] Task C2-2-2. (you can remove these lines)
		// This part need to fix. The arrow is static not dynamic with rotate.
		public static void DrawConnection(GraphGrammarConnection connection) {
			EditorCanvas.DrawLine(connection.StartPosition, connection.EndPosition, connection.OutlineColor, 5f);
			/*
			Vector3[] arrowHead = new Vector3[3];
			arrowHead[0] = new Vector3(connection.EndPositionX +  0, connection.EndPositionY - 10, 0);
			arrowHead[1] = new Vector3(connection.EndPositionX +  0, connection.EndPositionY + 10, 0);
			arrowHead[2] = new Vector3(connection.EndPositionX + 15, connection.EndPositionY +  0, 0);
			Handles.color = connection.OutlineColor;
			Handles.DrawAAConvexPolygon(arrowHead);
			*/
		}
		// Draw the connection in the connection list.
		public static void DrawConnectionInList(GraphGrammarConnection connection) {
			connection.StartPositionX = 10;
			connection.EndPositionX   = 60;
			connection.StartPositionY = connection.EndPositionY = 25 + 50 * _connections.FindIndex(c => c == connection);
			// Background color of selectable area.
			EditorCanvas.DrawQuad(new Rect(5, connection.StartPositionY - 23, Screen.width - 8, 46), connection.Selected ? new Color(0.75f, 0.75f, 1, 0.75f) : Color.clear);
			// Draw this connection.
			DrawConnection(connection);
		}
	}
}
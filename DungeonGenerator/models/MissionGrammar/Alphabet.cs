using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using EditorCanvas = EditorExtend.NodeCanvas;
using EditorStyle  = EditorExtend.Style;

namespace MissionGrammar {
	public static class Alphabet {
		private static List<GraphGrammarNode>       _nodes       = new List<GraphGrammarNode>();
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
		// Add a new node.
		public static void AddNode(GraphGrammarNode node) {
			_nodes.Add(node);
		}
		// Remove one node.
		public static void RemoveNode(GraphGrammarNode node) {
			_nodes.Remove(node);
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
			EditorCanvas.DrawQuad(node.OutlineScope, node.OutlineColor);
			EditorCanvas.DrawQuad(node.FilledScope, node.FilledColor);
			EditorCanvas.DrawQuad(node.TextScope, Color.clear, node.Abbreviation, node.TextColor);
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
	}
}
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

using EditorCanvas = EditorExtend.NodeCanvas;

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
		// Add a new node.
		public static void AddNode(GraphGrammarNode node) {
			// Create a new node.
			_nodes.Add(node);
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
			EditorCanvas.DrawQuad(node.TextScope, Color.clear, node.Abbreviation);
		}
	}
}
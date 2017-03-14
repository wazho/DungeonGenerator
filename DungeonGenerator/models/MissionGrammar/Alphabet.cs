using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

using EditorCanvas = EditorExtend.NodeCanvas;

namespace MissionGrammar {
	public static class Alphabet {
		private static List<GraphGrammarNode>       _nodes       = new List<GraphGrammarNode>();
		private static List<GraphGrammarConnection> _connections = new List<GraphGrammarConnection>();
		private static Vector2 offset = new Vector2(2, 2);

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
		// Draw the node on canvas.
		public static void DrawNode(GraphGrammarNode node) {
			EditorCanvas.DrawQuad(node.OutlineScope, node.OutlineColor);
			EditorCanvas.DrawQuad(node.FilledScope, node.FilledColor);
			EditorCanvas.DrawQuad(node.TextScope, Color.clear, node.Abbreviation);
		}
	}
}
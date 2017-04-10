using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Guid = System.Guid;

using EditorCanvas = EditorExtend.NodeCanvas;
using EditorStyle  = EditorExtend.Style;

namespace MissionGrammarSystem {
	public static class Alphabet {
		// Default nodes in alphabet.
		private static List<GraphGrammarNode> _nodes = new List<GraphGrammarNode>() {
				new GraphGrammarNode("any",      "?",    "System default.", NodeTerminalType.Terminal),
				new GraphGrammarNode("none",     "none", "System default.", NodeTerminalType.Terminal),
				new GraphGrammarNode("entrance", "en",   "System default.", NodeTerminalType.Terminal),
				new GraphGrammarNode("goal",     "go",   "System default.", NodeTerminalType.Terminal),
			};
		// Default connections in alphabet.
		private static List<GraphGrammarConnection> _connections = new List<GraphGrammarConnection>() {
				new GraphGrammarConnection("Weak requirement",   "System default.", ConnectionType.WeakRequirement,   ConnectionArrowType.Normal),
				new GraphGrammarConnection("Strong requirement", "System default.", ConnectionType.StrongRequirement, ConnectionArrowType.Double),
				new GraphGrammarConnection("Inhibition",         "System default.", ConnectionType.Inhibition,        ConnectionArrowType.WithCircle),
			};
		// Default settings.
		private static GraphGrammarNode _anyNode      = _nodes[0];
		private static GraphGrammarNode _startingNode = _nodes[1];
		private static GraphGrammarNode _defaultNode  = _nodes[1];
		private static ConnectionType   _defaultConnectionType;
		// Default "Any" node.
		public static GraphGrammarNode AnyNode {
			get { return _anyNode; }
			set { _anyNode = value; }
		}
		// Starting node in alphabet.
		public static GraphGrammarNode StartingNode {
			get { return _startingNode; }
			set { _startingNode = value; }
		}
		// Default node in alphabet.
		public static GraphGrammarNode DefaultNode {
			get { return _defaultNode; }
			set { _defaultNode = value; }
		}
		// Default connection type in alphabet.
		public static ConnectionType DefaultConnectionType {
			get { return _defaultConnectionType; }
			set { _defaultConnectionType = value; }
		}
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
		// Initialization.
		public static void Initial() {
			_nodes = new List<GraphGrammarNode>() {
				new GraphGrammarNode("any",      "?",    "System default.", NodeTerminalType.Terminal),
				new GraphGrammarNode("none",     "none", "System default.", NodeTerminalType.Terminal),
				new GraphGrammarNode("entrance", "en",   "System default.", NodeTerminalType.Terminal),
				new GraphGrammarNode("goal",     "go",   "System default.", NodeTerminalType.Terminal),
			};
			_connections = new List<GraphGrammarConnection>() {
				new GraphGrammarConnection("Weak requirement",   "System default.", ConnectionType.WeakRequirement,   ConnectionArrowType.Normal),
				new GraphGrammarConnection("Strong requirement", "System default.", ConnectionType.StrongRequirement, ConnectionArrowType.Double),
				new GraphGrammarConnection("Inhibition",         "System default.", ConnectionType.Inhibition,        ConnectionArrowType.WithCircle),
			};
			_anyNode      = _nodes[0];
			_startingNode = _nodes[1];
			_defaultNode  = _nodes[1];
		}
		// Get Node table that refer to Guid.
		public static Dictionary<System.Guid,GraphGrammarNode> ReferenceNodeTable {
			get {
				Dictionary<System.Guid, GraphGrammarNode> table = new Dictionary<System.Guid, GraphGrammarNode>();
				foreach (GraphGrammarNode node in _nodes) {
					table[node.AlphabetID] = node;
				}
				return table;
			}
		}
		// Get Connection table that refer to Guid.
		public static Dictionary<System.Guid, GraphGrammarConnection> ReferenceConnectionTable {
			get {
				Dictionary<System.Guid, GraphGrammarConnection> table = new Dictionary<System.Guid, GraphGrammarConnection>();
				foreach (GraphGrammarConnection connection in _connections) {
					table[connection.AlphabetID] = connection;
				}
				return table;
			}
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
				where node.Name == currentNode.Name && node != Alphabet.SelectedNode
				select node)
				.Any();
		}
		// Return a boolean about it's abbreviation never be used in alphabet.
		public static bool IsNodeAbbreviationUsed(GraphGrammarNode currentNode) {
			if (currentNode == null) { return false; }
			return (from node in Alphabet.Nodes
				where node.Abbreviation == currentNode.Abbreviation && node != Alphabet.SelectedNode
				select node)
				.Any();
		}
		// Return a boolean about pass the node is "Any Node".
		public static bool IsAnyNode(Guid alphabetID) {
			return (_anyNode.AlphabetID == alphabetID);
		}
		// Return a boolean when it's name never be used in alphabet.
		public static bool IsConnectionNameUsed(GraphGrammarConnection currentConnection) {
			return (from connection in Alphabet.Connections
				where connection.Name == currentConnection.Name && connection != Alphabet.SelectedConnection
				select connection)
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
		// Draw the node in the node list.
		public static void DrawNodeInList(GraphGrammarNode node) {
			node.PositionX = 30;
			node.PositionY = 25 + 50 * _nodes.FindIndex(n => n == node);
			// Background color of selectable area.
			EditorCanvas.DrawQuad(new Rect(5, node.PositionY - 23, Screen.width - 8, 46), node.Selected ? new Color(0.75f, 0.75f, 1, 0.75f) : Color.clear);
			// Draw this node.
			node.Draw();
		}
		// Draw the connection in the connection list.
		public static void DrawConnectionInList(GraphGrammarConnection connection) {
			connection.StartPositionX = 10;
			connection.EndPositionX   = 60;
			connection.StartPositionY = connection.EndPositionY = 25 + 50 * _connections.FindIndex(c => c == connection);
			// Background color of selectable area.
			EditorCanvas.DrawQuad(new Rect(5, connection.StartPositionY - 23, Screen.width - 8, 46), connection.Selected ? new Color(0.75f, 0.75f, 1, 0.75f) : Color.clear);
			// Draw this connection.
			connection.Draw();
		}
	}
}
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

// Options of symbol type.
public enum EnumSymbolType {
	None       = 0,
	Node       = 1,
	Connection = 2,
}

public class GraphGrammarSymbol {
	protected EnumSymbolType type;
	protected bool           selected;
	protected int            ordering;

	protected GraphGrammarSymbol() {
		this.type     = EnumSymbolType.None;
		this.selected = false;
		this.ordering = 0;
	}

	public EnumSymbolType Type {
		get { return this.type; }
		set { ; }
	}

	public bool Selected {
		get { return this.selected; }
		set { this.selected = value; }
	}

	public int Ordering {
		get { return this.ordering; }
		set { Debug.Log(value); this.ordering = value; }
	}
}

// Options of symbol terminal.
public enum EnumNodeTerminal {
	Terminal    = 0,
	NonTerminal = 1,
}

public class GraphGrammarNode : GraphGrammarSymbol {
	protected EnumNodeTerminal terminal;
	protected Rect             scope;

	protected GraphGrammarNode() : base() {
		this.type     = EnumSymbolType.Node;
		this.terminal = EnumNodeTerminal.Terminal;
		this.scope    = new Rect(100, 100, 50, 50);
	}

	public GraphGrammarNode(EnumNodeTerminal terminal) : this() {
		this.terminal = terminal;
	}

	public EnumNodeTerminal Terminal {
		get { return this.terminal; }
		set { this.terminal = value; }
	}

	public Vector2 Position {
		get { return this.scope.position; }
		set { this.scope.position = value; }
	}

	// Return the position is contained in this symbol or not.
	public bool IsInScope(Vector2 pos) {
		return this.scope.Contains(pos);
	}
}

public class GraphGrammarConnection : GraphGrammarSymbol {
	protected Vector2 startPosition, endPosition;

	public GraphGrammarConnection() : base() {
		this.type          = EnumSymbolType.Connection;
		this.startPosition = new Vector2(10, 10);
		this.endPosition   = new Vector2(100, 10);
	}

	public Vector2 StartPosition {
		get { return this.startPosition; }
		set { this.startPosition = value; }
	}

	public Vector2 EndPosition {
		get { return this.endPosition; }
		set { this.endPosition = value; }
	}
}

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
	public void TouchedSymbol(Vector2 pos) {
		bool selected;
		List<GraphGrammarSymbol> selectedSymbols = new List<GraphGrammarSymbol>();

		// Initial all symbol first.
		this.RevokeAllSelected();
		// Find the 'selected nodes', then filtering the top one of them.
		foreach (GraphGrammarNode symbol in this.nodes) {
			selected = symbol.IsInScope(pos) ? true : false;
			if (selected) { selectedSymbols.Add(symbol); }
		}
		// Find the 'selected connections', then filtering the top one of them.
		foreach (GraphGrammarConnection symbol in this.connections) {
			// TODO.
		}
		// If anything has been found or not.
		if (selectedSymbols.Count > 0) {
			this.selectedSymbol = selectedSymbols.OrderByDescending(symbol => symbol.Ordering).First();
			this.selectedSymbol.Selected = true;
		} else {
			this.selectedSymbol = null;
		}
	}

	// Add a new node.
	public void AddNode() {
		// Revoke all symbols first.
		this.RevokeAllSelected();
		// Create a new node and update its ordering and selected status.
		GraphGrammarNode node = new GraphGrammarNode(EnumNodeTerminal.NonTerminal);
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
		foreach (GraphGrammarSymbol symbol in this.nodes.Cast<GraphGrammarSymbol>().Concat(this.connections.Cast<GraphGrammarSymbol>())) {
			symbol.Selected = false;
		}
		return;
	}
	
	// [Draw on canvas] Draw the node on canvas.
	public static void DrawNode(Vector2 pos, bool isTerminal, bool isSelected) {
		const int thickness = 2;
		const int size = 50;
		// If this node is activated (selected), add the highlight boarder.
		if (isSelected) {
			EditorCanvas.DrawQuad((int) pos[0]-thickness, (int) pos[1]-thickness, size+thickness*4, size+thickness*4, Color.red);
		}
		// Basic square and boarder.
		EditorCanvas.DrawQuad((int) pos[0], (int) pos[1], size+thickness*2, size+thickness*2, Color.black);
		EditorCanvas.DrawQuad((int) pos[0]+thickness, (int) pos[1]+thickness, size, size, isTerminal ? Color.green : Color.yellow);
	}
	// [Draw on canvas] Draw the connection on canvas.
	public static void DrawConnection(Vector2 posA, Vector2 posB, bool isSelected) {
		const int thickness = 2;
		const int size = 10;

		// Basic square and boarder.
		EditorCanvas.DrawQuad((int) posA[0], (int) posA[1], size+thickness*2, size+thickness*2, Color.black);
		EditorCanvas.DrawQuad((int) posA[0]+thickness, (int) posA[1]+thickness, size, size, Color.red);

		EditorCanvas.DrawQuad((int) posB[0], (int) posB[1], size+thickness*2, size+thickness*2, Color.black);
		EditorCanvas.DrawQuad((int) posB[0]+thickness, (int) posB[1]+thickness, size, size, Color.red);

		EditorCanvas.DrawLine(posA, posB, Color.white);

	}
}
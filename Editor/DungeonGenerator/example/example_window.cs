/*
    This is example that is not follow the lateset style guide.
    Just make a reference for you.
*/  

using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using EditorAdvance = EditorExtend.Advance;

public class MissionAlphabetWindow : EditorWindow {
	// Settings of window.
	private const uint toolBoxHeight = 250;
	private uint defaultCanvasWidth  = 500;
	private uint defaultCanvasHeight = 350;

	// 
	private GraphGrammar symbolSet = new GraphGrammar();
	// Setting of the drawing canvas.
	private Rect canvas = new Rect(0, 0, Screen.width, Screen.height);
	// Field in the editor window.
	private int _symbolOrdering;
	private EnumNodeTerminal _symbolTerminal = EnumNodeTerminal.Terminal;
	private int _canvasWidth, _canvasHeight;
	private Vector2 _canvasOffset;
	// Temporary values or pointers.
	private Vector2 mousePosition;
	private GraphGrammarNode tempNode;
	private GraphGrammarConnection tempConnection;

	void Awake() {
		this._symbolOrdering = 0;
		this._canvasWidth  = (int) this.defaultCanvasWidth;
		this._canvasHeight = (int) this.defaultCanvasHeight;
		this._canvasOffset = new Vector2(0, 0);
	}

	void OnGUI() {
		// Update current mouse position.
		this.mousePosition = (Event.current.mousePosition - new Vector2(0, toolBoxHeight));

		// Buttons - Add a new node.
		EditorGUILayout.BeginHorizontal(EditorAdvance.PureTextureGUI(Color.red));
		// <Left-hand container>
		EditorGUILayout.BeginVertical();
		if (GUILayout.Button("Add node", GUILayout.Height(35))) {
			this.symbolSet.AddNode();
		}
		EditorGUILayout.EndVertical();
		// <Right-hand container>
		EditorGUILayout.BeginVertical();
		if (GUILayout.Button("Add connection", GUILayout.Height(35))) {
			this.symbolSet.AddConnection();
		}
		EditorGUILayout.EndVertical();
		EditorGUILayout.EndHorizontal();

		// Information of the selected symbol.
		GUILayout.Label("Selected node", EditorStyles.boldLabel);

		// IntSlider - Ordering for showing.
		if (this.symbolSet.SelectedSymbol is GraphGrammarNode) {
			// Update the ordering value.
			this._symbolOrdering = EditorGUILayout.IntSlider(new GUIContent("Ordering:"), ((GraphGrammarNode) this.symbolSet.SelectedSymbol).Ordering, 1, this.symbolSet.Nodes.Count);
			((GraphGrammarNode) this.symbolSet.SelectedSymbol).Ordering = this._symbolOrdering;
			EditorAdvance.ProgressBar((float) this._symbolOrdering / this.symbolSet.Nodes.Count, "lower <--> upper");
			// Sort via ordering.
			this.symbolSet.Nodes = this.symbolSet.Nodes.OrderBy(symbol => symbol.Ordering).ToList();
		} else {
			EditorGUI.BeginDisabledGroup(true);
			this._symbolOrdering = EditorGUILayout.IntSlider(new GUIContent("Ordering:"), 0, 0, 0);
			EditorAdvance.ProgressBar(0, "lower <--> upper");
			EditorGUI.EndDisabledGroup();
		}

		// EnumPopup - Switch the node to terminal or non-terminal.
		if (this.symbolSet.SelectedSymbol is GraphGrammarNode) {
			this._symbolTerminal = (EnumNodeTerminal) EditorGUILayout.EnumPopup("Terminal:", ((GraphGrammarNode) this.symbolSet.SelectedSymbol).Terminal);
			((GraphGrammarNode) this.symbolSet.SelectedSymbol).Terminal = this._symbolTerminal;
		} else {
			EditorGUI.BeginDisabledGroup(true);
			EditorGUILayout.EnumPopup("Terminal:", EnumNodeTerminal.Terminal);
			EditorGUI.EndDisabledGroup();
		}

		EditorGUILayout.Space();

		// Adjust the size of canvas.
		GUILayout.Label("Canvas size", EditorStyles.boldLabel);
		EditorGUILayout.BeginHorizontal();
		// <Left-hand container>
		EditorGUILayout.BeginVertical(GUILayout.Width(Screen.width/3));
		this._canvasWidth = EditorAdvance.LimitedIntField("Width:", this._canvasWidth, 300, 3500);
		EditorGUILayout.EndVertical();
		GUILayout.FlexibleSpace();
		// <Right-hand container>
		EditorGUILayout.BeginVertical(GUILayout.Width(Screen.width/3));
		this._canvasHeight = EditorAdvance.LimitedIntField("Height:", this._canvasHeight, 200, 2000);
		EditorGUILayout.EndVertical();
		EditorGUILayout.EndHorizontal();

		// Adjust the offset of canvas.
		GUILayout.Label("Canvas offset", EditorStyles.boldLabel);
		EditorGUILayout.BeginHorizontal();
		// <Left-hand container>
		EditorGUILayout.BeginVertical(GUILayout.Width(Screen.width/3));
		this._canvasOffset.x = EditorAdvance.LimitedIntField("X:", (int) this._canvasOffset.x, 0, (int) 3500-Screen.width);
		EditorGUILayout.EndVertical();
		GUILayout.FlexibleSpace();
		// <Right-hand container>
		EditorGUILayout.BeginVertical(GUILayout.Width(Screen.width/3));
		this._canvasOffset.y = EditorAdvance.LimitedIntField("Y:", (int) this._canvasOffset.y, 0, (int) 2000-Screen.height);
		EditorGUILayout.EndVertical();
		EditorGUILayout.EndHorizontal();

		GUILayout.FlexibleSpace();

		// Canvas for drawing.
		// Screen.height is wrong....
		GUILayout.BeginArea(new Rect(0, toolBoxHeight, Screen.width, Screen.height));
		this.canvas = new Rect(0, 0, this._canvasWidth-this._canvasOffset.x, this._canvasHeight-this._canvasOffset.y);
		EditorGUI.DrawRect(this.canvas, Color.grey);

		// Activate the symbol, make it could be selected.
		if (Event.current.type == EventType.MouseDown && this.canvas.Contains(this.mousePosition)) {
			this.symbolSet.TouchedSymbol(this.mousePosition);
			// Refresh the layout.
			this.Repaint();
		}

		// Drag and drop event, could move the symbols of canvas.
		if (Event.current.type == EventType.MouseDrag) {
			if (this.canvas.Contains(this.mousePosition)) {
				// Select node.
				if (this.symbolSet.SelectedSymbol is GraphGrammarNode) {
					this.tempNode = (GraphGrammarNode) this.symbolSet.SelectedSymbol;
					this.tempNode.Position += Event.current.delta;
				}
				// Select connection.
				else if (this.symbolSet.SelectedSymbol is GraphGrammarConnection) {
					this.tempConnection = (GraphGrammarConnection) this.symbolSet.SelectedSymbol;
					// Start point.
					if (this.tempConnection.StartSelected) {
						this.tempConnection.StartPosition = this.mousePosition;
						this.symbolSet.StickyNode(this.tempConnection, this.mousePosition, "start");
					}
					// End point.
					else if (this.tempConnection.EndSelected) {
						this.tempConnection.EndPosition = this.mousePosition;
						this.symbolSet.StickyNode(this.tempConnection, this.mousePosition, "end");
					}
				}
			} else {
				// Revoke all 'selected' to false.
				this.symbolSet.RevokeAllSelected();
			}
			// Refresh the layout.
			this.Repaint();
			// Release.
			this.tempNode = null;
			this.tempConnection = null;
		}

		// Draw whole nodes on canvas.
		foreach (GraphGrammarNode node in this.symbolSet.Nodes) {
			GraphGrammar.DrawNode(node.Position, (node.Terminal == EnumNodeTerminal.Terminal ? true : false), node.Selected);
		}

		// Draw whole connections on canvas.
		foreach (GraphGrammarConnection connection in this.symbolSet.Connections) {
			GraphGrammar.DrawConnection(connection);
		}

		int right = Screen.width - (int) this.canvas.xMax;

		Rect tempRight = new Rect((int) this.canvas.xMax, 0, right, Screen.height-toolBoxHeight);
		EditorGUI.DrawRect(tempRight, Color.black);


		GUILayout.EndArea();
	}
}
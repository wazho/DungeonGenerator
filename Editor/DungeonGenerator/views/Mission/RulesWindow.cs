using UnityEngine;
using UnityEditor;
using System.Collections;

using EditorAdvance = EditorExtend.Advance;
using EditorStyle = EditorExtend.Style;

namespace MissionGrammar {	
	public class RulesWindow : EditorWindow {
		// Types of the editor.
		public enum EditingMode {
			None,
			SetEdit,
			RuleEdit,
			SetDelete,
			RuleDelete,
			SetCreate,
			RuleCreate,
		}
		// Types of the tabs.(After Rule Preview Area)
		public enum SymbolEditingMode {
			None,
			AddNode,
			AddConnection,
			Copy,
			Delete,
		}
		// The array of set & rule.
		private string[] _currentSet;
		private string[] _currentRule;
		// The index of set & rule.
		private int _currentSetIndex;
		private int _currentRuleIndex;
		// The description of set or rule.
		private string _name;
		private string _description;
		// Enabled Button-Apply
		private bool _applyEditingButtonEnabled;
		private bool _applySymbolEditingButtonEnabled;
		// The texture of icons.
		private Texture2D _edit;
		private Texture2D _delete;
		// The mode of buttons.
		private EditingMode       _editingMode;
		private SymbolEditingMode _currentTab;
		// The scroll bar of list.
		private Vector2 _scrollPosition;
		// [Remove soon] Content of scroll area.
		private string testString;
		// RuleSymbolSet
		private GraphGrammar _sourceSymbolSet      = new GraphGrammar();
		private GraphGrammar _replacementSymbolSet = new GraphGrammar();
		// The drawing canvas.
		private Rect _ruleSourceCanvasInWindow;
		private Rect _ruleReplacementCanvasInWindow;

		void Awake() {
			_currentSet        = new string[] { "Set1", "Set2"};
			_currentRule       = new string[] { "Rule1", "Rule2" };
			_currentSetIndex   = 0;
			_currentRuleIndex  = 0;
			_name              = string.Empty;
			_description       = string.Empty;
			_applyEditingButtonEnabled = false;
			_applySymbolEditingButtonEnabled = false;
			_edit              = Resources.Load<Texture2D>("Icons/edit");
			_delete            = Resources.Load<Texture2D>("Icons/delete");
			_editingMode       = EditingMode.None;
			_currentTab        = SymbolEditingMode.None;
			_scrollPosition    = Vector2.zero;
			// [Remove soon]
			testString = "*\n*\n*\n*\n*\n*\n*\n*\n*\n*\n*\n*\n*\n*\n*\n*\n*\n*\n*\n";
		}

		void OnGUI() {
			// Current Set.
			EditorGUILayout.BeginHorizontal();
			// Dropdown list of Current Set Type.
			_currentSetIndex = EditorGUILayout.Popup("Current Set", _currentSetIndex, _currentSet);
			// Buttons - Editor, Delete and Add new.
			if (GUILayout.Button(_edit, EditorStyles.miniButtonLeft, EditorStyle.ButtonHeight)) {
				_editingMode = EditingMode.SetEdit;
			}
			if (GUILayout.Button(_delete, EditorStyles.miniButtonMid, EditorStyle.ButtonHeight)) {
				_editingMode = EditingMode.SetDelete;
			}
			if (GUILayout.Button("Add New", EditorStyles.miniButtonRight, EditorStyle.ButtonHeight)) {
				_editingMode = EditingMode.SetCreate;
			}
			EditorGUILayout.EndHorizontal();

			// Current Rule.
			EditorGUILayout.BeginHorizontal();
			// Dropdown list of Currect Rule Type.
			_currentRuleIndex = EditorGUILayout.Popup("Current Rule", _currentRuleIndex, _currentRule);
			// Buttons - Editor, Delete and Add new.
			if (GUILayout.Button(_edit, EditorStyles.miniButtonLeft, EditorStyle.ButtonHeight)) {
				_editingMode = EditingMode.RuleEdit;
			}
			if (GUILayout.Button(_delete, EditorStyles.miniButtonMid, EditorStyle.ButtonHeight)) {
				_editingMode = EditingMode.RuleDelete;
			}
			if (GUILayout.Button("Add New", EditorStyles.miniButtonRight, EditorStyle.ButtonHeight)) {
				_editingMode = EditingMode.RuleCreate;
			}
			EditorGUILayout.EndHorizontal();

			// Show the Editor of Set or Rule.
			switch (_editingMode) {
			case EditingMode.SetEdit:
			case EditingMode.SetCreate:
			case EditingMode.RuleEdit:
			case EditingMode.RuleCreate:
				ShowEditSetRule();
				break;
			}

			// Show the area of rule-preview.
			GUILayout.BeginArea(EditorStyle.RulePreviewArea);
			ShowRulePreviewArea();
			GUILayout.EndArea();

			// Show the area of after-rule-preview.
			GUILayout.BeginArea(EditorStyle.AfterRulePreviewArea);
			ShowAfterRulePreviewArea();
			GUILayout.EndArea();

			EventController();
		}
		// Control whole events.
		void EventController() {
			if (Event.current.type == EventType.MouseDown) {
				OnClickedSymbolInPreviewCanvas();
			} else if (Event.current.type == EventType.MouseDrag) {// Drag and drop event, could move the symbols of canvas.
				OnDraggedAndDroppedInPreviewCanvas();
			}
		}
		//Click Event (Just copy-paste from example_window.cs)
		void OnClickedSymbolInPreviewCanvas() {
			if (_ruleSourceCanvasInWindow.Contains(Event.current.mousePosition)) {
				_replacementSymbolSet.RevokeAllSelected();
				_sourceSymbolSet.TouchedSymbol(new Vector2(Event.current.mousePosition.x - _ruleSourceCanvasInWindow.x, Event.current.mousePosition.y - _ruleSourceCanvasInWindow.y)/*new Vector2(x, y)*/);
				this.Repaint();
			} else if (_ruleReplacementCanvasInWindow.Contains(Event.current.mousePosition)) {
				_sourceSymbolSet.RevokeAllSelected();
				_replacementSymbolSet.TouchedSymbol(new Vector2(Event.current.mousePosition.x - _ruleReplacementCanvasInWindow.x, Event.current.mousePosition.y - _ruleReplacementCanvasInWindow.y)/*new Vector2(x, y)*/);
				this.Repaint();
			} else {
				Debug.Log(Event.current.mousePosition);
				Debug.Log(_ruleSourceCanvasInWindow);
				Debug.Log(_ruleReplacementCanvasInWindow);
			}
		}
		//Drag and drop event (Just copy-paste from example_window.cs)
		private static GraphGrammarNode tempNode;
		private static GraphGrammarConnection tempConnection;
		void OnDraggedAndDroppedInPreviewCanvas() {
			if (_ruleSourceCanvasInWindow.Contains(Event.current.mousePosition)) {
				_replacementSymbolSet.RevokeAllSelected();
				Vector2 positionInCanvas = new Vector2(Event.current.mousePosition.x - _ruleSourceCanvasInWindow.x, Event.current.mousePosition.y - _ruleSourceCanvasInWindow.y);
				// Select node.
				if (_sourceSymbolSet.SelectedSymbol is GraphGrammarNode) {
					tempNode = (GraphGrammarNode) _sourceSymbolSet.SelectedSymbol;
					tempNode.Position += Event.current.delta;
				}
				// Select connection.
				else if (_sourceSymbolSet.SelectedSymbol is GraphGrammarConnection) {
					tempConnection = (GraphGrammarConnection) _sourceSymbolSet.SelectedSymbol;
					// Start point.
					if (tempConnection.StartSelected) {
						tempConnection.StartPosition = positionInCanvas;
						_sourceSymbolSet.StickyNode(tempConnection, positionInCanvas, "start");
					}
					// End point.
					else if (tempConnection.EndSelected) {
						tempConnection.EndPosition = positionInCanvas;
						_sourceSymbolSet.StickyNode(tempConnection, positionInCanvas, "end");
					}
				}
			} else if (_ruleReplacementCanvasInWindow.Contains(Event.current.mousePosition)) {
				_sourceSymbolSet.RevokeAllSelected();
					Vector2 positionInCanvas = new Vector2(Event.current.mousePosition.x - _ruleReplacementCanvasInWindow.x, Event.current.mousePosition.y - _ruleReplacementCanvasInWindow.y);
					// Select node.
					if (_replacementSymbolSet.SelectedSymbol is GraphGrammarNode) {
						tempNode = (GraphGrammarNode) _replacementSymbolSet.SelectedSymbol;
						tempNode.Position += Event.current.delta;
					}
					// Select connection.
					else if (_replacementSymbolSet.SelectedSymbol is GraphGrammarConnection) {
						tempConnection = (GraphGrammarConnection) _replacementSymbolSet.SelectedSymbol;
					// Start point.
					if (tempConnection.StartSelected) {
						tempConnection.StartPosition = positionInCanvas;
						_replacementSymbolSet.StickyNode(tempConnection, positionInCanvas, "start");
					}
					// End point.
					else if (tempConnection.EndSelected) {
						tempConnection.EndPosition = positionInCanvas;
						_replacementSymbolSet.StickyNode(tempConnection, positionInCanvas, "end");
					}
				}
			} else {
				// Revoke all 'selected' to false.
				_sourceSymbolSet.RevokeAllSelected();
				_replacementSymbolSet.RevokeAllSelected();
			}
			// Refresh the layout.
			this.Repaint();
			// Release.
			tempNode = null;
			tempConnection = null;
		}
		void ShowRulePreviewArea() {
			// Information of Source and Replacement.
			EditorGUILayout.BeginHorizontal();
			GUILayout.Label("Source", EditorStyle.Header2, GUILayout.Width(Screen.width / 2));
			GUILayout.Label("Replacement", EditorStyle.Header2, GUILayout.Width(Screen.width / 2));
			EditorGUILayout.EndHorizontal();

			// Canvas
			// SourceCanvas
			GUILayout.BeginArea(EditorStyle.RuleSourceCanvasArea);
			// Get the Rect in EditWindow from the GUI rect. (Position = Real screen position - this EditWindow position)
			_ruleSourceCanvasInWindow.position = GUIUtility.GUIToScreenPoint(EditorStyle.RuleGraphGrammarCanvas.position) - this.position.position;
			_ruleSourceCanvasInWindow.size = EditorStyle.RuleGraphGrammarCanvas.size;
			EditorGUI.DrawRect(EditorStyle.RuleGraphGrammarCanvas, Color.gray);
			//Draw Nodes and Connections.
			foreach (GraphGrammarNode node in _sourceSymbolSet.Nodes) {
				GraphGrammar.DrawNode(node);
			}
			foreach (GraphGrammarConnection connection in _sourceSymbolSet.Connections) {
				GraphGrammar.DrawConnection(connection);
			}
			GUILayout.EndArea();

			// ReplacementCanvas
			GUILayout.BeginArea(EditorStyle.RuleReplacementCanvasArea);
			_ruleReplacementCanvasInWindow.position = GUIUtility.GUIToScreenPoint(EditorStyle.RuleGraphGrammarCanvas.position) - this.position.position;
			_ruleReplacementCanvasInWindow.size = EditorStyle.RuleGraphGrammarCanvas.size;
			EditorGUI.DrawRect(EditorStyle.RuleGraphGrammarCanvas, Color.white);
			foreach (GraphGrammarNode node in _replacementSymbolSet.Nodes) {
				GraphGrammar.DrawNode(node);
			}
			foreach (GraphGrammarConnection connection in _replacementSymbolSet.Connections) {
				GraphGrammar.DrawConnection(connection);
			}
			GUILayout.EndArea();
		}

		void ShowAfterRulePreviewArea() {
			// Buttons - Add Node & Add Connection & Copy & Delete.
			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button("Add Node", EditorStyles.miniButtonLeft, EditorStyle.ButtonHeight)) {
				// [Will remove] Just test canvas.
				// Add Alphabet's Node 
				_sourceSymbolSet.AddNode(Alphabet.Nodes[0]);
				//_sourceSymbolSet.RevokeAllSelected();
				_replacementSymbolSet.AddNode(Alphabet.Nodes[1]);
				_replacementSymbolSet.RevokeAllSelected();

				_applySymbolEditingButtonEnabled = true;
				_currentTab = SymbolEditingMode.AddNode;
			}
			if (GUILayout.Button("Add Connection", EditorStyles.miniButtonMid, EditorStyle.ButtonHeight)) {
				_applySymbolEditingButtonEnabled = true;
				_currentTab = SymbolEditingMode.AddConnection;
			}
			if (GUILayout.Button("Copy", EditorStyles.miniButtonMid, EditorStyle.ButtonHeight)) {
				_applySymbolEditingButtonEnabled = true;
				_currentTab = SymbolEditingMode.Copy;
			}
			if (GUILayout.Button("Delete", EditorStyles.miniButtonRight, EditorStyle.ButtonHeight)) {
				_applySymbolEditingButtonEnabled = true;
				_currentTab = SymbolEditingMode.Delete;
			}
			EditorGUILayout.EndHorizontal();
			// Show the list.
			switch (_currentTab) {
				case SymbolEditingMode.AddNode:
					LayoutNodeList();
					break;
				case SymbolEditingMode.AddConnection:
					LayoutConnectionList();
					break;
			}
			
			// Remind user [need Modify]
			EditorGUILayout.HelpBox("Info \nThe Node's name has been used.", MessageType.Info);
			// Buttons - Apply.
			GUI.enabled = _applySymbolEditingButtonEnabled;
			if (GUILayout.Button("Apply", EditorStyles.miniButton, EditorStyle.ButtonHeight)) {
				if (EditorUtility.DisplayDialog("Saving", 
					"Are you sure to save?",
						"Yes", "No")) {
				} else {

				}
			}
			GUI.enabled = true;
		}

		void ShowEditSetRule() {
			// Information.
			_name = EditorGUILayout.TextField("Name", _name);
			_description = EditorGUILayout.TextField("Description", _description);
			// Remind user [need Modify]
			if (_name == string.Empty && _description == string.Empty) {
				EditorGUILayout.HelpBox("Info \nThe name is empty. \nThe description is empty.", MessageType.Info);
			}
			if (_name == string.Empty && _description != string.Empty) {
				EditorGUILayout.HelpBox("Info \nThe name is empty.", MessageType.Info);
			}
			if (_name != string.Empty && _description == string.Empty) {
				EditorGUILayout.HelpBox("Info \nThe description is empty.", MessageType.Info);
			}
			if (_name != string.Empty && _description != string.Empty) {
				_applyEditingButtonEnabled = true;
				EditorGUILayout.HelpBox("Info \nNothing.", MessageType.Info);
			}

			// Buttons - Apply.
			GUI.enabled = _applyEditingButtonEnabled;
			if (GUILayout.Button("Apply", EditorStyles.miniButton, EditorStyle.ButtonHeight)) {
				if (EditorUtility.DisplayDialog("Saving", 
				"Are you sure to save?",
				"Yes", "No")) {
				} else {

				}
			}
			GUI.enabled = true;
		}

		void LayoutNodeList() {
			// Content of Node-List.
			// Set the ScrollPosition.
			_scrollPosition = GUILayout.BeginScrollView(_scrollPosition, EditorStyle.AlphabetSymbolListHeight);
			// Content of scroll area.
			GUILayout.Label(testString, EditorStyles.label);
			GUILayout.EndScrollView();
		}

		void LayoutConnectionList() {
			// Content of Connection-List.
			// Set the ScrollPosition.
			_scrollPosition = GUILayout.BeginScrollView(_scrollPosition, EditorStyle.AlphabetSymbolListHeight);
			// Content of scroll area.
			GUILayout.Label(testString, EditorStyles.label);
			GUILayout.EndScrollView();
		}
	}
}
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections;
using System.Text.RegularExpressions;

using EditorAdvance = EditorExtend.Advance;
using EditorStyle   = EditorExtend.Style;

namespace MissionGrammarSystem {	
	public class RulesWindow : EditorWindow {
		// Types of the editor.
		public enum EditingMode {
			None,
			EditGroup,
			EditRule,
			DeleteGroup,
			DeleteRule,
			CreateGroup,
			CreateRule,
		}
		// Types of the tabs. (After Rule Preview Area)
		public enum SymbolEditingMode {
			None,
			AddNode,
			AddConnection,
			Copy,
			Delete,
		}
		// The mode of buttons.
		private EditingMode       _editingMode;
		private SymbolEditingMode _currentTab;
		// Mission rule of current editing.
		private MissionRule _missionRule = new MissionRule();
		// The array of group & rule.
		private string[] _groupsOptions;
		private string[] _rulesOptions;
		// The index of group & rule.
		private int _indexOfGroupsOptions;
		private int _indexOfRulesOptions;
		// The description of group or rule.
		private string _name;
		private string _description;
		// Enabled Button-Apply
		private bool _applyEditingButtonEnabled;
		private bool _applySymbolEditingButtonEnabled;
		// The texture of icons.
		private Texture2D _edit;
		private Texture2D _delete;
		// The drawing canvas.
		private Rect _ruleSourceCanvasInWindow;
		private Rect _ruleReplacementCanvasInWindow;
		// The scroll bar of canvas.
		private Vector2 _sourceCanvasScrollPosition;
		private Vector2 _replacementCanvasScrollPosition;
		// Size of source canvas & replacement canvas.
		private int _sourceCanvasSizeWidth;
		private int _sourceCanvasSizeHeight;
		private int _replacementCanvasSizeWidth;
		private int _replacementCanvasSizeHeight;
		// The scroll bar of list.
		private Vector2 _scrollPosition;
		// [Remove soon] Content of scroll area.
		private string testString;
		private enum SelectedCanvas {
			SourceCanvas,
			ReplacementCanvas
		};
		private SelectedCanvas _currentSelectedCanvas;

		void Awake() {
			_editingMode          = EditingMode.None;
			_currentTab           = SymbolEditingMode.None;
			_missionRule          = MissionGrammar.Groups[0].Rules[0];
			_groupsOptions        = MissionGrammar.Groups.Select(s => s.Name).ToArray();
			_rulesOptions         = MissionGrammar.Groups[0].Rules.Select(r => r.Name).ToArray();
			_indexOfGroupsOptions = 0;
			_indexOfRulesOptions  = 0;
			_name                 = string.Empty;
			_description          = string.Empty;
			_applyEditingButtonEnabled = false;
			_applySymbolEditingButtonEnabled = false;
			_edit                 = Resources.Load<Texture2D>("Icons/edit");
			_delete               = Resources.Load<Texture2D>("Icons/delete");
			_sourceCanvasScrollPosition      = Vector2.zero;
			_replacementCanvasScrollPosition = Vector2.zero;
			_scrollPosition       = Vector2.zero;
			_sourceCanvasSizeWidth       = 8000;
			_sourceCanvasSizeHeight      = 1000;
			_replacementCanvasSizeWidth  = 1000;
			_replacementCanvasSizeHeight = 300;
			_currentSelectedCanvas = SelectedCanvas.SourceCanvas;
			// [Remove soon]
			testString = "*\n*\n*\n*\n*\n*\n*\n*\n*\n*\n*\n*\n*\n*\n*\n*\n*\n*\n*\n";
		}

		void OnGUI() {
			// Layout the combobox and editor of mission group.
			LayoutMissionGroupOptions();
			// Layout the combobox and editor of mission rule.
			LayoutMissionRuleOptions();
			// Layout the editor of mission group or mission rule.
			switch (_editingMode) {
			case EditingMode.EditGroup:
			case EditingMode.CreateGroup:
			case EditingMode.EditRule:
			case EditingMode.CreateRule:
				LayoutBasicInfoEditor();
				break;
			}
			// Layout the canvas areas of two graph grammars.
			LayoutRulesCanvasArea();
			// Show the area of after-rule-preview.
			LayoutRuleCanvasEditor();
			// Control whole events.
			EventController();

			// [Remove soon] Just Testing
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.BeginVertical();
			_sourceCanvasSizeWidth  = EditorAdvance.LimitedIntField("SourceCanvasSizeWidth:", _sourceCanvasSizeWidth, 100, 5000);
			_sourceCanvasSizeHeight = EditorAdvance.LimitedIntField("SourceCanvasSizeHeight:", _sourceCanvasSizeHeight, 100, 2000);
			EditorGUILayout.EndVertical();
			EditorGUILayout.BeginVertical();
			_replacementCanvasSizeWidth  = EditorAdvance.LimitedIntField("ReplacementCanvasSizeWidth:", _replacementCanvasSizeWidth, 100, 5000);
			_replacementCanvasSizeHeight = EditorAdvance.LimitedIntField("ReplacementCanvasSizeHeight:", _replacementCanvasSizeHeight, 100, 5000);
			EditorGUILayout.EndVertical();
			EditorGUILayout.EndHorizontal();
		}
		// Layout the combobox and editor of mission group.
		void LayoutMissionGroupOptions() {
			// Current group.
			EditorGUILayout.BeginHorizontal();
			// Dropdown list of current group type.
			_indexOfGroupsOptions = EditorGUILayout.Popup("Current Group", _indexOfGroupsOptions, _groupsOptions);
			// Editor buttons, edit, delete and create.
			if (GUILayout.Button(_edit, EditorStyles.miniButtonLeft, EditorStyle.ButtonHeight)) {
				_editingMode = EditingMode.EditGroup;
			}
			if (GUILayout.Button(_delete, EditorStyles.miniButtonMid, EditorStyle.ButtonHeight)) {
				_editingMode = EditingMode.DeleteGroup;
			}
			if (GUILayout.Button("Add New", EditorStyles.miniButtonRight, EditorStyle.ButtonHeight)) {
				_editingMode = EditingMode.CreateGroup;
			}
			EditorGUILayout.EndHorizontal();
		}
		// Layout the combobox and editor of mission rule, mission rule is sub-member in current mission group.
		void LayoutMissionRuleOptions() {
			// Current rule.
			EditorGUILayout.BeginHorizontal();
			// Dropdown list of Currect Rule Type.
			_indexOfRulesOptions = EditorGUILayout.Popup("Current Rule", _indexOfRulesOptions, _rulesOptions);
			// Buttons - Editor, Delete and Add new.
			if (GUILayout.Button(_edit, EditorStyles.miniButtonLeft, EditorStyle.ButtonHeight)) {
				_editingMode = EditingMode.EditRule;
			}
			if (GUILayout.Button(_delete, EditorStyles.miniButtonMid, EditorStyle.ButtonHeight)) {
				_editingMode = EditingMode.DeleteRule;
			}
			if (GUILayout.Button("Add New", EditorStyles.miniButtonRight, EditorStyle.ButtonHeight)) {
				_editingMode = EditingMode.CreateRule;
			}
			EditorGUILayout.EndHorizontal();
		}
		// Layout the editor of mission group or mission rule.
		void LayoutBasicInfoEditor() {
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
		// Layout the canvas areas of two graph grammars.
		void LayoutRulesCanvasArea() {
			GUILayout.BeginArea(EditorStyle.RulePreviewArea);
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
			_ruleSourceCanvasInWindow.size     = EditorStyle.RuleGraphGrammarCanvas.size;
			// Show the source canvas.
			ShowSourceCanvas();
			GUILayout.EndArea();

			// ReplacementCanvas
			GUILayout.BeginArea(EditorStyle.RuleReplacementCanvasArea);
			_ruleReplacementCanvasInWindow.position = GUIUtility.GUIToScreenPoint(EditorStyle.RuleGraphGrammarCanvas.position) - this.position.position;
			_ruleReplacementCanvasInWindow.size = EditorStyle.RuleGraphGrammarCanvas.size;
			// Show the replacement canvas.
			ShowReplacementCanvas();
			GUILayout.EndArea();
			GUILayout.EndArea();
		}
		// Layout the canvas editor of current selected rules.
		void LayoutRuleCanvasEditor() {
			GUILayout.BeginArea(EditorStyle.AfterRulePreviewArea);
			// Buttons - Add Node & Add Connection & Copy & Delete.
			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button("Add Node", EditorStyles.miniButtonLeft, EditorStyle.ButtonHeight)) {
				// [Will remove] Just test canvas.
				// Add Alphabet's Node 
				_missionRule.SourceRule.AddNode(Alphabet.Nodes[0]);
				//_missionRule.SourceRule.RevokeAllSelected();
				_missionRule.ReplacementRule.AddNode(Alphabet.Nodes[1]);
				_missionRule.ReplacementRule.RevokeAllSelected();

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
			GUILayout.EndArea();
		}
		// Control whole events.
		void EventController() {
			if (Event.current.type == EventType.MouseDown) {
				OnClickedSymbolInCanvas();
			} else if (Event.current.type == EventType.MouseDrag) {
				// Drag and drop event, could move the symbols of canvas.
				OnDraggedAndDroppedInCanvas();
			}
		}

		// Click Event (Just copy-paste from example_window.cs)
		void OnClickedSymbolInCanvas() {
			if (_ruleSourceCanvasInWindow.Contains(Event.current.mousePosition)) {
				_currentSelectedCanvas = SelectedCanvas.SourceCanvas;
				_missionRule.ReplacementRule.RevokeAllSelected();
				_missionRule.SourceRule.TouchedSymbol(Event.current.mousePosition - _ruleSourceCanvasInWindow.position + _sourceCanvasScrollPosition);
				Repaint();
			} else if (_ruleReplacementCanvasInWindow.Contains(Event.current.mousePosition)) {
				_currentSelectedCanvas = SelectedCanvas.ReplacementCanvas;
				_missionRule.SourceRule.RevokeAllSelected();
				_missionRule.ReplacementRule.TouchedSymbol(Event.current.mousePosition - _ruleReplacementCanvasInWindow.position + _replacementCanvasScrollPosition);
				Repaint();
			}
		}
		// Drag and drop event (Just copy-paste from example_window.cs)
		private static GraphGrammarNode tempNode;
		private static GraphGrammarConnection tempConnection;
		void OnDraggedAndDroppedInCanvas() {
			// If mouse position is in the canvas of source rule. 
			if (_ruleSourceCanvasInWindow.Contains(Event.current.mousePosition)) {
				_missionRule.ReplacementRule.RevokeAllSelected();
				Vector2 positionInCanvas = Event.current.mousePosition - _ruleSourceCanvasInWindow.position;
				// Select node.
				if (_missionRule.SourceRule.SelectedSymbol is GraphGrammarNode) {
					tempNode = (GraphGrammarNode) _missionRule.SourceRule.SelectedSymbol;
					tempNode.Position += Event.current.delta;
				}
				// Select connection.
				else if (_missionRule.SourceRule.SelectedSymbol is GraphGrammarConnection) {
					tempConnection = (GraphGrammarConnection) _missionRule.SourceRule.SelectedSymbol;
					// Start point.
					if (tempConnection.StartSelected) {
						tempConnection.StartPosition = positionInCanvas;
						_missionRule.SourceRule.StickyNode(tempConnection, positionInCanvas, "start");
					}
					// End point.
					else if (tempConnection.EndSelected) {
						tempConnection.EndPosition = positionInCanvas;
						_missionRule.SourceRule.StickyNode(tempConnection, positionInCanvas, "end");
					}
				}
			}
			// If mouse position is in the canvas of replacement rule. 
			else if (_ruleReplacementCanvasInWindow.Contains(Event.current.mousePosition)) {
				_missionRule.SourceRule.RevokeAllSelected();
				Vector2 positionInCanvas = Event.current.mousePosition - _ruleReplacementCanvasInWindow.position;
				// Select node.
				if (_missionRule.ReplacementRule.SelectedSymbol is GraphGrammarNode) {
					tempNode = (GraphGrammarNode) _missionRule.ReplacementRule.SelectedSymbol;
					tempNode.Position += Event.current.delta;
				}
					// Select connection.
				else if (_missionRule.ReplacementRule.SelectedSymbol is GraphGrammarConnection) {
						tempConnection = (GraphGrammarConnection) _missionRule.ReplacementRule.SelectedSymbol;
					// Start point.
					if (tempConnection.StartSelected) {
						tempConnection.StartPosition = positionInCanvas;
						_missionRule.ReplacementRule.StickyNode(tempConnection, positionInCanvas, "start");
					}
					// End point.
					else if (tempConnection.EndSelected) {
						tempConnection.EndPosition = positionInCanvas;
						_missionRule.ReplacementRule.StickyNode(tempConnection, positionInCanvas, "end");
					}
				}
			} else {
				// Revoke all 'selected' to false.
				_missionRule.SourceRule.RevokeAllSelected();
				_missionRule.ReplacementRule.RevokeAllSelected();
			}
			// Refresh the layout.
			Repaint();
			// Release.
			tempNode = null;
			tempConnection = null;
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
		void ShowSourceCanvas() {
			// Set the scroll position.
			_sourceCanvasScrollPosition = GUILayout.BeginScrollView(_sourceCanvasScrollPosition, GUILayout.Width(Screen.width / 2), EditorStyle.RuleScrollViewHeight);
			Debug.Log(_sourceCanvasScrollPosition);
			// Content of canvas area.
			EditorStyle.ResizeRuleSourceCanvas(_sourceCanvasSizeWidth, _sourceCanvasSizeHeight);
			// If  this is current selected canvas, backgound will be white. Else gray.
			EditorGUI.DrawRect(EditorStyle.RuleSourceCanvas, _currentSelectedCanvas==SelectedCanvas.SourceCanvas ? Color.white : Color.gray);
			GUILayout.Label(string.Empty, EditorStyle.RuleSourceCanvasContent);
			// Draw Nodes and Connections.
			foreach (GraphGrammarNode node in _missionRule.SourceRule.Nodes) {
				GraphGrammar.DrawNode(node);
			}
			foreach (GraphGrammarConnection connection in _missionRule.SourceRule.Connections) {
				GraphGrammar.DrawConnection(connection);
			}
			GUILayout.EndScrollView();
		}

		void ShowReplacementCanvas() {
			// Set the scroll position.
			_replacementCanvasScrollPosition = GUILayout.BeginScrollView(_replacementCanvasScrollPosition, GUILayout.Width(Screen.width / 2), EditorStyle.RuleScrollViewHeight);
			// Content of canvas area.
			EditorStyle.ResizeRuleReplacementCanvas(_replacementCanvasSizeWidth, _replacementCanvasSizeHeight);
			EditorGUI.DrawRect(EditorStyle.RuleReplacementCanvas, _currentSelectedCanvas == SelectedCanvas.ReplacementCanvas ? Color.white : Color.gray);
			GUILayout.Label(string.Empty, EditorStyle.RuleReplacementCanvasContent);
			foreach (GraphGrammarNode node in _missionRule.ReplacementRule.Nodes) {
				GraphGrammar.DrawNode(node);
			}
			foreach (GraphGrammarConnection connection in _missionRule.ReplacementRule.Connections) {
				GraphGrammar.DrawConnection(connection);
			}
			GUILayout.EndScrollView();
		}
	}
}
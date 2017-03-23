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
		private int _tempIndexOfGroupsOptions;
		private int _tempIndexOfRulesOptions;
		// The description of group or rule.
		private string _name;
		private string _description;
		private bool _nameCanBeUsed;
		// Enabled Button-Apply
		private bool _applyEditingButtonEnabled;
		private bool _applySymbolEditingButtonEnabled;
		// The texture of icons.
		private Texture2D _edit;
		private Texture2D _delete;
		// The drawing canvas.
		private Rect _ruleSourceCanvasInWindow;
		private Rect _ruleReplacementCanvasInWindow;
		private Rect _symbolListCanvasInWindow;
		// The scroll bar of canvas.
		private Vector2 _sourceCanvasScrollPosition;
		private Vector2 _replacementCanvasScrollPosition;
		// Size of source canvas & replacement canvas.
		private int _sourceCanvasSizeWidth;
		private int _sourceCanvasSizeHeight;
		private int _replacementCanvasSizeWidth;
		private int _replacementCanvasSizeHeight;
		// The scroll bar of list.
		private Vector2 _listScrollPosition;
		private static Vector2 _positionInCanvas;
		private GraphGrammar _currentSelectedGraphGrammar;


		void Awake() {
			_editingMode          = EditingMode.None;
			_currentTab           = SymbolEditingMode.None;
			// [TODO] Here doesn't avoid the out of index, 
			//        if doesn't exist any member need initial in future.
			_missionRule          = MissionGrammar.Groups[0].Rules[0];
			_groupsOptions        = MissionGrammar.Groups.Select(s => s.Name).ToArray();
			_rulesOptions         = MissionGrammar.Groups[0].Rules.Select(r => r.Name).ToArray();
			_indexOfGroupsOptions = 0;
			_indexOfRulesOptions  = 0;
			_tempIndexOfGroupsOptions = 0;
			_tempIndexOfRulesOptions = 0;
			_name                 = string.Empty;
			_description          = string.Empty;
			_nameCanBeUsed = false;
			_applyEditingButtonEnabled = false;
			_applySymbolEditingButtonEnabled = false;
			_edit                 = Resources.Load<Texture2D>("Icons/edit");
			_delete               = Resources.Load<Texture2D>("Icons/delete");
			_sourceCanvasScrollPosition      = Vector2.zero;
			_replacementCanvasScrollPosition = Vector2.zero;
			_listScrollPosition       = Vector2.zero;
			_sourceCanvasSizeWidth       = 8000;
			_sourceCanvasSizeHeight      = 1000;
			_replacementCanvasSizeWidth  = 1000;
			_replacementCanvasSizeHeight = 300;
			_currentSelectedGraphGrammar = _missionRule.SourceRule;
			Alphabet.RevokeAllSelected();
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
			if(_tempIndexOfGroupsOptions != _indexOfGroupsOptions) {
				_editingMode = EditingMode.None;
				_tempIndexOfGroupsOptions = _indexOfGroupsOptions;
				_indexOfRulesOptions = 0;
				// Update the rules of selected group.
				_rulesOptions = MissionGrammar.Groups[_indexOfGroupsOptions].Rules.Select(r => r.Name).ToArray();
			}
			// Editor buttons, edit, delete and create.
			if (GUILayout.Button(_edit, EditorStyles.miniButtonLeft, EditorStyle.ButtonHeight)) {
				_editingMode = EditingMode.EditGroup;
				_name = MissionGrammar.Groups[_indexOfGroupsOptions].Name;
				_description = MissionGrammar.Groups[_indexOfGroupsOptions].Description;
			}
			if (GUILayout.Button(_delete, EditorStyles.miniButtonMid, EditorStyle.ButtonHeight)) {
				_editingMode = EditingMode.DeleteGroup;
				MissionGrammar.RemoveGroup(MissionGrammar.Groups[_indexOfGroupsOptions]);
				// If user delete the least one, add the new one.
				if (_groupsOptions.Length <= 1) {
					_name = "New group";
					_description = "Description here.";
					AddGroup();
				}
				_indexOfGroupsOptions = 0;
				_editingMode = EditingMode.None;
			}
			if (GUILayout.Button("Add New", EditorStyles.miniButtonRight, EditorStyle.ButtonHeight)) {
				_editingMode = EditingMode.CreateGroup;
				_name = string.Empty;
				_description = string.Empty;
			}
			// Update the content of Dropdown.
			_groupsOptions = MissionGrammar.Groups.Select(s => s.Name).ToArray();
			EditorGUILayout.EndHorizontal();
		}
		// Layout the combobox and editor of mission rule, mission rule is sub-member in current mission group.
		void LayoutMissionRuleOptions() {
			// Current rule.
			EditorGUILayout.BeginHorizontal();
			// Dropdown list of Currect Rule Type.
			_indexOfRulesOptions = EditorGUILayout.Popup("Current Rule", _indexOfRulesOptions, _rulesOptions);
			if (_tempIndexOfRulesOptions != _indexOfRulesOptions) {
				_editingMode = EditingMode.None;
				_tempIndexOfRulesOptions = _indexOfRulesOptions;
			}
			// Buttons - Editor, Delete and Add new.
			if (GUILayout.Button(_edit, EditorStyles.miniButtonLeft, EditorStyle.ButtonHeight)) {
				_editingMode = EditingMode.EditRule;
				_name = MissionGrammar.Groups[_indexOfGroupsOptions].Rules[_indexOfRulesOptions].Name;
				_description = MissionGrammar.Groups[_indexOfGroupsOptions].Rules[_indexOfRulesOptions].Description;
			}
			if (GUILayout.Button(_delete, EditorStyles.miniButtonMid, EditorStyle.ButtonHeight)) {
				_editingMode = EditingMode.DeleteRule;
				MissionGrammar.Groups[_indexOfGroupsOptions].RemoveRule(MissionGrammar.Groups[_indexOfGroupsOptions].Rules[_indexOfRulesOptions]);
				// If user delete the least one, add the new one.
				if (_rulesOptions.Length <= 1) {
					_name = "New rule";
					_description = "Description here.";
					AddRule(_indexOfGroupsOptions);
				}
				_indexOfRulesOptions = 0;
				_editingMode = EditingMode.None;
			}
			if (GUILayout.Button("Add New", EditorStyles.miniButtonRight, EditorStyle.ButtonHeight)) {
				_editingMode = EditingMode.CreateRule;
				_name = string.Empty;
				_description = string.Empty;
			}
			// Update the content of Dropdown.
			_rulesOptions = MissionGrammar.Groups[_indexOfGroupsOptions].Rules.Select(r => r.Name).ToArray();
			EditorGUILayout.EndHorizontal();
		}
		// Layout the editor of mission group or mission rule.
		void LayoutBasicInfoEditor() {
			// Information of mission group or mission rule.
			switch (_editingMode) {
				case EditingMode.EditGroup:
				case EditingMode.CreateGroup:
					_name = EditorGUILayout.TextField("Group Name", _name);
					_description = EditorGUILayout.TextField("Group Description", _description);
					// Check the name has never used before.
					_nameCanBeUsed = !IsGroupNameUsed(_name);
					break;
				case EditingMode.EditRule:
				case EditingMode.CreateRule:
					_name = EditorGUILayout.TextField("Rule Name", _name);
					_description = EditorGUILayout.TextField("Rule Description", _description);
					// Check the name has never used before.
					_nameCanBeUsed = !IsRuleNameUsed(_name, _indexOfGroupsOptions);
					break;
			}
			// Remind user [need Modify]
			if (_name == string.Empty && _description == string.Empty) {
				EditorGUILayout.HelpBox("Info \nThe name is empty. \nThe description is empty.", MessageType.Info);
				_applyEditingButtonEnabled = false;
			}
			if (_name == string.Empty && _description != string.Empty) {
				_applyEditingButtonEnabled = false;
				EditorGUILayout.HelpBox("Info \nThe name is empty.", MessageType.Info);
			}
			if (_name != string.Empty && _description == string.Empty && _nameCanBeUsed == false) {
				_applyEditingButtonEnabled = false;
				EditorGUILayout.HelpBox("Info  \nThe name has been used before. \nThe description is empty.", MessageType.Info);
			}
			if (_name != string.Empty && _description == string.Empty && _nameCanBeUsed == true) {
				_applyEditingButtonEnabled = false;
				EditorGUILayout.HelpBox("Info \nThe description is empty.", MessageType.Info);
			}
			if (_name != string.Empty && _description != string.Empty && _nameCanBeUsed == false) {
				_applyEditingButtonEnabled = false;
				EditorGUILayout.HelpBox("Info \nThe name has been used before.", MessageType.Info);
			}
			if (_name != string.Empty && _description != string.Empty && _nameCanBeUsed == true) {
				_applyEditingButtonEnabled = true;
				EditorGUILayout.HelpBox("Info \nNothing.", MessageType.Info);
			}
			// Buttons - Apply.
			GUI.enabled = _applyEditingButtonEnabled;
			if (GUILayout.Button("Apply", EditorStyles.miniButton, EditorStyle.ButtonHeight)) {
				if (EditorUtility.DisplayDialog("Saving", 
				"Are you sure to save?",
				"Yes", "No")) {
					switch (_editingMode) {
						case EditingMode.EditGroup:
							UpdateGroup(MissionGrammar.Groups[_indexOfGroupsOptions]);
							break;
						case EditingMode.CreateGroup:
							AddGroup();
							break;
						case EditingMode.EditRule:
							UpdateRule(MissionGrammar.Groups[_indexOfGroupsOptions].Rules[_indexOfRulesOptions]);
							break;
						case EditingMode.CreateRule:
							AddRule(_indexOfGroupsOptions);
							break;
					}
					_editingMode = EditingMode.None;
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
				
				_missionRule.SourceRule.RevokeAllSelected();
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
				LayoutEditingButtonGroup();
				break;
			case SymbolEditingMode.AddConnection:
				LayoutConnectionList();
				LayoutEditingButtonGroup();
				break;
			case SymbolEditingMode.Copy:
				_currentTab = SymbolEditingMode.AddNode;
				CopySelectedCanvas();
				break;
			case SymbolEditingMode.Delete:
				_currentTab = SymbolEditingMode.AddNode;
				DeleteSelectedNode();
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

		// Click Event
		void OnClickedSymbolInCanvas() {
			if (_ruleSourceCanvasInWindow.Contains(Event.current.mousePosition)) {
				_currentSelectedGraphGrammar = _missionRule.SourceRule;
				_missionRule.ReplacementRule.RevokeAllSelected();
				_missionRule.SourceRule.TouchedSymbol(Event.current.mousePosition - _ruleSourceCanvasInWindow.position + _sourceCanvasScrollPosition);
				Repaint();
			} else if (_ruleReplacementCanvasInWindow.Contains(Event.current.mousePosition)) {
				_currentSelectedGraphGrammar = _missionRule.ReplacementRule;
				_missionRule.SourceRule.RevokeAllSelected();
				_missionRule.ReplacementRule.TouchedSymbol(Event.current.mousePosition - _ruleReplacementCanvasInWindow.position + _replacementCanvasScrollPosition);
				Repaint();
			} else if(_symbolListCanvasInWindow .Contains(Event.current.mousePosition)) {
				_positionInCanvas = Event.current.mousePosition - _symbolListCanvasInWindow.position;
				int index = (int) ( _positionInCanvas.y ) / 50;
				Alphabet.RevokeAllSelected();
				switch (_currentTab) {
				case SymbolEditingMode.AddNode:
					if (index < Alphabet.Nodes.Count) {
						Alphabet.Nodes[index].Selected = true;
					}
					break;
				case SymbolEditingMode.AddConnection:
					if (index < Alphabet.Connections.Count) {
						Alphabet.Connections[index].Selected = true;
					}
					break;
				}
				Repaint();
			}
		}
		// Drag and drop event
		private static GraphGrammarNode tempNode;
		private static GraphGrammarConnection tempConnection;
		void OnDraggedAndDroppedInCanvas() {
			// If mouse position is in the canvas of source rule. 
			if (_ruleSourceCanvasInWindow.Contains(Event.current.mousePosition)) {
				_missionRule.ReplacementRule.RevokeAllSelected();
				_positionInCanvas = Event.current.mousePosition - _ruleSourceCanvasInWindow.position + _sourceCanvasScrollPosition;
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
						tempConnection.StartPosition = _positionInCanvas;
						_missionRule.SourceRule.StickyNode(tempConnection, _positionInCanvas, "start");
					}
					// End point.
					else if (tempConnection.EndSelected) {
						tempConnection.EndPosition = _positionInCanvas;
						_missionRule.SourceRule.StickyNode(tempConnection, _positionInCanvas, "end");
					}
				}
			}
			// If mouse position is in the canvas of replacement rule. 
			else if (_ruleReplacementCanvasInWindow.Contains(Event.current.mousePosition)) {
				_missionRule.SourceRule.RevokeAllSelected();
				_positionInCanvas = Event.current.mousePosition - _ruleReplacementCanvasInWindow.position + _replacementCanvasScrollPosition;
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
						tempConnection.StartPosition = _positionInCanvas;
						_missionRule.ReplacementRule.StickyNode(tempConnection, _positionInCanvas, "start");
					}
					// End point.
					else if (tempConnection.EndSelected) {
						tempConnection.EndPosition = _positionInCanvas;
						_missionRule.ReplacementRule.StickyNode(tempConnection, _positionInCanvas, "end");
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
			_listScrollPosition = GUILayout.BeginScrollView(_listScrollPosition, EditorStyle.AlphabetSymbolListHeight);
			// Content of scroll area.
			GUILayout.BeginArea(EditorStyle.AlphabetSymbolListArea);
			// Set the scroll position.
			_symbolListCanvasInWindow.position = GUIUtility.GUIToScreenPoint(EditorStyle.AlphabetSymbolListCanvas.position) - this.position.position;
			_symbolListCanvasInWindow.size     = _symbolListCanvasInWindow.size = EditorStyle.AlphabetSymbolListCanvas.size;
			EditorGUI.DrawRect(EditorStyle.AlphabetSymbolListCanvas, Color.gray);
			GUILayout.EndArea();
			// Layout each symbols in list.
			foreach (var node in Alphabet.Nodes) {
				Alphabet.DrawNodeInList(node);
				// Custom style to modify padding and margin for label.
				GUILayout.Label(node.ExpressName, EditorStyle.LabelInNodeList);
			}
			GUILayout.EndScrollView();
		}

		void LayoutConnectionList() {
			// Content of Connection-List.
			// Set the ScrollPosition.
			_listScrollPosition = GUILayout.BeginScrollView(_listScrollPosition, EditorStyle.AlphabetSymbolListHeight);
			// Content of scroll area.
			GUILayout.BeginArea(EditorStyle.AlphabetSymbolListArea);
			EditorGUI.DrawRect(EditorStyle.AlphabetSymbolListCanvas, Color.gray);
			GUILayout.EndArea();
			// Layout each symbols in list.:
			foreach (var connection in Alphabet.Connections) {
				Alphabet.DrawConnectionInList(connection);
				// Custom style to modify padding and margin for label.
				GUILayout.Label(connection.Name, EditorStyle.LabelInConnectionList);
			}
			GUILayout.EndScrollView();
		}
		// Buttons about adding new symbol, modifying and deleting.
		void LayoutEditingButtonGroup() {
			EditorGUILayout.BeginHorizontal();
			// Button of adding new symbol.
			GUI.enabled = (_currentSelectedGraphGrammar != null);
			if (GUILayout.Button("Add New", EditorStyles.miniButtonLeft, EditorStyle.ButtonHeight)) {
				// Add symbol.
				switch (_currentTab) {
				case SymbolEditingMode.AddNode:
					_currentSelectedGraphGrammar.AddNode(Alphabet.SelectedNode);
					break;
				case SymbolEditingMode.AddConnection:
					_currentSelectedGraphGrammar.AddConnection(Alphabet.SelectedConnection);
					break;
				}
				Repaint();
			}
			// Button of modifying new symbol.
			GUI.enabled = (_currentSelectedGraphGrammar != null && _currentSelectedGraphGrammar.SelectedSymbol != null);
			if (GUILayout.Button("Modify", EditorStyles.miniButtonMid, EditorStyle.ButtonHeight)) {
				switch (_currentTab) {
				case SymbolEditingMode.AddNode:
					_currentSelectedGraphGrammar.UpdateSymbol(_currentSelectedGraphGrammar.SelectedSymbol, Alphabet.SelectedNode);
					break;
				case SymbolEditingMode.AddConnection:
					_currentSelectedGraphGrammar.UpdateSymbol(_currentSelectedGraphGrammar.SelectedSymbol, Alphabet.SelectedConnection);
					break;
				}
				Repaint();
			}
			GUI.enabled = true;
			EditorGUILayout.EndHorizontal();
		}
		void ShowSourceCanvas() {
			// Set the scroll position.
			_sourceCanvasScrollPosition = GUILayout.BeginScrollView(_sourceCanvasScrollPosition, GUILayout.Width(Screen.width / 2), EditorStyle.RuleScrollViewHeight);
			// Content of canvas area.
			EditorStyle.ResizeRuleSourceCanvas(_sourceCanvasSizeWidth, _sourceCanvasSizeHeight);
			// If  this is current selected canvas, backgound will be white. Else gray.
			EditorGUI.DrawRect(EditorStyle.RuleSourceCanvas, _missionRule.SourceRule.Equals(_currentSelectedGraphGrammar) ? Color.white : Color.gray);
			GUILayout.Label(string.Empty, EditorStyle.RuleSourceCanvasContent);
			// Draw Nodes and Connections.
			GraphGrammarConnection _currentSelectedConnection = null;
			foreach (GraphGrammarConnection connection in _missionRule.SourceRule.Connections) {
				if (connection.Selected) {
					_currentSelectedConnection = connection;
				} else {
					connection.Draw();
				}
			}
			foreach (GraphGrammarNode node in _missionRule.SourceRule.Nodes) {
				node.Draw();
			}
			// Only selected connection need to place at the top.
			if (_currentSelectedConnection != null) {
				_currentSelectedConnection.Draw();
			}
			GUILayout.EndScrollView();
		}

		void ShowReplacementCanvas() {
			// Set the scroll position.
			_replacementCanvasScrollPosition = GUILayout.BeginScrollView(_replacementCanvasScrollPosition, GUILayout.Width(Screen.width / 2), EditorStyle.RuleScrollViewHeight);
			// Content of canvas area.
			EditorStyle.ResizeRuleReplacementCanvas(_replacementCanvasSizeWidth, _replacementCanvasSizeHeight);
			EditorGUI.DrawRect(EditorStyle.RuleReplacementCanvas, _missionRule.ReplacementRule.Equals(_currentSelectedGraphGrammar) ? Color.white : Color.gray);
			GUILayout.Label(string.Empty, EditorStyle.RuleReplacementCanvasContent);
			foreach (GraphGrammarNode node in _missionRule.ReplacementRule.Nodes) {
				node.Draw();
			}
			foreach (GraphGrammarConnection connection in _missionRule.ReplacementRule.Connections) {
				connection.Draw();
			}
			GUILayout.EndScrollView();
		}
		//Delete selected symbol.
		void DeleteSelectedNode() {
			//return while not selected symbol.
			if (_currentSelectedGraphGrammar.SelectedSymbol == null) { return; }
			if (_currentSelectedGraphGrammar.SelectedSymbol is GraphGrammarNode) {
				//Is node.
				_currentSelectedGraphGrammar.Nodes.Remove((GraphGrammarNode)_currentSelectedGraphGrammar.SelectedSymbol);
			}else if(_currentSelectedGraphGrammar.SelectedSymbol is GraphGrammarConnection) {
				//Is connection.
				_currentSelectedGraphGrammar.Connections.Remove((GraphGrammarConnection)_currentSelectedGraphGrammar.SelectedSymbol);
			}
			
		}
		//Copy selected canvas to another one.
		void CopySelectedCanvas() {
			if(_currentSelectedGraphGrammar != null && _currentSelectedGraphGrammar == _missionRule.SourceRule) {
				//Copy nodes.
				_missionRule.ReplacementRule.Nodes.Clear();
				foreach (GraphGrammarNode node in _missionRule.SourceRule.Nodes) {
					_missionRule.ReplacementRule.AddNode(node);
				}
				//Copy Connections.
				_missionRule.ReplacementRule.Connections.Clear();
				foreach (GraphGrammarConnection connection in _missionRule.SourceRule.Connections) {
					_missionRule.ReplacementRule.AddConnection(connection);
				}
				_missionRule.ReplacementRule.RevokeAllSelected();
			} else if(_currentSelectedGraphGrammar != null && _currentSelectedGraphGrammar == _missionRule.ReplacementRule) {
				//Copy nodes.
				_missionRule.SourceRule.Nodes.Clear();
				foreach (GraphGrammarNode node in _missionRule.ReplacementRule.Nodes) {
					_missionRule.SourceRule.AddNode(node);
				}
				//Copy connections.
				_missionRule.SourceRule.Connections.Clear();
				foreach (GraphGrammarConnection connection in _missionRule.ReplacementRule.Connections) {
					_missionRule.SourceRule.AddConnection(connection);
				}
				_missionRule.SourceRule.RevokeAllSelected();
			}
		}

		void AddGroup() {
			MissionGroup newGroup = new MissionGroup();
			newGroup.Name = _name;
			newGroup.Description = _description;
			MissionGrammar.AddGroup(newGroup);
			// Show the new one in dropdown list
			_indexOfGroupsOptions = _groupsOptions.Length;
		}

		void AddRule(int indexGroup) {
			MissionRule newRule = new MissionRule();
			newRule.Name = _name;
			newRule.Description = _description;
			MissionGrammar.Groups[indexGroup].AddRule(newRule);
			// Show the new one in dropdown list
			_indexOfRulesOptions = _rulesOptions.Length;
		}

		void UpdateGroup(MissionGroup group) {
			group.Name = _name;
			group.Description = _description;
		}

		void UpdateRule(MissionRule rule) {
			rule.Name = _name;
			rule.Description = _description;
		}
		// Return a boolean about name of group has never used before.
		bool IsGroupNameUsed(string nameNewGroup) {
			return (from usedGroup in MissionGrammar.Groups
					where usedGroup.Name.ToLower() == nameNewGroup.ToLower()
					select usedGroup)
				.Any();
		}
		// Return a boolean about name of rule has never used before.
		bool IsRuleNameUsed(string nameNewRule, int indexGroup) {
			return (from usedRule in MissionGrammar.Groups[indexGroup].Rules
					where usedRule.Name.ToLower() == nameNewRule.ToLower()
					select usedRule)
				.Any();
		}
	}
}
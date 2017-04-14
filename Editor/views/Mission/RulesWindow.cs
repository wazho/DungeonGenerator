using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System;
using Math = System.Math;
// Stylesheet.
using Style       = EditorExtend.CommonStyle;
using Container   = EditorExtend.MissionRuleWindow;
using GraphCanvas = EditorExtend.GraphCanvas;
using SymbolList  = EditorExtend.SymbolList;
using SampleStyle = EditorExtend.SampleStyle;

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
		private bool   _nameCanBeUsed;
		// Enabled Button-Apply
		private bool _applyEditingButtonEnabled;
		private bool _applySymbolEditingButtonEnabled;
		// The texture of icons.
		private Texture2D _editIcon;
		private Texture2D _deleteIcon;
		private Texture2D _redoTexture;
		private Texture2D _undoTexture;
		// The drawing canvas.
		private Rect _sourceCanvas;
		private Rect _replacementCanvas;
		private Rect _symbolListCanvasInWindow;
		// The scroll bar of canvas.
		private Vector2 _sourceCanvasScrollPosition;
		private Vector2 _replacementCanvasScrollPosition;
		// Size of source canvas & replacement canvas.
		private int _sourceCanvasWidth;
		private int _sourceCanvasHeight;
		private int _replacementCanvasWidth;
		private int _replacementCanvasHeight;
		// The scroll bar of list.
		private Vector2 _listScrollPosition;
		private Vector2 _positionInCanvas;
		private GraphGrammar _currentSelectedGraphGrammar;
		private StateRecorder _sourceRuleState = new StateRecorder();
		private StateRecorder _replaceRuleState = new StateRecorder();
		// [Will move to Style.cs]
		private static Rect _redoUndoArea = new Rect(Screen.width / 2 - 120, 0, 100, 25);
		public static Rect RedoUndoArea {
			get {
				_redoUndoArea.x = Screen.width / 2 - 120;
				return _redoUndoArea;
			}
		} 

		void Awake() {
			_editingMode          = EditingMode.None;
			_currentTab           = SymbolEditingMode.None;
			// Get the first rule of first group.
			_missionRule          = MissionGrammar.Groups[0].Rules[0];
			_groupsOptions        = MissionGrammar.Groups.Select(s => s.Name).ToArray();
			_rulesOptions         = MissionGrammar.Groups[0].Rules.Select(r => r.Name).ToArray();
			_indexOfGroupsOptions = 0;
			_indexOfRulesOptions  = 0;
			_tempIndexOfGroupsOptions = 0;
			_tempIndexOfRulesOptions  = 0;
			_name                 = string.Empty;
			_description          = string.Empty;
			_nameCanBeUsed = false;
			_applyEditingButtonEnabled = false;
			_applySymbolEditingButtonEnabled = false;
			_editIcon    = Resources.Load<Texture2D>("Icons/edit");
			_deleteIcon  = Resources.Load<Texture2D>("Icons/delete");
			_redoTexture = Resources.Load<Texture2D>("Icons/redo");
			_undoTexture = Resources.Load<Texture2D>("Icons/undo");
			_sourceCanvasScrollPosition      = Vector2.zero;
			_replacementCanvasScrollPosition = Vector2.zero;
			_listScrollPosition      = Vector2.zero;
			_sourceCanvasWidth       = Screen.width;
			_sourceCanvasHeight      = 200;
			_replacementCanvasWidth  = Screen.width;
			_replacementCanvasHeight = 200;
			_currentSelectedGraphGrammar = _missionRule.SourceRule;
			_sourceRuleState = new StateRecorder(_missionRule.SourceRule);
			_replaceRuleState = new StateRecorder(_missionRule.ReplacementRule);
			
			Alphabet.RevokeAllSelected();
		}
		void OnFocus() {
			_tempIndexOfGroupsOptions = -1;
			_tempIndexOfRulesOptions = -1;
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
		}
		// Layout the combobox and editor of mission group.
		void LayoutMissionGroupOptions() {
			// Current group.
			EditorGUILayout.BeginHorizontal();
			// Dropdown list of current group type.
			_indexOfGroupsOptions = EditorGUILayout.Popup("Current Group", _indexOfGroupsOptions, _groupsOptions);
			if (_tempIndexOfGroupsOptions != _indexOfGroupsOptions) {
				// Switch mode.
				_editingMode = EditingMode.None;
				_tempIndexOfGroupsOptions = _indexOfGroupsOptions;
				_indexOfRulesOptions = 0;
				// Update the rules of selected group.
				_rulesOptions = MissionGrammar.Groups[_indexOfGroupsOptions].Rules.Select(r => r.Name).ToArray();
				// Avoid the out of index.
				if (_indexOfGroupsOptions < _groupsOptions.Length && _indexOfRulesOptions < _rulesOptions.Length) {
					// Update the graph grammars below canvas.
					_missionRule = MissionGrammar.Groups[_indexOfGroupsOptions].Rules[_indexOfRulesOptions];
					_sourceRuleState = new StateRecorder(_missionRule.SourceRule);
					_replaceRuleState = new StateRecorder(_missionRule.ReplacementRule);
				}
				_currentSelectedGraphGrammar = null;
				ResizeResponsiveCanvas(_missionRule.SourceRule);
				ResizeResponsiveCanvas(_missionRule.ReplacementRule);
			}
			// Sub-button of editor, edit the group.
			if (GUILayout.Button(_editIcon, EditorStyles.miniButtonLeft, Style.ButtonHeight)) {
				// Switch mode.
				_editingMode = EditingMode.EditGroup;
				// Update info.
				_name        = MissionGrammar.Groups[_indexOfGroupsOptions].Name;
				_description = MissionGrammar.Groups[_indexOfGroupsOptions].Description;
			}
			// Sub-button of editor, delete the group.
			if (GUILayout.Button(_deleteIcon, EditorStyles.miniButtonMid, Style.ButtonHeight)) {
				// Switch mode.
				_editingMode = EditingMode.DeleteGroup;
				MissionGrammar.RemoveGroup(MissionGrammar.Groups[_indexOfGroupsOptions]);
				// If deleted the least one, add the new one.
				if (_groupsOptions.Length <= 1) { MissionGrammar.AddGroup(); }
				// Reset the index and mode.
				_indexOfGroupsOptions = 0;
				_editingMode = EditingMode.None;
			}
			// Sub-button of editor, create new group.
			if (GUILayout.Button("Add New", EditorStyles.miniButtonRight, Style.ButtonHeight)) {
				// Switch mode.
				_editingMode = EditingMode.CreateGroup;
				// Update info.
				_name        = MissionGrammar.GetDefaultGroupName(_groupsOptions);
				_description = "Description here.";
			}
			// Update the content of dropdown.
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
				// Switch mode.
				_editingMode             = EditingMode.None;
				_tempIndexOfRulesOptions = _indexOfRulesOptions;
				// Avoid the out of index.
				if (_indexOfGroupsOptions < _groupsOptions.Length && _indexOfRulesOptions < _rulesOptions.Length) {
					// Update the graph grammars below canvas.
					_missionRule = MissionGrammar.Groups[_indexOfGroupsOptions].Rules[_indexOfRulesOptions];
					_sourceRuleState = new StateRecorder(_missionRule.SourceRule);
					_replaceRuleState = new StateRecorder(_missionRule.ReplacementRule);
				}
				_currentSelectedGraphGrammar = null;
				ResizeResponsiveCanvas(_missionRule.SourceRule);
				ResizeResponsiveCanvas(_missionRule.ReplacementRule);
			}
			// Sub-button of editor, edit the rule.
			if (GUILayout.Button(_editIcon, EditorStyles.miniButtonLeft, Style.ButtonHeight)) {
				// Switch mode.
				_editingMode = EditingMode.EditRule;
				// Update info.
				_name        = MissionGrammar.Groups[_indexOfGroupsOptions].Rules[_indexOfRulesOptions].Name;
				_description = MissionGrammar.Groups[_indexOfGroupsOptions].Rules[_indexOfRulesOptions].Description;
			}
			// Sub-button of editor, delete the rule.
			if (GUILayout.Button(_deleteIcon, EditorStyles.miniButtonMid, Style.ButtonHeight)) {
				// Switch mode.
				_editingMode = EditingMode.DeleteRule;
				// Remove the rule from current group.
				MissionGrammar.Groups[_indexOfGroupsOptions].RemoveRule(MissionGrammar.Groups[_indexOfGroupsOptions].Rules[_indexOfRulesOptions]);
				// If deleted the least one, add the new one.
				if (_rulesOptions.Length <= 1) { MissionGrammar.Groups[_indexOfGroupsOptions].AddRule(); }
				// Reset the index and mode.
				_indexOfRulesOptions = 0;
				_editingMode = EditingMode.None;
			}
			// Sub-button of editor, create new rule.
			if (GUILayout.Button("Add New", EditorStyles.miniButtonRight, Style.ButtonHeight)) {
				// Switch mode.
				_editingMode = EditingMode.CreateRule;
				// Update info.
				_name        = MissionGrammar.GetDefaultRuleName(_rulesOptions, _indexOfGroupsOptions);
				_description = "Description here.";
			}
			// Update the content of dropdown.
			if (_indexOfGroupsOptions< MissionGrammar.Groups.Count) {
				_rulesOptions = MissionGrammar.Groups[_indexOfGroupsOptions].Rules.Select(r => r.Name).ToArray();
			}
			EditorGUILayout.EndHorizontal();
		}
		// Validate that the GroupName or RuleName is legal.
		private static Regex _nameStringNoDoubleSpace = new Regex(@"\s\s\w");
		private static Regex _nameEndOfStringNoSpace  = new Regex(@"\s$");
		// Layout the editor of mission group or mission rule.
		void LayoutBasicInfoEditor() {
			// Information of mission group or mission rule.
			switch (_editingMode) {
			case EditingMode.EditGroup:
				// Text fields.
				_name        = EditorGUILayout.TextField("Group Name", _name);
				_description = EditorGUILayout.TextField("Group Description", _description);
				// Check the name has never used before.
				_nameCanBeUsed = ! MissionGrammar.IsGroupNameUsed(_name);
				break;
			case EditingMode.CreateGroup:
				// Text fields.
				_name        = EditorGUILayout.TextField("New Group Name", _name);
				_description = EditorGUILayout.TextField("Group Description", _description);
				// Check the name has never used before.
				_nameCanBeUsed = ! MissionGrammar.IsGroupNameUsed(_name);
				break;
			case EditingMode.EditRule:
				// Text fields.
				_name        = EditorGUILayout.TextField("Rule Name", _name);
				_description = EditorGUILayout.TextField("Rule Description", _description);
				// Check the name has never used before.
				_nameCanBeUsed = ! MissionGrammar.IsRuleNameUsed(_name, _indexOfGroupsOptions);
				break;
			case EditingMode.CreateRule:
				// Text fields.
				_name        = EditorGUILayout.TextField("New Rule Name", _name);
				_description = EditorGUILayout.TextField("Rule Description", _description);
				// Check the name has never used before.
				_nameCanBeUsed = ! MissionGrammar.IsRuleNameUsed(_name, _indexOfGroupsOptions);
				break;
			}
			// [TODO] Data validation. Move this part.
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
			if (_name != string.Empty && _description == string.Empty && _nameCanBeUsed == true && _nameStringNoDoubleSpace.IsMatch(_name) == true && _nameEndOfStringNoSpace.IsMatch(_name) == true) {
				_applyEditingButtonEnabled = false;
				EditorGUILayout.HelpBox("Info \nThe name is illegal. \nCan't use more than one space continuously. \nEnd of name string can't be space. \nThe description is empty.", MessageType.Info);
			}
			if (_name != string.Empty && _description == string.Empty && _nameCanBeUsed == true && _nameStringNoDoubleSpace.IsMatch(_name) == true) {
				_applyEditingButtonEnabled = false;
				EditorGUILayout.HelpBox("Info \nThe name is illegal. \nCan't use more than one space continuously. \nThe description is empty.", MessageType.Info);
			}
			if (_name != string.Empty && _description == string.Empty && _nameCanBeUsed == true && _nameEndOfStringNoSpace.IsMatch(_name) == true) {
				_applyEditingButtonEnabled = false;
				EditorGUILayout.HelpBox("Info \nThe name is illegal. \nEnd of name string can't be space. \nThe description is empty.", MessageType.Info);
			}
			if (_name != string.Empty && _description != string.Empty && _nameCanBeUsed == false) {
				_applyEditingButtonEnabled = false;
				EditorGUILayout.HelpBox("Info \nThe name has been used before.", MessageType.Info);
			}
			if (_name != string.Empty && _description != string.Empty && _nameCanBeUsed == true && _nameStringNoDoubleSpace.IsMatch(_name) == true && _nameEndOfStringNoSpace.IsMatch(_name) == true) {
				_applyEditingButtonEnabled = false;
				EditorGUILayout.HelpBox("Info \nThe name is illegal. \nCan't use more than one space continuously. \nEnd of name string can't be space.", MessageType.Info);
			}
			if (_name != string.Empty && _description != string.Empty && _nameCanBeUsed == true && _nameStringNoDoubleSpace.IsMatch(_name) == true) {
				_applyEditingButtonEnabled = false;
				EditorGUILayout.HelpBox("Info \nThe name is illegal. \nCan't use more than one space continuously.", MessageType.Info);
			}
			if (_name != string.Empty && _description != string.Empty && _nameCanBeUsed == true && _nameEndOfStringNoSpace.IsMatch(_name) == true) {
				_applyEditingButtonEnabled = false;
				EditorGUILayout.HelpBox("Info \nThe name is illegal. \nEnd of name string can't be space.", MessageType.Info);
			}
			if (_name != string.Empty && _description != string.Empty && _nameCanBeUsed == true && _nameStringNoDoubleSpace.IsMatch(_name) == false && _nameEndOfStringNoSpace.IsMatch(_name) == false) {
				_applyEditingButtonEnabled = true;
				EditorGUILayout.HelpBox("Info \nNothing.", MessageType.Info);
			}
			// Submit button.
			GUI.enabled = _applyEditingButtonEnabled;
			if (GUILayout.Button("Apply", EditorStyles.miniButton, Style.ButtonHeight)) {
				if (EditorUtility.DisplayDialog("Saving", 
					"Are you sure to save?",
					"Yes", "No")) {
					switch (_editingMode) {
					case EditingMode.EditGroup:
						MissionGrammar.Groups[_indexOfGroupsOptions].Name        = _name;
						MissionGrammar.Groups[_indexOfGroupsOptions].Description = _description;
						break;
					case EditingMode.CreateGroup:
						MissionGrammar.AddGroup(_name, _description);
						_indexOfGroupsOptions = _groupsOptions.Length;
						break;
					case EditingMode.EditRule:
						MissionGrammar.Groups[_indexOfGroupsOptions].Rules[_indexOfRulesOptions].Name        = _name;
						MissionGrammar.Groups[_indexOfGroupsOptions].Rules[_indexOfRulesOptions].Description = _description;
						break;
					case EditingMode.CreateRule:
						MissionGrammar.Groups[_indexOfGroupsOptions].AddRule(_name, _description);
						_indexOfRulesOptions = _rulesOptions.Length;
						break;
					}
					// Reset the mode.
					_editingMode = EditingMode.None;
					// Unfocus from the field.
					GUI.FocusControl("FocusToNothing");
				} else {

				}
			}
			GUI.enabled = true;
		}
		// Layout the canvas areas of two graph grammars.
		void LayoutRulesCanvasArea() {
			GUILayout.BeginArea(Container.RulesArea);
			// Information of Source and Replacement.
			EditorGUILayout.BeginHorizontal();
			GUILayout.Label("Source", Style.HeaderTwo, GUILayout.Width(Screen.width / 2));
			GUILayout.Label("Replacement", Style.HeaderTwo, GUILayout.Width(Screen.width / 2));
			EditorGUILayout.EndHorizontal();

			// SourceCanvas
			GUILayout.BeginArea(Container.SourceRuleArea);
			// Get the Rect in EditWindow from the GUI rect. (Position = Real screen position - this EditWindow position)
			_sourceCanvas.position = GUIUtility.GUIToScreenPoint(Container.RuleGraphGrammarCanvas.position) - this.position.position;
			_sourceCanvas.size     = Container.RuleGraphGrammarCanvas.size;
			// Show the source canvas.
			ShowSourceCanvas();
			GUILayout.EndArea();

			// ReplacementCanvas
			GUILayout.BeginArea(Container.ReplacementRuleArea);
			_replacementCanvas.position = GUIUtility.GUIToScreenPoint(Container.RuleGraphGrammarCanvas.position) - this.position.position;
			_replacementCanvas.size     = Container.RuleGraphGrammarCanvas.size;
			// Show the replacement canvas.
			ShowReplacementCanvas();
			GUILayout.EndArea();
			GUILayout.EndArea();
		}
		// Layout the canvas editor of current selected rules.
		void LayoutRuleCanvasEditor() {
			// Show ordering slider
			GUILayout.BeginArea(Container.OrderingSliderArea);
			if (_currentSelectedGraphGrammar != null && _currentSelectedGraphGrammar.SelectedSymbol is GraphGrammarNode) {
				int sliderOrdering = EditorGUILayout.IntSlider("Ordering", _currentSelectedGraphGrammar.SelectedSymbol.Ordering, 1, _currentSelectedGraphGrammar.Nodes.Count);
				if (sliderOrdering != _currentSelectedGraphGrammar.SelectedSymbol.Ordering) {
					GraphGrammarNode node = _currentSelectedGraphGrammar.Nodes.Find(x => x.Ordering == sliderOrdering);
					if (node != null) {
						node.Ordering = _currentSelectedGraphGrammar.SelectedSymbol.Ordering;
					}
					_currentSelectedGraphGrammar.SelectedSymbol.Ordering = sliderOrdering;
					RecordState();
				}

			}
			GUILayout.EndArea();

			GUILayout.BeginArea(Container.EditorArea);
			// Buttons - Add Node & Add Connection & Copy & Delete.
			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button("Add Node", EditorStyles.miniButtonLeft, Style.ButtonHeight)) {
				// Add Alphabet's Node 
				_applySymbolEditingButtonEnabled = true;
				_currentTab = SymbolEditingMode.AddNode;
			}
			if (GUILayout.Button("Add Connection", EditorStyles.miniButtonMid, Style.ButtonHeight)) {
				_applySymbolEditingButtonEnabled = true;
				_currentTab = SymbolEditingMode.AddConnection;
			}
			if (GUILayout.Button("Copy", EditorStyles.miniButtonMid, Style.ButtonHeight)) {
				_applySymbolEditingButtonEnabled = true;
				_currentTab = SymbolEditingMode.Copy;
			}
			if (GUILayout.Button("Delete", EditorStyles.miniButtonRight, Style.ButtonHeight)) {
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
			if (GUILayout.Button("Apply", EditorStyles.miniButton, Style.ButtonHeight)) {
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
				if (Event.current.delta.magnitude > 1.0f) {
					OnDraggedAndDroppedInCanvas();
				}
			} else if (Event.current.type == EventType.MouseUp) {
				OnMouseUpInCanvas();
			}
		}
		// Click Event
		void OnClickedSymbolInCanvas() {
			if (_sourceCanvas.Contains(Event.current.mousePosition)) {
				_currentSelectedGraphGrammar = _missionRule.SourceRule;
				_missionRule.ReplacementRule.RevokeAllSelected();
				_missionRule.SourceRule.TouchedSymbol(Event.current.mousePosition - _sourceCanvas.position + _sourceCanvasScrollPosition);
				Repaint();
			} else if (_replacementCanvas.Contains(Event.current.mousePosition)) {
				_currentSelectedGraphGrammar = _missionRule.ReplacementRule;
				_missionRule.SourceRule.RevokeAllSelected();
				_missionRule.ReplacementRule.TouchedSymbol(Event.current.mousePosition - _replacementCanvas.position + _replacementCanvasScrollPosition);
				Repaint();
			} else if (_symbolListCanvasInWindow .Contains(Event.current.mousePosition)) {
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
		private static GraphGrammarNode       _tempNode;
		private static GraphGrammarConnection _tempConnection;
		private static bool _tempSticked;
		void OnDraggedAndDroppedInCanvas() {
			_tempSticked = false;
			// If mouse position is in the canvas of source rule. 
			if (_sourceCanvas.Contains(Event.current.mousePosition)) {
				// Revoke all of the selected in replacement canvas.
				_missionRule.ReplacementRule.RevokeAllSelected();
				_positionInCanvas = Event.current.mousePosition - _sourceCanvas.position + _sourceCanvasScrollPosition;
				// Select node.
				if (_missionRule.SourceRule.SelectedSymbol is GraphGrammarNode) {
					_tempNode = (GraphGrammarNode) _missionRule.SourceRule.SelectedSymbol;
					_tempNode.Position = _positionInCanvas;
				}
				// Select connection.
				else if (_missionRule.SourceRule.SelectedSymbol is GraphGrammarConnection) {
					_tempConnection = (GraphGrammarConnection) _missionRule.SourceRule.SelectedSymbol;
					// Start point.
					if (_tempConnection.StartSelected) {
						_tempConnection.StartPosition = _positionInCanvas;
						if(_missionRule.SourceRule.StickyNode(_tempConnection, _positionInCanvas, "start")) {
							_tempSticked = true;
						} else {
							_tempSticked = false;
						}
					}
					// End point.
					else if (_tempConnection.EndSelected) {
						_tempConnection.EndPosition = _positionInCanvas;
						if(_missionRule.SourceRule.StickyNode(_tempConnection, _positionInCanvas, "end")) {
							_tempSticked = true;
						} else {
							_tempSticked = false;
						}
					}
				}
				// When drag and drop the nodes, auto-resize the canvas size.
				ResizeResponsiveCanvas(_currentSelectedGraphGrammar);
				// Refresh the layout.
				Repaint();
			}
			// If mouse position is in the canvas of replacement rule. 
			else if (_replacementCanvas.Contains(Event.current.mousePosition)) {
				// Revoke all of the selected in source canvas.
				_missionRule.SourceRule.RevokeAllSelected();
				_positionInCanvas = Event.current.mousePosition - _replacementCanvas.position + _replacementCanvasScrollPosition;
				// Select node.
				if (_missionRule.ReplacementRule.SelectedSymbol is GraphGrammarNode) {
					_tempNode = (GraphGrammarNode) _missionRule.ReplacementRule.SelectedSymbol;
					_tempNode.Position = _positionInCanvas;
				}
				// Select connection.
				else if (_missionRule.ReplacementRule.SelectedSymbol is GraphGrammarConnection) {
					_tempConnection = (GraphGrammarConnection) _missionRule.ReplacementRule.SelectedSymbol;
					// Start point.
					if (_tempConnection.StartSelected) {
						_tempConnection.StartPosition = _positionInCanvas;
						if(_missionRule.ReplacementRule.StickyNode(_tempConnection, _positionInCanvas, "start")) {
							_tempSticked = true;
						} else {
							_tempSticked = false;
						}
					}
					// End point.
					else if (_tempConnection.EndSelected) {
						_tempConnection.EndPosition = _positionInCanvas;
						if (_missionRule.ReplacementRule.StickyNode(_tempConnection, _positionInCanvas, "end")) {
							_tempSticked = true;
						} else {
							_tempSticked = false;
						}
					}
				}
				// When drag and drop the nodes, auto-resize the canvas size.
				ResizeResponsiveCanvas(_currentSelectedGraphGrammar);
				// Refresh the layout.
				Repaint();
			}
			// Release.
			_tempNode = null;
			_tempConnection = null;
		}
		// Mouse up event
		void OnMouseUpInCanvas() {
			OnDraggedAndDroppedInCanvas();
			// When mouse up and selected conecction stick successfully then record state.
			if (_tempSticked) {
				RecordState();
			}
		}

		// When drag and drop the nodes, auto-resize the canvas size.
		// [TODO] Fixed the magic number after the Style.cs optimizing.
		void ResizeResponsiveCanvas(GraphGrammar graph) {
			if (graph == _missionRule.SourceRule) {
				if (_missionRule.SourceRule.Nodes.Any()) {
					_sourceCanvasWidth  = (int) Math.Max(_missionRule.SourceRule.Nodes.Max(n => n.PositionX) + 150, Screen.width / 2);
					_sourceCanvasHeight = (int) Math.Max(_missionRule.SourceRule.Nodes.Max(n => n.PositionY) + 150, 300);
				} else {
					_sourceCanvasWidth  = Screen.width / 2;
					_sourceCanvasHeight = 300;
				}
			} else if (graph == _missionRule.ReplacementRule) {
				if (_missionRule.ReplacementRule.Nodes.Any()) {
					_replacementCanvasWidth  = (int) Math.Max(_missionRule.ReplacementRule.Nodes.Max(n => n.PositionX) + 150, Screen.width / 2);
					_replacementCanvasHeight = (int) Math.Max(_missionRule.ReplacementRule.Nodes.Max(n => n.PositionY) + 150, 300);
				} else {
					_replacementCanvasWidth  = Screen.width / 2;
					_replacementCanvasHeight = 300;
				}
			}
		}

		void LayoutNodeList() {
			// Content of Node-List.
			_listScrollPosition = GUILayout.BeginScrollView(_listScrollPosition, SymbolList.HeightLayout);
			// Content of scroll area.
			GUILayout.BeginArea(Container.SymbolListArea);
			// Set the scroll position.
			_symbolListCanvasInWindow.position = GUIUtility.GUIToScreenPoint(Container.SymbolListCanvas.position) - this.position.position;
			_symbolListCanvasInWindow.size     = _symbolListCanvasInWindow.size = Container.SymbolListCanvas.size;
			EditorGUI.DrawRect(Container.SymbolListCanvas, Color.gray);
			GUILayout.EndArea();
			// Layout each symbols in list.
			foreach (var node in Alphabet.Nodes) {
				Alphabet.DrawNodeInList(node);
				// Custom style to modify padding and margin for label.
				GUILayout.Label(node.ExpressName, SymbolList.NodeElement);
			}
			GUILayout.EndScrollView();
		}

		void LayoutConnectionList() {
			// Content of Connection-List.
			// Set the ScrollPosition.
			_listScrollPosition = GUILayout.BeginScrollView(_listScrollPosition, SymbolList.HeightLayout);
			// Content of scroll area.
			GUILayout.BeginArea(Container.SymbolListArea);
			// Set the scroll position.
			_symbolListCanvasInWindow.position = GUIUtility.GUIToScreenPoint(Container.SymbolListCanvas.position) - this.position.position;
			_symbolListCanvasInWindow.size     = _symbolListCanvasInWindow.size = Container.SymbolListCanvas.size;
			EditorGUI.DrawRect(Container.SymbolListCanvas, Color.gray);
			GUILayout.EndArea();
			// Layout each symbols in list.:
			foreach (var connection in Alphabet.Connections) {
				Alphabet.DrawConnectionInList(connection);
				// Custom style to modify padding and margin for label.
				GUILayout.Label(connection.Name, SymbolList.ConnectionElement);
			}
			GUILayout.EndScrollView();
		}
		// Buttons about adding new symbol, modifying and deleting.
		void LayoutEditingButtonGroup() {
			EditorGUILayout.BeginHorizontal();
			// Button of adding new symbol.
			switch (_currentTab) {
			case SymbolEditingMode.AddNode:
				GUI.enabled = (_currentSelectedGraphGrammar != null && Alphabet.SelectedNode != null );
				break;
			case SymbolEditingMode.AddConnection:
				GUI.enabled = (_currentSelectedGraphGrammar != null && Alphabet.SelectedConnection != null );
				break;
			}
			if (GUILayout.Button("Add New", EditorStyles.miniButtonLeft, Style.ButtonHeight)) {
				// Add symbol.
				switch (_currentTab) {
				case SymbolEditingMode.AddNode:
					GraphGrammarNode newNode = _currentSelectedGraphGrammar.AddNode(Alphabet.SelectedNode);
					if (_currentSelectedGraphGrammar == _missionRule.SourceRule) {
						newNode.Position = _sourceCanvasScrollPosition + new Vector2(30, 30);
					} else if (_currentSelectedGraphGrammar == _missionRule.ReplacementRule) {
						newNode.Position = _replacementCanvasScrollPosition + new Vector2(30, 30);
					}
					RecordState();
					break;
				case SymbolEditingMode.AddConnection:
					GraphGrammarNode selectedNode = null;
					if (_currentSelectedGraphGrammar.SelectedSymbol is GraphGrammarNode) {
						selectedNode = (GraphGrammarNode) _currentSelectedGraphGrammar.SelectedSymbol;
					} 
					GraphGrammarConnection newConnection = _currentSelectedGraphGrammar.AddConnection(Alphabet.SelectedConnection);
					if (selectedNode != null) {
						// Auto stick on the node.
						_currentSelectedGraphGrammar.StickyNode(newConnection, selectedNode.Position, "start");
						newConnection.EndPosition = selectedNode.Position + new Vector2(35, 35);
					} else {
						// Appear the connection on the left-top of current canvas scroll position.
						if (_currentSelectedGraphGrammar == _missionRule.SourceRule) {
							newConnection.StartPosition = _sourceCanvasScrollPosition + new Vector2(10, 20);
							newConnection.EndPosition   = _sourceCanvasScrollPosition + new Vector2(60, 20);
						} else if (_currentSelectedGraphGrammar == _missionRule.ReplacementRule) {
							newConnection.StartPosition = _replacementCanvasScrollPosition + new Vector2(10, 20);
							newConnection.EndPosition   = _replacementCanvasScrollPosition + new Vector2(60, 20);
						}
					}
					RecordState();
					break;
				}
				Repaint();
			}
			// Button of modifying new symbol.
			switch (_currentTab) {
			case SymbolEditingMode.AddNode:
				GUI.enabled = (GUI.enabled && _currentSelectedGraphGrammar.SelectedSymbol is GraphGrammarNode);
				break;
			case SymbolEditingMode.AddConnection:
				GUI.enabled = (GUI.enabled && _currentSelectedGraphGrammar.SelectedSymbol is GraphGrammarConnection);
				break;
			}
			if (GUILayout.Button("Modify", EditorStyles.miniButtonMid, Style.ButtonHeight)) {
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
			_sourceCanvasScrollPosition = GUILayout.BeginScrollView(_sourceCanvasScrollPosition, GUILayout.Width(Screen.width / 2), GraphCanvas.RuleScrollViewHeightLayout);
			// Content of canvas area.
			GraphCanvas.ResizeSourceCanvas(_sourceCanvasWidth, _sourceCanvasHeight);
			// If  this is current selected canvas, backgound will be white. Else gray.
			EditorGUI.DrawRect(GraphCanvas.SourceCanvas, _missionRule.SourceRule.Equals(_currentSelectedGraphGrammar) ? Color.white : Color.gray);
			SampleStyle.DrawGrid(GraphCanvas.SourceCanvas, 20, 100, SampleStyle.ColorBlue, SampleStyle.ColorDarkestBlue);
			GUILayout.Label(string.Empty, GraphCanvas.SourceCanvasContent);
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
			// Redo & Undo Area
			GUILayout.BeginArea(RedoUndoArea);
			GUILayout.BeginHorizontal();
			// Set the button disabled when it have no undo state.
			EditorGUI.BeginDisabledGroup(!_sourceRuleState.hasUndoState);
			if (GUILayout.Button(_undoTexture, EditorStyles.miniButtonRight, Style.ButtonHeight)) {
				// Undo.
				_currentSelectedGraphGrammar = _missionRule.SourceRule;
				UndoState();
				Repaint();
				_currentTab = SymbolEditingMode.None;
			}
			// Set the button disabled when it have no redo state.
			EditorGUI.BeginDisabledGroup(!_sourceRuleState.hasRedoState);
			if (GUILayout.Button(_redoTexture, EditorStyles.miniButtonLeft, Style.ButtonHeight)) {
				// Redo.
				_currentSelectedGraphGrammar = _missionRule.SourceRule;
				RedoState();
				Repaint();
				_currentTab = SymbolEditingMode.None;
			}
			EditorGUI.EndDisabledGroup();
			EditorGUI.EndDisabledGroup();
			GUILayout.EndHorizontal();
			GUILayout.EndArea();
		}

		void ShowReplacementCanvas() {
			// Set the scroll position.
			_replacementCanvasScrollPosition = GUILayout.BeginScrollView(_replacementCanvasScrollPosition, GUILayout.Width(Screen.width / 2), GraphCanvas.RuleScrollViewHeightLayout);
			// Content of canvas area.
			GraphCanvas.ResizeReplacementCanvas(_replacementCanvasWidth, _replacementCanvasHeight);
			EditorGUI.DrawRect(GraphCanvas.ReplacementCanvas, _missionRule.ReplacementRule.Equals(_currentSelectedGraphGrammar) ? Color.white : Color.gray);
			GUILayout.Label(string.Empty, GraphCanvas.ReplacementCanvasContent);
			// Draw Nodes and Connections.
			GraphGrammarConnection _currentSelectedConnection = null;
			foreach (GraphGrammarConnection connection in _missionRule.ReplacementRule.Connections) {
				if (connection.Selected) {
					_currentSelectedConnection = connection;
				} else {
					connection.Draw();
				}
			}
			foreach (GraphGrammarNode node in _missionRule.ReplacementRule.Nodes) {
				node.Draw();
			}
			// Only selected connection need to place at the top.
			if (_currentSelectedConnection != null) {
				_currentSelectedConnection.Draw();
			}
			GUILayout.EndScrollView();
			// Redo & Undo Area
			GUILayout.BeginArea(RedoUndoArea);
			GUILayout.BeginHorizontal();
			// Set the button disabled when it have no undo state.
			EditorGUI.BeginDisabledGroup(!_replaceRuleState.hasUndoState);
			if (GUILayout.Button(_undoTexture, EditorStyles.miniButtonRight, Style.ButtonHeight)) {
				// Undo.
				_currentSelectedGraphGrammar = _missionRule.ReplacementRule;
				UndoState();
				Repaint();
				_currentTab = SymbolEditingMode.None;
			}
			// Set the button disabled when it have no redo state.
			EditorGUI.BeginDisabledGroup(!_replaceRuleState.hasRedoState);
			if (GUILayout.Button(_redoTexture, EditorStyles.miniButtonLeft, Style.ButtonHeight)) {
				// Redo.
				_currentSelectedGraphGrammar = _missionRule.ReplacementRule;
				RedoState();
				Repaint();
				_currentTab = SymbolEditingMode.None;
			}
			EditorGUI.EndDisabledGroup();
			EditorGUI.EndDisabledGroup();
			GUILayout.EndHorizontal();
			GUILayout.EndArea();
		}
		// Delete selected symbol.
		void DeleteSelectedNode() {
			//return while not selected symbol.
			if (_currentSelectedGraphGrammar.SelectedSymbol == null) { return; }
			if (_currentSelectedGraphGrammar.SelectedSymbol is GraphGrammarNode) {
				// Is node.
				GraphGrammarNode node = (GraphGrammarNode) _currentSelectedGraphGrammar.SelectedSymbol;
				foreach (var connection in _currentSelectedGraphGrammar.Connections) {
					node.RemoveStickiedConnection(connection, "start");
					node.RemoveStickiedConnection(connection, "end");
				}
				GraphGrammarNode[] nodesOrdering = _currentSelectedGraphGrammar.Nodes.OrderBy(x => x.Ordering).ToArray();
				// Following ordering -1
				for (int i = node.Ordering; i < nodesOrdering.Length; i++) {
					nodesOrdering[i].Ordering -= 1;
				}
				_currentSelectedGraphGrammar.Nodes.Remove(node);
				_currentSelectedGraphGrammar.SelectedSymbol = null;
				// Record state when node has deleted.
				RecordState();
			} else if (_currentSelectedGraphGrammar.SelectedSymbol is GraphGrammarConnection) {
				// Is connection.
				GraphGrammarConnection connection = (GraphGrammarConnection) _currentSelectedGraphGrammar.SelectedSymbol;
				if (connection.StartpointStickyOn != null) {
					connection.StartpointStickyOn.RemoveStickiedConnection(connection, "start");
				}
				if (connection.EndpointStickyOn != null) {
					connection.EndpointStickyOn.RemoveStickiedConnection(connection, "end");
				}
				_currentSelectedGraphGrammar.Connections.Remove(connection);
				_currentSelectedGraphGrammar.SelectedSymbol = null;
				// Record state when connection has deleted.
				RecordState();
			}
			
		}
		// Copy selected canvas to another one.
		void CopySelectedCanvas() {
			if (_currentSelectedGraphGrammar != null && _currentSelectedGraphGrammar == _missionRule.SourceRule) {
				// Copy nodes.
				_missionRule.ReplacementRule.Nodes.Clear();
				foreach (GraphGrammarNode node in _missionRule.SourceRule.Nodes) {
					_missionRule.ReplacementRule.AddNode(node);
				}
				// Copy Connections.
				_missionRule.ReplacementRule.Connections.Clear();
				foreach (GraphGrammarConnection connection in _missionRule.SourceRule.Connections) {
					_missionRule.ReplacementRule.AddConnection(connection);
					_missionRule.ReplacementRule.StickyNode(_missionRule.ReplacementRule.Connections.LastOrDefault(), connection.StartPosition, "start");
					_missionRule.ReplacementRule.StickyNode(_missionRule.ReplacementRule.Connections.LastOrDefault(), connection.EndPosition, "end");
				}
				_missionRule.ReplacementRule.RevokeAllSelected();
				// Record state when grammar has copied. 
				RecordState(_missionRule.ReplacementRule);
			} else if (_currentSelectedGraphGrammar != null && _currentSelectedGraphGrammar == _missionRule.ReplacementRule) {
				// Copy nodes.
				_missionRule.SourceRule.Nodes.Clear();
				foreach (GraphGrammarNode node in _missionRule.ReplacementRule.Nodes) {
					_missionRule.SourceRule.AddNode(node);
				}
				// Copy connections.
				_missionRule.SourceRule.Connections.Clear();
				foreach (GraphGrammarConnection connection in _missionRule.ReplacementRule.Connections) {
					_missionRule.SourceRule.AddConnection(connection);
					_missionRule.SourceRule.StickyNode(_missionRule.SourceRule.Connections.LastOrDefault(), connection.StartPosition, "start");
					_missionRule.SourceRule.StickyNode(_missionRule.SourceRule.Connections.LastOrDefault(), connection.EndPosition, "end");
					
				}
				_missionRule.SourceRule.RevokeAllSelected();
				// Record state when grammar has copied. 
				RecordState(_missionRule.SourceRule);
			}
		}
		// Record State via _currentSelectedGraphGrammar.
		void RecordState() {
			if (_currentSelectedGraphGrammar == _missionRule.SourceRule) {
				_sourceRuleState.AddState(_currentSelectedGraphGrammar);
			} else if (_currentSelectedGraphGrammar == _missionRule.ReplacementRule) {
				_replaceRuleState.AddState(_currentSelectedGraphGrammar);
			}
		}
		// Record State via GraphGrammar parameter.
		void RecordState(GraphGrammar graph) {
			if (graph == _missionRule.SourceRule) {
				_sourceRuleState.AddState(graph);
			} else if (graph == _missionRule.ReplacementRule) {
				_replaceRuleState.AddState(graph);
			}
		}
		// Undo via _currentSelectedGraphGrammar.
		void UndoState() {
			if (_currentSelectedGraphGrammar == _missionRule.SourceRule) {
				_sourceRuleState.Undo(ref _currentSelectedGraphGrammar);
			} else if (_currentSelectedGraphGrammar == _missionRule.ReplacementRule) {
				_replaceRuleState.Undo(ref _currentSelectedGraphGrammar);
			}
		}
		// Redo via _currentSelectedGraphGrammar.
		void RedoState() {
			if (_currentSelectedGraphGrammar == _missionRule.SourceRule) {
				_sourceRuleState.Redo(ref _currentSelectedGraphGrammar);
			} else if (_currentSelectedGraphGrammar == _missionRule.ReplacementRule) {
				_replaceRuleState.Redo(ref _currentSelectedGraphGrammar);
			}
		}
		// The class used to record the state and execute redo/undo.
		class StateRecorder {
			// Default constructor.
			public StateRecorder() {
				_states = new List<State>() { new State() };
				_index = 0;
			}
			// A constructor via GraphGrammar parameter.
			public StateRecorder(GraphGrammar graph) {
				// Add the GraphGrammar as origin.
				_states = new List<State>() { new State(graph)};
				_index = 0;
			}
			// Return true when this state can undo.
			public bool hasUndoState {
				get { return _index > 0; }
			}
			// Return true when this state can redo.
			public bool hasRedoState {
				get { return  _index < _states.Count - 1 ; }
			}
			// Add state
			public void AddState(GraphGrammar graph) {
				// Remove redo range.
				if(_index < _states.Count - 1) {
					_states.RemoveRange(_index + 1, _states.Count - 1 - _index);
				}
				// Add state.
				_states.Add(new State(graph));
				// Full then remove it. Now can store 10 state(contain current state).
				if (_states.Count > 10) {
					_states.RemoveAt(0);
				} else {
					_index++;
				}
			}
			// Undo.
			public void Undo(ref GraphGrammar graph) {
				// Back to previous state.
				_index--;
				State state = _states[_index];
				// Transform state into GraphGrammar.
				// Deep copy.
				graph.Nodes = new List<GraphGrammarNode>();
				for (int i = 0; i < state.Nodes.Length; i++) {
					graph.Nodes.Add(new GraphGrammarNode(state.Nodes[i]));
					graph.Nodes[i].Position = state.Nodes[i].Position;
					graph.Nodes[i].Ordering = state.Nodes[i].Ordering;
				}
				graph.Connections = new List<GraphGrammarConnection>();
				for (int i = 0; i < state.Connections.Length; i++) {
					graph.Connections.Add(new GraphGrammarConnection(state.Connections[i]));
					graph.Connections[i].StartPosition = state.Connections[i].StartPosition;
					graph.Connections[i].EndPosition = state.Connections[i].EndPosition;
					graph.Connections[i].Ordering = state.Connections[i].Ordering;
					// Stick.
					graph.StickyNode(graph.Connections[i], graph.Connections[i].StartPosition, "start");
					graph.StickyNode(graph.Connections[i], graph.Connections[i].EndPosition, "end");
				}
				graph.RevokeAllSelected();
			}
			public void Redo(ref GraphGrammar graph) {
				// Next state.
				_index++;
				State state = _states[_index];
				// Transform state into GraphGrammar.
				// Deep copy.
				graph.Nodes = new List<GraphGrammarNode>();
				for (int i = 0; i < state.Nodes.Length; i++) {
					graph.Nodes.Add(new GraphGrammarNode(state.Nodes[i]));
					graph.Nodes[i].Position = state.Nodes[i].Position;
					graph.Nodes[i].Ordering = state.Nodes[i].Ordering;
				}
				graph.Connections = new List<GraphGrammarConnection>();
				for (int i = 0; i < state.Connections.Length; i++) {
					graph.Connections.Add(new GraphGrammarConnection(state.Connections[i]));
					graph.Connections[i].StartPosition = state.Connections[i].StartPosition;
					graph.Connections[i].EndPosition = state.Connections[i].EndPosition;
					graph.Connections[i].Ordering = state.Connections[i].Ordering;
					// Stick.
					graph.StickyNode(graph.Connections[i], graph.Connections[i].StartPosition, "start");
					graph.StickyNode(graph.Connections[i], graph.Connections[i].EndPosition, "end");
				}
				graph.RevokeAllSelected();
			}
			
			private List<State> _states;
			private int _index;

			// State struct used to save nodes and connections. 
			private struct State {
				public GraphGrammarNode[] Nodes;
				public GraphGrammarConnection[] Connections;
				// Transform GraphGrammar into State.
				public State(GraphGrammar graph) {
					// Deep copy.
					Nodes = new GraphGrammarNode[graph.Nodes.Count];
					for (int i = 0; i < graph.Nodes.Count; i++) {
						Nodes[i] = new GraphGrammarNode(graph.Nodes[i]);
						Nodes[i].Position = graph.Nodes[i].Position;
						Nodes[i].Ordering = graph.Nodes[i].Ordering;
					}
					Connections = new GraphGrammarConnection[graph.Connections.Count];
					for (int i = 0; i < graph.Connections.Count; i++) {
						Connections[i] = new GraphGrammarConnection(graph.Connections[i]);
						Connections[i].StartPosition = graph.Connections[i].StartPosition;
						Connections[i].EndPosition = graph.Connections[i].EndPosition;
						Connections[i].Ordering = graph.Connections[i].Ordering;
					}
				}
			}
		}
	}
}

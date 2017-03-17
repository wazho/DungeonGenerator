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
			SetNew,
			RuleNew,
		}
		// Types of the tabs.(After Rule Preview Area)
		public enum AfterRulePreviewTab {
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
		// The texture of icons.
		private Texture2D _edit;
		private Texture2D _delete;
		// The mode of buttons.
		private EditingMode _editingMode;
		private AfterRulePreviewTab _currentTab;
		// The scroll bar of list.
		private Vector2 _scrollPosition;
		// [Remove soon] Content of scroll area.
		private string testString;

		void Awake() {
			_currentSet			= new string[] { "Set1", "Set2"};
			_currentRule		= new string[] { "Rule1", "Rule2" };
			_currentSetIndex	= 0;
			_currentRuleIndex	= 0;
			_name				= "";
			_description		= "";
			_edit				= Resources.Load<Texture2D>("Icons/edit");
			_delete				= Resources.Load<Texture2D>("Icons/delete");
			_editingMode		= EditingMode.None;
			_currentTab			= AfterRulePreviewTab.None;
			_scrollPosition		= Vector2.zero;
			// [Remove soon]
			testString			= "1. Contents of List \n2. Contents of List \n3. Contents of List \n4. Contents of List \n5. Contents of List" +
									"\n6. Contents of List \n7. Contents of List \n8. Contents of List \n9. Contents of List \n10. Contents of List";
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
				_editingMode = EditingMode.SetNew;
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
				_editingMode = EditingMode.RuleNew;
			}
			EditorGUILayout.EndHorizontal();

			// Show the Editor of Set or Rule.
			switch (_editingMode) {
				case EditingMode.SetEdit:
					ShowEditSetRule();
					break;
				case EditingMode.SetNew:
					ShowEditSetRule();
					break;
				case EditingMode.RuleEdit:
					ShowEditSetRule();
					break;
				case EditingMode.RuleNew:
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
		}

		void ShowRulePreviewArea() {
			// Information of Source and Replacement.
			EditorGUILayout.BeginHorizontal();
			GUI.skin.label.fontSize = EditorStyle.ContentFontSize;
			GUI.skin.label.alignment = TextAnchor.UpperLeft;
			GUILayout.Label("Source", GUILayout.Width(Screen.width / 2));
			GUILayout.Label("Replacement");
			EditorGUILayout.EndHorizontal();
			// Canvas.
			EditorGUI.DrawRect(EditorStyle.RuleSourceCanvas, Color.black);
			EditorGUI.DrawRect(EditorStyle.RuleReplacementCanvas, Color.white);
		}

		void ShowAfterRulePreviewArea() {
			// Buttons - Add Node & Add Connection & Copy & Delete.
			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button("Add Node", EditorStyles.miniButtonLeft, EditorStyle.ButtonHeight)) {
				_currentTab = AfterRulePreviewTab.AddNode;
			}
			if (GUILayout.Button("Add Connection", EditorStyles.miniButtonMid, EditorStyle.ButtonHeight)) {
				_currentTab = AfterRulePreviewTab.AddConnection;
			}
			if (GUILayout.Button("Copy", EditorStyles.miniButtonMid, EditorStyle.ButtonHeight)) {
				_currentTab = AfterRulePreviewTab.Copy;
			}
			if (GUILayout.Button("Delete", EditorStyles.miniButtonRight, EditorStyle.ButtonHeight)) {
				_currentTab = AfterRulePreviewTab.Delete;
			}
			EditorGUILayout.EndHorizontal();
			// Show the list.
			switch (_currentTab) {
				case AfterRulePreviewTab.AddNode:
					ShowAddNodeList();
					break;
				case AfterRulePreviewTab.AddConnection:
					ShowAddConnectionList();
					break;
			}
			
			// Remind user [need Modify]
			EditorGUILayout.HelpBox("Info \nThe Node's name has been used.", MessageType.Info);
			// Buttons - Apply.
			if (GUILayout.Button("Apply", EditorStyles.miniButton, EditorStyle.ButtonHeight)) {
				// [Modify soon]
				/*
					Need to create a pop up window. 
					To make sure the Information of content.
				 */
			}
		}

		void ShowEditSetRule() {
			// Information.
			_name = EditorGUILayout.TextField("Name", _name);
			_description = EditorGUILayout.TextField("Description", _description);
			// Remind user [need Modify]
			if (_name == "" && _description == "") {
				EditorGUILayout.HelpBox("Info \nThe name is empty. \nThe description is empty.", MessageType.Info);
			}
			if (_name == "" && _description != "") {
				EditorGUILayout.HelpBox("Info \nThe name is empty.", MessageType.Info);
			}
			if (_name != "" && _description == "") {
				EditorGUILayout.HelpBox("Info \nThe description is empty.", MessageType.Info);
			}
			if (_name != "" && _description != "") {
				EditorGUILayout.HelpBox("Info \nNothing.", MessageType.Info);
			}

			// Buttons - Apply.
			if (GUILayout.Button("Apply", EditorStyles.miniButton, EditorStyle.ButtonHeight)) {
				// [Modify soon]
				/*
				Need to create a pop up window. 
				To make sure the Information of content.
				*/
			}
		}

		void ShowAddNodeList() {
			// Content of Node-List.
			// Set the ScrollPosition.
			_scrollPosition = GUILayout.BeginScrollView(_scrollPosition, GUILayout.Height(100));
			// Content of scroll area.
			GUILayout.Label(testString, EditorStyles.label);
			GUILayout.EndScrollView();
		}

		void ShowAddConnectionList() {
			// Content of Connection-List.
			// Set the ScrollPosition.
			_scrollPosition = GUILayout.BeginScrollView(_scrollPosition, GUILayout.Height(100));
			// Content of scroll area.
			GUILayout.Label(testString, EditorStyles.label);
			GUILayout.EndScrollView();
		}
	}
}
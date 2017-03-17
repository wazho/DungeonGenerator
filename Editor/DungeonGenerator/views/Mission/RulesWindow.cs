using UnityEngine;
using UnityEditor;
using System.Collections;

using EditorAdvance = EditorExtend.Advance;
using EditorStyle = EditorExtend.Style;

namespace MissionGrammar {
	public enum CurrentSetType {
		Master
	}
	public enum CurrectRuleType {
		Dungeon
	}

	public class RulesWindow : EditorWindow {
		// The description of set or rule
		private string _name;
		private string _description;
		// The texture of icons.
		private Texture2D _edit;
		private Texture2D _delete;
		// The mode of editor.
		private bool _isInSetEdit;
		private bool _isInRuleEdit;
		private bool _isInSetNew;
		private bool _isInRuleNew;
		// The mode of list.
		private bool _isInNodeList;
		private bool _isInConnectionList;
		// The scroll bar of list.
		private Vector2 _scrollPosition;
		// The type.
		private CurrentSetType _currentSetType;
		private CurrectRuleType _currectRuleType;
		// [Remove soon] Content of scroll area.
		private string testString;

		void Awake() {
			_name = "";
			_description = "";
			_edit = Resources.Load<Texture2D>("Icons/edit");
			_delete = Resources.Load<Texture2D>("Icons/delete");
			_isInSetEdit = false;
			_isInSetNew = false;
			_isInRuleEdit = false;
			_isInRuleNew = false;
			_isInNodeList = false;
			_isInConnectionList = false;
			_scrollPosition = Vector2.zero;
			_currentSetType = CurrentSetType.Master;
			_currectRuleType = CurrectRuleType.Dungeon;
			// [Remove soon]
			testString = "1. Contents of List \n2. Contents of List \n3. Contents of List \n4. Contents of List \n5. Contents of List" +
								"\n6. Contents of List \n7. Contents of List \n8. Contents of List \n9. Contents of List \n10. Contents of List";
		}

		void OnGUI() {
			// Current Set.
			EditorGUILayout.BeginHorizontal();
			// Dropdown list of Current Set Type.
			_currentSetType = (CurrentSetType) EditorGUILayout.EnumPopup("Current Set", _currentSetType);
			// Buttons - Editor, Delete and Add new.
			if (GUILayout.Button(_edit, EditorStyles.miniButtonLeft, EditorStyle.ButtonHeight)) {
				_isInSetEdit = true;
				_isInSetNew  = false;
			}
			if (GUILayout.Button(_delete, EditorStyles.miniButtonMid, EditorStyle.ButtonHeight)) {
				_isInSetEdit  = false;
				_isInSetNew   = false;
				_isInRuleEdit = false;
				_isInRuleNew  = false;
			}
			if (GUILayout.Button("Add New", EditorStyles.miniButtonRight, EditorStyle.ButtonHeight)) {
				_isInSetEdit = false;
				_isInSetNew  = true;
			}
			EditorGUILayout.EndHorizontal();

			// Current Rule.
			EditorGUILayout.BeginHorizontal();
			// Dropdown list of Currect Rule Type.
			_currectRuleType = (CurrectRuleType)EditorGUILayout.EnumPopup("Currect Rule", _currectRuleType);
			// Buttons - Editor, Delete and Add new.
			if (GUILayout.Button(_edit, EditorStyles.miniButtonLeft, EditorStyle.ButtonHeight)) {
				_isInRuleEdit = true;
				_isInRuleNew  = false;
			}
			if (GUILayout.Button(_delete, EditorStyles.miniButtonMid, EditorStyle.ButtonHeight)) {
				_isInSetEdit  = false;
				_isInSetNew   = false;
				_isInRuleEdit = false;
				_isInRuleNew  = false;
			}
			if (GUILayout.Button("Add New", EditorStyles.miniButtonRight, EditorStyle.ButtonHeight)) {
				_isInRuleEdit = false;
				_isInRuleNew  = true;
			}
			EditorGUILayout.EndHorizontal();

			// Show the Editor of Set or Rule.
			if (_isInSetEdit == true || _isInSetNew == true || _isInRuleEdit == true || _isInRuleNew == true) {
				ShowEditSetRule();
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
				_isInNodeList = true;
				_isInConnectionList = false;
			}
			if (GUILayout.Button("Add Connection", EditorStyles.miniButtonMid, EditorStyle.ButtonHeight)) {
				_isInNodeList = false;
				_isInConnectionList = true;
			}
			if (GUILayout.Button("Copy", EditorStyles.miniButtonMid, EditorStyle.ButtonHeight)) {
				_isInNodeList = false;
				_isInConnectionList = false;
			}
			if (GUILayout.Button("Delete", EditorStyles.miniButtonRight, EditorStyle.ButtonHeight)) {
				_isInNodeList = false;
				_isInConnectionList = false;
			}
			EditorGUILayout.EndHorizontal();
			// Show the list.
			if (_isInNodeList == true) {
				ShowAddNodeList();
			}
			if(_isInConnectionList == true) {
				ShowAddConnectionList();
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
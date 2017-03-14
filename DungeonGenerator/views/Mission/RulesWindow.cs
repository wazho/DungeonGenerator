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
        // The value of slider.
        private float _hSliderValue;
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
            _hSliderValue = 0.0f;
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
            // Buttons - Nodes or Connections.
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Create Set", EditorStyles.miniButtonLeft, EditorStyle.TabButtonHeight)) {

            }
            if (GUILayout.Button("Create Rule", EditorStyles.miniButtonRight, EditorStyle.TabButtonHeight)) {
        
            }
            EditorGUILayout.EndHorizontal();

            // Information of Rule,
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
            // Dropdown list of Current Set Type.
            _currentSetType = (CurrentSetType)EditorGUILayout.EnumPopup("Current Set", _currentSetType);
            // Dropdown list of Currect Rule Type.
            _currectRuleType = (CurrectRuleType)EditorGUILayout.EnumPopup("Currect Rule", _currectRuleType);

            // Buttons - Modify or Delete.
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Modify", EditorStyles.miniButtonLeft, EditorStyle.ButtonHeight)) {

            }
            if (GUILayout.Button("Delete", EditorStyles.miniButtonRight, EditorStyle.ButtonHeight)) {

            }
            EditorGUILayout.EndHorizontal();
            
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
            // Slider. To implement which canvas.
            if(_hSliderValue <= 0.5f) {
                _hSliderValue = 0.0f;
            } else {
                _hSliderValue = 1.0f;
            }
            _hSliderValue = GUI.HorizontalSlider(EditorStyle.RuleCanvasSlider, _hSliderValue, 0.0f, 1.0f);
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
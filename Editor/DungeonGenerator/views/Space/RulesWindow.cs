using UnityEngine;
using UnityEditor;
using System.Collections;

using EditorAdvance = EditorExtend.Advance;
using EditorStyle = EditorExtend.Style;

namespace SpaceGrammar {
	public enum CurrentSetType {
		Master,
		Explore
	}
	public enum CurrentRuleType {
		Dungeon,
		Explore
	}
	public enum CurrentSymbolType {
		Symbol1,
		Symbol2
	}
	public enum CurrentInstructionType {
		Master,
		Explore
	}
	public enum CurrentReplaceType {
		Dungeon,
		Explore
	}

	public class RulesWindow : EditorWindow {
		//Name & description
		private string _name;
		private string _description;

		//Interface mode
		private bool _isInCreateSetInterface;
		private bool _isInCreateRuleInterface;

		//Add buttons- Node or Connection
		private bool _isShowNodeList;
		private bool _isShowConnectionList;
		private bool _isShowAreaList;

		//Scroll bar of lists
		private Vector2 _scrollPosition;

		//Types - Set and Rule
		private CurrentSetType    _currentSetType;
		private CurrentRuleType   _currentRuleType;
		private CurrentSymbolType _currentSymbolType;

		//Type - Instruction and Replace
		private CurrentInstructionType _currentInstructionType;
		private CurrentReplaceType     _currentReplaceType;

		//[Remove soon] Testing content for scroll area. 
		private string testString;

		//Information
		private string _messageInfoSetRule; 
		private string _messageInfoPreview;

		//Weight, Scale, Symbol, Start Symbol
		private float _weight;
		private float _maxScale;
		private bool  _isStartSymbol;

		private Rect _canvas;

		void Awake() {
			//[Remove soon] string.Empty, to be "". because can't be used in switch
			_name                    = string.Empty;
			_description             = string.Empty;
			_isInCreateSetInterface  = true;
			_isInCreateRuleInterface = false;
			_currentSetType          = CurrentSetType.Master;
			_currentRuleType         = CurrentRuleType.Dungeon;
			_currentSymbolType       = CurrentSymbolType.Symbol1;
			_isShowNodeList          = true;
			_isShowConnectionList    = _isShowAreaList = false;
			_scrollPosition          = Vector2.zero;
			//Canvas
			_messageInfoSetRule = "Info\nStill Empty!";
			_messageInfoPreview = "Info\nThe icon of the buttons will be changed later.";
			_weight = 1.0f;
			_maxScale = 3.0f;
			_isStartSymbol = false;
			testString = "1. Contents of List \n2. Contents of List \n3. Contents of List \n4. Contents of List \n5. Contents of List" +
				"\n6. Contents of List \n7. Contents of List \n8. Contents of List \n9. Contents of List \n10. Contents of List";
		}

		void OnGUI(){
			// Interface buttons
			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button ("Create Set", EditorStyles.miniButtonLeft, EditorStyle.TabButtonHeight)) {
				_isInCreateSetInterface = true;
				_isInCreateRuleInterface = false;
			} else if (GUILayout.Button ("Create Rule", EditorStyles.miniButtonRight, EditorStyle.TabButtonHeight)) {
				_isInCreateRuleInterface = true;
				_isInCreateSetInterface = false;
			}
			EditorGUILayout.EndHorizontal ();

			// Information of Set or Rule
			_name = EditorGUILayout.TextField("Name", _name);
			_description = EditorGUILayout.TextField ("Description", _description);
			EditorGUILayout.HelpBox (_messageInfoSetRule,MessageType.Info);

			// Apply button
			if (GUILayout.Button("Apply", EditorStyles.miniButton, EditorStyle.ButtonHeight)) {
				_messageInfoSetRule = "Applied";
			}

			// Show current Set and rule
			GUILayout.Space (EditorStyle.PaddingAfterBlock);
			_currentSetType = (CurrentSetType)EditorGUILayout.EnumPopup("Current Set", _currentSetType);
			_currentRuleType = (CurrentRuleType)EditorGUILayout.EnumPopup("Current Rule", _currentRuleType);

			// Modify and Delete buttons
			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button ("Modify", EditorStyles.miniButtonLeft, EditorStyle.ButtonHeight)) {
				testString += "\nHere is another line";
			}
			if (GUILayout.Button ("Delete", EditorStyles.miniButtonRight, EditorStyle.ButtonHeight)) {
				testString = "1. Element of List";
			}
			EditorGUILayout.EndHorizontal ();

			// Show Instruction and replace
			GUILayout.Space (EditorStyle.PaddingAfterBlock);
			_currentInstructionType = (CurrentInstructionType)EditorGUILayout.EnumPopup("Instruction", _currentInstructionType);
			_currentReplaceType = (CurrentReplaceType)EditorGUILayout.EnumPopup("Replace", _currentReplaceType);

			// Instruction & replace preview area
			GUILayout.BeginArea(EditorStyle.SpaceRulePreviewArea);
			_canvas = EditorStyle.SpaceRulePreviewCanvas;
			EditorGUI.DrawRect (_canvas, Color.white);
			GUILayout.EndArea();

			// Helpbox 
			GUILayout.Space (EditorStyle.PaddingAfterBlock + EditorStyle.SpaceRulePreviewArea.size.y);
			EditorGUILayout.HelpBox (_messageInfoPreview,MessageType.Info);

			// 6 Buttons
			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button ("Redo", EditorStyles.miniButtonLeft, EditorStyle.ButtonHeight)) {
				
			}
			if (GUILayout.Button ("Undo", EditorStyles.miniButtonMid, EditorStyle.ButtonHeight)) {
				
			}
			if (GUILayout.Button ("Group", EditorStyles.miniButtonMid, EditorStyle.ButtonHeight)) {

			}
			if (GUILayout.Button ("Ungroup", EditorStyles.miniButtonMid, EditorStyle.ButtonHeight)) {

			}
			if (GUILayout.Button ("Duplicate", EditorStyles.miniButtonMid, EditorStyle.ButtonHeight)) {

			}
			if (GUILayout.Button ("Delete", EditorStyles.miniButtonRight, EditorStyle.ButtonHeight)) {
				
			}
			EditorGUILayout.EndHorizontal();

			// Add buttons - Node, Connection, Area
			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button ("Add Node", EditorStyles.miniButtonLeft, EditorStyle.TabButtonHeight)) {
				_isShowNodeList = true;
				_isShowConnectionList = _isShowAreaList = false;
			} else if (GUILayout.Button ("Add Connection", EditorStyles.miniButtonMid, EditorStyle.TabButtonHeight)) {
				_isShowConnectionList = true;
				_isShowNodeList = _isShowAreaList = false;
			} else if (GUILayout.Button ("Add Area", EditorStyles.miniButtonRight, EditorStyle.TabButtonHeight)) {
				_isShowAreaList = true;
				_isShowNodeList = _isShowConnectionList = false;
			}
			EditorGUILayout.EndHorizontal();

			//List
			GUI.skin.label.fontSize = EditorStyle.HeaderFontSize;
			GUI.skin.label.alignment = TextAnchor.MiddleCenter;
			if (_isShowNodeList) {
				GUILayout.Label ("List of Nodes");
				testString = "";
				for (int i = 1; i <= 10; i++) {
					testString += i.ToString() + ". Element Node " + i.ToString ()+ "\n";
				}
			} else if (_isShowConnectionList) {
				GUILayout.Label ("List of Connections");	
				testString = "";
				for (int i = 1; i <= 10; i++) {
					testString += i.ToString() + ". Element Connections " + i.ToString ()+ "\n";
				}
			} else if (_isShowAreaList) {
				GUILayout.Label ("List of Areas");	
				testString = "";
				for (int i = 1; i <= 10; i++) {
					testString += i.ToString() + ". Element Areas " + i.ToString () + "\n";
				}
			}
			GUI.skin.label.fontSize = EditorStyle.ContentFontSize;
			GUI.skin.label.alignment = TextAnchor.UpperLeft;

			//Scroll bar & area
			_scrollPosition = GUILayout.BeginScrollView(_scrollPosition, GUILayout.Height(100));
			//content of scroll area
			GUILayout.Label(testString, EditorStyles.label);
			GUILayout.EndScrollView();

			//Weight, Symbol, Max.Scale, Start Symbol
			GUILayout.Space (EditorStyle.PaddingAfterBlock);
			EditorGUILayout.BeginHorizontal();
			_weight = EditorGUILayout.FloatField ("Weight", _weight);
			_maxScale = EditorGUILayout.FloatField ("Max. Scale",_maxScale);
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.BeginHorizontal();
			_currentSymbolType = (CurrentSymbolType)EditorGUILayout.EnumPopup ("Symbol", _currentSymbolType);
			_isStartSymbol = EditorGUILayout.ToggleLeft ("Set Start Symbol", _isStartSymbol);
			EditorGUILayout.EndHorizontal();

			//Apply button and popup
			if (GUILayout.Button("Apply", EditorStyles.miniButton, EditorStyle.ButtonHeight)) {
				if (EditorUtility.DisplayDialog ("Apply on the element", 
					"Are you sure want to apply the operation?", 
					"Yes", "No")) {
					_messageInfoPreview = "Info\nApplied";
				} else {
					_messageInfoPreview = "Info\nCancelled";
				}
			}
		}
		//[Remove soon]
		//void showCreateSetInterface(){}
    }
}
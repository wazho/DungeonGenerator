using UnityEngine;
using UnityEditor;
using System.Collections;

using EditorAdvance = EditorExtend.Advance;
using EditorStyle   = EditorExtend.Style;

namespace SpaceGrammar {
	//Enums 
	public enum SymbolTerminalType {
		NonTerminal,
		Terminal
	}

	public class AlphabetWindow : EditorWindow {
		//Color properties
		private Color _symbolOutlineColor;
		private Color _symbolFilledColor;
		private Color _symbolTextColor;
	
		//Modes of buttons
		private bool _isInPointsInterface;
		private bool _isInEdgesInterface;
		private bool _isInAreasInterface;

		//Scrollbar of list
		private Vector2 _scrollPosition;

		//Label of Points, Edges, or Areas
		private string _pointName;
		private string _pointAbbreviation;
		private string _pointDescription;
		private string _edgesName;
		private string _edgesAbbreviation;
		private string _edgesDescription; 
		private string _areasName;
		private string _areasAbbreviation;
		private string _areasDescription;

		//Canvas
		private Rect _canvas;

		//Symbol Type
		private SymbolTerminalType _symbolType;

		//[Remove soon] Testing content for scroll area. 
		private string testString;

		//Information
		private string _messageInfo;

		void Awake() {
			_symbolOutlineColor = Color.cyan;
			_symbolFilledColor = Color.green;
			_symbolTextColor = Color.black;
			_isInPointsInterface = true;
			_isInEdgesInterface = _isInAreasInterface = false;
			_scrollPosition = Vector2.zero;
			//[Remove soon] string.Empty, to be "" 
			_pointName = string.Empty;
			_pointAbbreviation = string.Empty;
			_pointDescription = string.Empty;
			_edgesName = string.Empty;
			_edgesAbbreviation = string.Empty;
			_edgesDescription = string.Empty;
			_areasName = string.Empty;
			_areasAbbreviation = string.Empty;
			_areasDescription = string.Empty;
			_canvas = new Rect (0, 0, Screen.width, Screen.height);
			_symbolType = SymbolTerminalType.Terminal;
			_messageInfo = "Info\nStill Empty!";
			//[Remove soon]
			testString = "1. Element of List";
		}

		void OnGUI() {
			//Interface buttons
			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button ("Points", EditorStyles.miniButtonLeft, EditorStyle.TabButtonHeight)) {
				_isInPointsInterface = true;
				_isInEdgesInterface = _isInAreasInterface = false;
			} else if (GUILayout.Button ("Edges", EditorStyles.miniButtonMid, EditorStyle.TabButtonHeight)) {
				_isInEdgesInterface = true;
				_isInPointsInterface = _isInAreasInterface = false;
			} else if (GUILayout.Button ("Areas", EditorStyles.miniButtonRight, EditorStyle.TabButtonHeight)) {
				_isInAreasInterface = true;
				_isInPointsInterface = _isInEdgesInterface = false;
			} 
			EditorGUILayout.EndHorizontal();

			if (_isInPointsInterface) { 
				//Drawing the interface's contents
				GUI.skin.label.fontSize = EditorStyle.HeaderFontSize;
				GUI.skin.label.alignment = TextAnchor.MiddleCenter;
				GUILayout.Label ("List of Points");
				GUI.skin.label.fontSize = EditorStyle.ContentFontSize;
				GUI.skin.label.alignment = TextAnchor.UpperLeft;

				ShowPointsInterface ();	
			}
			else if (_isInEdgesInterface) { 
				//Drawing the interface's contents
				GUI.skin.label.fontSize = EditorStyle.HeaderFontSize;
				GUI.skin.label.alignment = TextAnchor.MiddleCenter;
				GUILayout.Label ("List of Edges");
				GUI.skin.label.fontSize = EditorStyle.ContentFontSize;
				GUI.skin.label.alignment = TextAnchor.UpperLeft;

				ShowEdgesInterface (); 
			}
			else if (_isInAreasInterface){ 
				//Drawing the interface's contents
				GUI.skin.label.fontSize = EditorStyle.HeaderFontSize;
				GUI.skin.label.alignment = TextAnchor.MiddleCenter;
				GUILayout.Label ("List of Areas");
				GUI.skin.label.fontSize = EditorStyle.ContentFontSize;
				GUI.skin.label.alignment = TextAnchor.UpperLeft;

				ShowAreasInterface (); 
			}
		}

		void ShowPointsInterface(){
			//Showing content of points interface	
			//Scroll area
			_scrollPosition = GUILayout.BeginScrollView(_scrollPosition, GUILayout.Height(100));
			GUILayout.Label(testString, EditorStyles.label);
			GUILayout.EndScrollView();

			//Modify and Delete buttons
			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button("Modify", EditorStyles.miniButtonLeft, EditorStyle.ButtonHeight)) {
				testString += "\nHere is another line";
			}
			if (GUILayout.Button("Delete", EditorStyles.miniButtonRight, EditorStyle.ButtonHeight)) {
				testString = "1. Element of List";
			}
			EditorGUILayout.EndHorizontal ();

			//Canvas
			GUILayout.BeginArea(EditorStyle.AlphabetPreviewArea);
			_canvas = EditorStyle.AlphabetPreviewCanvas;
			EditorGUI.DrawRect (_canvas, Color.white);
			GUILayout.EndArea ();

			//Content of properties
			GUILayout.BeginArea(EditorStyle.AfterAlphabetPreviewArea);
			EditorGUILayout.BeginVertical ();
			GUILayout.Space (EditorStyle.PaddingAfterBlock);

			//Dropdown list of symbol type
			_symbolType = (SymbolTerminalType) EditorGUILayout.EnumPopup("Symbol Type", _symbolType);
		
			//Information of points
			_pointName = EditorGUILayout.TextField ("Name", _pointName);
			_pointAbbreviation = EditorGUILayout.TextField ("Abbreviation", _pointAbbreviation);
			_pointDescription = EditorGUILayout.TextField ("Description", _pointDescription);

			_symbolOutlineColor = EditorGUILayout.ColorField ("Outline Color", _symbolOutlineColor);
			_symbolFilledColor = EditorGUILayout.ColorField ("Filled Color", _symbolFilledColor);
			_symbolTextColor = EditorGUILayout.ColorField ("Text Color", _symbolTextColor);

			//Info box
			EditorGUILayout.HelpBox(_messageInfo, MessageType.Info);
			if (GUILayout.Button("Apply", EditorStyles.miniButton, EditorStyle.ButtonHeight)) { //, GUILayout.Width(50))) {
				if (EditorUtility.DisplayDialog ("Apply on the element", 
					"Are you sure want to apply the operation?", 
					"Yes", "No")) {
					_messageInfo = "Info\nNew point has been added";
				} else {
					_messageInfo = "Info\nNew point cancelled";
				}
			}
			EditorGUILayout.EndVertical ();
			GUILayout.EndArea ();
		}

		void ShowEdgesInterface(){
			//Showing content of edges interface
			//Scroll area
			_scrollPosition = GUILayout.BeginScrollView(_scrollPosition, GUILayout.Height(100));
			GUILayout.Label(testString, EditorStyles.label);
			GUILayout.EndScrollView();

			//Modify and Delete buttons
			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button("Modify", EditorStyles.miniButtonLeft, EditorStyle.ButtonHeight)) {
				testString += "\nHere is another line";
			}
			if (GUILayout.Button("Delete", EditorStyles.miniButtonRight, EditorStyle.ButtonHeight)) {
				testString = "1. Element of List";
			}
			EditorGUILayout.EndHorizontal ();

			//Canvas
			GUILayout.BeginArea(EditorStyle.AlphabetPreviewArea);
			_canvas = EditorStyle.AlphabetPreviewCanvas;
			EditorGUI.DrawRect (_canvas, Color.white);
			GUILayout.EndArea ();

			//Content of properties
			GUILayout.BeginArea(EditorStyle.AfterAlphabetPreviewArea);
			EditorGUILayout.BeginVertical ();
			GUILayout.Space (EditorStyle.PaddingAfterBlock);

			//Dropdown list of symbol type
			_symbolType = (SymbolTerminalType) EditorGUILayout.EnumPopup("Symbol Type", _symbolType);

			//Information of edges
			_pointName = EditorGUILayout.TextField ("Name", _pointName);
			_pointAbbreviation = EditorGUILayout.TextField ("Abbreviation", _pointAbbreviation);
			_pointDescription = EditorGUILayout.TextField ("Description", _pointDescription);

			_symbolOutlineColor = EditorGUILayout.ColorField ("Outline Color", _symbolOutlineColor);
			_symbolFilledColor = EditorGUILayout.ColorField ("Filled Color", _symbolFilledColor);
			_symbolTextColor = EditorGUILayout.ColorField ("Text Color", _symbolTextColor);

			//Info box
			EditorGUILayout.HelpBox(_messageInfo, MessageType.Info);
			if (GUILayout.Button("Apply", EditorStyles.miniButton, EditorStyle.ButtonHeight)) { //, GUILayout.Width(50))) {
				if (EditorUtility.DisplayDialog ("Apply on the element", 
					"Are you sure want to apply the operation?", 
					"Yes", "No")) {
					_messageInfo = "Info\nNew edge has been added";
				} else {
					_messageInfo = "Info\nNew edge cancelled";
				}
			}
			EditorGUILayout.EndVertical ();
			GUILayout.EndArea ();
		}

		void ShowAreasInterface(){
			//Showing content of areas interface
			//Scroll area
			_scrollPosition = GUILayout.BeginScrollView(_scrollPosition, GUILayout.Height(100));
			GUILayout.Label(testString, EditorStyles.label);
			GUILayout.EndScrollView();

			//Modify and Delete buttons
			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button("Modify", EditorStyles.miniButtonLeft, EditorStyle.ButtonHeight)) {
				testString += "\nHere is another line";
			}
			if (GUILayout.Button("Delete", EditorStyles.miniButtonRight, EditorStyle.ButtonHeight)) {
				testString = "1. Element of List";
			}
			EditorGUILayout.EndHorizontal ();

			//Canvas
			GUILayout.BeginArea(EditorStyle.AlphabetPreviewArea);
			_canvas = EditorStyle.AlphabetPreviewCanvas;
			EditorGUI.DrawRect (_canvas, Color.white);
			GUILayout.EndArea ();

			//Content of properties
			GUILayout.BeginArea(EditorStyle.AfterAlphabetPreviewArea);
			EditorGUILayout.BeginVertical ();
			GUILayout.Space (EditorStyle.PaddingAfterBlock);

			//Dropdown list of symbol type
			_symbolType = (SymbolTerminalType) EditorGUILayout.EnumPopup("Symbol Type", _symbolType);

			//Information of points
			_pointName = EditorGUILayout.TextField ("Name", _pointName);
			_pointAbbreviation = EditorGUILayout.TextField ("Abbreviation", _pointAbbreviation);
			_pointDescription = EditorGUILayout.TextField ("Description", _pointDescription);

			_symbolOutlineColor = EditorGUILayout.ColorField ("Outline Color", _symbolOutlineColor);
			_symbolFilledColor = EditorGUILayout.ColorField ("Filled Color", _symbolFilledColor);
			_symbolTextColor = EditorGUILayout.ColorField ("Text Color", _symbolTextColor);

			//Info box
			EditorGUILayout.HelpBox(_messageInfo, MessageType.Info);
			if (GUILayout.Button("Apply", EditorStyles.miniButton, EditorStyle.ButtonHeight)) { //, GUILayout.Width(50))) {
				if (EditorUtility.DisplayDialog ("Apply on the element", 
						"Are you sure want to apply the operation?", 
						"Yes", "No")) {
					_messageInfo = "Info\nNew area has been added";
				} else {
					_messageInfo = "Info\nNew area cancelled";
				}
			}
			EditorGUILayout.EndVertical ();
			GUILayout.EndArea ();
		}
	}
}
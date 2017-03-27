using UnityEngine;
using UnityEditor;
using System.Collections;

using EditorAdvance = EditorExtend.Advance;
using EditorStyle   = EditorExtend.Style;

namespace SpaceGrammarSystem {
	public class AlphabetWindow : EditorWindow {
		// Tabs interface modes
		public enum InterfaceTab {
			Points, 
			Edges,
			Areas
		}

		// Edit modes
		public enum EditingMode {
			None,
			Create, 
			Modify,
			Delete
		}

		// Interface mode
		private InterfaceTab _currentInterfaceTab;
		// Edit mode
		private EditingMode _editingMode;

		// Symbol description
		private string _symbolName;
		private string _symbolAbbreviation;
		private string _symbolDescription;

		// Symbol's color properties
		private Color _symbolOutlineColor;
		private Color _symbolFilledColor;
		private Color _symbolTextColor;
	
		// [Edit later]
		// Exclusive types
		private int _pointSymbolTypeIndex;
		private string[] _pointSymbolTypes;
		private string _pointSymbolType;

		// [Edit later]
		// Interface button's textures
		private static Texture2D _miniButtonLeftNormalTexture;
		private static Texture2D _miniButtonMidNormalTexture;
		private static Texture2D _miniButtonRightNormalTexture;
		private static Texture2D _miniButtonLeftActiveTexture;
		private static Texture2D _miniButtonMidActiveTexture;
		private static Texture2D _miniButtonRightActiveTexture;

		// Interface tab styles
		private GUIStyle _pointsInterfaceStyle;
		private GUIStyle _edgesInterfaceStyle;
		private GUIStyle _areasInterfaceStyle;

		// Canvas
		private Rect _symbolListCanvas;
		private Rect _symbolPreviewCanvas;

		private bool _isInterfaceInit; 

		// Information
		private string _messageInfo;

		// Scroll bar of list
		private Vector2 _scrollPosition;

		void Awake() {
			// Initialize the fields
			_symbolName = string.Empty;
			_symbolAbbreviation = string.Empty;
			_symbolDescription = string.Empty;
			_symbolOutlineColor = Color.cyan;
			_symbolFilledColor = Color.green;
			_symbolTextColor = Color.black;

			// Symbol type
			_pointSymbolTypeIndex = 1;
			_pointSymbolTypes = new string[]{ "Non-Terminal", "Terminal" };
			_pointSymbolType = _pointSymbolTypes[_pointSymbolTypeIndex];

			// Interface
			_currentInterfaceTab = InterfaceTab.Points;
			_symbolListCanvas = new Rect(0, 0, Screen.width, Screen.height);
			_symbolPreviewCanvas = new Rect(0, 0, Screen.width, Screen.height);

			_messageInfo = "Nothing to show.";

			_isInterfaceInit = true;
			_scrollPosition = Vector2.zero;
			_editingMode = EditingMode.None;
		}

		void OnGUI() {
			// Initialize interface style
			if (_isInterfaceInit) {
				//points
				_pointsInterfaceStyle = new GUIStyle(EditorStyles.miniButtonLeft);
				_miniButtonLeftNormalTexture = _pointsInterfaceStyle.normal.background;
				_miniButtonLeftActiveTexture = _pointsInterfaceStyle.active.background;
				//edges
				_edgesInterfaceStyle = new GUIStyle(EditorStyles.miniButtonMid);
				_miniButtonMidNormalTexture = _edgesInterfaceStyle.normal.background;
				_miniButtonMidActiveTexture = _edgesInterfaceStyle.active.background;
				//areas
				_areasInterfaceStyle = new GUIStyle(EditorStyles.miniButtonRight);
				_miniButtonRightNormalTexture = _areasInterfaceStyle.normal.background;
				_miniButtonRightActiveTexture = _areasInterfaceStyle.active.background;
				_isInterfaceInit = false;
			}
			// Interface buttons 
			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button("Points", _pointsInterfaceStyle, EditorStyle.TabButtonHeight)) {
				_currentInterfaceTab = InterfaceTab.Points;
			} else if (GUILayout.Button("Edges", _edgesInterfaceStyle, EditorStyle.TabButtonHeight)) {
				_currentInterfaceTab = InterfaceTab.Edges;
			} else if (GUILayout.Button("Areas", _areasInterfaceStyle, EditorStyle.TabButtonHeight)) {
				_currentInterfaceTab = InterfaceTab.Areas;
			}
			EditorGUILayout.EndHorizontal();

			// Switch the interface tab & switch the content
			// [Edit later] GUI.skin.label...
			GUI.skin.label.fontSize = EditorStyle.HeaderFontSize;
			GUI.skin.label.alignment = TextAnchor.MiddleCenter;
			// Interface contents
			switch(_currentInterfaceTab){
			case InterfaceTab.Points:
				// Interface button
				_pointsInterfaceStyle.normal.background = _miniButtonLeftActiveTexture;
				_edgesInterfaceStyle.normal.background = _miniButtonMidNormalTexture;
				_areasInterfaceStyle.normal.background = _miniButtonRightNormalTexture;
				GUILayout.Label("List of Points");
				break;
			case InterfaceTab.Edges:
				// Interface button
				_pointsInterfaceStyle.normal.background = _miniButtonLeftNormalTexture;
				_edgesInterfaceStyle.normal.background = _miniButtonMidActiveTexture;
				_areasInterfaceStyle.normal.background = _miniButtonRightNormalTexture;
				GUILayout.Label("List of Edges");
				break;
			case InterfaceTab.Areas:
				// Interface button
				_pointsInterfaceStyle.normal.background = _miniButtonLeftNormalTexture;
				_edgesInterfaceStyle.normal.background = _miniButtonMidNormalTexture;
				_areasInterfaceStyle.normal.background = _miniButtonRightActiveTexture;
				GUILayout.Label("List of Areas");
				break;
			}
			GUI.skin.label.fontSize = EditorStyle.ContentFontSize;
			GUI.skin.label.alignment = TextAnchor.UpperLeft;

			ShowInterface(_currentInterfaceTab);
		}

		void LayoutSymbolList(InterfaceTab tab){
			// Scroll position
			_scrollPosition = GUILayout.BeginScrollView(_scrollPosition, EditorStyle.AlphabetSymbolListHeight);

			// Contents of scroll area
			GUILayout.BeginArea(EditorStyle.AlphabetSymbolListArea);
			_symbolListCanvas = EditorStyle.AlphabetSymbolListCanvas;
			EditorGUI.DrawRect(_symbolListCanvas, Color.gray);
			GUILayout.EndArea();

			// [Edit later] draw based on the current interface tab
			// To do : Drawing the list elements

			GUILayout.EndScrollView();

			// To do : get the a rect for element list
		}

		void LayoutEditingButtons(InterfaceTab tab){
			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button("Add New", EditorStyles.miniButtonLeft, EditorStyle.ButtonHeight)) {
				_editingMode = EditingMode.Create;
				// To do : Re-initilaize 
				// To do : Manage the availability of Modify and delete button
				// To do : Action based on interface tab
			}
			if (GUILayout.Button("Modify", EditorStyles.miniButtonMid, EditorStyle.ButtonHeight)) {
				_editingMode = EditingMode.Modify;
				// To do : Re-initilaize 
				// To do : Action based on interface tab
			}
			if (GUILayout.Button("Delete", EditorStyles.miniButtonRight, EditorStyle.ButtonHeight)) {
				_editingMode = EditingMode.Delete;
				// To do : Re-initilaize 
				// To do : Action based on interface tab
			}
			// To do : Manage the availability of Modify and delete button

			EditorGUILayout.EndHorizontal();
		}

		void LayoutSymbolPreview(InterfaceTab tab){
			GUILayout.BeginArea(EditorStyle.AlphabetPreviewArea);
			_symbolPreviewCanvas = EditorStyle.AlphabetPreviewCanvas;
			EditorGUI.DrawRect(_symbolPreviewCanvas, Color.gray);
			// To do : Draw the previewed symbol based on interface tab
			GUILayout.EndArea ();
		}

		void LayoutFields(InterfaceTab tab){
			// Symbol type
			_pointSymbolTypeIndex = EditorGUILayout.Popup("Symbol Type", _pointSymbolTypeIndex, _pointSymbolTypes);
			_pointSymbolType = _pointSymbolTypes[_pointSymbolTypeIndex];
			// Fields
			// To do : Show the field based on interface tab
			// To do : Validate the input field
			_symbolName = EditorGUILayout.TextField("name", _symbolName);
			_symbolAbbreviation = EditorGUILayout.TextField ("Abbreviation", _symbolAbbreviation);
			_symbolDescription = EditorGUILayout.TextField ("Description", _symbolDescription);
			_symbolOutlineColor = EditorGUILayout.ColorField ("Outline Color", _symbolOutlineColor);
			_symbolFilledColor = EditorGUILayout.ColorField ("Filled Color", _symbolFilledColor);
			_symbolTextColor = EditorGUILayout.ColorField ("Text Color", _symbolTextColor);
		}

		void ShowInterface(InterfaceTab tab){
			// Showing content of points interface	
			// List of symbol canvas
			LayoutSymbolList(tab);
			// Editing buttons
			LayoutEditingButtons(tab);

			// Symbol preview canvas 
			LayoutSymbolPreview(tab);

			// Property fields
			GUILayout.BeginArea(EditorStyle.AfterAlphabetPreviewArea);
			EditorGUILayout.BeginVertical ();
			GUILayout.Space (EditorStyle.PaddingAfterBlock);
			// Fields
			LayoutFields(tab);
			// Info box
			// To do : show the information based on the current action
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
	}
}
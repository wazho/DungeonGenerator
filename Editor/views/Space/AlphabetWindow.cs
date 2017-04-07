using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections;
using System.Text.RegularExpressions;


using EditorAdvance = EditorExtend.Advance;
using EditorStyle   = EditorExtend.Style;

namespace SpaceGrammarSystem {
	public class AlphabetWindow : EditorWindow {
		// Tabs interface modes
		public enum AlphabetWindowTab {
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
		// Message of helpbox of submition.
		private string _messageHint;
		private MessageType _messageType;
		// point or connection in prview canvas.
		private SpaceGrammarPoint _point = new SpaceGrammarPoint(PointTerminalType.Terminal);
		// Interface mode
		private AlphabetWindowTab _currentTab;
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
		private int               _pointSymbolTypeIndex;
		private string[]          _pointSymbolTypes;
		private string            _pointSymbolType;
		private PointTerminalType _symbolTerminal;

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
		// The drawing canvas.
		private Rect _symbolListCanvasInWindow;
		private Rect _canvas;
		private Vector2 _centerPosition;

		private bool _isInterfaceInit; 

		// Information
		private string _messageInfo;

		// Scroll bar of list
		private Vector2 _scrollPosition;

		void Awake() {
			// Initialize the fields
			InitFields();

			// Symbol type
			_pointSymbolTypeIndex = 1;
			_pointSymbolTypes = new string[]{ "Non-Terminal", "Terminal" };
			_pointSymbolType = _pointSymbolTypes[_pointSymbolTypeIndex];

			// Interface
			_currentTab = AlphabetWindowTab.Points;
			_symbolPreviewCanvas = new Rect(0, 0, Screen.width, Screen.height);
			_symbolListCanvas = new Rect(0, 0, Screen.width, Screen.height);
			_symbolListCanvasInWindow = _symbolListCanvas;
			_canvas = new Rect(0, 0, Screen.width, Screen.height);
			_centerPosition = new Vector2(Screen.width / 2, 75);

			_messageInfo = "Nothing to show.";

			_isInterfaceInit = true;
			_scrollPosition = Vector2.zero;
			_editingMode = EditingMode.None;
		}
		// Initial whole fields in window.
		void InitFields() {
			_symbolName         = string.Empty;
			_symbolAbbreviation = string.Empty;
			_symbolDescription  = string.Empty;
			_symbolOutlineColor = Color.black;
			_symbolFilledColor  = Color.white;
			_symbolTextColor    = Color.black;
			_symbolTerminal     = PointTerminalType.Terminal;
			// Unfocus from the field.
			GUI.FocusControl("FocusToNothing");
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
				_currentTab = AlphabetWindowTab.Points;
			} else if (GUILayout.Button("Edges", _edgesInterfaceStyle, EditorStyle.TabButtonHeight)) {
				_currentTab = AlphabetWindowTab.Edges;
			} else if (GUILayout.Button("Areas", _areasInterfaceStyle, EditorStyle.TabButtonHeight)) {
				_currentTab = AlphabetWindowTab.Areas;
			}
			EditorGUILayout.EndHorizontal();

			// Switch the interface tab & switch the content
			// [Edit later] GUI.skin.label...
			GUI.skin.label.fontSize = EditorStyle.HeaderFontSize;
			GUI.skin.label.alignment = TextAnchor.MiddleCenter;
			// Interface contents
			switch(_currentTab){
			case AlphabetWindowTab.Points:
				// Interface button
				_pointsInterfaceStyle.normal.background = _miniButtonLeftActiveTexture;
				_edgesInterfaceStyle.normal.background = _miniButtonMidNormalTexture;
				_areasInterfaceStyle.normal.background = _miniButtonRightNormalTexture;
				GUILayout.Label("List of Points");
				break;
			case AlphabetWindowTab.Edges:
				// Interface button
				_pointsInterfaceStyle.normal.background = _miniButtonLeftNormalTexture;
				_edgesInterfaceStyle.normal.background = _miniButtonMidActiveTexture;
				_areasInterfaceStyle.normal.background = _miniButtonRightNormalTexture;
				GUILayout.Label("List of Edges");
				break;
			case AlphabetWindowTab.Areas:
				// Interface button
				_pointsInterfaceStyle.normal.background = _miniButtonLeftNormalTexture;
				_edgesInterfaceStyle.normal.background = _miniButtonMidNormalTexture;
				_areasInterfaceStyle.normal.background = _miniButtonRightActiveTexture;
				GUILayout.Label("List of Areas");
				break;
			}
			GUI.skin.label.fontSize = EditorStyle.ContentFontSize;
			GUI.skin.label.alignment = TextAnchor.UpperLeft;

			ShowInterface();
			EventController();
		}
		// Determine which tav to be show up
		void ShowInterface() {
			switch (_currentTab) {
				case AlphabetWindowTab.Points:
					ShowPointsInterface();
				break;
			}
		}
		// Content of points.
		void ShowPointsInterface() {
			// Show the canvas, that is the list of points.
			LayoutSymbolList();
			// Buttons for switching editing mode.
			LayoutEditingModeButtonGroup();
			// Canvas for preview symbol.
			GUILayout.BeginArea(EditorStyle.AlphabetPreviewArea);
			_canvas = EditorStyle.AlphabetPreviewCanvas;
			EditorGUI.DrawRect(_canvas, Color.gray);
			_centerPosition.x = Screen.width / 2;
			_point.Position    = _centerPosition;
			_point.Draw();
			GUILayout.EndArea();
			// Area for input fields.
			switch (_editingMode) {
			case EditingMode.Create:
			case EditingMode.Modify:
				// Content of property.
				GUILayout.BeginArea(EditorStyle.AfterAlphabetPreviewArea);
				EditorGUILayout.BeginVertical();
				GUILayout.Space(EditorStyle.PaddingAfterBlock);
				// Information of point.
				_symbolTerminal     = (PointTerminalType) EditorGUILayout.EnumPopup("Symbol Type", _symbolTerminal);
				_symbolName         = EditorGUILayout.TextField("Name", _symbolName);
				_symbolAbbreviation = EditorGUILayout.TextField("Abbreviation", _symbolAbbreviation);
				_symbolDescription  = EditorGUILayout.TextField("Description", _symbolDescription);
				_symbolOutlineColor = EditorGUILayout.ColorField("Outline Color", _symbolOutlineColor);
				_symbolFilledColor  = EditorGUILayout.ColorField("Filled Color", _symbolFilledColor);
				_symbolTextColor    = EditorGUILayout.ColorField("Text Color", _symbolTextColor);
				// Update the point.
				Updatepoint(_point);
				EditorGUILayout.EndVertical();
				GUILayout.Space(EditorStyle.PaddingAfterBlock);
				// Show content of submition.
				LayoutSubmitionHint();
				LayoutSubmitionButton();
				GUILayout.EndArea();
				break;
			}
		}
		// Symbol list in point tab and connection tab.
		void LayoutSymbolList() {
			// Set the scroll position.
			_scrollPosition = GUILayout.BeginScrollView(_scrollPosition, EditorStyle.AlphabetSymbolListHeight);
			// Content of scroll area.
			GUILayout.BeginArea(EditorStyle.AlphabetSymbolListArea);
			_symbolListCanvas = EditorStyle.AlphabetSymbolListCanvas;
			EditorGUI.DrawRect(_symbolListCanvas, Color.gray);
			GUILayout.EndArea();
			// Layout each symbols in list.
			switch (_currentTab) {
			case AlphabetWindowTab.Points:
				foreach (var point in Alphabet.Points) {
					Alphabet.DrawpointInList(point);
					// Custom style to modify padding and margin for label.
					GUILayout.Label(point.ExpressName, EditorStyle.LabelInNodeList);
				}
				break;
			case AlphabetWindowTab.Edges:

				break;
			}
			GUILayout.EndScrollView();
			// Get the Rect object from the last control when the event is Repaint.
			if (Event.current.type == EventType.Repaint) {
				_symbolListCanvasInWindow = GUILayoutUtility.GetLastRect();
			}
		}
		// Buttons about adding new symbol, modifying and deleting.
		void LayoutEditingModeButtonGroup() {
			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button("Add New", EditorStyles.miniButtonLeft, EditorStyle.ButtonHeight)) {
				// Switch the mode.
				_editingMode = EditingMode.Create;
				// Initial all fields and repaint.
				Alphabet.RevokeAllSelected();
				InitFields();
				Repaint();
			}
			switch (_currentTab) {
			case AlphabetWindowTab.Points:
				EditorGUI.BeginDisabledGroup(Alphabet.SelectedPoint == null);
				break;
			case AlphabetWindowTab.Edges:
				EditorGUI.BeginDisabledGroup(Alphabet.SelectedConnection == null);
				break;
			}
			if (GUILayout.Button("Modify", EditorStyles.miniButtonMid, EditorStyle.ButtonHeight)) {
				// Switch the mode.
				_editingMode = EditingMode.Modify;
			}
			if (GUILayout.Button("Delete", EditorStyles.miniButtonRight, EditorStyle.ButtonHeight)) {
				// Switch the mode.
				_editingMode = EditingMode.Delete;
				// Remove the point or connection from alphabet and repaint.
				switch (_currentTab) {
				case AlphabetWindowTab.Points:
					Alphabet.RemovePoint(Alphabet.SelectedPoint);
					break;
				case AlphabetWindowTab.Edges:

					break;
				}
				Repaint();
			}
			EditorGUI.EndDisabledGroup();
			EditorGUILayout.EndHorizontal();
		}
		// Refresh the fields when select any symbol.
		void UpdateFields(SpaceGrammarPoint point) {
			_symbolTerminal     = point.Terminal;
			_symbolName         = point.Name;
			_symbolAbbreviation = point.Abbreviation;
			_symbolDescription  = point.Description;
			_symbolOutlineColor = point.OutlineColor;
			_symbolFilledColor  = point.FilledColor;
			_symbolTextColor    = point.TextColor;
			// Repaint the window.
			Repaint();
		}
		// Update the point information from the current field values.
		void Updatepoint(SpaceGrammarPoint point) {
			if (point == null) { return; }
			point.Terminal     = _symbolTerminal;
			point.Name         = _symbolName;
			point.Abbreviation = _symbolAbbreviation;
			point.Description  = _symbolDescription;
			point.OutlineColor = _symbolOutlineColor;
			point.FilledColor  = _symbolFilledColor;
			point.TextColor    = _symbolTextColor;
			// Repaint the window.
			Repaint();
		}
		// Validate that the field data is legal.
		private static Regex _ruleOfTerminalSymbolName            = new Regex(@"^[a-z]{1}[a-zA-Z]{,19}$");
		private static Regex _ruleOfTerminalSymbolAbbreviation    = new Regex(@"^[a-z]{1,4}$");
		private static Regex _ruleOfNonTerminalSymbolName         = new Regex(@"^[A-Z]{1}[a-zA-Z]{,19}$");
		private static Regex _ruleOfNonTerminalSymbolAbbreviation = new Regex(@"^[A-Z]{1,4}$");
		void pointFieldValidation() {
			if (_symbolName == string.Empty ||
				_symbolAbbreviation == string.Empty ||
				_symbolDescription == string.Empty) {
				_messageHint = "Please fill every column.";
				_messageType = MessageType.Warning;
			} else if (_symbolTerminal == PointTerminalType.Terminal &&
				! _ruleOfTerminalSymbolName.IsMatch(_symbolName)) {
				_messageHint = "Name field Error! \nPlease use only letters (a-z,A-Z) under 20 characters and the first letter is lowercase.";
				_messageType = MessageType.Error;
			} else if (_symbolTerminal == PointTerminalType.Terminal &&
				! _ruleOfTerminalSymbolAbbreviation.IsMatch(_symbolAbbreviation)) {
				_messageHint = "Abbreviation field error! \nPlease use only lowercase letters (a-z) and 4 characters or less.";
				_messageType = MessageType.Error;
			} else if (_symbolTerminal == PointTerminalType.NonTerminal &&
				! _ruleOfNonTerminalSymbolName.IsMatch(_symbolName)) {
				_messageHint = "Name field Error! \nPlease use only letters (a-z,A-Z) under 20 characters and the first letter is uppercase.";
				_messageType = MessageType.Error;
			} else if (_symbolTerminal == PointTerminalType.NonTerminal &&
				! _ruleOfNonTerminalSymbolAbbreviation.IsMatch(_symbolAbbreviation)) {
				_messageHint = "Abbreviation field error! \nPlease use only uppercase letters (A-Z) and 4 characters or less.";
				_messageType = MessageType.Error;
			} else if (Alphabet.IsPointNameUsed(_point)) {
				_messageHint = "point name has been used!\nPlease try another one.";
				_messageType = MessageType.Error;
			} else if (Alphabet.IsPointAbbreviationUsed(_point)) {
				_messageHint = "point abbreviation has been used!\nPlease try another one.";
				_messageType = MessageType.Error;
			} else {
				_messageHint = "The data has changed, but still not save it.";
				_messageType = MessageType.Info;
			}
		}
		// Hint message about the form fields.
		void LayoutSubmitionHint() {
			switch (_currentTab) {
			case AlphabetWindowTab.Points:
				switch (_editingMode) {
				case EditingMode.Create:
					pointFieldValidation();
					break;
				case EditingMode.Modify:
					if (Alphabet.SelectedPoint != null && Alphabet.SelectedPoint.Terminal == _point.Terminal &&
						Alphabet.SelectedPoint.Name == _point.Name &&
						Alphabet.SelectedPoint.Abbreviation == _point.Abbreviation &&
						Alphabet.SelectedPoint.Description == _point.Description) {
						_messageHint = "The data is up to date.";
						_messageType = MessageType.Info;
					} else {
						pointFieldValidation();
					}
					break;
				}
				break;
			case AlphabetWindowTab.Edges:

				break;
			}
			EditorGUILayout.HelpBox(_messageHint, _messageType);
		}
		// Content of submition.
		void LayoutSubmitionButton() {
			// When click apply button.
			switch (_editingMode) {
			case EditingMode.Create:
				GUI.enabled = (_messageType != MessageType.Error && _messageType != MessageType.Warning);
				if (! GUILayout.Button("Add this symbol into alphabet", EditorStyles.miniButton, EditorStyle.ButtonHeight)) { break; }
				// When click the button, revoke all selected symbols and add the symbon in list.
				switch (_currentTab) {
				case AlphabetWindowTab.Points:
					Alphabet.RevokeAllSelected();
					Alphabet.AddPoint(new SpaceGrammarPoint(_point));
					Alphabet.Points.Last().Selected = true;
					break;
				case AlphabetWindowTab.Edges:

					break;
				}
				// Make the scroll position in list to bottom, and switch to modify mode.
				_scrollPosition.y = Mathf.Infinity;
				_editingMode      = EditingMode.Modify;
				// Unfocus from the field.
				GUI.FocusControl("FocusToNothing");
				GUI.enabled = true;
				break;
			case EditingMode.Modify:
				GUI.enabled = (_messageType != MessageType.Error && _messageType != MessageType.Warning);
				if (! GUILayout.Button("Update the changed", EditorStyles.miniButton, EditorStyle.ButtonHeight)) { break; }
				// When click the button, update the symbol informations.
				switch (_currentTab) {
				case AlphabetWindowTab.Points:
					// Update in alphabet and mission grammar.
					Updatepoint(Alphabet.SelectedPoint);
					break;
				case AlphabetWindowTab.Edges:

					break;
				}
				// Unfocus from the field.
				GUI.FocusControl("FocusToNothing");
				GUI.enabled = true;
				break;
			}
		}
		// Control whole events.
		void EventController() {
			if (Event.current.type == EventType.MouseDown) {
				OnClickedElementInList(Event.current.mousePosition.y - _symbolListCanvasInWindow.y);
			}
		}
		// [TODO] Temporary. The click event listener for list canvas.
		void OnClickedElementInList(float y) {
			if (y > 0 && y < EditorStyle.AlphabetSymbolListHeightValue) {			
				// [TODO] This is temporary to assign value, will promote it soon.
				int index = (int) (y + _scrollPosition.y) / 50;
				Alphabet.RevokeAllSelected();
				switch (_currentTab) {
				case AlphabetWindowTab.Points:
					if (index < Alphabet.Points.Count) {
						Alphabet.Points[index].Selected = true;
						UpdateFields(Alphabet.Points[index]);
					} else {
						// Initial the fields.
						InitFields();
					}
					break;
				case AlphabetWindowTab.Edges:

					break;
				}
				// Switch to the normal mode.
				_editingMode = EditingMode.None;
			}
		}
	}
}

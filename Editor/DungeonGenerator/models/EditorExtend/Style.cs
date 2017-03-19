using UnityEngine;
using UnityEditor;
using System.Collections;

namespace EditorExtend {
	public class Style {
		// Font size.
		private const int _headerFontSize  = 24;
		private const int _contentFontSize = 12;
		// Text field.
		private const int _textFieldHeight = 15;
		private const int _textAreaHeight  = 60;
		// Button.
		private const int _buttonHeight       = 20;
		private const int _tabButtonHeight    = 25;
		private const int _submitButtonHeight = 30;
		// Block.
		private const int _paddingAfterBlock = 10;
		// Canvas.
		private const int _alphabetSymbolListHeight = 150;
		private static Rect _alphabetSymbolListArea   = new Rect(0,   0, Screen.width, 1000);
		private static Rect _alphabetSymbolListCanvas = new Rect(0,   0, Screen.width, 1000);
		private static Rect _alphabetPreviewArea      = new Rect(0, 250, Screen.width,  150);
		private static Rect _alphabetPreviewCanvas    = new Rect(0,   0, Screen.width,  150);
		private static Rect _afterAlphabetPreviewArea = new Rect(0, 400, Screen.width, Screen.height);

		private static Rect _spaceRulePreviewArea     = new Rect (0, 250, Screen.width, 150);
		private static Rect _spaceRulePreviewCanvas   = new Rect (0,   0, Screen.width, 150);

		private static Rect _rulePreviewArea          = new Rect(0, 150, Screen.width, 300);
		private static Rect _afterRulePreviewArea     = new Rect(0, 400, Screen.width, Screen.height);

		// Font size.
		public static int HeaderFontSize {
			get { return _headerFontSize; }
		}
		public static int ContentFontSize {
			get { return _contentFontSize; }
		}
		// Text field.
		public static GUILayoutOption TextFieldHeight {
			get { return GUILayout.Height(_textFieldHeight); }
		}
		public static GUILayoutOption TextAreaHeight {
			get { return GUILayout.Height(_textAreaHeight); }
		}
		// Button.
		public static GUILayoutOption ButtonHeight {
			get { return GUILayout.Height(_buttonHeight); }
		}
		public static GUILayoutOption TabButtonHeight {
			get { return GUILayout.Height(_tabButtonHeight); }
		}
		public static GUILayoutOption SubmitButtonHeight {
			get { return GUILayout.Height(_submitButtonHeight); }
		}
		// Block.
		public static int PaddingAfterBlock {
			get { return _paddingAfterBlock; }
		}
		// Canvas.
		public static GUILayoutOption AlphabetSymbolListHeight {
			get { return GUILayout.Height(_alphabetSymbolListHeight); }
		}
		public static int AlphabetSymbolListHeightValue {
			get { return _alphabetSymbolListHeight; }
		}
		public static Rect AlphabetSymbolListArea {
			get {
				_alphabetSymbolListArea.width = Screen.width;
				return _alphabetSymbolListArea;
			}
		}
		public static Rect AlphabetSymbolListCanvas {
			get {
				_alphabetSymbolListCanvas.width = Screen.width;
				return _alphabetSymbolListCanvas;
			}
		}
		public static Rect AlphabetPreviewArea {
			get {
				_alphabetPreviewArea.width = Screen.width;
				return _alphabetPreviewArea;
			}
		}
		public static Rect AlphabetPreviewCanvas {
			get {
				_alphabetPreviewCanvas.width = Screen.width;
				return _alphabetPreviewCanvas;
			}
		}
		public static Rect AfterAlphabetPreviewArea {
			get {
				_afterAlphabetPreviewArea.width  = Screen.width;
				_afterAlphabetPreviewArea.height = Screen.height;
				return _afterAlphabetPreviewArea;
			}
		}
		public static Rect RulePreviewArea {
			get {
				_rulePreviewArea.width = Screen.width;
				return _rulePreviewArea;
			}
		}
		private const int _ruleScrollViewHeight = 200;
		public static GUILayoutOption RuleScrollViewHeight {
			get { return GUILayout.Height(_ruleScrollViewHeight); }
		}
		// RuleSourceCanvas
		private static int _ruleSourceCanvasWidth = 10;
		private static int _ruleSourceCanvasHeight = 10;
		private static int _ruleSourceCanvasRightBorder = (int) (_ruleSourceCanvasWidth * 0.9);
		private static int _ruleSourceCanvasBottomBorder = (int) (_ruleSourceCanvasHeight * 0.9);    
		public static void ResizeRuleSourceCanvas(int width, int height) {
			_ruleSourceCanvasWidth  = width;
			_ruleSourceCanvasHeight = height;
			_ruleSourceCanvasRightBorder  = (int) (width * 0.9);
			_ruleSourceCanvasBottomBorder = (int) (height * 0.9);
		}
		private static Rect _ruleSourceCanvas = new Rect(0, 0, _ruleSourceCanvasWidth, _ruleSourceCanvasHeight);
		public static Rect RuleSourceCanvas {
			get {
				_ruleSourceCanvas.width = _ruleSourceCanvasWidth;
				_ruleSourceCanvas.height = _ruleSourceCanvasHeight;
				return _ruleSourceCanvas;
			}
		}
		private static GUIStyle _ruleSourceCanvasContent = new GUIStyle(GUI.skin.label);
		public static GUIStyle RuleSourceCanvasContent {
			get {
				_ruleSourceCanvasContent.padding.right  = _ruleSourceCanvasRightBorder;
				_ruleSourceCanvasContent.padding.bottom = _ruleSourceCanvasBottomBorder;
				return _ruleSourceCanvasContent;
			}
		}
		// RuleReplacementCanvas
		private static int _ruleReplacementCanvasWidth = 10;
		private static int _ruleReplacementCanvasHeight = 10;
		private static int _ruleReplacementCanvasRightBorder = (int)(_ruleReplacementCanvasWidth * 0.9);
		private static int _ruleReplacementCanvasBottomBorder = (int)(_ruleReplacementCanvasHeight * 0.9);
		public static void ResizeRuleReplacementCanvas(int width, int height) {
			_ruleReplacementCanvasWidth  = width;
			_ruleReplacementCanvasHeight = height;
			_ruleReplacementCanvasRightBorder  = (int)(width * 0.9);
			_ruleReplacementCanvasBottomBorder = (int)(height * 0.9);
		}
		private static Rect _ruleReplacementCanvas = new Rect(0, 0, _ruleReplacementCanvasWidth, _ruleReplacementCanvasHeight);
		public static Rect RuleReplacementCanvas {
			get {
				_ruleReplacementCanvas.width  = _ruleReplacementCanvasWidth;
				_ruleReplacementCanvas.height = _ruleReplacementCanvasHeight;
				return _ruleReplacementCanvas;
			}
		}
		private static GUIStyle _ruleReplacementCanvasContent = new GUIStyle(GUI.skin.label);
		public static GUIStyle RuleReplacementCanvasContent {
			get {
				_ruleReplacementCanvasContent.padding.right  = _ruleReplacementCanvasRightBorder;
				_ruleReplacementCanvasContent.padding.bottom = _ruleReplacementCanvasBottomBorder;
				return _ruleReplacementCanvasContent;
			}
		}

		public static Rect AfterRulePreviewArea {
			get {
				_afterRulePreviewArea.width  = Screen.width;
				_afterRulePreviewArea.height = Screen.height;
				return _afterRulePreviewArea;
			}
		}
		public static Rect SpaceRulePreviewArea {
			get{
				_spaceRulePreviewArea.width = Screen.width;
				return _spaceRulePreviewArea;
			}
		}
		public static Rect SpaceRulePreviewCanvas {
			get{
				_spaceRulePreviewCanvas.width = Screen.width;
				return _spaceRulePreviewCanvas;
			}
		}

		private static GUIStyle _labelInNodeList       = new GUIStyle(GUI.skin.label);
		private static Font     _labelInNodeListFont   = Resources.Load("Fonts/texgyrecursor") as Font;
		private static Vector2  _labelInNodeListOffset = new Vector2(55, 0);

		public static GUIStyle LabelInNodeList {
			get {
				if (_labelInNodeList.name != "LabelInNodeList") {
					_labelInNodeList.name           = "LabelInNodeList";
					_labelInNodeList.fontSize       = 12;
					_labelInNodeList.margin.top     = 0;
					_labelInNodeList.margin.bottom  = 0;
					_labelInNodeList.padding.top    = 17;
					_labelInNodeList.padding.bottom = 18;
					_labelInNodeList.contentOffset  = _labelInNodeListOffset;
				}
				return _labelInNodeList;
			}
		}

		private static GUIStyle _labelInConnectionList       = new GUIStyle(GUI.skin.label);
		private static Vector2  _labelInConnectionListOffset = new Vector2(75, 0);

		public static GUIStyle LabelInConnectionList {
			get {
				if (_labelInNodeList.name != "LabelInConnectionList") {
					_labelInNodeList.name           = "LabelInConnectionList";
					_labelInNodeList.fontSize       = 12;
					_labelInNodeList.margin.top     = 0;
					_labelInNodeList.margin.bottom  = 0;
					_labelInNodeList.padding.top    = 17;
					_labelInNodeList.padding.bottom = 18;
					_labelInNodeList.contentOffset  = _labelInConnectionListOffset;
				}
				return _labelInNodeList;
			}
		}


		private const int _header2FontSize = 18;
		private static GUIStyle _header2 = new GUIStyle(GUI.skin.label);

		public static GUIStyle Header2 {
			get {
				if (_header2.name != "Header2") {
					_header2.fontSize  = _header2FontSize;
					_header2.alignment = TextAnchor.UpperLeft;
				}
				return _header2;
			}
		}

	}
}
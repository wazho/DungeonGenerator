using UnityEngine;
using UnityEditor;
using System.Collections;

namespace EditorExtend {
	public class Style {
		private static Rect _indexWindowCanvasArea = new Rect(0, 0, Screen.width, 250);
		private static Rect _indexWindowCanvas = new Rect(0, 0, Screen.width, 250);
		private static Rect _afterIndexWindowCanvasArea = new Rect(0, 250, Screen.width, Screen.height);

		// Block.
		private const int _paddingAfterBlock = 10;
		public static int PaddingAfterBlock {
			get { return _paddingAfterBlock; }
		}

		public static Rect IndexWindowCanvas {
			get {
				_indexWindowCanvas.width = Screen.width;
				return _indexWindowCanvas;
			}
		}
		public static Rect IndexWindowCanvasArea {
			get {
				_indexWindowCanvasArea.width = Screen.width;
				return _indexWindowCanvasArea;
			}
		}
		public static Rect AfterIndexWindowCanvasArea {
			get {
				_afterIndexWindowCanvasArea.width = Screen.width;
				return _afterIndexWindowCanvasArea;
			}
		}
	}

	public class CommonStyle {
		// Header 1.
		private static GUIStyle _headerOne = new GUIStyle(GUI.skin.label);
		public static GUIStyle HeaderOne {
			get {
				if (_headerOne.name != "HeaderOne") {
					_headerOne.fontSize  = 24;
					_headerOne.alignment = TextAnchor.MiddleCenter;
				}
				return _headerOne;
			}
		}
		// Height of header 1.
		private const int _headerOneHeight = 30;
		public static int HeaderOneHeight {
			get { return _headerOneHeight; }
		}
		public static GUILayoutOption HeaderOneHeightLayout {
			get { return GUILayout.Height(_headerOneHeight); }
		}
		// Header 2.
		private static GUIStyle _headerTwo = new GUIStyle(GUI.skin.label);
		public static GUIStyle HeaderTwo {
			get {
				if (_headerTwo.name != "HeaderTwo") {
					_headerTwo.fontSize  = 18;
					_headerTwo.alignment = TextAnchor.UpperLeft;
				}
				return _headerTwo;
			}
		}
		// Block.
		private const int _paddingSpace = 10;
		public static int PaddingSpace {
			get { return _paddingSpace; }
		}
		// Font size.
		private const int _headerFontSize = 24;
		public static int HeaderFontSize {
			get { return _headerFontSize; }
		}
		private const int _contentFontSize = 12;
		public static int ContentFontSize {
			get { return _contentFontSize; }
		}
		// Text field.
		private const int _textFieldHeight = 15;
		public static GUILayoutOption TextFieldHeight {
			get { return GUILayout.Height(_textFieldHeight); }
		}
		private const int _textAreaHeight  = 60;
		public static GUILayoutOption TextAreaHeight {
			get { return GUILayout.Height(_textAreaHeight); }
		}
		// Button.
		private const int _buttonHeight = 20;
		public static GUILayoutOption ButtonHeight {
			get { return GUILayout.Height(_buttonHeight); }
		}
		private const int _tabButtonHeight = 25;
		public static GUILayoutOption TabButtonHeight {
			get { return GUILayout.Height(_tabButtonHeight); }
		}
		private const int _submitButtonHeight = 30;
		public static GUILayoutOption SubmitButtonHeight {
			get { return GUILayout.Height(_submitButtonHeight); }
		}
	}

	public class MissionAlphabetWindow {
		private static Rect _symbolListArea = new Rect(0, 0, Screen.width, 1000);
		public static Rect SymbolListArea {
			get {
				_symbolListArea.width = Screen.width;
				return _symbolListArea;
			}
		}
		private static Rect _symbolListCanvas = new Rect(0, 0, Screen.width, 1000);
		public static Rect SymbolListCanvas {
			get {
				_symbolListCanvas.width = Screen.width;
				return _symbolListCanvas;
			}
		}
		private static Rect _symbolPreviewArea = new Rect(0, 250, Screen.width, 150);
		public static Rect SymbolPreviewArea {
			get {
				_symbolPreviewArea.width = Screen.width;
				return _symbolPreviewArea;
			}
		}
		private static Rect _symbolPreviewCanvas = new Rect(0, 0, Screen.width, 150);
		public static Rect SymbolPreviewCanvas {
			get {
				_symbolPreviewCanvas.width = Screen.width;
				return _symbolPreviewCanvas;
			}
		}
		private static Rect _symbolPropertiesArea = new Rect(0, 400, Screen.width, Screen.height);
		public static Rect SymbolPropertiesArea {
			get {
				_symbolPropertiesArea.width  = Screen.width;
				_symbolPropertiesArea.height = Screen.height;
				return _symbolPropertiesArea;
			}
		}
	}

	public class MissionRuleWindow {
		private static Rect _rulesArea = new Rect(0, 150, Screen.width, 300);
		public static Rect RulesArea {
			get {
				_rulesArea.width = Screen.width;
				return _rulesArea;
			}
		}
		private static Rect _sourceRuleArea = new Rect(0, 25, Screen.width / 2, 200);
		public static Rect SourceRuleArea {
			get {
				_sourceRuleArea.width = Screen.width/2;
				return _sourceRuleArea;
			}
		}
		private static Rect _replacementRuleArea = new Rect(Screen.width / 2, 25, Screen.width / 2, 200);
		public static Rect ReplacementRuleArea {
			get {
				_replacementRuleArea.x = Screen.width / 2;
				_replacementRuleArea.width = Screen.width/2;
				return _replacementRuleArea;
			}
		}
		private static Rect _ruleGraphGrammarCanvas = new Rect(0, 0, Screen.width, 200);
		public static Rect RuleGraphGrammarCanvas {
			get {
				_ruleGraphGrammarCanvas.width = Screen.width / 2;
				return _ruleGraphGrammarCanvas;
			}
		}
		private static Rect _orderingSliderArea = new Rect(0, 390, Screen.width, 30);
		public static Rect OrderingSliderArea {
			get {
				_orderingSliderArea.width  = Screen.width;
				return _orderingSliderArea;
			}
		}
		private static Rect _editorArea = new Rect(0, 420, Screen.width, Screen.height);
		public static Rect EditorArea {
			get {
				_editorArea.width  = Screen.width;
				_editorArea.height = Screen.height;
				return _editorArea;
			}
		}
		private static Rect _symbolListArea = new Rect(0, 0, Screen.width, 1000);
		public static Rect SymbolListArea {
			get {
				_symbolListArea.width = Screen.width;
				return _symbolListArea;
			}
		}
		private static Rect _symbolListCanvas = new Rect(0, 0, Screen.width, 1000);
		public static Rect SymbolListCanvas {
			get {
				_symbolListCanvas.width = Screen.width;
				return _symbolListCanvas;
			}
		}
	}

	public class GenerationWindow {
		private static Rect _missionGraphCanvasArea = new Rect(0, 50, Screen.width, 300);
		public static Rect MissionGraphArea {
			get {
				_missionGraphCanvasArea.width = Screen.width;
				return _missionGraphCanvasArea;
			}
		}
		// MissionGraphCanvas
		private static int _missionGraphCanvasWidth  = 10;
		private static int _missionGraphCanvasHeight = 10;
		private static Rect _missionGraphCanvas = new Rect(0, 0, _missionGraphCanvasWidth, _missionGraphCanvasHeight);
		public static Rect MissionGraphCanvas {
			get {
				_missionGraphCanvas.width = _missionGraphCanvasWidth;
				_missionGraphCanvas.height = _missionGraphCanvasHeight;
				return _missionGraphCanvas;
			}
		}
		public static void ResizeMissionGraphCanvas(int width, int height) {
			_missionGraphCanvasWidth  = width;
			_missionGraphCanvasHeight = height;
		}
		private static GUIStyle _missionGraphCanvasContent = new GUIStyle(GUI.skin.label);
		public static GUIStyle MissionGraphCanvasContent {
			get {
				// Values are ordered to avoid the boundary of canvas.
				_missionGraphCanvasContent.padding.right  = _missionGraphCanvasWidth - 10;
				_missionGraphCanvasContent.padding.bottom = _missionGraphCanvasHeight - 20;
				return _missionGraphCanvasContent;
			}
		}
	}

	public class SymbolList {
		// Height of the container.
		private const int _height = 150;
		public static int Height {
			get { return _height; }
		}
		public static GUILayoutOption HeightLayout {
			get { return GUILayout.Height(_height); }
		}
		// Using label to express the element border.
		private static Font     _labelInNodeListFont   = Resources.Load("Fonts/texgyrecursor") as Font;
		private static GUIStyle _labelInNodeList       = new GUIStyle(GUI.skin.label);
		private static Vector2  _labelInNodeListOffset = new Vector2(55, 0);
		public static GUIStyle NodeElement {
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
		// Using label to express the element border.
		private static GUIStyle _labelInConnectionList       = new GUIStyle(GUI.skin.label);
		private static Vector2  _labelInConnectionListOffset = new Vector2(75, 0);
		public static GUIStyle ConnectionElement {
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
	}

	public class GraphCanvas {
		private const int _ruleScrollViewHeight = 200;
		public static GUILayoutOption RuleScrollViewHeightLayout {
			get { return GUILayout.Height(_ruleScrollViewHeight); }
		}
		// RuleSourceCanvas
		private static int _sourceCanvasWidth  = 10;
		private static int _sourceCanvasHeight = 10;
		public static void ResizeSourceCanvas(int width, int height) {
			_sourceCanvasWidth  = width;
			_sourceCanvasHeight = height;
		}
		private static Rect _sourceCanvas = new Rect(0, 0, _sourceCanvasWidth, _sourceCanvasHeight);
		public static Rect SourceCanvas {
			get {
				_sourceCanvas.width = _sourceCanvasWidth;
				_sourceCanvas.height = _sourceCanvasHeight;
				return _sourceCanvas;
			}
		}
		private static GUIStyle _sourceCanvasContent = new GUIStyle(GUI.skin.label);
		public static GUIStyle SourceCanvasContent {
			get {
				// 0.9 value is ordered to avoid the boundary of canvas.
				_sourceCanvasContent.padding.right  = (int) (_sourceCanvasWidth * 0.9);
				_sourceCanvasContent.padding.bottom = (int) (_sourceCanvasHeight * 0.9);
				return _sourceCanvasContent;
			}
		}
		// RuleReplacementCanvas
		private static int _replacementCanvasWidth  = 10;
		private static int _replacementCanvasHeight = 10;
		public static void ResizeReplacementCanvas(int width, int height) {
			_replacementCanvasWidth  = width;
			_replacementCanvasHeight = height;
		}
		private static Rect _replacementCanvas = new Rect(0, 0, _replacementCanvasWidth, _replacementCanvasHeight);
		public static Rect ReplacementCanvas {
			get {
				_replacementCanvas.width  = _replacementCanvasWidth;
				_replacementCanvas.height = _replacementCanvasHeight;
				return _replacementCanvas;
			}
		}
		private static GUIStyle _replacementCanvasContent = new GUIStyle(GUI.skin.label);
		public static GUIStyle ReplacementCanvasContent {
			get {
				// 0.9 value is ordered to avoid the boundary of canvas.
				_replacementCanvasContent.padding.right  = (int) (_replacementCanvasWidth * 0.9);
				_replacementCanvasContent.padding.bottom = (int) (_replacementCanvasHeight * 0.9);
				return _replacementCanvasContent;
			}
		}
	}
}
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
		private static Rect _alphabetSymbolListArea   = new Rect(0,   0, Screen.width, 1000);
		private static Rect _alphabetSymbolListCanvas = new Rect(0,   0, Screen.width, 1000);
		private static Rect _alphabetPreviewArea      = new Rect(0, 250, Screen.width,  150);
		private static Rect _alphabetPreviewCanvas    = new Rect(0,   0, Screen.width,  150);
		private static Rect _afterAlphabetPreviewArea = new Rect(0, 400, Screen.width, Screen.height);

		private static Rect _spaceRulePreviewArea     = new Rect (0, 250, Screen.width, 150);
		private static Rect _spaceRulePreviewCanvas   = new Rect (0, 0, Screen.width, 150);

        private static Rect _rulePreviewArea          = new Rect(0, 150, Screen.width, 300);
        private static Rect _ruleSourceCanvas         = new Rect(0,  25, Screen.width / 2, 200);
        private static Rect _ruleReplacementCanvas    = new Rect(Screen.width / 2, 25, Screen.width / 2, 200);
        private static Rect _ruleCanvasSlider         = new Rect(0, 225, Screen.width, 10);
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
        public static Rect RuleSourceCanvas {
            get {
                _ruleSourceCanvas.width = Screen.width/2;
                return _ruleSourceCanvas;
            }
        }
        public static Rect RuleReplacementCanvas {
            get {
                _ruleReplacementCanvas.x = Screen.width / 2;
                _ruleReplacementCanvas.width = Screen.width/2;
                return _ruleReplacementCanvas;
            }
        }
        public static Rect RuleCanvasSlider {
            get {
                _ruleCanvasSlider.width = Screen.width;
                return _ruleCanvasSlider;
            }
        }       
        public static Rect AfterRulePreviewArea {
            get {
                _afterRulePreviewArea.width = Screen.width;
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
	}
}
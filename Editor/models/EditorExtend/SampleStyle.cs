using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using EditorStyle = EditorExtend.Style;

namespace EditorExtend {
	public class SampleStyle {
		public enum ButtonType {
			Regular, 
			Mid,
			Left,
			Right
		};
		public enum ButtonColor {
			Blue,
			Green,
			Grey,
			Yellow,
			Orange
		};
		public enum ButtonState {
			Normal,
			Hover,
			Active
		};

		// Textures
		public static Texture2D SplashImage { get { return Resources.Load<Texture2D>("Images/Splash"); } }

		public static MultiKeyDictionary<ButtonColor, ButtonType, ButtonState, Texture2D> ButtonStyleDict = new MultiKeyDictionary<ButtonColor, ButtonType, ButtonState, Texture2D>() {
			// Regular Normal.
			{ ButtonColor.Blue,   ButtonType.Regular, ButtonState.Normal, Resources.Load<Texture2D>("Images/UI/Blue/regular_normal_blue")     as Texture2D },
			{ ButtonColor.Green,  ButtonType.Regular, ButtonState.Normal, Resources.Load<Texture2D>("Images/UI/Green/regular_normal_green")   as Texture2D },
			{ ButtonColor.Grey,   ButtonType.Regular, ButtonState.Normal, Resources.Load<Texture2D>("Images/UI/Grey/regular_normal_grey")     as Texture2D },
			{ ButtonColor.Orange, ButtonType.Regular, ButtonState.Normal, Resources.Load<Texture2D>("Images/UI/Orange/regular_normal_orange") as Texture2D },
			{ ButtonColor.Yellow, ButtonType.Regular, ButtonState.Normal, Resources.Load<Texture2D>("Images/UI/Yellow/regular_normal_yellow") as Texture2D },
			// Regular Hover.
			{ ButtonColor.Blue,   ButtonType.Regular, ButtonState.Hover,  Resources.Load<Texture2D>("Images/UI/Blue/regular_hover_blue")      as Texture2D },
			{ ButtonColor.Green,  ButtonType.Regular, ButtonState.Hover,  Resources.Load<Texture2D>("Images/UI/Green/regular_hover_green")    as Texture2D },
			{ ButtonColor.Grey,   ButtonType.Regular, ButtonState.Hover,  Resources.Load<Texture2D>("Images/UI/Grey/regular_hover_grey")      as Texture2D },
			{ ButtonColor.Orange, ButtonType.Regular, ButtonState.Hover,  Resources.Load<Texture2D>("Images/UI/Orange/regular_hover_orange")  as Texture2D },
			{ ButtonColor.Yellow, ButtonType.Regular, ButtonState.Hover,  Resources.Load<Texture2D>("Images/UI/Yellow/regular_hover_yellow")  as Texture2D },
			// Regular Active.
			{ ButtonColor.Blue,   ButtonType.Regular, ButtonState.Active, Resources.Load<Texture2D>("Images/UI/Blue/regular_active_blue")     as Texture2D },
			{ ButtonColor.Green,  ButtonType.Regular, ButtonState.Active, Resources.Load<Texture2D>("Images/UI/Green/regular_active_green")   as Texture2D },
			{ ButtonColor.Grey,   ButtonType.Regular, ButtonState.Active, Resources.Load<Texture2D>("Images/UI/Grey/regular_active_grey")     as Texture2D },
			{ ButtonColor.Orange, ButtonType.Regular, ButtonState.Active, Resources.Load<Texture2D>("Images/UI/Orange/regular_active_orange") as Texture2D },
			{ ButtonColor.Yellow, ButtonType.Regular, ButtonState.Active, Resources.Load<Texture2D>("Images/UI/Yellow/regular_active_yellow") as Texture2D },
			// Mid Normal.
			{ ButtonColor.Blue,   ButtonType.Mid,     ButtonState.Normal, Resources.Load<Texture2D>("Images/UI/Blue/mid_normal_blue")         as Texture2D },
			{ ButtonColor.Green,  ButtonType.Mid,     ButtonState.Normal, Resources.Load<Texture2D>("Images/UI/Green/mid_normal_green")       as Texture2D },
			{ ButtonColor.Grey,   ButtonType.Mid,     ButtonState.Normal, Resources.Load<Texture2D>("Images/UI/Grey/mid_normal_grey")         as Texture2D },
			{ ButtonColor.Orange, ButtonType.Mid,     ButtonState.Normal, Resources.Load<Texture2D>("Images/UI/Orange/mid_normal_orange")     as Texture2D },
			{ ButtonColor.Yellow, ButtonType.Mid,     ButtonState.Normal, Resources.Load<Texture2D>("Images/UI/Yellow/mid_normal_yellow")     as Texture2D },
			// Mid Hover.
			{ ButtonColor.Blue,   ButtonType.Mid,     ButtonState.Hover,  Resources.Load<Texture2D>("Images/UI/Blue/mid_hover_blue")          as Texture2D },
			{ ButtonColor.Green,  ButtonType.Mid,     ButtonState.Hover,  Resources.Load<Texture2D>("Images/UI/Green/mid_hover_green")        as Texture2D },
			{ ButtonColor.Grey,   ButtonType.Mid,     ButtonState.Hover,  Resources.Load<Texture2D>("Images/UI/Grey/mid_hover_grey")          as Texture2D },
			{ ButtonColor.Orange, ButtonType.Mid,     ButtonState.Hover,  Resources.Load<Texture2D>("Images/UI/Orange/mid_hover_orange")      as Texture2D },
			{ ButtonColor.Yellow, ButtonType.Mid,     ButtonState.Hover,  Resources.Load<Texture2D>("Images/UI/Yellow/mid_hover_yellow")      as Texture2D },
			// Mid Active.
			{ ButtonColor.Blue,   ButtonType.Mid,     ButtonState.Active, Resources.Load<Texture2D>("Images/UI/Blue/mid_active_blue")         as Texture2D },
			{ ButtonColor.Green,  ButtonType.Mid,     ButtonState.Active, Resources.Load<Texture2D>("Images/UI/Green/mid_active_green")       as Texture2D },
			{ ButtonColor.Grey,   ButtonType.Mid,     ButtonState.Active, Resources.Load<Texture2D>("Images/UI/Grey/mid_active_grey")         as Texture2D },
			{ ButtonColor.Orange, ButtonType.Mid,     ButtonState.Active, Resources.Load<Texture2D>("Images/UI/Orange/mid_active_orange")     as Texture2D },
			{ ButtonColor.Yellow, ButtonType.Mid,     ButtonState.Active, Resources.Load<Texture2D>("Images/UI/Yellow/mid_active_yellow")     as Texture2D },
			// Left Normal.
			{ ButtonColor.Blue,   ButtonType.Left,    ButtonState.Normal, Resources.Load<Texture2D>("Images/UI/Blue/left_normal_blue")        as Texture2D },
			{ ButtonColor.Green,  ButtonType.Left,    ButtonState.Normal, Resources.Load<Texture2D>("Images/UI/Green/left_normal_green")      as Texture2D },
			{ ButtonColor.Grey,   ButtonType.Left,    ButtonState.Normal, Resources.Load<Texture2D>("Images/UI/Grey/left_normal_grey")        as Texture2D },
			{ ButtonColor.Orange, ButtonType.Left,    ButtonState.Normal, Resources.Load<Texture2D>("Images/UI/Orange/left_normal_orange")    as Texture2D },
			{ ButtonColor.Yellow, ButtonType.Left,    ButtonState.Normal, Resources.Load<Texture2D>("Images/UI/Yellow/left_normal_yellow")    as Texture2D },
			// Left Hover.
			{ ButtonColor.Blue,   ButtonType.Left,    ButtonState.Hover,  Resources.Load<Texture2D>("Images/UI/Blue/left_hover_blue")         as Texture2D },
			{ ButtonColor.Green,  ButtonType.Left,    ButtonState.Hover,  Resources.Load<Texture2D>("Images/UI/Green/left_hover_green")       as Texture2D },
			{ ButtonColor.Grey,   ButtonType.Left,    ButtonState.Hover,  Resources.Load<Texture2D>("Images/UI/Grey/left_hover_grey")         as Texture2D },
			{ ButtonColor.Orange, ButtonType.Left,    ButtonState.Hover,  Resources.Load<Texture2D>("Images/UI/Orange/left_hover_orange")     as Texture2D },
			{ ButtonColor.Yellow, ButtonType.Left,    ButtonState.Hover,  Resources.Load<Texture2D>("Images/UI/Yellow/left_hover_yellow")     as Texture2D },
			// Left Active.
			{ ButtonColor.Blue,   ButtonType.Left,    ButtonState.Active, Resources.Load<Texture2D>("Images/UI/Blue/left_active_blue")        as Texture2D },
			{ ButtonColor.Green,  ButtonType.Left,    ButtonState.Active, Resources.Load<Texture2D>("Images/UI/Green/left_active_green")      as Texture2D },
			{ ButtonColor.Grey,   ButtonType.Left,    ButtonState.Active, Resources.Load<Texture2D>("Images/UI/Grey/left_active_grey")        as Texture2D },
			{ ButtonColor.Orange, ButtonType.Left,    ButtonState.Active, Resources.Load<Texture2D>("Images/UI/Orange/left_active_orange")    as Texture2D },
			{ ButtonColor.Yellow, ButtonType.Left,    ButtonState.Active, Resources.Load<Texture2D>("Images/UI/Yellow/left_active_yellow")    as Texture2D },
			// Right Normal.
			{ ButtonColor.Blue,   ButtonType.Right,   ButtonState.Normal, Resources.Load<Texture2D>("Images/UI/Blue/right_normal_blue")        as Texture2D },
			{ ButtonColor.Green,  ButtonType.Right,   ButtonState.Normal, Resources.Load<Texture2D>("Images/UI/Green/right_normal_green")      as Texture2D },
			{ ButtonColor.Grey,   ButtonType.Right,   ButtonState.Normal, Resources.Load<Texture2D>("Images/UI/Grey/right_normal_grey")        as Texture2D },
			{ ButtonColor.Orange, ButtonType.Right,   ButtonState.Normal, Resources.Load<Texture2D>("Images/UI/Orange/right_normal_orange")    as Texture2D },
			{ ButtonColor.Yellow, ButtonType.Right,   ButtonState.Normal, Resources.Load<Texture2D>("Images/UI/Yellow/right_normal_yellow")    as Texture2D },
			// Right Hover.
			{ ButtonColor.Blue,   ButtonType.Right,   ButtonState.Hover,  Resources.Load<Texture2D>("Images/UI/Blue/right_hover_blue")         as Texture2D },
			{ ButtonColor.Green,  ButtonType.Right,   ButtonState.Hover,  Resources.Load<Texture2D>("Images/UI/Green/right_hover_green")       as Texture2D },
			{ ButtonColor.Grey,   ButtonType.Right,   ButtonState.Hover,  Resources.Load<Texture2D>("Images/UI/Grey/right_hover_grey")         as Texture2D },
			{ ButtonColor.Orange, ButtonType.Right,   ButtonState.Hover,  Resources.Load<Texture2D>("Images/UI/Orange/right_hover_orange")     as Texture2D },
			{ ButtonColor.Yellow, ButtonType.Right,   ButtonState.Hover,  Resources.Load<Texture2D>("Images/UI/Yellow/right_hover_yellow")     as Texture2D },
			// Right Active.
			{ ButtonColor.Blue,   ButtonType.Right,   ButtonState.Active, Resources.Load<Texture2D>("Images/UI/Blue/right_active_blue")        as Texture2D },
			{ ButtonColor.Green,  ButtonType.Right,   ButtonState.Active, Resources.Load<Texture2D>("Images/UI/Green/right_active_green")      as Texture2D },
			{ ButtonColor.Grey,   ButtonType.Right,   ButtonState.Active, Resources.Load<Texture2D>("Images/UI/Grey/right_active_grey")        as Texture2D },
			{ ButtonColor.Orange, ButtonType.Right,   ButtonState.Active, Resources.Load<Texture2D>("Images/UI/Orange/right_active_orange")    as Texture2D },
			{ ButtonColor.Yellow, ButtonType.Right,   ButtonState.Active, Resources.Load<Texture2D>("Images/UI/Yellow/right_active_yellow")    as Texture2D },
		};


		// Colors
		public static Color ColorDarkestBlue    = new Color( 22f / 255f, 110f / 255f, 147f / 255f);
		public static Color ColorDarkBlue       = new Color( 29f / 255f, 167f / 255f, 226f / 255f);
		public static Color ColorBlue           = new Color( 64f / 255f, 181f / 255f, 225f / 255f);
		public static Color ColorLightBlue      = new Color(118f / 255f, 208f / 255f, 255f / 255f);
		public static Color ColorLightestBlue   = new Color(166f / 255f, 226f / 255f, 255f / 255f);
		public static Color ColorDarkestGreen   = new Color( 22f / 255f, 100f / 255f,  91f / 255f);
		public static Color ColorDarkGreen      = new Color( 79f / 255f, 167f / 255f, 121f / 255f);
		public static Color ColorGreen          = new Color(119f / 255f, 207f / 255f, 104f / 255f);
		public static Color ColorLightGreen     = new Color(136f / 255f, 224f / 255f,  96f / 255f);
		public static Color ColorLightestGreen  = new Color(158f / 255f, 249f / 255f, 144f / 255f);
		public static Color ColorDarkestGrey    = new Color( 49f / 255f,  81f / 255f,  99f / 255f);
		public static Color ColorDarkGrey       = new Color(103f / 255f, 153f / 255f, 184f / 255f);
		public static Color ColorGrey           = new Color(130f / 255f, 174f / 255f, 192f / 255f);
		public static Color ColorLightGrey      = new Color(206f / 255f, 219f / 255f, 225f / 255f);
		public static Color ColorLightestGrey   = new Color(238f / 255f, 238f / 255f, 238f / 255f);
		public static Color ColorDarkestOrange  = new Color(193f / 255f, 125f / 255f,  74f / 255f);
		public static Color ColorDarkOrange     = new Color(216f / 255f, 126f / 255f,  65f / 255f);
		public static Color ColorOrange         = new Color(250f / 255f, 129f / 255f,  50f / 255f);
		public static Color ColorLightOrange    = new Color(250f / 255f, 159f / 255f,  71f / 255f);
		public static Color ColorLightestOrange = new Color(250f / 255f, 196f / 255f,  91f / 255f);
		public static Color ColorDarkestYellow  = new Color(197f / 255f, 191f / 255f,  91f / 255f);
		public static Color ColorDarkYellow     = new Color(220f / 255f, 201f / 255f,  83f / 255f);
		public static Color ColorYellow         = new Color(255f / 255f, 217f / 255f,  72f / 255f);
		public static Color ColorLightYellow    = new Color(255f / 255f, 223f / 255f, 143f / 255f);
		public static Color ColorLightestYellow = new Color(255f / 255f, 240f / 255f, 168f / 255f);

		// Fonts
		public static Font TabFont { get { return Resources.Load<Font>("Fonts/kenvector_future") as Font; } }

		// Grid's default parameters
		private const int _minorGridSize = 20;
		public static int MinorGridSize {
			get { return _minorGridSize; }
		}
		private const int _majorGridSize = 100;
		public static int MajorGridSize {
			get { return _majorGridSize; }
		}
		private static Color _gridColor = ColorLightBlue;
		public static Color GridColor {
			get { return _gridColor; }
		}
		private static Color _gridBackgroundColor = ColorLightestBlue;
		public static Color GridBackgroundColor {
			get { return _gridBackgroundColor; }
		}

		// Font size ; Generally same with the things in CommonStyle class. The default alignment is center. 
		private const int _headerOneFontSize = 24;
		public static int HeaderOneFontSize {
			get { return _headerOneFontSize; }
		}
		private const int _headerTwoFontSize = 18;
		public static int HeaderTwoFontSize {
			get { return _headerTwoFontSize; }
		}
		private const int _headerThreeFontSize = 14;
		public static int HeaderThreeFontSize {
			get { return _headerThreeFontSize; }
		}
		private const int _contentFontSize = 12;
		public static int ContentFontSize {
			get { return _contentFontSize; }
		}

		// Layout heights ; Generally same with things in CommonStyle class. 
		private const int _headerOneHeight = 30;
		public static int HeaderOneHeight {
			get { return _headerOneHeight; }
		}
		public static GUILayoutOption HeaderOneHeightLayout {
			get { return GUILayout.Height(_headerOneHeight); }
		}
		private const int _headerTwoHeight = 24;
		public static int HeaderTwoHeight {
			get { return _headerTwoHeight; }
		}
		public static GUILayoutOption HeaderTwoHeightLayout {
			get { return GUILayout.Height(_headerTwoHeight); }
		}
		private const int _headerThreeHeight = 20;
		public static int HeaderThreeHeight {
			get { return _headerThreeHeight; }
		}
		public static GUILayoutOption HeaderThreeHeightLayout {
			get { return GUILayout.Height(_headerThreeHeight); }
		}
		private const int _contentHeight = 18;
		public static int ContentHeight {
			get { return _contentHeight; }
		}
		public static GUILayoutOption ContentHeightLayout {
			get { return GUILayout.Height(_contentHeight); }
		}


		// Padding, Margin, Offset 
		private const int _paddingArea = 10;
		public static int PaddingArea {
			get { return _paddingArea; }
		}
		private const int _paddingBlock = 5;
		public static int PaddingBlock {
			get { return _paddingBlock; }
		}

		// Button heights ; Generally, same with in CommonStyle
		private const int _miniButtonHeight = 16;
		public static GUILayoutOption MiniButtonHeight {
			get { return GUILayout.Height(_miniButtonHeight); }
		}
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
		private const int _mainButtonHeight = 32;
		public static GUILayoutOption MainButtonHeight {
			get { return GUILayout.Height(_mainButtonHeight); }
		}

		// Textfield and textarea heights
		private const int _textFieldHeight = 15;
		public static GUILayoutOption TextFieldHeight {
			get { return GUILayout.Height(_textFieldHeight); }
		}
		private const int _textAreaHeight  = 60;
		public static GUILayoutOption TextAreaHeight {
			get { return GUILayout.Height(_textAreaHeight); }
		}
		private const int _enumPopUpHeight = 15;
		public static GUILayoutOption EnumPopUpHeight {
			get { return GUILayout.Height(_enumPopUpHeight); }
		}
		private const int _popUpHeight = 15;
		public static GUILayoutOption PopUpHeight {
			get { return GUILayout.Height(_popUpHeight); }
		}

		// GUIStyles
		// Header & Content styles ; Generally same with the things in CommonStyle class. The default alignment is center. 
		private static GUIStyle _headerOne = new GUIStyle(EditorStyles.label);
		public static GUIStyle HeaderOne {
			get {
				_headerOne.fontSize  = 24;
				_headerOne.alignment = TextAnchor.MiddleCenter;
				_headerOne.font = TabFont;
				return _headerOne;
			}
		}
		private static GUIStyle _headerTwo = new GUIStyle(EditorStyles.label);
		public static GUIStyle HeaderTwo {
			get {
				_headerTwo.fontSize  = 18;
				_headerTwo.font = TabFont;
				_headerTwo.alignment = TextAnchor.MiddleCenter;
				return _headerTwo;
			}
		}
		private static GUIStyle _headerThree = new GUIStyle(EditorStyles.label);
		public static GUIStyle HeaderThree {
			get {
				_headerThree.fontSize  = 14;
				_headerThree.alignment = TextAnchor.MiddleCenter;
				return _headerThree;
			}
		}
		private static GUIStyle _contentParagraph = new GUIStyle(EditorStyles.label);
		public static GUIStyle ContentParagraph {
			get {
				_contentParagraph.wordWrap = true;
				_contentParagraph.fontSize  = 12;
				_contentParagraph.alignment = TextAnchor.UpperLeft;
				return _contentParagraph;
			}
		}

		// Button styles ; Their default colors are blue. 
		private static GUIStyle _button = new GUIStyle(EditorStyles.miniButton);
		public static GUIStyle Button {
			get {
				_button.fontSize            = _contentFontSize;
				_button.normal.background   = ButtonStyleDict[ButtonColor.Blue, ButtonType.Regular, ButtonState.Normal];
				_button.onNormal.background = ButtonStyleDict[ButtonColor.Blue, ButtonType.Regular, ButtonState.Active];
				_button.hover.background    = ButtonStyleDict[ButtonColor.Blue, ButtonType.Regular, ButtonState.Hover];
				_button.active.background   = ButtonStyleDict[ButtonColor.Blue, ButtonType.Regular, ButtonState.Active];
				_button.normal.textColor    = Color.white;
				_button.hover.textColor     = Color.white;
				_button.active.textColor    = Color.white;
				return _button;
			}
		}
		private static GUIStyle _buttonLeft = new GUIStyle(EditorStyles.miniButtonLeft);
		public static GUIStyle ButtonLeft {
			get {
				_buttonLeft.fontSize            = _contentFontSize;
				_buttonLeft.normal.background   = ButtonStyleDict[ButtonColor.Blue, ButtonType.Left, ButtonState.Normal];
				_buttonLeft.onNormal.background = ButtonStyleDict[ButtonColor.Blue, ButtonType.Left, ButtonState.Active];
				_buttonLeft.hover.background    = ButtonStyleDict[ButtonColor.Blue, ButtonType.Left, ButtonState.Hover];
				_buttonLeft.active.background   = ButtonStyleDict[ButtonColor.Blue, ButtonType.Left, ButtonState.Active];
				_buttonLeft.normal.textColor    = Color.white;
				_buttonLeft.hover.textColor     = Color.white;
				_buttonLeft.active.textColor    = Color.white;

				return _buttonLeft;
			}
		}
		private static GUIStyle _buttonMid = new GUIStyle(EditorStyles.miniButtonMid);
		public static GUIStyle ButtonMid {
			get {
				_buttonMid.fontSize            = _contentFontSize;
				_buttonMid.normal.background   = ButtonStyleDict[ButtonColor.Blue, ButtonType.Mid, ButtonState.Normal];
				_buttonMid.onNormal.background = ButtonStyleDict[ButtonColor.Blue, ButtonType.Mid, ButtonState.Active];
				_buttonMid.hover.background    = ButtonStyleDict[ButtonColor.Blue, ButtonType.Mid, ButtonState.Hover];
				_buttonMid.active.background   = ButtonStyleDict[ButtonColor.Blue, ButtonType.Mid, ButtonState.Active];
				_buttonMid.normal.textColor    = Color.white;
				_buttonMid.hover.textColor     = Color.white;
				_buttonMid.active.textColor    = Color.white;
				return _buttonMid;
			}
		}
		private static GUIStyle _buttonRight = new GUIStyle(EditorStyles.miniButtonRight);
		public static GUIStyle ButtonRight {
			get { 
				_buttonRight.fontSize            = _contentFontSize;
				_buttonRight.normal.background   = ButtonStyleDict[ButtonColor.Blue, ButtonType.Right, ButtonState.Normal];
				_buttonRight.onNormal.background = ButtonStyleDict[ButtonColor.Blue, ButtonType.Right, ButtonState.Active];
				_buttonRight.hover.background    = ButtonStyleDict[ButtonColor.Blue, ButtonType.Right, ButtonState.Hover];
				_buttonRight.active.background   = ButtonStyleDict[ButtonColor.Blue, ButtonType.Right, ButtonState.Active];
				_buttonRight.normal.textColor    = Color.white;
				_buttonRight.hover.textColor     = Color.white;
				_buttonRight.active.textColor    = Color.white;
				return _buttonRight;
			}
		}

		public static GUIStyle GetButtonStyle(ButtonType buttonType, ButtonColor buttonColor) {
			GUIStyle buttonStyle = new GUIStyle(Button);

			if (buttonType == ButtonType.Mid) {
				buttonStyle = new GUIStyle(ButtonMid);
			} else if (buttonType == ButtonType.Left) {
				buttonStyle = new GUIStyle(ButtonLeft);
			} else if (buttonType == ButtonType.Right) {
				buttonStyle = new GUIStyle(ButtonRight);
			}

			buttonStyle.normal.background   = ButtonStyleDict[buttonColor, buttonType, ButtonState.Normal];
			buttonStyle.onNormal.background = ButtonStyleDict[buttonColor, buttonType, ButtonState.Active];
			buttonStyle.hover.background    = ButtonStyleDict[buttonColor, buttonType, ButtonState.Hover];
			buttonStyle.active.background   = ButtonStyleDict[buttonColor, buttonType, ButtonState.Active];

			return buttonStyle;
		}

		// Boxes
		private static GUIStyle _box = new GUIStyle(GUI.skin.box);
		public static GUIStyle Box {
			get {
				// Change the background later
				_box.normal.background = GUI.skin.window.active.background;
				return _box;
			}
		}

		// Frames
		private static GUIStyle _frameLightestBlue = new GUIStyle(GUI.skin.box);
		public static GUIStyle FrameLightestBlue {
			get {
				Texture2D t = new Texture2D(1, 1);
				t.SetPixel(0, 0, ColorLightestBlue);
				t.Apply();
				_frameLightestBlue.normal.background = t; // GUI.skin.window.active.background;
				return _frameLightestBlue;
			}
		}

		public static GUIStyle Frame(Color color){
			GUIStyle frame = new GUIStyle(GUI.skin.box);
			frame.normal.background = GetTextureFromColor(color);
			return frame;
		}

		// TextArea and Textfield styles, label etc
		private static GUIStyle _textArea = new GUIStyle(EditorStyles.textArea);
		public static GUIStyle TextArea {
			get {
				_textArea.focused.background = GetTextureFromColor(ColorLightBlue);
				_textArea.normal.textColor = Color.black;
				_textArea.focused.textColor = Color.white;
				return _textArea;
			}
		}
		private static GUIStyle _textField = new GUIStyle(EditorStyles.textField);
		public static GUIStyle TextField {
			get {
				_textField.focused.background = GetTextureFromColor(ColorLightBlue);
				_textField.normal.textColor = Color.black;
				_textField.focused.textColor = Color.white;
				return _textField;
			}
		}
		private static GUIStyle _colorField = new GUIStyle(EditorStyles.colorField);
		public static GUIStyle ColorField {
			get {
				return _colorField;
			}
		}
		private static GUIStyle _enumPopUp = new GUIStyle(EditorStyles.popup);
		public static GUIStyle EnumPopUp {
			get {
				_enumPopUp.normal.textColor  = Color.black;
				_enumPopUp.focused.textColor = SampleStyle.ColorDarkestBlue;
				_enumPopUp.hover.textColor   = SampleStyle.ColorDarkestBlue;
				return _enumPopUp;
			}
		}private static GUIStyle _popUp = new GUIStyle(EditorStyles.popup);
		public static GUIStyle PopUp {
			get {
				_popUp.padding = new RectOffset(6, 6, 2, 2);

				_popUp.normal.textColor  = Color.black;
				_popUp.focused.textColor = SampleStyle.ColorDarkestBlue;
				_popUp.hover.textColor   = SampleStyle.ColorDarkestBlue;
				return _popUp;
			}
		}

		private static GUIStyle _textFieldLabel = new GUIStyle(EditorStyles.label);
		public static GUIStyle TextFieldLabel {
			get { 
				_textFieldLabel.focused.textColor = ColorDarkestBlue;
				return _textFieldLabel;
			}
		}
		private static GUIStyle _textAreaLabel = new GUIStyle(EditorStyles.label);
		public static GUIStyle TextAreaLabel {
			get {
				_textAreaLabel.focused.textColor = ColorDarkestBlue;
				return _textAreaLabel;
			}
		}
		private static GUIStyle _colorFieldLabel = new GUIStyle(EditorStyles.label);
		public static GUIStyle ColorFieldLabel {
			get {
				_colorFieldLabel.focused.textColor = ColorDarkestBlue;
				return _textAreaLabel;
			}
		}
		private static GUIStyle _enumPopUpLabel = new GUIStyle(EditorStyles.label);
		public static GUIStyle EnumPopUpLabel {
			get { 
				_enumPopUpLabel.focused.textColor = ColorDarkestBlue;
				return _enumPopUpLabel;
			}
		}
		private static GUIStyle _popUpLabel = new GUIStyle(EditorStyles.label);
		public static GUIStyle PopUpLabel {
			get { 
				_popUpLabel.focused.textColor = ColorDarkestBlue;
				return _popUpLabel;
			}
		}
		public static string TextFieldLabeled(string label, string text, GUIStyle labelStyle, GUIStyle textFieldStyle, params GUILayoutOption[] options){
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel(label, TextField, labelStyle);
			text = EditorGUILayout.TextField(text, textFieldStyle, options);
			EditorGUILayout.EndHorizontal();	
			return text;
		}
		public static string TextAreaLabeled(string label, string text, GUIStyle labelStyle, GUIStyle textAreaStyle, params GUILayoutOption[] options){
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel(label, TextArea, labelStyle);
			text = EditorGUILayout.TextArea(text, textAreaStyle, options);
			EditorGUILayout.EndHorizontal();	
			return text;
		}
		public static Color ColorFieldLabeled(string label, Color color, GUIStyle labelStyle, GUIStyle colorFieldStyle, params GUILayoutOption[] options){
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel(label, ColorField, labelStyle);
			color = EditorGUILayout.ColorField(color, options); 
			EditorGUILayout.EndHorizontal();	
			return color;
		}
		public static Enum EnumPopupLabeled(string label, Enum enumPopUp, GUIStyle labelStyle, GUIStyle enumPopUpStyle, params GUILayoutOption[] options){
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel(label, EnumPopUp, EnumPopUpLabel);
			enumPopUp = EditorGUILayout.EnumPopup(enumPopUp, enumPopUpStyle, options);
			EditorGUILayout.EndHorizontal(); 	
			return enumPopUp;
		}
		public static int PopupLabeled(string label, int selectedIndex, string[] displayedOptions, GUIStyle labelStyle, GUIStyle PopUpStyle, int width, params GUILayoutOption[] options){
			EditorGUILayout.BeginHorizontal(GUILayout.Width(width));
			EditorGUILayout.PrefixLabel(label, PopUp, PopUpLabel);
			selectedIndex = EditorGUILayout.Popup(selectedIndex, displayedOptions, PopUpStyle, options);
			EditorGUILayout.EndHorizontal(); 	
			return selectedIndex;
		}

		public static Texture2D GetTextureFromColor(Color color){
			Texture2D texture = new Texture2D(1, 1);
			texture.SetPixel(0, 0, color);
			texture.Apply();
			return texture;
		}

		public static void DebugRect(Rect rect, Color color){
			color.a = 0.5f;
			EditorGUI.DrawRect(rect, color); 
			// Debug.Log("Rect "+color.ToString()+" = (X : " + rect.x + ", Y: " + rect.y + "). (W: " + rect.width + ", H: " + rect.height + "). (Cnt X: " + rect.center.x + ", Cnt Y: " + rect.center.y + ")");
		}

		public static Rect GetRectOfWindow(){
			return new Rect(0, 0, Screen.width, Screen.height);
		}

		public static void DrawWindowBackground(Color color){
			EditorGUI.DrawRect(GetRectOfWindow(), color);
		}
		// Suggestion : default color for background is light grey/ light blue, default color for grid is white or light blue.
		public static void DrawGrid(Rect rect, int minor, int major, Color rectColor, Color gridColor){
			// Draw Background
			EditorGUI.DrawRect(rect, rectColor); 

			// Vertical lines
			Vector3[] majorLines; 
			for (int i = 0, j = 0; i <= rect.width; i += minor, j += major) {
				Handles.BeginGUI();
				Handles.color = gridColor;
				Handles.DrawLine(new Vector2(rect.x + i, rect.y), new Vector2(rect.x + i, rect.height));
				majorLines = new [] { new Vector3(rect.x + j, rect.y), new Vector3(rect.x + j, rect.height) };
				Handles.DrawAAPolyLine(3f, majorLines);
				Handles.EndGUI();
			}
			// Horizontal lines
			for (int i = 0, j = 0; i <= rect.height; i += minor, j += major) {
				Handles.BeginGUI();
				Handles.color = gridColor;
				Handles.DrawLine(new Vector2(rect.x, rect.y + i), new Vector2(rect.width, rect.y + i));
				majorLines = new [] { new Vector3(rect.x, rect.y + j), new Vector3(rect.width, rect.y + j) };
				Handles.DrawAAPolyLine(3f, majorLines);
				Handles.EndGUI();
			}
		}

		// Multi-keys Dictionary. (Becuase Unity doesn't support 'Tuple class' in .Net 4.0.)
		public class MultiKeyDictionary<Key1, Key2, T>: Dictionary<Key1, Dictionary<Key2, T>> {
			public T this[Key1 key1, Key2 key2] {
				get {
					return base[key1][key2];
				}
				set {
					if (! ContainsKey(key1)) {
						this[key1] = new Dictionary<Key2, T>();
					}
					this[key1][key2] = value;
				}
			}

			public void Add(Key1 key1, Key2 key2, T value) {
				if (! ContainsKey(key1)) {
					this[key1] = new Dictionary<Key2, T>();
				}
				this[key1][key2] = value;
			}

			public bool ContainsKey(Key1 key1, Key2 key2) {
				return base.ContainsKey(key1) && this[key1].ContainsKey(key2);
			}
		}

		public class MultiKeyDictionary<Key1, Key2, Key3, T> : Dictionary<Key1, MultiKeyDictionary<Key2, Key3, T>> {
			public T this[Key1 key1, Key2 key2, Key3 key3] {
				get {
					return ContainsKey(key1) ? this[key1][key2, key3] : default(T);
				}
				set {
					if (! ContainsKey(key1)) {
						this[key1] = new MultiKeyDictionary<Key2, Key3, T>();
					}
					this[key1][key2, key3] = value;
				}
			}

			public void Add(Key1 key1, Key2 key2, Key3 key3, T value) {
				if (! ContainsKey(key1)) {
					this[key1] = new MultiKeyDictionary<Key2, Key3, T>();
				}
				this[key1][key2, key3] = value;
			}

			public bool ContainsKey(Key1 key1, Key2 key2, Key3 key3) {
				return base.ContainsKey(key1) && this[key1].ContainsKey(key2, key3);
			}
		}
	}

	// Separate them just like in Style.cs
	public class IndexWindow {
		private static Rect _indexWindowSplashCanvasArea = new Rect(0, 0, Screen.width, Screen.width / 2);
		private static Rect _indexWindowButtonsCanvasArea = new Rect(0, _indexWindowSplashCanvasArea.height, Screen.width, Screen.width / 4);
		public static Rect IndexWindowSplashCanvasArea {
			get { 
				_indexWindowSplashCanvasArea.width = Screen.width;
				_indexWindowSplashCanvasArea.height = Screen.width / 2;
				return _indexWindowSplashCanvasArea;
			}
		}
		public static Rect IndexWindowButtonsCanvasArea {
			get { 
				_indexWindowSplashCanvasArea.x = 0;
				_indexWindowButtonsCanvasArea.y = _indexWindowSplashCanvasArea.y + _indexWindowSplashCanvasArea.height;
				_indexWindowButtonsCanvasArea.width = Screen.width;
				_indexWindowButtonsCanvasArea.height = Screen.height - _indexWindowSplashCanvasArea.height;
				return _indexWindowButtonsCanvasArea; 
			}
		}	
	}
}

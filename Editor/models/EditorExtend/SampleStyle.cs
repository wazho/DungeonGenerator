using UnityEngine;
using UnityEditor;
using System.Collections;

using EditorAdvance = EditorExtend.Advance;
using EditorStyle   = EditorExtend.Style;

namespace EditorExtend {
	public class SampleStyle{

		public enum ButtonType {
			Regular, 
			Mid,
			Left,
			Right
		};
		public enum CustomButtonColor {
			Blue,
			Green,
			Grey,
			Yellow,
			Orange
		};

		// Textures
		public static Texture2D SplashImage { get { return Resources.Load<Texture2D>("Images/Splash"); } }

		// Regular
		public static Texture2D RegularNormalBlue { get { return Resources.Load<Texture2D>("Images/UI/Blue/regular_normal_blue") as Texture2D; } }
		public static Texture2D RegularNormalGreen { get { return Resources.Load<Texture2D>("Images/UI/Green/regular_normal_green") as Texture2D; } } 
		public static Texture2D RegularNormalGrey { get { return Resources.Load<Texture2D>("Images/UI/Grey/regular_normal_grey") as Texture2D; } }
		public static Texture2D RegularNormalOrange { get { return Resources.Load<Texture2D>("Images/UI/Orange/regular_normal_orange") as Texture2D; } }
		public static Texture2D RegularNormalYellow { get { return Resources.Load<Texture2D>("Images/UI/Yellow/regular_normal_yellow") as Texture2D; } }
		public static Texture2D RegularHoverBlue { get { return Resources.Load<Texture2D>("Images/UI/Blue/regular_hover_blue") as Texture2D; } }
		public static Texture2D RegularHoverGreen { get { return Resources.Load<Texture2D>("Images/UI/Green/regular_hover_green") as Texture2D; } }
		public static Texture2D RegularHoverGrey { get { return Resources.Load<Texture2D>("Images/UI/Grey/regular_hover_grey") as Texture2D; } }
		public static Texture2D RegularHoverOrange { get { return Resources.Load<Texture2D>("Images/UI/Orange/regular_hover_orange") as Texture2D; } }
		public static Texture2D RegularHoverYellow { get { return Resources.Load<Texture2D>("Images/UI/Yellow/regular_hover_yellow") as Texture2D; } }
		public static Texture2D RegularActiveBlue { get { return Resources.Load<Texture2D>("Images/UI/Blue/regular_active_blue") as Texture2D; } }
		public static Texture2D RegularActiveGreen { get { return Resources.Load<Texture2D>("Images/UI/Green/regular_active_green") as Texture2D; } }
		public static Texture2D RegularActiveGrey { get { return Resources.Load<Texture2D>("Images/UI/Grey/regular_active_grey") as Texture2D; } }
		public static Texture2D RegularActiveOrange { get { return Resources.Load<Texture2D>("Images/UI/Orange/regular_active_orange") as Texture2D; } }
		public static Texture2D RegularActiveYellow { get { return Resources.Load<Texture2D>("Images/UI/Yellow/regular_active_yellow") as Texture2D; } }
		// Mid 
		public static Texture2D MidNormalBlue{ get { return Resources.Load<Texture2D>("Images/UI/Blue/mid_normal_blue") as Texture2D; } }
		public static Texture2D MidNormalGreen{ get { return Resources.Load<Texture2D>("Images/UI/Green/mid_normal_green") as Texture2D; } }
		public static Texture2D MidNormalGrey { get { return Resources.Load<Texture2D>("Images/UI/Grey/mid_normal_grey") as Texture2D; } }
		public static Texture2D MidNormalOrange { get { return Resources.Load<Texture2D>("Images/UI/Orange/mid_normal_orange") as Texture2D; } }
		public static Texture2D MidNormalYellow { get { return Resources.Load<Texture2D>("Images/UI/Yellow/mid_normal_yellow") as Texture2D; } }
		public static Texture2D MidHoverBlue { get { return Resources.Load<Texture2D>("Images/UI/Blue/mid_hover_blue") as Texture2D; } }
		public static Texture2D MidHoverGreen { get { return Resources.Load<Texture2D>("Images/UI/Green/mid_hover_green") as Texture2D; } }
		public static Texture2D MidHoverGrey { get { return Resources.Load<Texture2D>("Images/UI/Grey/mid_hover_grey") as Texture2D; } }
		public static Texture2D MidHoverOrange { get { return Resources.Load<Texture2D>("Images/UI/Orange/mid_hover_orange") as Texture2D; } }
		public static Texture2D MidHoverYellow { get { return Resources.Load<Texture2D>("Images/UI/Yellow/mid_hover_yellow") as Texture2D; } }
		public static Texture2D MidActiveBlue { get { return Resources.Load<Texture2D>("Images/UI/Blue/mid_active_blue") as Texture2D; } }
		public static Texture2D MidActiveGreen { get { return Resources.Load<Texture2D>("Images/UI/Green/mid_active_green") as Texture2D; } }
		public static Texture2D MidActiveGrey { get { return Resources.Load<Texture2D>("Images/UI/Grey/mid_active_grey") as Texture2D; } }
		public static Texture2D MidActiveOrange { get { return Resources.Load<Texture2D>("Images/UI/Orange/mid_active_orange") as Texture2D; } }
		public static Texture2D MidActiveYellow { get { return Resources.Load<Texture2D>("Images/UI/Yellow/mid_active_yellow") as Texture2D; } }
		// Left
		public static Texture2D LeftNormalBlue{ get { return Resources.Load<Texture2D>("Images/UI/Blue/left_normal_blue") as Texture2D; } }
		public static Texture2D LeftNormalGreen{ get { return Resources.Load<Texture2D>("Images/UI/Green/left_normal_green") as Texture2D; } }
		public static Texture2D LeftNormalGrey { get { return Resources.Load<Texture2D>("Images/UI/Grey/left_normal_grey") as Texture2D; } }
		public static Texture2D LeftNormalOrange { get { return Resources.Load<Texture2D>("Images/UI/Orange/left_normal_orange") as Texture2D; } }
		public static Texture2D LeftNormalYellow { get { return Resources.Load<Texture2D>("Images/UI/Yellow/left_normal_yellow") as Texture2D; } }
		public static Texture2D LeftHoverBlue { get { return Resources.Load<Texture2D>("Images/UI/Blue/left_hover_blue") as Texture2D; } }
		public static Texture2D LeftHoverGreen { get { return Resources.Load<Texture2D>("Images/UI/Green/left_hover_green") as Texture2D; } }
		public static Texture2D LeftHoverGrey { get { return Resources.Load<Texture2D>("Images/UI/Grey/left_hover_grey") as Texture2D; } }
		public static Texture2D LeftHoverOrange { get { return Resources.Load<Texture2D>("Images/UI/Orange/left_hover_orange") as Texture2D; } }
		public static Texture2D LeftHoverYellow { get { return Resources.Load<Texture2D>("Images/UI/Yellow/left_hover_yellow") as Texture2D; } }
		public static Texture2D LeftActiveBlue { get { return Resources.Load<Texture2D>("Images/UI/Blue/left_active_blue") as Texture2D; } }
		public static Texture2D LeftActiveGreen { get { return Resources.Load<Texture2D>("Images/UI/Green/left_active_green") as Texture2D; } }
		public static Texture2D LeftActiveGrey { get { return Resources.Load<Texture2D>("Images/UI/Grey/left_active_grey") as Texture2D; } }
		public static Texture2D LeftActiveOrange { get { return Resources.Load<Texture2D>("Images/UI/Orange/left_active_orange") as Texture2D; } }
		public static Texture2D LeftActiveYellow { get { return Resources.Load<Texture2D>("Images/UI/Yellow/left_active_yellow") as Texture2D; } }
		// Right
		public static Texture2D RightNormalBlue{ get { return Resources.Load<Texture2D>("Images/UI/Blue/right_normal_blue") as Texture2D; } }
		public static Texture2D RightNormalGreen{ get { return Resources.Load<Texture2D>("Images/UI/Green/right_normal_green") as Texture2D; } }
		public static Texture2D RightNormalGrey { get { return Resources.Load<Texture2D>("Images/UI/Grey/right_normal_grey") as Texture2D; } }
		public static Texture2D RightNormalOrange { get { return Resources.Load<Texture2D>("Images/UI/Orange/right_normal_orange") as Texture2D; } }
		public static Texture2D RightNormalYellow { get { return Resources.Load<Texture2D>("Images/UI/Yellow/right_normal_yellow") as Texture2D; } }
		public static Texture2D RightHoverBlue { get { return Resources.Load<Texture2D>("Images/UI/Blue/right_hover_blue") as Texture2D; } }
		public static Texture2D RightHoverGreen { get { return Resources.Load<Texture2D>("Images/UI/Green/right_hover_green") as Texture2D; } }
		public static Texture2D RightHoverGrey { get { return Resources.Load<Texture2D>("Images/UI/Grey/right_hover_grey") as Texture2D; } }
		public static Texture2D RightHoverOrange { get { return Resources.Load<Texture2D>("Images/UI/Orange/right_hover_orange") as Texture2D; } }
		public static Texture2D RightHoverYellow { get { return Resources.Load<Texture2D>("Images/UI/Yellow/right_hover_yellow") as Texture2D; } }
		public static Texture2D RightActiveBlue { get { return Resources.Load<Texture2D>("Images/UI/Blue/right_active_blue") as Texture2D; } }
		public static Texture2D RightActiveGreen { get { return Resources.Load<Texture2D>("Images/UI/Green/right_active_green") as Texture2D; } }
		public static Texture2D RightActiveGrey { get { return Resources.Load<Texture2D>("Images/UI/Grey/right_active_grey") as Texture2D; } }
		public static Texture2D RightActiveOrange { get { return Resources.Load<Texture2D>("Images/UI/Orange/right_active_orange") as Texture2D; } }
		public static Texture2D RightActiveYellow { get { return Resources.Load<Texture2D>("Images/UI/Yellow/right_active_yellow") as Texture2D; } }

		// Colors
		public static Color ColorDarkestBlue { get { return new Color(22f / 255f, 110f / 255f, 147f / 255f); } }
		public static Color ColorDarkBlue { get { return new Color(29f / 255f, 167f / 255f, 226f/ 255f); } }
		public static Color ColorBlue { get { return new Color(64f / 255f, 181f / 255f, 225f/ 255f); } }
		public static Color ColorLightBlue { get { return new Color(118f / 255f, 208f / 255f, 255f / 255f); } }
		public static Color ColorLightestBlue { get { return new Color(166f / 255f, 226f / 255f, 255f / 255f); } }
		public static Color ColorDarkestGreen { get { return new Color(22f / 255f, 100f / 255f, 91f / 255f); } }
		public static Color ColorDarkGreen { get { return new Color(79f / 255f, 167f / 255f, 121f/ 255f); } }
		public static Color ColorGreen { get { return new Color(119f / 255f, 207f / 255f, 104f/ 255f); } }
		public static Color ColorLightGreen { get { return new Color(136f / 255f, 224f / 255f, 96f / 255f); } }
		public static Color ColorLightestGreen { get { return new Color(158f / 255f, 249f / 255f, 144f / 255f); } }
		public static Color ColorDarkestGrey { get { return new Color(49f / 255f, 81f / 255f, 99f / 255f); } }
		public static Color ColorDarkGrey { get { return new Color(103f / 255f, 153f / 255f, 184f/ 255f); } }
		public static Color ColorGrey { get { return new Color(130f / 255f, 174f / 255f, 192f/ 255f); } }
		public static Color ColorLightGrey { get { return new Color(206f / 255f, 219f / 255f, 225f / 255f); } }
		public static Color ColorLightestGrey { get { return new Color(238f / 255f, 238f / 255f, 238f / 255f); } }
		public static Color ColorDarkestOrange { get { return new Color(193f / 255f, 125f / 255f, 74f / 255f); } }
		public static Color ColorDarkOrange { get { return new Color(216f / 255f, 126f / 255f, 65f/ 255f); } }
		public static Color ColorOrange { get { return new Color(250f / 255f, 129f / 255f, 50f/ 255f); } }
		public static Color ColorLightOrange { get { return new Color(250f / 255f, 159f / 255f, 71f / 255f); } }
		public static Color ColorLightestOrange { get { return new Color(250f / 255f, 196f / 255f, 91f / 255f); } }
		public static Color ColorDarkestYellow { get { return new Color(197f / 255f, 191f / 255f, 91f / 255f); } }
		public static Color ColorDarkYellow { get { return new Color(220f / 255f, 201f / 255f, 83f/ 255f); } }
		public static Color ColorYellow { get { return new Color(255f / 255f, 217f / 255f, 72f/ 255f); } }
		public static Color ColorLightYellow { get { return new Color(255f / 255f, 223f / 255f, 143f / 255f); } }
		public static Color ColorLightestYellow { get { return new Color(255f / 255f, 240f / 255f, 168f / 255f); } }

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
				_button.normal.background = RegularNormalBlue;
				_button.hover.background = RegularHoverBlue;
				_button.active.background = RegularActiveBlue;
				_button.normal.textColor = Color.white;
				_button.hover.textColor = Color.white;
				_button.active.textColor = Color.white;
				return _button;
			}
			set { }
		}
		private static GUIStyle _buttonLeft = new GUIStyle(EditorStyles.miniButtonLeft);
		public static GUIStyle ButtonLeft {
			get { 
				_buttonLeft.normal.background = LeftNormalBlue;
				_buttonLeft.hover.background = LeftHoverBlue;
				_buttonLeft.active.background = LeftActiveBlue;
				_buttonLeft.normal.textColor = Color.white;
				_buttonLeft.hover.textColor = Color.white;
				_buttonLeft.active.textColor = Color.white;
				return _buttonLeft;
			}
			set { }
		}
		private static GUIStyle _buttonMid = new GUIStyle(EditorStyles.miniButtonMid);
		public static GUIStyle ButtonMid {
			get { 
				_buttonMid.normal.background = MidNormalBlue;
				_buttonMid.hover.background = MidHoverBlue;
				_buttonMid.active.background = MidActiveBlue;
				_buttonMid.normal.textColor = Color.white;
				_buttonMid.hover.textColor = Color.white;
				_buttonMid.active.textColor = Color.white;
				return _buttonMid;
			}
			set { }
		}
		private static GUIStyle _buttonRight = new GUIStyle(EditorStyles.miniButtonRight);
		public static GUIStyle ButtonRight {
			get { 
				_buttonRight.normal.background = RightNormalBlue;
				_buttonRight.hover.background = RightHoverBlue;
				_buttonRight.active.background = RightActiveBlue;
				_buttonRight.normal.textColor = Color.white;
				_buttonRight.hover.textColor = Color.white;
				_buttonRight.active.textColor = Color.white;
				return _buttonRight;
			}
			set { }
		}

		public static GUIStyle GetButtonStyle(ButtonType buttonType, CustomButtonColor customButtonColor){
			GUIStyle buttonStyle = new GUIStyle();

			if (buttonType == ButtonType.Regular) {
				buttonStyle = new GUIStyle(Button);

				switch (customButtonColor) {
				case CustomButtonColor.Green:
					buttonStyle.normal.background = RegularNormalGreen;
					buttonStyle.hover.background = RegularHoverGreen;
					buttonStyle.active.background = RegularActiveGreen;
					break;
				case CustomButtonColor.Grey:
					buttonStyle.normal.background = RegularNormalGrey;
					buttonStyle.hover.background = RegularHoverGrey;
					buttonStyle.active.background = RegularActiveGrey;
					break;
				case CustomButtonColor.Orange:
					buttonStyle.normal.background = RegularNormalOrange;
					buttonStyle.hover.background = RegularHoverOrange;
					buttonStyle.active.background = RegularActiveOrange;
					break;
				case CustomButtonColor.Yellow:
					buttonStyle.normal.background = RegularNormalYellow;
					buttonStyle.hover.background = RegularHoverYellow;
					buttonStyle.active.background = RegularActiveYellow;
					break;
				case CustomButtonColor.Blue: 
					buttonStyle = Button;	
					break;
				}
			} else if (buttonType == ButtonType.Mid) {
				buttonStyle = new GUIStyle(ButtonMid);
				switch (customButtonColor) {
				case CustomButtonColor.Green:
					buttonStyle.normal.background = MidNormalGreen;
					buttonStyle.hover.background = MidHoverGreen;
					buttonStyle.active.background = MidActiveGreen;
					break;
				case CustomButtonColor.Grey:
					buttonStyle.normal.background = MidNormalGrey;
					buttonStyle.hover.background = MidHoverGrey;
					buttonStyle.active.background = MidActiveGrey;
					break;
				case CustomButtonColor.Orange:
					buttonStyle.normal.background = MidNormalOrange;
					buttonStyle.hover.background = MidHoverOrange;
					buttonStyle.active.background = MidActiveOrange;
					break;
				case CustomButtonColor.Yellow:
					buttonStyle.normal.background = MidNormalYellow;
					buttonStyle.hover.background = MidHoverYellow;
					buttonStyle.active.background = MidActiveYellow;
					break;
				case CustomButtonColor.Blue: 
					buttonStyle = ButtonMid;
					break;
				}
			} else if (buttonType == ButtonType.Left) {
				buttonStyle = new GUIStyle(ButtonLeft);
				switch (customButtonColor) {
				case CustomButtonColor.Green:
					buttonStyle.normal.background = LeftNormalGreen;
					buttonStyle.hover.background = LeftHoverGreen;
					buttonStyle.active.background = LeftActiveGreen;
					break;
				case CustomButtonColor.Grey:
					buttonStyle.normal.background = LeftNormalGrey;
					buttonStyle.hover.background = LeftHoverGrey;
					buttonStyle.active.background = LeftActiveGrey;
					break;
				case CustomButtonColor.Orange:
					buttonStyle.normal.background = LeftNormalOrange;
					buttonStyle.hover.background = LeftHoverOrange;
					buttonStyle.active.background = LeftActiveOrange;
					break;
				case CustomButtonColor.Yellow:
					buttonStyle.normal.background = LeftNormalYellow;
					buttonStyle.hover.background = LeftHoverYellow;
					buttonStyle.active.background = LeftActiveYellow;
					break;
				case CustomButtonColor.Blue: 
					buttonStyle = ButtonLeft;
					break;
				}
			} else if (buttonType == ButtonType.Right) {
				buttonStyle = new GUIStyle(ButtonRight);
				switch (customButtonColor) {
				case CustomButtonColor.Green:
					buttonStyle.normal.background = RightNormalGreen;
					buttonStyle.hover.background = RightHoverGreen;
					buttonStyle.active.background = RightActiveGreen;
					break;
				case CustomButtonColor.Grey:
					buttonStyle.normal.background = RightNormalGrey;
					buttonStyle.hover.background = RightHoverGrey;
					buttonStyle.active.background = RightActiveGrey;
					break;
				case CustomButtonColor.Orange:
					buttonStyle.normal.background = RightNormalOrange;
					buttonStyle.hover.background = RightHoverOrange;
					buttonStyle.active.background = RightActiveOrange;
					break;
				case CustomButtonColor.Yellow:
					buttonStyle.normal.background = RightNormalYellow;
					buttonStyle.hover.background = RightHoverYellow;
					buttonStyle.active.background = RightActiveYellow;
					break;
				case CustomButtonColor.Blue: 
					buttonStyle = ButtonRight;
					break;
				}
			} 

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
			set { }
		}
		private static GUIStyle _textField = new GUIStyle(EditorStyles.textField);
		public static GUIStyle TextField {
			get { 
				_textField.focused.background = GetTextureFromColor(ColorLightBlue);
				_textField.normal.textColor = Color.black;
				_textField.focused.textColor = Color.white;
				return _textField;
			}
			set { }
		}

		private static GUIStyle _textFieldLabel = new GUIStyle(EditorStyles.label);
		public static GUIStyle TextFieldLabel {
			get { 
				_textFieldLabel.focused.textColor = ColorDarkestBlue;
				return _textFieldLabel;
			}
			set { }
		}
		private static GUIStyle _textAreaLabel = new GUIStyle(EditorStyles.label);
		public static GUIStyle TextAreaLabel {
			get {
				_textAreaLabel.focused.textColor = ColorDarkestBlue;
				return _textAreaLabel;
			}
			set { }
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
			//Color RectColor = rectColor ?? new Color(166f / 255f, 226f / 255f, 255f / 255f);
			//Color GridColor = gridColor ?? new Color(118f / 255f, 208f / 255f, 255f / 255f);
			// Draw Background
			EditorGUI.DrawRect(rect, rectColor); 

			// Vertical lines
			int i,j;
			Vector3[] majorLines; 
			for(i = 0, j = 0; i<= rect.width; i += minor, j += major){
				Handles.BeginGUI();
				Handles.color = gridColor;
				Handles.DrawLine(new Vector2(rect.x + i, rect.y), new Vector2(rect.x + i, rect.height));
				majorLines = new [] {new Vector3(rect.x + j, rect.y), new Vector3(rect.x + j, rect.height)};
				Handles.DrawAAPolyLine(3f, majorLines);
				Handles.EndGUI();
			}
			// Horizontal lines
			for(i = 0, j = 0; i<= rect.height; i += minor, j += major){
				Handles.BeginGUI();
				Handles.color = gridColor;
				Handles.DrawLine(new Vector2(rect.x, rect.y + i), new Vector2(rect.width, rect.y + i));
				majorLines = new [] {new Vector3(rect.x, rect.y + j), new Vector3(rect.width, rect.y + j)};
				Handles.DrawAAPolyLine(3f, majorLines);
				Handles.EndGUI();
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

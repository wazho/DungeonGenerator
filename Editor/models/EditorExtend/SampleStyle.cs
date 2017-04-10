using UnityEngine;
using UnityEditor;
using System.Collections;

using EditorAdvance = EditorExtend.Advance;
using EditorStyle   = EditorExtend.Style;

namespace EditorExtend {
	public class SampleStyle{
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

		// Fonts
		public static Font TabElementFont { get { return Resources.Load<Font>("Fonts/kenvector_future") as Font; } }

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

		// GUIStyles
		// Header & Content styles ; Generally same with the things in CommonStyle class. The default alignment is center. 
		private static GUIStyle _headerOne = new GUIStyle(EditorStyles.label);
		public static GUIStyle HeaderOne {
			get {
				_headerOne.fontSize  = 24;
				_headerOne.alignment = TextAnchor.MiddleCenter;
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
				_button.onNormal.background = RegularNormalBlue;
				_button.onHover.background = RegularHoverBlue;
				_button.onActive.background = RegularActiveBlue;
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
				_buttonLeft.onNormal.background = LeftNormalBlue;
				_buttonLeft.onHover.background = LeftHoverBlue;
				_buttonLeft.onActive.background = LeftActiveBlue;
				_buttonLeft.normal.textColor = Color.white;
				_buttonLeft.hover.textColor = Color.white;
				_buttonLeft.active.textColor = Color.white;
				return _buttonLeft;
			}
			set { }
		}
		private static GUIStyle _buttonMid = new GUIStyle(EditorStyles.miniButtonMid);
		public static GUIStyle ButtonMid{
			get { 
				_buttonMid.normal.background = MidNormalBlue;
				_buttonMid.hover.background = MidHoverBlue;
				_buttonMid.active.background = MidActiveBlue;
				_buttonMid.onNormal.background = MidNormalBlue;
				_buttonMid.onHover.background = MidHoverBlue;
				_buttonMid.onActive.background = MidActiveBlue;
				_buttonMid.normal.textColor = Color.white;
				_buttonMid.hover.textColor = Color.white;
				_buttonMid.active.textColor = Color.white;
				return _buttonMid;
			}
			set { }
		}
		private static GUIStyle _buttonRight= new GUIStyle(EditorStyles.miniButtonRight);
		public static GUIStyle ButtonRight{
			get { 
				_buttonRight.normal.background = RightNormalBlue;
				_buttonRight.hover.background = RightHoverBlue;
				_buttonRight.active.background = RightActiveBlue;
				_buttonRight.onNormal.background = RightNormalBlue;
				_buttonRight.onHover.background = RightHoverBlue;
				_buttonRight.onActive.background = RightActiveBlue;
				_buttonRight.normal.textColor = Color.white;
				_buttonRight.hover.textColor = Color.white;
				_buttonRight.active.textColor = Color.white;
				return _buttonRight;
			}
			set { }
		}

		public static void DebugRect(Rect rect, Color color){
			color.a = 0.5f;
			EditorGUI.DrawRect(rect, color); 
			// Debug.Log("Rect "+color.ToString()+" = (X : " + rect.x + ", Y: " + rect.y + "). (W: " + rect.width + ", H: " + rect.height + "). (Cnt X: " + rect.center.x + ", Cnt Y: " + rect.center.y + ")");
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
				_indexWindowButtonsCanvasArea.y = _indexWindowSplashCanvasArea.y + _indexWindowSplashCanvasArea.height;
				_indexWindowButtonsCanvasArea.width = Screen.width;
				_indexWindowButtonsCanvasArea.height = Screen.height - _indexWindowSplashCanvasArea.height;
				return _indexWindowButtonsCanvasArea; 
			}
		}	
	}

}

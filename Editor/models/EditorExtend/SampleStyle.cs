using UnityEngine;
using UnityEditor;
using System.Collections;

using EditorAdvance = EditorExtend.Advance;
using EditorStyle   = EditorExtend.Style;

namespace EditorExtend {
	public class SampleStyle{
		// Delete the "Mini" later
		// Button Textures
		public static Texture2D MiniButtonLeftNormalTexture = EditorStyles.miniButtonLeft.normal.background;
		public static Texture2D MiniButtonMidNormalTexture = EditorStyles.miniButtonMid.normal.background;
		public static Texture2D MiniButtonRightNormalTexture = EditorStyles.miniButtonRight.normal.background;
		public static Texture2D MiniButtonLeftActiveTexture = EditorStyles.miniButtonLeft.active.background;
		public static Texture2D MiniButtonMidActiveTexture = EditorStyles.miniButtonMid.active.background;
		public static Texture2D MiniButtonRightActiveTexture = EditorStyles.miniButtonRight.active.background;

		// new GUIStyle(EditorStyles.miniButtonLeft); ?
		public static GUIStyle MiniButtonLeftStyle = EditorStyles.miniButtonLeft; 
		public static GUIStyle MiniButtonMidStyle = EditorStyles.miniButtonMid;
		public static GUIStyle MiniButtonRightStyle = EditorStyles.miniButtonRight;
		// Don't forget to initialize and update in onGUI

		// Text Style. Use this to create different style of label later on
		public enum LabelStyleType {
			Header,
			Content
		};
		// Don't forget to refresh after changing the style
		public static void BeginLabelStyle(LabelStyleType labelStyle){
			switch (labelStyle) {
			case LabelStyleType.Header:
				GUI.skin.label.fontSize = EditorStyle.HeaderFontSize;
				GUI.skin.label.alignment = TextAnchor.MiddleCenter;
				break;
			case LabelStyleType.Content:
			default:
				GUI.skin.label.fontSize = EditorStyle.ContentFontSize;
				GUI.skin.label.alignment = TextAnchor.UpperLeft;
				break;
			}
		}
		public static void RefreshLabelStyle(){
			BeginLabelStyle(LabelStyleType.Content);
		}
	}
}

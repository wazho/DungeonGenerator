using UnityEngine;
using UnityEditor;
using System.Collections;

namespace EditorExtend {
	public class Advance {
		// Quickly get the pure color texture of GUI.
		public static GUIStyle PureTextureGUI(Color color) {
			GUIStyle gui = new GUIStyle();
			Texture2D texture = new Texture2D(1, 1);
			texture.SetPixel(0, 0, color);
			texture.Apply();
			gui.normal.background = texture;
			return gui;
		}
		// Custom GUILayout progress bar.
		public static void ProgressBar(float value, string label) {
			// Get a rect for the progress bar using the same margins as a textfield:
			Rect rect = GUILayoutUtility.GetRect(18, 18, "TextField");
			EditorGUI.ProgressBar(rect, value, label);
			EditorGUILayout.Space();
		}
		// .
		public static int LimitedIntField(string text, int val, int min, int max) {
			int curVal = EditorGUILayout.IntField(text, val);
			return (curVal < min ? min : (curVal > max ? max : curVal));
		}
	}
}
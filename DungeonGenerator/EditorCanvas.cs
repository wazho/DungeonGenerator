using UnityEngine;
using UnityEditor;
using System.Collections;

public class EditorCanvas {
	// static Color defaultBlack = new Color(0, 0, 0, 1);
	// static Color defaultWhite = new Color(255, 255, 255, 1);

	public static void DrawQuad(Rect rect, Color color) {
		Texture2D texture = new Texture2D(1, 1);
		texture.SetPixel(0, 0, color);
		texture.Apply();
		GUI.skin.box.normal.background = texture;
		GUI.Box(rect, GUIContent.none);
	}

	public static void DrawQuad(int x, int y, int width, int height, Color color) {
		Rect rect = new Rect(x, y, width, height);
		Texture2D texture = new Texture2D(1, 1);
		texture.SetPixel(0, 0, color);
		texture.Apply();
		GUI.skin.box.normal.background = texture;
		GUI.Box(rect, GUIContent.none);
	}

	public static void DrawLine(Vector2 startPos, Vector2 endPos, Color color) {
		Handles.BeginGUI();
		Handles.color = color;
		Vector3 [] lists = new Vector3[] { new Vector3(startPos[0], startPos[1], 0), new Vector3(endPos[0], endPos[1], 0) };
		Handles.DrawAAPolyLine(5f, lists);
		Handles.EndGUI();
	}

	public static void DrawCurve(Vector3 startPos, Vector3 endPos, Color color) {
		float mnog = Vector3.Distance(startPos, endPos);
		Vector3 startTangent = startPos + Vector3.right * (mnog / 3f) ;
		Vector3 endTangent = endPos + Vector3.left * (mnog / 3f);
		Handles.BeginGUI();
		Handles.DrawBezier(startPos, endPos, startTangent, endTangent, color, null, 3f);
		Handles.EndGUI();
	}
}

public class EditorAdvance {
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
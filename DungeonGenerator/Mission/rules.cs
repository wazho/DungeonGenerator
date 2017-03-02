using UnityEngine;
using UnityEditor;
using System.Collections;

public class MissionRulesWindow : EditorWindow {
	void OnGUI() {
		GUILayout.Label ("Base Settings", EditorStyles.boldLabel);
		EditorGUILayout.TextField ("Text Field", "test here 2.");
	}
}
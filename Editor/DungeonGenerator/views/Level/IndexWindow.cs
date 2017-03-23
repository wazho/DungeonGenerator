using UnityEngine;
using UnityEditor;
using System.Collections;

using EditorAdvance = EditorExtend.Advance;
using EditorStyle   = EditorExtend.Style;

namespace DungeonLevel {
	public class IndexWindow : EditorWindow {

		private static string _description = "This is description";

		void Awake() {
			
		}

		void OnGUI() {
			GUILayout.BeginArea(EditorStyle.IndexWindowCanvasArea);
			EditorGUI.DrawRect(EditorStyle.IndexWindowCanvas, Color.white);
			GUILayout.EndArea();
			GUILayout.BeginArea(EditorStyle.AfterIndexWindowCanvasArea);
			GUILayout.Space(EditorStyle.PaddingAfterBlock);
			GUILayout.BeginHorizontal();
			if (GUILayout.Button("Create Level", EditorStyles.miniButtonLeft, EditorStyle.TabButtonHeight)) {
				// Create Level.
			}
			if (GUILayout.Button("Import Level", EditorStyles.miniButtonLeft, EditorStyle.TabButtonHeight)) {
				// Import Level.
			}
			if (GUILayout.Button("Import Rewrite", EditorStyles.miniButtonLeft, EditorStyle.TabButtonHeight)) {
				// Import Rewrite.
			}
			if (GUILayout.Button("Export Level", EditorStyles.miniButtonLeft, EditorStyle.TabButtonHeight)) {
				// Export Level.
			}
			GUILayout.EndHorizontal();
			GUILayout.Space(EditorStyle.PaddingAfterBlock);
			// Show description.
			EditorGUILayout.SelectableLabel(_description, EditorStyles.textField, EditorStyle.TextAreaHeight);
			GUILayout.EndArea();
		}
	}
}
using UnityEngine;
using UnityEditor;
using System.Collections;

using EditorStyle = EditorExtend.Style;

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
				// Safety.
				if (EditorUtility.DisplayDialog("Create new level.",
					"Are you sure want to create a new level, this will overwrite the origin level?",
					"Yes", "No")) {
					// Initialization.
					LevelSettingsWindow.Initial();
					MissionGrammarSystem.Alphabet.Initial();
					MissionGrammarSystem.MissionGrammar.Initial();
					MissionGrammarSystem.RewriteSystem.Initial();
					GraphGeneration.MissionGraphWindow.Initial();
				}
			}
			if (GUILayout.Button("Import Level", EditorStyles.miniButtonMid, EditorStyle.TabButtonHeight)) {
				// Import Level.
			}
			if (GUILayout.Button("Import Rewrite", EditorStyles.miniButtonMid, EditorStyle.TabButtonHeight)) {
				// Import Rewrite.
				string path = EditorUtility.OpenFilePanel("Import xml", "", "xml");
				if(path.Length > 0) {
					DungeonLevel.OperateXML.Unserialize.UnserializeFromXml(path);
				}
			}
			if (GUILayout.Button("Export Level", EditorStyles.miniButtonRight, EditorStyle.TabButtonHeight)) {
				// Export Level.
				string path = EditorUtility.SaveFilePanel("Export xml", "", "Level.xml", "xml");
				if (path.Length > 0) {
					DungeonLevel.OperateXML.Serialize.SerializeToXml(path);
				}
			}
			GUILayout.EndHorizontal();
			GUILayout.Space(EditorStyle.PaddingAfterBlock);
			// Show description.
			EditorGUILayout.SelectableLabel(_description, EditorStyles.textField, EditorStyle.TextAreaHeight);
			GUILayout.EndArea();
		}
	}
}

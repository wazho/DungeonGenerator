using UnityEngine;
using UnityEditor;
using System.Collections;

// will remove.
using EditorStyle = EditorExtend.Style;
using Style = EditorExtend.CommonStyle;

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
			if (GUILayout.Button("Create Level", EditorStyles.miniButtonLeft, Style.TabButtonHeight)) {
				// Create Level.
			}
			if (GUILayout.Button("Import Level", EditorStyles.miniButtonMid, Style.TabButtonHeight)) {
				// Import Level.
			}
			if (GUILayout.Button("Import Rewrite", EditorStyles.miniButtonMid, Style.TabButtonHeight)) {
				// Import Rewrite.
				string path = EditorUtility.OpenFilePanel("Import xml", "", "xml");
				if(path.Length > 0) {
					DungeonLevel.OperateXML.Unserialize.UnserializeFromXml(path);
				}
			}
			if (GUILayout.Button("Export Level", EditorStyles.miniButtonRight, Style.TabButtonHeight)) {
				// Export Level.
				string path = EditorUtility.SaveFilePanel("Export xml", "", "Level.xml", "xml");
				if (path.Length > 0) {
					DungeonLevel.OperateXML.Serialize.SerializeToXml(path);
				}
			}
			GUILayout.EndHorizontal();
			GUILayout.Space(EditorStyle.PaddingAfterBlock);
			// Show description.
			EditorGUILayout.SelectableLabel(_description, EditorStyles.textField, Style.TextAreaHeight);
			GUILayout.EndArea();
		}
	}
}

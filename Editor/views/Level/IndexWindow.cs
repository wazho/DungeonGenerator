using UnityEngine;
using UnityEditor;
using System.Collections;

// will remove.
using EditorStyle = EditorExtend.CommonStyle;
using SampleStyle = EditorExtend.SampleStyle;
using Container = EditorExtend.IndexWindow;

namespace DungeonLevel {
	public class IndexWindow : EditorWindow {

		private static string _description = 
			"Dungeon Generation is a tool for procedurally generate a Game Level specifically for Dungeon. " +
			"This tool concerns about the player progression by using Mission & Space framework originally established by Joris Dormans. " +
			"Developed by NTUST GameLab, Taiwan, advised by Prof. Wen-Kai Tai. \nCopyright \u00A9 2017.";

		void Awake() {
			
		}
		
		void OnGUI() {
			//SampleStyle.DebugRect(SampleStyle.IndexWindowSplashCanvasArea, Color.blue);
			GUILayout.BeginArea(Container.IndexWindowSplashCanvasArea);
			EditorGUI.DrawPreviewTexture(Container.IndexWindowSplashCanvasArea, SampleStyle.SplashImage); 
			GUILayout.EndArea();
			//SampleStyle.DebugRect(SampleStyle.IndexWindowButtonsCanvasArea, Color.red);
			GUILayout.BeginArea(Container.IndexWindowButtonsCanvasArea);
			GUILayout.Space(SampleStyle.PaddingArea);
			GUILayout.BeginVertical("Box");
			GUILayout.BeginHorizontal();
<<<<<<< HEAD
			if (GUILayout.Button("Create Level", SampleStyle.ButtonLeft, SampleStyle.MainButtonHeight)) {
				// Create Level.
=======
			if (GUILayout.Button("Create Level", EditorStyles.miniButtonLeft, Style.TabButtonHeight)) {
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
>>>>>>> master
			}
			if (GUILayout.Button("Import Level", SampleStyle.ButtonMid, SampleStyle.MainButtonHeight)) {
				// Import Level.
			}
			if (GUILayout.Button("Import Rewrite", SampleStyle.ButtonMid, SampleStyle.MainButtonHeight)) {
				// Import Rewrite.
				string path = EditorUtility.OpenFilePanel("Import xml", "", "xml");
				if(path.Length > 0) {
					DungeonLevel.OperateXML.Unserialize.UnserializeFromXml(path);
				}
			}
			if (GUILayout.Button("Export Level", SampleStyle.ButtonRight, SampleStyle.MainButtonHeight)) {
				// Export Level.
				string path = EditorUtility.SaveFilePanel("Export xml", "", "Level.xml", "xml");
				if (path.Length > 0) {
					DungeonLevel.OperateXML.Serialize.SerializeToXml(path);
				}
			}
			GUILayout.EndHorizontal();
			GUILayout.Space(SampleStyle.PaddingBlock);
			// Show description.
			GUILayout.Label(_description, EditorStyles.wordWrappedLabel);
			GUILayout.EndVertical();
			GUILayout.EndArea();
		}
		void OnInspectorUpdate(){
			Repaint();	
		}
	}
}

using UnityEngine;
using UnityEditor;
using System.Collections;

// will remove.
using EditorStyle = EditorExtend.CommonStyle;
using SampleStyle = EditorExtend.SampleStyle;
using Container = EditorExtend.IndexWindow;
// Locales.
using Languages = LanguageManager;

namespace DungeonLevel {
	public class IndexWindow : EditorWindow {

		private static string _description;
		//= 
		//	"Dungeon Generation is a tool for procedurally generate a Game Level specifically for Dungeon. " +
		//	"This tool concerns about the player progression by using Mission & Space framework originally established by Joris Dormans. " +
		//	"Developed by NTUST GameLab, Taiwan, advised by Prof. Wen-Kai Tai. \nCopyright \u00A9 2017.";

		void Awake() {
			
		}

		void OnGUI() {
			Languages.Initialize();
			_description = Languages.GetText("Index-About");

			SampleStyle.DrawWindowBackground(SampleStyle.ColorGrey);
			GUILayout.BeginArea(Container.IndexWindowSplashCanvasArea);
			EditorGUI.DrawPreviewTexture(Container.IndexWindowSplashCanvasArea, SampleStyle.SplashImage); 
			GUILayout.EndArea();

			GUILayout.BeginArea(Container.IndexWindowButtonsCanvasArea, SampleStyle.Box);
			GUILayout.BeginVertical(SampleStyle.Frame(SampleStyle.ColorLightestGrey));
			GUILayout.Label(_description, EditorStyles.wordWrappedLabel);
			GUILayout.BeginHorizontal();
			if (GUILayout.Button(Languages.GetText("Index-CreateLevel"), SampleStyle.ButtonLeft, SampleStyle.MainButtonHeight)) {
				// Safety.
				if (EditorUtility.DisplayDialog("Create new level.",
					"Are you sure want to create a new level, this will overwrite the origin level?",
					"Yes", "No")) {
					// Initialization.
					LevelSettingsWindow.Initial();
					// Reset the windows of mission grammar system.
					MissionGrammarSystem.AlphabetWindow.Initialize();
					MissionGrammarSystem.RulesWindow.Initialize();
					GraphGeneration.MissionGraphWindow.Initialize();
					// Reset mission alphabet, mission grammar and rewrite system.
					MissionGrammarSystem.Alphabet.Initial();
					MissionGrammarSystem.MissionGrammar.Initial();
					MissionGrammarSystem.RewriteSystem.Initial(GraphGeneration.MissionGraphWindow.Seed);
				}
			}
			if (GUILayout.Button(Languages.GetText("Index-ImportLevel"), SampleStyle.ButtonMid, SampleStyle.MainButtonHeight)) {
				// Import Level.
			}
			if (GUILayout.Button(Languages.GetText("Index-ImportRewrite"), SampleStyle.ButtonMid, SampleStyle.MainButtonHeight)) {
				// Import Rewrite.
				string path = EditorUtility.OpenFilePanel("Import xml", "", "xml");
				if (path.Length > 0) {
					DungeonLevel.OperateXML.Unserialize.UnserializeFromXml(path);
					DungeonWindow.ShowMissionAlphabetWindow();
					DungeonWindow.ShowMissionRulesWindow();
					DungeonWindow.ShowGenerateMissionWindow();
				}
			}
			if (GUILayout.Button(Languages.GetText("Index-ExportLevel"), SampleStyle.ButtonRight, SampleStyle.MainButtonHeight)) {
				// Export Level.
				string path = EditorUtility.SaveFilePanel("Export xml", "", "Level.xml", "xml");
				if (path.Length > 0) {
					DungeonLevel.OperateXML.Serialize.SerializeToXml(path);
				}
			}
			GUILayout.EndHorizontal();
			GUILayout.EndVertical();
			GUILayout.EndArea();
		}
	}
}

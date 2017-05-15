using UnityEngine;
using UnityEditor;
using System.Collections;

using Mission    = MissionGrammarSystem;
using Generation = GraphGeneration;
using Dungeon    = DungeonLevel;

public class DungeonWindow : EditorWindow {
	// Add the 'Level settings' in 'Dungeon' menu.
	[MenuItem("Dungeon/Index", false, 1)]
	public static void ShowIndexWindow() {
		EditorWindow window = EditorWindow.GetWindow<Dungeon.IndexWindow>("Index", true);
		window.position = new Rect(35, 35, 500, 370);
	}

	// Add the 'Level settings' in 'Dungeon' menu.
	[MenuItem("Dungeon/Level settings", false, 1)]
	public static void ShowLevelSettingWindow() {
		EditorWindow window = EditorWindow.GetWindow<Dungeon.LevelSettingsWindow>("Level settings", true);
		window.position = new Rect(35, 60, 500, 210);
	}

	// Add the 'Mission > Alphabet' in 'Dungeon' menu.
	[MenuItem("Dungeon/Mission/Alphabet %#&ma", false, 11)]
	public static void ShowMissionAlphabetWindow() {
		bool hasOpened = Mission.AlphabetWindow.IsOpen;
		var window = EditorWindow.GetWindow<Mission.AlphabetWindow>("Mission alphabet", true) as Mission.AlphabetWindow;
		Mission.AlphabetWindow.Initialize();
		// Relocate the position of the window if it hasn't been opened.
		if (! hasOpened) {
			window.position = new Rect(35, 60, 500, 660);
		}
	}

	// Add the 'Mission > Rules' in 'Dungeon' menu.
	[MenuItem("Dungeon/Mission/Rules %#&mr", false, 12)]
	public static void ShowMissionRulesWindow() {
		bool hasOpened = Mission.RulesWindow.IsOpen;
		var window = EditorWindow.GetWindow<Mission.RulesWindow>("Mission rules", true) as Mission.RulesWindow;
		Mission.RulesWindow.Initialize();
		// Relocate the position of the window if it hasn't been opened.
		if (! hasOpened) {
			window.position = new Rect(35, 60, 750, 745);
		}
	}

	// Add the 'Generation > Mission' in 'Dungeon' menu.
	[MenuItem("Dungeon/Generation/Mission", false, 31)]
	public static void ShowGenerateMissionWindow() {
		bool hasOpened = Generation.MissionGraphWindow.IsOpen;
		var window = EditorWindow.GetWindow<Generation.MissionGraphWindow>("Generate mission graph", true) as Generation.MissionGraphWindow;
		// Relocate the position of the window if it hasn't been opened.
		if (! hasOpened) {
			window.position = new Rect(35, 60, 500, 620);
		}
	}

	// Add the 'Languages > English' in 'Dungeon' menu.
	[MenuItem("Dungeon/Languages/English", false, 51)]
	public static void LocaleEnglish() {
		Mgl.I18n.SetLocale("en-us");
	}

	// Add the 'Languages > Traditional Chinese' in 'Dungeon' menu.
	[MenuItem("Dungeon/Languages/Traditional Chinese", false, 52)]
	public static void LocaleTraditionalChinese() {
		Mgl.I18n.SetLocale("zh-tw");
	}
}

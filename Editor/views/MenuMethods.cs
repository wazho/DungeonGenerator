using UnityEngine;
using UnityEditor;
using System.Collections;

using Mission    = MissionGrammarSystem;
using Generation = GraphGeneration;
using Dungeon    = DungeonLevel;

public class DungeonWindow : EditorWindow {
	private static EditorWindow _window;
	private static Rect _defaultSize = new Rect(35, 35, 500, 650);

	// Add the 'Level settings' in 'Dungeon' menu.
	[MenuItem("Dungeon/Index", false, 1)]
	public static void ShowIndexWindow() {
		_window = EditorWindow.GetWindow<Dungeon.IndexWindow>("Index", true);
		_window.position = new Rect(35, 35, 500, 370);
	}

	// Add the 'Level settings' in 'Dungeon' menu.
	[MenuItem("Dungeon/Level settings", false, 1)]
	public static void ShowLevelSettingWindow() {
		_window = EditorWindow.GetWindow<Dungeon.LevelSettingsWindow>("Level settings", true);
		_window.position = new Rect(35, 60, 500, 210);
	}

	// Add the 'Mission > Alphabet' in 'Dungeon' menu.
	[MenuItem("Dungeon/Mission/Alphabet %#&ma", false, 11)]
	public static void ShowMissionAlphabetWindow() {
		_window = EditorWindow.GetWindow<Mission.AlphabetWindow>("Mission alphabet", true);
		_window.position = new Rect(35, 60, 500, 660);
	}

	// Add the 'Mission > Rules' in 'Dungeon' menu.
	[MenuItem("Dungeon/Mission/Rules %#&mr", false, 12)]
	public static void ShowMissionRulesWindow() {
		_window = EditorWindow.GetWindow<Mission.RulesWindow>("Mission rules", true);
		_window.position = new Rect(35, 60, 750, 785);
	}

	// Add the 'Generation > Mission' in 'Dungeon' menu.
	[MenuItem("Dungeon/Generation/Mission", false, 31)]
	public static void ShowGenerateMissionWindow() {
		_window = EditorWindow.GetWindow<Generation.MissionGraphWindow>("Generate mission graph", true);
		_window.position = new Rect(35, 60, 500, 640);
	}
}

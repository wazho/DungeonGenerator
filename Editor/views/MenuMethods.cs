using UnityEngine;
using UnityEditor;
using System.Collections;

using Mission    = MissionGrammarSystem;
using Space      = SpaceGrammarSystem;
using Generation = GraphGeneration;
using Dungeon    = DungeonLevel;

public class DungeonWindow : EditorWindow {
	private static EditorWindow _window;
	private static Rect _defaultSize = new Rect(35, 35, 500, 650);

	// Add the 'Level settings' in 'Dungeon' menu.
	[MenuItem("Dungeon/Index", false, 1)]
	public static void ShowIndexWindow() {
		_window = EditorWindow.GetWindow<Dungeon.IndexWindow>("Index", true);
		_window.position = new Rect(35, 35, 500, 400);
	}

	// Add the 'Level settings' in 'Dungeon' menu.
	[MenuItem("Dungeon/Level settings", false, 1)]
	public static void ShowLevelSettingWindow() {
		_window = EditorWindow.GetWindow<Dungeon.LevelSettingsWindow>("Level settings", true);
		_window.position = new Rect(35, 60, 500, 195);
	}

	// Add the 'Mission > Alphabet' in 'Dungeon' menu.
	[MenuItem("Dungeon/Mission/Alphabet %#&ma", false, 11)]
	public static void ShowMissionAlphabetWindow() {
		_window = EditorWindow.GetWindow<Mission.AlphabetWindow>("Mission alphabet", true);
		_window.position = new Rect(35, 60, 500, 615);
	}

	// Add the 'Mission > Rules' in 'Dungeon' menu.
	[MenuItem("Dungeon/Mission/Rules %#&mr", false, 12)]
	public static void ShowMissionRulesWindow() {
		_window = EditorWindow.GetWindow<Mission.RulesWindow>("Mission rules", true);
		_window.position = new Rect(35, 60, 750, 680);
	}

/*
	// Add the 'Space > Grammar settings' in 'Dungeon' menu.
	[MenuItem("Dungeon/Space/Grammar settings %#&sa", false, 21)]
	public static void ShowSpaceSettingWindow() {
		_window = EditorWindow.GetWindow<Space.SettingsWindow>("Space settings", true);
		_window.position = _defaultSize;
	}

	// Add the 'Space > Alphabet' in 'Dungeon' menu.
	[MenuItem("Dungeon/Space/Alphabet %#&sa", false, 22)]
	public static void ShowSpaceAlphabetWindow() {
		_window = EditorWindow.GetWindow<Space.AlphabetWindow>("Space alphabet", true);
		_window.position = new Rect(35, 60, 500, 615);
	}
	
	// Add the 'Space > Rules' in 'Dungeon' menu.
	[MenuItem("Dungeon/Space/Rules %#&sr", false, 23)]
	public static void ShowSpaceRulesWindow() {
		_window = EditorWindow.GetWindow<Space.RulesWindow>("Space rules", true);
		_window.position = _defaultSize;
	}
*/

	// Add the 'Generation > Mission' in 'Dungeon' menu.
	[MenuItem("Dungeon/Generation/Mission", false, 31)]
	public static void ShowGenerateMissionWindow() {
		_window = EditorWindow.GetWindow<Generation.MissionGraphWindow>("Generate mission graph", true);
		_window.position = _defaultSize;
	}
	
/*
	// Add the 'Generation > Space' in 'Dungeon' menu.
	[MenuItem("Dungeon/Generation/Space", false, 32)]
	public static void ShowGenerateSpaceWindow() {
		_window = EditorWindow.GetWindow<Generation.SpaceGraphWindow>("Generate space graph", true);
		_window.position = _defaultSize;
	}
*/

	// Add the 'SampleStyleWindow' in 'Dungeon' menu.
	[MenuItem("Dungeon/SampleStyle", false, 33)]
	public static void ShowSampleStyleWindow(){
		_window = EditorWindow.GetWindow<SampleStyleWindow>("Sample Style Window");
		_window.position = _defaultSize;
	}
}

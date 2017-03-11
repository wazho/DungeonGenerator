using UnityEngine;
using UnityEditor;
using System.Collections;

using Mission    = MissionGrammar;
using Space      = SpaceGrammar;
using Generation = GraphGeneration;

public class DungeonWindow : EditorWindow {
	private static EditorWindow _window;
	private static Rect _defaultSize = new Rect(35, 35, 500, 500);

	// Add the 'Mission > Grammar settings' in 'Dungeon' menu.
	[MenuItem("Dungeon/Mission/Grammar settings %#&ms", false, 1)]
	public static void ShowMissionSettingWindow() {
		_window = EditorWindow.GetWindow<Mission.SettingsWindow>("Mission settings", true);
		_window.position = _defaultSize;
	}

	// Add the 'Mission > Alphabet' in 'Dungeon' menu.
	[MenuItem("Dungeon/Mission/Alphabet %#&ma", false, 2)]
	public static void ShowMissionAlphabetWindow() {
		_window = EditorWindow.GetWindow<Mission.AlphabetWindow>("Mission alphabet", true);
		_window.position = _defaultSize;
	}

	// Add the 'Mission > Rules' in 'Dungeon' menu.
	[MenuItem("Dungeon/Mission/Rules %#&mr", false, 3)]
	public static void ShowMissionRulesWindow() {
		_window = EditorWindow.GetWindow<Mission.RulesWindow>("Mission rules", true);
		_window.position = _defaultSize;
	}

	// Add the 'Space > Grammar settings' in 'Dungeon' menu.
	[MenuItem("Dungeon/Space/Grammar settings %#&sa", false, 11)]
	public static void ShowSpaceSettingWindow() {
		_window = EditorWindow.GetWindow<Space.AlphabetWindow>("Space settings", true);
		_window.position = _defaultSize;
	}

	// Add the 'Space > Alphabet' in 'Dungeon' menu.
	[MenuItem("Dungeon/Space/Alphabet %#&sa", false, 12)]
	public static void ShowSpaceAlphabetWindow() {
		_window = EditorWindow.GetWindow<Space.AlphabetWindow>("Space alphabet", true);
		_window.position = _defaultSize;
	}
	
	// Add the 'Space > Rules' in 'Dungeon' menu.
	[MenuItem("Dungeon/Space/Rules %#&sr", false, 13)]
	public static void ShowSpaceRulesWindow() {
		_window = EditorWindow.GetWindow<Space.RulesWindow>("Space rules", true);
		_window.position = _defaultSize;
	}
	
	// Add the 'Generation > Mission' in 'Dungeon' menu.
	[MenuItem("Dungeon/Generation/Mission", false, 21)]
	public static void ShowGenerateMissionWindow() {
		_window = EditorWindow.GetWindow<Generation.MissionWindow>("Generate mission graph", true);
		_window.position = _defaultSize;
	}
	
	// Add the 'Generation > Space' in 'Dungeon' menu.
	[MenuItem("Dungeon/Generation/Space", false, 22)]
	public static void ShowGenerateSpaceWindow() {
		_window = EditorWindow.GetWindow<Generation.SpaceWindow>("Generate space graph", true);
		_window.position = _defaultSize;
	}

	// Example.
	[MenuItem("Dungeon/Example canvas", false, 100)]
	public static void ExampleCanvasWindow() {
		_window = EditorWindow.GetWindow<MissionAlphabetWindow>("Canvas", true);
		_window.position = _defaultSize;
	}
}

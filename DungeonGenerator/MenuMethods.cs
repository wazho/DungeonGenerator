using UnityEngine;
using UnityEditor;
using System.Collections;

public class DungeonWindow : EditorWindow
{
	// Add the 'Mission > Alphabet' in 'Dungeon' menu.
	[MenuItem("Dungeon/Mission/Alphabet %#&ma", false, 1)]
	public static void ShowMissionGrammarWindow() {
		EditorWindow window = GetWindow<MissionAlphabetWindow>(false, "Mission grammar", true);
		window.position = new Rect(0, 0, 500, 500);
		window.Show();
	}
	
	// Add the 'Mission > Rules' in 'Dungeon' menu.
	[MenuItem("Dungeon/Mission/Rules %#&mr", false, 2)]
	public static void ShowMissionRuleWindow() {
		EditorWindow.GetWindow<MissionRulesWindow>(false, "Mission rules", true);
	}


	// Add the 'Space > Rules' in 'Dungeon' menu.
	[MenuItem("Dungeon/Space/Alphabet %#&sa", false, 11)]
	public static void ShowSpaceGrammarWindow() {
		EditorWindow.GetWindow<SpaceAlphabetWindow>(false, "Mission grammar", true);
	}
	
	// Add the 'Space > Rules' in 'Dungeon' menu.
	[MenuItem("Dungeon/Space/Rules %#&sr", false, 12)]
	public static void ShowSpaceRulesWindow() {
		EditorWindow.GetWindow<SpaceRulesWindow>(false, "Space rules", true);
	}
}

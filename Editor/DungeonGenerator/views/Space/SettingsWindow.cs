using UnityEngine;
using UnityEditor;
using System.Collections;

using EditorAdvance = EditorExtend.Advance;
using EditorStyle = EditorExtend.Style;

namespace SpaceGrammar {
	public class SettingsWindow : EditorWindow {
		// [Edit later] change the string type with SpaceGrammar...
		// Types - Symbol & Connection
		private int _startingSymbolIndex;
		private int _defaultSymbolIndex;
		private int _connectingFloorIndex;
		private int _connectingWallIndex;
		private string[] _symbolTypes;
		private string[] _connectingFloorTypes;
		private string[] _connectingWallTypes;
		private string _startingSymbol;
		private string _defaultSymbol;
		private string _connectingFloor;
		private string _connectingWall;

		// Info
		private string _messageInfo;

		void Awake(){
			_startingSymbolIndex = 0;
			_defaultSymbolIndex = 1;
			_connectingFloorIndex = 0;
			_connectingWallIndex = 0;

			// [Edit later] Initialization 
			_symbolTypes = new string[]{ "Entrance", "Task" };
			_connectingFloorTypes = new string[]{ "Floor", "Basement" };
			_connectingWallTypes = new string[]{ "Wall", "Window" };
			_startingSymbol = _symbolTypes[_startingSymbolIndex];
			_defaultSymbol = _symbolTypes[_defaultSymbolIndex];
			_connectingFloor = _connectingFloorTypes[_connectingFloorIndex];
			_connectingWall = _connectingWallTypes[_connectingWallIndex];

			_messageInfo = "Info!\nNothing to show. ";
		}

		void OnGUI() {
			// Symbol Block
			GUILayout.Space(EditorStyle.PaddingAfterBlock);
			GUILayout.Label("Symbol");
			_startingSymbolIndex = EditorGUILayout.Popup("Starting Symbol", _startingSymbolIndex, _symbolTypes);
			_defaultSymbolIndex = EditorGUILayout.Popup("Default Symbol", _defaultSymbolIndex, _symbolTypes);

			// Connection Block
			GUILayout.Space (EditorStyle.PaddingAfterBlock);
			GUILayout.Label ("Connection");
			_connectingFloorIndex = EditorGUILayout.Popup("Connecting Floor", _connectingFloorIndex, _connectingFloorTypes);
			_connectingWallIndex = EditorGUILayout.Popup("Connecting Wall", _connectingWallIndex, _connectingWallTypes);

			// Info Block
			GUILayout.Space (EditorStyle.PaddingAfterBlock);
			EditorGUILayout.HelpBox(_messageInfo,MessageType.Info);
			if (GUILayout.Button("Apply", EditorStyles.miniButtonMid, EditorStyle.ButtonHeight)) {
				// [Edit later]
				_startingSymbol = _symbolTypes[_startingSymbolIndex];
				_defaultSymbol = _symbolTypes[_defaultSymbolIndex];
				_connectingFloor = _connectingFloorTypes[_connectingFloorIndex];
				_connectingWall = _connectingWallTypes[_connectingWallIndex];

				if (EditorUtility.DisplayDialog("Saving the Grammar Settings",
					   "Are you sure to save?", "Yes", "No")) {
					// [Edit later] Just to make sure we render to strings
					_messageInfo = "Info\n" + 
						_startingSymbol.ToString() + ", " + 
						_defaultSymbol.ToString() + ", " +
						_connectingFloor.ToString() + ", " +
						_connectingWall.ToString() + " have been saved. ";
				} else {
					_messageInfo = "Info\n" + 
						_startingSymbol.ToString() + ", " + 
						_defaultSymbol.ToString() + ", " +
						_connectingFloor.ToString() + ", " +
						_connectingWall.ToString() + " not yet saved. ";
				}
			}
		}
	}
}
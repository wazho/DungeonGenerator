using UnityEngine;
using UnityEditor;
using System.Collections;

using EditorAdvance = EditorExtend.Advance;
using EditorStyle = EditorExtend.Style;

namespace SpaceGrammar {

	//Enums
	public enum StartingSymbolType{
		Symbol1,
		Symbol2
	}
	public enum DefaultSymbolType{
		DefaultSymbol1,
		DefaultSymbol2
	}
	public enum ConnectingFloorType{
		Floor,
		Grass
	}
	public enum ConnectingWallType{
		Wall,
		Window, 
		Door
	}

	public class SettingsWindow : EditorWindow {

		//Types
		private StartingSymbolType _startingSymboltype;
		private DefaultSymbolType _defaultSymboltype;
		private ConnectingFloorType _connectingFloorType;
		private ConnectingWallType _connectingWallType;

		//Info
		private string _messageInfo;

		//[Remove soon]
		//asdasda

		void Awake(){
			_startingSymboltype = StartingSymbolType.Symbol1;
			_defaultSymboltype = DefaultSymbolType.DefaultSymbol1;
			_connectingFloorType = ConnectingFloorType.Floor;
			_connectingWallType = ConnectingWallType.Wall;
			_messageInfo = "Info!\nNothing to show. ";
		}

		void OnGUI() {
			//Symbol Block
			GUILayout.Space (EditorStyle.PaddingAfterBlock);
			GUILayout.Label ("Symbol");
			_startingSymboltype = (StartingSymbolType)EditorGUILayout.EnumPopup ("Starting Symbol", _startingSymboltype);
			_defaultSymboltype = (DefaultSymbolType)EditorGUILayout.EnumPopup ("Default Symbol", _defaultSymboltype);

			//Connection Block
			GUILayout.Space (EditorStyle.PaddingAfterBlock);
			GUILayout.Label ("Connection");
			_connectingFloorType = (ConnectingFloorType)EditorGUILayout.EnumPopup ("Connectiong Floor", _connectingFloorType);
			_connectingWallType = (ConnectingWallType)EditorGUILayout.EnumPopup ("Connecting Wall", _connectingWallType);

			//Info Block
			GUILayout.Space (EditorStyle.PaddingAfterBlock);
			EditorGUILayout.HelpBox (_messageInfo,MessageType.Info);
			if (GUILayout.Button ("Apply", EditorStyles.miniButtonMid, EditorStyle.ButtonHeight)) {
				if (EditorUtility.DisplayDialog ("Saving the Grammar Settings",
					   "Are you sure to save?",
					   "Yes", "No")) {
					_messageInfo = "Info\n" + 
						_startingSymboltype.ToString() + ", " + 
						_defaultSymboltype.ToString() + ", " +
						_connectingFloorType.ToString() + ", " +
						_connectingWallType.ToString() + " have been saved. ";
				} else {
					_messageInfo = "Info\n" + 
						_startingSymboltype.ToString() + ", " + 
						_defaultSymboltype.ToString() + ", " +
						_connectingFloorType.ToString() + ", " +
						_connectingWallType.ToString()  + " not yet saved! ";
				}
			}
		}
	}
}
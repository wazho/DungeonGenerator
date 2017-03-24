using UnityEngine;
using UnityEditor;
using System.Collections;

using EditorAdvance = EditorExtend.Advance;
using EditorStyle   = EditorExtend.Style;

namespace GraphGeneration {
	// Error type. 
	public enum ErrorType {
		None,
		Error
	}
	//select status
	public enum GraphState { 
		Mission,
		Space
	}

	public class MissionGraphWindow : EditorWindow {
		//Scroll view
		private Vector2 _scrollView;
		//error type & selected graph
		private ErrorType _errorType;
		private GraphState _graphState;

		void Awake() {
			_scrollView = new Vector2(0, 50);

			_errorType  = ErrorType.None;
			_graphState = GraphState.Mission;
		}

		void OnGUI() {
			//Mission and Space Graph button.
			GUILayout.BeginHorizontal();
			if (GUILayout.Button("Mission Graph",EditorStyles.miniButtonLeft, EditorStyle.SubmitButtonHeight)) {
				_graphState = GraphState.Mission;
			}
			if(GUILayout.Button("Mission Graph", EditorStyles.miniButtonRight, EditorStyle.SubmitButtonHeight)) {
				_graphState = GraphState.Space;
			}
			GUILayout.EndHorizontal();
			//[Temp]Canvas
			EditorGUILayout.TextArea("\n\t\t\tCanvas", GUILayout.Height(80));
			//HelpBox
			EditorGUILayout.HelpBox(FormValidation(), MessageType.Info, true);
			//Check boxies.
			_scrollView = EditorGUILayout.BeginScrollView(_scrollView,GUILayout.Height(150));
				foreach (MissionGrammarSystem.MissionGroup missionGroup in MissionGrammarSystem.MissionGrammar.Groups) {
					missionGroup.Selected = EditorGUILayout.Foldout(missionGroup.Selected, missionGroup.Name);
					if (missionGroup.Selected) { 
						foreach (MissionGrammarSystem.MissionRule missionRule in missionGroup.Rules) {
							missionRule.Enable = EditorGUILayout.Toggle(missionRule.Name,missionRule.Enable);
						}
					}
				}
			EditorGUILayout.EndScrollView();
			// If error occur, disable apply button.
			EditorGUI.BeginDisabledGroup(_errorType != ErrorType.None);
			//Mission and Space Graph button.
			GUILayout.BeginHorizontal();
			if (GUILayout.Button("Initial",EditorStyles.miniButtonLeft, EditorStyle.SubmitButtonHeight)) {
			}
			if(GUILayout.Button("Iterate", EditorStyles.miniButtonMid, EditorStyle.SubmitButtonHeight)) {
			}
			if(GUILayout.Button("Complete", EditorStyles.miniButtonRight, EditorStyle.SubmitButtonHeight)) {
			}
			GUILayout.EndHorizontal();
			// Apply button and popup
			if (GUILayout.Button("Save", EditorStyles.miniButton, EditorStyle.ButtonHeight)) {
				if (EditorUtility.DisplayDialog("Save",
					"Are you sure?",
					"Yes", "No")) {
					// Commit changes
					Debug.Log("Settings is changed :}");
				} else {
					// Cancel changes;
					Debug.Log("Settings isn't changed");
				}
			}
			EditorGUI.EndDisabledGroup();
		}
		// Form validation can determine the error type.
		string FormValidation() {
			/*
			 Validation.
			*/
			return ErrorMessage(_errorType);
		}
		// Return error message depends on error type.
		string ErrorMessage(ErrorType errorType) {
			string message = string.Empty;
			// Select the mapping message by error type.
			switch (errorType) {
			case ErrorType.None:
				message = "No error occur!";
				break;
			case ErrorType.Error:
				message = "Error occur!";
				break;
			}
			return message;
		}
	}
}
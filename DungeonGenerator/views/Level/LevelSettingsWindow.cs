using UnityEngine;
using UnityEditor;
using System.Collections;

using EditorAdvance = EditorExtend.Advance;
using EditorStyle   = EditorExtend.Style;

namespace DungeonLevel {
	// Error type. 
	public enum ErrorType {
		NoError,
		LevelNameIsUsed
	}

	public class LevelSettingsWindow : EditorWindow {
		// Name, Abbreviation, description, tag & ErrorType.
		private string _name;
		private string _abbreviation;
		private string _description;
		private string _tag;
		private ErrorType _errorType;

		void Awake() {
			// Setting properties.
			_name         = string.Empty;
			_abbreviation = string.Empty;
			_description  = string.Empty;
			_tag          = string.Empty;
			// Setting errors.
			_errorType    = ErrorType.NoError;
		}

		void OnGUI() {
			// Level information.
			_name         = EditorGUILayout.TextField("Level Name", _name, EditorStyle.TextFieldHeight);
			_abbreviation = EditorGUILayout.TextField("Level Abbreviation", _abbreviation, EditorStyle.TextFieldHeight);
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("Description");
			_description  = EditorGUILayout.TextArea(_description, EditorStyle.TextAreaHeight);
			EditorGUILayout.EndHorizontal();
			_tag          = EditorGUILayout.TextField("Tag", _tag);
			EditorGUILayout.HelpBox(FormValidation(), MessageType.Info, true);
			// If error occur, disable apply button.
			EditorGUI.BeginDisabledGroup(_errorType != ErrorType.NoError);
			// Apply button and popup
			if (GUILayout.Button("Apply", EditorStyles.miniButton, EditorStyle.SubmitButtonHeight)) {
				if (EditorUtility.DisplayDialog("Apply on level settings",
					"Are you sure want to save these level settings?",
					"Yes", "No")) {
					// Commit changes
					Debug.Log("Level Settings is changed :}");
				} else {
					// Cancel changes;
					Debug.Log("Level Settings isn't changed");
				}
			}
			EditorGUI.EndDisabledGroup();
		}
		// Form validation can determine the error type.
		string FormValidation() {
			// [TEST] if level name is used.
			if (_name == "test") {
				_errorType = ErrorType.LevelNameIsUsed;
			} else {
				_errorType = ErrorType.NoError;
			}
			return ErrorMessage(_errorType);
		}
		// Return error message depends on error type.
		string ErrorMessage(ErrorType errorType) {
			string message = string.Empty;
			// Select the mapping message by error type.
			switch (errorType) {
			case ErrorType.NoError:
				message = "No error occur!";
				break;
			case ErrorType.LevelNameIsUsed:
				message = "The Level's name has been used!";
				break;
			}
			return message;
		}
	}
}
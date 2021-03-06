using UnityEngine;
using UnityEditor;
using System.Collections;

// will remove.
using EditorStyle = EditorExtend.Style;
using Style = EditorExtend.CommonStyle;
using SampleStyle = EditorExtend.SampleStyle;
// Locales.
using Languages = LanguageManager;

namespace DungeonLevel {
	// Error type. 
	public enum ErrorType {
		NoError,
		LevelNameIsUsed
	}

	public class LevelSettingsWindow : EditorWindow {
		// Initialization
		public static void Initial() {
			// Setting properties.
			_name = string.Empty;
			_abbreviation = string.Empty;
			_description = string.Empty;
			_tag = string.Empty;
		}

		// Name, Abbreviation, description, tag & ErrorType.
		private static string _name;
		private static string _abbreviation;
		private static string _description;
		private static string _tag;

		private ErrorType     _errorType;

		public static string Name {
			get { return _name; }
		}
		public static string Abbreviation {
			get { return _abbreviation; }
		}
		public static string Description {
			get { return _description; }
		}
		public static string Tag {
			get { return _tag; }
		}
		private static string _messageNoError;
		private static string _messageLevelNameIsUsed;
		void Awake() {
			// Setting errors.
			_errorType = ErrorType.NoError;
		}

		void OnGUI() {

			SampleStyle.DrawWindowBackground(SampleStyle.ColorGrey);
			GUILayout.BeginVertical(SampleStyle.Frame(SampleStyle.ColorLightestGrey));
			_name         = SampleStyle.TextFieldLabeled(Languages.GetText("LevelSetting-LevelName"), _name, SampleStyle.TextFieldLabel, SampleStyle.TextField, SampleStyle.TextFieldHeight);
			_abbreviation = SampleStyle.TextFieldLabeled(Languages.GetText("LevelSetting-LevelAbbreviation"), _abbreviation , SampleStyle.TextFieldLabel, SampleStyle.TextField, SampleStyle.TextFieldHeight);
			_description  = SampleStyle.TextAreaLabeled(Languages.GetText("LevelSetting-Description"), _description, SampleStyle.TextAreaLabel, SampleStyle.TextArea, SampleStyle.TextAreaHeight);
			_tag          = SampleStyle.TextFieldLabeled(Languages.GetText("LevelSetting-Tag"), _tag, SampleStyle.TextFieldLabel, SampleStyle.TextField, SampleStyle.TextFieldHeight);

			EditorGUILayout.HelpBox(FormValidation(), MessageType.Info, true);
			// If error occur, disable apply button.
			EditorGUI.BeginDisabledGroup(_errorType != ErrorType.NoError);
			// Apply button and popup
			if (GUILayout.Button(Languages.GetText("LevelSetting-Apply"), SampleStyle.GetButtonStyle(SampleStyle.ButtonType.Regular, SampleStyle.ButtonColor.Green), SampleStyle.SubmitButtonHeight)) {
				if (EditorUtility.DisplayDialog(Languages.GetText("LevelSetting-Dialog-Title"),
					Languages.GetText("LevelSetting-Dialog-Content"),
					Languages.GetText("LevelSetting-Dialog-Yes"), Languages.GetText("LevelSetting-Dialog-No"))) {
					// Commit changes
					Debug.Log(Languages.GetText("LevelSetting-Dialog-Yes-Content"));
				} else {
					// Cancel changes;
					Debug.Log(Languages.GetText("LevelSetting-Dialog-No-Content"));
				}
			}
			EditorGUI.EndDisabledGroup();
			GUILayout.EndVertical();
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
				message = Languages.GetText("LevelSetting-NoError");
				break;
			case ErrorType.LevelNameIsUsed:
				message = Languages.GetText("LevelSetting-LevelNameIsUsed");
				break;
			}
			return message;
		}
	}
}
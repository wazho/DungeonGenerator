using UnityEngine;
using UnityEditor;
using System.Collections;

using EditorAdvance = EditorExtend.Advance;
using EditorStyle = EditorExtend.Style;

namespace GlobalSettings{
    //Enums 
    public enum ErrorType{
        NoError,
        LevelNameIsUsed
    }

    public class SettingsWindow : EditorWindow{
        //Name, Abbreviation, description, tag & ErrorType.
        private string _name;
        private string _abbreviation;
        private string _description;
        private string _tag;
        private ErrorType _errorType;
        //Help message.
        private string _messageInfoHint;
        //[TEST] test error detect
        private string _testLevelName;
        //Layout styles.
        private int _fontSize;
        private int _buttonSize;  

        //return error message depends on error type.
        string ErrorHints(ErrorType errorType){
            string message = "";

            switch (errorType){
            case ErrorType.NoError:
                message = "No error occur!";
                break;
            case ErrorType.LevelNameIsUsed:
                message = "The Level's name has been used!";
                break;
            }

            return message;
        }

        void Awake(){
            //Setting properties.
            _name = string.Empty;
            _abbreviation = string.Empty;
            _description = string.Empty;
            _tag = string.Empty;
            //Setting hints.
            _messageInfoHint = "Info\nStill Empty!";
            //Setting layout styles.
            _fontSize = 15;
            _buttonSize = 25;
            //Setting errors.
            _errorType = ErrorType.NoError;
            //[TEST]Set test string
            _testLevelName = "test";
        }

        void OnGUI(){
            //Level information
            _name = EditorGUILayout.TextField("Level Name", _name, GUILayout.Height(_fontSize));
            _abbreviation = EditorGUILayout.TextField("Level Abbreviation", _abbreviation, GUILayout.Height(_fontSize));
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Description");
            _description = EditorGUILayout.TextArea(_description, GUILayout.Height(_fontSize*5));
            EditorGUILayout.EndHorizontal();
            _tag = EditorGUILayout.TextField("Tag", _tag);

            EditorGUILayout.HelpBox(_messageInfoHint, MessageType.Info, true);

            //[TEST] if level name is used.
            if(_name == _testLevelName){
                _errorType = ErrorType.LevelNameIsUsed;
            }else{
                _errorType = ErrorType.NoError;
            }

            //set hint message.
            _messageInfoHint = ErrorHints(_errorType);

            //If error occur, disable apply button
            EditorGUI.BeginDisabledGroup(_errorType!=ErrorType.NoError);
            //Apply button and popup
            if (GUILayout.Button("Apply", EditorStyles.miniButton, EditorStyle.ButtonHeight,GUILayout.Height(_buttonSize))){
                if (EditorUtility.DisplayDialog("Apply on global settings",
                    "Are you sure want to save these global settings?",
                    "Yes", "No")){
                    //Commit changes
                    Debug.Log("Global Settings is changed :}");
                }else{
                    //Cancel changes;
                    Debug.Log("Global Settings isn't changed ");
                }
            }
            EditorGUI.EndDisabledGroup();

        }
    }
}
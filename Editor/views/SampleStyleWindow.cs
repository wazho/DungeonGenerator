using UnityEngine;
using UnityEditor;
using System.Collections;

using EditorAdvance = EditorExtend.Advance;
using EditorStyle   = EditorExtend.CommonStyle;
using SampleStyle = EditorExtend.SampleStyle;
using Container = EditorExtend.IndexWindow;

public class SampleStyleWindow : EditorWindow {
	private bool _isInitButtonTexture;

	public GUIStyle LeftButtonStyle;
	public GUIStyle MidButtonStyle;
	public GUIStyle RightButtonStyle;

	public enum SampleTab {
		Left,
		Mid,
		Right
	};
	private SampleTab _sampleTab;

	void Awake(){
		_isInitButtonTexture = true;
		_sampleTab = SampleTab.Left;
		LeftButtonStyle = new GUIStyle(SampleStyle.ButtonLeft);
		MidButtonStyle = new GUIStyle(SampleStyle.ButtonMid);
		RightButtonStyle = new GUIStyle(SampleStyle.ButtonRight);
		LeftButtonStyle.font = SampleStyle.TabElementFont;
		RightButtonStyle.font = SampleStyle.TabElementFont;
	}

	void OnGUI(){
		// Change this later
		EditorGUILayout.BeginHorizontal();
		if (GUILayout.Button("Left", LeftButtonStyle, GUILayout.Height(25))) {
			_sampleTab = SampleTab.Left;
		} else if (GUILayout.Button("Mid", MidButtonStyle, GUILayout.Height(25))) {
			_sampleTab = SampleTab.Mid;
		} else if (GUILayout.Button("Right", RightButtonStyle, GUILayout.Height(25))) {
			_sampleTab = SampleTab.Right;
		}
		EditorGUILayout.EndHorizontal();

		switch(_sampleTab){
		case SampleTab.Left:
			// or just simply use this? SampleStyle.MiniButtonLeftStyle.normal.background; 
			LeftButtonStyle.normal.background = SampleStyle.LeftActiveBlue;
			MidButtonStyle.normal.background = SampleStyle.MidNormalBlue;
			RightButtonStyle.normal.background = SampleStyle.RightNormalBlue;
			EditorStyle.HeaderOne.font = SampleStyle.TabElementFont;
			GUILayout.Label("Left", EditorStyle.HeaderOne);
			break;
		case SampleTab.Mid:
			LeftButtonStyle.normal.background = SampleStyle.LeftNormalBlue;
			MidButtonStyle.normal.background = SampleStyle.MidActiveBlue;
			RightButtonStyle.normal.background = SampleStyle.RightNormalBlue;
			GUILayout.Label("Middle", EditorStyle.HeaderTwo);
			break;
		case SampleTab.Right:
			LeftButtonStyle.normal.background = SampleStyle.LeftNormalBlue; 
			MidButtonStyle.normal.background = SampleStyle.MidNormalBlue; 
			RightButtonStyle.normal.background = SampleStyle.RightActiveBlue; 
			GUILayout.Label("Right", EditorStyle.HeaderOne);
			break;
		}
		GUILayout.Label("This is a content");
	}
	void OnInspectorUpdate(){
		Repaint();	
	}
}

using UnityEngine;
using UnityEditor;
using System.Collections;

using EditorAdvance = EditorExtend.Advance;
using EditorStyle   = EditorExtend.Style;
using SampleStyle = EditorExtend.SampleStyle;

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
		LeftButtonStyle = SampleStyle.MiniButtonLeftStyle; // new GUIStyle(EditorStyles.minibuttonleft);
		MidButtonStyle = SampleStyle.MiniButtonMidStyle;
		RightButtonStyle = SampleStyle.MiniButtonRightStyle;
	}

	void OnGUI(){
		EditorGUILayout.BeginHorizontal();
		if (GUILayout.Button("Left", LeftButtonStyle, EditorStyle.TabButtonHeight)) {
			_sampleTab = SampleTab.Left;
		} else if (GUILayout.Button("Mid", MidButtonStyle, EditorStyle.TabButtonHeight)) {
			_sampleTab = SampleTab.Mid;
		} else if (GUILayout.Button("Right", RightButtonStyle, EditorStyle.TabButtonHeight)) {
			_sampleTab = SampleTab.Right;
		}
		EditorGUILayout.EndHorizontal();

		SampleStyle.BeginLabelStyle(SampleStyle.LabelStyleType.Header);
		switch(_sampleTab){
		case SampleTab.Left:
			// or just simply use this? SampleStyle.MiniButtonLeftStyle.normal.background; 
			LeftButtonStyle.normal.background = SampleStyle.MiniButtonLeftActiveTexture;
			MidButtonStyle.normal.background = SampleStyle.MiniButtonMidNormalTexture;
			RightButtonStyle.normal.background = SampleStyle.MiniButtonRightNormalTexture;
			GUILayout.Label("Left");
			break;
		case SampleTab.Mid:
			LeftButtonStyle.normal.background = SampleStyle.MiniButtonLeftNormalTexture;
			MidButtonStyle.normal.background = SampleStyle.MiniButtonMidActiveTexture;
			RightButtonStyle.normal.background = SampleStyle.MiniButtonRightNormalTexture;
			GUILayout.Label("Middle");
			break;
		case SampleTab.Right:
			LeftButtonStyle.normal.background = SampleStyle.MiniButtonLeftNormalTexture; 
			MidButtonStyle.normal.background = SampleStyle.MiniButtonMidNormalTexture; 
			RightButtonStyle.normal.background = SampleStyle.MiniButtonRightActiveTexture; 
			GUILayout.Label("Right");
			break;
		}
		SampleStyle.RefreshLabelStyle();
		GUILayout.Label("This is a content");
	}
}

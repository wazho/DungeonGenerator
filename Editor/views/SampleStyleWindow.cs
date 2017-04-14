using UnityEngine;
using UnityEditor;
using System.Collections;

using EditorAdvance = EditorExtend.Advance;
using EditorStyle   = EditorExtend.CommonStyle;
using SampleStyle = EditorExtend.SampleStyle;
using Container = EditorExtend.IndexWindow;
//using SampleDrawing = EditorExtend.Drawing;

public class SampleStyleWindow : EditorWindow {
	private bool _isInitButtonTexture;

	public GUIStyle LeftButtonStyle;
	public GUIStyle MidButtonStyle;
	public GUIStyle RightButtonStyle;
	public int toolbarInt = 0;
	public string[] toolbarStrings = new string[] {"Toolbar1", "Toolbar2", "Toolbar3"};
	Vector2 ScrollPosition;

	public enum SampleTab {
		Left,
		Mid,
		Right
	};
	private SampleTab _sampleTab;

	void Awake(){
		ScrollPosition = Vector2.zero;
		_isInitButtonTexture = true;
		_sampleTab = SampleTab.Left;
		LeftButtonStyle = new GUIStyle(SampleStyle.ButtonLeft);
		MidButtonStyle = new GUIStyle(SampleStyle.ButtonMid);
		RightButtonStyle = new GUIStyle(SampleStyle.ButtonRight);
		LeftButtonStyle.font = SampleStyle.TabFont;
		RightButtonStyle.font = SampleStyle.TabFont;
		Debug.Log("Texture Size WH " + LeftButtonStyle.normal.background.width + "" + LeftButtonStyle.normal.background.height);
		Debug.Log("LFS: " + LeftButtonStyle.lineHeight + "," + LeftButtonStyle.fixedHeight+ "," + LeftButtonStyle.stretchHeight);
		Debug.Log("Border Size LRTB: " + LeftButtonStyle.border.left + "," + LeftButtonStyle.border.right + "," + LeftButtonStyle.border.top + "," + LeftButtonStyle.border.bottom);
		Debug.Log("Padding Size LRTB: " + LeftButtonStyle.padding.left + "," + LeftButtonStyle.padding.right + "," + LeftButtonStyle.padding.top + "," + LeftButtonStyle.padding.bottom);
	}

	void OnGUI(){
		SampleStyle.DrawWindowBackground(SampleStyle.ColorLightestGrey);
		ScrollPosition = GUILayout.BeginScrollView(ScrollPosition, GUILayout.Height(500));
		SampleStyle.DrawGrid(SampleStyle.GetRectOfWindow(), SampleStyle.MinorGridSize, SampleStyle.MajorGridSize, new Color(166f/255f, 226f/255f, 255f/255f), new Color(118f / 255f, 208f / 255f, 255f / 255f));

		//toolbarInt = GUILayout.Toolbar(toolbarInt, toolbarStrings, SampleStyle.Button,SampleStyle.MainButtonHeight);

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
			EditorStyle.HeaderOne.font = SampleStyle.TabFont;
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
		Rect r = new Rect(10, 10, 100, 100);
		/*
		EditorGUI.DrawRect(r, new Color(30f/255f,167f/255, 225f/255f));
		GUILayout.BeginArea(r);
		GUILayout.Button("Click me");
		GUILayout.Button("Or me");
		GUILayout.EndArea();*/

		//Color defaultColor = GUI.color;
		//GUI.contentColor = new Color(30f / 255f, 167f / 255, 225f / 255f);
		//GUI.color
		if (GUILayout.Button("mybutton", SampleStyle.Button, GUILayout.Height(25))) {
		
		}
		if (GUILayout.Button("BUTTON", SampleStyle.ButtonLeft, GUILayout.Height(25))) {

		}

		GUILayout.BeginArea(new Rect(0, 250,Screen.width,50));
		EditorGUI.DrawRect(new Rect(0, 0, Screen.width, 10), SampleStyle.ColorDarkestBlue);
		EditorGUI.DrawRect(new Rect(0, 10, Screen.width, 10), SampleStyle.ColorDarkBlue);
		EditorGUI.DrawRect(new Rect(0, 20, Screen.width, 10), SampleStyle.ColorBlue);
		EditorGUI.DrawRect(new Rect(0, 30, Screen.width, 10), SampleStyle.ColorLightBlue);
		EditorGUI.DrawRect(new Rect(0, 40, Screen.width, 10), SampleStyle.ColorLightestBlue);
		GUILayout.Label("Blue", EditorStyle.HeaderOne); 
		GUILayout.EndArea();
		GUILayout.BeginArea(new Rect(0, 300,Screen.width,50));
		EditorGUI.DrawRect(new Rect(0, 0, Screen.width, 10), SampleStyle.ColorDarkestGreen);
		EditorGUI.DrawRect(new Rect(0, 10, Screen.width, 10), SampleStyle.ColorDarkGreen);
		EditorGUI.DrawRect(new Rect(0, 20, Screen.width, 10), SampleStyle.ColorGreen);
		EditorGUI.DrawRect(new Rect(0, 30, Screen.width, 10), SampleStyle.ColorLightGreen);
		EditorGUI.DrawRect(new Rect(0, 40, Screen.width, 10), SampleStyle.ColorLightestGreen);
		GUILayout.Label("Green", EditorStyle.HeaderOne); 
		GUILayout.EndArea();
		GUILayout.BeginArea(new Rect(0, 350,Screen.width,50));
		EditorGUI.DrawRect(new Rect(0, 0, Screen.width, 10), SampleStyle.ColorDarkestGrey);
		EditorGUI.DrawRect(new Rect(0, 10, Screen.width, 10), SampleStyle.ColorDarkGrey);
		EditorGUI.DrawRect(new Rect(0, 20, Screen.width, 10), SampleStyle.ColorGrey);
		EditorGUI.DrawRect(new Rect(0, 30, Screen.width, 10), SampleStyle.ColorLightGrey);
		EditorGUI.DrawRect(new Rect(0, 40, Screen.width, 10), SampleStyle.ColorLightestGrey);
		GUILayout.Label("Grey", EditorStyle.HeaderOne); 
		GUILayout.EndArea();
		GUILayout.BeginArea(new Rect(0, 400,Screen.width,50));
		EditorGUI.DrawRect(new Rect(0, 0, Screen.width, 10), SampleStyle.ColorDarkestOrange);
		EditorGUI.DrawRect(new Rect(0, 10, Screen.width, 10), SampleStyle.ColorDarkOrange);
		EditorGUI.DrawRect(new Rect(0, 20, Screen.width, 10), SampleStyle.ColorOrange);
		EditorGUI.DrawRect(new Rect(0, 30, Screen.width, 10), SampleStyle.ColorLightOrange);
		EditorGUI.DrawRect(new Rect(0, 40, Screen.width, 10), SampleStyle.ColorLightestOrange);
		GUILayout.Label("Orange", EditorStyle.HeaderOne); 
		GUILayout.EndArea();
		GUILayout.BeginArea(new Rect(0, 450,Screen.width,50));
		EditorGUI.DrawRect(new Rect(0, 0, Screen.width, 10), SampleStyle.ColorDarkestYellow);
		EditorGUI.DrawRect(new Rect(0, 10, Screen.width, 10), SampleStyle.ColorDarkYellow);
		EditorGUI.DrawRect(new Rect(0, 20, Screen.width, 10), SampleStyle.ColorYellow);
		EditorGUI.DrawRect(new Rect(0, 30, Screen.width, 10), SampleStyle.ColorLightYellow);
		EditorGUI.DrawRect(new Rect(0, 40, Screen.width, 10), SampleStyle.ColorLightestYellow);
		GUILayout.Label("Yellow", EditorStyle.HeaderOne); 
		GUILayout.EndArea();
		//GUI.color = defaultColor;
		for (int i = 0; i < 5;i++) {
			GUILayout.Button("mybutton", SampleStyle.Button, GUILayout.Height(100), GUILayout.Width(50));	
		}

		GUILayout.EndScrollView();
		Drawing.DrawCircle (new Vector2 (200f, 200f), 100, Color.black, 5,true, 36, 1.0f);
		Drawing.DrawLine(Vector2.zero, new Vector2(400,400), Color.red, 5, true, 1.0f);
		Drawing.DrawBezierLine (Vector2.zero, new Vector2 (400, 150), new Vector2 (600, 800), new Vector2 (200, 500), Color.blue, 10, true, 24, 2.0f);
		//Drawing.DrawCircle(new Vector2(Screen.width / 2, Screen.height / 2), Screen.width / 4, SampleStyle.ColorLightOrange, 5, false, 21, 1.0f);
		//SampleDrawing.DrawCircle(new Vector2(Screen.width / 2, Screen.height / 2), Screen.width / 4, SampleStyle.ColorLightOrange, 5, true, 21, 1.0f);
	}
	void OnInspectorUpdate(){
		Repaint();	
	}
}

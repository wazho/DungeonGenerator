using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Linq;
// Stylesheet.
using Style     = EditorExtend.CommonStyle;
using Container = EditorExtend.GenerationWindow;
// Models.
using Mission = MissionGrammarSystem;

namespace GraphGeneration {
	// Error type. 
	public enum ErrorType {
		None,
		Error
	}
	// Select status.
	public enum GraphState { 
		Mission,
		Space
	}

	public class MissionGraphWindow : EditorWindow {
		// Initialize.
		public static void Initial() {
			_currentGraph = new Mission.GraphGrammar();
		}
		// Scroll view.
		private Vector2 _scrollView;
		// error type & selected graph
		private ErrorType  _errorType;
		private GraphState _graphState;
		// Starting index
		private int _startingNodeIndex;
		private int _tempStartingNodeIndex;
		private string[] _nodeNames = Mission.Alphabet.Nodes.Select(n => n.ExpressName).ToArray();
		// Canvas.
		private Vector2 _canvasScrollPosition;
		private int _missionGraphCanvasSizeWidth;
		private int _missionGraphCanvasSizeHeight;
		private static Mission.GraphGrammar _currentGraph = new Mission.GraphGrammar();

		void Awake() {
			_scrollView   = new Vector2(0, 50);
			_errorType    = ErrorType.None;
			_graphState   = GraphState.Mission;
			_currentGraph = new Mission.GraphGrammar();
			_startingNodeIndex = Mission.Alphabet.Nodes.FindIndex(x => x == Mission.Alphabet.StartingNode);
			_nodeNames = Mission.Alphabet.Nodes.Select(n => n.ExpressName).ToArray();
		}
		// If Alphabet updated then update too.
		void OnFocus() {
			_startingNodeIndex = Mission.Alphabet.Nodes.FindIndex(x => x == Mission.Alphabet.StartingNode);
			_nodeNames = Mission.Alphabet.Nodes.Select(n => n.ExpressName).ToArray();
		}
		void OnGUI() {
			// Buttons for switching mission graph and space graph.
			LayoutStateButtons();
			_startingNodeIndex = EditorGUILayout.Popup("Starting Node", _startingNodeIndex, _nodeNames);
			if(_startingNodeIndex != _tempStartingNodeIndex) {
				_tempStartingNodeIndex = _startingNodeIndex;
				Mission.Alphabet.StartingNode = Mission.Alphabet.Nodes[_startingNodeIndex];
			}
			// Canvas to draw current mission graph.
			LayoutMissionGraphCanvas();
			// Layout the list of mission group.
			LayoutMissionGroupList();
			// Buttons for operating the graph.
			LayoutFunctionButtons();
		}

		// Buttons for switching mission graph and space graph.
		private void LayoutStateButtons() {
			GUILayout.BeginHorizontal();
			if (GUILayout.Button("Mission Graph",EditorStyles.miniButtonLeft, Style.TabButtonHeight)) {
				_graphState = GraphState.Mission;
			}
			if (GUILayout.Button("Space Graph", EditorStyles.miniButtonRight, Style.TabButtonHeight)) {
				_graphState = GraphState.Space;
			}
			GUILayout.EndHorizontal();
		}
		// Canvas to draw current mission graph.
		private void LayoutMissionGraphCanvas() {
			GUILayout.BeginArea(Container.MissionGraphArea);
			_canvasScrollPosition = GUILayout.BeginScrollView(_canvasScrollPosition, GUILayout.Width(Screen.width), GUILayout.Height(300));
			Container.ResizeMissionGraphCanvas(_missionGraphCanvasSizeWidth, _missionGraphCanvasSizeHeight);
			EditorGUI.DrawRect(Container.MissionGraphCanvas, Color.gray);
			GUILayout.Label(string.Empty, Container.MissionGraphCanvasContent);
			// Connection 
			foreach (Mission.GraphGrammarConnection connection in _currentGraph.Connections) {
				connection.Draw();
			}
			// Node
			// Draw and get right bottom position.
			Vector2 positionRightBotton = new Vector2(0,0);
			foreach (Mission.GraphGrammarNode node in _currentGraph.Nodes) {
				// Get right position
				if (node.PositionX > positionRightBotton.x) {
					positionRightBotton.x = node.PositionX;
				}
				// Get bottom position
				if (node.PositionY > positionRightBotton.y) {
					positionRightBotton.y = node.PositionY;
				}
				node.Draw();
			}
			// Compare with screen size.
			_missionGraphCanvasSizeWidth  = Mathf.Max((int) Container.MissionGraphArea.width, (int) positionRightBotton.x + 25);
			_missionGraphCanvasSizeHeight = Mathf.Max((int) Container.MissionGraphArea.height, (int) positionRightBotton.y + 25);
			GUILayout.EndScrollView();
			GUILayout.EndArea();
		}
		// Layout the list of mission group.
		private void LayoutMissionGroupList() {
			GUILayout.BeginArea(new Rect(0, 360, Screen.width, Screen.height));
			// HelpBox
			EditorGUILayout.HelpBox(FormValidation(), MessageType.Info, true);
			// Check boxies.
			_scrollView = EditorGUILayout.BeginScrollView(_scrollView,GUILayout.Height(150));
			foreach (Mission.MissionGroup missionGroup in Mission.MissionGrammar.Groups) {
				missionGroup.Selected = EditorGUILayout.Foldout(missionGroup.Selected, missionGroup.Name);
				if (missionGroup.Selected) { 
					foreach (Mission.MissionRule missionRule in missionGroup.Rules) {
						missionRule.Enable = EditorGUILayout.Toggle(missionRule.Name,missionRule.Enable);
					}
				}
			}
			EditorGUILayout.EndScrollView();
			GUILayout.EndArea();
		}
		// Buttons for operating the graph.
		private void LayoutFunctionButtons() {
			GUILayout.BeginArea(new Rect(0, 560, Screen.width, Screen.height));
			// If error occur, disable apply button.
			EditorGUI.BeginDisabledGroup(_errorType != ErrorType.None);
			// Mission and Space Graph button.
			GUILayout.BeginHorizontal();
			if (GUILayout.Button("Initial",EditorStyles.miniButtonLeft, Style.ButtonHeight)) {
				// Rewrite system initialization.
				Mission.RewriteSystem.Initial();
				// Update the current graph.
				_currentGraph = Mission.RewriteSystem.TransformFromGraph();
			}
			if (GUILayout.Button("Iterate", EditorStyles.miniButtonMid, Style.ButtonHeight)) {
				// Rewrite system iteration.
				Mission.RewriteSystem.Iterate();
				// Update the current graph.
				_currentGraph = Mission.RewriteSystem.TransformFromGraph();
			}
			if (GUILayout.Button("Complete", EditorStyles.miniButtonRight, Style.ButtonHeight)) {

			}
			GUILayout.EndHorizontal();
			// Apply button and popup
			if (GUILayout.Button("Save", EditorStyles.miniButton, Style.SubmitButtonHeight)) {
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
			GUILayout.EndArea();
		}
		// Form validation can determine the error type.
		private string FormValidation() {
			/*
			 Validation.
			*/
			return ErrorMessage(_errorType);
		}
		// Return error message depends on error type.
		private string ErrorMessage(ErrorType errorType) {
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

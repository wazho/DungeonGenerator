using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Linq;
// Stylesheet.
using Container   = EditorExtend.GenerationWindow;
using SampleStyle = EditorExtend.SampleStyle;
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
		// Buttons 
		private GUIStyle MissionTabButtonStyle;
		private GUIStyle SpaceTabButtonStyle;
		private bool _isInitTabButton;

		private bool _isRuleChanged;

		void Awake() {
			Initialize();
		}

		public void Initialize() {
			_scrollView        = new Vector2(0, 60);
			_errorType         = ErrorType.None;
			_graphState        = GraphState.Mission;
			_currentGraph      = new Mission.GraphGrammar();
			_startingNodeIndex = Mission.Alphabet.Nodes.FindIndex(x => x == Mission.Alphabet.StartingNode);
			_nodeNames         = Mission.Alphabet.Nodes.Select(n => n.ExpressName).ToArray();
			_isInitTabButton   = true;
			_isRuleChanged     = false;
		}

		void OnGUI() {
			if (_isInitTabButton) {
				MissionTabButtonStyle = new GUIStyle(SampleStyle.GetButtonStyle(SampleStyle.ButtonType.Left, SampleStyle.ButtonColor.Blue));
				SpaceTabButtonStyle   = new GUIStyle(SampleStyle.GetButtonStyle(SampleStyle.ButtonType.Right, SampleStyle.ButtonColor.Blue));
				_isInitTabButton      = false;
			}
			SampleStyle.DrawWindowBackground(SampleStyle.ColorGrey);
			// Buttons for switching mission graph and space graph.
			GUILayout.BeginVertical(SampleStyle.Frame(SampleStyle.ColorLightestGrey));
			LayoutStateButtons();
			_startingNodeIndex = SampleStyle.PopupLabeled("Starting Node", _startingNodeIndex, _nodeNames, SampleStyle.PopUpLabel, SampleStyle.PopUp, Screen.width - 15);
			if(_startingNodeIndex != _tempStartingNodeIndex) {
				_tempStartingNodeIndex = _startingNodeIndex;
				Mission.Alphabet.StartingNode = Mission.Alphabet.Nodes[_startingNodeIndex];
			}
			GUILayout.EndVertical();

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
			if (GUILayout.Toggle(_graphState == GraphState.Mission, "Mission Graph", MissionTabButtonStyle, SampleStyle.TabButtonHeight)) {
				_graphState = GraphState.Mission;
			}
			if (GUILayout.Toggle(_graphState == GraphState.Space, "Space Graph", SpaceTabButtonStyle, SampleStyle.TabButtonHeight)) {
				_graphState = GraphState.Space;
			}
			GUILayout.EndHorizontal();
		}
		// Canvas to draw current mission graph.
		private void LayoutMissionGraphCanvas() {
			GUILayout.BeginArea(Container.MissionGraphArea);
			GUILayout.BeginVertical(SampleStyle.Frame(SampleStyle.ColorLightestGrey));
			_canvasScrollPosition = GUILayout.BeginScrollView(_canvasScrollPosition, GUILayout.Width(Screen.width-15), GUILayout.Height(300 - 5));
			Container.ResizeMissionGraphCanvas(_missionGraphCanvasSizeWidth, _missionGraphCanvasSizeHeight);
			SampleStyle.DrawGrid(Container.MissionGraphCanvas, SampleStyle.MinorGridSize, SampleStyle.MajorGridSize, SampleStyle.GridBackgroundColor, SampleStyle.GridColor);
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
			GUILayout.EndVertical();
			GUILayout.EndArea();
		}
		// Layout the list of mission group.
		private bool tempGroupEnable;
		private bool tempRuleEnable;
		private void LayoutMissionGroupList() {
			GUILayout.BeginArea(Container.MissionGroupListArea);
			GUILayout.BeginVertical(SampleStyle.Frame(SampleStyle.ColorLightestGrey));
			// HelpBox
			EditorGUILayout.HelpBox(FormValidation(), MessageType.Info, true);
			// Check boxies.
			_scrollView = EditorGUILayout.BeginScrollView(_scrollView,GUILayout.Height(150));
			int indentLevel = EditorGUI.indentLevel;
			foreach (Mission.MissionGroup missionGroup in Mission.MissionGrammar.Groups) {
				EditorGUI.indentLevel = 0;
				GUILayout.BeginHorizontal();
				missionGroup.Selected = EditorGUILayout.Foldout(missionGroup.Selected, missionGroup.Name, SampleStyle.FoldoutLabel);
				tempGroupEnable = missionGroup.AllEnable;
				missionGroup.AllEnable = EditorGUILayout.Toggle("", missionGroup.AllEnable);
				if (tempGroupEnable != missionGroup.AllEnable) {
					_isRuleChanged = true;
					foreach (Mission.MissionRule missionRule in missionGroup.Rules) {
						missionRule.Enable = missionGroup.AllEnable;
					}
				}
				missionGroup.AllEnable = true;
				foreach (Mission.MissionRule missionRule in missionGroup.Rules) {
					if (missionGroup.AllEnable && !missionRule.Enable) {
						missionGroup.AllEnable = false;
					}
				}
				GUILayout.EndHorizontal();
				if (missionGroup.Selected) { 
					foreach (Mission.MissionRule missionRule in missionGroup.Rules) {
						EditorGUI.indentLevel = 2;
						tempRuleEnable = missionRule.Enable;
						missionRule.Enable = EditorGUILayout.Toggle(missionRule.Name, missionRule.Enable);
						if (tempRuleEnable != missionRule.Enable) {
							_isRuleChanged = true;
						}

						if (missionGroup.AllEnable && !missionRule.Enable) {
							missionGroup.AllEnable = false;
						}
					}
				}
			}
			EditorGUILayout.EndScrollView();
			GUILayout.EndVertical();
			GUILayout.EndArea();

		}
		// Buttons for operating the graph.
		private void LayoutFunctionButtons() {
			GUILayout.BeginArea(Container.FunctionButtonsArea);
			GUILayout.BeginVertical(SampleStyle.Frame(SampleStyle.ColorLightestGrey));
			// If error occur, disable apply button.
			EditorGUI.BeginDisabledGroup(_errorType != ErrorType.None);
			// Mission and Space Graph button.
			GUILayout.BeginHorizontal();
			if (GUILayout.Button("Initial", SampleStyle.GetButtonStyle(SampleStyle.ButtonType.Left, SampleStyle.ButtonColor.Blue), SampleStyle.ButtonHeight)) {
				// Rewrite system initialization.
				Mission.RewriteSystem.Initial();
				_isRuleChanged = false;
				// Update the current graph.
				_currentGraph = Mission.RewriteSystem.TransformFromGraph();
			}

			EditorGUI.BeginDisabledGroup(_isRuleChanged);
			if (GUILayout.Button("Iterate", SampleStyle.GetButtonStyle(SampleStyle.ButtonType.Mid, SampleStyle.ButtonColor.Blue), SampleStyle.ButtonHeight)) {
				// Rewrite system iteration.
				Mission.RewriteSystem.Iterate();
				// Update the current graph.
				_currentGraph = Mission.RewriteSystem.TransformFromGraph();
			}
			if (GUILayout.Button("Complete", SampleStyle.GetButtonStyle(SampleStyle.ButtonType.Right, SampleStyle.ButtonColor.Blue), SampleStyle.ButtonHeight)) {

			}
			EditorGUI.EndDisabledGroup();
			GUILayout.EndHorizontal();
			GUILayout.Space(SampleStyle.PaddingBlock);
			// Apply button and popup
			if (GUILayout.Button("Save", SampleStyle.GetButtonStyle(SampleStyle.ButtonType.Regular, SampleStyle.ButtonColor.Green), SampleStyle.SubmitButtonHeight)) {
				if (EditorUtility.DisplayDialog("Save",
					"Are you sure?",
					"Yes", "No")) {
					// Setting root node for CreVoxAttach.
					Mission.CreVoxAttach.SetCreVoxNodeRoot(_currentGraph.Nodes[0]);
					// Commit changes
					Debug.Log("Settings are changed :}");
				} else {
					// Cancel changes;
					Debug.Log("Settings aren't changed");
				}
			}
			EditorGUI.EndDisabledGroup();
			GUILayout.EndVertical();
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

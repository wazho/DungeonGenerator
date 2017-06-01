using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Linq;
// Stylesheet.
using Container   = EditorExtend.GenerationWindow;
using SampleStyle = EditorExtend.SampleStyle;
// Models.
using Mission = MissionGrammarSystem;
// Locales.
using Languages = LanguageManager;

namespace GraphGeneration {
	// The mission graph window.
	[InitializeOnLoad]
	public class MissionGraphWindow : EditorWindow {
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
		// Window self.
		public static MissionGraphWindow Instance { get; private set; }
		public static bool IsOpen { get { return Instance != null; } }
		// Scroll view.
		private static Vector2 _scrollView;
		// error type & selected graph
		private static ErrorType  _errorType;
		private static GraphState _graphState;
		// Canvas.
		private static Vector2 _canvasScrollPosition;
		private static int _missionGraphCanvasSizeWidth;
		private static int _missionGraphCanvasSizeHeight;
		private static Mission.GraphGrammar _currentGraph = new Mission.GraphGrammar();
		// Buttons 
		private static GUIStyle MissionTabButtonStyle;
		private static GUIStyle SpaceTabButtonStyle;
		private static bool _isInitTabButton;
		private static bool _isRuleChanged;
		// Random seed.
		public static int Seed { get; private set; }

		// Initialize when trigger reload scripts via 'InitializeOnLoad'.
		static MissionGraphWindow() {
			Initialize();
		}
		// Initialize when open the editor window.
		void Awake() {
			Initialize();
		}
		void OnEnable() {
			Instance = this;
		}
		void OnDisable() {
			Instance = null;
		}

		public static void Initialize() {
			_scrollView      = new Vector2(0, 60);
			_errorType       = ErrorType.None;
			_graphState      = GraphState.Mission;
			_currentGraph    = new Mission.GraphGrammar();
			_isInitTabButton = true;
			_isRuleChanged   = true;
			Seed             = 0;
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
			// Dropdown for strating node.
			int startingIndex = Mission.Alphabet.Nodes.FindIndex(n => n == Mission.Alphabet.StartingNode);
			startingIndex = SampleStyle.PopupLabeled(Languages.GetText("GenerateMission-StartingNode"), startingIndex, Mission.Alphabet.Nodes.Select(n => n.ExpressName).ToArray(), SampleStyle.PopUpLabel, SampleStyle.PopUp, Screen.width - 15);
			Mission.Alphabet.StartingNode = Mission.Alphabet.Nodes[startingIndex];
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
			if (GUILayout.Toggle(_graphState == GraphState.Mission, Languages.GetText("GenerateMission-MissionGraph"), MissionTabButtonStyle, SampleStyle.TabButtonHeight)) {
				_graphState = GraphState.Mission;
			}
			if (GUILayout.Toggle(_graphState == GraphState.Space, Languages.GetText("GenerateMission-SpaceGraph"), SpaceTabButtonStyle, SampleStyle.TabButtonHeight)) {
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
						missionRule.Enable = missionGroup.AllEnable & missionRule.Valid;
					}
				}
				missionGroup.AllEnable = true;
				foreach (Mission.MissionRule missionRule in missionGroup.Rules) {
					if (missionRule.Valid && missionGroup.AllEnable && !missionRule.Enable) {
						missionGroup.AllEnable = false;
					}
				}
				GUILayout.EndHorizontal();
				if (missionGroup.Selected) { 
					foreach (Mission.MissionRule missionRule in missionGroup.Rules) {
						EditorGUI.indentLevel = 2;
						tempRuleEnable = missionRule.Enable;
						GUILayout.BeginHorizontal();
						// If not legal disable button.
						EditorGUI.BeginDisabledGroup(!missionRule.Valid);
						missionRule.Enable = EditorGUILayout.Toggle(missionRule.Name, missionRule.Enable);
						EditorGUI.EndDisabledGroup();
						// Hint user this rule is disable because it's illegal.
						if (!missionRule.Valid) {
							EditorGUILayout.LabelField(Languages.GetText("GenerateMission-IllegalRule"));
						}
						GUILayout.EndHorizontal();
						if (tempRuleEnable != missionRule.Enable) {
							_isRuleChanged = true;
						}

						if (missionRule.Valid && missionGroup.AllEnable && !missionRule.Enable) {
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
			GUILayout.BeginHorizontal();
			// Random seed.
			Seed = SampleStyle.IntFieldLabeled(Languages.GetText("GenerateMission-Seed"), Seed, SampleStyle.IntFieldLabel, SampleStyle.IntField, SampleStyle.IntFieldHeight);
			if (GUILayout.Button(Languages.GetText("GenerateMission-Random"), SampleStyle.GetButtonStyle(SampleStyle.ButtonType.Regular, SampleStyle.ButtonColor.Blue), SampleStyle.ButtonHeight)) {
				Seed = Random.Range(1, 1000000);
				// Unfocus from the field.
				GUI.FocusControl("FocusToNothing");
			}
			GUILayout.EndHorizontal();
			// If error occur, disable apply button.
			EditorGUI.BeginDisabledGroup(_errorType != ErrorType.None);
			// Mission and Space Graph button.
			GUILayout.BeginHorizontal();
			if (GUILayout.Button(Languages.GetText("GenerateMission-Initial"), SampleStyle.GetButtonStyle(SampleStyle.ButtonType.Left, SampleStyle.ButtonColor.Blue), SampleStyle.ButtonHeight)) {
				// Rewrite system initialization.
				Mission.RewriteSystem.Initial(Seed);
				_isRuleChanged = false;
				// Update the current graph.
				_currentGraph = Mission.RewriteSystem.TransformFromGraph();
				// Setting root node for CreVoxAttach.
				Mission.CreVoxAttach.SetCreVoxNodeRoot(_currentGraph.Nodes[0]);
			}
			EditorGUI.BeginDisabledGroup(_isRuleChanged);
			if (GUILayout.Button(Languages.GetText("GenerateMission-Iterate"), SampleStyle.GetButtonStyle(SampleStyle.ButtonType.Mid, SampleStyle.ButtonColor.Blue), SampleStyle.ButtonHeight)) {
				// Rewrite system iteration.
				Mission.RewriteSystem.Iterate();
				// Update the current graph.
				_currentGraph = Mission.RewriteSystem.TransformFromGraph();
				// Setting root node for CreVoxAttach.
				Mission.CreVoxAttach.SetCreVoxNodeRoot(_currentGraph.Nodes[0]);
			}
			if (GUILayout.Button(Languages.GetText("GenerateMission-Complete"), SampleStyle.GetButtonStyle(SampleStyle.ButtonType.Right, SampleStyle.ButtonColor.Blue), SampleStyle.ButtonHeight)) {
				var stopWatch = System.Diagnostics.Stopwatch.StartNew();
				// Iterate until finish.
				while (
					(
						// Still exist non-terminal nodes.
						_currentGraph.Nodes.Exists(n => n.Terminal == Mission.NodeTerminalType.NonTerminal)
						// Have to exhauste all rules that set minimum.
						|| Mission.RewriteSystem.Rules.Sum(r => r.QuantityLimitMin) > 0
					) 
					// Time limit is 3,000 ms.
					&& stopWatch.ElapsedMilliseconds <= 3000
				) {
					// Rewrite system iteration.
					Mission.RewriteSystem.Iterate();
					// Update the current graph.
					_currentGraph = Mission.RewriteSystem.TransformFromGraph();
				}
				stopWatch.Stop();
				// Setting root node for CreVoxAttach.
				Mission.CreVoxAttach.SetCreVoxNodeRoot(_currentGraph.Nodes[0]);
			}
			EditorGUI.EndDisabledGroup();
			GUILayout.EndHorizontal();
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
				message = Languages.GetText("MissionGraph-NoError");
				break;
			case ErrorType.Error:
				message = Languages.GetText("MissionGraph-Error");
				break;
			}
			return message;
		}
	}
}

using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections;
using System.Text.RegularExpressions;
// Stylesheet.
using Style      = EditorExtend.CommonStyle;
using Container  = EditorExtend.MissionAlphabetWindow;
using SymbolList = EditorExtend.SymbolList;

namespace MissionGrammarSystem {
	// The mission alphabet window.
	public class AlphabetWindow : EditorWindow {
		// Tabs of this window.
		public enum AlphabetWindowTab {
			Nodes,
			Connections,
		}
		// Types of the symbol.
		public enum EditingMode {
			None,
			Create,
			Modify,
			Delete,
		}
		// The mode of buttons.
		private AlphabetWindowTab _currentTab;
		private EditingMode       _editingMode;
		// The scroll bar of list.
		private Vector2 _scrollPosition;
		// Message of helpbox of submition.
		private string      _messageHint;
		private MessageType _messageType;
		// Node or connection in prview canvas.
		private GraphGrammarNode       _node       = new GraphGrammarNode(NodeTerminalType.Terminal);
		private GraphGrammarConnection _connection = new GraphGrammarConnection();
		// The description of nodes and connections.
		private string _symbolName;
		private string _symbolAbbreviation;
		private string _symbolDescription;
		// Symbol colors.
		private Color _symbolOutlineColor;
		private Color _symbolFilledColor;
		private Color _symbolTextColor;
		// The exclusive types.
		private NodeTerminalType    _symbolTerminal;
		private ConnectionType      _connectionType;
		private ConnectionArrowType _connectionArrowType;
		// The drawing canvas.
		private Rect    _symbolListCanvas;
		private Rect    _symbolListCanvasInWindow;
		private Vector2 _centerPosition;

		// Native function for Editor Window. Trigger via opening the window.
		void Awake() {
			// Initial whole fields in window.
			InitFields();
			// Set the first values.
			_currentTab               = AlphabetWindowTab.Nodes;
			_editingMode              = EditingMode.None;
			_scrollPosition           = Vector2.zero;
			_messageHint              = string.Empty;
			_messageType              = MessageType.Info;
			_node                     = new GraphGrammarNode(NodeTerminalType.Terminal);
			_connection               = new GraphGrammarConnection();
			_symbolListCanvas         = new Rect(0, 0, Screen.width, Screen.height);
			_symbolListCanvasInWindow = _symbolListCanvas;
			_centerPosition           = new Vector2(Screen.width / 2, 75);
			_connectionType           = ConnectionType.WeakRequirement;
			_connectionArrowType      = ConnectionArrowType.Normal;
			// Revoke all.
			Alphabet.RevokeAllSelected();
		}
		// Initial whole fields in window.
		void InitFields() {
			_symbolName         = string.Empty;
			_symbolAbbreviation = string.Empty;
			_symbolDescription  = string.Empty;
			_symbolOutlineColor = Color.black;
			_symbolFilledColor  = Color.white;
			_symbolTextColor    = Color.black;
			_symbolTerminal     = NodeTerminalType.Terminal;
			// Unfocus from the field.
			GUI.FocusControl("FocusToNothing");
		}
		// Native function for Editor Window.
		void OnGUI() {
			// Buttons - Nodes or Connections.
			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button("Nodes", EditorStyles.miniButtonLeft, Style.TabButtonHeight)) {
				_editingMode = EditingMode.None;
				_currentTab  = AlphabetWindowTab.Nodes;
			}
			if (GUILayout.Button("Connections", EditorStyles.miniButtonRight, Style.TabButtonHeight)) {
				_editingMode = EditingMode.None;
				_currentTab  = AlphabetWindowTab.Connections;
			}
			EditorGUILayout.EndHorizontal();
			// Toggle for nodes interface and connection interface.
			switch (_currentTab) {
			case AlphabetWindowTab.Nodes:
				// Header.
				GUILayout.Label("List of Nodes", Style.HeaderOne, Style.HeaderOneHeightLayout);
				// Content of nodes.
				LayoutNodesInterface();
				break;
			case AlphabetWindowTab.Connections:
				// Header.
				GUILayout.Label("List of Connections", Style.HeaderOne, Style.HeaderOneHeightLayout);
				// Content of connections.
				LayoutConnectionsInterface();
				break;
			}
			// Event controller.
			EventController();
		}
		// Content of nodes.
		void LayoutNodesInterface() {
			// Show the canvas, that is the list of nodes.
			LayoutSymbolList();
			// Buttons for switching editing mode.
			LayoutEditingModeButtonGroup();
			// Canvas for preview symbol.
			GUILayout.BeginArea(Container.SymbolPreviewArea);
			EditorGUI.DrawRect(Container.SymbolPreviewCanvas, Color.gray);
			_centerPosition.x = Screen.width / 2;
			_node.Position    = _centerPosition;
			_node.Draw();
			GUILayout.EndArea();
			// Area for input fields.
			switch (_editingMode) {
			case EditingMode.Create:
			case EditingMode.Modify:
				// Content of property.
				GUILayout.BeginArea(Container.SymbolPropertiesArea);
				EditorGUILayout.BeginVertical();
				GUILayout.Space(Style.PaddingSpace);
				// Information of node.
				_symbolTerminal     = (NodeTerminalType) EditorGUILayout.EnumPopup("Symbol Type", _symbolTerminal);
				_symbolName         = EditorGUILayout.TextField("Name", _symbolName);
				_symbolAbbreviation = EditorGUILayout.TextField("Abbreviation", _symbolAbbreviation);
				_symbolDescription  = EditorGUILayout.TextField("Description", _symbolDescription);
				_symbolOutlineColor = EditorGUILayout.ColorField("Outline Color", _symbolOutlineColor);
				_symbolFilledColor  = EditorGUILayout.ColorField("Filled Color", _symbolFilledColor);
				_symbolTextColor    = EditorGUILayout.ColorField("Text Color", _symbolTextColor);
				// Update the node.
				UpdateNode(_node);
				EditorGUILayout.EndVertical();
				GUILayout.Space(Style.PaddingSpace);
				// Show content of submition.
				LayoutSubmitionHint();
				LayoutSubmitionButton();
				GUILayout.EndArea();
				break;
			}
		}
		// Content of connections.
		void LayoutConnectionsInterface() {
			// Show the canvas, that is the list of nodes.
			LayoutSymbolList();
			// Buttons for switching editing mode.
			LayoutEditingModeButtonGroup();
			// Canvas for preview symbol.
			GUILayout.BeginArea(Container.SymbolPreviewArea);
			EditorGUI.DrawRect(Container.SymbolPreviewCanvas, Color.gray);
			// [TODO] This part (value assign) is temporary.
			_centerPosition.x         = Screen.width / 2 - 25;
			_connection.StartPosition = _centerPosition;
			_centerPosition.x         = Screen.width / 2 + 25;
			_connection.EndPosition   = _centerPosition;
			// Draw this connection.
			_connection.Draw();
			GUILayout.EndArea();
			switch (_editingMode) {
			case EditingMode.Create:
			case EditingMode.Modify:
				// Content of property.
				GUILayout.BeginArea(Container.SymbolPropertiesArea);
				EditorGUILayout.BeginVertical();
				GUILayout.Space(Style.PaddingSpace);
				// Information of connection.
				_symbolName          = EditorGUILayout.TextField("Name", _symbolName);
				_symbolDescription   = EditorGUILayout.TextField("Description", _symbolDescription);
				_symbolOutlineColor  = EditorGUILayout.ColorField("Outline Color", _symbolOutlineColor);
				_connectionType      = (ConnectionType) EditorGUILayout.EnumPopup("Connection Type", _connectionType);
				_connectionArrowType = (ConnectionArrowType) EditorGUILayout.EnumPopup("Arrow Type", _connectionArrowType);
				// Update the conntection.
				UpdateConnection(_connection);
				EditorGUILayout.EndVertical();
				GUILayout.Space(Style.PaddingSpace);
				// Show content of submition.
				LayoutSubmitionHint();
				LayoutSubmitionButton();
				GUILayout.EndArea();
				break;
			}
		}
		// Symbol list in node tab and connection tab.
		void LayoutSymbolList() {
			// Set the scroll position.
			_scrollPosition = GUILayout.BeginScrollView(_scrollPosition, SymbolList.HeightLayout);
			// Content of scroll area.
			GUILayout.BeginArea(Container.SymbolListArea);
			_symbolListCanvas = Container.SymbolListCanvas;
			EditorGUI.DrawRect(_symbolListCanvas, Color.gray);
			GUILayout.EndArea();
			// Layout each symbols in list.
			switch (_currentTab) {
			case AlphabetWindowTab.Nodes:
				foreach (var node in Alphabet.Nodes) {
					Alphabet.DrawNodeInList(node);
					// Custom style to modify padding and margin for label.
					GUILayout.Label(node.ExpressName, SymbolList.NodeElement);
				}
				break;
			case AlphabetWindowTab.Connections:
				foreach (var connection in Alphabet.Connections) {
					Alphabet.DrawConnectionInList(connection);
					// Custom style to modify padding and margin for label.
					GUILayout.Label(connection.Name, SymbolList.ConnectionElement);
				}
				break;
			}
			GUILayout.EndScrollView();
			// Get the Rect object from the last control when the event is Repaint.
			if (Event.current.type == EventType.Repaint) {
				_symbolListCanvasInWindow = GUILayoutUtility.GetLastRect();
			}
		}
		// Buttons about adding new symbol, modifying and deleting.
		void LayoutEditingModeButtonGroup() {
			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button("Add New", EditorStyles.miniButtonLeft, Style.ButtonHeight)) {
				// Switch the mode.
				_editingMode = EditingMode.Create;
				// Initial the preview node and connection.
				_node       = new GraphGrammarNode();
				_connection = new GraphGrammarConnection();
				// Initial all fields and repaint.
				Alphabet.RevokeAllSelected();
				InitFields();
				Repaint();
			}
			switch (_currentTab) {
			case AlphabetWindowTab.Nodes:
				EditorGUI.BeginDisabledGroup(Alphabet.SelectedNode == null);
				break;
			case AlphabetWindowTab.Connections:
				EditorGUI.BeginDisabledGroup(Alphabet.SelectedConnection == null);
				break;
			}
			if (GUILayout.Button("Modify", EditorStyles.miniButtonMid, Style.ButtonHeight)) {
				// Switch the mode.
				_editingMode = EditingMode.Modify;
			}
			if (GUILayout.Button("Delete", EditorStyles.miniButtonRight, Style.ButtonHeight)) {
				// Switch the mode.
				_editingMode = EditingMode.Delete;
				// Remove the node or connection from alphabet and repaint.
				switch (_currentTab) {
				case AlphabetWindowTab.Nodes:
					Alphabet.RemoveNode(Alphabet.SelectedNode);
					break;
				case AlphabetWindowTab.Connections:
					Alphabet.RemoveConnection(Alphabet.SelectedConnection);
					break;
				}
				Repaint();
			}
			EditorGUI.EndDisabledGroup();
			EditorGUILayout.EndHorizontal();
		}
		// Refresh the fields when select any symbol.
		void UpdateFields(GraphGrammarNode node) {
			_symbolTerminal     = node.Terminal;
			_symbolName         = node.Name;
			_symbolAbbreviation = node.Abbreviation;
			_symbolDescription  = node.Description;
			_symbolOutlineColor = node.OutlineColor;
			_symbolFilledColor  = node.FilledColor;
			_symbolTextColor    = node.TextColor;
			// Repaint the window.
			Repaint();
		}
		void UpdateFields(GraphGrammarConnection connection) {
			_symbolName          = connection.Name;
			_symbolDescription   = connection.Description;
			_symbolOutlineColor  = connection.OutlineColor;
			_connectionType      = connection.Requirement;
			_connectionArrowType = connection.Arrow;
			// Repaint the window.
			Repaint();
		}
		// Update the node information from the current field values.
		void UpdateNode(GraphGrammarNode node) {
			if (node == null) { return; }
			node.Terminal     = _symbolTerminal;
			node.Name         = _symbolName;
			node.Abbreviation = _symbolAbbreviation;
			node.Description  = _symbolDescription;
			node.OutlineColor = _symbolOutlineColor;
			node.FilledColor  = _symbolFilledColor;
			node.TextColor    = _symbolTextColor;
			// Repaint the window.
			Repaint();
		}
		// Update the connection information from the current field values.
		void UpdateConnection(GraphGrammarConnection connection) {
			if (connection == null) { return; }
			connection.Name         = _symbolName;
			connection.Description  = _symbolDescription;
			connection.OutlineColor = _symbolOutlineColor;
			connection.Requirement  = _connectionType;
			connection.Arrow        = _connectionArrowType;
			// Repaint the window.
			Repaint();
		}
		// Validate that the field data is legal.
		private static Regex _ruleOfTerminalSymbolName            = new Regex(@"^[a-z]{1}[a-zA-Z]{,19}$");
		private static Regex _ruleOfTerminalSymbolAbbreviation    = new Regex(@"^[a-z]{1,4}$");
		private static Regex _ruleOfNonTerminalSymbolName         = new Regex(@"^[A-Z]{1}[a-zA-Z]{,19}$");
		private static Regex _ruleOfNonTerminalSymbolAbbreviation = new Regex(@"^[A-Z]{1,4}$");
		void NodeFieldValidation() {
			if (_symbolName == string.Empty ||
				_symbolAbbreviation == string.Empty ||
				_symbolDescription == string.Empty) {
				_messageHint = "Please fill every column.";
				_messageType = MessageType.Warning;
			} else if (_symbolTerminal == NodeTerminalType.Terminal &&
				! _ruleOfTerminalSymbolName.IsMatch(_symbolName)) {
				_messageHint = "Name field Error! \nPlease use only letters (a-z,A-Z) under 20 characters and the first letter is lowercase.";
				_messageType = MessageType.Error;
			} else if (_symbolTerminal == NodeTerminalType.Terminal &&
				! _ruleOfTerminalSymbolAbbreviation.IsMatch(_symbolAbbreviation)) {
				_messageHint = "Abbreviation field error! \nPlease use only lowercase letters (a-z) and 4 characters or less.";
				_messageType = MessageType.Error;
			} else if (_symbolTerminal == NodeTerminalType.NonTerminal &&
				! _ruleOfNonTerminalSymbolName.IsMatch(_symbolName)) {
				_messageHint = "Name field Error! \nPlease use only letters (a-z,A-Z) under 20 characters and the first letter is uppercase.";
				_messageType = MessageType.Error;
			} else if (_symbolTerminal == NodeTerminalType.NonTerminal &&
				! _ruleOfNonTerminalSymbolAbbreviation.IsMatch(_symbolAbbreviation)) {
				_messageHint = "Abbreviation field error! \nPlease use only uppercase letters (A-Z) and 4 characters or less.";
				_messageType = MessageType.Error;
			} else if (Alphabet.IsNodeNameUsed(_node)) {
				_messageHint = "Node name has been used!\nPlease try another one.";
				_messageType = MessageType.Error;
			} else if (Alphabet.IsNodeAbbreviationUsed(_node)) {
				_messageHint = "Node abbreviation has been used!\nPlease try another one.";
				_messageType = MessageType.Error;
			} else {
				_messageHint = "The data has changed, but still not save it.";
				_messageType = MessageType.Info;
			}
		}
		// Validate that the field data is legal.
		private static Regex _ruleOfConnectionName         = new Regex(@"^[a-z]{1}[a-zA-Z]{,19}$");
		private static Regex _ruleOfConnectionAbbreviation = new Regex(@"^[a-z]{1,4}$");
		void ConnectionFieldValidation() {
			if (_symbolName == string.Empty ||
				_symbolDescription == string.Empty) {
				_messageHint = "Please fill every column.";
				_messageType = MessageType.Warning;
			} else if (! _ruleOfConnectionName.IsMatch(_symbolName)) {
				_messageHint = "Name field Error! \nPlease use only letters (a-z,A-Z) under 20 characters and the first letter is lowercase.";
				_messageType = MessageType.Error;
			} else if (Alphabet.IsConnectionNameUsed(_connection)) {
				_messageHint = "Node name has been used!\nPlease try another one.";
				_messageType = MessageType.Error;
			} else {
				_messageHint = "The data has changed, but still not save it.";
				_messageType = MessageType.Info;
			}
		}
		// Hint message about the form fields.
		void LayoutSubmitionHint() {
			switch (_currentTab) {
			case AlphabetWindowTab.Nodes:
				switch (_editingMode) {
				case EditingMode.Create:
					NodeFieldValidation();
					break;
				case EditingMode.Modify:
					if (Alphabet.SelectedNode != null && Alphabet.SelectedNode.Terminal == _node.Terminal &&
						Alphabet.SelectedNode.Name == _node.Name &&
						Alphabet.SelectedNode.Abbreviation == _node.Abbreviation &&
						Alphabet.SelectedNode.Description == _node.Description) {
						_messageHint = "The data is up to date.";
						_messageType = MessageType.Info;
					} else {
						NodeFieldValidation();
					}
					break;
				}
				break;
			case AlphabetWindowTab.Connections:
					switch (_editingMode) {
				case EditingMode.Create:
						ConnectionFieldValidation();
					break;
				case EditingMode.Modify:
					if (Alphabet.SelectedConnection != null && Alphabet.SelectedConnection.Arrow == _connection.Arrow &&
						Alphabet.SelectedConnection.Requirement == _connection.Requirement &&
						Alphabet.SelectedConnection.Name == _connection.Name &&
						Alphabet.SelectedConnection.Description == _connection.Description) {
						_messageHint = "The data is up to date.";
						_messageType = MessageType.Info;
					} else {
						ConnectionFieldValidation();
					}
					break;
				}
				break;
			}
			EditorGUILayout.HelpBox(_messageHint, _messageType);
		}
		// Content of submition.
		void LayoutSubmitionButton() {
			// When click apply button.
			switch (_editingMode) {
			case EditingMode.Create:
				GUI.enabled = (_messageType != MessageType.Error && _messageType != MessageType.Warning);
				if (! GUILayout.Button("Add this symbol into alphabet", EditorStyles.miniButton, Style.ButtonHeight)) { break; }
				// When click the button, revoke all selected symbols and add the symbon in list.
				switch (_currentTab) {
				case AlphabetWindowTab.Nodes:
					Alphabet.RevokeAllSelected();
					Alphabet.AddNode(new GraphGrammarNode(_node));
					Alphabet.Nodes.Last().Selected = true;
					break;
				case AlphabetWindowTab.Connections:
					Alphabet.RevokeAllSelected();
					Alphabet.AddConnection(new GraphGrammarConnection(_connection));
					Alphabet.Connections.Last().Selected = true;
					break;
				}
				// Make the scroll position in list to bottom, and switch to modify mode.
				_scrollPosition.y = Mathf.Infinity;
				_editingMode      = EditingMode.Modify;
				// Unfocus from the field.
				GUI.FocusControl("FocusToNothing");
				GUI.enabled = true;
				break;
			case EditingMode.Modify:
				GUI.enabled = (_messageType != MessageType.Error && _messageType != MessageType.Warning);
				if (! GUILayout.Button("Update the changed", EditorStyles.miniButton, Style.ButtonHeight)) { break; }
				// When click the button, update the symbol informations.
				switch (_currentTab) {
				case AlphabetWindowTab.Nodes:
					// Update in alphabet and mission grammar.
					UpdateNode(Alphabet.SelectedNode);
					MissionGrammar.OnAlphabetUpdated(Alphabet.SelectedNode);
					break;
				case AlphabetWindowTab.Connections:
					// Update in alphabet and mission grammar.
					UpdateConnection(Alphabet.SelectedConnection);
					MissionGrammar.OnAlphabetUpdated(Alphabet.SelectedConnection);
					break;
				}
				// Unfocus from the field.
				GUI.FocusControl("FocusToNothing");
				GUI.enabled = true;
				break;
			}
		}
		// Control whole events.
		void EventController() {
			if (Event.current.type == EventType.MouseDown) {
				OnClickedElementInList(Event.current.mousePosition.y - _symbolListCanvasInWindow.y);
			}
		}
		// [TODO] Temporary. The click event listener for list canvas.
		void OnClickedElementInList(float y) {
			if (y > 0 && y < SymbolList.Height) {
				// [TODO] This is temporary to assign value, will promote it soon.
				int index = (int) (y + _scrollPosition.y) / 50;
				Alphabet.RevokeAllSelected();
				switch (_currentTab) {
				case AlphabetWindowTab.Nodes:
					if (index < Alphabet.Nodes.Count) {
						Alphabet.Nodes[index].Selected = true;
						UpdateFields(Alphabet.Nodes[index]);
					} else {
						// Initial the fields.
						InitFields();
					}
					break;
				case AlphabetWindowTab.Connections:
					if (index < Alphabet.Connections.Count) {
						Alphabet.Connections[index].Selected = true;
						UpdateFields(Alphabet.Connections[index]);
					} else {
						// Initial the fields.
						InitFields();
					}
					break;
				}
				// Switch to the normal mode.
				_editingMode = EditingMode.None;
			}
		}
	}
}

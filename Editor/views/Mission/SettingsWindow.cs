using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections;

using EditorStyle = EditorExtend.Style;

namespace MissionGrammarSystem {
	public class SettingsWindow : EditorWindow {
		// Node's name.
		private string[] _nodeNames;
		// Types
		private static int _startingNodeIndex;
		private static int _defaultNodeIndex;
		private static NodeTerminalType _symbolTerminal;
		private static ConnectionType _connectionType;

		private int _tempStartingNodeIndex;
		private int _tempDefaultNodeIndex;
		private NodeTerminalType _tempSymbolTerminal;
		private ConnectionType _tempConnectionType;

		private GraphGrammarNode _startingNode;
		private GraphGrammarNode _defaultNode;
		// Enabled
		private bool _applyButtonEnabled;
		// Info
		private string _messageInfo;

		void Awake() {  
			_nodeNames             = Alphabet.Nodes.Select(n => n.ExpressName).ToArray();
			_symbolTerminal        = NodeTerminalType.Terminal;
			_connectionType        = ConnectionType.WeakRequirement;
			_applyButtonEnabled    = false;
			_messageInfo           = "Nothing changed.";
			_tempStartingNodeIndex = Alphabet.Nodes.FindIndex(n => n.AlphabetID == Alphabet.StartingNode.AlphabetID);
			_tempDefaultNodeIndex  = _defaultNodeIndex;
		}

		void OnGUI() {
			// Setting Block
			GUILayout.Space(EditorStyle.PaddingAfterBlock);
			_tempStartingNodeIndex = EditorGUILayout.Popup("Starting Node", _tempStartingNodeIndex, _nodeNames);
			_tempDefaultNodeIndex  = EditorGUILayout.Popup("Default Node", _tempDefaultNodeIndex, _nodeNames);
			_tempSymbolTerminal    = (NodeTerminalType) EditorGUILayout.EnumPopup("Default Symbol", _tempSymbolTerminal);
			_tempConnectionType    = (ConnectionType)   EditorGUILayout.EnumPopup("Default Connection", _tempConnectionType);

			// Info Block.
			GUILayout.Space(EditorStyle.PaddingAfterBlock);
			EditorGUILayout.HelpBox(_messageInfo, MessageType.Info);

			//if the data has changed
			if (_defaultNodeIndex != _tempDefaultNodeIndex ||
				_startingNodeIndex != _tempStartingNodeIndex ||
				_symbolTerminal != _tempSymbolTerminal ||
				_connectionType != _tempConnectionType) {

				_applyButtonEnabled = true;
			}
			LayoutSubmitionButton();
		}
		// When user close window.
		void OnDestroy() {
			LayoutSubmitionButton();
		}
		// Content of submition.
		void LayoutSubmitionButton() {
			GUI.enabled = _applyButtonEnabled;
			if (GUILayout.Button("Apply", EditorStyles.miniButton, EditorStyle.ButtonHeight)) {
				//git nodes info
				_startingNode = Alphabet.Nodes[_startingNodeIndex];
				_defaultNode = Alphabet.Nodes[_defaultNodeIndex];
				if (EditorUtility.DisplayDialog("Saving the Grammar Settings",
						"Are you sure to save?",
						"Yes", "No")) {
					_messageInfo = "Info\n" +
						_startingNode.Name.ToString() + ", " +
						_defaultNode.Name.ToString() + ", " +
						_symbolTerminal.ToString() + ", " +
						_connectionType.ToString() + " have been saved. ";
					// Apply changes.
					_startingNodeIndex = _tempStartingNodeIndex;
					_defaultNodeIndex  = _tempDefaultNodeIndex;
					_symbolTerminal    = _tempSymbolTerminal;
					_connectionType    = _tempConnectionType;
					UpdateSettings();
					_applyButtonEnabled = false;
				} else {
					_messageInfo = "Info\n" +
						_startingNode.Name.ToString() + ", " +
						_defaultNode.Name.ToString() + ", " +
						_symbolTerminal.ToString() + ", " +
						_connectionType.ToString() + " not yet saved! ";
				}
			}
			GUI.enabled = true;
		}
		void UpdateSettings() {
			Alphabet.StartingNode          = Alphabet.Nodes[_startingNodeIndex];
			Alphabet.DefaultNode           = Alphabet.Nodes[_defaultNodeIndex];
			Alphabet.DefaultConnectionType = _connectionType;
		}
	}
}
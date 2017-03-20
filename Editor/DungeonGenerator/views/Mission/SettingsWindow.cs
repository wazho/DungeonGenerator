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
		private int _startingNodeIndex;
		private int _defaultNodeIndex;
		private GraphGrammarNode _startingNode;
		private GraphGrammarNode _defaultNode;
		private NodeTerminalType _symbolTerminal;
		private ConnectionType   _connectionType;
		// Enabled
		private bool _applyButtonEnabled;
		// Info
		private string _messageInfo;

		void Awake() {  
			_nodeNames = Alphabet.Nodes.Select(n => n.ExpressName).ToArray();
			_startingNodeIndex  = 0;
			_defaultNodeIndex   = 0;
			_symbolTerminal     = NodeTerminalType.Terminal;
			_connectionType     = ConnectionType.WeakRequirement;
			_applyButtonEnabled = true;
			if (_nodeNames.Length > 0) {
				_messageInfo = "Info\nNothing to show.";
			} else {
				_applyButtonEnabled = false;
				_messageInfo = "Info\nYou must have a node at least.";
			}
		}

		void OnGUI() {
			// Setting Block
			GUILayout.Space(EditorStyle.PaddingAfterBlock);
			_startingNodeIndex = EditorGUILayout.Popup("Starting Node", _startingNodeIndex, _nodeNames);
			_defaultNodeIndex  = EditorGUILayout.Popup("Default Node", _defaultNodeIndex, _nodeNames);
			_symbolTerminal    = (NodeTerminalType) EditorGUILayout.EnumPopup("Default Symbol", _symbolTerminal);
			_connectionType    = (ConnectionType) EditorGUILayout.EnumPopup("Default Connection", _connectionType);

			// Info Block.
			GUILayout.Space(EditorStyle.PaddingAfterBlock);
			EditorGUILayout.HelpBox(_messageInfo, MessageType.Info);
			// Avoid out of range.
			if (_applyButtonEnabled) {
				_startingNode = Alphabet.Nodes[_startingNodeIndex];
				_defaultNode = Alphabet.Nodes[_defaultNodeIndex];
			}
			GUI.enabled = _applyButtonEnabled;
			if (GUILayout.Button("Apply", EditorStyles.miniButton, EditorStyle.ButtonHeight)) {
				if (EditorUtility.DisplayDialog("Saving the Grammar Settings",
						"Are you sure to save?",
						"Yes", "No")) {
					_messageInfo = "Info\n" +
						_startingNode.Name.ToString() + ", " +
						_defaultNode.Name.ToString() + ", " +
						_symbolTerminal.ToString() + ", " +
						_connectionType.ToString() + " have been saved. ";
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
	}
}
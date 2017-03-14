using UnityEngine;
using UnityEditor;
using System.Collections;

using EditorAdvance = EditorExtend.Advance;
using EditorStyle   = EditorExtend.Style;

namespace MissionGrammar {
    public class AlphabetWindow : EditorWindow {
        // Set the original color of Property.
        private Color _symbolOutlineColor;
        private Color _symbolFilledColor;
        private Color _symbolTextColor;
        // The mode of buttons.
        private bool _isInNodesInterface;
        // The scroll bar of list.
        private Vector2 _scrollPosition;
        // The description of nodes or connections
        private GraphGrammarNode _node = new GraphGrammarNode(NodeTerminalType.Terminal);
        private string _nodeName;
        private string _nodeAbbreviation;
        private string _nodeDescription;
        private string _connectionName;
        private string _connectionAbbreviation;
        private string _connectionDescription;
        // The drawing canvas.
        private Rect    _canvas;
        private Vector2 _centerPosition;
        // The type.
        private NodeTerminalType    _symbolTerminal;
        private ConnectionType      _connectionType;
        private ConnectionArrowType _ConnectionArrowType;
        // [Remove soon] Content of scroll area.
        private string testString;

        void Awake() {
            _symbolOutlineColor     = Color.black;
            _symbolFilledColor      = Color.white;
            _symbolTextColor        = Color.black;
            _isInNodesInterface     = true;
            _scrollPosition         = Vector2.zero;
            _node                   = new GraphGrammarNode(NodeTerminalType.Terminal);
            _nodeName               = "";
            _nodeAbbreviation       = "";
            _nodeDescription        = "";
            _connectionName         = "";
            _connectionAbbreviation = "";
            _connectionDescription  = "";
            _canvas                 = new Rect(0, 0, Screen.width, Screen.height);
            _centerPosition         = new Vector2(Screen.width / 2, 75);
            _symbolTerminal         = NodeTerminalType.Terminal;
            _connectionType         = ConnectionType.WeakRequirement;
            _ConnectionArrowType    = ConnectionArrowType.Normal;
            // [Remove soon]
            testString              = "1. Contents of List";
        }

        void OnGUI() {
            // Buttons - Nodes or Connections.
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Nodes", EditorStyles.miniButtonLeft, EditorStyle.TabButtonHeight)) {
                _isInNodesInterface = true;
            }
            if (GUILayout.Button("Connections", EditorStyles.miniButtonRight, EditorStyle.TabButtonHeight)) {
                _isInNodesInterface = false;
            }
            EditorGUILayout.EndHorizontal();
            // Information of selecting different button.
            if (_isInNodesInterface) {
                GUI.skin.label.fontSize  = EditorStyle.HeaderFontSize;
                GUI.skin.label.alignment = TextAnchor.MiddleCenter;
                GUILayout.Label("List of Nodes");
                GUI.skin.label.fontSize  = EditorStyle.ContentFontSize;
                GUI.skin.label.alignment = TextAnchor.UpperLeft;
                ShowNodesInterface();
            } else {
                GUI.skin.label.fontSize  = EditorStyle.HeaderFontSize;
                GUI.skin.label.alignment = TextAnchor.MiddleCenter;
                GUILayout.Label("List of Connections");
                GUI.skin.label.fontSize  = EditorStyle.ContentFontSize;
                GUI.skin.label.alignment = TextAnchor.UpperLeft;
                ShowConnectionsInterface();
            }
        }

        void ShowNodesInterface() {
            // Content of Nodes.
            // Set the ScrollPosition.
            _scrollPosition = GUILayout.BeginScrollView(_scrollPosition, GUILayout.Height(100));
            // Content of scroll area.
            GUILayout.Label(testString, EditorStyles.label);
            GUILayout.EndScrollView();
            // Buttons - Modify or Delete.
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Modify", EditorStyles.miniButtonLeft, EditorStyle.ButtonHeight)) {
                testString += "\nHere is another line";
            }
            if (GUILayout.Button("Delete", EditorStyles.miniButtonRight, EditorStyle.ButtonHeight)) {
                testString = "";
            }
            EditorGUILayout.EndHorizontal();
            // Canvas.
            GUILayout.BeginArea(EditorStyle.AlphabetPreviewArea);
            _canvas = EditorStyle.AlphabetPreviewCanvas;
            EditorGUI.DrawRect(_canvas, Color.gray);
            _centerPosition.x = Screen.width / 2;
            _node.Position    = _centerPosition;
            Alphabet.DrawNode(_node);
            GUILayout.EndArea();
            //  Content of property.
            GUILayout.BeginArea(EditorStyle.AfterAlphabetPreviewArea);
            EditorGUILayout.BeginVertical();
            GUILayout.Space(EditorStyle.PaddingAfterCanvas);
            // Dropdown list of Symbol Type
            _symbolTerminal = (NodeTerminalType) EditorGUILayout.EnumPopup("Symbol Type", _symbolTerminal);
            // Information of Nodes
            _nodeName           = EditorGUILayout.TextField("Name", _nodeName);
            _node.Abbreviation  = _nodeAbbreviation   = EditorGUILayout.TextField("Abbreviation", _nodeAbbreviation);
            _nodeDescription    = EditorGUILayout.TextField("Description", _nodeDescription);
            _node.OutlineColor  = _symbolOutlineColor = EditorGUILayout.ColorField("Outline Color", _symbolOutlineColor);
            _node.FilledColor   = _symbolFilledColor  = EditorGUILayout.ColorField("Filled Color", _symbolFilledColor);
            _node.TextColor     = _symbolTextColor    = EditorGUILayout.ColorField("Text Color", _symbolTextColor);
            EditorGUILayout.EndVertical();
            GUILayout.EndArea();
        }

        void ShowConnectionsInterface() {
            // Content of Connections.
            // Set the ScrollPosition.
            _scrollPosition = GUILayout.BeginScrollView(_scrollPosition, GUILayout.Height(100));
            // Content of scroll area.
            GUILayout.Label(testString, EditorStyles.label);
            GUILayout.EndScrollView();
            // Buttons - Modify or Delete.
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Modify", EditorStyles.miniButtonLeft, EditorStyle.ButtonHeight)) {
                testString += "\nHere is another line";
            }
            if (GUILayout.Button("Delete", EditorStyles.miniButtonRight, EditorStyle.ButtonHeight)) {
                testString = "";
            }
            EditorGUILayout.EndHorizontal();
            // Canvas.
            GUILayout.BeginArea(EditorStyle.AlphabetPreviewArea);
            _canvas = EditorStyle.AlphabetPreviewCanvas;
            EditorGUI.DrawRect(_canvas, Color.black);
            GUILayout.EndArea();
            // Content of property.
            GUILayout.BeginArea(EditorStyle.AfterAlphabetPreviewArea);
            EditorGUILayout.BeginVertical();
            GUILayout.Space(EditorStyle.PaddingAfterCanvas);
            // Dropdown list of Connection Type
            _connectionType = (ConnectionType) EditorGUILayout.EnumPopup("Connection Type", _connectionType);
            // Information of Nodes
            _connectionName         = EditorGUILayout.TextField("Name", _connectionName);
            _connectionAbbreviation = EditorGUILayout.TextField("Abbreviation", _connectionAbbreviation);
            _connectionDescription  = EditorGUILayout.TextField("Description", _connectionDescription);
            _symbolOutlineColor     = EditorGUILayout.ColorField("Outline Color", _symbolOutlineColor);
            // Dropdown list of Arrow Type
            _ConnectionArrowType    = (ConnectionArrowType) EditorGUILayout.EnumPopup("Arrow Type", _ConnectionArrowType);
            EditorGUILayout.EndVertical();
            GUILayout.EndArea();
        }
    }
}
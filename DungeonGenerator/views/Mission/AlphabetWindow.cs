using UnityEngine;
using UnityEditor;
using System.Linq;
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
        private Rect    _symbolListCanvas;
        private Rect    _symbolListCanvasInWindow;
        private Rect    _canvas;
        private Vector2 _centerPosition;
        // The type.
        private NodeTerminalType    _symbolTerminal;
        private ConnectionType      _connectionType;
        private ConnectionArrowType _ConnectionArrowType;

        void Awake() {
            // Set the first values.
            _symbolOutlineColor       = Color.black;
            _symbolFilledColor        = Color.white;
            _symbolTextColor          = Color.black;
            _isInNodesInterface       = true;
            _scrollPosition           = Vector2.zero;
            _node                     = new GraphGrammarNode(NodeTerminalType.Terminal);
            _nodeName                 = "";
            _nodeAbbreviation         = "";
            _nodeDescription          = "";
            _connectionName           = "";
            _connectionAbbreviation   = "";
            _connectionDescription    = "";
            _symbolListCanvas         = new Rect(0, 0, Screen.width, Screen.height);
            _symbolListCanvasInWindow = _symbolListCanvas;
            _canvas                   = new Rect(0, 0, Screen.width, Screen.height);
            _centerPosition           = new Vector2(Screen.width / 2, 75);
            _symbolTerminal           = NodeTerminalType.Terminal;
            _connectionType           = ConnectionType.WeakRequirement;
            _ConnectionArrowType      = ConnectionArrowType.Normal;
        }

        void OnGUI() {
            // Buttons - Nodes or Connections.
            EditorGUILayout.BeginHorizontal(GUILayout.Height(30));
            if (GUILayout.Button("Nodes", EditorStyles.miniButtonLeft, GUILayout.Height(30))) {
                _isInNodesInterface = true;
            }
            if (GUILayout.Button("Connections", EditorStyles.miniButtonRight, GUILayout.Height(30))) {
                _isInNodesInterface = false;
            }
            EditorGUILayout.EndHorizontal();
            // Toggle for nodes interface and connection interface.
            if (_isInNodesInterface) {
                GUI.skin.label.fontSize  = EditorStyle.HeaderFontSize;
                GUI.skin.label.alignment = TextAnchor.MiddleCenter;
                GUILayout.Label("List of Nodes", GUILayout.Height(30));
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
            // Event controller.
            EventController();
        }

        // Content of nodes.
        void ShowNodesInterface() {
            // Show the canvas, that is the list of nodes.
            LayoutNodeList();
            // Buttons - Modify or Delete.
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Modify", EditorStyles.miniButtonLeft, EditorStyle.ButtonHeight)) {
                Alphabet.AddNode(new GraphGrammarNode(_node));
                _scrollPosition.y = Mathf.Infinity;
            }
            if (GUILayout.Button("Delete", EditorStyles.miniButtonRight, EditorStyle.ButtonHeight)) {
                Alphabet.ClearAllNodes();
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
            // Content of property.
            GUILayout.BeginArea(EditorStyle.AfterAlphabetPreviewArea);
            EditorGUILayout.BeginVertical();
            GUILayout.Space(EditorStyle.PaddingAfterBlock);
            // Information of Node.
            _node.Terminal     = _symbolTerminal     = (NodeTerminalType) EditorGUILayout.EnumPopup("Symbol Type", _symbolTerminal);
            _node.Name         = _nodeName           = EditorGUILayout.TextField("Name", _nodeName);
            _node.Abbreviation = _nodeAbbreviation   = EditorGUILayout.TextField("Abbreviation", _nodeAbbreviation);
            _node.Description  = _nodeDescription    = EditorGUILayout.TextField("Description", _nodeDescription);
            _node.OutlineColor = _symbolOutlineColor = EditorGUILayout.ColorField("Outline Color", _symbolOutlineColor);
            _node.FilledColor  = _symbolFilledColor  = EditorGUILayout.ColorField("Filled Color", _symbolFilledColor);
            _node.TextColor    = _symbolTextColor    = EditorGUILayout.ColorField("Text Color", _symbolTextColor);
            EditorGUILayout.EndVertical();
            GUILayout.Space(EditorStyle.PaddingAfterBlock);
            // Show content of submition.
            ShowSubmitionInterface();
            GUILayout.EndArea();
        }

        // Content of connections.
        void ShowConnectionsInterface() {
            // Set the ScrollPosition.
            _scrollPosition = GUILayout.BeginScrollView(_scrollPosition, GUILayout.Height(100));
            // Content of scroll area.
            GUILayout.EndScrollView();
            // Buttons - Modify or Delete.
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Modify", EditorStyles.miniButtonLeft, EditorStyle.ButtonHeight)) {
            }
            if (GUILayout.Button("Delete", EditorStyles.miniButtonRight, EditorStyle.ButtonHeight)) {
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
            GUILayout.Space(EditorStyle.PaddingAfterBlock);
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
            GUILayout.Space(EditorStyle.PaddingAfterBlock);
            // Show content of Submit.
            ShowSubmitionInterface();
            GUILayout.EndArea();
        }

        // .
        void LayoutNodeList() {
            // Set the scroll position.
            _scrollPosition = GUILayout.BeginScrollView(_scrollPosition, GUILayout.Height(150));
            // Content of scroll area.
            GUILayout.BeginArea(EditorStyle.AlphabetSymbolListArea);
            _symbolListCanvas = EditorStyle.AlphabetSymbolListCanvas;
            EditorGUI.DrawRect(_symbolListCanvas, Color.gray);
            GUILayout.EndArea();

            foreach (var node in Alphabet.Nodes) {
                node.PositionX = 30;
                node.PositionY = 25 + 50 * Alphabet.Nodes.FindIndex(n => n == node);
                EditorExtend.NodeCanvas.DrawQuad(new Rect(5, node.PositionY - 23, Screen.width - 8, 46), node.Selected ? Color.red : Color.cyan);
                Alphabet.DrawNode(node);
                // Custom style to modify padding and margin for label.
                GUILayout.Label(node.ExpressName, EditorStyle.LabelInNodeList);
            }

            GUILayout.EndScrollView();

            // Get the Rect object from the last control when the event is Repaint.
            if (Event.current.type == EventType.Repaint) {
                _symbolListCanvasInWindow = GUILayoutUtility.GetLastRect();
            }
        }

        // Content of submition.
        void ShowSubmitionInterface() {
            // Remind user [need Modify]
            EditorGUILayout.HelpBox("Header\nContent of information.", MessageType.Info);
            // Buttons - Apply.
            if (GUILayout.Button("Apply", EditorStyles.miniButton, EditorStyle.ButtonHeight)) {
                // [Modify soon]
                /*
                    Need to create a pop up window. 
                    To make sure the Information of content.
                 */
            }
        }

        // Control whole events.
        void EventController() {
            if (Event.current.type == EventType.MouseDown) {
                OnClickedElementInList();
            }
        }

        void OnClickedElementInList() {
            float mm = Event.current.mousePosition.y - _symbolListCanvasInWindow.y;
            if (mm > 0 && mm < 150) {
                int index = (int) (mm + _scrollPosition.y) / 50;
                Alphabet.RevokeAllSelected();
                if (index < Alphabet.Nodes.Count) {
                    Alphabet.Nodes[index].Selected = true;
                    Repaint();
                }
            }
        }
    }
}
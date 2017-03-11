using UnityEngine;
using UnityEditor;
using System.Collections;

using EditorAdvance = EditorExtend.Advance;

namespace MissionGrammar
{
    public enum EnumConnectionType
    {
        _weak,
        _strong,
        _inhibition
    }

    public enum EnumArrowType
    {
        Type1,
        Type2,
        Type3
    }

    public class AlphabetWindow : EditorWindow
    {
        // Set the original color of Property.
        private Color _outlineColor_Nodes;
        private Color _outlineColor_Connections;
        private Color _fillColor;
        private Color _textColor;
        // The mode of buttons.
        private bool _nodes;
        private bool _connections;
        // The scroll bar of list.
        private Vector2 _scrollPosition;
        // The description of nodes or connections
        private string _nameNodes;
        private string _abbreviationNodes;
        private string _descriptionNodes;
        private string _nameConnections;
        private string _abbreviationConnections;
        private string _descriptionConnections;
        // The drawing canvas.
        private Rect _canvas;
        // The type.
        private EnumNodeTerminal _symbolTerminal = EnumNodeTerminal.Terminal;
        private EnumConnectionType _connectionType;
        private EnumArrowType _arrowType;

        // Content of scroll area. ( Test )
        private string testString;

        void Awake()
        {
            _outlineColor_Nodes = Color.white;
            _outlineColor_Connections = Color.white;
            _fillColor = Color.white;
            _textColor = Color.white;
            _nodes = true;
            _connections = false;
            _scrollPosition = Vector2.zero;
            testString = "1.Contents of List";
            _nameNodes = "";
            _abbreviationNodes = "";
            _descriptionNodes = "";
            _nameConnections = "";
            _abbreviationConnections = "";
            _descriptionConnections = "";
            _canvas = new Rect(0, 0, Screen.width, Screen.height);
        }

        void OnGUI()
        {
            #region Buttons
            // Buttons - Nodes or Connections.
            EditorGUILayout.BeginHorizontal();
            // <Left-hand container>
            EditorGUILayout.BeginVertical();
            if (GUILayout.Button("Nodes", GUILayout.Height(25)))
            {
                _nodes = true;
                _connections = false;
            }
            EditorGUILayout.EndVertical();
            // <Right-hand container>
            EditorGUILayout.BeginVertical();
            if (GUILayout.Button("Connections", GUILayout.Height(25)))
            {
                _nodes = false;
                _connections = true;
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
            #endregion
            #region Label
            // Information of the selected button.
            if (_nodes)
            {
                GUI.skin.label.fontSize = 24;
                GUI.skin.label.alignment = TextAnchor.MiddleCenter;
                GUILayout.Label("List of Nodes");
                GUI.skin.label.fontSize = 12;
                GUI.skin.label.alignment = TextAnchor.UpperLeft;
                ContentNodes();
            }
            else
            {
                GUI.skin.label.fontSize = 24;
                GUI.skin.label.alignment = TextAnchor.MiddleCenter;
                GUILayout.Label("List of Connections");
                GUI.skin.label.fontSize = 12;
                GUI.skin.label.alignment = TextAnchor.UpperLeft;
                ContentConnections();
            }
            #endregion
        }

        void ContentNodes()
        {
            // Content of Nodes.
            // Set the ScrollPosition.
            _scrollPosition = GUILayout.BeginScrollView(_scrollPosition, GUILayout.Height(100));
            // Content of scroll area.
            GUILayout.Label(testString, EditorStyles.label);
            GUILayout.EndScrollView();
            #region Buttons
            // Buttons - Modify or Delete.
            EditorGUILayout.BeginHorizontal();
            // <Left-hand container>
            EditorGUILayout.BeginVertical();
            if (GUILayout.Button("Modify", GUILayout.Height(20)))
            {
                testString += "\nHere is another line";
            }
            EditorGUILayout.EndVertical();
            // <Right-hand container>
            EditorGUILayout.BeginVertical();
            if (GUILayout.Button("Delete", GUILayout.Height(20)))
            {
                testString = "";
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
            #endregion
            #region Canvas
            // Canvas.
            GUILayout.BeginArea(new Rect(0, 200, Screen.width, 150));
            _canvas = new Rect(0, 0, Screen.width, 150);
            EditorGUI.DrawRect(_canvas, Color.black);
            GUILayout.EndArea();
            #endregion
            #region Property
            //  Content of property.
            GUILayout.BeginArea(new Rect(0, 350, Screen.width, Screen.height));
            EditorGUILayout.BeginVertical();
            GUILayout.Space(10);
            // Dropdown list of Symbol Type
            _symbolTerminal = (EnumNodeTerminal)EditorGUILayout.EnumPopup("Symbol Type", _symbolTerminal);
            // Information of Nodes
            _nameNodes = EditorGUILayout.TextField("Name", _nameNodes);
            _abbreviationNodes = EditorGUILayout.TextField("Abbreviation", _abbreviationNodes);
            _descriptionNodes = EditorGUILayout.TextField("Description", _descriptionNodes);
            _outlineColor_Nodes = EditorGUILayout.ColorField("Outline Color", _outlineColor_Nodes);
            _fillColor = EditorGUILayout.ColorField("Fill Color", _fillColor);
            _textColor = EditorGUILayout.ColorField("Text Color", _textColor);
            EditorGUILayout.EndVertical();
            GUILayout.EndArea();
            #endregion
        }

        void ContentConnections()
        {
            // Content of Connections.
            // Set the ScrollPosition.
            _scrollPosition = GUILayout.BeginScrollView(_scrollPosition, GUILayout.Height(100));
            // Content of scroll area.
            GUILayout.Label(testString, EditorStyles.label);
            GUILayout.EndScrollView();
            #region Buttons
            // Buttons - Modify or Delete.
            EditorGUILayout.BeginHorizontal();
            // <Left-hand container>
            EditorGUILayout.BeginVertical();
            if (GUILayout.Button("Modify", GUILayout.Height(20)))
            {
                testString += "\nHere is another line";
            }
            EditorGUILayout.EndVertical();
            // <Right-hand container>
            EditorGUILayout.BeginVertical();
            if (GUILayout.Button("Delete", GUILayout.Height(20)))
            {
                testString = "";
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
            #endregion
            #region Canvas
            // Canvas.
            GUILayout.BeginArea(new Rect(0, 200, Screen.width, 150));
            _canvas = new Rect(0, 0, Screen.width, 150);
            EditorGUI.DrawRect(_canvas, Color.black);
            GUILayout.EndArea();
            #endregion
            #region Property
            //  Content of property.
            GUILayout.BeginArea(new Rect(0, 350, Screen.width, Screen.height));
            EditorGUILayout.BeginVertical();
            GUILayout.Space(10);
            // Dropdown list of Connection Type
            _connectionType = (EnumConnectionType)EditorGUILayout.EnumPopup("Connection Type", _connectionType);
            // Information of Nodes
            _nameConnections = EditorGUILayout.TextField("Name", _nameConnections);
            _abbreviationConnections = EditorGUILayout.TextField("Abbreviation", _abbreviationConnections);
            _descriptionConnections = EditorGUILayout.TextField("Description", _descriptionConnections);
            _outlineColor_Connections = EditorGUILayout.ColorField("Outline Color", _outlineColor_Connections);
            // Dropdown list of Arrow Type
            _arrowType = (EnumArrowType)EditorGUILayout.EnumPopup("Arrow Type", _arrowType);
            EditorGUILayout.EndVertical();
            GUILayout.EndArea();
            #endregion
        }

    }
}
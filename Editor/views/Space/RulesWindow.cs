using UnityEngine;
using UnityEditor;
using System.Collections;

using EditorAdvance = EditorExtend.Advance;
using EditorStyle = EditorExtend.Style;

namespace SpaceGrammarSystem {
	public class RulesWindow : EditorWindow {
		// Group editing modes
		public enum GroupEditingModes {
			None,
			CreateGroup,
			CreateRule,
			EditGroup,
			EditRule,
			DeleteGroup,
			DeleteRule
		}

		// Symbol editing modes
		public enum InstructionEditingModes {
			None,
			Redo,
			Undo,
			Duplicate,
			Delete,
			Group,
			Ungroup,

			AddPoint, 
			AddEdges,
			AddAreas
		}

		// Lists interface
		public enum ListInterfaceTypes{
			Points, 
			Edges,
			Areas
		}

		// Edit mode
		private GroupEditingModes _groupEditingMode;
		private InstructionEditingModes _instructionEditingMode;
		private ListInterfaceTypes _listInterfaceType;

		// Name & description
		private string _name;
		private string _description;

		// Types - Group and Rule
		private int _groupOptionsIndex;
		private int _rulesOptionsIndex;
		private string[] _groupOptions;
		private string[] _rulesOptions;
		private string _currentGroup;
		private string _currentRule;
		// Types - Instruction - Replace
		private int _instructionOptionsIndex;
		private int _replaceOptionsIndex;
		private string[] _instructionOptions;
		private string[] _replaceOptions;
		private string _currentInstruction;
		private string _currentReplace;
		// Lists
		private string[] _nodesList;
		private string[] _edgesList;
		private string[] _areasList;
		// Symbol
		private string[] _symbols;
		private string _currentSymbol;
		private int _symbolsIndex;

		// Information
		private string _messageInfoGroup; 
		private string _messageInfoInstruction;

		// Weight, Scale, Symbol, Start Symbol
		private float _weight;
		private float _maxScale;
		private bool  _isStartSymbol;

		// Instruction Preview Canvas
		private Rect _canvas;

		// Icon's textures
		private Texture2D _editGroupRuleTexture;
		private Texture2D _deleteGroupRuleTexture;
		private Texture2D _undoTexture;
		private Texture2D _redoTexture;
		private Texture2D _groupTexture;
		private Texture2D _unGroupTexture;
		private Texture2D _duplicateTexture;
		private Texture2D _deleteTexture;

		//Scroll bar of lists
		private Vector2 _scrollListPosition;
		// To do : Scroll bar position - Vertical and Horizontal 

		// To do : Canvas Size/ Zooming scale

		//[Remove soon] Testing content for scroll area. 
		private string testString;

		void Awake() {
			_groupEditingMode = GroupEditingModes.None;
			_instructionEditingMode = InstructionEditingModes.None;
			_listInterfaceType = ListInterfaceTypes.Points;

			_groupOptionsIndex = 0;
			_rulesOptionsIndex = 0;
			// [Edit later]
			_groupOptions = new string[]{ "Group1", "Group2" };
			_rulesOptions = new string[]{ "Rule1", "Rule2" };
			// [Edit later] string.Empty to be "". which one is better? 
			_name                    = string.Empty;
			_description             = string.Empty;
			_instructionOptionsIndex = 0;
			_replaceOptionsIndex = 0;
			// [Edit later]
			_instructionOptions = new string[]{"Instruction"};
			_replaceOptions = new string[]{ "Replace1", "Replace2" };
			// [Edit later]
			_nodesList = new string[]{ "Node" }; 
			_edgesList = new string[]{ "Edge1", "Edge2" };
			_areasList = new string[]{ "Area1", "Area2" };
			// [Edit later]
			_symbolsIndex=0;
			_symbols = new string[]{"Symbol1", "Symbol2", "Symbol3"};
			_currentSymbol = _symbols [_symbolsIndex];

			_currentGroup = _groupOptions [_groupOptionsIndex];
			_currentRule = _rulesOptions [_rulesOptionsIndex];
			_currentInstruction = _instructionOptions [_instructionOptionsIndex];
			_currentReplace = _replaceOptions [_replaceOptionsIndex];

			_messageInfoGroup = "Info\nNothing to show.";
			_messageInfoInstruction = "Info\nNothing to show";
			_scrollListPosition = Vector2.zero;

			_weight = 1.0f;
			_maxScale = 10.0f;
			_isStartSymbol = false;

			_editGroupRuleTexture = Resources.Load<Texture2D>("Icons/edit");
			_deleteGroupRuleTexture = Resources.Load<Texture2D>("Icons/delete");
			_redoTexture = Resources.Load<Texture2D> ("Icons/redo");
			_undoTexture = Resources.Load<Texture2D> ("Icons/undo");
			_groupTexture = Resources.Load<Texture2D> ("Icons/group");
			_unGroupTexture = Resources.Load<Texture2D> ("Icons/ungroup");
			_duplicateTexture = Resources.Load<Texture2D> ("Icons/copy");
			_deleteTexture = Resources.Load<Texture2D> ("Icons/delete-element");

			// [Remove soon]
			testString = "1. Contents of List \n2. Contents of List \n3. Contents of List \n4. Contents of List \n5. Contents of List" +
				"\n6. Contents of List \n7. Contents of List \n8. Contents of List \n9. Contents of List \n10. Contents of List";
		}

		void LayoutInstructionReplace(){
			GUILayout.Space (EditorStyle.PaddingAfterBlock);
			// [Edit later] Change the layout later
			//EditorGUILayout.BeginHorizontal ();
			_instructionOptionsIndex = EditorGUILayout.Popup ("Instruction", _instructionOptionsIndex, _instructionOptions);
			_currentInstruction = _instructionOptions [_instructionOptionsIndex];
			_replaceOptionsIndex = EditorGUILayout.Popup ("Replace", _replaceOptionsIndex, _replaceOptions);
			_currentReplace= _replaceOptions[_replaceOptionsIndex];
			//EditorGUILayout.EndHorizontal ();

			// Drawing Canvas
			GUILayout.BeginArea(EditorStyle.SpaceRulePreviewArea);
			_canvas = EditorStyle.SpaceRulePreviewCanvas;
			EditorGUI.DrawRect (_canvas, Color.white);
			GUILayout.EndArea();

			// Helpbox 
			GUILayout.Space (EditorStyle.PaddingAfterBlock + EditorStyle.SpaceRulePreviewArea.size.y);

			// Buttons
			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button (_redoTexture, EditorStyles.miniButtonLeft, EditorStyle.ButtonHeight)) {
				_instructionEditingMode = InstructionEditingModes.Redo;
			}
			if (GUILayout.Button (_undoTexture, EditorStyles.miniButtonMid, EditorStyle.ButtonHeight)) {
				_instructionEditingMode = InstructionEditingModes.Undo;
			}
			if (GUILayout.Button (_groupTexture, EditorStyles.miniButtonMid, EditorStyle.ButtonHeight)) {
				_instructionEditingMode = InstructionEditingModes.Group;
			}
			if (GUILayout.Button (_unGroupTexture, EditorStyles.miniButtonMid, EditorStyle.ButtonHeight)) {
				_instructionEditingMode = InstructionEditingModes.Ungroup;
			}
			if (GUILayout.Button (_duplicateTexture, EditorStyles.miniButtonMid, EditorStyle.ButtonHeight)) {
				_instructionEditingMode = InstructionEditingModes.Duplicate;
			}
			if (GUILayout.Button (_deleteTexture, EditorStyles.miniButtonRight, EditorStyle.ButtonHeight)) {
				_instructionEditingMode = InstructionEditingModes.Delete;
			}
			EditorGUILayout.EndHorizontal();

			// Add buttons - Point, Edge, Area
			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button ("Add Point", EditorStyles.miniButtonLeft, EditorStyle.TabButtonHeight)) {
				_listInterfaceType = ListInterfaceTypes.Points;
			} else if (GUILayout.Button ("Add Edge", EditorStyles.miniButtonMid, EditorStyle.TabButtonHeight)) {
				_listInterfaceType = ListInterfaceTypes.Edges;
			} else if (GUILayout.Button ("Add Area", EditorStyles.miniButtonRight, EditorStyle.TabButtonHeight)) {
				_listInterfaceType = ListInterfaceTypes.Areas;
			}
			EditorGUILayout.EndHorizontal();
		}

		void LayoutLists(){
			//List
			GUI.skin.label.fontSize = EditorStyle.HeaderFontSize;
			GUI.skin.label.alignment = TextAnchor.MiddleCenter;
			if (_listInterfaceType == ListInterfaceTypes.Points) {
				GUILayout.Label ("List of Points");
			} else if (_listInterfaceType == ListInterfaceTypes.Edges) {
				GUILayout.Label ("List of Edges");	
			} else if (_listInterfaceType == ListInterfaceTypes.Areas) {
				GUILayout.Label ("List of Areas");	
			}
			GUI.skin.label.fontSize = EditorStyle.ContentFontSize;
			GUI.skin.label.alignment = TextAnchor.UpperLeft;

			// Scroll bar & area
			_scrollListPosition = GUILayout.BeginScrollView(_scrollListPosition, GUILayout.Height(100));
			GUILayout.Label(testString, EditorStyles.label);
			GUILayout.EndScrollView();

			// Helpbox 
			EditorGUILayout.HelpBox (_messageInfoInstruction, MessageType.Info);

			//Weight, Symbol, Max.Scale, Start Symbol
			EditorGUILayout.BeginHorizontal();
			_weight = EditorGUILayout.FloatField ("Weight", _weight);
			_maxScale = EditorGUILayout.FloatField ("Max. Scale",_maxScale);
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.BeginHorizontal();
			_symbolsIndex = EditorGUILayout.Popup ("Symbol", _symbolsIndex, _symbols);
			_isStartSymbol = EditorGUILayout.ToggleLeft ("Set Start Symbol", _isStartSymbol);
			EditorGUILayout.EndHorizontal();

			//Apply button and popup
			if (GUILayout.Button("Apply", EditorStyles.miniButton, EditorStyle.ButtonHeight)) {
				if (EditorUtility.DisplayDialog ("Apply on the element", "Are you sure want to apply the operation?", "Yes", "No")) {
					_messageInfoInstruction = "Saving Instruction : "+ _currentInstruction + ", Replace : " +_currentReplace + ".";
				} else {
					_messageInfoInstruction = "Info\nNothing to show.";
				}
			}
		}

		void OnGUI(){
			// To do : init the GUI button style

			// Blocks of Group, Rules, and Apply button. 
			LayoutGroupRule(); 
			// Blocks of Instruction, Canvas, Buttons
			LayoutInstructionReplace();
			// BLocks of List, Apply button
			LayoutLists();
		}

		void LayoutGroupRule(){
			// Groups
			EditorGUILayout.BeginHorizontal ();
			_groupOptionsIndex = EditorGUILayout.Popup ("Current Group", _groupOptionsIndex, _groupOptions);
			_currentGroup = _groupOptions [_groupOptionsIndex];
			if (GUILayout.Button (_editGroupRuleTexture, EditorStyles.miniButtonLeft, EditorStyle.ButtonHeight)) {
				_groupEditingMode = GroupEditingModes.EditGroup;
			}
			if (GUILayout.Button (_deleteGroupRuleTexture, EditorStyles.miniButtonMid, EditorStyle.ButtonHeight)) {
				_groupEditingMode = GroupEditingModes.DeleteGroup;
			}
			if (GUILayout.Button ("Add New", EditorStyles.miniButtonRight, EditorStyle.ButtonHeight)) {
				_groupEditingMode = GroupEditingModes.DeleteGroup;
			}
			EditorGUILayout.EndHorizontal ();

			// Rules
			EditorGUILayout.BeginHorizontal ();
			_rulesOptionsIndex = EditorGUILayout.Popup ("Current Rule", _rulesOptionsIndex, _rulesOptions);
			_currentRule = _rulesOptions [_rulesOptionsIndex];
			if (GUILayout.Button (_editGroupRuleTexture, EditorStyles.miniButtonLeft, EditorStyle.ButtonHeight)) {
				_groupEditingMode = GroupEditingModes.EditGroup;
			}
			if (GUILayout.Button (_deleteGroupRuleTexture, EditorStyles.miniButtonMid, EditorStyle.ButtonHeight)) {
				_groupEditingMode = GroupEditingModes.DeleteGroup;
			}
			if (GUILayout.Button ("Add New", EditorStyles.miniButtonRight, EditorStyle.ButtonHeight)) {
				_groupEditingMode = GroupEditingModes.DeleteGroup;
			}
			EditorGUILayout.EndHorizontal ();

			// Info and Apply button
			// [Edit later] show the helpbox and button only if we want to edit or add new. Switch based on editgroupmode
			_name = EditorGUILayout.TextField("Name", _name);
			_description = EditorGUILayout.TextField ("Description", _description);
			EditorGUILayout.HelpBox(_messageInfoGroup, MessageType.Info);
			if (GUILayout.Button ("Apply", EditorStyles.miniButton, EditorStyle.ButtonHeight)) {
				if (EditorUtility.DisplayDialog ("Applying", "Are you sure?", "Yes", "No")) {
					// [Edit later] Validate the input, add element to group and rule list, autohide. 
					_messageInfoGroup = "Saving Group : "+ _currentGroup + ", Rule : " + _currentRule + ".";
				} else {
					_messageInfoGroup = "Info\nNothing to show.";
				}
			}
		}

		//
    }
}
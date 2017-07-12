#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;

namespace EditorExtend {
	public class NodeCanvas {
		private static Texture2D _texture;
        private static Texture2D Texture {
            get {
                if (_texture == null) {
                    _texture = new Texture2D(1, 1);
                }
                return _texture;
            }
            set { _texture = value; }
        }
		// Draw a quad.
		public static void DrawQuad(Rect rect, Color color) {
			Texture2D texture = Texture;
			texture.SetPixel(0, 0, color);
			texture.Apply();
			GUI.skin.box.normal.background = texture;
			GUI.Box(rect, GUIContent.none);
		}
		// Draw a quad with the text.
		public static void DrawQuad(Rect rect, Color color, string content, Color textColor) {
			Texture2D texture = Texture;
			texture.SetPixel(0, 0, color);
			texture.Apply();
			GUI.skin.box.normal.background = texture;
			GUI.skin.box.normal.textColor  = textColor;
			GUI.Box(rect, content);
		}
		// Draw a disc.
		public static void DrawDisc(Vector3 position, float radius, Color color) {
			Handles.BeginGUI();
			Handles.color = color;
			Handles.DrawSolidDisc(position, Vector3.forward, radius);
			Handles.EndGUI();
		}
		// Draw a triangle.
		public static void DrawTriangle(Vector3[] position, Color color) {
			Handles.BeginGUI();
			Handles.color = color;
			Handles.DrawAAConvexPolygon(position);
			Handles.EndGUI();
		}
		// Draw a line with two positions.
		public static void DrawLine(Vector2 startPos, Vector2 endPos, Color color, float thickness) {
			Handles.BeginGUI();
			Handles.color = color;
			Vector3[] lists = new Vector3[] { new Vector3(startPos[0], startPos[1], 0), new Vector3(endPos[0], endPos[1], 0) };
			Handles.DrawAAPolyLine(thickness, lists);
			Handles.EndGUI();
		}
		// Draw a Bezier line with two positions.
		public static void DrawCurve(Vector3 startPos, Vector3 endPos, Color color) {
			float mnog = Vector3.Distance(startPos, endPos);
			Vector3 startTangent = startPos + Vector3.right * (mnog / 3f) ;
			Vector3 endTangent = endPos + Vector3.left * (mnog / 3f);
			Handles.BeginGUI();
			Handles.DrawBezier(startPos, endPos, startTangent, endTangent, color, null, 3f);
			Handles.EndGUI();
		}
	}
}
#endif
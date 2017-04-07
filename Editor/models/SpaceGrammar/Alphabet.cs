using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using EditorCanvas = EditorExtend.NodeCanvas;
using EditorStyle  = EditorExtend.Style;

namespace SpaceGrammarSystem {
	public static class Alphabet {
		// Default points in alphabet.
		private static List<SpaceGrammarPoint> _points = new List<SpaceGrammarPoint>() {
				new SpaceGrammarPoint("none",     "none", "System default.", PointTerminalType.Terminal),
				new SpaceGrammarPoint("entrance", "en",   "System default.", PointTerminalType.Terminal),
				new SpaceGrammarPoint("goal",     "go",   "System default.", PointTerminalType.Terminal),
			};
		private static List<SpaceGrammarEdge> _edges = new List<SpaceGrammarEdge>() {
				
			};

		// point list in alphabet.
		public static List<SpaceGrammarPoint> Points {
			get { return _points; }
			set { _points = value; }
		}
		// Connection list in alphabet.
		public static List<SpaceGrammarEdge> Connections {
			get { return _edges; }
			set { _edges = value; }
		}
		// Return the first selected point.
		public static SpaceGrammarPoint SelectedPoint {
			get { return _points.Where(n => n.Selected == true).FirstOrDefault(); }
		}
		// Return the first selected point.
		public static SpaceGrammarEdge SelectedConnection {
			get { return _edges.Where(c => c.Selected == true).FirstOrDefault(); }
		}
		// Add a new point.
		public static void AddPoint(SpaceGrammarPoint point) {
			_points.Add(point);
		}
		// Add a new connection.
		public static void AddConnection(SpaceGrammarEdge connection) {
			_edges.Add(connection);
		}
		// Remove one point.
		public static void RemovePoint(SpaceGrammarPoint point) {
			_points.Remove(point);
		}
		// Remove one connection.
		public static void RemoveConnection(SpaceGrammarEdge connection) {
			_edges.Remove(connection);
		}
		// Return a boolean when it's name never be used in alphabet.
		public static bool IsPointNameUsed(SpaceGrammarPoint currentpoint) {
			if (currentpoint == null) { return false; }
			return (from point in Points
				where point.Name == currentpoint.Name && point != SelectedPoint
				select point)
				.Any();
		}
		// Return a boolean about it's abbreviation never be used in alphabet.
		public static bool IsPointAbbreviationUsed(SpaceGrammarPoint currentpoint) {
			if (currentpoint == null) { return false; }
			return (from point in Points
				where point.Abbreviation == currentpoint.Abbreviation && point != SelectedPoint
				select point)
				.Any();
		}
		
		// Remove all points.
		public static void ClearAllPoints() {
			// Create a new point.
			_points.Clear();
		}
		// Set all 'seleted' of symbols to false.
		public static void RevokeAllSelected() {
			foreach (SpaceGrammarPoint point in _points) {
				point.Selected = false;
			}
			foreach (SpaceGrammarEdge connection in _edges) {
				connection.Selected = false;
			}
		}
		// Draw the point in the point list.
		public static void DrawpointInList(SpaceGrammarPoint point) {
			point.PositionX = 30;
			point.PositionY = 25 + 50 * _points.FindIndex(n => n == point);
			// Background color of selectable area.
			EditorCanvas.DrawQuad(new Rect(5, point.PositionY - 23, Screen.width - 8, 46), point.Selected ? new Color(0.75f, 0.75f, 1, 0.75f) : Color.clear);
			// Draw this point.
			point.Draw();
		}
	}
}
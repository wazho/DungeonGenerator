using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using EditorCanvas = EditorExtend.NodeCanvas;

namespace SpaceGrammarSystem {
	public class GraphGrammar {
		private List<SpaceGrammarPoint> _points;
		private List<SpaceGrammarEdge>  _edges;
		private SpaceGrammarSymbol      _selectedSymbol;

		public GraphGrammar() {
			this._points          = new List<SpaceGrammarPoint>();
			this._edges    = new List<SpaceGrammarEdge>();
			this._selectedSymbol = null;
		}

		// points, getter and setter.
		public List<SpaceGrammarPoint> points {
			get { return _points; }
			set { _points = value; }
		}
		// Connections, getter and setter.
		public List<SpaceGrammarEdge> Edges {
			get { return _edges; }
			set { _edges = value; }
		}
		// SelectedSymbol, getter and setter.
		// This is the parent class, it can be assigned by SpaceGrammarPoint of SpaceGrammarEdge.
		public SpaceGrammarSymbol SelectedSymbol {
			get { return _selectedSymbol; }
			set { _selectedSymbol = value; }
		}

		// Pass the mouse position.
		// Be careful for one thing. '_points' and '_edges' must pre-order by ordering.
		public void TouchedSymbol(Vector2 pos) {
			// Find the 'points of selected connection'.
			foreach (SpaceGrammarEdge symbol in _edges.AsEnumerable().Reverse()) {
				// This connection is selected, check its endpoints are in scope or not.
				if (symbol.Selected) {
					// Update selected symbol.
					_selectedSymbol = symbol;
					return;
				}
			}
			// Find the 'selected points'.
			foreach (SpaceGrammarPoint symbol in _points.AsEnumerable().Reverse()) {
				if (symbol.IsInScope(pos)) {
					// Initial all symbol first.
					RevokeAllSelected();
					// Update the symbol status.
					symbol.Selected = true;
					// Update selected symbol.
					_selectedSymbol = symbol;
					return;
				}
			}
			// If anything has been found or not.
			RevokeAllSelected();
			_selectedSymbol = null;
		}

		// Add a new node.
		public void AddPoint() {
			// Revoke all symbols first.
			RevokeAllSelected();
			// Create a new node and update its ordering and selected status.
			SpaceGrammarPoint point = new SpaceGrammarPoint(PointTerminalType.NonTerminal);
			point.Ordering = _points.Count + 1;
			point.Selected = true;
			_points.Add(point);
			// Update the current node.
			_selectedSymbol = point;
		}
		// Add a new node from another exist node.
		public void AddNode(SpaceGrammarPoint nodeClone) {
			RevokeAllSelected();
			// Deep copy.
			SpaceGrammarPoint node = new SpaceGrammarPoint(nodeClone);
			node.Ordering = _points.Count + 1;
			node.Selected = true;
			_points.Add(node);
			_selectedSymbol = node;
		}
		// Update symbol appearance.
		public void UpdateSymbol(SpaceGrammarSymbol before, SpaceGrammarSymbol after) {
			int symbolIndex                   = -1;
			SpaceGrammarPoint       node       = null;
			SpaceGrammarEdge connection = null;
			if (before is SpaceGrammarPoint) {
				node = (SpaceGrammarPoint) after;
				symbolIndex = _points.FindIndex(x => x.Equals(before));
				_points[symbolIndex].Terminal     = node.Terminal;
				_points[symbolIndex].Name         = node.Name;
				_points[symbolIndex].Abbreviation = node.Abbreviation;
				_points[symbolIndex].Description  = node.Description;
				_points[symbolIndex].OutlineColor = node.OutlineColor;
				_points[symbolIndex].FilledColor  = node.FilledColor;
				_points[symbolIndex].TextColor    = node.TextColor;
			} else if (before is SpaceGrammarEdge) {

			}
		}
		// Add a new connection.
		public void AddConnection() {
			// Revoke all symbols first.
			RevokeAllSelected();
			// Create a new connection.
			SpaceGrammarEdge connection = new SpaceGrammarEdge();
			connection.Selected = true;
			_edges.Add(connection);
			// Update the current connection.
			_selectedSymbol = connection;
		}
		// Set all 'seleted' of symbols to false.
		public void RevokeAllSelected() {
			foreach (var symbol in _points) {
				symbol.Selected = false;
			}
			/*
			foreach (var symbol in _edges) {
				symbol.Selected = symbol.StartSelected = symbol.EndSelected = false;
			}
			*/
			_selectedSymbol = null;
			return;
		}
	}
}
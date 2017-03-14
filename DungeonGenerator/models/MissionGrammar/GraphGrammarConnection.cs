using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace MissionGrammar {
	// Types of connection.
	public enum ConnectionType {
		WeakRequirement   = 0,
		StrongRequirement = 1,
		Inhibition        = 2
	}
	// Types of connection arrow.
	public enum ConnectionArrowType {
		Normal     = 0,
		Double     = 1,
		WithCircle = 2
	}

	public class GraphGrammarConnection : GraphGrammarSymbol {
		// Values of setting.
		private int pointScopeSize  = 11;
		private float lineThickness = 5f;
		// Members.
		private bool startSelected, endSelected;
		private Rect startpointScope, endpointScope;
		private GraphGrammarNode startpointStickyOn, endpointStickyOn;

		public GraphGrammarConnection() : base() {
			this._type            = SymbolType.Connection;
			this.startpointScope = new Rect(10, 10, this.pointScopeSize, this.pointScopeSize);
			this.endpointScope   = new Rect(100, 10, this.pointScopeSize, this.pointScopeSize);
		}
		// .
		public int PointScopeSize {
			get { return this.pointScopeSize; }
		}
		// .
		public float LineThickness {
			get { return this.lineThickness; }
		}
		// .
		public bool StartSelected {
			get { return this.startSelected; }
			set { this.startSelected = value; }
		}
		// .
		public bool EndSelected {
			get { return this.endSelected; }
			set { this.endSelected = value; }
		}
		// .
		public Vector2 StartPosition {
			get { return this.startpointScope.position; }
			set { this.startpointScope.position = value; }
		}
		// .
		public Vector2 EndPosition {
			get { return this.endpointScope.position; }
			set { this.endpointScope.position = value; }
		}
		// .
		public GraphGrammarNode StartpointStickyOn {
			get { return this.startpointStickyOn; }
			set { this.startpointStickyOn = value; }
		}
		// .
		public GraphGrammarNode EndpointStickyOn {
			get { return this.endpointStickyOn; }
			set { this.endpointStickyOn = value; }
		}
		// .
		public bool IsInStartscope(Vector2 pos) {
			return this.startpointScope.Contains(pos);
		}
		// .
		public bool IsInEndscope(Vector2 pos) {
			return this.endpointScope.Contains(pos);
		}
	}
}
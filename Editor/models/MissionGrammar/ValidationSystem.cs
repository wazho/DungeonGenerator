using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using System;

namespace MissionGrammarSystem {
	// Label of the validation system in mission rule.
	public enum ValidationLabel {
		LeftMoreThanRight,
		EmptyLeft,
		HeadHasParent,
		IsolatedNode,
		IsolatedConnection,
		ExactlyDuplicated,
		MultipleRelations,
		CyclicLink,
	}

	public class ValidationSystem {
		private static List<ValidationLabel> _error = new List<ValidationLabel>();
		// Validate the rule is legal or not.
		private static Dictionary<ValidationLabel, Predicate<GraphGrammar>> _validationMethods = new Dictionary<ValidationLabel, Predicate<GraphGrammar>>() {
			{ ValidationLabel.LeftMoreThanRight,  (GraphGrammar graphGrammar) => ValidateLeftMoreThanRight(graphGrammar) },
			{ ValidationLabel.EmptyLeft,          (GraphGrammar graphGrammar) => ValidateEmptyLeft(graphGrammar) },
			{ ValidationLabel.HeadHasParent,      (GraphGrammar graphGrammar) => ValidateHeadHasParent(graphGrammar) },
			{ ValidationLabel.IsolatedNode,       (GraphGrammar graphGrammar) => ValidateIsolatedNode(graphGrammar) },
			{ ValidationLabel.IsolatedConnection, (GraphGrammar graphGrammar) => ValidateIsolatedConnection(graphGrammar) },
			{ ValidationLabel.ExactlyDuplicated,  (GraphGrammar graphGrammar) => ValidateExactlyDuplicated(graphGrammar) },
			{ ValidationLabel.MultipleRelations,  (GraphGrammar graphGrammar) => ValidateMultipleRelations(graphGrammar) },
			{ ValidationLabel.CyclicLink,         (GraphGrammar graphGrammar) => ValidateCyclicLink(graphGrammar) },
		};

		// Validate the graph grammar (one of pair of rule).
		public static void Validate(GraphGrammar graphGrammar) {
			// Initial the error list.
			_error.Clear();
			// Execute each method.
			foreach (var method in _validationMethods) {
				if (! method.Value.Invoke(graphGrammar)) {
					if (! _error.Contains(method.Key)) {
						_error.Add(method.Key);
					}
				} else {
					_error.Remove(method.Key);
				}
			}
			// Debug.
			Debug.Log(string.Join(", ", _error.Cast<ValidationLabel>().Select(v => v.ToString()).ToArray()));
		}
		// No 1. LeftMoreThanRight.
		private static bool ValidateLeftMoreThanRight(GraphGrammar graphGrammar) {

			return false;
		}
		// No 2. EmptyLeft.
		private static bool ValidateEmptyLeft(GraphGrammar graphGrammar) {

			return false;
		}
		// No 3. HeadHasParent.
		private static bool ValidateHeadHasParent(GraphGrammar graphGrammar) {

			return true;
		}
		// No 4. IsolatedNode.
		private static bool ValidateIsolatedNode(GraphGrammar graphGrammar) {

			return true;
		}
		// No 5. IsolatedConnection.
		private static bool ValidateIsolatedConnection(GraphGrammar graphGrammar) {

			return true;
		}
		// No 6. ExactlyDuplicated.
		private static bool ValidateExactlyDuplicated(GraphGrammar graphGrammar) {

			return true;
		}
		// No 7. MultipleRelations.
		private static bool ValidateMultipleRelations(GraphGrammar graphGrammar) {

			return true;
		}
		// No 8. CyclicLink.
		private static bool ValidateCyclicLink(GraphGrammar graphGrammar) {

			return true;
		}
	}
}
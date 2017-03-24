using UnityEngine;
using System.Collections;
using System.Xml.Linq;
using System;

using Mission = MissionGrammarSystem;
using System.Collections.Generic;

namespace DungeonLevel {
	public static class OperateXML {

		// Static method for other file calling.
		public static void SerializeToXml(string path) {
			XDocument xmlDocument = new XDocument();
			xmlDocument.Add(SerializeMissionGrammar());
			xmlDocument.Save(path);
		}

		// Serialize MissionGrammar
		private static XElement SerializeMissionGrammar() {
			XElement elementMissionGrammar = new XElement("MissionGrammar");
			elementMissionGrammar.Add(SerializeMissionAlphabet());
			elementMissionGrammar.Add(SerializeMissionRules());

			return elementMissionGrammar;
		}

		// Serialize MissionAlphabet
		private static XElement SerializeMissionAlphabet() {
			XElement elementMissionAlphabet = new XElement("MissionAlphabet");
			elementMissionAlphabet.Add(SerializeNodes(Mission.Alphabet.Nodes),
				SerializeConnections(Mission.Alphabet.Connections));

			return elementMissionAlphabet;
		}
		// Serialize MissionRule
		private static XElement SerializeMissionRules() {
			XElement elementMissionGraph = new XElement("MissionRules");
			foreach (var group in Mission.MissionGrammar.Groups) {
				XElement elementGroup = new XElement("RuleGroup");
				elementGroup.Add(new XElement("Name", group.Name));
				elementGroup.Add(new XElement("Description", group.Description));
				foreach (var rule in group.Rules) {
					XElement elementRule = new XElement("Rule");
					elementRule.Add(new XElement("Name", rule.Name));
					elementRule.Add(new XElement("Description", rule.Description));
					elementRule.Add(new XElement("SourceRule", SerializeMissionGraph(rule.SourceRule)));
					elementRule.Add(new XElement("ReplacementRule", SerializeMissionGraph(rule.ReplacementRule)));
					elementRule.Add(new XElement("Enable", rule.Enable.ToString()));

					elementGroup.Add(elementRule);
				}

				elementMissionGraph.Add(elementGroup);
			}
			return elementMissionGraph;
		}

		// Serialize MissionGraph
		private static XElement SerializeMissionGraph(Mission.GraphGrammar graph) {
			XElement elementMissionGraph = new XElement("MissionGraph");
			elementMissionGraph.Add(SerializeNodes(graph.Nodes),
				SerializeConnections(graph.Connections));

			return elementMissionGraph;
		}

		// Serialize nodes
		private static XElement SerializeNodes(List<Mission.GraphGrammarNode> nodes) {
			XElement element = new XElement("Nodes");
			foreach (var node in nodes) {
				XElement elementNode = new XElement("Node", new XAttribute("id", node.ID));
				elementNode.Add(new XElement("AlphabetID", node.AlphabetID));
				elementNode.Add(new XElement("Name", node.Name));
				elementNode.Add(new XElement("Abbreviation", node.Abbreviation));
				elementNode.Add(new XElement("Description", node.Description));
				elementNode.Add(new XElement("Terminal", node.Terminal.ToString()));
				elementNode.Add(new XElement("Scope"));
				elementNode.Add(new XElement("SymbolColor"));
				elementNode.Add(new XElement("StickiedConnections"));

				XElement elementScope = elementNode.Element("Scope");
				elementScope.Add(SerializeRect("Outline", node.OutlineScope));
				elementScope.Add(SerializeRect("Filled", node.FilledScope));
				elementScope.Add(SerializeRect("Text", node.TextScope));

				XElement elementSymbolColor = elementNode.Element("SymbolColor");
				elementSymbolColor.Add(SerializeColor("Outline", node.OutlineColor));
				elementSymbolColor.Add(SerializeColor("Filled", node.FilledColor));
				elementSymbolColor.Add(SerializeColor("Text", node.TextColor));

				XElement elementStickiedConnection = elementNode.Element("StickiedConnections");
				foreach (var guid in node.StickiedConnectionsGuid) {
					elementStickiedConnection.Add(new XElement("ConnectionID",guid));
				}
				element.Add(elementNode);
			}
			return element;
		}
		// Serialize connections
		private static XElement SerializeConnections(List<Mission.GraphGrammarConnection> connections) {
			XElement element = new XElement("Connections");
			foreach (var connection in connections) {
				XElement elementConnection = new XElement("Connection", new XAttribute("id", connection.ID));
				elementConnection.Add(new XElement("AlphabetID", connection.AlphabetID));
				elementConnection.Add(new XElement("Name", connection.Name));
				elementConnection.Add(new XElement("Description", connection.Description));
				elementConnection.Add(new XElement("Requirement", connection.Requirement));
				elementConnection.Add(new XElement("ArrowType", connection.Arrow.ToString()));
				elementConnection.Add(new XElement("Scope"));
				elementConnection.Add(new XElement("SymbolColor"));
				elementConnection.Add(new XElement("StartpointStickyOn", connection.StartpointStickyOn == null ? "" : connection.StartpointStickyOn.ID.ToString()));
				elementConnection.Add(new XElement("EndpointStickyOn", connection.EndpointStickyOn == null ? "" : connection.EndpointStickyOn.ID.ToString()));
				elementConnection.Add(new XElement("StartSelected", connection.StartSelected.ToString()));
				elementConnection.Add(new XElement("EndSelected", connection.EndSelected.ToString()));

				XElement elementScope = elementConnection.Element("Scope");
				elementScope.Add(SerializeRect("Startpoint", connection.StartpointScope));
				elementScope.Add(SerializeRect("Endpoint", connection.EndpointScope));

				XElement elementSymbolColor = elementConnection.Element("SymbolColor");
				elementSymbolColor.Add(SerializeColor("Outline", connection.OutlineColor));

				element.Add(elementConnection);
			}
			return element;
		}
		// Serialize rect
		private static XElement SerializeRect(string name, Rect rect) {
			XElement element = new XElement(name);
			element.Add(new XElement("X", rect.x),
				new XElement("Y", rect.y),
				new XElement("Width", rect.width),
				new XElement("Height", rect.height));
			return element;
		}
		// Serialize color
		private static XElement SerializeColor(string name, Color color) {
			return new XElement(name, "#" + ( (byte) ( color.r * 255 ) ).ToString("X2") + ( (byte) ( color.g * 255 ) ).ToString("X2") + ( (byte) ( color.b * 255 ) ).ToString("X2"));
		}
	}
}

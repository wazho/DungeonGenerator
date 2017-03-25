using UnityEngine;
using System.Collections;
using System.Xml.Linq;
using System;

using Mission = MissionGrammarSystem;
using System.Collections.Generic;

namespace DungeonLevel {
	public static class OperateXML {
		public static class Serialize {
			// Static method for other class calling.
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
					elementNode.Add(new XElement("Ordering", node.Ordering));
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
						elementStickiedConnection.Add(new XElement("ConnectionID", guid));
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

		public static class Unserialize {
			// Static method for other class calling.
			public static void UnserializeFromXml(string path) {
				XDocument xmlDocument = XDocument.Load(path);
				UnserializeMissionGrammar(xmlDocument);
			}
			// Unserialize MissionGrammar
			private static void UnserializeMissionGrammar(XDocument xmlDocument) {
				XElement elementMissionGrammar = xmlDocument.Element("MissionGrammar");
				UnserializeMissionAlphabet(elementMissionGrammar);
				UnserializeMissionRules(elementMissionGrammar);
			}
			// Unserialize MissionAlphabet
			private static void UnserializeMissionAlphabet(XElement elementMissionGrammar) {
				XElement elementMissionAlphabet = elementMissionGrammar.Element("MissionAlphabet");
				Mission.Alphabet.Nodes = UnserializeNodes(elementMissionAlphabet);
				Mission.Alphabet.Connections = UnserializeConnections(elementMissionAlphabet);
			}
			// Unserialize MissionRule
			private static void UnserializeMissionRules(XElement elementMissionGrammar) {
				List<Mission.MissionGroup> groups = new List<Mission.MissionGroup>();
				XElement elementMissionGraph = elementMissionGrammar.Element("MissionRules");
				foreach (var elementGroup in elementMissionGraph.Elements("RuleGroup")) {
					Mission.MissionGroup group = new MissionGrammarSystem.MissionGroup();
					group.Name = elementGroup.Element("Name").Value;
					group.Description = elementGroup.Element("Description").Value;
					group.Rules = new List<Mission.MissionRule>();
					foreach (var elementRule in elementGroup.Elements("Rule")) {
						Mission.MissionRule rule = new Mission.MissionRule();
						rule.Name = elementRule.Element("Name").Value;
						rule.Description = elementRule.Element("Description").Value;
						rule.SourceRule = UnserializeMissionGraph(elementRule.Element("SourceRule"));
						rule.ReplacementRule = UnserializeMissionGraph(elementRule.Element("ReplacementRule"));
						rule.Enable = bool.Parse(elementRule.Element("Enable").Value);

						group.Rules.Add(rule);
					}
					groups.Add(group);
				}
				Mission.MissionGrammar.Groups = groups;
			}
			// Unserialize MissionGraph
			private static Mission.GraphGrammar UnserializeMissionGraph(XElement element) {
				Mission.GraphGrammar graph = new Mission.GraphGrammar();
				XElement elementMissionGraph = element.Element("MissionGraph");
				graph.Nodes = UnserializeNodes(elementMissionGraph);
				graph.Connections = UnserializeConnections(elementMissionGraph);
				// Unserialize sticky
				int connectionIndex = 0;
				foreach (var elementConnection in elementMissionGraph.Element("Connections").Elements("Connection")) {
					if (!elementConnection.Element("StartpointStickyOn").IsEmpty) {
						Guid guid = new Guid(elementConnection.Element("StartpointStickyOn").Value);
						int nodeIndex = graph.Nodes.FindIndex(x => x.ID == guid);
						graph.Connections[connectionIndex].StartpointStickyOn = graph.Nodes[nodeIndex];
						graph.Nodes[nodeIndex].AddStickiedConnection(graph.Connections[connectionIndex], "start");
					}
					if (!elementConnection.Element("EndpointStickyOn").IsEmpty) {
						Guid guid = new Guid(elementConnection.Element("EndpointStickyOn").Value);
						int nodeIndex = graph.Nodes.FindIndex(x => x.ID == guid);
						graph.Connections[connectionIndex].StartpointStickyOn = graph.Nodes[nodeIndex];
						graph.Nodes[nodeIndex].AddStickiedConnection(graph.Connections[connectionIndex], "end");
					}
					connectionIndex++;
				}
				return graph;
			}
			// Unserialize nodes
			private static List<Mission.GraphGrammarNode> UnserializeNodes(XElement element) {
				List<Mission.GraphGrammarNode> nodes = new List<Mission.GraphGrammarNode>();
				XElement elementNodes = element.Element("Nodes");
				foreach (var elementNode in elementNodes.Elements("Node")) {
					Mission.GraphGrammarNode node = new Mission.GraphGrammarNode();

					node.ID = new Guid(elementNode.Attribute("id").Value);
					node.AlphabetID = new Guid(elementNode.Element("AlphabetID").Value);
					node.Ordering = System.Int32.Parse(elementNode.Element("Ordering").Value);
					node.Name = elementNode.Element("Name").Value;
					node.Abbreviation = elementNode.Element("Abbreviation").Value;
					node.Description = elementNode.Element("Description").Value;
					node.Terminal = (Mission.NodeTerminalType) Enum.Parse(typeof(Mission.NodeTerminalType), elementNode.Element("Terminal").Value);

					XElement elementScope = elementNode.Element("Scope");
					node.OutlineScope = UnserializeRect(elementScope.Element("Outline"));
					node.FilledScope = UnserializeRect(elementScope.Element("Filled"));
					node.TextScope = UnserializeRect(elementScope.Element("Text"));

					XElement elementSymbolColor = elementNode.Element("SymbolColor");
					node.OutlineColor = UnserializeColor(elementSymbolColor.Element("Outline"));
					node.FilledColor = UnserializeColor(elementSymbolColor.Element("Filled"));
					node.TextColor = UnserializeColor(elementSymbolColor.Element("Text"));

					nodes.Add(node);
				}
				return nodes;
			}
			// Unserialize connections
			private static List<Mission.GraphGrammarConnection> UnserializeConnections(XElement element) {
				List<Mission.GraphGrammarConnection> connections = new List<Mission.GraphGrammarConnection>();
				XElement elementConnections = element.Element("Connections");
				foreach (var elementConnection in elementConnections.Elements("Connection")) {
					Mission.GraphGrammarConnection connection = new Mission.GraphGrammarConnection();

					connection.ID = new Guid(elementConnection.Attribute("id").Value);
					connection.AlphabetID = new Guid(elementConnection.Element("AlphabetID").Value);
					connection.Name = elementConnection.Element("Name").Value;
					connection.Description = elementConnection.Element("Description").Value;
					connection.Requirement = (Mission.ConnectionType) Enum.Parse(typeof(Mission.ConnectionType), elementConnection.Element("Requirement").Value);
					connection.Arrow = (Mission.ConnectionArrowType) Enum.Parse(typeof(Mission.ConnectionArrowType), elementConnection.Element("ArrowType").Value);
					connection.StartSelected = bool.Parse(elementConnection.Element("StartSelected").Value);
					connection.EndSelected = bool.Parse(elementConnection.Element("EndSelected").Value);

					XElement elementScope = elementConnection.Element("Scope");
					connection.StartpointScope = UnserializeRect(elementScope.Element("Startpoint"));
					connection.EndpointScope = UnserializeRect(elementScope.Element("Endpoint"));

					XElement elementSymbolColor = elementConnection.Element("SymbolColor");
					connection.OutlineColor = UnserializeColor(elementSymbolColor.Element("Outline"));

					connections.Add(connection);
				}
				return connections;
			}
			// Unserialize rect
			private static Rect UnserializeRect(XElement element) {
				return new Rect(float.Parse(element.Element("X").Value),
					float.Parse(element.Element("Y").Value),
					float.Parse(element.Element("Width").Value),
					float.Parse(element.Element("Height").Value));
			}
			// Unserialize color
			private static Color UnserializeColor(XElement element) {
				return new Color((float)int.Parse(element.Value.Substring(1, 2), System.Globalization.NumberStyles.AllowHexSpecifier) / 255f,
					(float) int.Parse(element.Value.Substring(3, 2), System.Globalization.NumberStyles.AllowHexSpecifier) / 255f,
					(float) int.Parse(element.Value.Substring(5, 2), System.Globalization.NumberStyles.AllowHexSpecifier) / 255f);
			}
		}
	}
}

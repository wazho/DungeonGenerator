using UnityEngine;
using System.Collections;
using System.Xml.Linq;
using System;

using Mission = MissionGrammarSystem;

namespace DungeonLevel {
	public static class OperateXML {

		// Static method for other file calling.
		public static void SerializeToXml(string path) {
			XDocument xmlDocument = new XDocument();
			SerializeMissionAlphabet(xmlDocument);
			xmlDocument.Save(path);
		}

		// Serialize MissionAlphabet to XML
		private static void SerializeMissionAlphabet(XDocument xmlDocument) {
			XElement elementMissionAlphabet = new XElement("MissionAlphabet");
			elementMissionAlphabet.Add(new XElement("Nodes"), new XElement("Connections"));
			XElement elementNodes = elementMissionAlphabet.Element("Nodes");
			XElement elementConnections = elementMissionAlphabet.Element("Connections");
			// Serialize nodes
			foreach (var node in Mission.Alphabet.Nodes) {
				XElement elementNode = new XElement("Node", new XAttribute("id", node.ID));
				elementNode.Add(new XElement("AlphabetID", node.AlphabetID));
				elementNode.Add(new XElement("Name", node.Name));
				elementNode.Add(new XElement("Abbreviation", node.Abbreviation));
				elementNode.Add(new XElement("Description", node.Description));
				elementNode.Add(new XElement("Terminal", node.Terminal.ToString()));
				elementNode.Add(new XElement("Scope"));
				elementNode.Add(new XElement("SymbolColor"));

				XElement elementScope = elementNode.Element("Scope");
				elementScope.Add(elementRect("Outline", node.OutlineScope));
				elementScope.Add(elementRect("Filled", node.FilledScope));
				elementScope.Add(elementRect("Text", node.TextScope));

				XElement elementSymbolColor = elementNode.Element("SymbolColor");
				elementSymbolColor.Add(elementColor("Outline", node.OutlineColor));
				elementSymbolColor.Add(elementColor("Filled", node.FilledColor));
				elementSymbolColor.Add(elementColor("Text", node.TextColor));

				elementNode.Add(new XElement("StickiedConnections"));
				XElement elementStickiedConnection = elementNode.Element("StickiedConnections");
				foreach (var guid in node.StickiedConnectionsGuid) {
					elementStickiedConnection.Add(new XElement(guid.ToString()));
				}
				elementNodes.Add(elementNode);
			}
			// Serialize connections
			foreach (var connection in Mission.Alphabet.Connections) {
				XElement elementConnection = new XElement("Connection", new XAttribute("id", connection.ID));
				elementConnection.Add(new XElement("AlphabetID", connection.AlphabetID));
				elementConnection.Add(new XElement("Name", connection.Name));
				elementConnection.Add(new XElement("Description", connection.Description));
				elementConnection.Add(new XElement("Requirement", connection.Requirement));
				elementConnection.Add(new XElement("ArrowType", connection.Arrow.ToString()));
				elementConnection.Add(new XElement("Scope"));
				elementConnection.Add(new XElement("SymbolColor"));
				
				XElement elementScope = elementConnection.Element("Scope");
				elementScope.Add(elementRect("Startpoint", connection.StartpointScope));
				elementScope.Add(elementRect("Endpoint", connection.EndpointScope));

				XElement elementSymbolColor = elementConnection.Element("SymbolColor");
				elementSymbolColor.Add(elementColor("Outline", connection.OutlineColor));

				elementConnection.Add(new XElement("StartpointStickyOn", connection.StartpointStickyOn == null ? "" : connection.StartpointStickyOn.ID.ToString()));
				elementConnection.Add(new XElement("EndpointStickyOn", connection.EndpointStickyOn == null ? "" : connection.EndpointStickyOn.ID.ToString()));
				elementConnection.Add(new XElement("StartSelected", connection.StartSelected.ToString()));
				elementConnection.Add(new XElement("EndSelected", connection.EndSelected.ToString()));
				elementConnections.Add(elementConnection);
			}
			xmlDocument.Add(elementMissionAlphabet);
		}
		// Serialize rect
		private static XElement elementRect(string name, Rect rect) {
			XElement element = new XElement(name);
			element.Add(new XElement("X", rect.x),
				new XElement("Y", rect.y),
				new XElement("Width", rect.width),
				new XElement("Height", rect.height));
			return element;
		}
		// Serialize color
		private static XElement elementColor(string name, Color color) {
			return new XElement(name, "#" + ( (byte) ( color.r * 255 ) ).ToString("X2") + ( (byte) ( color.g * 255 ) ).ToString("X2") + ( (byte) ( color.b * 255 ) ).ToString("X2"));
		}
	}
}

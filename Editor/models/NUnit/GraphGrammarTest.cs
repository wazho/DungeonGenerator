namespace MissionGrammarSystem {
	using NUnit.Framework;
	[TestFixture]
	[Category("MissionGrammarSystem")]
	public class GraphGrammarTest {
		GraphGrammar testGraph;
		[SetUp]
		public void Initialize() {
			testGraph = new GraphGrammar();
		}
		[TearDown]
		public void Dispose() {
			testGraph = null;
		}
		[Test]
		public void TouchedSymbolTest() {
			GraphGrammarNode node0_0 = new GraphGrammarNode() { Position = new UnityEngine.Vector2(0, 0) };
			GraphGrammarNode node2000_3000 = new GraphGrammarNode() { Position = new UnityEngine.Vector2(2000, 3000) };
			GraphGrammarNode nodeN100_N200 = new GraphGrammarNode() { Position = new UnityEngine.Vector2(-100, -200) };
			testGraph.Nodes.Add(node0_0);
			testGraph.Nodes.Add(node2000_3000);
			testGraph.Nodes.Add(nodeN100_N200);
			// Test origin.
			testGraph.TouchedSymbol(new UnityEngine.Vector2(0, 0));
			Assert.AreEqual(node0_0, testGraph.SelectedSymbol);
			// Test boundary.
			testGraph.TouchedSymbol(new UnityEngine.Vector2(2000, 3000));
			Assert.AreEqual(node2000_3000, testGraph.SelectedSymbol);
			// Test negative.
			testGraph.TouchedSymbol(new UnityEngine.Vector2(-100, -200));
			Assert.AreEqual(nodeN100_N200, testGraph.SelectedSymbol);
			// Test null.
			testGraph.TouchedSymbol(new UnityEngine.Vector2(987, 654));
			Assert.AreEqual(null, testGraph.SelectedSymbol);
		}
	}
}
namespace MissionGrammarSystem {
	using NUnit.Framework;
	[TestFixture]
	[Category("MissionGrammarSystem")]
	public class ValidationSystemTest: ValidationSystem {
		MissionRule testRule;
		[SetUp]
		public void Initialize() {
			testRule = new MissionRule();
			testRule.SourceRule = new GraphGrammar();
			testRule.ReplacementRule = new GraphGrammar();
		}
		[TearDown]
		public void Dispose() {
			testRule = null;
		}
		[Test]
		public void ValidateLeftMoreThanRightTest() {
			testRule.SourceRule.AddNode(new GraphGrammarNode());
			// Source: 1, Replacement: 0.
			Assert.AreEqual(false, ValidateLeftMoreThanRight(testRule, testRule.SourceRule));
			// Source: 1, Replacement: 1.
			testRule.ReplacementRule.AddNode(new GraphGrammarNode());
			Assert.AreEqual(true, ValidateLeftMoreThanRight(testRule, testRule.SourceRule));
			// Source: 1, Replacement: 2.
			testRule.ReplacementRule.AddNode(new GraphGrammarNode());
			Assert.AreEqual(true, ValidateLeftMoreThanRight(testRule, testRule.SourceRule));
		}
		[Test]
		[Ignore("Ignore no.2.")]
		public void ValidateEmptyLeftTest() {
			// Source: 0.
			Assert.AreEqual(false, ValidateEmptyLeft(testRule, testRule.SourceRule));
			// Source: 1.
			testRule.SourceRule.AddNode(new GraphGrammarNode());
			Assert.AreEqual(true, ValidateEmptyLeft(testRule, testRule.SourceRule));
			// Source: 2.
			testRule.SourceRule.AddNode(new GraphGrammarNode());
			Assert.AreEqual(true, ValidateEmptyLeft(testRule, testRule.SourceRule));
		}
	}
}
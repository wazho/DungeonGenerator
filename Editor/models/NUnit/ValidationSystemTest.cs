namespace MissionGrammarSystem {
	using NUnit.Framework;
	[TestFixture]
	public class ValidationSystemTest: ValidationSystem {
		[Test]
		public void ValidateLeftMoreThanRightTest() {
			MissionRule testRule = new MissionRule();
			testRule.SourceRule = new GraphGrammar();
			testRule.SourceRule.AddNode(new GraphGrammarNode());
			testRule.ReplacementRule = new GraphGrammar();
			// Source: 1, Replacement: 0.
			Assert.AreEqual(ValidateLeftMoreThanRight(testRule, testRule.ReplacementRule), false);
			// Source: 1, Replacement: 1.
			testRule.ReplacementRule.AddNode(new GraphGrammarNode());
			Assert.AreEqual(ValidateLeftMoreThanRight(testRule, testRule.SourceRule), true);
			// Source: 1, Replacement: 2.
			testRule.ReplacementRule.AddNode(new GraphGrammarNode());
			Assert.AreEqual(ValidateLeftMoreThanRight(testRule, testRule.SourceRule), true);
		}
		[Test]
		public void ValidateEmptyLeftTest() {
			MissionRule testRule = new MissionRule();
			testRule.SourceRule = new GraphGrammar();
			// Source: 0.
			Assert.AreEqual(ValidateEmptyLeft(testRule, testRule.SourceRule), false);
			// Source: 1.
			testRule.SourceRule.AddNode(new GraphGrammarNode());
			Assert.AreEqual(ValidateEmptyLeft(testRule, testRule.SourceRule), true);
			// Source: 2.
			testRule.SourceRule.AddNode(new GraphGrammarNode());
			Assert.AreEqual(ValidateEmptyLeft(testRule, testRule.SourceRule), true);
		}
	}
}
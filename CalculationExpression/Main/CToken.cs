using Superpower.Display;

namespace CalculationExpression.Main {
	public enum CToken {
		[Token(Category = "Number", Example = "-1234.567")]
		Number,

		[Token(Category = "ExpressionGroup", Example = "(")]
		LeftParentheses,

		[Token(Category = "ExpressionGroup", Example = ")")]
		RightParentheses,

		[Token(Category = "ExpressionDelimiter", Example = "{", Description = "Whole expression is {MonsterPushUps200.MainScore} or {MonsterPushUps200.TieBreaker}")]
		LeftBracket,

		[Token(Category = "ExpressionDelimiter", Example = "}", Description = "Whole expression is {MonsterPushUps200.MainScore} or {MonsterPushUps200.TieBreaker}")]
		RightBracket,

		[Token(Category = "Variable", Example = "MonsterPushUps200", Description = "Variable (workout alias, score type)")]
		Variable,

		[Token(Category = "MemberAccessOperator", Example = ".", Description = "Member access operator")]
		Dot,

		[Token(Category = "Operator", Example = "+")]
		Plus,

		[Token(Category = "Operator", Example = "-")]
		Minus,

		[Token(Category = "Operator", Example = "*")]
		Asterisk,

		[Token(Category = "Operator", Example = "/")]
		Slash
	}
}
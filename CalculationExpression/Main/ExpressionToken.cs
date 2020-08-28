using Superpower.Display;

namespace CalculationExpression.Main {
	public enum ExpressionToken {
		[Token(Category = "Number", Example = "-1234.567")]
		Number,

		[Token(Category = "ExpressionGroup", Example = "(")]
		LeftParentheses,

		[Token(Category = "ExpressionGroup", Example = ")")]
		RightParentheses,

		[Token(Category = "WorkoutScoreReference", Example = "{MonsterPushUps200.MainScore}")]
		WorkoutScoreReference,

		[Token(Category = "Operator", Example = "+")]
		Plus,

		[Token(Category = "Operator", Example = "-")]
		Minus,

		[Token(Category = "Operator", Example = "*")]
		Multiply,

		[Token(Category = "Operator", Example = "/")]
		Divide
	}
}
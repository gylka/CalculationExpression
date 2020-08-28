using CalculationExpression.Main.Expressions;
using Superpower;
using Superpower.Parsers;

namespace CalculationExpression.Main {
	public static class ExpressionParser {
		public static TokenListParser<ExpressionToken, Expression> Expression => Parse.Chain(Add.Or(Subtract), HighPriorityOperation, BinaryExpression.Create);

		public static TokenListParser<ExpressionToken, Expression> Number =>
			Token.EqualTo(ExpressionToken.Number)
				.Apply(Numerics.DecimalDecimal)
				.Select(v => (Expression) new NumberExpression(v));

		private static TokenListParser<ExpressionToken, OperationType> Add => Token.EqualTo(ExpressionToken.Plus).Value(OperationType.Add);
		private static TokenListParser<ExpressionToken, OperationType> Subtract => Token.EqualTo(ExpressionToken.Minus).Value(OperationType.Subtract);
		private static TokenListParser<ExpressionToken, OperationType> Multiply => Token.EqualTo(ExpressionToken.Multiply).Value(OperationType.Multiply);
		private static TokenListParser<ExpressionToken, OperationType> Divide => Token.EqualTo(ExpressionToken.Divide).Value(OperationType.Divide);

		private static TokenListParser<ExpressionToken, Expression> Factor =>
			(
				from lparen in Token.EqualTo(ExpressionToken.LeftParentheses)
				from expr in Parse.Ref(() => Expression)
				from rparen in Token.EqualTo(ExpressionToken.RightParentheses)
				select expr
			)
			.Or(Number);

		private static TokenListParser<ExpressionToken, Expression> HighPriorityOperation => Parse.Chain(Multiply.Or(Divide), Factor, BinaryExpression.Create);
	}
}
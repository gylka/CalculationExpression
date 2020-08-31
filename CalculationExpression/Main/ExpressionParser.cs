using System.Text.RegularExpressions;
using CalculationExpression.Main.Expressions;
using CalculationExpression.Main.Models;
using Superpower;
using Superpower.Parsers;

namespace CalculationExpression.Main {
	public static class ExpressionParser {
		public static TokenListParser<CToken, Expression> Expression => LowPriorityOperation;

		public static TokenListParser<CToken, Expression> Number =>
			Token.EqualTo(CToken.Number)
				.Apply(Numerics.DecimalDecimal)
				.Select(v => (Expression) NumberExpression.Of(v));

		private static TokenListParser<CToken, OperationType> Add => Token.EqualTo(CToken.Plus).Value(OperationType.Add);
		private static TokenListParser<CToken, OperationType> Subtract => Token.EqualTo(CToken.Minus).Value(OperationType.Subtract);
		private static TokenListParser<CToken, OperationType> Multiply => Token.EqualTo(CToken.Asterisk).Value(OperationType.Multiply);
		private static TokenListParser<CToken, OperationType> Divide => Token.EqualTo(CToken.Slash).Value(OperationType.Divide);

		private static TokenListParser<CToken, Expression> OperationBlock =>
			(
				from lparen in Token.EqualTo(CToken.LeftParentheses)
				from expr in Parse.Ref(() => Expression)
				from rparen in Token.EqualTo(CToken.RightParentheses)
				select expr
			)
			.Or(Number)
			.Or(WorkoutScoreReference);

		private static TokenListParser<CToken, Expression> HighPriorityOperation => Parse.Chain(Multiply.Or(Divide), OperationBlock, BinaryExpression.Create);

		private static TokenListParser<CToken, Expression> LowPriorityOperation => Parse.Chain(Add.Or(Subtract), HighPriorityOperation, BinaryExpression.Create);

		internal static TokenListParser<CToken, Expression> WorkoutScoreReference =>
			from _ in Token.EqualTo(CToken.LeftBracket)
			from workoutAlias in Variable
			from __ in Token.EqualTo(CToken.Dot)
			from scoreType in ScoreType
			from ___ in Token.EqualTo(CToken.RightBracket)
			select (Expression) ScoreReferenceExpression.Create(ScoreReference.To(workoutAlias, scoreType));

		internal static TokenListParser<CToken, string> Variable =>
			Token.EqualTo(CToken.Variable)
				.Apply(Span.Regex("^[a-zA-Z]{1}\\w*$", RegexOptions.Singleline))
				.Select(s => s.ToStringValue());
		
		internal static TokenListParser<CToken, ScoreType> ScoreType =>
			Token.EqualTo(CToken.Variable)
				.Apply(Lexer.ScoreType)
				.Select(s => s);
	}
}
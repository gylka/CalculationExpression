using System;
using System.Net.Mime;
using CalculationExpression.Main.Models;
using Superpower;
using Superpower.Model;
using Superpower.Parsers;
using Superpower.Tokenizers;

namespace CalculationExpression.Main {
	public static class Lexer {
		public static Tokenizer<ExpressionToken> Instance => new TokenizerBuilder<ExpressionToken>()
			.Match(Character.EqualTo('('), ExpressionToken.LeftParentheses)
			.Match(Character.EqualTo(')'), ExpressionToken.RightParentheses)
			.Match(Character.EqualTo('+'), ExpressionToken.Plus)
			.Match(Numerics.Decimal, ExpressionToken.Number)
			.Match(Character.EqualTo('-'), ExpressionToken.Minus)
			.Match(Character.EqualTo('*'), ExpressionToken.Multiply)
			.Match(Character.EqualTo('/'), ExpressionToken.Divide)
			.Match(WorkoutScoreEnclosed, ExpressionToken.WorkoutScoreReference)
			.Ignore(Span.WhiteSpace)
			.Build();
		
		public static TextParser<ScoreReference> WorkoutScore =>
			from alias in Character.Letter.AtLeastOnce()
				.Then(c => Character.LetterOrDigit.Many())
			from dot in Character.EqualTo('.')
			from scoreType in Character.LetterOrDigit.AtLeastOnce()
			select new ScoreReference(new string(alias), (ScoreType) Enum.Parse(typeof(ScoreType), new string(scoreType)));
		
		public static TextParser<ScoreReference> WorkoutScoreEnclosed =>
			from start in Character.EqualTo('{')
			from alias in Identifier
			from _ in Character.EqualTo('.')
			from scoreType in ScoreType
			from end in Character.EqualTo('}')  
			select new ScoreReference(new string(alias), scoreType);

		private static TextParser<char[]> Identifier
			=> Character.Letter.Then(c => Character.LetterOrDigit.Many());

		private static TextParser<ScoreType> ScoreType =>
			Span.EqualTo("MainScore")
				.Or(Span.EqualTo("TieBreaker"))
				.Select(s => (ScoreType) Enum.Parse(typeof(ScoreType), s.ToStringValue()));
	}
}
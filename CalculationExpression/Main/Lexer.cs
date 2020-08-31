using System;
using CalculationExpression.Main.Models;
using Superpower;
using Superpower.Parsers;
using Superpower.Tokenizers;

namespace CalculationExpression.Main {
	public static class Lexer {
		public static Tokenizer<CToken> Instance => new TokenizerBuilder<CToken>()
			.Match(Numerics.Decimal, CToken.Number)
			.Match(Character.EqualTo('('), CToken.LeftParentheses)
			.Match(Character.EqualTo(')'), CToken.RightParentheses)
			.Match(Character.EqualTo('+'), CToken.Plus)
			.Match(Character.EqualTo('-'), CToken.Minus)
			.Match(Character.EqualTo('*'), CToken.Asterisk)
			.Match(Character.EqualTo('/'), CToken.Slash)
			.Match(Character.EqualTo('{'), CToken.LeftBracket)
			.Match(Character.EqualTo('}'), CToken.RightBracket)
			.Match(Variable, CToken.Variable)
			.Match(Character.EqualTo('.'), CToken.Dot)
			.Ignore(Span.WhiteSpace)
			.Build();

		public static TextParser<ScoreType> ScoreType =>
			Span.EqualTo("MainScore")
				.Try()
				.Or(Span.EqualTo("TieBreaker"))
				.Select(s => (ScoreType) Enum.Parse(typeof(ScoreType), s.ToStringValue()));

		internal static TextParser<string> Variable =>
			from s in Character.Matching(c => char.IsDigit(c) || char.IsLetter(c) || c == '_', "digit or letter or underscore").AtLeastOnce()
			select new string(s);
	}
}
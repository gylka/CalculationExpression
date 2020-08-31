using System;
using System.Linq;
using CalculationExpression.Main.Models;
using Superpower;
using Superpower.Parsers;

namespace CalculationExpression.Main {
	public static class TextParsers {
		// public static TextParser<ScoreReference> WorkoutScoreParser =>
		// 	from alias in Character.Letter.AtLeastOnce()
		// 		.Then(c => Character.LetterOrDigit.Many())
		// 	from dot in Character.EqualTo('.')
		// 	from scoreType in Character.LetterOrDigit.AtLeastOnce()
		// 	select new ScoreReference(new string(alias), (ScoreType) Enum.Parse(typeof(ScoreType), new string(scoreType)));
		//
		// public static TokenListParser<CToken, ScoreReference> WorkoutScoreParser =>
		// 	from _ in Token.EqualTo(CToken.LeftBracket)
		// 	from workoutAlias in Token.EqualTo(CToken.Variable)
		// 	from __ in Token.EqualTo(CToken.Dot)
		// 	from scoreType in Token.EqualTo(CToken.Variable)
		// 	from ___ in Token.EqualTo(CToken.RightBracket)
		// 	select new ScoreReference(workoutAlias.ToStringValue(), (ScoreType) Enum.Parse(typeof(ScoreType), scoreType.ToStringValue()));
		//

		// internal static TextParser<string> Variable =>
		// 	from start in Character.Letter
		// 	from end in Character.Matching(c => char.IsDigit(c) || char.IsLetter(c) || c == '_', "digit or letter or underscore").Many()
		// 	select new string(new[] {start}.Concat(end).ToArray());
	}
}
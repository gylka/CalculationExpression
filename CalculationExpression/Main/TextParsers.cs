using System;
using CalculationExpression.Main.Models;
using Superpower;
using Superpower.Parsers;

namespace CalculationExpression.Main
{
	public static class TextParsers
	{
		public static TextParser<ScoreReference> WorkoutScoreParser =>
			from alias in Character.Letter.AtLeastOnce()
				.Then(c => Character.LetterOrDigit.Many())
			from dot in Character.EqualTo('.')
			from scoreType in Character.LetterOrDigit.AtLeastOnce()
			select new ScoreReference(new string(alias), (ScoreType) Enum.Parse(typeof(ScoreType), new string(scoreType)));
	}
}
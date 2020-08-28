using System.Diagnostics;

namespace CalculationExpression.Main.Models
{
	[DebuggerDisplay("{ToString(),nq}")]
	public class ScoreReference
	{
		public string WorkoutAlias { get; }
		public ScoreType ScoreType { get; }

		public ScoreReference(string workoutAlias, ScoreType scoreType)
		{
			WorkoutAlias = workoutAlias;
			ScoreType = scoreType;
		}
		
		public static ScoreReference To(string workoutAlias, ScoreType scoreType) => new ScoreReference(workoutAlias, scoreType);

		public override string ToString() => $"{WorkoutAlias}.{ScoreType}";
	}

	public enum ScoreType
	{
		MainScore, TieBreaker
	}
}
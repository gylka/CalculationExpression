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

		protected bool Equals(ScoreReference other) => WorkoutAlias == other.WorkoutAlias && ScoreType == other.ScoreType;

		public override bool Equals(object obj) {
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((ScoreReference) obj);
		}

		public override int GetHashCode() {
			unchecked {
				return ((WorkoutAlias != null ? WorkoutAlias.GetHashCode() : 0) * 397) ^ (int) ScoreType;
			}
		}

		public override string ToString() => $"{WorkoutAlias}.{ScoreType}";
	}


	
	public enum ScoreType
	{
		MainScore, TieBreaker
	}
}
using System.Diagnostics;
using CalculationExpression.Main.Models;

namespace CalculationExpression.Main.Expressions {
	[DebuggerDisplay("{ToDebugString(),nq}")]
	public class ScoreReferenceExpression : Expression {
		public ScoreReference Value { get; }

		public ScoreReferenceExpression(ScoreReference value) {
			Value = value;
		}

		public static ScoreReferenceExpression Create(ScoreReference value) => new ScoreReferenceExpression(value);

		public override bool Equals(Expression other) => other is ScoreReferenceExpression scoreReference && Value.Equals(scoreReference.Value);

		public override int GetHashCode() => Value.GetHashCode();

		internal override string ToDebugString() => $"{{{Value}}}";

		public override string ToString() => $"ScoreReference {{ Value = {Value} }}";
	}
}
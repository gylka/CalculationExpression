using System.Diagnostics;
using System.Globalization;

namespace CalculationExpression.Main.Expressions {
	[DebuggerDisplay("{ToDebugString(),nq}")]
	public class NumberExpression : Expression {
		public decimal Value { get; }

		private NumberExpression(decimal value) {
			Value = value;
		}

		public static NumberExpression Of(decimal value) => new NumberExpression(value);

		public override bool Equals(Expression other) => other is NumberExpression numberExpression && Value == numberExpression.Value;

		public override int GetHashCode() => Value.GetHashCode();

		internal override string ToDebugString() => Value.ToString(CultureInfo.InvariantCulture);

		public override string ToString() => $"Number {{ Value = {Value} }}";
	}
}
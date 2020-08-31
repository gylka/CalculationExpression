using System;
using System.Diagnostics;

namespace CalculationExpression.Main.Expressions {
	[DebuggerDisplay("{ToDebugString(),nq}")]
	public abstract class Expression : IEquatable<Expression> {
		internal abstract string ToDebugString();

		public abstract bool Equals(Expression other);

		public override bool Equals(object obj) {
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType())
				return false;
			return Equals((Expression) obj);
		}

		public abstract int GetHashCode();
	}
}
using System;
using System.Diagnostics;

namespace CalculationExpression.Main.Expressions {
	[DebuggerDisplay("{ToDebugString(),nq}")]
	public class BinaryExpression : Expression {
		public OperationType Type { get; }
		public Expression Left { get; }
		public Expression Right { get; }

		public BinaryExpression(OperationType type, Expression left, Expression right) {
			Type = type;
			Left = left;
			Right = right;
		}

		public static BinaryExpression Create(OperationType type, Expression left, Expression right) => new BinaryExpression(type, left, right);

		public override bool Equals(Expression other) {
			if (!(other is BinaryExpression another))
				return false;
			return Type.Equals(another.Type) && Left.Equals(another.Left) && Right.Equals(another.Right);
		}

		public override int GetHashCode() {
			unchecked {
				var hashCode = (int) Type;
				hashCode = (hashCode * 397) ^ (Left != null ? Left.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (Right != null ? Right.GetHashCode() : 0);
				return hashCode;
			}
		}

		internal override string ToDebugString() {
			switch (Type) {
				case OperationType.Add:
					return $"({Left.ToDebugString()} + {Right.ToDebugString()})";
				case OperationType.Subtract:
					return $"({Left.ToDebugString()} - {Right.ToDebugString()})";
				case OperationType.Multiply:
					return $"({Left.ToDebugString()} * {Right.ToDebugString()})";
				case OperationType.Divide:
					return $"({Left.ToDebugString()} / {Right.ToDebugString()})";
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		public override string ToString() => $"BinaryExpression [\n{Type},\n{Left}\n{Right}\n]";
	}


	public enum OperationType {
		Add,
		Subtract,
		Multiply,
		Divide
	}
}
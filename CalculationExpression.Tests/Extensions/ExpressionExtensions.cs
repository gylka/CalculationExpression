using CalculationExpression.Main.Expressions;
using Shouldly;

namespace CalculationExpression.Tests.Extensions
{
	internal static class ExpressionExtensions
	{
		public static void ShouldBeExpression(this Expression expression, Expression expected)
			=> expression.ShouldBe(expected, $"Expected:\n{expected.ToDebugString()}\nBut was:\n{expression.ToDebugString()}");
	}
}
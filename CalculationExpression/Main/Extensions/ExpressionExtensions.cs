using System;
using System.Collections.Generic;
using CalculationExpression.Main.Expressions;

namespace CalculationExpression.Main.Extensions {
	public static class ExpressionExtensions {
		public static ScoreReferenceExpression[] CollectAllScoreReferenceExpressions(this Expression expression)
			=> CollectAllScoreReferenceExpressionsRecursive(expression).ToArray();

		private static List<ScoreReferenceExpression> CollectAllScoreReferenceExpressionsRecursive(this Expression expression) {
			var result = new List<ScoreReferenceExpression>();
			if (expression is NumberExpression)
				return result;
			if (expression is ScoreReferenceExpression scoreReferenceExpression) {
				result.Add(scoreReferenceExpression);
				return result;
			}

			if (expression is BinaryExpression binaryExpression) {
				result.AddRange(binaryExpression.Left.CollectAllScoreReferenceExpressionsRecursive());
				result.AddRange(binaryExpression.Right.CollectAllScoreReferenceExpressionsRecursive());
				return result;
			}

			throw new InvalidOperationException($"Unknown expression type '{expression.GetType()}': '{expression}'");
		}
	}
}
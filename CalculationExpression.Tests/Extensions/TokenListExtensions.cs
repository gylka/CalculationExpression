using System;
using System.Collections.Generic;
using System.Linq;
using Superpower.Model;

namespace CalculationExpression.Tests.Extensions
{
	internal static class TokenListExtensions
	{
		public static bool ShouldHaveSameTokens<T>(this TokenList<T> actual, IEnumerable<Token<T>> expected)
		{
			if (expected is null)
				throw new ArgumentNullException();
			return actual.SequenceEqual(expected, new TokenEqualityComparer<T>());
		}
	}

	internal class TokenEqualityComparer<T> : IEqualityComparer<Token<T>>
	{
		public bool Equals(Token<T> x, Token<T> y)
		{
			return Equals(x.Kind, y.Kind) && x.Span.Equals(y.Span) && x.Position.Equals(y.Position) && x.HasValue == y.HasValue;
		}

		public int GetHashCode(Token<T> obj)
		{
			unchecked
			{
				var hashCode = EqualityComparer<T>.Default.GetHashCode(obj.Kind);
				hashCode = (hashCode * 397) ^ obj.Span.GetHashCode();
				hashCode = (hashCode * 397) ^ obj.Position.GetHashCode();
				hashCode = (hashCode * 397) ^ obj.HasValue.GetHashCode();
				return hashCode;
			}
		}
	}
}
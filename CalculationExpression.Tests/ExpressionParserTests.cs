using System.Collections.Generic;
using CalculationExpression.Main;
using CalculationExpression.Main.Expressions;
using CalculationExpression.Tests.Extensions;
using Shouldly;
using Xunit;

namespace CalculationExpression.Tests
{
	public class ExpressionParserTests
	{
		[Theory]
		[InlineData("1", 1)]
		[InlineData("1234.6789", 1234.6789)]
		[InlineData("-1234.004", -1234.004)] 
		[InlineData("-1234.00", -1234)] 
		public void ExpressionParser_parses_simple_number(string input, decimal expectedDecimal)
		{
			var tokens = Lexer.Instance.Tokenize(input);
			var expression = ExpressionParser.Expression.Invoke(tokens);
			expression.Value.ShouldBeOfType<NumberExpression>();
			((NumberExpression)expression.Value).Value.ShouldBe(expectedDecimal);
		}

		public static IEnumerable<object[]> SimpleBinaryExpressionData => new[]
		{
			new object[] {"1 + 2", BinaryExpression.Create(OperationType.Add, NumberExpression.Of(1), NumberExpression.Of(2))},
			new object[] {"1 - 2", BinaryExpression.Create(OperationType.Subtract, NumberExpression.Of(1), NumberExpression.Of(2))},
			new object[] {"123.456 * 245.90", BinaryExpression.Create(OperationType.Multiply, NumberExpression.Of(123.456m), NumberExpression.Of(245.9m))},
			new object[] {"1 / -2.3", BinaryExpression.Create(OperationType.Divide, NumberExpression.Of(1), NumberExpression.Of(-2.3m))},
			new object[] {"1.2 + -2.4", BinaryExpression.Create(OperationType.Add, NumberExpression.Of(1.2m), NumberExpression.Of(-2.4m))},
			new object[] {"1.2 - -2.4", BinaryExpression.Create(OperationType.Subtract, NumberExpression.Of(1.2m), NumberExpression.Of(-2.4m))}
		};
		
		[Theory]
		[MemberData(nameof(SimpleBinaryExpressionData))]
		public void ExpressionParser_parses_simple_binary_expressions(string input, BinaryExpression expected)
		{
			var tokens = Lexer.Instance.Tokenize(input);
			var expression = ExpressionParser.Expression.Invoke(tokens);
			expression.Value.ShouldBeOfType<BinaryExpression>();
			expression.Value.ShouldBeExpression(expected);
		}

		public static IEnumerable<object[]> ComplexSequenceExpressionData => new[]
		{
			new object[] {"1 + 5 - 4 - 2", BinaryExpression.Create(OperationType.Subtract,
				BinaryExpression.Create(OperationType.Subtract,
					BinaryExpression.Create(OperationType.Add, NumberExpression.Of(1), NumberExpression.Of(5)),
					NumberExpression.Of(4)),
				NumberExpression.Of(2)) },
			new object[] {"-2.34 - -3.4 + -23", BinaryExpression.Create(OperationType.Add,
				BinaryExpression.Create(OperationType.Subtract, NumberExpression.Of(-2.34m), NumberExpression.Of(-3.4m)),
				NumberExpression.Of(-23)) } 
		};
		
		[Theory]
		[MemberData(nameof(ComplexSequenceExpressionData))]
		public void ExpressionParser_parses_complex_binary_expressions(string input, BinaryExpression expected)
		{
			var tokens = Lexer.Instance.Tokenize(input);
			var expression = ExpressionParser.Expression.Invoke(tokens);
			expression.Value.ShouldBeOfType<BinaryExpression>();
			expression.Value.ShouldBeExpression(expected);
		}

		public static IEnumerable<object[]> ComplexSequenceExpressionWithPriorityData => new[]
		{
			new object[] {"1 + 2 * 3 - 4", BinaryExpression.Create(OperationType.Subtract,
				BinaryExpression.Create(OperationType.Add, 
					NumberExpression.Of(1), 
					BinaryExpression.Create(OperationType.Multiply, NumberExpression.Of(2),NumberExpression.Of(3))),
				NumberExpression.Of(4)
				) }, 
			new object[] {"1 + ( 5 - 4 ) - 2", BinaryExpression.Create(OperationType.Subtract,
				BinaryExpression.Create(OperationType.Add,
					NumberExpression.Of(1),
					BinaryExpression.Create(OperationType.Subtract, NumberExpression.Of(5), NumberExpression.Of(4))),
				NumberExpression.Of(2)) },
			new object[] {"-2.34 - (-3.4 + -23)", BinaryExpression.Create(OperationType.Subtract,
				NumberExpression.Of(-2.34m), 
				BinaryExpression.Create(OperationType.Add, NumberExpression.Of(-3.4m), NumberExpression.Of(-23m))) }
		};
		
		[Theory]
		[MemberData(nameof(ComplexSequenceExpressionWithPriorityData))]
		public void ExpressionParser_parses_complex_binary_expression_and_respects_priority(string input, BinaryExpression expected)
		{
			var tokens = Lexer.Instance.Tokenize(input);
			var expression = ExpressionParser.Expression.Invoke(tokens);
			expression.Value.ShouldBeOfType<BinaryExpression>();
			expression.Value.ShouldBeExpression(expected);
		}
	}
}
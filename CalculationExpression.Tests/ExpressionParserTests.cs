using System.Collections.Generic;
using CalculationExpression.Main;
using CalculationExpression.Main.Expressions;
using CalculationExpression.Main.Models;
using CalculationExpression.Tests.Extensions;
using Shouldly;
using Xunit;

namespace CalculationExpression.Tests {
	public class ExpressionParserTests {
		[Theory]
		[InlineData("1", 1)]
		[InlineData("1234.6789", 1234.6789)]
		[InlineData("-1234.004", -1234.004)]
		[InlineData("-1234.00", -1234)]
		public void ExpressionParser_parses_simple_number(string input, decimal expectedDecimal) {
			var tokens = Lexer.Instance.Tokenize(input);
			var result = ExpressionParser.Expression.Invoke(tokens);
			result.Value.ShouldBeOfType<NumberExpression>();
			((NumberExpression) result.Value).Value.ShouldBe(expectedDecimal);
		}

		public static IEnumerable<object[]> SimpleBinaryExpressionData => new[] {
			new object[] {"1 + 2", BinaryExpression.Create(OperationType.Add, NumberExpression.Of(1), NumberExpression.Of(2))},
			new object[] {"1 - 2", BinaryExpression.Create(OperationType.Subtract, NumberExpression.Of(1), NumberExpression.Of(2))},
			new object[] {"123.456 * 245.90", BinaryExpression.Create(OperationType.Multiply, NumberExpression.Of(123.456m), NumberExpression.Of(245.9m))},
			new object[] {"1 / -2.3", BinaryExpression.Create(OperationType.Divide, NumberExpression.Of(1), NumberExpression.Of(-2.3m))},
			new object[] {"1.2 + -2.4", BinaryExpression.Create(OperationType.Add, NumberExpression.Of(1.2m), NumberExpression.Of(-2.4m))},
			new object[] {"1.2 - -2.4", BinaryExpression.Create(OperationType.Subtract, NumberExpression.Of(1.2m), NumberExpression.Of(-2.4m))},
			new object[] {"1.2 + {Aa.MainScore}", BinaryExpression.Create(OperationType.Add,
				NumberExpression.Of(1.2m), 
				ScoreReferenceExpression.Create(ScoreReference.To("Aa", ScoreType.MainScore)))},
			new object[] {"{Aa.MainScore} - -3.4", BinaryExpression.Create(OperationType.Subtract,
				ScoreReferenceExpression.Create(ScoreReference.To("Aa", ScoreType.MainScore)),
				NumberExpression.Of(-3.4m))},
			new object[] {"{Aa_234.TieBreaker} * -3.4", BinaryExpression.Create(OperationType.Multiply,
				ScoreReferenceExpression.Create(ScoreReference.To("Aa_234", ScoreType.TieBreaker)),
				NumberExpression.Of(-3.4m))}
		};

		[Theory]
		[MemberData(nameof(SimpleBinaryExpressionData))]
		public void ExpressionParser_parses_simple_binary_expressions(string input, BinaryExpression expected) {
			var tokens = Lexer.Instance.Tokenize(input);
			var result = ExpressionParser.Expression.Invoke(tokens);
			result.Value.ShouldBeOfType<BinaryExpression>();
			result.Value.ShouldBeExpression(expected);
		}

		[Theory]
		[InlineData("1 + ")]
		[InlineData("* 4")]
		[InlineData(".44 +")]
		[InlineData("1 / -")]
		public void ExpressionParser_should_not_parse_invalid_simple_expressions(string input) {
			var tokens = Lexer.Instance.Tokenize(input);
			var result = ExpressionParser.Expression.Invoke(tokens);
			result.HasValue.ShouldBeFalse();
		}

		public static IEnumerable<object[]> ComplexSequenceExpressionData => new[] {
			new object[] {
				"1 + 5 - 4 - 2", BinaryExpression.Create(OperationType.Subtract,
					BinaryExpression.Create(OperationType.Subtract,
						BinaryExpression.Create(OperationType.Add, NumberExpression.Of(1), NumberExpression.Of(5)),
						NumberExpression.Of(4)),
					NumberExpression.Of(2))
			},
			new object[] {
				"-2.34 - -3.4 + -23", BinaryExpression.Create(OperationType.Add,
					BinaryExpression.Create(OperationType.Subtract, NumberExpression.Of(-2.34m), NumberExpression.Of(-3.4m)),
					NumberExpression.Of(-23))
			}
		};

		[Theory]
		[MemberData(nameof(ComplexSequenceExpressionData))]
		public void ExpressionParser_parses_complex_binary_expressions(string input, BinaryExpression expected) {
			var tokens = Lexer.Instance.Tokenize(input);
			var result = ExpressionParser.Expression.Invoke(tokens);
			result.Value.ShouldBeOfType<BinaryExpression>();
			result.Value.ShouldBeExpression(expected);
		}

		public static IEnumerable<object[]> ComplexSequenceExpressionWithPriorityData => new[] {
			new object[] {
				"1 + 2 * 3 - 4", BinaryExpression.Create(OperationType.Subtract,
					BinaryExpression.Create(OperationType.Add,
						NumberExpression.Of(1),
						BinaryExpression.Create(OperationType.Multiply, NumberExpression.Of(2), NumberExpression.Of(3))),
					NumberExpression.Of(4)
				)
			},
			new object[] {
				"1 + ( 5 - 4 ) - 2", BinaryExpression.Create(OperationType.Subtract,
					BinaryExpression.Create(OperationType.Add,
						NumberExpression.Of(1),
						BinaryExpression.Create(OperationType.Subtract, NumberExpression.Of(5), NumberExpression.Of(4))),
					NumberExpression.Of(2))
			},
			new object[] {
				"-2.34 - (-3.4 + -23)", BinaryExpression.Create(OperationType.Subtract,
					NumberExpression.Of(-2.34m),
					BinaryExpression.Create(OperationType.Add, NumberExpression.Of(-3.4m), NumberExpression.Of(-23m)))
			},
			new object[] {
				"-2.34 - (-3.4 - {Aa.MainScore} * -23)", BinaryExpression.Create(OperationType.Subtract,
					NumberExpression.Of(-2.34m),
					BinaryExpression.Create(OperationType.Subtract,
						NumberExpression.Of(-3.4m),
						BinaryExpression.Create(OperationType.Multiply,
							ScoreReferenceExpression.Create(ScoreReference.To("Aa", ScoreType.MainScore)),
							NumberExpression.Of(-23))))
			},
			new object[] {
				"-2.34 - ({Aa.TieBreaker} + (-3.4 - {Bb.TieBreaker} * 5))",
				BinaryExpression.Create(OperationType.Subtract,
					NumberExpression.Of(-2.34m),
					BinaryExpression.Create(OperationType.Add,
						ScoreReferenceExpression.Create(ScoreReference.To("Aa", ScoreType.TieBreaker)),
						BinaryExpression.Create(OperationType.Subtract,
							NumberExpression.Of(-3.4m),
							BinaryExpression.Create(OperationType.Multiply,
								ScoreReferenceExpression.Create(ScoreReference.To("Bb", ScoreType.TieBreaker)),
								NumberExpression.Of(5)
								)
							)
						))
			}
		};

		[Theory]
		[MemberData(nameof(ComplexSequenceExpressionWithPriorityData))]
		public void ExpressionParser_parses_complex_binary_expression_and_respects_priority(string input, BinaryExpression expected) {
			var tokens = Lexer.Instance.Tokenize(input);
			var result = ExpressionParser.Expression.Invoke(tokens);
			result.Value.ShouldBeOfType<BinaryExpression>();
			result.Value.ShouldBeExpression(expected);
		}
		
		[Theory]
		[InlineData("{A.MainScore}", "A", ScoreType.MainScore)]
		[InlineData("{Abcd.TieBreaker}", "Abcd", ScoreType.TieBreaker)]
		[InlineData("{ Abcd . TieBreaker } ", "Abcd", ScoreType.TieBreaker)]
		[InlineData("{ Ab12_rr_5.MainScore} ", "Ab12_rr_5", ScoreType.MainScore)]
		public void ExpressionParser_parses_score_reference(string input, string workoutAlias, ScoreType scoreType) {
			var tokens = Lexer.Instance.Tokenize(input);
			var result = ExpressionParser.Expression.Invoke(tokens);
			result.Value.ShouldBeOfType<ScoreReferenceExpression>();
			((ScoreReferenceExpression) result.Value).ShouldBeExpression(ScoreReferenceExpression.Create(ScoreReference.To(workoutAlias, scoreType)));
		}
		
		[Theory]
		[InlineData("{A.MainScore")]
		[InlineData("{Abcd..TieBreaker}")]
		[InlineData("{1A.TieBreaker} ")]
		[InlineData("{_a.MainScore} ")]
		[InlineData("{A}")]
		[InlineData("{Aaaa}")]
		[InlineData("{.Aaaa}")]
		[InlineData(".Aaaa}")]
		[InlineData("{A.A}")]
		[InlineData("{A.Mainscore}")]
		[InlineData("{A.MainScore.B}")]
		[InlineData("{A.MainScore.B}}")]
		[InlineData("A.MainScore.B}}")]
		public void ExpressionParser_does_not_parse_invalid_score_reference(string input) {
			var tokens = Lexer.Instance.Tokenize(input);
			var result = ExpressionParser.Expression.Invoke(tokens);
			result.HasValue.ShouldBeFalse();
		}
	}
}
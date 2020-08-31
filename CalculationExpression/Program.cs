using CalculationExpression.Main;
using CalculationExpression.Main.Extensions;

namespace CalculationExpression
{
	internal class Program
	{
		public static void Main(string[] args)
		{
			var exp = "{Cc.TieBreaker} + (5 + 6) + 1.24 + ( 5 - ({Bb.MainScore} - (555 * {Aa.MainScore}) )) - 75";
			// var tokens = Lexer.Instance.Tokenize(exp);
			// var str = "1 - { Aaaa.Rrr}";
			// var str = "{Aa55a.MainScore";
			var res = Lexer.Instance.Tokenize(exp);
			
			var result = ExpressionParser.Expression.Invoke(res);
			var aaaaaaa = result.Value.CollectAllScoreReferenceExpressions();
			// var tt = "MainScore";
			// var tok = Lexer.Instance.Tokenize(tt);
			// var rrr = ExpressionParser.ScoreType.Invoke(tok);
			var a = 1;
		}
	}
}
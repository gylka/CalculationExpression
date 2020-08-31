using CalculationExpression.Main;

namespace CalculationExpression
{
	internal class Program
	{
		public static void Main(string[] args)
		{
			// var exp = "1.24 + ( 5 - 555 ) - 75";
			// var tokens = Lexer.Instance.Tokenize(exp);
			// var str = "1 - { Aaaa.Rrr}";
			var str = "{Aa55a.MainScore";
			var res = Lexer.Instance.Tokenize(str);
			
			var result = ExpressionParser.Expression.Invoke(res);

			// var tt = "MainScore";
			// var tok = Lexer.Instance.Tokenize(tt);
			// var rrr = ExpressionParser.ScoreType.Invoke(tok);
			var a = 1;
		}
	}
}
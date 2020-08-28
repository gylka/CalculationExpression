using CalculationExpression.Main;
using Superpower.Model;

namespace CalculationExpression
{
	internal class Program
	{
		public static void Main(string[] args)
		{
			// var exp = "1.24 + ( 5 - 555 ) - 75";
			// var tokens = Lexer.Instance.Tokenize(exp);
			var str = "{Aa55a.MainScore}";
			var res = Lexer.WorkoutScoreEnclosed.Invoke(new TextSpan(str));
			
			// var result = ExpressionParser.Expression.Invoke(tokens);

			var a = 1;
		}
	}
}
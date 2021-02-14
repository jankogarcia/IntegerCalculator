using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Linq;

namespace CalculatorBE.Validators
{
    public class ExpressionValidator : IValidator
    {
        private Regex ValidExpression = new Regex(@"^[0-9+\-*\/\s]*$");
        //^[0-9+\-*\/\(\)]*$

        private bool IsValidExpression(string expression)
        {
            return ValidExpression.IsMatch(expression);
        }

        private string GetErrorMessage(string expression)
        {
            var error = expression.FirstOrDefault(v => v < 42 || v == 46 || v == 44 || v > 57);
            return $"error: invalid character: '{error}'";
        }

        public async Task<string> GetErrorMessageAsync(string expression)
        {
            return await Task.Run(() => GetErrorMessage(expression));
        }

        public async Task<bool> IsValidExpressionAsync(string expression)
        {
            return await Task.Run(() => IsValidExpression(expression));
        }
    }
}

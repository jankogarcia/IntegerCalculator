using System;
using System.Data;
using System.Threading.Tasks;

namespace CalculatorBE.Calculators
{
    public class ExpressionCalculator : ICalculator
    {
        public ExpressionCalculator() { }

        private long CalculateExpression(string expression)
        {
            var res = new DataTable().Compute(expression, null);
            return (long)Math.Truncate(Convert.ToDecimal(res));
        }

        public async Task<long> CalculateExpressionAsync(string expression)
        {
            return await Task.Run(() => CalculateExpression(expression));
        }
    }
}

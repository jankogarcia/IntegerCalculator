
using System.Threading.Tasks;

namespace CalculatorBE.Calculators
{
    public interface ICalculator
    {
        Task<long> CalculateExpressionAsync(string expression);
    }
}

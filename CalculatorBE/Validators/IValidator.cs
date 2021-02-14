
using System.Threading.Tasks;

namespace CalculatorBE.Validators
{
    public interface IValidator
    {
        Task<string> GetErrorMessageAsync(string expression);

        Task<bool> IsValidExpressionAsync(string expression);
    }
}

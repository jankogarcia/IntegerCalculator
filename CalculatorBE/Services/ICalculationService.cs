using System.Threading.Tasks;

namespace CalculatorBE.Services
{
    public interface ICalculationService
    {
        Task<string> CalculateExpressionAsync(string expression);
        Task<string> GetErrorInformationAsync(string expression);
        Task<bool> ValidateExpressionAsync(string expression);
        Task RunProcessAsync(string inputPath, string outputpath);
    }
}
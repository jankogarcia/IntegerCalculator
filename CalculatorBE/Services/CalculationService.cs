using System;
using System.Collections.Concurrent;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using CalculatorBE.Calculators;
using CalculatorBE.Validators;

namespace CalculatorBE.Services
{
    public class CalculationService : ICalculationService
    {
        private readonly IValidator _validator;
        private readonly ICalculator _calculator;

        private ConcurrentQueue<string> WorkingQueue = new ConcurrentQueue<string>();
        private ConcurrentQueue<string> ProcessedQueue = new ConcurrentQueue<string>();

        public CalculationService(IValidator validator, ICalculator calculator)
        {
            _validator = validator;
            _calculator = calculator;
        }

        public async Task RunProcessAsync(string inputPath, string outputPath)
        {
            if (string.IsNullOrWhiteSpace(inputPath))
                throw new ArgumentNullException("Argument inputPath can't be empty or null.");

            if (string.IsNullOrWhiteSpace(outputPath))
                throw new ArgumentNullException("Argument outputPath can't be empty or null.");

            if (!File.Exists(inputPath))
                throw new FileNotFoundException($"Specified file '{inputPath}' does not exist.");

            await ReadAndEnqueue(inputPath);
            await DequeueProcessAndEnqueue();
            await DequeueAndWrite(outputPath);
        }


        private async Task ReadAndEnqueue(string inputPath)
        {
            using (var stream = File.OpenText(inputPath))
            {
                var lineToProcess = string.Empty;
                do {
                    lineToProcess = await stream.ReadLineAsync();
                    if(!string.IsNullOrWhiteSpace(lineToProcess))
                        WorkingQueue.Enqueue(lineToProcess);
                }
                while (lineToProcess != null);
            }
        }


        private async Task DequeueAndWrite(string outputPath)
        {
            using (var stream = File.OpenWrite(outputPath))
            {
                var processedLine = string.Empty;
                while (ProcessedQueue.TryDequeue(out processedLine))
                {
                    await stream.WriteAsync(Encoding.ASCII.GetBytes(processedLine), 0, processedLine.Length);
                }
            }
        }


        private async Task DequeueProcessAndEnqueue()
        {
            string expression;
            while (WorkingQueue.TryDequeue(out expression))
            {
                string processedExp;
                if (await ValidateExpressionAsync(expression))
                {
                    processedExp = await CalculateExpressionAsync(expression);
                }
                else
                {
                    processedExp = await GetErrorInformationAsync(expression);
                }
                ProcessedQueue.Enqueue($"{processedExp}{Environment.NewLine}");
            }
        }

        public async Task<string> CalculateExpressionAsync(string expression)
        {
            try
            {
                var result = await _calculator.CalculateExpressionAsync(expression);
                return result.ToString();
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        public async Task<string> GetErrorInformationAsync(string expression)
        {
            try
            {
                return await _validator.GetErrorMessageAsync(expression);
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        public async Task<bool> ValidateExpressionAsync(string expression)
        {
            try 
            {
                return await _validator.IsValidExpressionAsync(expression);
            }
            catch (Exception ex)
            {
                //log some exception?
                return await Task.FromResult(false);
            }
        }
    }
}

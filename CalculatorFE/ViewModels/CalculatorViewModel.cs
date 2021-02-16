using CalculatorFE.Commands;
using CalculatorFE.Models;
using System;
using System.Windows.Input;
using CalculatorBE.Services;
using CalculatorBE.Calculators;
using CalculatorBE.Validators;
using System.Threading.Tasks;

namespace CalculatorFE.ViewModels
{
    public class CalculatorViewModel
    {
        private CalculatorModel _calculatorModel;
        private ICalculationService _service;
        private IValidator _validator;
        private ICalculator _calculator;

        public ICommand EvaluateExpression { get; private set; }
        public ICommand RunExpressionFile { get; private set; }
        public ICommand ShowInputDialog { get; private set; }
        public ICommand ShowOutputDialog { get; private set; }

        public ICommand StartExpressionFile { get; private set; }

        public CalculatorViewModel()
        {
            _calculatorModel = new CalculatorModel()
            {
                Expression = "0",
                InputPath = @"input.txt",
                OutputPath = @"output.txt",
                FileNotification = string.Empty,
                ExpressionNotification = string.Empty
            };

            _validator = new ExpressionValidator();
            _calculator = new ExpressionCalculator();
            _service = new CalculationService(_validator, _calculator);

            EvaluateExpression = new EvaluateExpressionCommand(this);
            RunExpressionFile = new RunExpressionFileCommand(this);
            ShowInputDialog = new ShowInputDialogCommand(this);
            ShowOutputDialog = new ShowOutputDialogCommand(this);

        }

        public CalculatorModel Calculator
        {
            get { return _calculatorModel; }
        }

        public bool CanEvaluateExpression 
        {
            get 
            {
                return _calculatorModel == null 
                    ? false 
                    : !string.IsNullOrWhiteSpace(_calculatorModel.Expression);
            }
        }

        public bool CanEvaluateFile
        {
            get 
            {
                return _calculatorModel == null
                    ? false
                    : !string.IsNullOrWhiteSpace(_calculatorModel.InputPath) && !string.IsNullOrWhiteSpace(_calculatorModel.OutputPath);
            }
        }

        public async Task RunExpression()
        {
            Calculator.ExpressionNotification = string.Empty;
            var expression = _calculatorModel.Expression;
            if (await _service.ValidateExpressionAsync(expression))
            {
                var result = await _service.CalculateExpressionAsync(expression);
                if (Int64.TryParse(result, out long output))
                    Calculator.Expression = output.ToString();
                else
                    Calculator.ExpressionNotification = result;
            }
            else {
                Calculator.ExpressionNotification = await _service.GetErrorInformationAsync(expression);
            }
            
        }

        public async Task RunFileExpression()
        {
            var finishCorrectly = true;
            try
            {
                Calculator.FileNotification = "Working on file...";
                await _service.RunProcessAsync(_calculatorModel.InputPath, _calculatorModel.OutputPath);
            }
            catch (Exception ex)
            {
                finishCorrectly = false;
                Calculator.FileNotification = ex.Message;
            }
            finally
            {
                if(finishCorrectly)
                    Calculator.FileNotification = "Work is done.";
            }
        }

        public void ShowOpenDialog()
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.DefaultExt = ".txt";

            var res = dialog.ShowDialog();
            if (res == true)
            {
                Calculator.InputPath = dialog.FileName;
            }
        }

        public void ShowSaveDialog() 
        {
            var dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.DefaultExt = ".txt";

            var res = dialog.ShowDialog();
            if (res == true)
            {
                Calculator.OutputPath = dialog.FileName;
            }
        }
    }
}

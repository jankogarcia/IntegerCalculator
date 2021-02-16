using CalculatorFE.ViewModels;
using System;
using System.Windows.Input;

namespace CalculatorFE.Commands
{
    internal class EvaluateExpressionCommand : ICommand
    {
        private CalculatorViewModel ViewModel;
        public EvaluateExpressionCommand(CalculatorViewModel viewModel)
        {
            ViewModel = viewModel;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return ViewModel.CanEvaluateExpression;
        }

        public async void Execute(object parameter)
        {
            await ViewModel.RunExpression();
        }
    }
}

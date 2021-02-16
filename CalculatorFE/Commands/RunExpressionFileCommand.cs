using CalculatorFE.ViewModels;
using System;
using System.Windows.Input;

namespace CalculatorFE.Commands
{
    public class RunExpressionFileCommand : ICommand
    {
        private CalculatorViewModel ViewModel;
        public RunExpressionFileCommand(CalculatorViewModel viewModel)
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
            return ViewModel.CanEvaluateFile;
        }

        public async void Execute(object parameter)
        {
            await ViewModel.RunFileExpression();
        }
    }
}

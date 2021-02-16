using CalculatorFE.ViewModels;
using System;
using System.Windows.Input;

namespace CalculatorFE.Commands
{
    public class ShowOutputDialogCommand : ICommand
    {
        private CalculatorViewModel ViewModel;
        public ShowOutputDialogCommand(CalculatorViewModel viewModel)
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
            return true;
        }

        public void Execute(object parameter)
        {
            ViewModel.ShowSaveDialog();
        }
    }
}

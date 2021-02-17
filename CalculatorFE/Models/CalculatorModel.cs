using System;
using System.ComponentModel;

namespace CalculatorFE.Models
{
    public class CalculatorModel : INotifyPropertyChanged
    {
        private string _outputPath;
        private string _inputPath;
        private string _expression;
        private string _fileNotification;
        private string _expressionNotification;

        public event PropertyChangedEventHandler PropertyChanged;

        public string OutputPath 
        {
            get 
            {
                return _outputPath;
            }
            set 
            {
                _outputPath = value;
                PropertyChange("OutputPath");
            }
        }
        public string InputPath 
        {
            get
            {
                return _inputPath;
            }
            set
            {
                _inputPath = value;
                PropertyChange("InputPath");
            }
        }
        public string Expression
        {
            get 
            {
                return _expression; 
            }
            set 
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    _expression = value;
                    PropertyChange("Expression");
                }
            }
        }
        public string Instructions
        {
            get 
            {
                return $"Place an expression with only integer numbers and algebra operators ['+','-','*' and '*'] {Environment.NewLine} e.g. 1+2+3";
            }
        }
        public string FileNotification 
        {
            get 
            {
                return _fileNotification;
            }
            set {
                _fileNotification = value;
                PropertyChange("FileNotification");
            }
        }
        public string ExpressionNotification
        {
            get 
            {
                return _expressionNotification; 
            }
            set 
            { 
                _expressionNotification = value;
                PropertyChange("ExpressionNotification");
            }
        }

        public CalculatorModel()
        {

        }

        private void PropertyChange(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

using System.Windows.Controls;
using System.Windows.Input;
using System.Text.RegularExpressions;


namespace CalculatorFE.CustomControls
{
    public class CalculatorTextBox : TextBox
    {
        private static readonly Regex regex = new Regex(@"^[0-9+\-*\/\s]*$");

        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            if (!regex.IsMatch(e.Text))
                e.Handled = true;
            base.OnPreviewTextInput(e);
        }
    }
}

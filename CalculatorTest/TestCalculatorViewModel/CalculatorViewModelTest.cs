using System.Threading.Tasks;
using CalculatorFE.ViewModels;
using NUnit.Framework;

namespace CalculatorTest.TestCalculatorViewModel
{
    public class CalculatorViewModelTest
    {
        CalculatorViewModel viewModel;
        [SetUp]
        public void Setup()
        {
            viewModel = new CalculatorViewModel();
        }

        [TestCase("2+1","3")]
        [TestCase("2 + -3 * 2", "-4")]
        [TestCase("2 / 3", "0")]
        [TestCase("-4 - 2+6", "0")]
        [TestCase("-4-2 +    6", "0")]
        [TestCase("2 + -3 * 2 - 26 + 4 - 78", "-104")]
        [TestCase("2 + -14 / 2 * 3", "-19")]
        [TestCase("2 + -15 / 2 * 3", "-20")]
        [TestCase("-4-2+0-78+7", "-77")]
        [TestCase("1/1+1", "2")]
        public async Task RunExpressionOk(string expression, string expected)
        {
            viewModel.Calculator.Expression = expression;
            await viewModel.RunExpression();
            Assert.AreEqual(viewModel.Calculator.Expression, expected);
        }

        [TestCase("4 + 8 + 9*-33 / 6 * a")]
        [TestCase("2.1 + -3 * 2")]
        [TestCase("a + -3 * 2")]
        [TestCase("2+(-3.1*2")]
        [TestCase("2 + -2a * 2")]
        [TestCase("a+b/(c*d)")]
        [TestCase("a + b / ( c * d )")]
        [TestCase("1.1+2")]
        [TestCase(".1+2")]
        public async Task RunExpressionInvalidNotOk(string expression)
        {
            viewModel.Calculator.Expression = expression;
            await viewModel.RunExpression();
            Assert.IsNotNull(viewModel.Calculator.ExpressionNotification);
            Assert.IsNotEmpty(viewModel.Calculator.ExpressionNotification);
        }

        [TestCase("4///")]
        [TestCase("*3/2")]
        [TestCase("/3-2")]
        [TestCase("////***")]
        [TestCase("++---*/98")]
        [TestCase("98178178*978728987 / 7887 + 2")]
        public async Task RunExpressionValidNotOk(string expression)
        {
            viewModel.Calculator.Expression = expression;
            await viewModel.RunExpression();
            Assert.IsNotNull(viewModel.Calculator.ExpressionNotification);
            Assert.IsNotEmpty(viewModel.Calculator.ExpressionNotification);
        }

        [TestCase(@"C:\tests\input_test1.txt", @"C:\tests\output_test1.txt")]
        public async Task RunExpressionFileOk(string input, string output)
        {
            viewModel.Calculator.InputPath = input;
            viewModel.Calculator.OutputPath = output;
            await viewModel.RunFileExpression();
            Assert.AreEqual("Job is done.", viewModel.Calculator.FileNotification);
        }

        [Test]
        public async Task RunExpressionFileMissingInput()
        {
            viewModel.Calculator.InputPath = null;
            viewModel.Calculator.OutputPath = @"C:\tests\output_test1.txt";
            await viewModel.RunFileExpression();
            Assert.AreNotEqual("Working on file...", viewModel.Calculator.FileNotification);
            Assert.AreNotEqual("Job is done.", viewModel.Calculator.FileNotification);
            Assert.IsNotEmpty(viewModel.Calculator.FileNotification);
        }

        [Test]
        public async Task RunExpressionFileMissingOutput()
        {
            viewModel.Calculator.InputPath = @"C:\tests\input_test1.txt";
            viewModel.Calculator.OutputPath = null;
            await viewModel.RunFileExpression();
            Assert.AreNotEqual("Working on file...", viewModel.Calculator.FileNotification);
            Assert.AreNotEqual("Job is done.", viewModel.Calculator.FileNotification);
            Assert.IsNotEmpty(viewModel.Calculator.FileNotification);
        }

        [Test]
        public void CanEvaluateExpressionOK()
        {
            viewModel.Calculator.Expression = null;
            Assert.IsFalse(viewModel.Calculator.Expression == null);
            Assert.IsTrue(viewModel.CanEvaluateExpression);
        }

        [Test]
        public void CanEvaluateFileNoInputOk()
        {
            viewModel.Calculator.InputPath = null;
            Assert.IsFalse(viewModel.CanEvaluateFile);
        }

        [Test]
        public void CanEvaluateFileNoOutputOk()
        {
            viewModel.Calculator.OutputPath = null;
            Assert.IsFalse(viewModel.CanEvaluateFile);
        }

        [Test]
        public void CanEvaluateFileInvalid()
        {
            viewModel.Calculator.InputPath = null;
            viewModel.Calculator.OutputPath = null;
            Assert.IsFalse(viewModel.CanEvaluateFile);
        }
    }
}

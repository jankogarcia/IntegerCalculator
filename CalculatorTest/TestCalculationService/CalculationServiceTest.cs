using NUnit.Framework;
using CalculatorBE.Calculators;
using System.Threading.Tasks;
using CalculatorBE.Services;
using CalculatorBE.Validators;
using System;

namespace CalculatorTest.TestCalculationService
{
    public class CalculationServiceTest
    {
        ICalculationService service;
        IValidator validator;
        ICalculator calculator;

        [SetUp]
        public void Setup()
        {
            validator = new ExpressionValidator();
            calculator = new ExpressionCalculator();
            service = new CalculationService(validator, calculator);
        }

        [TestCase(@"C:\tests\input_test1.txt", @"C:\tests\output_test1.txt")]
        public async Task RunProcessOk(string input, string output)
        {
            await service.RunProcessAsync(input, output);
        }

        [Test]
        public async Task RunProcessMissingInput()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () => await service.RunProcessAsync(null, @"C:\tests\output_test1.txt"));
        }

        [Test]
        public async Task RunProcessMissingOutput()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () => await service.RunProcessAsync(@"C:\tests\input_test1.txt", null));
        }

        [TestCase("2.1 + -3 * 2")]
        [TestCase("a + -3 * 2")]
        [TestCase("2+(-3.1*2")]
        [TestCase("2 + -2a * 2")]
        [TestCase("a+b/(c*d)")]
        [TestCase("a + b / ( c * d )")]
        public async Task GetErrorInformationAsyncOk(string expression)
        {
            var information = await service.GetErrorInformationAsync(expression);
            Assert.IsNotEmpty(information);
            Assert.IsNotNull(information);
        }

        [TestCase("2 + -3 * 2", "-4")]
        [TestCase("2 / 3", "0")]
        [TestCase("-4 - 2+6", "0")]
        [TestCase("-4-2 +    6", "0")]
        [TestCase("2 + -3 * 2 - 26 + 4 - 78", "-104")]
        [TestCase("2 + -14 / 2 * 3", "-19")]
        [TestCase("2 + -15 / 2 * 3", "-20")]
        [TestCase("-4-2+0-78+7", "-77")]
        [TestCase("1/1+1", "2")]
        public async Task CalculateExpressionAsyncOk(string expression, string expected)
        {
            var response = await service.CalculateExpressionAsync(expression);
            Assert.AreEqual(expected, response);
        }

        [TestCase("2 + -3 * 2")]
        [TestCase("2 + -15 / 2 * 32")]
        [TestCase("2 + -3 * 2 - 26 + 4 - 78")]
        [TestCase("-4-2+6")]
        [TestCase("-4 - 2+6")]
        [TestCase("-4-2 +    6")]
        [TestCase("-4-2+6/15*98/47-78+7")]
        [TestCase("981781798*978728987 / 7887 + 2")]
        [TestCase("*3/2")]
        [TestCase("/3-2")]
        [TestCase("////***")]
        public async Task ValidateExpressionOk(string expression)
        {
            var resp = await service.ValidateExpressionAsync(expression);
            Assert.IsTrue(resp);
        }

        [TestCase("2.1 + -3 * 2")]
        [TestCase("a + -3 * 2")]
        [TestCase("2+(-3.1*2")]
        [TestCase("2 + -2a * 2")]
        [TestCase("a+b/(c*d)")]
        [TestCase("a + b / ( c * d )")]
        [TestCase("1.1+2")]
        [TestCase(".1+2")]
        public async Task ValidateExpressionNotOk(string expression)
        {
            var resp = await service.ValidateExpressionAsync(expression);
            Assert.IsFalse(resp);
        }


        [TestCase("*3/2")]
        [TestCase("/3-2")]
        [TestCase("////***")]
        [TestCase("++---*/98")]
        [TestCase("98178178*978728987 / 7887 + 2")]
        [TestCase("")]
        //technically valids (mega big number and missing operands will pass the as valid but we expect a string with information)
        public async Task TechnicallyYesButNotQuite(string expression)
        {
            var passValidation = await service.ValidateExpressionAsync(expression);
            var result = await service.CalculateExpressionAsync(expression);

            Assert.IsTrue(passValidation);
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<string>(result);
        }

    }
}

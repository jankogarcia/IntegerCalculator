using CalculatorBE.Validators;
using NUnit.Framework;
using System.Threading.Tasks;

namespace CalculatorTest.TestsValidator
{
    public class ValidatorTests
    {
        IValidator validator;
        
        [SetUp]
        public void Setup()
        {
            validator = new ExpressionValidator();
        }

        [TestCase("2.1 + -3 * 2")]
        [TestCase("a + -3 * 2")]
        [TestCase("2+(-3.1*2")]
        [TestCase("2 + -2a * 2")]
        [TestCase("a+b/(c*d)")]
        [TestCase("a + b / ( c * d )")]
        [TestCase("1.1+2")]
        [TestCase(".1+2")]
        public async Task ValidatorInvalidExpressions(string expression)
        {
            var resp = await validator.IsValidExpressionAsync(expression);
            Assert.IsFalse(resp);
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
        public async Task ValidatorValidExpressions(string expression)
        {
            var resp = await validator.IsValidExpressionAsync(expression);
            Assert.IsTrue(resp);
        }


        [TestCase("2.1 + -3 * 2")]
        [TestCase("a + -3 * 2")]
        [TestCase("2+(-3.1*2")]
        [TestCase("2 + -2a * 2")]
        [TestCase("a+b/(c*d)")]
        [TestCase("a + b / ( c * d )")]
        public async Task ValidatorGetErrorResultOK(string expression)
        {
            var resp = await validator.GetErrorMessageAsync(expression);
            Assert.IsNotEmpty(resp);
            Assert.IsNotNull(resp);
        }
    }
}

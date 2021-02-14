using NUnit.Framework;
using CalculatorBE.Calculators;
using System.Threading.Tasks;

namespace CalculatorTest.TestExpression
{
    public class ExpressionCalculatorTest
    {
        ICalculator calculator;

        [SetUp]
        public void Setup()
        {
            calculator = new ExpressionCalculator();
        }

        [TestCase("2 + -3 * 2", -4)]
        [TestCase("2 / 3", 0)]
        [TestCase("-4 - 2+6", 0)]
        [TestCase("-4-2 +    6", 0)]
        [TestCase("2 + -3 * 2 - 26 + 4 - 78", -104)]
        [TestCase("2 + -14 / 2 * 3", -19)]
        [TestCase("2 + -15 / 2 * 3", -20)]
        [TestCase("-4-2+0-78+7", -77)]
        [TestCase("1/1+1", 2)]
        public async Task CalculateExpressionOk(string expression, long expected)
        {
            var result = await calculator.CalculateExpressionAsync(expression);
            Assert.AreEqual(expected, result);
        }
    }
}

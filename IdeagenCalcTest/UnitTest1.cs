using IdeagenCalc;
using System;
using System.Linq;
using System.Reflection;
using Xunit;

namespace IdeagenCalcTest
{
    public class CalculatorTest
    {
        [Fact]
        public void Calculate_EmptyStringInput_Return0()
        {
            Exception ex = Assert.Throws<Exception>(() => Calculator.Calculate(""));
            Assert.Equal("Expression string cannot be null or empty.", ex.Message);
        }

        [Theory]
        [InlineData('+',1,2,3)]
        [InlineData('-', 5, 2, 3)]
        [InlineData('*', 10, 2, 20)]
        [InlineData('/', 10, 2, 5)]
        public void Calculate_Operator_ReturnCorrectResult(char c_operator, decimal operandA, decimal operandB, decimal expected)
        {
            Type type = typeof(Calculator);
            var calculator = Activator.CreateInstance(type);
            MethodInfo method = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Static).Where(x => x.Name == "Calculate" && x.IsPrivate).First();
            var actual = (decimal)method.Invoke(calculator, new object[] { c_operator, operandA, operandB });
            Assert.True(actual == expected);
        }

        [Fact]
        public void Calculate_InvalidOperator_ThrowException()
        {
            Type type = typeof(Calculator);
            var calculator = Activator.CreateInstance(type);
            MethodInfo method = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Static).Where(x => x.Name == "Calculate" && x.IsPrivate).First();            
            Exception ex = Assert.Throws<System.Reflection.TargetInvocationException>(() => method.Invoke(calculator, new object[] { '|', (decimal)10, (decimal)10 }));
            Assert.Equal($"Expression contain invalid operator : |", ex.InnerException.Message);
        }

        [Theory]
        [InlineData("1 + 1", 2)]
        [InlineData("2 * 2", 4)]
        [InlineData("1 + 2 + 3", 6)]
        [InlineData("6 / 2", 3)]
        [InlineData("11 + 23", 34)]
        [InlineData("11.1 + 23", 34.1)]
        [InlineData("1 + 1 * 3", 4)]
        public void Calculate_MultipleExpression_ReturnCorrectResults(string input, decimal expected)
        {
            var actual = Calculator.Calculate(input);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("( 11.5 + 15.4 ) + 10.1", 37)]
        [InlineData("23 - ( 29.3 - 12.5 )", 6.2)]
        [InlineData("( 1 / 2 ) - 1 + 1", 0.5)]
        public void Calculate_MultipleExpressionWithBracket_ReturnCorrectResults(string input, decimal expected)
        {
            var actual = Calculator.Calculate(input);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("10 - ( 2 + 3 * ( 7 - 5 ) )", 2)]
        public void Calculate_MultipleExpressionWithMultipleBracket_ReturnCorrectResults(string input, decimal expected)
        {
            var actual = Calculator.Calculate(input);
            Assert.Equal(expected, actual);
        }
    }
}

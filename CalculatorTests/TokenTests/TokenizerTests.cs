using Calculator.Tokens;
using FluentAssertions;

namespace CalculatorTests.TokenTests
{
    public class TokenizerTests
    {
        public TokenizerTests()
        {
            _tokenizer = new Tokenizer();
        }

        [Theory]
        [InlineData("+", OperatorType.Addition)]
        [InlineData("-", OperatorType.Subtraction)]
        [InlineData("*", OperatorType.Multiplication)]
        [InlineData("/", OperatorType.Division)]
        [InlineData("(", OperatorType.OpeningBracket)]
        [InlineData(")", OperatorType.ClosingBracket)]

        public void OperatorTest(string expression, OperatorType expectedType)
        {
            var actual = _tokenizer.Parse(expression);
            var expected = new List<IToken>() { new OperatorToken(expectedType) };
            Compare(actual, expected);
        }

        [Theory]
        [InlineData("2.25", 2.25)]
        [InlineData("0.1", 0.1)]
        [InlineData("\u02C91", -1)]
        [InlineData("\u02C94.2", -4.2)]
        public void CorrectOperandTest(string expression, double expectedValue)
        {
            var actual = _tokenizer.Parse(expression);
            var expected = new List<IToken>() { new OperandToken(expectedValue) };
            Compare(actual, expected);
        }

        [Theory]
        [InlineData("2.25.23")]
        public void IncorrectOperandTest(string expression)
        {
            Action action = () => _tokenizer.Parse(expression);
            action.Should().ThrowExactly<ArgumentException>();
        }

        private static void Compare(IEnumerable<IToken> actual, IEnumerable<IToken> expected)
        {
            actual.Should().BeEquivalentTo(expected,
                opts => opts.RespectingRuntimeTypes().WithStrictOrdering()
            );
        }

        private readonly Tokenizer _tokenizer;
    }
}

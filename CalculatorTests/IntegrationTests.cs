﻿using Calculator.Parsers;
using Calculator.Tokens;
using FluentAssertions;

namespace CalculatorTests;

public class IntegrationTests
{
    public IntegrationTests()
    {
        _tokenizer = new Tokenizer();
        _algorithm = new ShuntingYardAlgorithm();
        _calculator = new PostfixNotationCalculator();
    }

    [Fact]
    public void AllSupportedOperatorsTest()
    {
        var actual = Calculate("(1+2)*3 - (2.2*1.1)/0.5 + 0.2");
        var expected = 4.36;

        actual.Should().BeApproximately(expected, Precision);
    }

    [Fact]
    public void RedundantOpeningBracketTest()
    {
        Action action = () => Calculate("((2 + 3)");
        action.Should().ThrowExactly<ArgumentException>();
    }

    [Fact]
    public void RedundantClosingBracketTest()
    {
        Action action = () => Calculate("(2+3))");
        action.Should().ThrowExactly<ArgumentException>();
    }

    [Fact]
    public void ExpressionWithBracketsTest()
    {
        var actual = Calculate(" (2 +2)* 2 ");
        var expected = 8;
        actual.Should().BeApproximately(expected, Precision);
    }

    [Fact]
    public void OperatorsWithDifferentPrioritiesTest()
    {
        var actual = Calculate("2+2 * 2");
        var expected = 6;
        actual.Should().BeApproximately(expected, Precision);
    }

    private double Calculate(string expression)
    {
        var infixNotationTokens = _tokenizer.Parse(expression);
        var postfixNotationTokens = _algorithm.Apply(infixNotationTokens);
        return _calculator.Calculate(postfixNotationTokens).Value;
    }

    private const double Precision = 1e-7;

    private readonly Tokenizer _tokenizer;
    private readonly ShuntingYardAlgorithm _algorithm;
    private readonly PostfixNotationCalculator _calculator;
}

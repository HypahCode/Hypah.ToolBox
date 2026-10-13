using Hypah.Calculator.Errors;
using Hypah.Calculator.Tokenizer;
using Xunit;

namespace Hypah.Calculator.Tests;

public class TokenizerTests
{
    [Fact]
    public void SimpleNumber()
    {
        var tokens = ExpressionTokenizer.Tokenize("5");
        Assert.Single(tokens);
        Assert.Equal(TokenType.Number, tokens[0].Type);
        Assert.Equal("5", tokens[0].Text);
        Assert.Equal(5.0, tokens[0].NumericValue);
    }

    [Fact]
    public void SimpleAddition()
    {
        var tokens = ExpressionTokenizer.Tokenize("5+8");
        Assert.Equal(3, tokens.Count);
        Assert.Equal(TokenType.Number, tokens[0].Type);
        Assert.Equal(TokenType.Operator, tokens[1].Type);
        Assert.Equal(TokenType.Number, tokens[2].Type);
    }

    [Fact]
    public void DecimalNumber()
    {
        var tokens = ExpressionTokenizer.Tokenize("5.2");
        Assert.Single(tokens);
        Assert.Equal(5.2, tokens[0].NumericValue);
    }

    [Fact]
    public void UnitAfterNumber()
    {
        var tokens = ExpressionTokenizer.Tokenize("3kg");
        Assert.Equal(2, tokens.Count);
        Assert.Equal(TokenType.Number, tokens[0].Type);
        Assert.Equal(TokenType.Unit, tokens[1].Type);
        Assert.Equal("kg", tokens[1].Text);
    }

    [Fact]
    public void CompositeUnit()
    {
        var tokens = ExpressionTokenizer.Tokenize("120km/h");
        Assert.Equal(2, tokens.Count);
        Assert.Equal(TokenType.Number, tokens[0].Type);
        Assert.Equal(TokenType.Unit, tokens[1].Type);
        Assert.Equal("km/h", tokens[1].Text);
    }

    [Fact]
    public void WhitespaceIgnored()
    {
        var tokens = ExpressionTokenizer.Tokenize("  3  kg  *  ( 5 + 2 )  -  1 g  ");
        Assert.Equal(11, tokens.Count);
    }

    [Fact]
    public void ParenthesesTokens()
    {
        var tokens = ExpressionTokenizer.Tokenize("(5+2)");
        Assert.Equal(5, tokens.Count);
        Assert.Equal(TokenType.LeftParen, tokens[0].Type);
        Assert.Equal(TokenType.Number, tokens[1].Type);
        Assert.Equal(TokenType.Operator, tokens[2].Type);
        Assert.Equal(TokenType.Number, tokens[3].Type);
        Assert.Equal(TokenType.RightParen, tokens[4].Type);
    }

    [Fact]
    public void PercentUnit()
    {
        var tokens = ExpressionTokenizer.Tokenize("100kg*5%");
        Assert.Equal(5, tokens.Count);
        Assert.Equal("%", tokens[4].Text);
    }

    [Fact]
    public void UnknownUnitThrows()
    {
        Assert.Throws<UnknownUnitException>(() => ExpressionTokenizer.Tokenize("3xyz"));
    }

    [Fact]
    public void AllSupportedUnitsTokenize()
    {
        var units = new[] { "ms", "s", "min", "h", "g", "kg", "mm", "cm", "m", "km", "ml", "l", "deg", "rad", "%", "m/s", "km/h", "J", "kJ", "MJ" };
        foreach (var unit in units)
        {
            var tokens = ExpressionTokenizer.Tokenize($"1{unit}");
            Assert.Equal(2, tokens.Count);
            Assert.Equal(TokenType.Unit, tokens[1].Type);
            Assert.Equal(unit, tokens[1].Text);
        }
    }
}

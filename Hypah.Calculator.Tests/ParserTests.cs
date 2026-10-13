using Hypah.Calculator.AST;
using Hypah.Calculator.Errors;
using Hypah.Calculator.Parser;
using Xunit;

namespace Hypah.Calculator.Tests;

public class ParserTests
{
    [Fact]
    public void SimpleNumber_ReturnsNumberNode()
    {
        var ast = ExpressionParser.Parse("5");
        var node = Assert.IsType<NumberNode>(ast);
        Assert.Equal(5.0, node.Value);
        Assert.Null(node.Unit);
    }

    [Fact]
    public void NumberWithUnit_ReturnsNumberNodeWithUnit()
    {
        var ast = ExpressionParser.Parse("3kg");
        var node = Assert.IsType<NumberNode>(ast);
        Assert.Equal(3.0, node.Value);
        Assert.NotNull(node.Unit);
        Assert.Equal("kg", node.Unit!.Symbol);
    }

    [Fact]
    public void Addition_ReturnsBinaryOpNode()
    {
        var ast = ExpressionParser.Parse("5+8");
        var node = Assert.IsType<BinaryOpNode>(ast);
        Assert.Equal("+", node.Operator);
    }

    [Fact]
    public void Subtraction_ReturnsBinaryOpNode()
    {
        var ast = ExpressionParser.Parse("10-3");
        var node = Assert.IsType<BinaryOpNode>(ast);
        Assert.Equal("-", node.Operator);
    }

    [Fact]
    public void Multiplication_ReturnsBinaryOpNode()
    {
        var ast = ExpressionParser.Parse("5*8");
        var node = Assert.IsType<BinaryOpNode>(ast);
        Assert.Equal("*", node.Operator);
    }

    [Fact]
    public void Division_ReturnsBinaryOpNode()
    {
        var ast = ExpressionParser.Parse("10/2");
        var node = Assert.IsType<BinaryOpNode>(ast);
        Assert.Equal("/", node.Operator);
    }

    [Fact]
    public void Parentheses_CreateNestedStructure()
    {
        var ast = ExpressionParser.Parse("3kg*(5+2)");
        var node = Assert.IsType<BinaryOpNode>(ast);
        Assert.Equal("*", node.Operator);
        var inner = Assert.IsType<BinaryOpNode>(node.Right);
        Assert.Equal("+", inner.Operator);
    }

    [Fact]
    public void OperatorPrecedence_MultiplyBindsTighterThanAdd()
    {
        var ast = ExpressionParser.Parse("5+8*2");
        var node = Assert.IsType<BinaryOpNode>(ast);
        Assert.Equal("+", node.Operator);
        var right = Assert.IsType<BinaryOpNode>(node.Right);
        Assert.Equal("*", right.Operator);
    }

    [Fact]
    public void UnaryMinus_ReturnsUnaryOpNode()
    {
        var ast = ExpressionParser.Parse("-5");
        var node = Assert.IsType<UnaryOpNode>(ast);
        Assert.Equal("-", node.Operator);
    }

    [Fact]
    public void InvalidSyntax_ThrowsInvalidSyntaxException()
    {
        Assert.Throws<InvalidSyntaxException>(() => ExpressionParser.Parse("5+*8"));
    }

    [Fact]
    public void MissingParenthesis_ThrowsInvalidSyntaxException()
    {
        Assert.Throws<InvalidSyntaxException>(() => ExpressionParser.Parse("(5+2"));
    }
}

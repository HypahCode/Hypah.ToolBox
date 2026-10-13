using Hypah.Calculator.Errors;
using Xunit;

namespace Hypah.Calculator.Tests;

public class ErrorHandlingTests
{
    [Fact]
    public void UnitMismatch_ThrowsCorrectException()
    {
        var ex = Assert.Throws<UnitMismatchException>(() => Calculator.Calculate("3kg + 5m"));
        Assert.Contains("kg", ex.Message);
        Assert.Contains("m", ex.Message);
    }

    [Fact]
    public void DivisionByZero_ThrowsCorrectException()
    {
        Assert.Throws<DivisionByZeroException>(() => Calculator.Calculate("5/0"));
    }

    [Fact]
    public void UnknownUnit_ThrowsCorrectException()
    {
        var ex = Assert.Throws<UnknownUnitException>(() => Calculator.Calculate("3xyz + 5"));
        Assert.Contains("xyz", ex.Message);
    }

    [Fact]
    public void InvalidSyntax_ThrowsCorrectException()
    {
        Assert.Throws<InvalidSyntaxException>(() => Calculator.Calculate("5+*8"));
    }

    [Fact]
    public void EmptyExpression_ThrowsCorrectException()
    {
        Assert.Throws<InvalidSyntaxException>(() => Calculator.Calculate(""));
    }
}

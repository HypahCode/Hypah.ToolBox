using Hypah.Calculator.Formatter;
using Xunit;

namespace Hypah.Calculator.Tests;

public class CalculatorIntegrationTests
{
    [Theory]
    [InlineData("5+8", 13, "13.000")]
    [InlineData("6s + 1h", 3606, "1h 6s")]
    [InlineData("3kg * (5+2) - 1g", 20999, "20.999kg")]
    [InlineData("3m + 20cm", 3.2, "3m 20cm")]
    [InlineData("120km/h * 3s", 100, "100m")]
    [InlineData("90deg + 1rad", 2.570796, "147.296deg")]
    [InlineData("100 * 5%", 5, "5.000")]
    [InlineData("2l + 500ml", 2.5, "2l 500ml")]
    public void Evaluate_VariousExpressions(string expression, double expectedRaw, string expectedPretty)
    {
        var result = Calculator.Calculate(expression);
        Assert.Equal(expectedRaw, result.RawValue, 2);
        Assert.Equal(expectedPretty, result.PrettyFormatted);
    }

    [Fact]
    public void TimeCalc()
    {
        var result = Calculator.Calculate("1h + 30min");
        Assert.Equal(5400, result.RawValue, 0);
        Assert.Equal("1h 30min", result.PrettyFormatted);
    }

    [Fact]
    public void WeightCalc()
    {
        var result = Calculator.Calculate("2kg + 500g");
        Assert.Equal(2500, result.RawValue, 0);
        Assert.Equal("2.5kg", result.PrettyFormatted);
    }

    [Fact]
    public void LengthCalc()
    {
        var result = Calculator.Calculate("1km + 500m");
        Assert.Equal(1500, result.RawValue, 0);
        Assert.Equal("1km 500m", result.PrettyFormatted);
    }

    [Fact]
    public void VolumeCalc()
    {
        var result = Calculator.Calculate("1l + 250ml");
        Assert.Equal(1.25, result.RawValue, 2);
        Assert.Equal("1l 250ml", result.PrettyFormatted);
    }

    [Fact]
    public void EnergyCalc()
    {
        var result = Calculator.Calculate("1kJ + 500J");
        Assert.Equal(1500, result.RawValue, 0);
        Assert.Equal("1.5kJ", result.PrettyFormatted);
    }

    [Fact]
    public void NestedParens()
    {
        var result = Calculator.Calculate("(2kg + 500g) * 2");
        Assert.Equal(5000, result.RawValue, 0);
    }

    [Fact]
    public void UnaryNegative()
    {
        var result = Calculator.Calculate("-5kg + 3kg");
        Assert.Equal(-2000, result.RawValue, 0);
    }

    [Fact]
    public void Division()
    {
        var result = Calculator.Calculate("10m / 2s");
        Assert.Equal(5, result.RawValue, 3);
        Assert.Equal("18km/h", result.PrettyFormatted);
    }
}

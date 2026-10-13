using Hypah.Calculator.Errors;
using Hypah.Calculator.Parser;
using Hypah.Calculator.Units;
using Xunit;

namespace Hypah.Calculator.Tests;

public class NormalizerTests
{
    [Fact]
    public void Kilograms_NormalizeToGrams()
    {
        var (value, unit) = Calc("3kg");
        Assert.Equal(3000, value, 3);
        Assert.Equal("g", unit!.EffectiveBase);
    }

    [Fact]
    public void Hours_NormalizeToSeconds()
    {
        var (value, unit) = Calc("1h");
        Assert.Equal(3600, value, 3);
        Assert.Equal("s", unit!.EffectiveBase);
    }

    [Fact]
    public void Degrees_NormalizeToRadians()
    {
        var (value, unit) = Calc("90deg");
        Assert.Equal(Math.PI / 2, value, 3);
        Assert.Equal("rad", unit!.EffectiveBase);
    }

    [Fact]
    public void Percent_NormalizeToDecimal()
    {
        var (value, unit) = Calc("5%");
        Assert.Equal(0.05, value, 3);
        Assert.Equal("%", unit!.EffectiveBase);
    }

    [Fact]
    public void Kmh_NormalizeToMs()
    {
        var (value, unit) = Calc("120km/h");
        Assert.Equal(120.0 / 3.6, value, 2);
        Assert.Equal("m/s", unit!.EffectiveBase);
    }

    [Fact]
    public void MixedTime_Addition()
    {
        var (value, unit) = Calc("6s + 1h");
        Assert.Equal(3606, value, 3);
        Assert.Equal("s", unit!.EffectiveBase);
    }

    [Fact]
    public void MixedLength_Addition()
    {
        var (value, unit) = Calc("3m + 20cm");
        Assert.Equal(3.2, value, 3);
        Assert.Equal("m", unit!.EffectiveBase);
    }

    [Fact]
    public void MixedWeight_Complex()
    {
        var (value, unit) = Calc("3kg * (5+2) - 1g");
        Assert.Equal(20999, value, 3);
        Assert.Equal("g", unit!.EffectiveBase);
    }

    [Fact]
    public void MixedVolume_Addition()
    {
        var (value, unit) = Calc("2l + 500ml");
        Assert.Equal(2.5, value, 3);
        Assert.Equal("l", unit!.EffectiveBase);
    }

    [Fact]
    public void UnitMismatch_ThrowsUnitMismatchException()
    {
        Assert.Throws<UnitMismatchException>(() => Calc("3kg + 5m"));
    }

    [Fact]
    public void SpeedTimesTime_GivesLength()
    {
        var (value, unit) = Calc("120km/h * 3s");
        Assert.Equal(100, value, 0);
        Assert.Equal("m", unit!.EffectiveBase);
    }

    [Fact]
    public void AngleAddition_DegAndRad()
    {
        var (value, unit) = Calc("90deg + 1rad");
        Assert.Equal(Math.PI / 2 + 1, value, 3);
        Assert.Equal("rad", unit!.EffectiveBase);
    }

    [Fact]
    public void UnitlessPercentage()
    {
        var (value, unit) = Calc("100 * 5%");
        Assert.Equal(5, value, 3);
    }

    private static (double Value, UnitDefinition? Unit) Calc(string expr)
    {
        var ast = ExpressionParser.Parse(expr);
        return Normalizer.UnitNormalizer.Normalize(ast);
    }
}

using Hypah.Calculator.Formatter;
using Hypah.Calculator.Units;
using Xunit;

namespace Hypah.Calculator.Tests;

public class FormatterTests
{
    [Fact]
    public void GramsToKilograms()
    {
        var unit = UnitTable.GetUnit("kg");
        var result = ResultFormatter.Format(20999, unit);
        Assert.Equal("20.999kg", result);
    }

    [Fact]
    public void SecondsToCompoundTime()
    {
        var unit = UnitTable.GetUnit("s");
        var result = ResultFormatter.Format(3661, unit);
        Assert.Equal("1h 1min 1s", result);
    }

    [Fact]
    public void MetersToCompoundLength()
    {
        var unit = UnitTable.GetUnit("m");
        var result = ResultFormatter.Format(3.2, unit);
        Assert.Equal("3m 20cm", result);
    }

    [Fact]
    public void LitersToCompoundVolume()
    {
        var unit = UnitTable.GetUnit("l");
        var result = ResultFormatter.Format(2.5, unit);
        Assert.Equal("2l 500ml", result);
    }

    [Fact]
    public void RadiansToDegrees()
    {
        var unit = UnitTable.GetUnit("rad");
        var result = ResultFormatter.Format(Math.PI / 2, unit);
        Assert.Equal("90deg", result);
    }

    [Fact]
    public void DecimalToPercent()
    {
        var unit = UnitTable.GetUnit("%");
        var result = ResultFormatter.Format(0.05, unit);
        Assert.Equal("5%", result);
    }

    [Fact]
    public void Dimensionless()
    {
        var result = ResultFormatter.Format(13, null);
        Assert.Equal("13.000", result);
    }

    [Fact]
    public void MsToKmh()
    {
        var unit = UnitTable.GetUnit("m/s");
        var result = ResultFormatter.Format(33.333, unit);
        Assert.Equal("119.999km/h", result);
    }

    [Fact]
    public void CustomDecimalPlaces()
    {
        var unit = UnitTable.GetUnit("m");
        var options = new FormatterOptions(DecimalPlaces: 2);
        var result = ResultFormatter.Format(3.2, unit, options);
        Assert.Equal("3m 20cm", result);
    }
}

using Hypah.Calculator.Errors;
using Hypah.Calculator.Parser;
using Hypah.Calculator.Units;
using Xunit;

namespace Hypah.Calculator.Tests;

public class NewUnitsNormalizerTests
{
    [Fact]
    public void Bitrate_Normalize()
    {
        var (value, unit) = Calc("1Mbps");
        Assert.Equal(1_000_000, value, 0);
        Assert.Equal(UnitCategory.Bitrate, unit!.Category);
    }

    [Fact]
    public void Bitrate_Addition()
    {
        var (value, unit) = Calc("500kbps + 500kbps");
        Assert.Equal(1_000_000, value, 0);
        Assert.Equal(UnitCategory.Bitrate, unit!.Category);
    }

    [Fact]
    public void Memory_Normalize()
    {
        var (value, unit) = Calc("1KB");
        Assert.Equal(1024, value, 0);
        Assert.Equal(UnitCategory.Memory, unit!.Category);
    }

    [Fact]
    public void Memory_Addition()
    {
        var (value, unit) = Calc("1KB + 512B");
        Assert.Equal(1536, value, 0);
        Assert.Equal(UnitCategory.Memory, unit!.Category);
    }

    [Fact]
    public void Bandwidth_Normalize()
    {
        var (value, unit) = Calc("1MB/s");
        Assert.Equal(1_000_000, value, 0);
        Assert.Equal(UnitCategory.Bandwidth, unit!.Category);
    }

    [Fact]
    public void Force_Normalize()
    {
        var (value, unit) = Calc("1kN");
        Assert.Equal(1000, value, 0);
        Assert.Equal(UnitCategory.Force, unit!.Category);
    }

    [Fact]
    public void Pressure_Normalize()
    {
        var (value, unit) = Calc("1bar");
        Assert.Equal(100_000, value, 0);
        Assert.Equal(UnitCategory.Pressure, unit!.Category);
    }

    [Fact]
    public void Density_Normalize()
    {
        var (value, unit) = Calc("2kg/m3");
        Assert.Equal(2, value, 3);
        Assert.Equal(UnitCategory.Density, unit!.Category);
    }

    [Fact]
    public void FlowRate_Normalize()
    {
        var (value, unit) = Calc("500ml/s");
        Assert.Equal(0.5, value, 3);
        Assert.Equal(UnitCategory.FlowRate, unit!.Category);
    }

    [Fact]
    public void Torque_Normalize()
    {
        var (value, unit) = Calc("10Nm");
        Assert.Equal(10, value, 0);
        Assert.Equal(UnitCategory.Torque, unit!.Category);
    }

    [Fact]
    public void Torque_FromLengthTimesForce()
    {
        var (value, unit) = Calc("5m * 3N");
        Assert.Equal(15, value, 0);
        Assert.Equal(UnitCategory.Torque, unit!.Category);
    }

    [Fact]
    public void Power_Normalize()
    {
        var (value, unit) = Calc("2kW");
        Assert.Equal(2000, value, 0);
        Assert.Equal(UnitCategory.Power, unit!.Category);
    }

    [Fact]
    public void Frequency_Normalize()
    {
        var (value, unit) = Calc("2kHz");
        Assert.Equal(2000, value, 0);
        Assert.Equal(UnitCategory.Frequency, unit!.Category);
    }

    [Fact]
    public void Wavelength_Normalize()
    {
        var (value, unit) = Calc("500nm");
        Assert.Equal(500e-9, value, 12);
        Assert.Equal(UnitCategory.Wavelength, unit!.Category);
    }

    [Fact]
    public void Voltage_Normalize()
    {
        var (value, unit) = Calc("12V");
        Assert.Equal(12, value, 0);
        Assert.Equal(UnitCategory.Voltage, unit!.Category);
    }

    [Fact]
    public void Current_Normalize()
    {
        var (value, unit) = Calc("5A");
        Assert.Equal(5, value, 0);
        Assert.Equal(UnitCategory.Current, unit!.Category);
    }

    [Fact]
    public void Resistance_Normalize()
    {
        var (value, unit) = Calc("100Ω");
        Assert.Equal(100, value, 0);
        Assert.Equal(UnitCategory.Resistance, unit!.Category);
    }

    [Fact]
    public void Lumens_Normalize()
    {
        var (value, unit) = Calc("50lm");
        Assert.Equal(50, value, 0);
        Assert.Equal(UnitCategory.Lumens, unit!.Category);
    }

    [Fact]
    public void ColorTemperature_Normalize()
    {
        var (value, unit) = Calc("6500K");
        Assert.Equal(6500, value, 0);
        Assert.Equal(UnitCategory.ColorTemperature, unit!.Category);
    }

    [Fact]
    public void SoundLevel_Normalize()
    {
        var (value, unit) = Calc("10dB");
        Assert.Equal(10, value, 0);
        Assert.Equal(UnitCategory.SoundLevel, unit!.Category);
    }

    [Fact]
    public void SoundLevel_Addition_Throws()
    {
        Assert.Throws<CalculatorException>(() => Calc("10dB + 20dB"));
    }

    [Fact]
    public void FrameRate_Normalize()
    {
        var (value, unit) = Calc("60fps");
        Assert.Equal(60, value, 0);
        Assert.Equal(UnitCategory.FrameRate, unit!.Category);
    }

    private static (double Value, UnitDefinition? Unit) Calc(string expr)
    {
        var ast = ExpressionParser.Parse(expr);
        return Normalizer.UnitNormalizer.Normalize(ast);
    }
}

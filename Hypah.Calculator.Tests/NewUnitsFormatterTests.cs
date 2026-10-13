using Hypah.Calculator.Formatter;
using Hypah.Calculator.Units;
using Xunit;

namespace Hypah.Calculator.Tests;

public class NewUnitsFormatterTests
{
    [Fact]
    public void Memory_CompoundFormat()
    {
        var unit = UnitTable.GetUnit("B");
        var result = ResultFormatter.Format(1536, unit);
        Assert.Equal("1KB 512B", result);
    }

    [Fact]
    public void Memory_LargeValue()
    {
        var unit = UnitTable.GetUnit("B");
        var result = ResultFormatter.Format(1024 * 1024 + 512 * 1024, unit);
        Assert.Equal("1MB 512KB", result);
    }

    [Fact]
    public void Bitrate_Format()
    {
        var unit = UnitTable.GetUnit("bps");
        var result = ResultFormatter.Format(1_500_000, unit);
        Assert.Equal("1.5Mbps", result);
    }

    [Fact]
    public void Bitrate_SmallValue()
    {
        var unit = UnitTable.GetUnit("bps");
        var result = ResultFormatter.Format(500, unit);
        Assert.Equal("500bps", result);
    }

    [Fact]
    public void Power_Format()
    {
        var unit = UnitTable.GetUnit("W");
        var result = ResultFormatter.Format(2500, unit);
        Assert.Equal("2.5kW", result);
    }

    [Fact]
    public void Frequency_Format()
    {
        var unit = UnitTable.GetUnit("Hz");
        var result = ResultFormatter.Format(500, unit);
        Assert.Equal("500Hz", result);
    }

    [Fact]
    public void Frequency_Kilo()
    {
        var unit = UnitTable.GetUnit("Hz");
        var result = ResultFormatter.Format(2500, unit);
        Assert.Equal("2.5kHz", result);
    }

    [Fact]
    public void SoundLevel_Format()
    {
        var unit = UnitTable.GetUnit("dB");
        var result = ResultFormatter.Format(-3, unit);
        Assert.Equal("-3dB", result);
    }

    [Fact]
    public void Torque_Format()
    {
        var unit = UnitTable.GetUnit("Nm");
        var result = ResultFormatter.Format(10, unit);
        Assert.Equal("10Nm", result);
    }

    [Fact]
    public void Force_Format()
    {
        var unit = UnitTable.GetUnit("N");
        var result = ResultFormatter.Format(5000, unit);
        Assert.Equal("5kN", result);
    }

    [Fact]
    public void Pressure_Format()
    {
        var unit = UnitTable.GetUnit("Pa");
        var result = ResultFormatter.Format(200000, unit);
        Assert.Equal("2bar", result);
    }

    [Fact]
    public void Memory_SingleUnit()
    {
        var unit = UnitTable.GetUnit("B");
        var result = ResultFormatter.Format(2048, unit);
        Assert.Equal("2KB", result);
    }

    [Fact]
    public void Bitrate_Giga()
    {
        var unit = UnitTable.GetUnit("bps");
        var result = ResultFormatter.Format(2_500_000_000, unit);
        Assert.Equal("2.5Gbps", result);
    }
}

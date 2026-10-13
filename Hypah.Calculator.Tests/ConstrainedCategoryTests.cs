using Hypah.Calculator.Errors;
using Hypah.Calculator.Units;
using Xunit;

namespace Hypah.Calculator.Tests;

public class ConstrainedCategoryTests
{
    [Fact]
    public void Weight_Addition_Constrained()
    {
        var result = Calculator.Calculate("6g + 3kg", UnitCategory.Weight);
        Assert.Equal(UnitCategory.Weight, result.Category);
        Assert.Equal(3006, result.RawValue, 0);
    }

    [Fact]
    public void Weight_ScalarMultiply_Constrained()
    {
        var result = Calculator.Calculate("6g * 2", UnitCategory.Weight);
        Assert.Equal(UnitCategory.Weight, result.Category);
        Assert.Equal(12, result.RawValue, 0);
    }

    [Fact]
    public void Weight_Mismatch_Throws()
    {
        Assert.Throws<UnitMismatchException>(
            () => Calculator.Calculate("6g + 3m", UnitCategory.Weight));
    }

    [Fact]
    public void Time_Constrained()
    {
        var result = Calculator.Calculate("100ms + 2s", UnitCategory.Time);
        Assert.Equal(UnitCategory.Time, result.Category);
        Assert.Equal(2.1, result.RawValue, 2);
    }

    [Fact]
    public void Time_Mismatch_Throws()
    {
        Assert.Throws<UnitMismatchException>(
            () => Calculator.Calculate("100ms + 3kg", UnitCategory.Time));
    }

    [Fact]
    public void Speed_Constrained()
    {
        var result = Calculator.Calculate("60km/h + 40km/h", UnitCategory.Speed);
        Assert.Equal(UnitCategory.Speed, result.Category);
        Assert.Equal(27.777, result.RawValue, 2);
    }

    [Fact]
    public void Speed_Mismatch_Throws()
    {
        Assert.Throws<UnitMismatchException>(
            () => Calculator.Calculate("60km/h + 5m", UnitCategory.Speed));
    }

    [Fact]
    public void NullCategory_GenericBehavior()
    {
        var result = Calculator.Calculate("6g + 3kg");
        Assert.Equal(UnitCategory.Weight, result.Category);
    }

    [Fact]
    public void NullCategory_SpeedTimesTime()
    {
        var result = Calculator.Calculate("10m / 2s");
        Assert.Equal(UnitCategory.Speed, result.Category);
    }

    [Fact]
    public void Energy_Constrained()
    {
        var result = Calculator.Calculate("1kJ + 500J", UnitCategory.Energy);
        Assert.Equal(UnitCategory.Energy, result.Category);
        Assert.Equal(1500, result.RawValue, 0);
    }

    [Fact]
    public void Length_UnitTimesScalar()
    {
        var result = Calculator.Calculate("5m * 3", UnitCategory.Length);
        Assert.Equal(UnitCategory.Length, result.Category);
        Assert.Equal(15, result.RawValue, 0);
    }

    [Fact]
    public void Length_TwoUnitsMultiply_ThrowsWithConstraint()
    {
        var ex = Record.Exception(() => Calculator.Calculate("3m * 2s", UnitCategory.Length));
        Assert.NotNull(ex);
        Assert.IsType<CalculatorException>(ex);
    }

    [Fact]
    public void Volume_Constrained()
    {
        var result = Calculator.Calculate("1l + 500ml", UnitCategory.Volume);
        Assert.Equal(UnitCategory.Volume, result.Category);
        Assert.Equal(1.5, result.RawValue, 2);
    }

    [Fact]
    public void Division_ResultCategory_Constrained()
    {
        var result = Calculator.Calculate("100m / 5s", UnitCategory.Speed);
        Assert.Equal(UnitCategory.Speed, result.Category);
        Assert.Equal(20, result.RawValue, 0);
    }
}

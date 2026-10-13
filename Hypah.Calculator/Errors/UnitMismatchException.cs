namespace Hypah.Calculator.Errors;

public class UnitMismatchException : CalculatorException
{
    public string LeftUnit { get; init; }
    public string RightUnit { get; init; }

    public UnitMismatchException(string leftUnit, string rightUnit)
        : base($"Cannot operate on mismatched units: '{leftUnit}' and '{rightUnit}'")
    {
        LeftUnit = leftUnit;
        RightUnit = rightUnit;
    }
}

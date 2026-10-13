namespace Hypah.Calculator.Errors;

public class UnknownUnitException : CalculatorException
{
    public string Symbol { get; init; }

    public UnknownUnitException(string symbol)
        : base($"Unknown unit: '{symbol}'")
    {
        Symbol = symbol;
    }
}

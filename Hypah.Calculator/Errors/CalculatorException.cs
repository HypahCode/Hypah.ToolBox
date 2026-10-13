namespace Hypah.Calculator.Errors;

public class CalculatorException : Exception
{
    public CalculatorException(string message) : base(message) { }
    public CalculatorException(string message, Exception inner) : base(message, inner) { }
}

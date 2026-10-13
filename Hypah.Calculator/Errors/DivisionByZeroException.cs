namespace Hypah.Calculator.Errors;

public class DivisionByZeroException : CalculatorException
{
    public DivisionByZeroException() : base("Division by zero") { }
}

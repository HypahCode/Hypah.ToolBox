namespace Hypah.Calculator.Errors;

public class InvalidSyntaxException : CalculatorException
{
    public int Position { get; }

    public InvalidSyntaxException(string message, int position) : base(message)
    {
        Position = position;
    }
}

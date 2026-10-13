namespace Hypah.Calculator.Tokenizer;

public sealed record Token(TokenType Type, string Text, int Position)
{
    public double NumericValue { get; init; }
}

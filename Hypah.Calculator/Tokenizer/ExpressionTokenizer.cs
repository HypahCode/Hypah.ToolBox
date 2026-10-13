using Hypah.Calculator.Errors;
using Hypah.Calculator.Units;

namespace Hypah.Calculator.Tokenizer;

public static class ExpressionTokenizer
{
    public static List<Token> Tokenize(string input)
    {
        var tokens = new List<Token>();
        var i = 0;

        while (i < input.Length)
        {
            var c = input[i];

            if (char.IsWhiteSpace(c))
            {
                i++;
                continue;
            }

            if (c == '(')
            {
                tokens.Add(new Token(TokenType.LeftParen, "(", i));
                i++;
                continue;
            }

            if (c == ')')
            {
                tokens.Add(new Token(TokenType.RightParen, ")", i));
                i++;
                continue;
            }

            if (c is '+' or '-' or '*' or '/')
            {
                tokens.Add(new Token(TokenType.Operator, c.ToString(), i));
                i++;
                continue;
            }

            if (char.IsDigit(c) || c == '.')
            {
                var start = i;
                while (i < input.Length && (char.IsDigit(input[i]) || input[i] == '.'))
                    i++;

                var numStr = input[start..i];
                if (!double.TryParse(numStr, out var value))
                    throw new InvalidSyntaxException($"Invalid number: '{numStr}'", start);

                tokens.Add(new Token(TokenType.Number, numStr, start) { NumericValue = value });
                continue;
            }

            if (char.IsLetter(c) || c == '%')
            {
                var start = i;
                while (i < input.Length && (char.IsLetter(input[i]) || char.IsDigit(input[i]) || input[i] == '%' || input[i] == '/'))
                    i++;

                var unitStr = input[start..i];

                var remaining = input[start..];
                var matched = false;
                foreach (var composite in UnitTable.CompositeSymbols.OrderByDescending(s => s.Length))
                {
                    if (remaining.StartsWith(composite, StringComparison.Ordinal))
                    {
                        tokens.Add(new Token(TokenType.Unit, composite, start));
                        i = start + composite.Length;
                        matched = true;
                        break;
                    }
                }

                if (matched)
                    continue;

                if (UnitTable.TryGetUnit(unitStr, out _))
                {
                    tokens.Add(new Token(TokenType.Unit, unitStr, start));
                    continue;
                }

                throw new UnknownUnitException(unitStr);
            }

            throw new InvalidSyntaxException($"Unexpected character: '{c}'", i);
        }

        return tokens;
    }
}

using Hypah.Calculator.AST;
using Hypah.Calculator.Errors;
using Hypah.Calculator.Tokenizer;
using Hypah.Calculator.Units;

namespace Hypah.Calculator.Parser;

public static class ExpressionParser
{
    public static ExprNode Parse(string input)
    {
        var tokens = ExpressionTokenizer.Tokenize(input);
        var pos = 0;
        var node = ParseExpression(tokens, ref pos);

        if (pos < tokens.Count)
            throw new InvalidSyntaxException($"Unexpected token: '{tokens[pos].Text}'", tokens[pos].Position);

        return node;
    }

    private static ExprNode ParseExpression(List<Token> tokens, ref int pos)
    {
        var left = ParseTerm(tokens, ref pos);

        while (pos < tokens.Count && tokens[pos].Type == TokenType.Operator
            && (tokens[pos].Text == "+" || tokens[pos].Text == "-"))
        {
            var op = tokens[pos].Text;
            pos++;
            var right = ParseTerm(tokens, ref pos);
            left = new BinaryOpNode(op, left, right);
        }

        return left;
    }

    private static ExprNode ParseTerm(List<Token> tokens, ref int pos)
    {
        var left = ParseFactor(tokens, ref pos);

        while (pos < tokens.Count && tokens[pos].Type == TokenType.Operator
            && (tokens[pos].Text == "*" || tokens[pos].Text == "/"))
        {
            var op = tokens[pos].Text;
            pos++;
            var right = ParseFactor(tokens, ref pos);
            left = new BinaryOpNode(op, left, right);
        }

        return left;
    }

    private static ExprNode ParseFactor(List<Token> tokens, ref int pos)
    {
        if (pos >= tokens.Count)
            throw new InvalidSyntaxException("Unexpected end of expression", 0);

        var token = tokens[pos];

        if (token.Type == TokenType.Operator && (token.Text == "+" || token.Text == "-"))
        {
            pos++;
            var operand = ParseFactor(tokens, ref pos);
            return new UnaryOpNode(token.Text, operand);
        }

        if (token.Type == TokenType.LeftParen)
        {
            pos++;
            var node = ParseExpression(tokens, ref pos);

            if (pos >= tokens.Count || tokens[pos].Type != TokenType.RightParen)
                throw new InvalidSyntaxException("Expected ')'", token.Position);

            pos++;
            return node;
        }

        if (token.Type == TokenType.Number)
        {
            pos++;
            var unit = PeekUnit(tokens, ref pos);
            return new NumberNode(token.NumericValue, unit);
        }

        if (token.Type == TokenType.Unit)
        {
            pos++;
            return new NumberNode(1.0, UnitTable.GetUnit(token.Text));
        }

        throw new InvalidSyntaxException($"Unexpected token: '{token.Text}'", token.Position);
    }

    private static UnitDefinition? PeekUnit(List<Token> tokens, ref int pos)
    {
        if (pos < tokens.Count && tokens[pos].Type == TokenType.Unit)
        {
            var unit = UnitTable.GetUnit(tokens[pos].Text);
            pos++;
            return unit;
        }
        return null;
    }
}

using Hypah.Calculator.AST;
using Hypah.Calculator.Errors;

namespace Hypah.Calculator.Evaluator;

public static class ExpressionEvaluator
{
    public static double Evaluate(ExprNode node) => node switch
    {
        NumberNode n => n.Value,
        BinaryOpNode b => EvaluateBinary(b),
        UnaryOpNode u => EvaluateUnary(u),
        _ => throw new InvalidSyntaxException("Unknown node type", 0)
    };

    private static double EvaluateBinary(BinaryOpNode node)
    {
        var left = Evaluate(node.Left);
        var right = Evaluate(node.Right);

        return node.Operator switch
        {
            "+" => left + right,
            "-" => left - right,
            "*" => left * right,
            "/" => right == 0 ? throw new DivisionByZeroException() : left / right,
            _ => throw new InvalidSyntaxException($"Unknown operator: '{node.Operator}'", 0)
        };
    }

    private static double EvaluateUnary(UnaryOpNode node)
    {
        var value = Evaluate(node.Operand);
        return node.Operator switch
        {
            "+" => +value,
            "-" => -value,
            _ => throw new InvalidSyntaxException($"Unknown unary operator: '{node.Operator}'", 0)
        };
    }
}

using Hypah.Calculator.AST;
using Hypah.Calculator.Errors;
using Hypah.Calculator.Units;

namespace Hypah.Calculator.Normalizer;

public static class UnitNormalizer
{
    public static (double Value, UnitDefinition? Unit) Normalize(ExprNode node) => node switch
    {
        NumberNode n => NormalizeNumber(n),
        BinaryOpNode b => NormalizeBinary(b),
        UnaryOpNode u => NormalizeUnary(u),
        _ => throw new InvalidSyntaxException("Unknown node type", 0)
    };

    private static (double Value, UnitDefinition? Unit) NormalizeNumber(NumberNode node)
    {
        if (node.Unit == null)
            return (node.Value, null);

        return (node.Value * node.Unit.FactorToBase, node.Unit);
    }

    private static (double Value, UnitDefinition? Unit) NormalizeBinary(BinaryOpNode node)
    {
        var (leftValue, leftUnit) = Normalize(node.Left);
        var (rightValue, rightUnit) = Normalize(node.Right);

        switch (node.Operator)
        {
            case "+" or "-":
                ValidateSameCategory(leftUnit, rightUnit);
                ValidateNotLogarithmic(leftUnit, rightUnit);
                var addValue = node.Operator == "+" ? leftValue + rightValue : leftValue - rightValue;
                return (addValue, leftUnit ?? rightUnit);

            case "*":
                return Multiply(leftValue, leftUnit, rightValue, rightUnit);

            case "/":
                if (rightValue == 0)
                    throw new DivisionByZeroException();
                var divValue = leftValue / rightValue;
                var resultUnit = GetDivisionResultUnit(leftUnit, rightUnit);
                return (divValue, resultUnit);

            default:
                throw new InvalidSyntaxException($"Unknown operator: '{node.Operator}'", 0);
        }
    }

    private static (double Value, UnitDefinition? Unit) Multiply(
        double leftValue, UnitDefinition? leftUnit, double rightValue, UnitDefinition? rightUnit)
    {
        if (leftUnit != null && rightUnit != null)
        {
            if (leftUnit.Category == UnitCategory.Speed && rightUnit.Category == UnitCategory.Time)
                return (leftValue * rightValue, new UnitDefinition("m", UnitCategory.Length, 1, BaseUnitSymbol: "m"));
            if (leftUnit.Category == UnitCategory.Time && rightUnit.Category == UnitCategory.Speed)
                return (leftValue * rightValue, new UnitDefinition("m", UnitCategory.Length, 1, BaseUnitSymbol: "m"));
            if (leftUnit.Category == UnitCategory.Length && rightUnit.Category == UnitCategory.Force)
                return (leftValue * rightValue, new UnitDefinition("Nm", UnitCategory.Torque, 1, BaseUnitSymbol: "Nm"));
            if (leftUnit.Category == UnitCategory.Force && rightUnit.Category == UnitCategory.Length)
                return (leftValue * rightValue, new UnitDefinition("Nm", UnitCategory.Torque, 1, BaseUnitSymbol: "Nm"));
            throw new CalculatorException(
                $"Multiplication of two units ('{leftUnit.Symbol}' * '{rightUnit.Symbol}') is not supported");
        }

        var value = leftValue * rightValue;
        var unit = leftUnit ?? rightUnit;

        if (unit != null && unit.Category == UnitCategory.Percent)
            return (value, null);

        return (value, unit);
    }

    private static (double Value, UnitDefinition? Unit) NormalizeUnary(UnaryOpNode node)
    {
        var (value, unit) = Normalize(node.Operand);
        return (node.Operator == "-" ? -value : value, unit);
    }

    private static void ValidateSameCategory(UnitDefinition? left, UnitDefinition? right)
    {
        if (left == null || right == null)
            return;

        if (left.Category != right.Category)
            throw new UnitMismatchException(left.Symbol, right.Symbol);
    }

    private static void ValidateNotLogarithmic(UnitDefinition? left, UnitDefinition? right)
    {
        if (left?.IsLogarithmic == true || right?.IsLogarithmic == true)
            throw new CalculatorException(
                $"Cannot perform arithmetic on logarithmic unit: '{left?.Symbol}' '{right?.Symbol}'");
    }

    private static UnitDefinition? GetDivisionResultUnit(UnitDefinition? left, UnitDefinition? right)
    {
        if (left == null && right == null)
            return null;

        if (left == null)
            return right;

        if (right == null)
            return left;

        if (left.Category == right.Category)
            return null;

        if (left.Category == UnitCategory.Length && right.Category == UnitCategory.Time)
            return new UnitDefinition("m/s", UnitCategory.Speed, 1, BaseUnitSymbol: "m/s");

        return null;
    }
}

using Hypah.Calculator.Errors;
using Hypah.Calculator.Formatter;
using Hypah.Calculator.Normalizer;
using Hypah.Calculator.Parser;
using Hypah.Calculator.Units;

namespace Hypah.Calculator;

public static class Calculator
{
    public static CalculatorResult Calculate(string expression, FormatterOptions? options = null)
        => Calculate(expression, null, options);

    public static CalculatorResult Calculate(
        string expression, UnitCategory? expectedCategory, FormatterOptions? options = null)
    {
        var ast = ExpressionParser.Parse(expression);
        var (rawValue, unit) = UnitNormalizer.Normalize(ast);

        var baseUnit = unit?.EffectiveBase ?? "dimensionless";
        var prettyFormatted = ResultFormatter.Format(rawValue, unit, options);
        var category = unit?.Category ?? UnitCategory.None;

        if (expectedCategory.HasValue && category != UnitCategory.None && category != expectedCategory.Value)
            throw new UnitMismatchException(baseUnit, expectedCategory.Value.ToString());

        return new CalculatorResult(rawValue, baseUnit,
            expectedCategory ?? category, prettyFormatted);
    }
}

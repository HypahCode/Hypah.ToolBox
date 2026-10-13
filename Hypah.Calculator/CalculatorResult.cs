using Hypah.Calculator.Units;

namespace Hypah.Calculator;

public sealed record CalculatorResult(
    double RawValue,
    string BaseUnit,
    UnitCategory Category,
    string PrettyFormatted);

namespace Hypah.Calculator.Units;

public sealed record UnitDefinition(
    string Symbol,
    UnitCategory Category,
    double FactorToBase,
    bool IsComposite = false,
    string? NumeratorUnit = null,
    string? DenominatorUnit = null,
    string? BaseUnitSymbol = null,
    bool IsLogarithmic = false)
{
    public string EffectiveBase => BaseUnitSymbol ?? Symbol;
}

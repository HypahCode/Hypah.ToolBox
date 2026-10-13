using Hypah.Calculator.Errors;

namespace Hypah.Calculator.Units;

public static class UnitTable
{
    private static readonly Dictionary<string, UnitDefinition> _units = new()
    {
        ["ms"] = new("ms", UnitCategory.Time, 0.001, BaseUnitSymbol: "s"),
        ["s"] = new("s", UnitCategory.Time, 1, BaseUnitSymbol: "s"),
        ["min"] = new("min", UnitCategory.Time, 60, BaseUnitSymbol: "s"),
        ["h"] = new("h", UnitCategory.Time, 3600, BaseUnitSymbol: "s"),

        ["g"] = new("g", UnitCategory.Weight, 1, BaseUnitSymbol: "g"),
        ["kg"] = new("kg", UnitCategory.Weight, 1000, BaseUnitSymbol: "g"),

        ["mm"] = new("mm", UnitCategory.Length, 0.001, BaseUnitSymbol: "m"),
        ["cm"] = new("cm", UnitCategory.Length, 0.01, BaseUnitSymbol: "m"),
        ["m"] = new("m", UnitCategory.Length, 1, BaseUnitSymbol: "m"),
        ["km"] = new("km", UnitCategory.Length, 1000, BaseUnitSymbol: "m"),

        ["ml"] = new("ml", UnitCategory.Volume, 0.001, BaseUnitSymbol: "l"),
        ["l"] = new("l", UnitCategory.Volume, 1, BaseUnitSymbol: "l"),

        ["deg"] = new("deg", UnitCategory.Angle, Math.PI / 180, BaseUnitSymbol: "rad"),
        ["rad"] = new("rad", UnitCategory.Angle, 1, BaseUnitSymbol: "rad"),

        ["%"] = new("%", UnitCategory.Percent, 0.01, BaseUnitSymbol: "%"),

        ["m/s"] = new("m/s", UnitCategory.Speed, 1, IsComposite: true,
            NumeratorUnit: "m", DenominatorUnit: "s", BaseUnitSymbol: "m/s"),
        ["km/h"] = new("km/h", UnitCategory.Speed, 1000.0 / 3600.0, IsComposite: true,
            NumeratorUnit: "km", DenominatorUnit: "h", BaseUnitSymbol: "m/s"),

        ["J"] = new("J", UnitCategory.Energy, 1, BaseUnitSymbol: "J"),
        ["kJ"] = new("kJ", UnitCategory.Energy, 1000, BaseUnitSymbol: "J"),
        ["MJ"] = new("MJ", UnitCategory.Energy, 1_000_000, BaseUnitSymbol: "J"),

        ["bps"] = new("bps", UnitCategory.Bitrate, 1, BaseUnitSymbol: "bps"),
        ["kbps"] = new("kbps", UnitCategory.Bitrate, 1_000, BaseUnitSymbol: "bps"),
        ["Mbps"] = new("Mbps", UnitCategory.Bitrate, 1_000_000, BaseUnitSymbol: "bps"),
        ["Gbps"] = new("Gbps", UnitCategory.Bitrate, 1_000_000_000, BaseUnitSymbol: "bps"),

        ["B"] = new("B", UnitCategory.Memory, 1, BaseUnitSymbol: "B"),
        ["KB"] = new("KB", UnitCategory.Memory, 1024, BaseUnitSymbol: "B"),
        ["MB"] = new("MB", UnitCategory.Memory, 1024 * 1024, BaseUnitSymbol: "B"),
        ["GB"] = new("GB", UnitCategory.Memory, 1024 * 1024 * 1024, BaseUnitSymbol: "B"),
        ["TB"] = new("TB", UnitCategory.Memory, 1024L * 1024 * 1024 * 1024, BaseUnitSymbol: "B"),

        ["fps"] = new("fps", UnitCategory.FrameRate, 1, BaseUnitSymbol: "fps"),

        ["MB/s"] = new("MB/s", UnitCategory.Bandwidth, 1_000_000, IsComposite: true,
            NumeratorUnit: "MB", DenominatorUnit: "s", BaseUnitSymbol: "B/s"),
        ["GB/s"] = new("GB/s", UnitCategory.Bandwidth, 1_000_000_000, IsComposite: true,
            NumeratorUnit: "GB", DenominatorUnit: "s", BaseUnitSymbol: "B/s"),

        ["N"] = new("N", UnitCategory.Force, 1, BaseUnitSymbol: "N"),
        ["kN"] = new("kN", UnitCategory.Force, 1_000, BaseUnitSymbol: "N"),

        ["Pa"] = new("Pa", UnitCategory.Pressure, 1, BaseUnitSymbol: "Pa"),
        ["kPa"] = new("kPa", UnitCategory.Pressure, 1_000, BaseUnitSymbol: "Pa"),
        ["bar"] = new("bar", UnitCategory.Pressure, 100_000, BaseUnitSymbol: "Pa"),

        ["kg/m3"] = new("kg/m3", UnitCategory.Density, 1, IsComposite: true,
            NumeratorUnit: "kg", DenominatorUnit: "m3", BaseUnitSymbol: "kg/m3"),

        ["l/s"] = new("l/s", UnitCategory.FlowRate, 1, IsComposite: true,
            NumeratorUnit: "l", DenominatorUnit: "s", BaseUnitSymbol: "l/s"),
        ["ml/s"] = new("ml/s", UnitCategory.FlowRate, 0.001, IsComposite: true,
            NumeratorUnit: "ml", DenominatorUnit: "s", BaseUnitSymbol: "l/s"),

        ["Nm"] = new("Nm", UnitCategory.Torque, 1, BaseUnitSymbol: "Nm"),

        ["W"] = new("W", UnitCategory.Power, 1, BaseUnitSymbol: "W"),
        ["kW"] = new("kW", UnitCategory.Power, 1_000, BaseUnitSymbol: "W"),

        ["Hz"] = new("Hz", UnitCategory.Frequency, 1, BaseUnitSymbol: "Hz"),
        ["kHz"] = new("kHz", UnitCategory.Frequency, 1_000, BaseUnitSymbol: "Hz"),

        ["nm"] = new("nm", UnitCategory.Wavelength, 1e-9, BaseUnitSymbol: "m"),
        ["um"] = new("um", UnitCategory.Wavelength, 1e-6, BaseUnitSymbol: "m"),

        ["V"] = new("V", UnitCategory.Voltage, 1, BaseUnitSymbol: "V"),

        ["A"] = new("A", UnitCategory.Current, 1, BaseUnitSymbol: "A"),

        ["Ω"] = new("Ω", UnitCategory.Resistance, 1, BaseUnitSymbol: "Ω"),

        ["lm"] = new("lm", UnitCategory.Lumens, 1, BaseUnitSymbol: "lm"),

        ["K"] = new("K", UnitCategory.ColorTemperature, 1, BaseUnitSymbol: "K"),

        ["dB"] = new("dB", UnitCategory.SoundLevel, 1, BaseUnitSymbol: "dB", IsLogarithmic: true),
    };

    public static bool TryGetUnit(string symbol, out UnitDefinition? definition)
        => _units.TryGetValue(symbol, out definition);

    public static UnitDefinition GetUnit(string symbol)
    {
        if (!_units.TryGetValue(symbol, out var def))
            throw new UnknownUnitException(symbol);
        return def;
    }

    public static IEnumerable<string> AllSymbols => _units.Keys;

    public static IEnumerable<string> CompositeSymbols =>
        _units.Where(kv => kv.Value.IsComposite).Select(kv => kv.Key);

    public static void Register(UnitDefinition definition)
        => _units[definition.Symbol] = definition;
}

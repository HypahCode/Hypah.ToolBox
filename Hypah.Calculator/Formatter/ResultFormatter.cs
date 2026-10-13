using Hypah.Calculator.Units;

namespace Hypah.Calculator.Formatter;

public sealed record FormatterOptions(
    int DecimalPlaces = 3,
    bool UseCompoundFormat = true,
    double ThresholdForUnitSwitch = 1.0);

public static class ResultFormatter
{
    private static readonly Dictionary<UnitCategory, List<string>> _formatOrder = new()
    {
        [UnitCategory.Time] = new() { "h", "min", "s", "ms" },
        [UnitCategory.Length] = new() { "km", "m", "cm", "mm" },
        [UnitCategory.Weight] = new() { "kg", "g" },
        [UnitCategory.Volume] = new() { "l", "ml" },
        [UnitCategory.Energy] = new() { "MJ", "kJ", "J" },
        [UnitCategory.Angle] = new() { "rad", "deg" },
        [UnitCategory.Percent] = new() { "%" },
        [UnitCategory.Speed] = new() { "m/s", "km/h" },
        [UnitCategory.Memory] = new() { "TB", "GB", "MB", "KB", "B" },
        [UnitCategory.Bitrate] = new() { "Gbps", "Mbps", "kbps", "bps" },
        [UnitCategory.Force] = new() { "kN", "N" },
        [UnitCategory.Pressure] = new() { "bar", "kPa", "Pa" },
        [UnitCategory.Power] = new() { "kW", "W" },
        [UnitCategory.Frequency] = new() { "kHz", "Hz" },
        [UnitCategory.Wavelength] = new() { "m", "um", "nm" },
        [UnitCategory.FrameRate] = new() { "fps" },
        [UnitCategory.Force] = new() { "kN", "N" },
        [UnitCategory.Pressure] = new() { "bar", "kPa", "Pa" },
        [UnitCategory.Density] = new() { "kg/m3" },
        [UnitCategory.FlowRate] = new() { "l/s", "ml/s" },
        [UnitCategory.Torque] = new() { "Nm" },
        [UnitCategory.Power] = new() { "kW", "W" },
        [UnitCategory.Voltage] = new() { "V" },
        [UnitCategory.Current] = new() { "A" },
        [UnitCategory.Resistance] = new() { "Ω" },
        [UnitCategory.Lumens] = new() { "lm" },
        [UnitCategory.ColorTemperature] = new() { "K" },
        [UnitCategory.SoundLevel] = new() { "dB" },
        [UnitCategory.Latency] = new() { "h", "min", "s", "ms" },
    };

    private static readonly HashSet<UnitCategory> _compoundCategories = new()
    {
        UnitCategory.Time,
        UnitCategory.Length,
        UnitCategory.Volume,
        UnitCategory.Memory,
    };

    public static string Format(double baseValue, UnitDefinition? unit, FormatterOptions? options = null)
    {
        options ??= new FormatterOptions();

        if (unit == null)
            return Math.Round(baseValue, options.DecimalPlaces).ToString($"F{options.DecimalPlaces}");

        if (unit.Category == UnitCategory.Percent)
            return $"{Math.Round(baseValue * 100, options.DecimalPlaces)}%";

        if (unit.Category == UnitCategory.Speed)
            return FormatSpeed(baseValue, options);

        if (unit.Category == UnitCategory.Angle)
            return FormatAngle(baseValue, options);

        if (unit.Category == UnitCategory.SoundLevel)
            return $"{Math.Round(baseValue, options.DecimalPlaces)}dB";

        if (options.UseCompoundFormat && _compoundCategories.Contains(unit.Category))
            return FormatCompound(baseValue, unit.Category, options);

        return FormatBestUnit(baseValue, unit.Category, options);
    }

    private static string FormatCompound(double baseValue, UnitCategory category, FormatterOptions options)
    {
        var order = _formatOrder[category];
        var parts = new List<string>();
        var remaining = Math.Abs(baseValue);
        var sign = baseValue < 0 ? "-" : "";

        foreach (var symbol in order)
        {
            if (!UnitTable.TryGetUnit(symbol, out var def) || def is null || def.FactorToBase <= 0)
                continue;

            var unitValue = remaining / def.FactorToBase;
            var intPart = Math.Floor(unitValue);

            if (intPart >= 1 || (parts.Count == 0 && symbol == order[^1]))
            {
                parts.Add($"{intPart}{symbol}");
                remaining -= intPart * def.FactorToBase;
            }
        }

        return parts.Count > 0 ? sign + string.Join(" ", parts) : $"{sign}0{order[^1]}";
    }

    private static string FormatBestUnit(double baseValue, UnitCategory category, FormatterOptions options)
    {
        if (_formatOrder.TryGetValue(category, out var order))
        {
            foreach (var symbol in order)
            {
                if (!UnitTable.TryGetUnit(symbol, out var def) || def is null || def.FactorToBase <= 0)
                    continue;

                var displayValue = baseValue / def.FactorToBase;
                if (Math.Abs(displayValue) >= 1 || symbol == order[^1])
                    return $"{Math.Round(displayValue, options.DecimalPlaces)}{symbol}";
            }
        }

        if (UnitTable.TryGetUnit(category.ToString(), out var baseDef) && baseDef != null)
        {
            var displayValue = baseValue / baseDef.FactorToBase;
            return $"{Math.Round(displayValue, options.DecimalPlaces)}{baseDef.Symbol}";
        }

        return Math.Round(baseValue, options.DecimalPlaces).ToString($"F{options.DecimalPlaces}");
    }

    private static string FormatSpeed(double baseValueMs, FormatterOptions options)
    {
        var kmh = baseValueMs * 3.6;
        return Math.Abs(kmh) >= 1
            ? $"{Math.Round(kmh, options.DecimalPlaces)}km/h"
            : $"{Math.Round(baseValueMs, options.DecimalPlaces)}m/s";
    }

    private static string FormatAngle(double baseValueRad, FormatterOptions options)
    {
        var deg = baseValueRad * 180.0 / Math.PI;
        return Math.Abs(deg) >= 1
            ? $"{Math.Round(deg, options.DecimalPlaces)}deg"
            : $"{Math.Round(baseValueRad, options.DecimalPlaces)}rad";
    }
}

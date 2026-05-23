namespace Hypah.Utility.Primitives
{
    public static class Decimal
    {
        public static decimal Parse(string str) => decimal.Parse(str, System.Globalization.CultureInfo.InvariantCulture);
        public static bool TryParse(string str, out decimal value)
        {
            if (string.IsNullOrEmpty(str))
            {
                value = 0;
                return false;
            }
            return decimal.TryParse(str, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out value);
        }

        public static string ToString(decimal value) => value.ToString(System.Globalization.CultureInfo.InvariantCulture);
    }
}
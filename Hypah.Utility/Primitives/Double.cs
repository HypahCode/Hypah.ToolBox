namespace Hypah.Utility.Primitives
{
    public static class Double
    {
        public static double Parse(string str) => double.Parse(str, System.Globalization.CultureInfo.InvariantCulture);
        public static bool TryParse(string str, out double value)
        {
            if (string.IsNullOrEmpty(str))
            {
                value = 0;
                return false;
            }
            return double.TryParse(str, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out value);
        }

        public static string ToString(double value) => value.ToString(System.Globalization.CultureInfo.InvariantCulture);
    }
}


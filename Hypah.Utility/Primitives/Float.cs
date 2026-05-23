
namespace Hypah.Utility.Primitives
{
    public static class Float
    {
        public static float Parse(string str) => float.Parse(str, System.Globalization.CultureInfo.InvariantCulture);
        public static bool TryParse(string str, out float value)
        {
            if (string.IsNullOrEmpty(str))
            {
                value = 0;
                return false;
            }
            return float.TryParse(str, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out value);
        }

        public static string ToString(float value) => value.ToString(System.Globalization.CultureInfo.InvariantCulture);
    }
}

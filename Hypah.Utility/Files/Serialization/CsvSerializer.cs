namespace Hypah.Utility.Files.Serialization
{
    public class CsvSerializer<T> where T : class, new()
    {
        public string Seperator { get; set; } = ",";
        public bool Trim { get; set; } = true;

        public IEnumerable<T> Read(string filename)
        {
            var lines = File.ReadAllLines(filename);
            if (lines.Length == 0)
                yield break;

            var props = typeof(T).GetProperties();
            var header = lines[0].Split(Seperator);
            var propMap = header
                .Select((h, i) => new { Index = i, Prop = props.FirstOrDefault(p => p.Name == h) })
                .Where(x => x.Prop != null)
                .ToArray();

            for (int i = 1; i < lines.Length; i++)
            {
                var line = lines[i];
                var fields = ParseCsvLine(line, Seperator);
                var obj = new T();
                foreach (var map in propMap)
                {
                    if (map.Index < fields.Length)
                    {
                        var value = fields[map.Index];
                        if (Trim) value = value.Trim();
                        var propType = map.Prop!.PropertyType;
                        object? converted = string.IsNullOrEmpty(value) ? null : Convert.ChangeType(value, propType);
                        map.Prop.SetValue(obj, converted);
                    }
                }
                yield return obj;
            }
        }

        private static string[] ParseCsvLine(string line, string separator)
        {
            var result = new List<string>();
            bool inQuotes = false;
            var value = new System.Text.StringBuilder();
            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];
                if (c == '"')
                {
                    if (inQuotes && i + 1 < line.Length && line[i + 1] == '"')
                    {
                        value.Append('"');
                        i++;
                    }
                    else
                    {
                        inQuotes = !inQuotes;
                    }
                }
                else if (!inQuotes && line.Substring(i).StartsWith(separator))
                {
                    result.Add(value.ToString());
                    value.Clear();
                    i += separator.Length - 1;
                }
                else
                {
                    value.Append(c);
                }
            }
            result.Add(value.ToString());
            return result.ToArray();
        }

        public void Write(string filename, IEnumerable<T> values)
        {
            using (var writer = new StreamWriter(filename))
            {
                writer.WriteLine(CreateHeaderLine());
                foreach (var line in CreateLines(values))
                {
                    writer.WriteLine(line);
                }
            }
        }

        public void WirteAppend(string filename, IEnumerable<T> values)
        {
            if (!File.Exists(filename))
            {
                Write(filename, values);
                return;
            }

            using (var writer = new StreamWriter(filename, true))
            {
                foreach (var line in CreateLines(values))
                {
                    writer.WriteLine(line);
                }
            }
        }

        private string CreateHeaderLine()
        {
            var props = typeof(T).GetProperties();
            return string.Join(Seperator, props.Select(p => p.Name));
        }

        private IEnumerable<string> CreateLines(IEnumerable<T> values)
        {
            var props = typeof(T).GetProperties();
            foreach (var value in values)
            {
                var fields = props.Select(p =>
                {
                    var val = p.GetValue(value);
                    var str = val?.ToString() ?? string.Empty;
                    if (Trim) str = str.Trim();
                    // Escape separator and quotes
                    if (str.Contains(Seperator) || str.Contains("\""))
                    {
                        str = "\"" + str.Replace("\"", "\"\"") + "\"";
                    }
                    return str;
                });
                yield return string.Join(Seperator, fields);
            }
        }
    }
}

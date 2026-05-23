
namespace Hypah.WebServer.Request
{
    public sealed class WebUrl
    {
        private readonly string _url;
        internal WebUrl(string completeUrl)
        {
            _url = ExtractSimpleUrl(completeUrl);
        }

        private string ExtractSimpleUrl(string url)
        {
            // in: GET /page?query HTTP/1.1 and moar
            // out: page?query


            string[] content = url.Split(' ');
            if (content.Length >= 1)
            {
                return content[1].Substring(1);
            }
            return "";
        }

        public string GetFile()
        {
            string[] content = _url.Split('?');
            if (content.Length >= 1)
            {
                return content[0];
            }
            return "";
        }

        internal string[] GetQuery()
        {
            int idx = _url.IndexOf("?");
            if (idx != -1)
            {
                string q = _url.Substring(idx + 1);
                return q.Split('&');
            }
            return [];
        }

        public override string ToString()
        {
            return _url;
        }

        public string GetValueFor(string key, string returnDefault)
        {
            string[] args = GetQuery();
            for (int i = 0; i < args.Length; i++)
            {
                KeyValuePair<string, string> pair = GetKeyValue(args[i]);
                if (key == pair.Key)
                {
                    var value = pair.Value;
                    value = value.Replace("%20", " ");
                    return value;
                }
            }
            return returnDefault;
        }

        private KeyValuePair<string, string> GetKeyValue(string s)
        {
            int index = s.IndexOf('=');
            if (index != -1)
            {
                return new KeyValuePair<string, string>(s.Substring(0, index), s.Substring(index + 1));
            }
            else
            {
                return new KeyValuePair<string, string>(s, "");
            }
        }

        public bool TryGetObject<T>(out T value) where T : class, new()
        {
            try
            {
                value = GetObject<T>();
                return true;
            }
            catch
            {
                value = new T();
                return false;
            }
        }

        public T GetObject<T>() where T : class, new()
        {
            var obj = Activator.CreateInstance<T>();
            var props = typeof(T).GetProperties();

            foreach (var prop in props)
            {
                var value = GetValueFor(prop.Name, "");
                if (prop.PropertyType.IsEnum)
                {
                    var enumValue = Enum.Parse(prop.PropertyType, value);
                    prop.SetValue(obj, enumValue);
                    continue;
                }
                else if (prop.PropertyType == typeof(bool))
                {
                    prop.SetValue(obj, !string.IsNullOrEmpty(value)); // bool is a special case...
                }
                if (value != "")
                {
                    try
                    {
                        prop.SetValue(obj, Convert.ChangeType(value, prop.PropertyType));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error setting property {prop.Name}: {ex.Message}");
                    }
                }
            }
            return obj;
        }
    }
}

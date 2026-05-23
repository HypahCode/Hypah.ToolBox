using Hypah.WebServer.Html.HtmlElements.FormAttributes;
using System.Reflection;
using System.Text;

namespace Hypah.WebServer.Html.HtmlElements
{
    public class HtmlForm : HtmlBuilder
    {
        protected override string HtmlTag => "form";

        public HtmlForm(string action)
        {
            Properties["action"] = action;
            //Properties["method"] = "post"; // Default to POST method
        }

        public HtmlForm(string action, object obj)
             : this(action)
        {
            AppendObject(obj);
        }

        private void AppendObject(object obj)
        {
            var sb = new StringBuilder();
            foreach (var property in obj.GetType().GetProperties())
            {
                if (property.GetCustomAttribute<HiddenAttribute>() != null)
                {
                    Add(HtmlInput.Hidden(property.Name, property.GetValue(obj)?.ToString() ?? ""));
                    continue;
                }

                if (property.PropertyType == typeof(bool))
                {
                    Add(HtmlInput.CheckBox(property.Name, (bool)property.GetValue(obj)!));
                    Add(new HtmlLabel(GetDisplayName(property), property.Name));
                    Add(new HtmlBr());
                    Add(new HtmlBr());
                    continue;
                }

                Add(new HtmlLabel(GetDisplayName(property), property.Name));
                Add(new HtmlBr());
                if (property.PropertyType == typeof(decimal) ||
                    property.PropertyType == typeof(double) || 
                    property.PropertyType == typeof(float))
                {
                    Add(HtmlInput.Number(property.Name, property.GetValue(obj)!.ToString()!));
                    Add(new HtmlBr());
                    Add(new HtmlBr());
                    continue;
                }
                if (property.PropertyType == typeof(long) ||
                    property.PropertyType == typeof(int))
                {
                    Add(HtmlInput.Number(property.Name, property.GetValue(obj)!.ToString()!, "1"));
                    Add(new HtmlBr());
                    Add(new HtmlBr());
                    continue;
                }
                if (property.PropertyType == typeof(byte))
                {
                    Add(HtmlInput.Number(property.Name, property.GetValue(obj)!.ToString()!, "1", "0", "255"));
                    Add(new HtmlBr());
                    Add(new HtmlBr());
                    continue;
                }

                Add(HtmlInput.Text(property.Name, property.GetValue(obj)?.ToString() ?? ""));
                Add(new HtmlBr());
                Add(new HtmlBr());
            }
            Add(HtmlInput.SubmitButton("Submit"));
        }

        private string GetDisplayName(PropertyInfo property)
        {
            var displayName = property.GetCustomAttribute<DisplayNameAttribute>();
            if (displayName != null)
            {
                return displayName.DisplayName;
            }
            return property.Name;
        }
    }
}

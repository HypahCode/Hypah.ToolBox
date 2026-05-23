using Hypah.WebServer.Html.DefaultPageBuilder;

namespace Hypah.WebServer.Html.HtmlElements
{
    public interface IParameterDecorator
    {
        IHtmlBuildable? DecorateValue(object? value, Type type, string name);
    }

    public class HtmlTable : HtmlBuilder
    {
        protected override string HtmlTag => "table";
        public List<HtmlTh> Headers { get; } = new List<HtmlTh>();
        public bool AutoAlignColumnsToHeaders { get; set; } = true;

        public HtmlTable(params string[] headers)
        {
            var row = Add(new HtmlTr());
            foreach (var header in headers)
            {
                var th = new HtmlTh(header);
                row.Add(th);
                Headers.Add(th);
            }
        }

        public HtmlTable(params HtmlTh[] headers)
        {
            var row = Add(new HtmlTr());
            foreach (var th in headers)
            {
                row.Add(th);
                Headers.Add(th);
            }
        }

        public void AddRow(params object[] items)
        {
            if (items.Length != Headers.Count) throw new Exception("Html table column count and item count is not equal!");
            var row = Add(new HtmlTr());
            int index = 0;
            foreach (var item in items)
            {
                var td = new HtmlTd();
                if (AutoAlignColumnsToHeaders)
                {
                    td.Alignment = Headers[index++].Alignment;
                }

                if (item is IHtmlBuildable builder)
                    td.Add(builder);
                else
                    td.Add(new HtmlText(item.ToString()!));
                row.Add(td);
            }
        }

        public static HtmlTable FromObject(object obj, IParameterDecorator? decorator = null)
        {
            var table = new HtmlTable("Property", "Value");
            table.AddRowsFromObject(obj, decorator);
            return table;
        }

        public static HtmlTable FromObjects(IEnumerable<object> objects, IParameterDecorator? decorator = null)
        {
            var table = new HtmlTable("Property", "Value");
            foreach (var o in objects)
            {
                table.AddRowsFromObject(o, decorator);
            }
            return table;
        }

        public void AddRowsFromObject(object obj, IParameterDecorator? decorator = null)
        {
            obj.GetType().GetProperties().ToList().ForEach(x =>
            {
                var row = Add(new HtmlTr());
                var td = new HtmlTd();
                td.Add(new HtmlText(x.Name));
                row.Add(td);
                td = new HtmlTd();
                if (x.GetValue(obj) is IHtmlBuildable builder)
                {
                    td.Add(builder);
                }
                else
                {
                    if (decorator != null)
                    {
                        var decoratedValue = decorator.DecorateValue(x.GetValue(obj), x.PropertyType, x.Name);
                        if (decoratedValue != null)
                        {
                            td.Add(decoratedValue);
                        }
                        else
                        {
                            td.Add(new HtmlText(x.GetValue(obj)?.ToString()!));
                        }
                    }
                    else
                    {
                        td.Add(new HtmlText(x.GetValue(obj)?.ToString()!));
                    }
                }
                row.Add(td);
            });
        }

        public override string BuildHtml(ICssBuilder cssBuilder)
        {
            if (string.IsNullOrEmpty(Id))
            {
                cssBuilder.Set("#defaulttable").Set("border-collapse", "collapse");
                cssBuilder.Set("#defaulttable").Set("width", "100%");

                //cssBuilder.Set("#defaulttable td, #defaulttable th").Set("border", $"1px solid {DefaultColors.TableBorderColor}");
                cssBuilder.Set("#defaulttable td, #defaulttable th").Set("padding", "4px");

                cssBuilder.Set("#defaulttable tr:nth-child(even)").Set("background-color", DefaultColors.TableEvenBackground);

                cssBuilder.Set("#defaulttable tr:hover").Set("background-color", DefaultColors.TableHoverColor);

                cssBuilder.Set("#defaulttable th").Set("padding-top", "8px");
                cssBuilder.Set("#defaulttable th").Set("padding-bottom", "8px");
                cssBuilder.Set("#defaulttable th").Set("text-align", "left");
                cssBuilder.Set("#defaulttable th").Set("background-color", DefaultColors.TableHeaderColor);
                cssBuilder.Set("#defaulttable th").Set("color", "white");

                cssBuilder.Set("#defaulttable th").Set("border", $"1px solid {DefaultColors.ContentBackground}");
                cssBuilder.Set("#defaulttable tr").Set("border", $"1px solid {DefaultColors.ContentBackground}");

            }
            return base.BuildHtml(cssBuilder);
        }

        protected override void FillProperties()
        {
            base.FillProperties();
            if (string.IsNullOrEmpty(Id))
            {
                Properties["id"] = "defaulttable";
            }
        }

        public static IHtmlBuildable FromList<T>(IEnumerable<T> items)
        {
            var headers = typeof(T).GetProperties().Select(x => x.Name).ToArray();
            var table = new HtmlTable(headers);

            foreach (var item in items)
            {
                var row = table.Add(new HtmlTr());
                foreach (var header in headers)
                {
                    var td = new HtmlTd();
                    var value = item.GetType().GetProperty(header)?.GetValue(item);
                    if (value is IHtmlBuildable builder)
                        td.Add(builder);
                    else
                        td.Add(new HtmlText(value?.ToString()!));
                    row.Add(td);
                }
            }
            return table;
        }
    }
}


namespace Hypah.WebServer.Html.DefaultPageBuilder
{
    internal static class DefaultHtml
    {
        public static string BuildHtmlPage(string title, string inlineCss, string body)
        {
            return $@"
<!DOCTYPE html>
<html lang=""en-US"">
<head>
    <meta content=""text/html;charset=utf-8"" http-equiv=""Content-Type"">
    <meta content=""utf-8"" http-equiv=""encoding"">
    <title>{title}</title>
<style type=""text/css"">
{inlineCss}
</style>
</head>
<body bgcolor=#000000 text=#EEEEEE>
{body}
</body>
</html>
";
        }
    }
}

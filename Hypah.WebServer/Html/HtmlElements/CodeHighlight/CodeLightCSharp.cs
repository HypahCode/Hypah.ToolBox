
namespace Hypah.WebServer.Html.HtmlElements.CodeHighlight
{
    using System.Text;

    namespace Hypah.DebugServer.Web.Builders
    {
        internal sealed class CodeLightCSharp : CodeLightBase
        {
            private readonly string _code;
            private readonly StringBuilder _sb = new();
            private readonly List<int>? _highlightedLines = new List<int>();

            private CodeLightCSharp(string c, IEnumerable<int>? highlightedLines = null)
            {
                _code = c;
                if (highlightedLines != null)
                {
                    _highlightedLines.AddRange(highlightedLines);
                }
            }

            public static string ToHtml(string code, IEnumerable<int>? highlightedLines = null)
            {
                CodeLightCSharp builder = new(code, highlightedLines);
                builder.FiddleWithcode();
                return "<pre>" + builder._sb.ToString() + "</pre>";
            }

            private void FiddleWithcode()
            {
                StringReader strReader = new(_code);
                int lineNum = 0;
                while (true)
                {
                    string? line = strReader.ReadLine();
                    lineNum++;
                    if (line == null)
                        break;
                    ProcessLine(line, _highlightedLines.Contains(lineNum));
                }
            }

            private void ProcessLine(string l, bool isHighlighted)
            {
                l = l.Replace("&", "&amp;");
                l = l.Replace("<", "&lt;");
                l = l.Replace(">", "&gt;");

                string t = l.Trim();
                if (isHighlighted)
                {
                    _sb.Append("<font style=\"background-color:tomato;\">" + l + "</font>\n");
                    return;
                }
                if (t == "")
                {
                    _sb.Append(l + "\n");
                    return;
                }
                if (t.StartsWith("#include"))
                {
                    _sb.Append($"<font color={_includeColor}>" + l + "</font>\n");
                    return;
                }
                if (t.StartsWith("//") || t.StartsWith("#"))
                {
                    _sb.Append($"<font color={_commentColor}>" + l + "</font>\n");
                    return;
                }
                if (t.StartsWith("[") && t.EndsWith("]"))
                {
                    _sb.Append($"<font color={_attributeColor}>" + l + "</font>\n");
                    return;
                }

                int commentIdx = l.IndexOf("//");
                string comment = "";
                if (commentIdx > 0)
                {
                    comment = l.Substring(commentIdx);
                    l = l.Substring(0, commentIdx);
                }

                l = l.Replace("/", "&#47;");
                l = l.Replace("=", "&#61;");

                // Operators
                l = HighlightReplace(l, _operatorColor,
                    "&#61;", // = equal
                    "+",
                    "-",
                    "*",
                    "^",
                    "&#47;", // / divide
                    "&lt;", // <
                    "&gt;", // <
                    ",",
                    ".",
                    "(",
                    ")");

                // Keywords
                l = Highlight(l, _keywordColor,
                    "abstract", "as", "base", "bool", "break", "byte", "case", "catch", "char", "checked",
                    "class", "const", "continue", "decimal", "default", "delegate", "do", "double", "else",
                    "enum", "event", "explicit", "extern", "false", "finally", "fixed", "float", "for",
                    "foreach", "goto", "if", "implicit", "in", "int", "interface", "internal", "is", "lock",
                    "long", "namespace", "new", "null", "object", "operator", "out", "override", "params",
                    "private", "protected", "public", "readonly", "ref", "return", "sbyte", "sealed",
                    "short", "sizeof", "stackalloc", "static", "string", "struct", "switch", "this",
                    "throw", "true", "try", "typeof", "uint", "ulong", "unchecked", "unsafe", "ushort",
                    "using", "virtual", "void", "volatile", "while", "add", "allows", "alias", "and", "ascending",
                    "args", "async", "await", "by", "descending", "dynamic", "equals", "field", "file", "from", "get",
                    "global", "group", "init", "into", "join", "let", "managed", "nameof", "nint", "not", "notnull",
                    "nuint", "on", "or", "orderby", "partial", "record", "remove", "required", "scoped", "select",
                    "set", "unmanaged", "value", "var", "when", "where", "with", "yield");

                // Shader styled stuff
                l = Highlight(l, _typeColor,
                    "bool",
                    "int",
                    "short",
                    "Vector2",
                    "Vector3",
                    "Vector4",
                    "float",
                    "float2",
                    "float3",
                    "float4",
                    "matrix",
                    "float3x3",
                    "float4x4",
                    "Texture2D",
                    "SamplerState");

                l = Highlight(l, _hlslFuncColor,
                    "abort", "abs", "acos", "all", "AllMemoryBarrier", "AllMemoryBarrierWithGroupSync", "any", "asdouble", "asfloat", "asin",
                    "asint", "asuint", "asuint", "atan", "atan2", "ceil", "CheckAccessFullyMapped", "clamp", "clip", "cos", "cosh",
                    "countbits", "cross", "D3DCOLORtoUBYTE4", "ddx", "ddx_coarse", "ddx_fine", "ddy", "ddy_coarse", "ddy_fine", "degrees",
                    "determinant", "DeviceMemoryBarrier", "DeviceMemoryBarrierWithGroupSync", "distance", "dot", "dst", "errorf",
                    "EvaluateAttributeCentroid", "EvaluateAttributeAtSample", "EvaluateAttributeSnapped", "exp", "exp2", "f16tof32", "f32tof16",
                    "faceforward", "firstbithigh", "firstbitlow", "floor", "fma", "fmod", "frac", "frexp", "fwidth", "GetRenderTargetSampleCount",
                    "GetRenderTargetSamplePosition", "GroupMemoryBarrier", "GroupMemoryBarrierWithGroupSync", "InterlockedAdd", "InterlockedAnd",
                    "InterlockedCompareExchange", "InterlockedCompareStore", "InterlockedExchange", "InterlockedMax", "InterlockedMin",
                    "InterlockedOr", "InterlockedXor", "isfinite", "isinf", "isnan", "ldexp", "length", "lerp", "lit", "log", "log10",
                    "log2", "mad", "max", "min", "modf", "msad4", "mul", "noise", "normalize", "pow", "printf", "Process2DQuadTessFactorsAvg",
                    "Process2DQuadTessFactorsMax", "Process2DQuadTessFactorsMin", "ProcessIsolineTessFactors", "ProcessQuadTessFactorsAvg",
                    "ProcessQuadTessFactorsMax", "ProcessQuadTessFactorsMin", "ProcessTriTessFactorsAvg", "ProcessTriTessFactorsMax",
                    "ProcessTriTessFactorsMin", "radians", "rcp", "reflect", "refract", "reversebits", "round", "rsqrt", "saturate", "sign",
                    "sin", "sincos", "sinh", "smoothstep", "sqrt", "step", "tan", "tanh", "tex1D", "tex1D", "tex1Dbias", "tex1Dgrad",
                    "tex1Dlod", "tex1Dproj", "tex2D", "tex2D", "tex2Dbias", "tex2Dgrad", "tex2Dlod", "tex2Dproj", "tex3D", "tex3D",
                    "tex3Dbias", "tex3Dgrad", "tex3Dlod", "tex3Dproj", "texCUBE", "texCUBE", "texCUBEbias", "texCUBEgrad", "texCUBElod",
                    "texCUBEproj", "transpose", "trunc");

                if (comment != "")
                    l += "<font color=#00e68e>" + comment + "</font>";
                _sb.Append(l + "\n");
            }
        }
    }
}

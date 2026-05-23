namespace Hypah.WebServer.Html.HtmlElements.CanvasImages
{
    internal static class CanvasImageJs
    {
        public const string script = @"
var array = [
[[ARRAYDATA]]
];

init();
function init()
{
    drawPicture();
}


function setPixel(ctx, x, y, r, g, b, a) 
{
    // Yeah... probably there is a better way of doing this...
    ctx.fillStyle = ""rgba(""+r+"",""+g+"",""+b+"",""+a+"")"";
    ctx.fillRect( x, y, 1, 1 );
}


function drawPicture()
{
    var c = document.getElementById(""GRAPH_CANVAS"");
    var ctx = c.getContext(""2d"");
   
    // Clear screen
    ctx.fillStyle = ""#222222"";
    ctx.fillRect(0, 0, c.width, c.height);

    for (y = 0; y < c.height; y++)
    {
        for (x = 0; x < c.width; x++)
        {
            var pixel = array[x + (y * c.width)];
            setPixel(ctx, x, y, pixel.r, pixel.g, pixel.b, pixel.a);
        }
    }
}
";
    }
}

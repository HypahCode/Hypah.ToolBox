namespace Hypah.WebServer.Html.HtmlElements.Graph
{
    internal static class LineGraph
    {
        public static string CreateScript(string backgroundColor)
        {
            return @"
var array = [];
[[ARRAYDATA]]


background = ""#333333"";
    

init();
function init()
{
    drawTimeline();
}

function drawTimeline()
{
    var c = document.getElementById(""GRAPH_CANVAS"");
    var ctx = c.getContext(""2d"");
   
    // Clear screen
    ctx.fillStyle = ""#222222"";
    ctx.fillRect(0, 0, c.width, c.height);

    // Find min/max
    var minx = array[0].data[0].x;
    var miny = array[0].data[0].y;
    var maxx = minx;
    var maxy = miny;
    for (var i = 0; i < array.length; i++)
    {
        for (var j = 0; j < array[i].data.length; j++)
        {
            if (array[i].data[j].x < minx) minx = array[i].data[j].x;
            if (array[i].data[j].y < miny) miny = array[i].data[j].y;
            if (array[i].data[j].x > maxx) maxx = array[i].data[j].x;
            if (array[i].data[j].y > maxy) maxy = array[i].data[j].y;
        }
    }
    var size = { x: (maxx - minx), y: (maxy - miny) };

    var drawScale = { x: (c.width / size.x), y: c.height / size.y };
    var offset    = { x: minx, y: miny };

    // Border a little bit
    offset.x -= (size.x * 0.1);
    offset.y -= (size.y * 0.1);
    drawScale.x *= 0.8;
    drawScale.y *= 0.8;

    // Draw lines
    ctx.strokeStyle = ""#EEEEEE"";
    ctx.fillStyle = ""#EEEEEE"";
    ctx.beginPath();
    var midPoint = transformXY(0, 0, drawScale, offset, c.height);
    ctx.moveTo(0, midPoint.y);
    ctx.lineTo(c.width, midPoint.y);
    ctx.moveTo(midPoint.x, 0);
    ctx.lineTo(midPoint.x, c.height);
    ctx.stroke();

    // Print text
    ctx.font = ""12px Arial"";
    if (maxx > 0.0)
    {
        var xAxisValue = (Math.round(maxx * 100) / 100);
        ctx.fillText(xAxisValue,  c.width - ctx.measureText(xAxisValue).width - 4, midPoint.y + 12);
    }
    if (maxy > 0.0) { ctx.fillText((Math.round(maxy * 100) / 100), midPoint.x + 4, 0 + 12); }
    if (minx < -0.2) { ctx.fillText((Math.round(minx * 100) / 100), 4, midPoint.y + 12); }
    if (miny < -0.2) { ctx.fillText((Math.round(miny * 100) / 100), midPoint.x + 4, c.height - 2); }


    // Draw graph
    for (var i = 0; i < array.length; i++)
    {
        ctx.strokeStyle = array[i].color;
        ctx.beginPath();
        for (var j = 0; j < array[i].data.length - 1; j++)
        {
            var xy1 = transformXY(array[i].data[j + 0].x, array[i].data[j + 0].y, drawScale, offset, c.height);
            var xy2 = transformXY(array[i].data[j + 1].x, array[i].data[j + 1].y, drawScale, offset, c.height);

            ctx.moveTo(xy1.x, xy1.y);
            ctx.lineTo(xy2.x, xy2.y);
        }
        ctx.stroke();        
    }
}

function transformXY(x, y, drawScale, offset, canvasHeight)
{
    var xScaled = (x - offset.x) * drawScale.x;
    var yScaled = canvasHeight - ((y - offset.y) * drawScale.y);
    return { x: xScaled, y: yScaled };
}
";
        }
    }
}

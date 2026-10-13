using Hypah.Calculator.Errors;
using Hypah.Calculator.Tokenizer;
using Xunit;

namespace Hypah.Calculator.Tests;

public class NewUnitsTokenizerTests
{
    [Fact]
    public void BitrateUnits_Tokenize()
    {
        var units = new[] { "bps", "kbps", "Mbps", "Gbps" };
        foreach (var unit in units)
        {
            var tokens = ExpressionTokenizer.Tokenize($"1{unit}");
            Assert.Equal(2, tokens.Count);
            Assert.Equal(TokenType.Unit, tokens[1].Type);
            Assert.Equal(unit, tokens[1].Text);
        }
    }

    [Fact]
    public void MemoryUnits_Tokenize()
    {
        var units = new[] { "B", "KB", "MB", "GB", "TB" };
        foreach (var unit in units)
        {
            var tokens = ExpressionTokenizer.Tokenize($"1{unit}");
            Assert.Equal(2, tokens.Count);
            Assert.Equal(TokenType.Unit, tokens[1].Type);
            Assert.Equal(unit, tokens[1].Text);
        }
    }

    [Fact]
    public void CompositeBandwidth_Tokenize()
    {
        var tokens = ExpressionTokenizer.Tokenize("5MB/s");
        Assert.Equal(2, tokens.Count);
        Assert.Equal(TokenType.Unit, tokens[1].Type);
        Assert.Equal("MB/s", tokens[1].Text);
    }

    [Fact]
    public void CompositeBandwidth_GBs_Tokenize()
    {
        var tokens = ExpressionTokenizer.Tokenize("5GB/s");
        Assert.Equal(2, tokens.Count);
        Assert.Equal(TokenType.Unit, tokens[1].Type);
        Assert.Equal("GB/s", tokens[1].Text);
    }

    [Fact]
    public void CompositeDensity_Tokenize()
    {
        var tokens = ExpressionTokenizer.Tokenize("5kg/m3");
        Assert.Equal(2, tokens.Count);
        Assert.Equal(TokenType.Unit, tokens[1].Type);
        Assert.Equal("kg/m3", tokens[1].Text);
    }

    [Fact]
    public void CompositeFlowRate_Tokenize()
    {
        var tokens = ExpressionTokenizer.Tokenize("5ml/s");
        Assert.Equal(2, tokens.Count);
        Assert.Equal(TokenType.Unit, tokens[1].Type);
        Assert.Equal("ml/s", tokens[1].Text);
    }

    [Fact]
    public void ResistanceUnicode_Tokenize()
    {
        var tokens = ExpressionTokenizer.Tokenize("5Ω");
        Assert.Equal(2, tokens.Count);
        Assert.Equal(TokenType.Unit, tokens[1].Type);
        Assert.Equal("Ω", tokens[1].Text);
    }

    [Fact]
    public void CaseSensitivity_NmVsNm()
    {
        var torqueTokens = ExpressionTokenizer.Tokenize("5Nm");
        Assert.Equal("Nm", torqueTokens[1].Text);

        var lengthTokens = ExpressionTokenizer.Tokenize("5nm");
        Assert.Equal("nm", lengthTokens[1].Text);
    }

    [Fact]
    public void CollisionMbVsMBs_PicksCorrectUnit()
    {
        var memoryTokens = ExpressionTokenizer.Tokenize("5MB");
        Assert.Equal("MB", memoryTokens[1].Text);

        var bandwidthTokens = ExpressionTokenizer.Tokenize("5MB/s");
        Assert.Equal("MB/s", bandwidthTokens[1].Text);
    }

    [Fact]
    public void CollisionMlVsMls_PicksCorrectUnit()
    {
        var volumeTokens = ExpressionTokenizer.Tokenize("5ml");
        Assert.Equal("ml", volumeTokens[1].Text);

        var flowTokens = ExpressionTokenizer.Tokenize("5ml/s");
        Assert.Equal("ml/s", flowTokens[1].Text);
    }

    [Fact]
    public void AllNewUnits_Tokenize()
    {
        var units = new[]
        {
            "bps", "kbps", "Mbps", "Gbps",
            "B", "KB", "MB", "GB", "TB",
            "fps", "MB/s", "GB/s",
            "N", "kN", "Pa", "kPa", "bar",
            "kg/m3", "l/s", "ml/s",
            "Nm", "W", "kW", "Hz", "kHz",
            "nm", "um", "V", "A", "Ω", "lm", "K", "dB"
        };

        foreach (var unit in units)
        {
            var tokens = ExpressionTokenizer.Tokenize($"1{unit}");
            Assert.Equal(2, tokens.Count);
            Assert.Equal(TokenType.Unit, tokens[1].Type);
            Assert.Equal(unit, tokens[1].Text);
        }
    }
}

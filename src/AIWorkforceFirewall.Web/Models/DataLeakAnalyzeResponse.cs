namespace AIWorkforceFirewall.Web.Models;

public sealed class DataLeakAnalyzeResponse
{
    public string Action { get; set; } = string.Empty;
    public List<string> DetectedTypes { get; set; } = [];
    public string? Message { get; set; }
}

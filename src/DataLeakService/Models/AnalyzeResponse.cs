namespace AIWorkforceFirewall.DataLeakService.Models;

public sealed class AnalyzeResponse
{
    public string Action { get; set; } = "ALLOW"; // ALLOW | BLOCK
    public List<string> DetectedTypes { get; set; } = new();
    public string? Message { get; set; }
}

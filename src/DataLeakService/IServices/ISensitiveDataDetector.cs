namespace AIWorkforceFirewall.DataLeakService.Services.Detectors;

public interface ISensitiveDataDetector
{
    string Type { get; }
    bool IsDetected(string text);
}

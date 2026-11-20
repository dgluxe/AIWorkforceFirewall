namespace AIWorkforceFirewall.DataLeakService.IServices;

public interface ISensitiveDataDetector
{
    string Type { get; }
    bool IsDetected(string text);
}

using AIWorkforceFirewall.DataLeakService.IServices;
using AIWorkforceFirewall.DataLeakService.IServices.Policies;
using AIWorkforceFirewall.DataLeakService.Models;
using AIWorkforceFirewall.DataLeakService.Models.Policies;
using AIWorkforceFirewall.DataLeakService.Services.Policies;

namespace AIWorkforceFirewall.DataLeakService.Services;

public sealed class DataLeakAnalyzer : IDataLeakAnalyzer
{
    private readonly IEnumerable<ISensitiveDataDetector> _detectors;
    private readonly IPolicyEngine _policyEngine;

    public DataLeakAnalyzer(IEnumerable<ISensitiveDataDetector> detectors, IPolicyEngine policyEngine)
    {
        _detectors = detectors;
        _policyEngine = policyEngine;
    }
    public AnalyzeResponse Analyze(string text)
    {

        if (string.IsNullOrWhiteSpace(text))
        {
            return new AnalyzeResponse
            {
                Action = PolicyAction.Allow.ToString().ToUpperInvariant(),
                Message = "Texto vazio."
            };
        }

        var detectedTypes = new List<string>();

        foreach(var detector in _detectors)
        {
            if (detector.IsDetected(text))
                detectedTypes.Add(detector.Type);
        }

        var decision = _policyEngine.Evaluate(detectedTypes);

        return new AnalyzeResponse
        {
            Action = decision.Action.ToString().ToUpperInvariant(),
            DetectedTypes = decision.DetectedTypes.ToList(),
            Message = decision.Reason
        };

    }
}

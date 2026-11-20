namespace AIWorkforceFirewall.DataLeakService.Models.Policies;

public sealed class PolicyDecision
{
    public PolicyAction Action { get; init; }

    public IReadOnlyCollection<string> DetectedTypes { get; init; } =
        Array.Empty<string>();

    public string Reason { get; init; } = string.Empty;
}

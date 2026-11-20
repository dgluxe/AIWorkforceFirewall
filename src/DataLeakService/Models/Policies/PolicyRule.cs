namespace AIWorkforceFirewall.DataLeakService.Models.Policies;

public sealed class PolicyRule
{
    public string DataType { get; init; } = string.Empty;
    public PolicyAction Action { get; init; } = PolicyAction.Block;
}

namespace AIWorkforceFirewall.DataLeakService.Models.Policies;

public sealed class SensitiveDataPolicyOptions
{
    public const string SectionName = "SensitiveDataPolicy";
    public PolicyAction DefaultAction { get; init; } = PolicyAction.Block;
    public List<PolicyRule> Rules { get; init; } = [];
}

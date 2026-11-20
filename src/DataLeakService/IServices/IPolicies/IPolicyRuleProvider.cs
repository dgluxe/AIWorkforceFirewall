using AIWorkforceFirewall.DataLeakService.Models.Policies;

namespace AIWorkforceFirewall.DataLeakService.IServices.Policies;

public interface IPolicyRuleProvider
{
    PolicyAction DefaultAction { get; }
    IReadOnlyCollection<PolicyRule> GetRules();
}

using AIWorkforceFirewall.DataLeakService.IServices.Policies;
using AIWorkforceFirewall.DataLeakService.Models.Policies;
using Microsoft.Extensions.Options;

namespace AIWorkforceFirewall.DataLeakService.Services.Policies;

public sealed class ConfigurationPolicyRuleProvider : IPolicyRuleProvider
{
    private readonly SensitiveDataPolicyOptions _options;
    public ConfigurationPolicyRuleProvider(
        IOptions<SensitiveDataPolicyOptions> options)
    {
        _options = options.Value;
    }

    public PolicyAction DefaultAction => _options.DefaultAction;

    public IReadOnlyCollection<PolicyRule> GetRules()
    {
        return _options.Rules;
    }
}

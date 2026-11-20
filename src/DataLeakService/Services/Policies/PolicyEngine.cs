using AIWorkforceFirewall.DataLeakService.IServices.Policies;
using AIWorkforceFirewall.DataLeakService.Models.Policies;

namespace AIWorkforceFirewall.DataLeakService.Services.Policies;

public sealed class PolicyEngine : IPolicyEngine
{
    private readonly IPolicyRuleProvider _ruleProvider;

    public PolicyEngine(IPolicyRuleProvider ruleProvider)
    {
        _ruleProvider = ruleProvider;
    }

    public PolicyDecision Evaluate(
        IReadOnlyCollection<string> detectedTypes)
    {
        var normalizedTypes = detectedTypes
            .Where(type => !string.IsNullOrWhiteSpace(type))
            .Select(type => type.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();

        if (normalizedTypes.Length == 0)
        {
            return new PolicyDecision
            {
                Action = PolicyAction.Allow,
                DetectedTypes = normalizedTypes,
                Reason = "Nenhum dado sensível foi detectado."
            };
        }

        var rules = _ruleProvider.GetRules();

        var blockedTypes = new List<string>();

        foreach (var detectedType in normalizedTypes)
        {
            var typeRules = rules
                .Where(rule =>
                string.Equals(
                    rule.DataType,
                    detectedType,
                    StringComparison.OrdinalIgnoreCase))
                .ToList();

            var action = typeRules.Count == 0
                ? _ruleProvider.DefaultAction
                : typeRules.Any(rule => rule.Action == PolicyAction.Block)
                    ? PolicyAction.Block
                    : PolicyAction.Allow;

            if(action == PolicyAction.Block)
            {
                blockedTypes.Add(detectedType);
            }
        }

        if(blockedTypes.Count > 0)
        {
            return new PolicyDecision
            {
                Action = PolicyAction.Block,
                DetectedTypes = normalizedTypes,
                Reason = $" A política bloqueou os tipos: {string.Join(", ", blockedTypes)}."
            };
        }

        return new PolicyDecision
        {
            Action = PolicyAction.Allow,
            DetectedTypes = normalizedTypes,
            Reason = "Os tipos detectados estão permitidos pela política."
        };
    }
}

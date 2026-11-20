using AIWorkforceFirewall.DataLeakService.IServices.Policies;
using AIWorkforceFirewall.DataLeakService.Models.Policies;
using AIWorkforceFirewall.DataLeakService.Services.Policies;
using System;
using System.Collections.Generic;
using System.Text;

namespace AIWorkforceFirewall.DataLeakService.Tests;

public sealed class PolicyEngineTests
{
    [Fact]
    public void Evaluate_SemTiposDetectados_DeveRetornarAllow()
    {
        var engine = CreateEngine();

        var result = engine.Evaluate(Array.Empty<string>());

        Assert.Equal(PolicyAction.Allow, result.Action);
        Assert.Empty(result.DetectedTypes);
    }

    [Fact]
    public void Evaluate_ComRegraBlock_DeveRetornarBlock()
    {
        var engine = CreateEngine(
            new PolicyRule
            {
                DataType = "CPF",
                Action = PolicyAction.Block
            });

        var result = engine.Evaluate(new[] { "CPF" });

        Assert.Equal(PolicyAction.Block, result.Action);
        Assert.Contains("CPF", result.DetectedTypes);
    }

    [Fact]
    public void Evaluate_ComRegraAllow_DeveRetornarAllow()
    {
        var engine = CreateEngine(
            new PolicyRule
            {
                DataType = "EMAIL",
                Action = PolicyAction.Allow
            });

        var result = engine.Evaluate(new[] { "EMAIL" });

        Assert.Equal(PolicyAction.Allow, result.Action);
        Assert.Contains("EMAIL", result.DetectedTypes);
    }

    [Fact]
    public void Evaluate_SemRegraEspecifica_DeveUsarBlockComoFallback()
    {
        var engine = CreateEngine();

        var result = engine.Evaluate(new[] { "NOVO_TIPO_SENSIVEL" });

        Assert.Equal(PolicyAction.Block, result.Action);
        Assert.Contains("NOVO_TIPO_SENSIVEL", result.DetectedTypes);
    }

    [Fact]
    public void Evaluate_ComAllowEBlock_DevePriorizarBlock()
    {
        var engine = CreateEngine(
            new PolicyRule
            {
                DataType = "EMAIL",
                Action = PolicyAction.Allow
            },
            new PolicyRule
            {
                DataType = "CPF",
                Action = PolicyAction.Block
            });

        var result = engine.Evaluate(new[] { "EMAIL", "CPF" });

        Assert.Equal(PolicyAction.Block, result.Action);
        Assert.Contains("EMAIL", result.DetectedTypes);
        Assert.Contains("CPF", result.DetectedTypes);
    }

    [Fact]
    public void Evaluate_NaoDeveExporValorSensivel()
    {
        var engine = CreateEngine(
            new PolicyRule
            {
                DataType = "CPF",
                Action = PolicyAction.Block
            });

        var result = engine.Evaluate(new[] { "CPF" });

        Assert.DoesNotContain("529.982.247-25", result.Reason);
        Assert.Contains("CPF", result.Reason);
    }

    private static PolicyEngine CreateEngine(
        params PolicyRule[] rules)
    {
        var provider = new TestPolicyRuleProvider(
            rules,
            PolicyAction.Block);

        return new PolicyEngine(provider);
    }

    private sealed class TestPolicyRuleProvider : IPolicyRuleProvider
    {
        private readonly IReadOnlyCollection<PolicyRule> _rules;

        public TestPolicyRuleProvider(
            IEnumerable<PolicyRule> rules,
            PolicyAction defaultAction)
        {
            _rules = rules.ToArray();
            DefaultAction = defaultAction;
        }

        public PolicyAction DefaultAction { get; }

        public IReadOnlyCollection<PolicyRule> GetRules()
        {
            return _rules;
        }
    }
}

using AIWorkforceFirewall.DataLeakService.Models.Policies;
using AIWorkforceFirewall.DataLeakService.Services.Policies;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace AIWorkforceFirewall.DataLeakService.Tests;

public sealed class ConfigurationPolicyRuleProviderTests
{
    [Fact]
    public void GetRules_DeveRetornarAsRegrasConfiguradas()
    {
        var options = Options.Create(
            new SensitiveDataPolicyOptions
            {
                DefaultAction = PolicyAction.Block,
                Rules =
                [
                    new PolicyRule
                    {
                        DataType = "CPF",
                        Action = PolicyAction.Block
                    },
                    new PolicyRule
                    {
                        DataType = "EMAIL",
                        Action = PolicyAction.Allow
                    }
                ]
            });

        var provider = new ConfigurationPolicyRuleProvider(options);

        var rules = provider.GetRules();

        Assert.Equal(2, rules.Count);
        Assert.Contains(
            rules,
            rule => rule.DataType == "CPF"
                && rule.Action == PolicyAction.Block);
        Assert.Contains(
            rules,
            rule => rule.DataType == "EMAIL"
                && rule.Action == PolicyAction.Allow);
    }

    [Fact]
    public void DefaultAction_DeveRetornarBlock()
    {
        var options = Options.Create(
            new SensitiveDataPolicyOptions
            {
                DefaultAction = PolicyAction.Block
            });

        var provider = new ConfigurationPolicyRuleProvider(options);

        Assert.Equal(PolicyAction.Block, provider.DefaultAction);
    }
}

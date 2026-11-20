using AIWorkforceFirewall.DataLeakService.IServices;
using AIWorkforceFirewall.DataLeakService.IServices.Policies;
using AIWorkforceFirewall.DataLeakService.Models.Policies;
using AIWorkforceFirewall.DataLeakService.Services;
using AIWorkforceFirewall.DataLeakService.Services.Detectors;
using AIWorkforceFirewall.DataLeakService.Services.Policies;
using System;
using System.Collections.Generic;
using System.Text;

namespace AIWorkforceFirewall.DataLeakService.Tests;

public sealed class DataLeakAnalyzerTest
{
    private readonly DataLeakAnalyzer _analyzer;

    public DataLeakAnalyzerTest()
    {
        var detectores = new List<ISensitiveDataDetector>
        {
            new CpfDetector(),
            new CnpjDetector(),
            new EmailDetector(),
            new PhoneDetector(),
            new RgDetector(),
            new CreditCardDetector(),
            new ApiKeyDetector()
        };
        var policyProvider = new TestPolicyRuleProvider();
        var policyEngine = new PolicyEngine(policyProvider);

        _analyzer = new DataLeakAnalyzer(
        detectores,
        policyEngine);

    }

    private sealed class TestPolicyRuleProvider : IPolicyRuleProvider
    {
        public PolicyAction DefaultAction => PolicyAction.Block;

        public IReadOnlyCollection<PolicyRule> GetRules()
        {
            return Array.Empty<PolicyRule>();
        }
    }

    [Fact]
    public void Analyze_ComApiKeyConhecida_DeveRetornarBlockSemExporValor()
    {
        const string secret = "ghp_0123456789abcdef";
        var result = _analyzer.Analyze($"Token de acesso: {secret}");

        Assert.Equal("BLOCK", result.Action);
        Assert.Contains("API_KEY", result.DetectedTypes);
        Assert.DoesNotContain(secret, result.Message ?? string.Empty);

    }

    [Fact]
    public void Analyze_TextComum_DeveRetornarAllow()
    {

        //Act
        var result = _analyzer.Analyze(
            "Explique orientação a objetos em C#");

        //Assert
        Assert.Equal("ALLOW", result.Action);
        Assert.Empty(result.DetectedTypes);

    }

    [Fact]
    public void Analyze_ComTextoVazio_DeveRetornarAllow()
    {

        //Act
        var result = _analyzer.Analyze(
            string.Empty);

        //Assert
        Assert.Equal("ALLOW", result.Action);
        Assert.Empty(result.DetectedTypes);

    }

    [Fact]
    public void Analyze_ComMultiplosDadosSensiveis_DeveRetornarTodosOsTipos()
    {
        var texto = "E-mail: teste@empresa.com.br " +
            "Telefone: (11) 91234-5678";

        var result = _analyzer.Analyze(texto);

        Assert.Equal("BLOCK", result.Action);
        Assert.Contains("EMAIL", result.DetectedTypes);
        Assert.Contains("PHONE", result.DetectedTypes);
    }
}

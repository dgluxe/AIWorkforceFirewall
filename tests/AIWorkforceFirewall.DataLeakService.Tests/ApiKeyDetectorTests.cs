using AIWorkforceFirewall.DataLeakService.Services.Detectors;
using System;
using System.Collections.Generic;
using System.Text;

namespace AIWorkforceFirewall.DataLeakService.Tests;

public sealed class ApiKeyDetectorTests
{
    private readonly ApiKeyDetector _detector = new();

    [Theory]
    [InlineData("ghp_0123456789abcdef")]
    [InlineData("github_pat_0123456789abcdef")]
    [InlineData("gho_0123456789abcdef")]
    [InlineData("ghu_0123456789abcdef")]
    [InlineData("ghs_0123456789abcdef")]
    [InlineData("ghr_0123456789abcdef")]
    [InlineData("AIzaSyA1234567890abcdef")]
    [InlineData("xoxb-12345678-abcdef")]
    [InlineData("xoxp-12345678-abcdef")]
    [InlineData("AKIAIOSFODNN7EXAMPLE")]
    [InlineData("ASIA1234567890ABCDEF")]
    public void IsDetected_ComPrefixoConhecido_DeveRetornarTrue(string value)
    {
        var result = _detector.IsDetected(value);

        Assert.True(result);
    }

    [Theory]
    [InlineData("password=Teste123!")]
    [InlineData("password: Teste123!")]
    [InlineData("api_key=abc123")]
    [InlineData("api-key: abc123")]
    [InlineData("client_secret=abc123")]
    [InlineData("access_token=abc123")]
    public void IsDetected_ComCredencialAtribuida_DeveRetornarTrue(string text)
    {
        var result = _detector.IsDetected(text);

        Assert.True(result);
    }

    [Theory]
    [InlineData("3238382930")]
    [InlineData("password")]
    [InlineData("senha")]
    [InlineData("token")]
    [InlineData("A aplicação possui um campo password.")]
    [InlineData("Precisamos configurar uma API Key.")]
    [InlineData("api_key")]
    [InlineData("api_key=")]
    [InlineData("api_key=SUA_API_KEY")]
    [InlineData("api_key=[REDACTED]")]
    [InlineData("api_key=<SECRET>")]
    [InlineData("Pedido 12345678901234567890")]
    [InlineData("Meu código de acesso é 123456")]
    public void IsDetected_ComTextoSemCredencialReal_DeveRetornarFalse(string text)
    {
        var result = _detector.IsDetected(text);

        Assert.False(result);
    }
}

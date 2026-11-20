using AIWorkforceFirewall.DataLeakService.Services.Detectors;
using System;
using System.Collections.Generic;
using System.Text;

namespace AIWorkforceFirewall.DataLeakService.Tests;

public sealed class PhoneDetectorTests
{
    private readonly PhoneDetector _detector = new();

    [Theory]
    [InlineData("(11) 91234-5678")]
    [InlineData("11 91234-5678")]
    [InlineData("11912345678")]
    [InlineData("+55 11 91234-5678")]
    [InlineData("+5511912345678")]
    [InlineData("(21) 2345-6789")]
    [InlineData("1123456789")]
    public void IsDetected_ComTelefoneBrasileiroValido_DeveRetornarTrue(
        string telefone)
    {
        var resultado = _detector.IsDetected(telefone);

        Assert.True(resultado);
    }

    [Theory]
    [InlineData("12345678")]
    [InlineData("999999999")]
    [InlineData("(00) 91234-5678")]
    [InlineData("11 1234-567")]
    [InlineData("Meu código de acesso é 12345678")]
    public void IsDetected_ComTextoSemTelefoneValido_DeveRetornarFalse(
        string texto)
    {
        var resultado = _detector.IsDetected(texto);

        Assert.False(resultado);
    }
}

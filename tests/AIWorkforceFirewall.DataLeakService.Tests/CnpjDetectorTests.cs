using AIWorkforceFirewall.DataLeakService.Services.Detectors;
using System;
using System.Collections.Generic;
using System.Text;

namespace AIWorkforceFirewall.DataLeakService.Tests;

public sealed class CnpjDetectorTests
{
    private readonly CnpjDetector _detector = new();

    [Theory]
    [InlineData("45.723.174/0001-10")]
    [InlineData("45723174000110")]
    public void IsDetected_ComCnpjValido_DeveRetornarTrue(string cnpj)
    {
        var resultado = _detector.IsDetected(cnpj);

        Assert.True(resultado);
    }

    [Theory]
    [InlineData("45.723.174/0001-11")]
    [InlineData("45723174000111")]
    [InlineData("11.111.111/1111-11")]
    [InlineData("00.000.000/0000-00")]
    [InlineData("45.723.174/0001")]
    [InlineData("texto comum sem CNPJ")]
    public void IsDetected_ComCnpjInvalido_DeveRetornarFalse(string texto)
    {
        var resultado = _detector.IsDetected(texto);

        Assert.False(resultado);
    }
}

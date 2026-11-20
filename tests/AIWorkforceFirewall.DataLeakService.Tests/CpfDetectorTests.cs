using AIWorkforceFirewall.DataLeakService.Services.Detectors;
using System;
using System.Collections.Generic;
using System.Text;

namespace AIWorkforceFirewall.DataLeakService.Tests;

public sealed class CpfDetectorTests
{
    private readonly CpfDetector _detector = new();

    [Theory]
    [InlineData("529.982.247-25")]
    [InlineData("52998224725")]
    public void IsDetected_ComCpfValido_DeveRetornarTrue(string cpf)
    {
        var resultado = _detector.IsDetected(cpf);

        Assert.True(resultado);
    }

    [Theory]
    [InlineData("529.982.247-26")]
    [InlineData("52998224726")]
    [InlineData("111.111.111-11")]
    [InlineData("000.000.000-00")]
    [InlineData("123.456.789")]
    [InlineData("texto comum sem CPF")]
    public void IsDetected_ComCpfInvalido_DeveRetornarFalse(string texto)
    {
        var resultado = _detector.IsDetected(texto);

        Assert.False(resultado);
    }
}

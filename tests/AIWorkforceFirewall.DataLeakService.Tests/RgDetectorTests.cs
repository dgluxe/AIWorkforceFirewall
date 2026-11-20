using AIWorkforceFirewall.DataLeakService.Services.Detectors;
using System;
using System.Collections.Generic;
using System.Text;

namespace AIWorkforceFirewall.DataLeakService.Tests;

public sealed class RgDetectorTests
{
    private readonly RgDetector _detector = new();

    [Theory]
    [InlineData("12.345.678-9")]
    [InlineData("123456789")]
    [InlineData("12.345.678-X")]
    [InlineData("12345678X")]
    public void IsDetected_ComRgNoFormatoDefinido_DeveRetornarTrue(string rg)
    {
        var resultado = _detector.IsDetected(rg);

        Assert.True(resultado);
    }

    [Theory]
    [InlineData("12.345.678")]
    [InlineData("12345678")]
    [InlineData("1234567890")]
    [InlineData("12.345.678-99")]
    [InlineData("texto comum sem RG")]
    public void IsDetected_ComTextoForaDoFormato_DeveRetornarFalse(string texto)
    {
        var resultado = _detector.IsDetected(texto);

        Assert.False(resultado);
    }
}

using AIWorkforceFirewall.DataLeakService.Services.Detectors;
using System;
using System.Collections.Generic;
using System.Text;

namespace AIWorkforceFirewall.DataLeakService.Tests;

public sealed class EmailDetectorTests
{
    private readonly EmailDetector _detector = new();

    [Theory]
    [InlineData("teste@empresa.com")]
    [InlineData("teste@empresa.com.br")]
    [InlineData("usuario.nome@empresa.com")]
    [InlineData("usuario+tag@empresa.com.br")]
    public void IsDetected_ComEmailValido_DeveRetornarTrue(string email)
    {
        var resultado = _detector.IsDetected(email);

        Assert.True(resultado);
    }

    [Theory]
    [InlineData("usuario@")]
    [InlineData("@empresa.com")]
    [InlineData("usuario@empresa")]
    [InlineData("usuario empresa.com")]
    [InlineData("texto comum sem email")]
    public void IsDetected_ComTextoSemEmailValido_DeveRetornarFalse(string texto)
    {
        var resultado = _detector.IsDetected(texto);

        Assert.False(resultado);
    }
}

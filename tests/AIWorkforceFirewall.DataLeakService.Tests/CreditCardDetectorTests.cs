using AIWorkforceFirewall.DataLeakService.Services.Detectors;
using System;
using System.Collections.Generic;
using System.Text;

namespace AIWorkforceFirewall.DataLeakService.Tests;

public sealed class CreditCardDetectorTests
{
    private readonly CreditCardDetector _detector = new();

    [Theory]
    [InlineData("4111111111111111")]
    [InlineData("4111 1111 1111 1111")]
    [InlineData("4111-1111-1111-1111")]
    [InlineData("378282246310005")]
    public void IsDetected_ComCartaoValido_DeveRetornarTrue(string cartao)
    {
        var resultado = _detector.IsDetected(cartao);

        Assert.True(resultado);
    }

    [Theory]
    [InlineData("4111111111111112")]
    [InlineData("4111 1111 1111 1112")]
    [InlineData("1234567890123456")]
    [InlineData("0000000000000000")]
    [InlineData("texto comum sem cartão")]
    public void IsDetected_ComCartaoInvalido_DeveRetornarFalse(string texto)
    {
        var resultado = _detector.IsDetected(texto);

        Assert.False(resultado);
    }
}

using AIWorkforceFirewall.DataLeakService.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace AIWorkforceFirewall.DataLeakService.Tests;

public class DataLeakAnalyzerTest
{
    private readonly DataLeakAnalyzer _analyzer = new();

    [Fact]
    public void Analyze_TextComum_DevRetornarAllow()
    {
        //Arrange
        var text = "Explique JWT em C#";

        //Act
        var result = _analyzer.Analyze(text);

        //Assert
        Assert.Equal("ALLOW", result.Action);
        Assert.Empty(result.DetectedTypes);

    }

    [Fact]
    public void Analyze_ComCpfValido_DeveRetornarBlock()
    {
        //Arrange
        var text = "Meu CPF é 529.982.247-25";

        //Act
        var result = _analyzer.Analyze(text);

        //Assert
        Assert.Equal("BLOCK", result.Action);
        Assert.Contains("CPF", result.DetectedTypes);
    }

    [Fact]
    public void Analyze_ComCnpjValido_DeveRetornarBlock()
    {
        //Arrange
        var text = "Meu CPNJ da empresa: 45.723.174/0001-10";

        //Act
        var result = _analyzer.Analyze(text);

        //Assert
        Assert.Equal("BLOCK", result.Action);
        Assert.Contains("CNPJ", result.DetectedTypes);
    }
}

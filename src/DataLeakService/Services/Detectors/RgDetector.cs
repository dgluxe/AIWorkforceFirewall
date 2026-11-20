using AIWorkforceFirewall.DataLeakService.IServices;
using System.Text.RegularExpressions;

namespace AIWorkforceFirewall.DataLeakService.Services.Detectors;

public sealed class RgDetector : ISensitiveDataDetector
{
    public string Type => "RG";

    // MVP: detecta formatos comuns como 12.345.678-9 e 123456789  
    // (sem validar dígito verificador por estado)

    private static readonly Regex RgRegex =
        new(@"\b(\d{2}\.?\d{3}\.?\d{3}-?[\dXx])\b", RegexOptions.Compiled);

    public bool IsDetected(string text)
    {
        if(string.IsNullOrWhiteSpace(text)) return false;
        return RgRegex.IsMatch(text);
    }
}

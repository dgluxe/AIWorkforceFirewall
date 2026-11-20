using AIWorkforceFirewall.DataLeakService.IServices;
using System.Text.RegularExpressions;

namespace AIWorkforceFirewall.DataLeakService.Services.Detectors;

public sealed class PhoneDetector : ISensitiveDataDetector
{
    public string Type => "PHONE";

    /* Formatos aceitos:
     * 
     * Celular:
     * (11) 91234-5678
     * 11 91234-5678
     * 11912345678
     * +55 11 91234-5678
     * +5511912345678
     * 
     * Fixo:
     * (11) 2345-678
     * 1123456789
     * 
     * O DDD é obrigadotiro para reduzir falsos positivos
     */


    private static readonly Regex PhoneRegex = new(
        @"(?<!\d)(?:\+55[\s-]?)?\(?[1-9]\d\)?[\s-]?(?:9\d{4}|[2-5]\d{3})[\s-]?\d{4}(?!\d)",
        RegexOptions.Compiled | RegexOptions.CultureInvariant);

    public bool IsDetected(string text)
    {
        if (string.IsNullOrWhiteSpace(text)) { return false; }
        return PhoneRegex.IsMatch(text);
    }
}

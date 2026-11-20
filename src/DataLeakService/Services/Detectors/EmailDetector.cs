using AIWorkforceFirewall.DataLeakService.IServices;
using System.Text.RegularExpressions;

namespace AIWorkforceFirewall.DataLeakService.Services.Detectors;

public sealed class EmailDetector : ISensitiveDataDetector
{
    public string Type => "EMAIL";

    //Regex simples e segura para MVP

    private static readonly Regex EmailRegex =
        new(@"\b[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}\b", RegexOptions.Compiled);
    public bool IsDetected(string text)
    {
        if(string.IsNullOrWhiteSpace(text)) return false;
        return EmailRegex.IsMatch(text);    
    }
}

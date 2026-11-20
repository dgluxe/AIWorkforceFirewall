using AIWorkforceFirewall.DataLeakService.IServices;
using System.Text.RegularExpressions;

namespace AIWorkforceFirewall.DataLeakService.Services.Detectors;

public sealed class ApiKeyDetector : ISensitiveDataDetector
{
    public string Type => "API_KEY";

    private static readonly Regex KnownProviderPrefixRegex = new(
        @"(?<![A-Za-z0-9_])(?:ghp_|github_pat_|gho_|ghu_|ghs_|ghr_|AIza|xox[bp]-|AKIA|ASIA)[A-Za-z0-9_-]{8,}(?![A-Za-z0-9_-])",
        RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

    private static readonly Regex ExplicitCredentialRegex = new(
        @"(?<![A-Za-z0-9_])(?:password|senha|api[_-]?key|client[_-]?secret|access[_-]?token)\s*[=:]\s*(?<value>[^\s,;]+)",
        RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
    public bool IsDetected(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return false;
        }

        if (KnownProviderPrefixRegex.IsMatch(text))
        {
            return true;
        }

        foreach (Match match in ExplicitCredentialRegex.Matches(text))
        {
            var value = match.Groups["value"].Value;

            if (!IsPlaceholder(value))
            {
                return true;
            }
        }

        return false;
    }

    private static bool IsPlaceholder(string value)
    {
        var normalized = value.Trim().Trim('"', '\'');

        return normalized.Equals("SUA_API_KEY", StringComparison.OrdinalIgnoreCase)
         || normalized.Equals("[REDACTED]", StringComparison.OrdinalIgnoreCase)
         || normalized.Equals("<SECRET>", StringComparison.OrdinalIgnoreCase);
    }
}


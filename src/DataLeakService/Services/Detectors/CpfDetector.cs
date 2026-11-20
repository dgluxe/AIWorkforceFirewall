using AIWorkforceFirewall.DataLeakService.IServices;
using System.Text.RegularExpressions;

namespace AIWorkforceFirewall.DataLeakService.Services.Detectors;

public sealed class CpfDetector : ISensitiveDataDetector
{
    public string Type => "CPF";

    private static readonly Regex CpfRegex =
        new(@"\b\d{3}\.?\d{3}\.?\d{3}-?\d{2}\b", RegexOptions.Compiled);

    public bool IsDetected(string text)
    {
        if (string.IsNullOrWhiteSpace(text)) return false;

        foreach (Match m in CpfRegex.Matches(text))
        {
            var digits = OnlyDigits(m.Value);
            if (IsValidCpf(digits)) return true;
        }

        return false;
    }

    private static string OnlyDigits(string input) =>
        new(input.Where(char.IsDigit).ToArray());

    private static bool IsValidCpf(string cpf)
    {
        if (cpf.Length != 11) return false;
        if (cpf.Distinct().Count() == 1) return false;

        int[] nums = cpf.Select(c => c - '0').ToArray();

        int sum1 = 0;
        for (int i = 0; i < 9; i++) sum1 += nums[i] * (10 - i);
        int dig1 = 11 - (sum1 % 11);
        if (dig1 >= 10) dig1 = 0;
        if (nums[9] != dig1) return false;

        int sum2 = 0;
        for (int i = 0; i < 10; i++) sum2 += nums[i] * (11 - i);
        int dig2 = 11 - (sum2 % 11);
        if (dig2 >= 10) dig2 = 0;

        return nums[10] == dig2;
    }

}

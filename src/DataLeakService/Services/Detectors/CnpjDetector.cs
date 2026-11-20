using AIWorkforceFirewall.DataLeakService.IServices;
using System.Text.RegularExpressions;

namespace AIWorkforceFirewall.DataLeakService.Services.Detectors;

public sealed class CnpjDetector : ISensitiveDataDetector
{
    public string Type => "CNPJ";

    private static readonly Regex CnpjRegex =
        new(@"\b\d{2}\.?\d{3}\.?\d{3}/?\d{4}-?\d{2}\b", RegexOptions.Compiled);

    public bool IsDetected(string text)
    {
        if (string.IsNullOrWhiteSpace(text)) return false;

        foreach (Match m in CnpjRegex.Matches(text))
        {
            var digits = OnlyDigits(m.Value);
            if (IsValidCnpj(digits)) return true;
        }

        return false;
    }

    private static string OnlyDigits(string input) => new(input.Where(char.IsDigit).ToArray());

    private static bool IsValidCnpj(string cnpj)
    {
        if (cnpj.Length != 14) return false;
        if (cnpj.Distinct().Count() == 1) return false;

        int[] nums = cnpj.Select(c => c - '0').ToArray();

        int[] w1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        int[] w2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

        int sum1 = 0;
        for (int i = 0; i < 12; i++) sum1 += nums[i] * w1[i];
        int dig1 = sum1 % 11;
        dig1 = dig1 < 2 ? 0 : 11 - dig1;
        if (nums[12] != dig1) return false;

        int sum2 = 0;
        for (int i = 0; i < 13; i++) sum2 += nums[i] * w2[i];
        int dig2 = sum2 % 11;
        dig2 = dig2 < 2 ? 0 : 11 - dig2;

        return nums[13] == dig2;
    }
}

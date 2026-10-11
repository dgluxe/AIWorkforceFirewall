using AIWorkforceFirewall.DataLeakService.Models;
using System.Text.RegularExpressions;

namespace AIWorkforceFirewall.DataLeakService.Services;

public sealed class DataLeakAnalyzer : IDataLeakAnalyzer
{
    private static readonly Regex CpfRegex = new(@"\b\d{3}\.?\d{3}\.?\d{3}-?\d{2}\b",
        RegexOptions.Compiled);
    private static readonly Regex CnpjRegex = new(@"\b\d{2}\.?\d{3}\.?\d{3}/?\d{4}-?\d{2}\b", RegexOptions.Compiled);
    public AnalyzeResponse Analyze(string text)
    {
        var response = new AnalyzeResponse();

        if (string.IsNullOrWhiteSpace(text))
        {
            response.Action = "ALLOW";
            response.Message = "Texto Vazio.";
            return response;
        }

        var detected = new List<string>();

        foreach(Match m in CpfRegex.Matches(text))
        {
            var digits = OnlyDigits(m.Value);
            if (IsValidCpf(digits)) detected.Add("CPF");
        }

        foreach(Match m in CnpjRegex.Matches(text))
        {
            var digits = OnlyDigits(m.Value);
            if (IsValidCnpj(digits)) detected.Add("CNPJ");

        }

        detected = detected.Distinct().ToList();

        if(detected.Count > 0)
        {
            response.Action = "BLOCK";
            response.DetectedTypes = detected;
            response.Message = "Dados sensíveis detectados pela política.";
            return response;
        }

        response.Action = "ALLOW";
        response.Message = "Nenhum dado sensível detectado.";
        return response;

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

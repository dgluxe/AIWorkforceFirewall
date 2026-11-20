using AIWorkforceFirewall.DataLeakService.IServices;
using System.Text.RegularExpressions;

namespace AIWorkforceFirewall.DataLeakService.Services.Detectors;

public sealed class CreditCardDetector : ISensitiveDataDetector
{
    public string Type => "CREDIT_CARD";

    /*
     * Aceita números com 13 a 19 digitos, com espaços ou hífens opcionais entre os dígitos.
     * 
     * Exemplos:
     * 4111111111111111
     * 4111 1111 1111 1111
     * 4111-1111-1111-1111
     */

    private static readonly Regex CardRegex = new(
        @"(?<!\d)(?:\d[ -]?){13,19}(?!\d)",
        RegexOptions.Compiled);

    public bool IsDetected(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return false;
        }

        foreach (Match match in CardRegex.Matches(text))
        {
            var digits = OnlyDigits(match.Value);

            if (IsValidCardNumber(digits))
            {
                return true;
            }
        }
        return false;
    }

        private static string OnlyDigits(string value)
        {
            return new(value.Where(char.IsDigit).ToArray());

        }
    private static bool IsValidCardNumber(string cardNumber)
    {
        if (cardNumber.Length is < 13 or > 19)
            return false;
        // Evita aceitar sequencias como 00000000000000.

        if (cardNumber.Distinct().Count() == 1)
            return false;

        var sum = 0;
        var shouldDouble = false;

        for(var index = cardNumber.Length - 1; index >= 0; index--)
        {
            var digit = cardNumber[index] - '0';

            if (shouldDouble)
            {
                digit *= 2;

                if (digit > 9)
                    digit -= 9;

            }

            sum += digit;
            shouldDouble = !shouldDouble;
        }
        return sum % 10 == 0;
    }
}

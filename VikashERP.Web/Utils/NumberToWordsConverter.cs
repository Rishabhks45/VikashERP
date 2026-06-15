using System;

namespace VikashERP.Web.Utils;

public static class NumberToWordsConverter
{
    private static readonly string[] Ones = { "", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
    private static readonly string[] Tens = { "", "", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };

    public static string ConvertToWords(decimal amount)
    {
        if (amount == 0)
            return "Zero Only";

        long rupees = (long)Math.Floor(amount);
        int paise = (int)Math.Round((amount - rupees) * 100);

        string words = ConvertWholeNumber(rupees);
        string result = $"Indian Rupees {words}";

        if (paise > 0)
        {
            result += $" and {ConvertWholeNumber(paise)} paise";
        }

        return result + " Only";
    }

    private static string ConvertWholeNumber(long number)
    {
        if (number == 0) return "";

        if (number < 20)
            return Ones[number];

        if (number < 100)
            return Tens[number / 10] + ((number % 10 > 0) ? " " + Ones[number % 10] : "");

        if (number < 1000)
            return Ones[number / 100] + " Hundred" + ((number % 100 > 0) ? " " + ConvertWholeNumber(number % 100) : "");

        if (number < 100000)
            return ConvertWholeNumber(number / 1000) + " Thousand" + ((number % 1000 > 0) ? " " + ConvertWholeNumber(number % 1000) : "");

        if (number < 10000000)
            return ConvertWholeNumber(number / 100000) + " Lakh" + ((number % 100000 > 0) ? " " + ConvertWholeNumber(number % 100000) : "");

        return ConvertWholeNumber(number / 10000000) + " Crore" + ((number % 10000000 > 0) ? " " + ConvertWholeNumber(number % 10000000) : "");
    }
}

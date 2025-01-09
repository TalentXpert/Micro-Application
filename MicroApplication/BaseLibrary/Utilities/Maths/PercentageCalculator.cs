using System;
using System.Collections.Generic;
using System.Text;

namespace BaseLibrary.Utilities
{
    public class PercentageCalculator
    {
        public decimal GetPercentage(decimal amount, decimal totalAmount)
        {
            if (totalAmount > 0)
            {
                return ((totalAmount - amount) / 100) * amount;
            }
            return 0;
        }

        public int GetPercentage(int amount, int totalAmount)
        {
            try
            {
                if (totalAmount > 0 && amount > 0)
                {
                    return ((totalAmount - amount) * 100) / amount;
                }
                return 0;
            }
            catch
            {
                return 0;
            }
        }

        public int GetMarksPercentage(int score, int totalMarks)
        {
            return (score * 100 / totalMarks);
        }
    }

    public class DecimalNumber
    {
        public static decimal Convert(string? number, decimal defaultValue)
        {
            if (string.IsNullOrWhiteSpace(number))
                if (decimal.TryParse(number, out decimal result)) { return result; }
            return defaultValue;
        }
    }
}

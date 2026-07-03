using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseLibrary.Tests
{
    public class TestDummyDataGenerator
    {
        private static Random random = new Random();
        public static string GetRandomStringOf(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public static string GetRandomNumber(int length)
        {
            const string digits = "0123456789";
            return new string(Enumerable.Repeat(digits, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public static string GetRandomSymbol(int length)
        {
            const string digits = "#$&*@";
            return new string(Enumerable.Repeat(digits, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public static string GetRandomEmail()
        {
            return $"Email.{GetRandomStringOf(8)}@tx.com";
        }
        public static string GetRandomPassword()
        {
            return GetRandomStringOf(4) + GetRandomStringOf(4).ToLower() + GetRandomSymbol(1) + GetRandomNumber(3);
        }

        public static string GetRandomEmployerName()
        {
            return $"Employer_{GetRandomStringOf(10)}";
        }
        public static string GetRandomAgencyName()
        {
            return $"Agency_{GetRandomStringOf(10)}";
        }
        public static string GetRandomVendorName()
        {
            return $"Vendor_{GetRandomStringOf(10)}";
        }
    }
}

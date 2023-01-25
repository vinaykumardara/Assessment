using System;
using System.Globalization;
using System.Linq;
using System.Numerics;
using static CarrierPricing.Data.Constants;

namespace CarrierPricing.Service
{
    public class CommonService
    {
        public static int GetPercentageValue(int baseprice, int servicemarkup)
        {
            return (int)Math.Ceiling(((decimal)baseprice * (decimal)servicemarkup)/100);
        }

        public static long Decode(string value)
        {
            value = Sanitize(value);
            CheckOverflow(value);
            var negative = value[0] == '-';
            value = Abs(value);
            var decoded = 0L;
            for (var i = 0; i < value.Length; ++i)
                decoded += Digits.IndexOf(value[i]) * (long)BigInteger.Pow(Digits.Length, value.Length - i - 1);
            return negative ? decoded * -1 : decoded;
        }

        private static string Abs(string value)
        {
            return value[0] == '-' ? value.Substring(1, value.Length - 1) : value;
        }

        private static void CheckOverflow(string value)
        {
            if (Compare(Min, value) < 0)
                throw new ArgumentException(string.Format(CultureInfo.InvariantCulture,
                    "Value \"{0}\" would overflow since it's less than long.MinValue.", value));
            if (Compare(value, Max) < 0)
                throw new ArgumentException(string.Format(CultureInfo.InvariantCulture,
                    "Value \"{0}\" would overflow since it's greater than long.MaxValue.", value));
        }

        private static string Sanitize(string value)
        {
            if (value == null)
                throw new ArgumentNullException("An null string was passed.", (Exception)null);
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("An empty string was passed.");
            value = value.Trim().ToUpperInvariant();
            if (Abs(value).Any(c => !Digits.Contains(c)))
                throw new ArgumentException(
                    string.Format(CultureInfo.InvariantCulture, "Invalid value: \"{0}\".", value));
            return value;
        }

        private static int Compare(string valueA, string valueB)
        {
            if (valueA == valueB)
                return 0;
            var negativeA = valueA[0] == '-';
            var negativeB = valueB[0] == '-';
            var bothNegative = negativeA && negativeB;
            if (!bothNegative && negativeA)
                return 1;
            if (!bothNegative && negativeB)
                return -1;
            valueA = Abs(valueA);
            valueB = Abs(valueB);
            if (valueA.Length < valueB.Length)
                return bothNegative ? -1 : 1;
            if (valueA.Length > valueB.Length)
                return bothNegative ? 1 : -1;
            for (var i = 0; i < valueA.Length; ++i)
            {
                var digitA = Digits.IndexOf(valueA[i]);
                var digitB = Digits.IndexOf(valueB[i]);
                if (digitA != digitB)
                    return (digitA < digitB ? 1 : -1) * (bothNegative ? -1 : 1);
            }
            throw new Exception("Logic error in the library, please contact the library author.");
        }
    }
}

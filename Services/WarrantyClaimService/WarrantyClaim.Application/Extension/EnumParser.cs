using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarrantyClaim.Application.Extension
{
    internal static class EnumParser
    {
        public static bool TryParseEnum<TEnum>(string? s, out TEnum value)
            where TEnum : struct, Enum
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                value = default; 
                return false;
            }

            return Enum.TryParse<TEnum>(s.Trim(), true, out value);
        }

        public static TEnum ParseOrDefault<TEnum>(string? value, TEnum fallback)
            where TEnum : struct, Enum
        {
            if (string.IsNullOrWhiteSpace(value)) return fallback;
            return Enum.TryParse<TEnum>(value.Trim(), true, out var parsed)
                ? parsed
                : fallback;
        }
    }
}

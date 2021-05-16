using System;
using System.Collections.Generic;
using System.Text;

namespace Cleverbit.CodingTask.Utilities
{
    public static class Extensions
    {
        public static string Base64Encode(this string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VSpniff.Core
{
    public static class Extensions
    {
        public static string ToString(this IEnumerable<string> items, char delimiter)
        {
            if (items != null)
            {
                StringBuilder builder = new StringBuilder();
                foreach (string s in items)
                {
                    builder.Append(s + delimiter);
                }
                return builder.ToString().TrimEnd(new char[] { delimiter });
            }
            return string.Empty;
        }

        public static bool In(this string str, params string[] matches)
        {
            foreach(var match in matches)
            {
                if (str == match)
                {
                    return true;
                }
            }
            return false;
        }
    }
}

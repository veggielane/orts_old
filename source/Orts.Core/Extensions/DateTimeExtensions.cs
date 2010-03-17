using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orts.Core
{
    public static class DateTimeExtensions
    {
        public static string ToStringSafe(this DateTime? dt, string format)
        {
            if (dt.HasValue)
                return dt.Value.ToString(format);
            else
                return "NULL";
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orts
{
    public static class StringExtensions
    {
        public static string fmt(this string format, params object[] args)
        {
            return string.Format(format, args);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepoTools
{
    internal static class BoolAsNumber
    {
        public static string GetBoolAsNumber(bool b)
        {
            string digit = b ? "1" : digit = "0";
            return digit;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OnlineCinemaContracts
{
    public static class Extentions
    {
        public static bool IsNullOrEmpty(this string? str) => string.IsNullOrEmpty(str); 

        public static string toTwoDigits(this string? str)
        {
            string sName = Regex.Replace($"_{str}_", @"\D\d\D", x => $"{x.Value[0]}0{x.Value[1]}{x.Value[2]}");
            return sName.Substring(1, sName.Length - 2);
        }
    }
}

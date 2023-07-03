using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCinemaContracts
{
    public static class Extentions
    {
        public static bool IsNullOrEmpty(this string? str) => string.IsNullOrEmpty(str); 
    }
}

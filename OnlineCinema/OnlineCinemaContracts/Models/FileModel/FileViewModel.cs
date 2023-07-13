using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCinemaContracts.Models.FileModel
{
    public class FileViewModel
    {
        public string Name { get; set; } = string.Empty;

        public string Extentions { get; set; } = string.Empty;

        public string Type { get; set; } = string.Empty;

        public double Size { get; set; }
    }
}

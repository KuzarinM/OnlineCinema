using OnlineCinemaContracts.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCinemaContracts.Models.FileModel
{
    public class SeasonFileModel
    {
        public SeasonViewModel? Model { get; set; }

        public Stream? Stream { get; set; }
    }
}

using OnlineCinemaContracts.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCinemaContracts.Models.FileModel
{
    public class EpisodeFileModel
    {
        public EpisodeViewModel? Model { get; set; }

        public FileStream? Stream { get; set; }
    }
}

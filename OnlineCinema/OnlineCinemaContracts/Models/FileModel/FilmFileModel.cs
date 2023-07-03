using OnlineCinemaContracts.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCinemaContracts.Models.FileModel
{
    public class FilmFileModel
    {
        public FilmViewModel? Model { get; set; } = null;

        public FileStream? Stream { get; set; }
    }
}

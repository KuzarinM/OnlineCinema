using OnlineCinemaContracts.Models.BindingModels;
using OnlineCinemaContracts.Models.FileModel;
using OnlineCinemaContracts.Models.SearchModels;
using OnlineCinemaContracts.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCinemaContracts.Logic
{
    public interface IFilmLogic
    {
        List<FilmViewModel>? ReadList(FilmSearchModel? model);

        FilmViewModel? ReadElement(FilmSearchModel model);

        bool Create(FilmBindingModel model);

        bool Update(FilmBindingModel model);

        bool Delete(FilmSearchModel model);

        FilmFileModel? GetFile(FilmSearchModel model);
    }
}

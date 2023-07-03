using OnlineCinemaContracts.Models.BindingModels;
using OnlineCinemaContracts.Models.SearchModels;
using OnlineCinemaContracts.Models.SettingsModel;
using OnlineCinemaContracts.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCinemaContracts.Storage
{
    public interface IFilmStorage
    {
        List<FilmViewModel> GetFullList();

        List<FilmViewModel> GetFiltredList(FilmSearchModel model);

        FilmViewModel? GetElement(FilmSearchModel model);

        FilmViewModel? Insert(FilmBindingModel model);

        FilmViewModel? Update(FilmBindingModel model);

        FilmViewModel? Delete(FilmSearchModel model);
    }
}

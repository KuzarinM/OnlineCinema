using OnlineCinemaContracts.Models.BindingModels;
using OnlineCinemaContracts.Models.SearchModels;
using OnlineCinemaContracts.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCinemaContracts.Storage
{
    public interface ISeriesStorage
    {
        List<SeriesViewModel> GetFullList();

        List<SeriesViewModel> GetFiltredList(SeriesSearchModel model);

        SeriesViewModel? GetElement(SeriesSearchModel model);

        SeriesViewModel? GetElement(EpisodeSearchModel model);

        SeriesViewModel? Insert(SeriesBindingModel model);

        SeriesViewModel? Update(SeriesBindingModel model);

        SeriesViewModel? Delete(SeriesSearchModel model);
    }
}

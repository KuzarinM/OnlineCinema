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
    public interface ISeasonStorage
    {
        List<SeasonViewModel> GetFullList();

        List<SeasonViewModel> GetFiltredList(SeasonSearchModel model);

        SeasonViewModel? GetElement(SeasonSearchModel model);

        SeasonViewModel? Insert(SeasonBindingModel model);

        SeasonViewModel? Update(SeasonBindingModel model);

        SeasonViewModel? Delete(SeasonSearchModel model);
    }
}

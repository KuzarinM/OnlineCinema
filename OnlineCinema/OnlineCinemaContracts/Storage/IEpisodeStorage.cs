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
    public interface IEpisodeStorage
    {
        List<EpisodeViewModel> GetFullList();

        List<EpisodeViewModel> GetFiltredList(EpisodeSearchModel model);

        EpisodeViewModel? GetElement(EpisodeSearchModel model);

        EpisodeViewModel? Insert(EpisodeBindingModel model);

        EpisodeViewModel? Update(EpisodeBindingModel model);

        EpisodeViewModel? Delete(EpisodeSearchModel model);
    }
}

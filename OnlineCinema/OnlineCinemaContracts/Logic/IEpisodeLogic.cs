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
    public interface IEpisodeLogic
    {
        List<EpisodeViewModel>? ReadList(EpisodeSearchModel? model);

        EpisodeViewModel? ReadElement(EpisodeSearchModel model);

        bool Create(EpisodeBindingModel model);

        bool Update(EpisodeBindingModel model);

        bool Delete(EpisodeSearchModel model);

        Task<EpisodeFileModel?> GetFile(EpisodeSearchModel model);
    }
}

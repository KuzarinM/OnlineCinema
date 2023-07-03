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
    public interface ISeasonLogic
    {
        List<SeasonViewModel>? ReadList(SeasonSearchModel? model);

        SeasonViewModel? ReadElement(SeasonSearchModel model);

        bool Create(SeasonBindingModel model);

        bool Update(SeasonBindingModel model);

        bool Delete(SeasonSearchModel model);

        Task<SeasonFileModel?> GetSeasonFolder(SeasonSearchModel model);
    }
}

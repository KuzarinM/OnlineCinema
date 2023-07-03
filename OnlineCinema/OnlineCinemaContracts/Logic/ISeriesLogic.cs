using OnlineCinemaContracts.Models.BindingModels;
using OnlineCinemaContracts.Models.SearchModels;
using OnlineCinemaContracts.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCinemaContracts.Logic
{
    public interface ISeriesLogic
    {
        List<SeriesViewModel>? ReadList(SeriesSearchModel? model);

        SeriesViewModel? ReadElement(SeriesSearchModel model);

        bool Create(SeriesBindingModel model);

        bool Update(SeriesBindingModel model);

        bool Delete(SeriesSearchModel model);
    }
}

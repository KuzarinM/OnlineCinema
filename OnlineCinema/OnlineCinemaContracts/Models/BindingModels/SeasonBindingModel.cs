using OnlineCinemaContracts.Models.Interfases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCinemaContracts.Models.BindingModels
{
    public class SeasonBindingModel : ISeasonModel
    {
        public string Id { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string SeriesId { get; set; } = string.Empty;

        public List<IEpisodeModel> Episodes { get; set; } = new();
    }
}

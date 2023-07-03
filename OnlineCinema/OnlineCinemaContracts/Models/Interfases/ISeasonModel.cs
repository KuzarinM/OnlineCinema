using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCinemaContracts.Models.Interfases
{
    public interface ISeasonModel : IId
    {
        public string Name { get; }

        public string SeriesId { get; }

        public List<IEpisodeModel> Episodes { get; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCinemaContracts.Models.Interfases
{
    public interface IEpisodeModel : IId
    {
        public string Name { get; }

        public string Path { get; }

        public string SeasonId { get; }

        public string Extention { get; }
    }
}

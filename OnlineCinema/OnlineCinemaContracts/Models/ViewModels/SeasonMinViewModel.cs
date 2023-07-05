using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCinemaContracts.Models.ViewModels
{
    public class SeasonMinViewModel
    {
        public string id { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public int EpisodeCount { get; set; }

        public List<(string id, string name)> Episodes { get; set; } = new();
    }
}

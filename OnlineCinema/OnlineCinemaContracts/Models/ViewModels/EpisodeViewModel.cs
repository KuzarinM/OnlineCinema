using Newtonsoft.Json;
using OnlineCinemaContracts.Models.Interfases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCinemaContracts.Models.ViewModels
{
    public class EpisodeViewModel : IEpisodeModel
    {
        public string Id { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        [JsonIgnore]
        public string Path { get; set; } = string.Empty;

        public string SeasonId { get; set; } = string.Empty;

        [JsonIgnore]
        public string Extention { get; set; } = string.Empty;
    }
}

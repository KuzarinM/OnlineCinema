using Newtonsoft.Json;
using OnlineCinemaContracts.Enums;
using OnlineCinemaContracts.Models.Interfases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCinemaContracts.Models.ViewModels
{
    public class SeriesViewModel : ISeriesModel
    {
        public string Id { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        [JsonIgnore]
        public string Path { get; set; } = string.Empty;

        [JsonIgnore]
        public List<ISeasonModel> Seasons { get; set; } = new();

        public Dictionary<string, (string name, int episodeCount)> MySeasons { get; set; } = new();

        public int TotalEpisodes { get; set; }

        [JsonIgnore]
        public ElementStatus mIndex { get; set; }

        public List<string> Tags { get; set; } = new();
    }
}

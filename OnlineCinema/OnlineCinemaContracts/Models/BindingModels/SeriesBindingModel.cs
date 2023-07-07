using Newtonsoft.Json;
using OnlineCinemaContracts.Enums;
using OnlineCinemaContracts.Models.Interfases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCinemaContracts.Models.BindingModels
{
    public class SeriesBindingModel : ISeriesModel
    {
        public string Id { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        [JsonIgnore]
        public string Path { get; set; } = string.Empty;

        [JsonIgnore]
        public string? PosterPath { get; set; }

        [JsonIgnore]
        public string? BackgroundPath { get; set; }

        public List<ISeasonModel> Seasons { get; set; } = new();

        public ElementStatus mIndex { get; set; }

        public List<string> Tags { get; set; } = new();
    }
}

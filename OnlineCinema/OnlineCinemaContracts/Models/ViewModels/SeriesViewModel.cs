﻿using Newtonsoft.Json;
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

        public string? PosterPath { get; set; }

        public string? BackgroundPath { get; set; }

        [JsonIgnore]
        public List<ISeasonModel> Seasons { get; set; } = new();

        public List<SeasonMinViewModel> MySeasons { get; set; } = new();

        public int TotalEpisodes { get; set; }

        public ElementStatus mIndex { get; set; }

        public List<string> Tags { get; set; } = new();
    }
}

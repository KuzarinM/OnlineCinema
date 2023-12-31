﻿using Newtonsoft.Json;
using OnlineCinemaContracts.Enums;
using OnlineCinemaContracts.Models.Interfases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCinemaContracts.Models.BindingModels
{
    public class FilmBindingModel : IFilmModel
    {
        public string Id { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string PakageName { get; set; } = string.Empty;

        [JsonIgnore]
        public string Path { get; set; } = string.Empty;

        [JsonIgnore]
        public string? PosterPath { get; set; }

        [JsonIgnore]
        public string? BackgroundPath { get; set; }

        public string Extention { get; set; } = string.Empty;

        [JsonIgnore]
        public ElementStatus mIndex { get; set; }

        public List<string> Tags { get; set; } = new();
    }
}

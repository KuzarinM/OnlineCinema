﻿using OnlineCinemaContracts.Models.Interfases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using OnlineCinemaContracts.Enums;

namespace OnlineCinemaContracts.Models.ViewModels
{
    public class FilmViewModel : IFilmModel
    {
        public string Id { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string PakageName { get; set; } = string.Empty;

        public string Path { get; set; } = string.Empty;

        public string? PosterPath { get; set; }

        public string? BackgroundPath { get; set; }

        public string Extention { get; set; } = string.Empty;

        [JsonIgnore]
        public ElementStatus mIndex { get; set; }

        public List<string> Tags { get; set; } = new();
    }
}

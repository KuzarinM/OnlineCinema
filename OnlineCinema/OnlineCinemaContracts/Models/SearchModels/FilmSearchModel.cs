using OnlineCinemaContracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCinemaContracts.Models.SearchModels
{
    public class FilmSearchModel
    {
        public string? Id { get; set; }

        public string? Name { get; set; }

        public string? Path { get; set; }

        public ElementStatus? mIndex { get; set; }

        public List<string>? HasTags { get; set; }

        public List<string>? WithoutTags { get; set; }

        public int? Page { get; set; }

        public int? Count { get; set; }
    }
}

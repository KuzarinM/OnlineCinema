using OnlineCinemaContracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCinemaContracts.Models.Interfases
{
    public interface IFilmModel : IId
    {
        public string Name { get; }

        public string? Description { get; }

        public string PakageName { get; }

        public string Path { get; }

        public string Extention { get; }

        public ElementStatus mIndex { get; }

        public List<string> Tags { get; }
    }
}

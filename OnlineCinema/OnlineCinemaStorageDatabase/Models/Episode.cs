using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using OnlineCinemaContracts;
using OnlineCinemaContracts.Models.BindingModels;
using OnlineCinemaContracts.Models.Interfases;
using OnlineCinemaContracts.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCinemaStorageDatabase.Models
{
    [BsonIgnoreExtraElements]
    public class Episode : IEpisodeModel
    {
        [BsonId]
        public ObjectId _id;

        [BsonIgnore]
        public string Id
        {
            get
            {
                return _id.ToString();
            }
            set
            {
                _id = ObjectId.Parse(value);
            }
        }

        [BsonElement("name")]
        public string Name { get; set; } = string.Empty;

        [BsonElement("path")]
        public string Path { get; set; } = string.Empty;

        [BsonElement("seasonId")]
        public string SeasonId { get; set; } = string.Empty;

        [BsonElement("extention")]
        public string Extention { get; set; } = string.Empty;

        public static Episode Create(EpisodeBindingModel model)
        {
            return new()
            {
                Name = model.Name,
                Path = model.Path,
                SeasonId = model.SeasonId,
                Extention = model.Extention,
            };
        }

        public void Update(EpisodeBindingModel model)
        {
            if(!model.Name.IsNullOrEmpty()) Name = model.Name;
            if(!model.Path.IsNullOrEmpty()) Path = model.Path;
            if(!model.SeasonId.IsNullOrEmpty()) SeasonId = model.SeasonId;
            if(!model.Extention.IsNullOrEmpty()) Extention = model.Extention;
        }

        public EpisodeViewModel GetViewModel => new()
        {
            Id = Id,
            Name = Name,
            Path = Path,
            SeasonId = SeasonId,
            Extention = Extention,
        };
    }
}

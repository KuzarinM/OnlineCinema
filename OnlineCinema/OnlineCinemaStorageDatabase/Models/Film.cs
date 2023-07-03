using Amazon.Util.Internal;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using OnlineCinemaContracts.Enums;
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
    public class Film : IFilmModel
    {
        [BsonId]
        public ObjectId _id;

        [BsonIgnore]
        public string Id {
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

        [BsonIgnoreIfNull]
        [BsonElement("description")]
        public string? Description { get; set; }

        [BsonElement("packageName")]
        public string PakageName { get; set; } = string.Empty;

        [BsonElement("path")]
        public string Path { get; set; } = string.Empty;

        [BsonElement("extention")]
        public string Extention { get; set; } = string.Empty;

        [BsonElement("index")]
        public ElementStatus mIndex { get; set; }

        [BsonIgnoreIfNull]
        [BsonElement("tags")]
        public List<string> Tags { get; set; } = new();

        public static Film Create(FilmBindingModel model)
        {
            return new()
            {
                Name = model.Name,
                Description = model.Description,
                PakageName = model.PakageName,
                Path = model.Path,
                Extention = model.Extention,
                mIndex = model.mIndex,
                Tags = model.Tags,
            };
        }

        public void Update(FilmBindingModel model)
        {
            if(!string.IsNullOrEmpty(model.Name)) Name = model.Name;
            if(!string.IsNullOrEmpty(model.Description)) Description = model.Description;
            if(!string.IsNullOrEmpty(model.PakageName)) PakageName = model.PakageName;
            if(!string.IsNullOrEmpty(model.Path)) Path = model.Path;
            if(!string.IsNullOrEmpty(model.Extention)) Extention = model.Extention;
            if(model.mIndex!=ElementStatus.None) mIndex = model.mIndex;
            if(Tags!=null) Tags = model.Tags;
        }

        public FilmViewModel GetViewModel => new()
        {
            Id = Id,
            Name = Name,
            Description = Description,
            PakageName = PakageName,
            Path = Path,
            Extention = Extention,
            mIndex = mIndex,
            Tags = Tags
        };
    }
}

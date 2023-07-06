using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using OnlineCinemaContracts;
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
    public class Series : ISeriesModel
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

        [BsonIgnoreIfNull]
        [BsonElement("description")]
        public string? Description { get; set; }

        [BsonElement("path")]
        public string Path { get; set; } = string.Empty;

        [BsonElement("seasons")]
        public List<ObjectId> MySeasons { get; set; } = new();

        private List<ISeasonModel>? _seasons = null;

        [BsonIgnore]
        public List<ISeasonModel> Seasons 
        {
            get
            {
                if(_seasons == null)
                {
                    _seasons = MongoDBSingleton.Instance().Seasons.Find(new BsonDocument("_id", new BsonDocument("$in", new BsonArray(MySeasons)))).
                        ToList().Select(x=>x.GetViewModel as ISeasonModel).ToList();
                }
                return _seasons;
            }
        }

        [BsonElement("index")]
        public ElementStatus mIndex { get; set; }

        [BsonIgnoreIfNull]
        [BsonElement("tags")]
        public List<string> Tags { get; set; } = new();

        public static Series Create(SeriesBindingModel model)
        {
            return new()
            {
                Name = model.Name,
                Description = model.Description,
                Path = model.Path,
                mIndex = model.mIndex,
                Tags = model.Tags,
                MySeasons = model.Seasons.Select(x=>ObjectId.Parse(x.Id)).ToList()
            };
        }

        public void Update(SeriesBindingModel model)
        {
            if (!model.Name.IsNullOrEmpty()) Name= model.Name;
            if (!model.Description.IsNullOrEmpty()) Description = model.Description;
            if (!model.Path.IsNullOrEmpty()) Path= model.Path;
            if (model.mIndex != ElementStatus.None) mIndex = model.mIndex;
            if (model.Tags != null && model.Tags.Count > 0) Tags = model.Tags;
            if (model.Seasons!=null && model.Seasons.Count > 0)
            {
                MySeasons = model.Seasons.Select(x => ObjectId.Parse(x.Id)).ToList();
                _seasons = null;
            }

        }

        public SeriesViewModel GetViewModel => new()
        {
            Id = Id,
            Name = Name,
            Description = Description,
            Seasons = Seasons,
            MySeasons = Seasons.Select(x=>new SeasonMinViewModel()
            {
                id= x.Id,
                Name= x.Name,
                EpisodeCount = x.Episodes.Count,
                Episodes = x.Episodes.Select(y=>(y.Id,y.Name, y.Path    )).ToList()
            }).ToList(),
            TotalEpisodes = Seasons.Sum(x=>x.Episodes.Count),
            Tags = Tags
        };
    }
}

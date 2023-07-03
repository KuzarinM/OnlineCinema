using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
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
    public class Season : ISeasonModel
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

        [BsonElement("seriesId")]
        public string SeriesId { get; set; } = string.Empty;

        [BsonElement("episodes")]
        public List<ObjectId> MyEpisodes = new();

        private List<IEpisodeModel>? _episodes = null;

        [BsonIgnore]
        public List<IEpisodeModel> Episodes 
        {
            get
            {
                if(_episodes == null)
                {
                    _episodes = MongoDBSingleton.Instance().Episodes.Find(new BsonDocument("_id", new BsonDocument("$in", new BsonArray(MyEpisodes))))
                        .ToList().Select(x=>x.GetViewModel as IEpisodeModel).ToList();
                }
                return _episodes;
            }
            set { _episodes = value; }
        }

        public static Season Create(SeasonBindingModel model)
        {
            return new()
            {
                Name = model.Name,
                SeriesId = model.SeriesId,
                MyEpisodes = model.Episodes.Select(x => ObjectId.Parse(x.Id)).ToList(),
            };
        }

        public void Update(SeasonBindingModel model)
        {
            if(!model.Name.IsNullOrEmpty()) Name= model.Name;
            if(!model.SeriesId.IsNullOrEmpty()) SeriesId= model.SeriesId;
            if(model.Episodes!=null && model.Episodes.Count > 0)
            {
                MyEpisodes = model.Episodes.Select(x => ObjectId.Parse(x.Id)).ToList();
                _episodes = null;
            }
        }

        public SeasonViewModel GetViewModel => new()
        {
            Id = Id,
            Name = Name,
            SeriesId = SeriesId,
            Episodes = Episodes,
        };
    }
}

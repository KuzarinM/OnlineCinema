using MongoDB.Bson;
using MongoDB.Driver;
using OnlineCinemaContracts;
using OnlineCinemaContracts.Models.BindingModels;
using OnlineCinemaContracts.Models.SearchModels;
using OnlineCinemaContracts.Models.ViewModels;
using OnlineCinemaContracts.Storage;
using OnlineCinemaStorageDatabase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCinemaStorageDatabase.Implements
{
    public class SeasonStorage : ISeasonStorage
    {
        public List<SeasonViewModel> GetFullList()
        {
            return MongoDBSingleton.Instance().Seasons.AsQueryable().ToList().Select(x => x.GetViewModel).ToList();
        }

        public List<SeasonViewModel> GetFiltredList(SeasonSearchModel model) 
        {
            if (model == null || model.SeriesId.IsNullOrEmpty())
                return new();

            return MongoDBSingleton.Instance().Seasons.Find(new BsonDocument("seriesId", ObjectId.Parse(model.SeriesId))).ToList().Select(x => x.GetViewModel).ToList();
        }

        public SeasonViewModel? GetElement(SeasonSearchModel model)
        {
            if (model == null || (model.Name.IsNullOrEmpty() && model.Id.IsNullOrEmpty()))
                return new();

            if (!model.Id.IsNullOrEmpty())
                return MongoDBSingleton.Instance().Seasons.Find(new BsonDocument("_id", ObjectId.Parse(model.Id))).FirstOrDefault()?.GetViewModel;

            return MongoDBSingleton.Instance().Seasons.Find(new BsonDocument("name", model.Name)).FirstOrDefault()?.GetViewModel;
        }

        public SeasonViewModel? Insert(SeasonBindingModel model)
        {
            if (model == null)
                return null;

            var newSeason = Season.Create(model);
            if (newSeason == null)
                return null;

            MongoDBSingleton.Instance().Seasons.InsertOne(newSeason);
            return newSeason.GetViewModel;
        }

        public SeasonViewModel? Update(SeasonBindingModel model)
        {
            if (model == null)
                return null;

            var season = MongoDBSingleton.Instance().Seasons.Find(new BsonDocument("_id", ObjectId.Parse(model.Id))).FirstOrDefault();
            if (season == null)
                return null;

            season.Update(model);
            MongoDBSingleton.Instance().Seasons.FindOneAndReplace(new BsonDocument("_id", ObjectId.Parse(model.Id)), season);
            return season.GetViewModel;
        }

        public SeasonViewModel? Delete(SeasonSearchModel model)
        {
            if (model == null)
                return null;

            var season = MongoDBSingleton.Instance().Seasons.FindOneAndDelete(new BsonDocument("_id", ObjectId.Parse(model.Id)));
            if (season != null)
            {
                return season.GetViewModel;
            }
            return null;
        }
    }
}

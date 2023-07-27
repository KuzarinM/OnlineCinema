using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using OnlineCinemaContracts;
using OnlineCinemaContracts.Enums;
using OnlineCinemaContracts.Models.BindingModels;
using OnlineCinemaContracts.Models.SearchModels;
using OnlineCinemaContracts.Models.SettingsModel;
using OnlineCinemaContracts.Models.ViewModels;
using OnlineCinemaContracts.Storage;
using OnlineCinemaStorageDatabase.DiskFileSystem;
using OnlineCinemaStorageDatabase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCinemaStorageDatabase.Storage
{
    public class FilmStorage : IFilmStorage
    {
        public List<FilmViewModel> GetFullList()
        {
            return MongoDBSingleton.Instance().Films.AsQueryable().ToList().Select(x => x.GetViewModel).ToList();
        }

        public List<FilmViewModel> GetFiltredList(FilmSearchModel model)
        {
            if (model == null || (!model.mIndex.HasValue && model.HasTags == null && model.WithoutTags == null && model.Name.IsNullOrEmpty() && (!model.Page.HasValue || !model.Count.HasValue)))
                return new();

            BsonArray condition = new();
            if (model.mIndex.HasValue)
                condition.Add(new BsonDocument("index", model.mIndex.Value));
            if (model.HasTags != null)
                condition.Add(new BsonDocument("tags", new BsonDocument("$all", new BsonArray(model.HasTags))));
            if (model.WithoutTags != null)
                condition.Add(new BsonDocument("tags", new BsonDocument("$not", new BsonDocument("$in", new BsonArray(model.WithoutTags)))));
            if (!model.Name.IsNullOrEmpty())
                condition.Add(new BsonDocument("name", new BsonRegularExpression($"^{model.Name}")));

            var resout = MongoDBSingleton.Instance().Films.Find(new BsonDocument("$and", condition)).Sort(new BsonDocument("_id", -1));

            if (model.Page.HasValue)
            {
                resout = resout.Skip(model.Count * model.Page).Limit(model.Count);
            }

            return resout.ToList().Select(x => x.GetViewModel).ToList();
        }

        public FilmViewModel? GetElement(FilmSearchModel model)
        {
            if(model == null || (model.Id.IsNullOrEmpty() && model.Name.IsNullOrEmpty() && model.Path.IsNullOrEmpty() ) )
                return null;

            if (!model.Id.IsNullOrEmpty())
                return MongoDBSingleton.Instance().Films.Find(new BsonDocument("_id",ObjectId.Parse(model.Id))).FirstOrDefault()?.GetViewModel;
            if (!model.Name.IsNullOrEmpty())
                return MongoDBSingleton.Instance().Films.Find(new BsonDocument("name", model.Name)).FirstOrDefault()?.GetViewModel;
            if (!model.Path.IsNullOrEmpty())
                return MongoDBSingleton.Instance().Films.Find(new BsonDocument("path", model.Path)).FirstOrDefault()?.GetViewModel;

            return null;
        }

        public FilmViewModel? Insert(FilmBindingModel model)
        {
            if (model == null)
                return null;

            var newFilm = Film.Create(model);
            if (newFilm == null)
                return null;

            MongoDBSingleton.Instance().Films.InsertOne(newFilm);
            return newFilm.GetViewModel;
        }

        public FilmViewModel? Update(FilmBindingModel model)
        {
            if (model == null)
                return null;

            var film = MongoDBSingleton.Instance().Films.Find(new BsonDocument("_id", ObjectId.Parse(model.Id))).FirstOrDefault();
            if (film == null)
                return null;

            film.Update(model);
            MongoDBSingleton.Instance().Films.FindOneAndReplace(new BsonDocument("_id", ObjectId.Parse(model.Id)), film);
            return film.GetViewModel;
        }

        public FilmViewModel? Delete(FilmSearchModel model)
        {
            if (model == null)
                return null;

            var film = MongoDBSingleton.Instance().Films.FindOneAndDelete(new BsonDocument("_id", ObjectId.Parse(model.Id)));
            if (film != null)
            {
                return film.GetViewModel;
            }
            return null;
        }
    }
}

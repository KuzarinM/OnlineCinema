﻿using MongoDB.Bson;
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

namespace OnlineCinemaStorageDatabase.Implements
{
    public class SeriesStorage : ISeriesStorage
    {
        public List<SeriesViewModel> GetFullList()
        {
            return MongoDBSingleton.Instance().Series.AsQueryable().ToList().Select(x => x.GetViewModel).ToList();
        }

        public List<SeriesViewModel> GetFiltredList(SeriesSearchModel model)
        {
            if (model == null || (!model.mIndex.HasValue && model.HasTags == null && model.WithoutTags == null && (!model.Page.HasValue || !model.Count.HasValue)))
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

            var resout = MongoDBSingleton.Instance().Series.Find(new BsonDocument("$and", condition)).Sort(new BsonDocument("_id",-1));

            if (model.Page.HasValue)
            {
                resout = resout.Skip(model.Count * model.Page).Limit(model.Count);
            }
            return resout.ToList().Select(x => x.GetViewModel).ToList();
        }

        public SeriesViewModel? GetElement(SeriesSearchModel model)
        {
            if (model == null || (model.Id.IsNullOrEmpty() && model.Name.IsNullOrEmpty() && model.Path.IsNullOrEmpty()))
                return null;

            if (!model.Id.IsNullOrEmpty())
                return MongoDBSingleton.Instance().Series.Find(new BsonDocument("_id", ObjectId.Parse(model.Id))).FirstOrDefault()?.GetViewModel;
            if (!model.Name.IsNullOrEmpty())
                return MongoDBSingleton.Instance().Series.Find(new BsonDocument("name", model.Name)).FirstOrDefault()?.GetViewModel;
            if (!model.Path.IsNullOrEmpty())
                return MongoDBSingleton.Instance().Series.Find(new BsonDocument("path", model.Path)).FirstOrDefault()?.GetViewModel;

            return null;
        }

        public SeriesViewModel? GetElement(EpisodeSearchModel model)
        {
            if(model == null || model.Id.IsNullOrEmpty()) return null;

            var ep = MongoDBSingleton.Instance().Episodes.Find(new BsonDocument("_id", ObjectId.Parse(model.Id))).FirstOrDefault();
            if (ep == null) return null;
            var seas = MongoDBSingleton.Instance().Seasons.Find(new BsonDocument("_id", ObjectId.Parse(ep.SeasonId))).FirstOrDefault();
            if (seas == null) return null;
            return MongoDBSingleton.Instance().Series.Find(new BsonDocument("_id", ObjectId.Parse(seas.SeriesId))).FirstOrDefault()?.GetViewModel;
        }

        public SeriesViewModel? Insert(SeriesBindingModel model)
        {
            if (model == null)
                return null;

            var newSeries = Series.Create(model);
            if (newSeries == null)
                return null;

            MongoDBSingleton.Instance().Series.InsertOne(newSeries);
            return newSeries.GetViewModel;
        }

        public SeriesViewModel? Update(SeriesBindingModel model)
        {
            if (model == null)
                return null;

            var series = MongoDBSingleton.Instance().Series.Find(new BsonDocument("_id", ObjectId.Parse(model.Id))).FirstOrDefault();
            if (series == null)
                return null;

            series.Update(model);
            MongoDBSingleton.Instance().Series.FindOneAndReplace(new BsonDocument("_id", ObjectId.Parse(model.Id)), series);
            return series.GetViewModel;
        }

        public SeriesViewModel? Delete(SeriesSearchModel model)
        {
            if (model == null)
                return null;

            var series = MongoDBSingleton.Instance().Series.FindOneAndDelete(new BsonDocument("_id", ObjectId.Parse(model.Id)));
            if (series != null)
            {
                return series.GetViewModel;
            }
            return null;
        }
    }
}

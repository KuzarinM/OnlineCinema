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
    public class EpisodeStorage : IEpisodeStorage
    {
        public List<EpisodeViewModel> GetFullList()
        {
            return MongoDBSingleton.Instance().Episodes.AsQueryable().ToList().Select(x => x.GetViewModel).ToList();
        }

        public List<EpisodeViewModel> GetFiltredList(EpisodeSearchModel model)
        {
            if (model == null || model.SeasonId.IsNullOrEmpty())
                return new();

            return MongoDBSingleton.Instance().Episodes.Find(new BsonDocument("seasonId", ObjectId.Parse(model.SeasonId))).ToList().Select(x=>x.GetViewModel).ToList();
        }

        public EpisodeViewModel? GetElement(EpisodeSearchModel model)
        {
            if (model == null || (model.Name.IsNullOrEmpty() && model.Id.IsNullOrEmpty()))
                return new();

            if(!model.Id.IsNullOrEmpty())
                return MongoDBSingleton.Instance().Episodes.Find(new BsonDocument("_id",ObjectId.Parse(model.Id))).FirstOrDefault()?.GetViewModel;

            return MongoDBSingleton.Instance().Episodes.Find(new BsonDocument("name", model.Name)).FirstOrDefault()?.GetViewModel;
        }

        public EpisodeViewModel? Insert(EpisodeBindingModel model)
        {
            if (model == null)
                return null;

            var newEpisode = Episode.Create(model);
            if (newEpisode == null)
                return null;

            MongoDBSingleton.Instance().Episodes.InsertOne(newEpisode);
            return newEpisode.GetViewModel;
        }

        public EpisodeViewModel? Update(EpisodeBindingModel model)
        {
            if (model == null)
                return null;

            var episode = MongoDBSingleton.Instance().Episodes.Find(new BsonDocument("_id", ObjectId.Parse(model.Id))).FirstOrDefault();
            if (episode == null)
                return null;

            episode.Update(model);
            MongoDBSingleton.Instance().Episodes.FindOneAndReplace(new BsonDocument("_id", ObjectId.Parse(model.Id)), episode);
            return episode.GetViewModel;
        }

        public EpisodeViewModel? Delete(EpisodeSearchModel model)
        {
            if (model == null)
                return null;

            var episode = MongoDBSingleton.Instance().Episodes.FindOneAndDelete(new BsonDocument("_id", ObjectId.Parse(model.Id)));
            if (episode != null)
            {
                return episode.GetViewModel;
            }
            return null;
        }
    }
}

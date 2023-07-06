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
    public class UserStorage : IUserStorage
    {
        public List<UserViewModel> GetFullList()
        {
            return MongoDBSingleton.Instance().Users.AsQueryable().ToList().Select(x => x.GetViewModel).ToList();
        }

        public List<UserViewModel> GetFiltredList(UserSearchModel model)
        {
            throw new NotImplementedException();
        }

        public UserViewModel? GetElement(UserSearchModel model)
        {
            if (model == null || (model.Login.IsNullOrEmpty() && model.Id.IsNullOrEmpty()))
                return new();

            if (!model.Id.IsNullOrEmpty())
                return MongoDBSingleton.Instance().Users.Find(new BsonDocument("_id", ObjectId.Parse(model.Id))).FirstOrDefault()?.GetViewModel;
            if(model.Password == null)
                return MongoDBSingleton.Instance().Users.Find(new BsonDocument("login", model.Login)).FirstOrDefault()?.GetViewModel;

            return MongoDBSingleton.Instance().Users.Find(new BsonDocument("$and", new BsonArray(new List<BsonDocument>() { new BsonDocument("login", model.Login), new BsonDocument("password", model.Password) })))
                .FirstOrDefault()?.GetViewModel;

        }

        public UserViewModel? Insert(UserBindingModel model)
        {
            if (model == null)
                return null;

            var newUser = User.Create(model);
            if (newUser == null)
                return null;

            MongoDBSingleton.Instance().Users.InsertOne(newUser);
            return newUser.GetViewModel;
        }

        public UserViewModel? Update(UserBindingModel model)
        {
            if (model == null)
                return null;

            var user = MongoDBSingleton.Instance().Users.Find(new BsonDocument("_id", ObjectId.Parse(model.Id))).FirstOrDefault();
            if (user == null)
                return null;

            user.Update(model);
            MongoDBSingleton.Instance().Users.FindOneAndReplace(new BsonDocument("_id", ObjectId.Parse(model.Id)), user);
            return user.GetViewModel;
        }

        public UserViewModel? Delete(UserSearchModel model)
        {
            if (model == null)
                return null;

            var user = MongoDBSingleton.Instance().Users.FindOneAndDelete(new BsonDocument("_id", ObjectId.Parse(model.Id)));
            if (user != null)
            {
                return user.GetViewModel;
            }
            return null;
        }
    }
}

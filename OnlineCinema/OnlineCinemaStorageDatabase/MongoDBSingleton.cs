using Amazon.SecurityToken.Model;
using DnsClient.Protocol;
using Microsoft.Extensions.Logging;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using OnlineCinemaStorageDatabase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCinemaStorageDatabase
{
    public class MongoDBSingleton
    {
        #region Singletone
        private static MongoDBSingleton? _instance;
        public static MongoDBSingleton Instance()
        {
            if (_instance == null)
            {
                _instance = new MongoDBSingleton();
            }
            return _instance;
        }
        #endregion Singletone

        public IMongoCollection<Film> Films;
        public IMongoCollection<Series> Series;
        public IMongoCollection<Season> Seasons;
        public IMongoCollection<Episode> Episodes;
        public IMongoCollection<User> Users;

        public MongoDBSingleton()
        {
            BsonSerializer.RegisterSerializer(DateTimeSerializer.LocalInstance);
            MongoClient client = new MongoClient("mongodb://localhost:27017");
            Films = client.GetDatabase("OnlineCinema").GetCollection<Film>("Films");
            Series = client.GetDatabase("OnlineCinema").GetCollection<Series>("Series");
            Seasons = client.GetDatabase("OnlineCinema").GetCollection<Season>("Seasons");
            Episodes = client.GetDatabase("OnlineCinema").GetCollection<Episode>("Episodes");
            Users = client.GetDatabase("OnlineCinema").GetCollection<User>("Users");
        }
    }
}

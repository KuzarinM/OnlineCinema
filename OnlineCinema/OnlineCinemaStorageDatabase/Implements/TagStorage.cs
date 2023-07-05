using MongoDB.Driver;
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
    public class TagStorage : ITagStorage
    {
        public List<TagViewModel>? GetFullList()
        {
            FieldDefinition<Film, string> fieldF = "tags";
            FieldDefinition<Series, string> fieldS = "tags";
            var filmTags = MongoDBSingleton.Instance().Films.Distinct(fieldF, x=>true).ToList();
            var seriesTags = MongoDBSingleton.Instance().Series.Distinct(fieldS, x=>true).ToList();
            return filmTags.Union(seriesTags).Select(x=>new TagViewModel
            {
                Name = x
            }).ToList();
        }
    }
}

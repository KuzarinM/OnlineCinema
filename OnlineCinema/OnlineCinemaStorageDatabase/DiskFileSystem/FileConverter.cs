using MongoDB.Bson;
using MongoDB.Driver;
using OnlineCinemaContracts;
using OnlineCinemaContracts.Enums;
using OnlineCinemaContracts.Models.Interfases;
using OnlineCinemaContracts.Models.SettingsModel;
using OnlineCinemaContracts.Storage;
using OnlineCinemaStorageDatabase.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OnlineCinemaStorageDatabase.DiskFileSystem
{
    public class FileConverter : IFileConverter
    {
        public void LoadData(FileSystemSingletoneModel model)
        {
            MongoDBSingleton.Instance().Films.DeleteMany(new BsonDocument("index", (int)ElementStatus.Obsolete));
            MongoDBSingleton.Instance().Films.UpdateMany(new BsonDocument(), new BsonDocument("$set", new BsonDocument("index", ElementStatus.Obsolete)));

            TreeGoing(model.drivePath, model);
        }

        private void TreeGoing(string path, FileSystemSingletoneModel model)
        {
            foreach (var item in Directory.EnumerateDirectories(path))
            {
                string name = Path.GetFileNameWithoutExtension(item);
                if (name.StartsWith("$") || model.blackFolderList.Any(x => x == name))
                    continue;
                try
                {
                    List<Film> tmp = IsFilm(item, model);
                    if (tmp.Count > 0)
                    {
                        tmp.ForEach(x => AddFilm(x, model));
                    }
                    Series? data = IsSeries(item, model);
                    if (data != null)
                    {
                        AddSeries(data, model);
                        continue;
                    }

                    TreeGoing(item, model);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Acsess to folder {item} denied({ex.Message})");
                }
            }
        }

        private void AddFilm(Film newfilm, FileSystemSingletoneModel model)
        {
            Film film = MongoDBSingleton.Instance().Films.Find(new BsonDocument("name", newfilm.Name)).FirstOrDefault();
            if (film == null)
            {
                newfilm.mIndex = ElementStatus.Added;

                if (newfilm.Path.Contains("Ролики"))
                {
                    newfilm.Tags.Add("видео");
                    newfilm.Tags.Add("private");
                }
                newfilm.PosterPath = Directory.GetFiles(model.posterDir, $"{film.Name}.*").FirstOrDefault(model.defaultImg);
                newfilm.BackgroundPath = Directory.GetFiles(model.bacgroundDir, $"{film.Name}.*").FirstOrDefault(model.defaultImg);

                MongoDBSingleton.Instance().Films.InsertOne(newfilm);
            }
            else
            {
                bool flag = false;
                if (film.PakageName != newfilm.PakageName)
                {
                    flag = true;
                    film.PakageName = newfilm.PakageName;
                }
                if (film.Extention != newfilm.Extention)
                {
                    flag = true;
                    film.Extention = newfilm.Extention;
                }
                string defaultPath = Directory.GetFiles(model.posterDir, $"{film.Name}.*").FirstOrDefault(model.defaultImg);
                if (film.PosterPath.IsNullOrEmpty() || film.PosterPath != defaultPath) 
                {
                    flag = true;
                    film.PosterPath = defaultPath;
                }
                defaultPath = Directory.GetFiles(model.bacgroundDir, $"{film.Name}.*").FirstOrDefault(model.defaultImg);
                if (film.BackgroundPath.IsNullOrEmpty() ||  film.BackgroundPath != defaultPath)
                {
                    flag = true;
                    film.BackgroundPath = defaultPath;
                }
                film.mIndex = flag ? ElementStatus.Updated : ElementStatus.Active;

                MongoDBSingleton.Instance().Films.FindOneAndReplace(new BsonDocument("_id", film._id), film);
            }
        }

        private void AddSeries(Series newSeries, FileSystemSingletoneModel model) 
        {
            Series series =  MongoDBSingleton.Instance().Series.Find(new BsonDocument("name", newSeries.Name)).FirstOrDefault();
            if (series != null)
            {
                if (series.Path != newSeries.Path)
                {
                    series.Path = newSeries.Path;
                    series.mIndex = ElementStatus.Updated;
                }
                series.mIndex= ElementStatus.Active;
            }
            else
            {
                var tags = new List<string>();
                if (newSeries.Path.Contains("Ролики"))
                {
                    tags.Add("видео");
                    tags.Add("private");
                }
                MongoDBSingleton.Instance().Series.InsertOne(new Series
                {
                    Name = newSeries.Name,
                    Path= newSeries.Path,
                    mIndex = ElementStatus.Added,
                    Tags =tags
                });
                series =  MongoDBSingleton.Instance().Series.Find(new BsonDocument("name", newSeries.Name)).FirstOrDefault();
            }
            string defaultPath = Directory.GetFiles(model.posterDir, $"{series.Name}.*").FirstOrDefault(model.defaultImg);
            if (series.PosterPath.IsNullOrEmpty() || series.PosterPath != defaultPath)
            {
                series.mIndex = ElementStatus.Updated;
                series.PosterPath = defaultPath;
            }
            defaultPath = Directory.GetFiles(model.bacgroundDir, $"{series.Name}.*").FirstOrDefault(model.defaultImg);
            if (series.BackgroundPath.IsNullOrEmpty() || series.BackgroundPath != defaultPath)
            {
                series.mIndex = ElementStatus.Updated;
                series.BackgroundPath = defaultPath;
            }

            List<ObjectId> mySeasons = new();

            foreach (var season in newSeries.Seasons)
            {
                bool sflag = false;
                var oldSeason = MongoDBSingleton.Instance().Seasons.Find(new BsonDocument("name", season.Name)).FirstOrDefault();

                if(oldSeason == null)
                {
                    MongoDBSingleton.Instance().Seasons.InsertOne(new()
                    {
                        Name= season.Name,
                        SeriesId= series.Id,
                    });
                    oldSeason = MongoDBSingleton.Instance().Seasons.Find(new BsonDocument("name", season.Name)).FirstOrDefault();
                }
                else if(oldSeason.SeriesId != series.Id)
                {
                    oldSeason.SeriesId = series.Id;
                }

                if (!series.MySeasons.Contains(oldSeason._id)) 
                {
                    series.MySeasons.Add(oldSeason._id);
                    sflag = true;
                }

                List<ObjectId> myEpisodes = new();

                foreach (var episode in season.Episodes)
                {
                    bool eflag = false;

                    var oldEpisode = MongoDBSingleton.Instance().Episodes.Find(new BsonDocument("name", episode.Name)).FirstOrDefault();
                    if (oldEpisode == null)
                    {
                        MongoDBSingleton.Instance().Episodes.InsertOne(new()
                        {
                            Name = episode.Name,
                            SeasonId = oldSeason.Id,
                            Path = episode.Path,
                        });
                        oldEpisode = MongoDBSingleton.Instance().Episodes.Find(new BsonDocument("name", episode.Name)).FirstOrDefault();
                    }
                    else
                    {
                        if(oldEpisode.Path != episode.Path)
                        {
                            oldEpisode.Path = episode.Path;
                            eflag= true;
                        }
                        if (oldEpisode.Extention != episode.Extention)
                        {
                            oldEpisode.Extention = episode.Extention;
                            eflag= true;
                        }
                        if (oldEpisode.SeasonId != oldSeason.Id)
                        {
                            oldEpisode.SeasonId = oldSeason.Id;
                            eflag= true;
                        }
                    }

                    if (!oldSeason.MyEpisodes.Contains(oldEpisode._id)) 
                    {
                        oldSeason.MyEpisodes.Add(oldEpisode._id);
                        sflag = true;
                    }

                    if (eflag)
                    {
                        MongoDBSingleton.Instance().Episodes.FindOneAndReplace(new BsonDocument("_id", oldEpisode._id), oldEpisode);
                        sflag = true;
                    }

                    myEpisodes.Add(oldEpisode._id);
                }

                List<ObjectId> deleteList = oldSeason.MyEpisodes.Except(myEpisodes).ToList();
                if(deleteList.Count > 0)
                {
                    oldSeason.MyEpisodes.RemoveAll(x => deleteList.Contains(x));
                    MongoDBSingleton.Instance().Episodes.DeleteMany(new BsonDocument("_id", new BsonDocument("$in", new BsonArray(deleteList))));
                    sflag = true;
                }

                if (sflag)
                {
                    MongoDBSingleton.Instance().Seasons.FindOneAndReplace(new BsonDocument("_id", oldSeason._id), oldSeason);
                    if (series.mIndex != ElementStatus.Added) series.mIndex = ElementStatus.Updated;
                }
                mySeasons.Add(oldSeason._id);
            }

            List<ObjectId> deleteSeasons = series.MySeasons.Except(mySeasons).ToList();

            if(deleteSeasons.Count > 0)
            {
                series.MySeasons.RemoveAll(x=>deleteSeasons.Contains(x));
                MongoDBSingleton.Instance().Seasons.DeleteMany(new BsonDocument("_id", new BsonDocument("$in", new BsonArray(deleteSeasons))));
                if (series.mIndex != ElementStatus.Added) series.mIndex = ElementStatus.Updated;
            }

            if(series.mIndex == ElementStatus.Updated || series.mIndex == ElementStatus.Added)
            {
                MongoDBSingleton.Instance().Series.FindOneAndReplace(new BsonDocument("_id", series._id), series);
            }
        }

        private Series? IsSeries(string path, FileSystemSingletoneModel model)
        {
            Series? result = null;
            IEnumerable<string> seasons = Directory.EnumerateDirectories(path);
            if (seasons != null && seasons.Count() > 0)
            {
                result = new()
                {
                    Name = Path.GetFileNameWithoutExtension(path),
                    Path = path,
                };

                foreach (var season in seasons)
                {
                    if (!model.whiteSeasonList.Any(x => season.ToLower().Contains(x))) return null;

                    IEnumerable<string> episodes = Directory.EnumerateFiles(season);
                    if (episodes.Count() > 0)
                    {
                        string sName = Path.GetFileName(season).toTwoDigits();

                        Season thisSeason = new()
                        {
                            Name = $"{result.Name} {sName}",
                            Episodes = episodes.Select(x => new Episode()
                            {
                                Name = $"{result.Name} {sName} {Path.GetFileNameWithoutExtension(x).toTwoDigits()}",
                                Path = x,
                                Extention = Path.GetExtension(x)
                            } as IEpisodeModel).ToList()
                        };
                        result.Seasons.Add(thisSeason);
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            return result;
        }

        private List<Film> IsFilm(string path, FileSystemSingletoneModel model)
        {
            List<Film> result = new();

            foreach (var item in Directory.EnumerateFiles(path))
            {
                string extention = Path.GetExtension(item);
                if (model.blackExtensionList.Any(x => x == extention)) continue;

                string pakage = Path.GetFileName(path);
                result.Add(new Film
                {
                    Name = Path.GetFileNameWithoutExtension(item),
                    Path = item,
                    PakageName = model.blackPackageList.Any(x => x.ToLower() == pakage.ToLower()) ? null : pakage,
                    Extention = Path.GetExtension(item)
                });
            }
            return result;
        }
    }
}

using Microsoft.Extensions.Logging;
using OnlineCinemaContracts;
using OnlineCinemaContracts.Logic;
using OnlineCinemaContracts.Models.BindingModels;
using OnlineCinemaContracts.Models.SearchModels;
using OnlineCinemaContracts.Models.ViewModels;
using OnlineCinemaContracts.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCinemaBusnesLogic.Logics
{
    public class SeriesLogic : ISeriesLogic
    {
        private readonly ILogger _logger;
        private readonly ISeriesStorage _seriesStorage;
        private readonly ISeasonStorage _seasonStorage;
        private readonly IEpisodeStorage _episodeStorage;

        public SeriesLogic(ILogger<SeriesLogic> logger, ISeriesStorage seriesStorage, ISeasonStorage seasonStorage, IEpisodeStorage episodeStorage)
        {
            _logger = logger;
            _seriesStorage = seriesStorage;
            _seasonStorage = seasonStorage;
            _episodeStorage = episodeStorage;
        }

        public List<SeriesViewModel>? ReadList(SeriesSearchModel? model)
        {
            _logger.LogInformation("ReadList.");
            var list = model == null ? _seriesStorage.GetFullList() : _seriesStorage.GetFiltredList(model);
            if (list == null)
            {
                _logger.LogWarning("ReadList. Return null list.");
                return null;
            }
            _logger.LogInformation("ReadList. Return list whith Count:{Count}", list.Count);
            return list;
        }

        public SeriesViewModel? ReadElement(SeriesSearchModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            _logger.LogInformation("ReadElement. Id:{Id}.Name:{Name}", model?.Id, model?.Name);

            var Series = _seriesStorage.GetElement(model);//Нет, тут model не может быть null. Проверка есть выше
            if (Series == null)
            {
                _logger.LogWarning("ReadElement. Element not found.");
                return null;
            }
            _logger.LogInformation("ReadElement. Element find Id:{Id}", Series.Id);
            return Series;
        }

        public EpisodeViewModel? GetNextEpisode(EpisodeSearchModel? model) 
        {
            if (model == null) return null;

            var episode = _episodeStorage.GetElement(model);
            if (episode == null) return null;
            var season = _seasonStorage.GetElement(new SeasonSearchModel { Id = episode.SeasonId});
            if (season == null) return null;
            var series = _seriesStorage.GetElement(new SeriesSearchModel { Id = season.SeriesId });
            if (series == null) return null;
            int index = season.Episodes.FindIndex(x=>x.Id == episode.Id);
            if (index + 1 < season.Episodes.Count)
            {
                return season.Episodes[index + 1] as EpisodeViewModel;
            }
            index = series.Seasons.FindIndex(x => x.Id == season.Id);
            if(index +1 < series.Seasons.Count)
            {
                return (series.Seasons[index + 1] as SeasonViewModel)?.Episodes[0] as EpisodeViewModel;
            }
            return null;
        }

        public EpisodeViewModel? GetPreveousEpisode(EpisodeSearchModel? model)
        {
            if (model == null) return null;

            var episode = _episodeStorage.GetElement(model);
            if (episode == null) return null;
            var season = _seasonStorage.GetElement(new SeasonSearchModel { Id = episode.SeasonId });
            if (season == null) return null;
            var series = _seriesStorage.GetElement(new SeriesSearchModel { Id = season.SeriesId });
            if (series == null) return null;
            int index = season.Episodes.FindIndex(x => x.Id == episode.Id);
            if (index - 1 >= 0)
            {
                return season.Episodes[index - 1] as EpisodeViewModel;
            }
            index = series.Seasons.FindIndex(x => x.Id == season.Id);
            if (index - 1 >= 0)
            {
                return (series.Seasons[index - 1] as SeasonViewModel)?.Episodes[0] as EpisodeViewModel;
            }
            return null;
        }

        public bool Create(SeriesBindingModel model)
        {
            CheckModel(model);
            if (_seriesStorage.Insert(model) == null)
            {
                _logger.LogWarning("Create. Operation failed.");
                return false;
            }
            _logger.LogInformation("Create. Operation success.");
            return true;
        }

        public bool Update(SeriesBindingModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (_seriesStorage.Update(model) == null)
            {
                _logger.LogWarning("Update. Operation failed.");
                return false;
            }
            _logger.LogInformation("Update. Operation success.");
            return true;
        }

        public bool Delete(SeriesSearchModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (model.Id.IsNullOrEmpty())
                throw new ArgumentException("Delete. Id must be present!", nameof(model.Id));

            _logger.LogInformation("Delete. Id:{Id}", model.Id);
            if (_seriesStorage.Delete(model) == null)
            {
                _logger.LogWarning("Delete. Operation failed.");
                return false;
            }
            _logger.LogInformation("Delete. Operation success.");
            return true;
        }

        private void CheckModel(SeriesBindingModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (string.IsNullOrEmpty(model.Name))
                throw new ArgumentException("Name must be present!", nameof(model.Name));

            _logger.LogInformation("Series. Id:{Id}.Name:{Name}", model.Id, model.Name);

            var film = _seriesStorage.GetElement(new SeriesSearchModel
            {
                Name = model.Name
            });
            if (film != null && film.Id != model.Id)
            {
                throw new InvalidOperationException("Series with this Name is already exists!");
            }
        }
    }
}

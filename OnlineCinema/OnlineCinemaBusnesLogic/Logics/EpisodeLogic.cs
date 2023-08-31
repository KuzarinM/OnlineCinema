using Microsoft.Extensions.Logging;
using OnlineCinemaContracts;
using OnlineCinemaContracts.Logic;
using OnlineCinemaContracts.Models.BindingModels;
using OnlineCinemaContracts.Models.FileModel;
using OnlineCinemaContracts.Models.SearchModels;
using OnlineCinemaContracts.Models.SettingsModel;
using OnlineCinemaContracts.Models.ViewModels;
using OnlineCinemaContracts.Storage;
using Xabe.FFmpeg;

namespace OnlineCinemaBusnesLogic.Logics
{
    public class EpisodeLogic : IEpisodeLogic
    {
        private readonly ILogger _logger;
        private readonly IEpisodeStorage _episodeStorage;
        private readonly ISeriesLogic _seriesLogic;

        public EpisodeLogic(ILogger<EpisodeLogic> logger, IEpisodeStorage episodeStorage, ISeriesLogic seriesLogic)
        {
            _logger = logger;
            _episodeStorage = episodeStorage;
            _seriesLogic = seriesLogic;
        }

        public List<EpisodeViewModel>? ReadList(EpisodeSearchModel? model)
        {
            _logger.LogInformation("ReadList.");
            var list = model == null ? _episodeStorage.GetFullList() : _episodeStorage.GetFiltredList(model);
            if (list == null)
            {
                _logger.LogWarning("ReadList. Return null list.");
                return null;
            }
            _logger.LogInformation("ReadList. Return list whith Count:{Count}", list.Count);
            return list;
        }

        public EpisodeViewModel? ReadElement(EpisodeSearchModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            _logger.LogInformation("ReadElement. Id:{Id}.Name:{Name}", model?.Id, model?.Name);

            var Episode = _episodeStorage.GetElement(model);//Нет, тут model не может быть null. Проверка есть выше
            if (Episode == null)
            {
                _logger.LogWarning("ReadElement. Element not found.");
                return null;
            }
            _logger.LogInformation("ReadElement. Element find Id:{Id}", Episode.Id);
            return Episode;
        }

        public bool Create(EpisodeBindingModel model)
        {
            CheckModel(model);
            if (_episodeStorage.Insert(model) == null)
            {
                _logger.LogWarning("Create. Operation failed.");
                return false;
            }
            _logger.LogInformation("Create. Operation success.");
            return true;
        }

        public bool Update(EpisodeBindingModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (_episodeStorage.Update(model) == null)
            {
                _logger.LogWarning("Update. Operation failed.");
                return false;
            }
            _logger.LogInformation("Update. Operation success.");
            return true;
        }

        public bool Delete(EpisodeSearchModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (model.Id.IsNullOrEmpty())
                throw new ArgumentException("Delete. Id must be present!", nameof(model.Id));

            _logger.LogInformation("Delete. Id:{Id}", model.Id);
            if (_episodeStorage.Delete(model) == null)
            {
                _logger.LogWarning("Delete. Operation failed.");
                return false;
            }
            _logger.LogInformation("Delete. Operation success.");
            return true;
        }

        public async Task<EpisodeFileModel?> GetFile(EpisodeSearchModel model)
        {
            _logger.LogInformation("GetFile.");

            var episode = ReadElement(model);

            if (episode != null)
            {
                var returnModel = new EpisodeFileModel
                {
                    Model = episode,
                    Path = episode.Path,
                    Preveous = _seriesLogic.GetPreveousEpisode(new EpisodeSearchModel { Id = episode.Id })
                };

                if (Path.GetExtension(episode.Path) != ".mp4")//Файл не в mp4 -> конвертируем
                {
                    string output = Path.Combine(FileSystemSingletoneModel.Instance().tmpDirPath, $"{episode.Id}.mp4");

                    if (!File.Exists(output))
                    {
                        FFmpeg.SetExecutablesPath("C:\\Program Files\\FFmpeg\\bin");

                        var mediaInfo = await FFmpeg.GetMediaInfo(episode.Path);

                        await FFmpeg.Conversions.New().AddStream(mediaInfo.Streams)
                            .AddParameter("-threads 16")
                            .AddParameter("-ac 2")
                            .AddParameter("-c:v h264_nvenc")
                            .SetOutput(output).SetOutputFormat(Format.mp4).Start();
                    }

                    returnModel.Path = output;//В случае конвертации нужен путь не до файла, а до его сконвертированное версии в папке tmp
                }

                returnModel.Next = _seriesLogic.GetNextEpisode(new EpisodeSearchModel { Id = episode.Id });
                if (returnModel.Next != null)//Для следующего эпизода сложнее, ибо есть маркер его готовности. Иначе будут ошибки(плохо)
                    returnModel.HasNext = Path.GetExtension(returnModel.Next.Path) == ".mp4" || File.Exists($"{FileSystemSingletoneModel.Instance().tmpDirPath}\\{returnModel.Next.Id}.mp4");

                return returnModel;
            }
            _logger.LogWarning("Episode not found.");
            return null;
        }

        private void CheckModel(EpisodeBindingModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (string.IsNullOrEmpty(model.Name))
                throw new ArgumentException("Name must be present!", nameof(model.Name));

            _logger.LogInformation("Episode. Id:{Id}.Name:{Name}", model.Id, model.Name);

            var film = _episodeStorage.GetElement(new EpisodeSearchModel
            {
                Name = model.Name
            });
            if (film != null && film.Id != model.Id)
            {
                throw new InvalidOperationException("Episode with this Name is already exists!");
            }
        }
    }
}

using Microsoft.Extensions.Logging;
using OnlineCinemaContracts;
using OnlineCinemaContracts.Logic;
using OnlineCinemaContracts.Models.BindingModels;
using OnlineCinemaContracts.Models.FileModel;
using OnlineCinemaContracts.Models.SearchModels;
using OnlineCinemaContracts.Models.SettingsModel;
using OnlineCinemaContracts.Models.ViewModels;
using OnlineCinemaContracts.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xabe.FFmpeg;

namespace OnlineCinemaBusnesLogic.Logics
{
    public class FilmLogic : IFilmLogic
    {
        private readonly ILogger _logger;
        private readonly IFilmStorage _filmStorage;

        public FilmLogic(ILogger<FilmLogic> logger, IFilmStorage filmStorage)
        {
            _logger = logger;
            _filmStorage = filmStorage;
        }

        public List<FilmViewModel>? ReadList(FilmSearchModel? model)
        {
            _logger.LogInformation("ReadList.");
            var list = model == null ? _filmStorage.GetFullList() : _filmStorage.GetFiltredList(model);
            if (list == null)
            {
                _logger.LogWarning("ReadList. Return null list.");
                return null;
            }
            _logger.LogInformation("ReadList. Return list whith Count:{Count}", list.Count);
            return list;
        }

        public FilmViewModel? ReadElement(FilmSearchModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            _logger.LogInformation("ReadElement. Id:{Id}.Name:{Name}", model?.Id, model?.Name);

            var film = _filmStorage.GetElement(model);//Нет, тут model не может быть null. Проверка есть выше
            if (film == null)
            {
                _logger.LogWarning("ReadElement. Element not found.");
                return null;
            }
            _logger.LogInformation("ReadElement. Element find Id:{Id}", film.Id);
            return film;
        }

        public bool Create(FilmBindingModel model)
        {
            CheckModel(model);
            if (_filmStorage.Insert(model) == null)
            {
                _logger.LogWarning("Create. Operation failed.");
                return false;
            }
            _logger.LogInformation("Create. Operation success.");
            return true;
        }

        public bool Update(FilmBindingModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (_filmStorage.Update(model) == null)
            {
                _logger.LogWarning("Update. Operation failed.");
                return false;
            }
            _logger.LogInformation("Update. Operation success.");
            return true;
        }

        public bool Delete(FilmSearchModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (model.Id.IsNullOrEmpty())
                throw new ArgumentException("Delete. Id must be present!", nameof(model.Id));

            _logger.LogInformation("Delete. Id:{Id}", model.Id);
            if (_filmStorage.Delete(model) == null)
            {
                _logger.LogWarning("Delete. Operation failed.");
                return false;
            }
            _logger.LogInformation("Delete. Operation success.");
            return true;
        }

        public async Task<FilmFileModel?> GetFile(FilmSearchModel model)
        {
            _logger.LogInformation("GetFile.");

            var film = ReadElement(model);

            if (film != null)
            {
                if (Path.GetExtension(film.Path) != ".mp4")
                {
                    string output = Path.Combine(FileSystemSingletoneModel.Instance().tmpDirPath, $"{film.Id}.mp4");

                    if (!File.Exists(output))
                    {
                        FFmpeg.SetExecutablesPath("C:\\Program Files\\FFmpeg\\bin");

                        var mediaInfo = await FFmpeg.GetMediaInfo(film.Path);

                        var a = await FFmpeg.Conversions.New().AddStream(mediaInfo.Streams).SetOutput(output).SetOutputFormat(Format.mp4).Start();
                    }

                    return new FilmFileModel
                    {
                        Model = film,
                        Path = output
                    };
                }

                return new FilmFileModel
                {
                    Model = film,
                    Path = film.Path
                };
            }
            _logger.LogWarning("Film not found.");
            return null;
        }

        private void CheckModel(FilmBindingModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (string.IsNullOrEmpty(model.Name))
                throw new ArgumentException("Name must be present!", nameof(model.Name));

            _logger.LogInformation("Film. Id:{Id}.Name:{Name}", model.Id, model.Name);

            var film = _filmStorage.GetElement(new FilmSearchModel
            {
                Name = model.Name
            });
            if (film != null && film.Id != model.Id)
            {
                throw new InvalidOperationException("Film with this Name is already exists!");
            }
        }
    }
}

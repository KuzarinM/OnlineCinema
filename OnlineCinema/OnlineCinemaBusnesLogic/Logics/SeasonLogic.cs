using Microsoft.Extensions.Logging;
using OnlineCinemaContracts;
using OnlineCinemaContracts.Logic;
using OnlineCinemaContracts.Models.BindingModels;
using OnlineCinemaContracts.Models.FileModel;
using OnlineCinemaContracts.Models.SearchModels;
using OnlineCinemaContracts.Models.ViewModels;
using OnlineCinemaContracts.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCinemaBusnesLogic.Logics
{
    public class SeasonLogic : ISeasonLogic
    {
        private readonly ILogger _logger;
        private readonly ISeasonStorage _seasonStorage;

        public SeasonLogic(ILogger<SeasonLogic> logger, ISeasonStorage seasonStorage)
        {
            _logger = logger;
            _seasonStorage = seasonStorage;
        }

        public List<SeasonViewModel>? ReadList(SeasonSearchModel? model)
        {
            _logger.LogInformation("ReadList.");
            var list = model == null ? _seasonStorage.GetFullList() : _seasonStorage.GetFiltredList(model);
            if (list == null)
            {
                _logger.LogWarning("ReadList. Return null list.");
                return null;
            }
            _logger.LogInformation("ReadList. Return list whith Count:{Count}", list.Count);
            return list;
        }

        public SeasonViewModel? ReadElement(SeasonSearchModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            _logger.LogInformation("ReadElement. Id:{Id}.Name:{Name}", model?.Id, model?.Name);

            var Season = _seasonStorage.GetElement(model);//Нет, тут model не может быть null. Проверка есть выше
            if (Season == null)
            {
                _logger.LogWarning("ReadElement. Element not found.");
                return null;
            }
            _logger.LogInformation("ReadElement. Element find Id:{Id}", Season.Id);
            return Season;
        }

        public bool Create(SeasonBindingModel model)
        {
            CheckModel(model);
            if (_seasonStorage.Insert(model) == null)
            {
                _logger.LogWarning("Create. Operation failed.");
                return false;
            }
            _logger.LogInformation("Create. Operation success.");
            return true;
        }

        public bool Update(SeasonBindingModel model)
        {
            CheckModel(model);
            if (_seasonStorage.Update(model) == null)
            {
                _logger.LogWarning("Update. Operation failed.");
                return false;
            }
            _logger.LogInformation("Update. Operation success.");
            return true;
        }

        public bool Delete(SeasonSearchModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (model.Id.IsNullOrEmpty())
                throw new ArgumentException("Delete. Id must be present!", nameof(model.Id));

            _logger.LogInformation("Delete. Id:{Id}", model.Id);
            if (_seasonStorage.Delete(model) == null)
            {
                _logger.LogWarning("Delete. Operation failed.");
                return false;
            }
            _logger.LogInformation("Delete. Operation success.");
            return true;
        }

        public async Task<SeasonFileModel?> GetSeasonFolder(SeasonSearchModel model)
        {
            _logger.LogInformation("GetSeasonFolder. Id:{Id}", model?.Id);
            var season = ReadElement(model);

            if (season != null)
            {
                string zipPath = $"{GlobalLogicSettings.tmpDirPath}/CinemaCash/{model.Id}.zip";

                if (!File.Exists(zipPath))
                {
                    FileStream fs = File.Open(zipPath, FileMode.Create);
                    await Task.Run(async () =>
                    {
                        using (ZipArchive archive = new ZipArchive(fs, ZipArchiveMode.Update))
                        {
                            foreach (var item in season.Episodes)
                            {
                                await Task.Run(()=> { archive.CreateEntryFromFile(item.Path, $"{item.Name}.{item.Extention}"); });
                            }
                        }
                    });
                }

                return new SeasonFileModel
                {
                    Model = season,
                    Stream = File.OpenRead(zipPath)
                };
            }

            return null;
        } 

        private void CheckModel(SeasonBindingModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (string.IsNullOrEmpty(model.Name))
                throw new ArgumentException("Name must be present!", nameof(model.Name));

            _logger.LogInformation("Season. Id:{Id}.Name:{Name}", model.Id, model.Name);

            var film = _seasonStorage.GetElement(new SeasonSearchModel
            {
                Name = model.Name
            });
            if (film != null && film.Id != model.Id)
            {
                throw new InvalidOperationException("Season with this Name is already exists!");
            }
        }
    }
}

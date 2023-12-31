﻿using Microsoft.Extensions.Logging;
using OnlineCinemaContracts.Logic;
using OnlineCinemaContracts.Models.FileModel;
using OnlineCinemaContracts.Models.SettingsModel;
using OnlineCinemaContracts.Storage;

namespace OnlineCinemaBusnesLogic.Logics
{
    public class ServiceLogic : IServiceLogic
    {
        private readonly ILogger _logger;
        private readonly IFileConverter _fileConverter;
        private readonly IEpisodeStorage _episodeStorage;
        private readonly IFilmStorage _filmStorage;
        private readonly ISeasonStorage _seasonStorage;
        private readonly IUserLogic _userLogic;

        public ServiceLogic(ILogger<ServiceLogic> logger, IFileConverter fileConverter, IEpisodeStorage episodeStorage, IFilmStorage filmStorage, ISeasonStorage seasonStorage, IUserLogic userLogic)
        {
            _logger = logger;
            _fileConverter = fileConverter;
            _episodeStorage = episodeStorage;
            _filmStorage = filmStorage;
            _seasonStorage = seasonStorage;
            _userLogic = userLogic;
        }

        public void LoadData(FileSystemSingletoneModel model)
        {
            _logger.LogInformation("LoadData. ");
            _fileConverter.LoadData(model);
        }

        public void TruncateTMPFolder()
        {
            string tmpDir = $"{FileSystemSingletoneModel.Instance().tmpDirPath}/CinemaCash";

            foreach (var item in Directory.GetFiles(tmpDir))
            {
                File.Delete(item);
            }
            foreach (var item in Directory.GetDirectories(tmpDir))
            {
                Directory.Delete(item);
            }
        }

        public List<FileViewModel> GetTMPFolderData()
        {
            List<FileViewModel> resout = new();
            foreach (var item in Directory.EnumerateFiles($"{FileSystemSingletoneModel.Instance().tmpDirPath}", "*.mp4"))
            {
                string id = Path.GetFileNameWithoutExtension(item);
                FileViewModel tmp = null;

                var episode = _episodeStorage.GetElement(new() { Id = id });
                if (episode == null)
                {
                    var film = _filmStorage.GetElement(new() { Id = id });
                    if (film != null)
                    {
                        tmp = new()
                        {
                            Name = film.Name,
                            Extentions = ".mp4",
                            Type = "Фильм",
                        };
                    }
                }
                else
                {
                    tmp = new()
                    {
                        Name = episode.Name,
                        Extentions = ".mp4",
                        Type = "Сериал",
                    };
                }

                if (tmp != null)
                {
                    tmp.Size = new FileInfo(item).Length / 1024f / 1024;
                    resout.Add(tmp);
                }
            }

            foreach (var item in Directory.EnumerateFiles(FileSystemSingletoneModel.Instance().tmpDirPath, "*.zip"))
            {
                string id = Path.GetFileNameWithoutExtension(item);
                FileViewModel tmp = null;

                var season = _seasonStorage.GetElement(new() { Id = id });

                if (season != null)
                {
                    resout.Add(new FileViewModel
                    {
                        Name = season.Name,
                        Extentions = ".zip",
                        Type = "Сезон",
                        Size = new FileInfo(item).Length / 1024f / 1024
                    });
                }
            }

            return resout;
        }

        public void CreateAdmin()
        {
            var admin = _userLogic.ReadElement(new()
            {
                Login = "admin",
                Password = null
            });
            if (admin == null)
            {
                _userLogic.Create(new()
                {
                    Login = "admin",
                    Password = "admin",
                    Role = OnlineCinemaContracts.Enums.UserRole.ADMIN
                });
            }
        }
    }
}

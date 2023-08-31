using Microsoft.Extensions.Logging;
using OnlineCinemaContracts.Logic;
using OnlineCinemaContracts.Models.SettingsModel;
using OnlineCinemaContracts.Storage;

namespace OnlineCinemaBusnesLogic.Logics
{
    public class FileConverterLogic : IFileConverterLogic
    {
        private readonly ILogger _logger;
        private readonly IFileConverter _fileConverter;

        public FileConverterLogic(ILogger<FileConverterLogic> logger, IFileConverter fileConverter)
        {
            _logger = logger;
            _fileConverter = fileConverter;
        }

        public void LoadData(FileSystemSingletoneModel model)
        {
            _logger.LogInformation("Loading data from Disk.");
            _fileConverter.LoadData(model);
        }
    }
}

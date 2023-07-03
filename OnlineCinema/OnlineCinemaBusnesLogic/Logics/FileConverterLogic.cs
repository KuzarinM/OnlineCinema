using Microsoft.Extensions.Logging;
using OnlineCinemaContracts.Logic;
using OnlineCinemaContracts.Models.SettingsModel;
using OnlineCinemaContracts.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public void LoadData(FileSystemDiskModel model)
        {
            _logger.LogInformation("Loading data from Disk.");
            _fileConverter.LoadData(model);
        }
    }
}

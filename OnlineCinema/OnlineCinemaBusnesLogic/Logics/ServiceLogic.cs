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
    public class ServiceLogic : IServiceLogic
    {
        private readonly ILogger _logger;
        private readonly IFileConverter _fileConverter;

        public ServiceLogic(ILogger<ServiceLogic> logger, IFileConverter fileConverter)
        {
            _logger = logger;
            _fileConverter = fileConverter;
        }

        public void LoadData(FileSystemDiskModel model)
        {
            _logger.LogInformation("LoadData. ");
            _fileConverter.LoadData(model);
        }

        public void TruncateTMPFolder()
        {
            string tmpDir = $"{GlobalLogicSettings.tmpDirPath}/CinemaCash";

            foreach (var item in Directory.GetFiles(tmpDir))
            {
                File.Delete(item);
            }
            foreach (var item in Directory.GetDirectories(tmpDir))
            {
                Directory.Delete(item);
            }
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineCinemaContracts.Logic;
using OnlineCinemaContracts.Models.SettingsModel;
using OnlineCinemaContracts.Storage;

namespace OnlineCinemaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Tags("Сервис")]
    [Authorize(Roles ="ADMIN")]
    public class ServiceController : Controller
    {
        private readonly ILogger _logger;
        private readonly IServiceLogic _logic;
        private readonly IUserLogic _userLogic;
        
        public ServiceController(ILogger<ServiceCollection> logger, IServiceLogic logic, IUserLogic userLogic)
        {
            _logger = logger;
            _logic = logic;
            _userLogic = userLogic;
        }

        [HttpGet("synchronization")]
        [Authorize(Roles ="ADMIN")]
        public void DiscSynchronization()
        {
            _logger.LogInformation("Trying to synchronize to disk");
            try
            {
                _logic.LoadData(new FileSystemDiskModel
                {
                    drivePath = "E:\\",
                    blackFolderList = new()
                    {
                        "EKT_PVR",
                        "EKT_PVR_TIMESHIFT",
                        "значки",
                        "System Volume Information",
                        "SERVICES"
                    },
                    whiteSeasonList = new()
                    {
                        "сезон",
                        "диск"
                    },
                    blackExtensionList = new()
                    {
                        ".7z",
                        ".ini",
                        ".db",
                        ".srt"
                    },
                    blackPackageList = new()
                    {
                        "фильмы"
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in synchronize to disk");
                Response.StatusCode = 500;
            }
        }

        [HttpDelete("tmpTruncate")]
        [Authorize(Roles = "ADMIN")]
        public void TruncateTMP()
        {
            _logger.LogInformation("Trying to delete all from tmp dir");
            try
            {
                _logic.TruncateTMPFolder();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in deleting all from tmp dir");
                Response.StatusCode = 500;
            }
        }

    }
}

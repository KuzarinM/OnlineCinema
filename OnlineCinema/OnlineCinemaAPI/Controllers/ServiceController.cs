using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineCinemaContracts.Logic;
using OnlineCinemaContracts.Models.FileModel;
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
                _logic.LoadData(FileSystemSingletoneModel.Instance());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in synchronize to disk");
                Response.StatusCode = 500;
            }
        }

        [HttpGet("settings")]
        [Authorize(Roles = "ADMIN")]
        public FileSystemSingletoneModel? GetFileSettings()
        {
            _logger.LogInformation("Trying to get a File settings model" );
            try
            {
                return FileSystemSingletoneModel.Instance();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in getting File settings model");
                Response.StatusCode = 500;
                return null;
            }
        }

        [HttpPost("settings")]
        [Authorize(Roles = "ADMIN")]
        public void UpdateFileSettings(FileSystemSingletoneModel model)
        {
            _logger.LogInformation("Trying to update a File settings model");
            try
            {
                FileSystemSingletoneModel.update(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in updating File settings model");
                Response.StatusCode = 500;
            }
        }

        [HttpGet("tmpGet")]
        [Authorize(Roles = "ADMIN")]
        public List<FileViewModel>? GetTMP()
        {
            _logger.LogInformation("Trying to get tmp folder info");
            try
            {
                return _logic.GetTMPFolderData();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in getting tmp folder info");
                Response.StatusCode = 500;
                return null;
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

        [HttpGet("init")]
        public void CreateAdmin()
        {
            _logger.LogInformation("Trying to create ADMIN user");
            try
            {
                _logic.CreateAdmin();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in creating ADMIN user");
                Response.StatusCode = 500;
            }
        }

    }
}

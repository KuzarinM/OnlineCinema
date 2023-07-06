using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using OnlineCinemaContracts;
using OnlineCinemaContracts.Logic;
using OnlineCinemaContracts.Models.BindingModels;
using OnlineCinemaContracts.Models.SearchModels;
using OnlineCinemaContracts.Models.ViewModels;
using OnlineCinemaStorageDatabase.Models;

namespace OnlineCinemaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Tags("Сезоны")]
    public class SeasonsController : Controller
    {
        private readonly ILogger _logger;
        private readonly ISeasonLogic _logic;

        public SeasonsController(ILogger<SeasonsController> logger, ISeasonLogic logic)
        {
            _logger = logger;
            _logic = logic;
        }

        [HttpGet]
        public List<SeasonViewModel>? GetSeasons()
        {
            _logger.LogInformation("Trying to get a list of episodes");
            try
            {
                var list = _logic.ReadList(null);
                if (list == null || list.Count == 0)
                    return null;
                return list;
                //204 = No Content 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in getting a list of episodes");
                Response.StatusCode = 500;
                return null;
            }
        }

        [HttpGet("{id}")]
        public SeasonViewModel? GetSeasonById(string id)
        {
            _logger.LogInformation("Trying to get a episode by Id:{Id}", id);
            try
            {
                return _logic.ReadElement(new SeasonSearchModel
                {
                    Id = id
                });
                //204 = No Content 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in getting a episode by Id");
                Response.StatusCode = 500;
                return null;
            }
        }

        [HttpPut]//todo WARNING Так то, по правилам REST нужно было бы и тут по id делать, но так как в BindingModel он уже есть, пусть будет вот так
        public void UpdateSeason(SeasonBindingModel model)
        {
            if (model.Id.IsNullOrEmpty())
            {
                Response.StatusCode = 400;
                return;
            }
            _logger.LogInformation("Trying to update a episode with Id:{Id}", model.Id);
            try
            {
                if (_logic.Update(model))
                    Response.StatusCode = 202;//202 = Accepted 
                else
                    Response.StatusCode = 400;//Ответа "Не получилось" я не нашёл
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in updating a episode info");
                Response.StatusCode = 500;
            }
        }

        [HttpDelete("{id}")]
        public void DeleteSeason(string id)
        {
            _logger.LogInformation("Trying to delete a episode with Id:{Id}", id);
            try
            {
                if (_logic.Delete(new SeasonSearchModel
                {
                    Id = id
                }))
                    Response.StatusCode = 202;//202 = Accepted 
                else
                    Response.StatusCode = 400;//Ответа "Не получилось" я не нашёл
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in updating a episode info");
                Response.StatusCode = 500;
            }
        }

        [HttpGet("{id}/download")]
        public async Task<string?> DownloadSeason(string id)
        {
            _logger.LogInformation("Trying to download a season by Id:{Id}", id);
            try
            {
                var season = await _logic.GetSeasonFolder(new SeasonSearchModel
                {
                    Id = id
                });
                if (season != null && !season.path.IsNullOrEmpty())
                {
                    return season.path;
                }
                Response.StatusCode = 500;
                return null;
                //204 = No Content 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in getting a episode by Id");
                Response.StatusCode = 500;
                return null;
            }
        }
    }
}

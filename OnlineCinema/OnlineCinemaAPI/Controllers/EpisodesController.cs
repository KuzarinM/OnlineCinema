using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using OnlineCinemaContracts.Logic;
using OnlineCinemaContracts.Models.BindingModels;
using OnlineCinemaContracts.Models.SearchModels;
using OnlineCinemaContracts.Models.ViewModels;
using OnlineCinemaContracts;
using OnlineCinemaContracts.Models.FileModel;

namespace OnlineCinemaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Tags("Серии")]
    public class EpisodesController : Controller
    {
        private readonly ILogger _logger;
        private readonly IEpisodeLogic _logic;

        public EpisodesController(ILogger<EpisodesController> logger, IEpisodeLogic logic)
        {
            _logger = logger;
            _logic = logic;
        }

        [HttpGet]
        public List<EpisodeViewModel>? GetEpisodes()
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
        public EpisodeViewModel? GetEpisodeById(string id)
        {
            _logger.LogInformation("Trying to get a episode by Id:{Id}", id);
            try
            {
                return _logic.ReadElement(new EpisodeSearchModel
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
        public void UpdateEpisode(EpisodeBindingModel model)
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
        public void DeleteEpisode(string id)
        {
            _logger.LogInformation("Trying to delete a episode with Id:{Id}", id);
            try
            {
                if (_logic.Delete(new EpisodeSearchModel
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
        public async Task<EpisodeFileModel?> DownloadEpisode(string id)
        {
            _logger.LogInformation("Trying to download a episode by Id:{Id}", id);
            try
            {
                return await _logic.GetFile(new EpisodeSearchModel
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
    }
}

using Microsoft.AspNetCore.Mvc;
using OnlineCinemaContracts;
using OnlineCinemaContracts.Logic;
using OnlineCinemaContracts.Models.BindingModels;
using OnlineCinemaContracts.Models.SearchModels;
using OnlineCinemaContracts.Models.ViewModels;

namespace OnlineCinemaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Tags("Сериалы")]
    public class SeriesController : Controller
    {
        private readonly ILogger _logger;
        private readonly ISeriesLogic _logic;

        public SeriesController(ILogger<SeriesController> logger, ISeriesLogic logic)
        {
            _logger = logger;
            _logic = logic;
        }

        [HttpGet]
        public List<SeriesViewModel>? GetSeriess()
        {
            _logger.LogInformation("Trying to get a list of series");
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
                _logger.LogError(ex, "Error in getting a list of series");
                Response.StatusCode = 500;
                return null;
            }
        }

        [HttpPost]
        public List<SeriesViewModel>? GetSeriess(SeriesSearchModel model)
        {
            _logger.LogInformation("Trying to get a filtred list of series");
            try
            {
                var list = _logic.ReadList(model);
                if (list == null || list.Count == 0)
                    return null;
                return list;
                //204 = No Content 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in getting a filtred list of series");
                Response.StatusCode = 500;
                return null;
            }
        }

        [HttpGet("{id}")]
        public SeriesViewModel? GetSeriesById(string id)
        {
            _logger.LogInformation("Trying to get a film by Id:{Id}", id);
            try
            {
                return _logic.ReadElement(new SeriesSearchModel
                {
                    Id = id
                });
                //204 = No Content 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in getting a film by Id");
                Response.StatusCode = 500;
                return null;
            }
        }

        [HttpPut]//todo WARNING Так то, по правилам REST нужно было бы и тут по id делать, но так как в BindingModel он уже есть, пусть будет вот так
        public void UpdateSeries(SeriesBindingModel model)
        {
            if (model.Id.IsNullOrEmpty())
            {
                Response.StatusCode = 400;
                return;
            }
            _logger.LogInformation("Trying to update a film with Id:{Id}", model.Id);
            try
            {
                if (_logic.Update(model))
                    Response.StatusCode = 202;//202 = Accepted 
                else
                    Response.StatusCode = 400;//Ответа "Не получилось" я не нашёл
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in updating a film info");
                Response.StatusCode = 500;
            }
        }

        [HttpDelete("{id}")]
        public void DeleteSeries(string id)
        {
            _logger.LogInformation("Trying to delete a film with Id:{Id}", id);
            try
            {
                if (_logic.Delete(new SeriesSearchModel
                {
                    Id = id
                }))
                    Response.StatusCode = 202;//202 = Accepted 
                else
                    Response.StatusCode = 400;//Ответа "Не получилось" я не нашёл
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in updating a film info");
                Response.StatusCode = 500;
            }
        }
    }
}

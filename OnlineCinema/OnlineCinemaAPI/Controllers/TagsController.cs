using Microsoft.AspNetCore.Mvc;
using OnlineCinemaContracts.Logic;
using OnlineCinemaContracts.Models.ViewModels;
using OnlineCinemaContracts.Storage;

namespace OnlineCinemaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Tags("Теги")]
    public class TagsController : Controller
    {
        private readonly ILogger _logger;
        private readonly ITagStorage _storage;

        public TagsController(ILogger<TagsController> logger, ITagStorage storage)
        {
            _logger = logger;
            _storage = storage;
        }

        [HttpGet]
        public List<TagViewModel>? GetFilms()
        {
            _logger.LogInformation("Trying to get a list of tags");
            try
            {
                var list = _storage.GetFullList();
                if (list == null || list.Count == 0)
                    return null;
                return list;
                //204 = No Content 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in getting a list of tags");
                Response.StatusCode = 500;
                return null;

            }
        }
    }
}

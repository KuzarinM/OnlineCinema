using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using OnlineCinemaContracts;
using OnlineCinemaContracts.Logic;
using OnlineCinemaContracts.Models.BindingModels;
using OnlineCinemaContracts.Models.FileModel;
using OnlineCinemaContracts.Models.SearchModels;
using OnlineCinemaContracts.Models.ViewModels;
using System.Net.Mime;

namespace OnlineCinemaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Tags("Фильмы")]
    public class FilmsController : Controller
    {
        private readonly ILogger _logger;
        private readonly IFilmLogic _logic;

        public FilmsController(ILogger<FilmsController> logger, IFilmLogic logic)
        {
            _logger = logger;
            _logic = logic;
        }

        [HttpGet]
        public List<FilmViewModel>? GetFilms()
        {
            _logger.LogInformation("Trying to get a list of films");
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
                _logger.LogError(ex, "Error in getting a list of films");
                Response.StatusCode = 500;
                return null;
            }
        }

        [HttpPost]
        public List<FilmViewModel>? GetFilms(FilmSearchModel model)
        {
            _logger.LogInformation("Trying to get a filtred list of films");
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
                _logger.LogError(ex, "Error in getting a filtred list of films");
                Response.StatusCode = 500;
                return null;
            }
        }

        [HttpGet("{id}")]
        public FilmViewModel? GetFilmById(string id)
        {
            _logger.LogInformation("Trying to get a film by Id:{Id}", id);
            try
            {
                return _logic.ReadElement(new FilmSearchModel
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
        public void UpdateFilm(FilmBindingModel model)
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
        public void DeleteFilm(string id)
        {
            _logger.LogInformation("Trying to delete a film with Id:{Id}", id);
            try
            {
                if (_logic.Delete(new FilmSearchModel
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

        [HttpGet("{id}/download")]
        public ActionResult DownloadFilm(string id)
        {
            _logger.LogInformation("Trying to download a film by Id:{Id}", id);
            try
            {
                var film = _logic.GetFile(new FilmSearchModel
                {
                    Id = id
                });
                if (film != null && film.Stream!=null)
                {
                    string? contentType;
                    new FileExtensionContentTypeProvider().TryGetContentType(Path.GetFileName(film.Model.Path), out contentType);
                    if (contentType.IsNullOrEmpty())
                        return StatusCode(500);
                    return File(film.Stream, contentType, Path.GetFileName(film.Model.Path));
                }
                return StatusCode(500);
                //204 = No Content 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in getting a film by Id");
                return StatusCode(500);
            }
        }
    }
}

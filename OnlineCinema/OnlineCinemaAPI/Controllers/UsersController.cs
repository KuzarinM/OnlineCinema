using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OnlineCinemaAPI.Sequrity;
using OnlineCinemaContracts;
using OnlineCinemaContracts.Logic;
using OnlineCinemaContracts.Models.BindingModels;
using OnlineCinemaContracts.Models.SearchModels;
using OnlineCinemaContracts.Models.ViewModels;
using OnlineCinemaContracts.Storage;
using OnlineCinemaStorageDatabase.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace OnlineCinemaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Tags("Пользователи")]
    public class UsersController : Controller
    {
        private readonly ILogger _logger;
        private readonly IUserLogic _logic;
        private readonly JWTUser _jwtUser;

        public UsersController(ILogger<UsersController> logger, IUserLogic logic, JWTUser jwtUser)
        {
            _logger = logger;
            _logic = logic;
            _jwtUser = jwtUser;
        }

        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        public List<UserViewModel>? GetUsers()
        {
            _logger.LogInformation("Trying to get a list of users");
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
                _logger.LogError(ex, "Error in getting a list of users");
                Response.StatusCode = 500;
                return null;
            }
        }

        [HttpGet("{id}")]
        public UserViewModel? GetUserById(string id)
        {
            _logger.LogInformation("Trying to get a user by Id:{Id}", id);
            try
            {
                return _logic.ReadElement(new UserSearchModel
                {
                    Id = id
                });
                //204 = No Content 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in getting a user by Id");
                Response.StatusCode = 500;
                return null;
            }
        }

        [HttpGet("me")]
        [Authorize]
        public UserViewModel? GetMyUser()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            _logger.LogInformation("Trying to get a user by token");
            try
            {
                return _jwtUser.GetUser(identity);
                //204 = No Content 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in getting a user by Id");
                Response.StatusCode = 500;
                return null;
            }
        }

        [HttpGet("role")]
        [Authorize]
        public string GetMyRole()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            _logger.LogInformation("Trying to get a role by token");
            try
            {
                return _jwtUser.GetUser(identity).Role.ToString();
                //204 = No Content 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in getting a role by token");
                Response.StatusCode = 500;
                return null;
            }
        }

        [HttpPost("login")]
        public IActionResult Token(UserBindingModel model)
        {
            var identity = _jwtUser.GetIdentity(model);
            if (identity == null)
            {
                return BadRequest(new { errorText = "Invalid username or password." });
            }

            var now = DateTime.UtcNow;
            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                username = identity.Name,
                role = identity.Claims.Last().Value,
            };

            return Json(response);
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public void CreateUser(UserBindingModel model)
        {
            _logger.LogInformation("Trying to crate a user with login:{login}", model.Login);
            try
            {
                if (_logic.Create(model))
                    Response.StatusCode = 202;//202 = Accepted 
                else
                    Response.StatusCode = 400;//Ответа "Не получилось" я не нашёл
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in updating a user info");
                Response.StatusCode = 500;
            }
        }

        [HttpPut]//todo WARNING Так то, по правилам REST нужно было бы и тут по id делать, но так как в BindingModel он уже есть, пусть будет вот так
        [Authorize]
        public void UpdateUser(UserBindingModel model)
        {
            if (model.Id.IsNullOrEmpty())
            {
                Response.StatusCode = 400;
                return;
            }
            _logger.LogInformation("Trying to update a user with Id:{Id}", model.Id);
            try
            {
                if (_logic.Update(model))
                    Response.StatusCode = 202;//202 = Accepted 
                else
                    Response.StatusCode = 400;//Ответа "Не получилось" я не нашёл
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in updating a user info");
                Response.StatusCode = 500;
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public void DeleteUser(string id)
        {
            _logger.LogInformation("Trying to delete a user with Id:{Id}", id);
            try
            {
                if (_logic.Delete(new UserSearchModel
                {
                    Id = id
                }))
                    Response.StatusCode = 202;//202 = Accepted 
                else
                    Response.StatusCode = 400;//Ответа "Не получилось" я не нашёл
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in updating a user info");
                Response.StatusCode = 500;
            }
        }
    }
}

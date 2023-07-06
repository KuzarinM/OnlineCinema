using OnlineCinemaContracts;
using OnlineCinemaContracts.Logic;
using OnlineCinemaContracts.Models.BindingModels;
using OnlineCinemaContracts.Models.SearchModels;
using OnlineCinemaContracts.Models.ViewModels;
using System.Security.Claims;

namespace OnlineCinemaAPI.Sequrity
{
    public class JWTUser
    {
        private readonly IUserLogic _logic;

        public JWTUser(IUserLogic logic)
        {
            _logic = logic;
        }

        public ClaimsIdentity GetIdentity(UserBindingModel model)
        {
            var user = _logic.ReadElement(new UserSearchModel
            {
                Login = model.Login,
                Password = model.Password.IsNullOrEmpty() ? " " : model.Password
            });

            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.ToString())
                };
                ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }

            // если пользователя не найдено
            return null;
        }

        public UserViewModel? GetUser(ClaimsIdentity identity)
        {
            if (identity != null)
            {
                string? login = identity.Name;

                return _logic.ReadElement(new UserSearchModel { Login = login });
            }
            return null;
        }
    }
}

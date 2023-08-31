using Microsoft.Extensions.Logging;
using OnlineCinemaContracts;
using OnlineCinemaContracts.Logic;
using OnlineCinemaContracts.Models.BindingModels;
using OnlineCinemaContracts.Models.SearchModels;
using OnlineCinemaContracts.Models.ViewModels;
using OnlineCinemaContracts.Storage;

namespace OnlineCinemaBusnesLogic.Logics
{
    public class UserLogic : IUserLogic
    {
        private readonly ILogger _logger;
        private readonly IUserStorage _userStorage;

        public UserLogic(ILogger<UserLogic> logger, IUserStorage userStorage)
        {
            _logger = logger;
            _userStorage = userStorage;
        }
        public List<UserViewModel>? ReadList(UserSearchModel? model)
        {
            _logger.LogInformation("ReadList.");
            var list = model == null ? _userStorage.GetFullList() : _userStorage.GetFiltredList(model);
            if (list == null)
            {
                _logger.LogWarning("ReadList. Return null list.");
                return null;
            }
            _logger.LogInformation("ReadList. Return list whith Count:{Count}", list.Count);
            return list;
        }

        public UserViewModel? ReadElement(UserSearchModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            _logger.LogInformation("ReadElement. Id:{Id}.Login:{login}", model?.Id, model?.Login);

            var User = _userStorage.GetElement(model);//Нет, тут model не может быть null. Проверка есть выше
            if (User == null)
            {
                _logger.LogWarning("ReadElement. Element not found.");
                return null;
            }
            _logger.LogInformation("ReadElement. Element find Id:{Id}", User.Id);
            return User;
        }

        public bool Create(UserBindingModel model)
        {
            CheckModel(model);
            if (_userStorage.Insert(model) == null)
            {
                _logger.LogWarning("Create. Operation failed.");
                return false;
            }
            _logger.LogInformation("Create. Operation success.");
            return true;
        }

        public bool Update(UserBindingModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (_userStorage.Update(model) == null)
            {
                _logger.LogWarning("Update. Operation failed.");
                return false;
            }
            _logger.LogInformation("Update. Operation success.");
            return true;
        }

        public bool Delete(UserSearchModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (model.Id.IsNullOrEmpty())
                throw new ArgumentException("Delete. Id must be present!", nameof(model.Id));

            _logger.LogInformation("Delete. Id:{Id}", model.Id);
            if (_userStorage.Delete(model) == null)
            {
                _logger.LogWarning("Delete. Operation failed.");
                return false;
            }
            _logger.LogInformation("Delete. Operation success.");
            return true;
        }

        private void CheckModel(UserBindingModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (string.IsNullOrEmpty(model.Login))
                throw new ArgumentException("Login must be present!", nameof(model.Login));

            _logger.LogInformation("User. Id:{Id}.Login:{login}", model.Id, model.Login);

            var film = _userStorage.GetElement(new UserSearchModel
            {
                Login = model.Login
            });
            if (film != null && film.Id != model.Id)
            {
                throw new InvalidOperationException("User with this Name is already exists!");
            }
        }
    }
}

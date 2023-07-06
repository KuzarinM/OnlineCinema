using OnlineCinemaContracts.Models.BindingModels;
using OnlineCinemaContracts.Models.SearchModels;
using OnlineCinemaContracts.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCinemaContracts.Storage
{
    public interface IUserStorage
    {
        List<UserViewModel> GetFullList();

        List<UserViewModel> GetFiltredList(UserSearchModel model);

        UserViewModel? GetElement(UserSearchModel model);

        UserViewModel? Insert(UserBindingModel model);

        UserViewModel? Update(UserBindingModel model);

        UserViewModel? Delete(UserSearchModel model);
    }
}

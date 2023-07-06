using OnlineCinemaContracts.Models.BindingModels;
using OnlineCinemaContracts.Models.SearchModels;
using OnlineCinemaContracts.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCinemaContracts.Logic
{
    public interface IUserLogic
    {
        List<UserViewModel>? ReadList(UserSearchModel? model);

        UserViewModel? ReadElement(UserSearchModel model);

        bool Create(UserBindingModel model);

        bool Update(UserBindingModel model);

        bool Delete(UserSearchModel model);
    }
}

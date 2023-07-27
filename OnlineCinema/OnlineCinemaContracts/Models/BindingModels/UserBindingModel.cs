using Newtonsoft.Json;
using OnlineCinemaContracts.Enums;
using OnlineCinemaContracts.Models.Interfases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCinemaContracts.Models.BindingModels
{
    public class UserBindingModel : IUserModel
    {
        public string Id { get; set; } = string.Empty;

        public string Login { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public UserRole Role { get; set; } = UserRole.USER;
    }
}

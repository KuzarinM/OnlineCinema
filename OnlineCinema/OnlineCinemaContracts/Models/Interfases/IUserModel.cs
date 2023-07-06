using OnlineCinemaContracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCinemaContracts.Models.Interfases
{
    public interface IUserModel : IId
    {
        public string Login { get; }

        public string Password { get; }

        public UserRole Role { get; }
    }
}

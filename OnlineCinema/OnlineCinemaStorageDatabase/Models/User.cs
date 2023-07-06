using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using OnlineCinemaContracts;
using OnlineCinemaContracts.Enums;
using OnlineCinemaContracts.Models.BindingModels;
using OnlineCinemaContracts.Models.Interfases;
using OnlineCinemaContracts.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCinemaStorageDatabase.Models
{
    [BsonIgnoreExtraElements]
    public class User : IUserModel
    {
        [BsonId]
        public ObjectId _id;

        [BsonIgnore]
        public string Id
        {
            get
            {
                return _id.ToString();
            }
            set
            {
                if(!value.IsNullOrEmpty())
                    _id = ObjectId.Parse(value);
            }
        }

        [BsonElement("login")]
        public string Login { get; set; } = string.Empty;

        [BsonElement("password")]
        public string Password { get; set; } = string.Empty;

        [BsonElement("role")]
        public UserRole Role { get; set; } = UserRole.USER;

        public static User Create(UserBindingModel user) 
        {
            return new()
            {
                Id = user.Id,
                Login = user.Login,
                Password = user.Password,
                Role = user.Role,
            };
        }

        public void Update(UserBindingModel user)
        {
            if(!user.Login.IsNullOrEmpty()) Login= user.Login;
            if(!user.Password.IsNullOrEmpty()) Password= user.Password;
        }

        public UserViewModel GetViewModel => new()
        {
            Id = Id,
            Login = Login,
            Password = Password,
            Role = Role,
        };
    }
}

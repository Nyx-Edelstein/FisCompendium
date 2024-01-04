using System.ComponentModel.DataAnnotations;
using FisCompendium.Data.User_Data;
using FisCompendium.Web.Utilities.UserData;
using Microsoft.AspNetCore.Mvc;

namespace FisCompendium.Web.Models.Account
{
    public class AccountChangePassword : IAccountAction
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string OldPassword { get; set; }

        [Required] [Remote("ValidateStrongPassword", "Account")]
        public string Password { get; set; }

        public bool CanExecute(UserLoginData existingLoginData)
        {
            if (existingLoginData == null) return false;

            return BCrypt.CheckPassword(OldPassword, existingLoginData.SaltedHash);
        }

        public UserLoginData Update(UserLoginData existingLoginData)
        {
            existingLoginData.SaltedHash = BCrypt.HashPassword(Password, BCrypt.GenerateSalt());
            return existingLoginData;
        }
    }
}

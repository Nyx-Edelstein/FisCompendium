using System.ComponentModel.DataAnnotations;
using FisCompendium.Data.User_Data;
using FisCompendium.Web.Utilities.UserData;

namespace FisCompendium.Web.Models.Account
{
    public class AccountChangeEmail : IAccountAction
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required] [EmailAddress]
        public string NewEmail { get; set; }

        public bool CanExecute(UserLoginData existingLoginData)
        {
            return existingLoginData != null && BCrypt.CheckPassword(Password, existingLoginData.SaltedHash);
        }

        public UserLoginData Update(UserLoginData existingLoginData)
        {
            existingLoginData.RecoveryEmail = NewEmail;
            return existingLoginData;
        }
    }
}

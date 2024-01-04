using System.ComponentModel.DataAnnotations;
using FisCompendium.Data.User_Data;
using FisCompendium.Web.Utilities.UserData;

namespace FisCompendium.Web.Models.Account
{
    public class AccountLoginRaw
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        public string ConfirmHuman { get; set; }
    }

    public class AccountLogin : IAccountAction
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        public static AccountLogin FromRaw(AccountLoginRaw data)
        {
            return new AccountLogin
            {
                Username = data.Username,
                Password = data.Password,
            };
        }

        public bool CanExecute(UserLoginData existingLoginData)
        {
            return existingLoginData != null && BCrypt.CheckPassword(Password, existingLoginData.SaltedHash);
        }

        public UserLoginData Update(UserLoginData existingLoginData)
        {
            return null;
        }
    }
}

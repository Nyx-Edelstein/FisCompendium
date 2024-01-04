using System.ComponentModel.DataAnnotations;
using FisCompendium.Data.User_Data;

namespace FisCompendium.Web.Models.Account
{
    public class AccountUpdatePreferences : IAccountAction
    {
        public string Username { get; set; }
        public string RedditUsername { get; set; }

        [Required]
        public bool Subscribed { get; set; }

        [Required]
        public bool UselessCheckboxPreference { get; set; }

        public bool CanExecute(UserLoginData existingLoginData)
        {
            return !string.IsNullOrWhiteSpace(Username) && Username != "Guest";
        }

        public UserLoginData Update(UserLoginData existingLoginData)
        {
            existingLoginData.Subscribed = Subscribed;
            existingLoginData.UselessCheckboxPreference = UselessCheckboxPreference;
            return existingLoginData;
        }
    }
}

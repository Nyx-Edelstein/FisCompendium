using System;
using System.ComponentModel.DataAnnotations;
using FisCompendium.Data.User_Data;

namespace FisCompendium.Web.Models.Account
{
    public class AccountPasswordRecovery
    {
        [Required]
        public string Username { get; set; }

        [Required] [EmailAddress]
        public string RecoveryEmail { get; set; }

        public bool IsValid(UserLoginData existingLoginData)
        {
            return existingLoginData != null && string.Equals(RecoveryEmail, existingLoginData.RecoveryEmail, StringComparison.OrdinalIgnoreCase);
        }
    }
}

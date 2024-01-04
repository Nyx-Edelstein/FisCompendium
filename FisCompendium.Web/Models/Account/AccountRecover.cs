using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace FisCompendium.Web.Models.Account
{
    public class AccountRecover
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string RecoveryToken { get; set; }

        [Required] [Remote("ValidateStrongPassword", "Account")]
        public string Password { get; set; }
    }
}

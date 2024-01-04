using System;
using FisCompendium.Data.User_Data;
using FisCompendium.Web.Utilities.UserData;

namespace FisCompendium.Web.Models.Account
{
    public class AccountAuthenticate
    {
        public string Username { get; set; }
        public string AuthTicket { get; set; }

        public bool IsValid(UserAuthData authData)
        {
            return authData != null
                && authData.AuthTicketExpiry >= DateTime.UtcNow
                && authData.Username == Username
                && BCrypt.CheckPassword(AuthTicket, authData.AuthTicket);
        }
    }
}

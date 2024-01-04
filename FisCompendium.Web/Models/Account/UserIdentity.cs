using System;

namespace FisCompendium.Web.Models.Account
{
    public class UserIdentity
    {
        public string Username { get; set; }
        public string AuthTicket { get; set; }
        public DateTime AuthTicketExpiry { get; set; }
    }
}

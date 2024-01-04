using System;
using FisCompendium.Repository;

namespace FisCompendium.Data.User_Data
{
    [HasStringKey("Username", isUnique: false)]
    public class UserAuthData : DataItem
    {
        public string Username { get; set; }
        public string AuthTicket { get; set; }
        public DateTime AuthTicketExpiry { get; set; }
    }
}

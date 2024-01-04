using System;
using FisCompendium.Repository;

namespace FisCompendium.Data.User_Data
{
    public class UserRecoveryToken : DataItem
    {
        public string Username { get; set; }
        public string HashedSecret { get; set; }
        public DateTime Expiry { get; set; }
    }
}

using FisCompendium.Repository;

namespace FisCompendium.Data.User_Data
{
    [HasStringKey("UserName", isUnique: true)]
    public class UserLoginData : DataItem
    {
        public string UserName { get; set; }
        public string RecoveryEmail { get; set; }
        public bool Subscribed { get; set; }
        public bool UselessCheckboxPreference { get; set; }
        public string RedditRequestIdentifier { get; set; }
        public string RedditUsername { get; set; }
        public string SaltedHash { get; set; }
    }
}

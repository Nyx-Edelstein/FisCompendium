using FisCompendium.Repository;

namespace FisCompendium.Data.User_Data
{
    [HasStringKey("Username", isUnique: true)]
    public class UserPoints : DataItem
    {
        public string Username { get; set; }
        public int CurrentPoints { get; set; }
        public int PendingPoints { get; set; }
    }
}

using FisCompendium.Repository;

namespace FisCompendium.Data.User_Data
{
    [HasStringKey("UserName", isUnique: true)]
    public class UserPermissions : DataItem
    {
        public string UserName { get; set; }
        public PermissionsLevel PermissionsLevel { get; set; }
    }

    public enum PermissionsLevel
    {
        Default = 0,
        TrustedPlayer = 1,
        QM = 2
    }
}

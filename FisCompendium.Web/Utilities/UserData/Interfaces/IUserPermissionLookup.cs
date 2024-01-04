using System.Collections.Generic;
using FisCompendium.Data.User_Data;

namespace FisCompendium.Web.Utilities.UserData.Interfaces
{
    public interface IUserPermissionLookup
    {
        bool UpdatePermissionFor(string userName, PermissionsLevel permissionsLevel);
        List<UserPermissions> GetUserPermissions();
        PermissionsLevel GetPermissionsForUser(string userName);
    }
}

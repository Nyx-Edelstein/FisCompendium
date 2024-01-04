using FisCompendium.Web.Models.Account;

namespace FisCompendium.Web.Utilities.UserData.Interfaces
{
    public interface IAccountActionExecutor
    {
        bool TryExecute(IAccountAction accountAction);
        bool TryAuthenticate(AccountAuthenticate authenticationAction);
        void Logout(string currentUser);
        //bool LogUserIP(UserIPEntry userIPEntry);
    }
}

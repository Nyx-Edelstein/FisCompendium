using FisCompendium.Web.Models.Account;

namespace FisCompendium.Web.Utilities.UserData.Interfaces
{
    public interface IAccountRecovery
    {
        void TryInitiateRecovery(AccountPasswordRecovery model);
        bool TryRecover(AccountRecover model);
    }
}

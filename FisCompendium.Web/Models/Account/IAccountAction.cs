using FisCompendium.Data.User_Data;

namespace FisCompendium.Web.Models.Account
{
    public interface IAccountAction
    {
        string Username { get; }
        bool CanExecute(UserLoginData existingLoginData);
        UserLoginData Update(UserLoginData existingLoginData);
    }
}

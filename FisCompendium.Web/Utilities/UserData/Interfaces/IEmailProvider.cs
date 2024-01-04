namespace FisCompendium.Web.Utilities.UserData.Interfaces
{
    public interface IEmailProvider
    {
        void SendRecoveryEmail(string userName, string recoveryEmail, string recoveryTicket);
    }
}

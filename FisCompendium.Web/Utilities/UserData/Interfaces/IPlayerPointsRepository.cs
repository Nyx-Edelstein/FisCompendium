using System.Collections.Generic;
using FisCompendium.Data.User_Data;
using FisCompendium.Web.Models.PlayerPoints;

namespace FisCompendium.Web.Utilities.UserData.Interfaces
{
    public interface IPlayerPointsRepository
    {
        void Update(PlayerPointsUpdate model);
        void ReleasePendingPoints();
        List<string> GetValidPlayerNames();
        List<PlayerLogEntry> GetPlayerLogs(int numToGet);
        int GetCurrentPointsForPlayer(string modelFromPlayer);
        bool CompleteTransfer(PlayerPointsTransfer model);
        List<UserPoints> GetTopCurrentPlayers(int toGet);
        UserPoints GetUserPoints(string currentUser);
    }
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using FisCompendium.Data.User_Data;
using FisCompendium.Repository;
using FisCompendium.Web.Extensions;
using FisCompendium.Web.Models.PlayerPoints;
using FisCompendium.Web.Utilities.UserData.Interfaces;

namespace FisCompendium.Web.Utilities.UserData
{
    public class PlayerPointsRepository : IPlayerPointsRepository
    {
        public IRepository<UserPoints> UserPointsRepository { get; }
        public IRepository<PlayerLogEntry> PlayerLogRepository { get; }
        public IRepository<UserLoginData> UserLoginDataRepository { get; }

        public PlayerPointsRepository(IRepository<UserPoints> userPointsRepository, IRepository<PlayerLogEntry> playerLogRepository, IRepository<UserLoginData> userLoginDataRepository)
        {
            UserPointsRepository = userPointsRepository;
            PlayerLogRepository = playerLogRepository;
            UserLoginDataRepository = userLoginDataRepository;
        }

        public void Update(PlayerPointsUpdate model)
        {
            var usernameList = model.UsernameList;

            var allRedditUsernames = UserLoginDataRepository.GetWhere(x => !string.IsNullOrWhiteSpace(x.RedditUsername));

            var redditUsernamesLookup = allRedditUsernames.ToDictionary(x => x.UserName, x => x.RedditUsername);
            var reverseLookup = allRedditUsernames.ToDictionary(x => x.RedditUsername, x => x.UserName);

            var allUserPoints = UserPointsRepository.GetWhere(x => true);

            var affectedUserPoints = allUserPoints
                .Where(x => usernameList.ContainsStandardized(x.Username) || (redditUsernamesLookup.ContainsKey(x.Username) && usernameList.Contains(redditUsernamesLookup[x.Username])))
                .ToList();

            foreach (var userPoints in affectedUserPoints)
            {
                userPoints.PendingPoints += model.PointChange;
                UserPointsRepository.Upsert(userPoints);
            }



            var newPlayerPoints = usernameList
                .Where(redditUsername =>
                {
                    if (allUserPoints.Select(x => x.Username).ContainsStandardized(redditUsername)) return false;

                    var siteUsername = reverseLookup.ContainsKey(redditUsername) ? reverseLookup[redditUsername] : null;
                    if (siteUsername == null) return true;

                    return !allUserPoints.Select(x => x.Username).ContainsStandardized(siteUsername);
                })
                .Select(username => new UserPoints
                {
                    Username = username,
                    CurrentPoints = 0,
                    PendingPoints = model.PointChange,
                }).ToList();

            foreach (var userPoints in newPlayerPoints)
            {
                UserPointsRepository.Upsert(userPoints);
            }

            var playerLogEntry = new PlayerLogEntry
            {
                Timestamp = DateTime.UtcNow.ToString("yyyy/MM/dd hh:mm:ss tt"),
                Message = model.UpdateMessage()
            };

            PlayerLogRepository.Upsert(playerLogEntry);
        }

        public void ReleasePendingPoints()
        {
            var allUserPoints = UserPointsRepository.GetWhere(x => true);

            foreach (var userPoints in allUserPoints)
            {
                userPoints.CurrentPoints += userPoints.PendingPoints;
                userPoints.PendingPoints = 0;
                UserPointsRepository.Upsert(userPoints);
            }

            var playerLogEntry = new PlayerLogEntry
            {
                Timestamp = DateTime.UtcNow.ToString("yyyy/MM/dd hh:mm:ss tt"),
                Message = "Pending points have been released."
            };

            PlayerLogRepository.Upsert(playerLogEntry);
        }

        public List<string> GetValidPlayerNames()
        {
            var allUsersWithPoints = UserPointsRepository.GetWhere(x => true)
                .Select(x => x.Username)
                .ToList();

            var allUsersWithAccounts = UserLoginDataRepository.GetWhere(x => true)
                .Select(x => x.UserName)
                .ToList();

            var validNames = allUsersWithPoints.Union(allUsersWithAccounts)
                .Distinct()
                .ToList();

            return validNames;
        }

        public List<PlayerLogEntry> GetPlayerLogs(int numToGet)
        {
            return PlayerLogRepository.GetWhere(x => true)
                .OrderByDescending(x => x.Timestamp)
                .Take(numToGet)
                .ToList();
        }

        private static bool Matches(string a, string b)
        {
            return string.Equals(a, b, StringComparison.InvariantCultureIgnoreCase);
        }

        public int GetCurrentPointsForPlayer(string playerName)
        {
            return UserPointsRepository
                .GetWhere(x => Matches(x.Username, playerName))
                .FirstOrDefault()
                ?.CurrentPoints ?? 0;
        }

        public bool CompleteTransfer(PlayerPointsTransfer model)
        {
            var fromPlayerPoints = UserPointsRepository.GetWhere(x => Matches(x.Username, model.FromPlayer)).FirstOrDefault();
            var toPlayerPoints = UserPointsRepository.GetWhere(x => Matches(x.Username, model.ToPlayer)).FirstOrDefault();

            if (fromPlayerPoints == null) return false;
            

            if (toPlayerPoints == null)
            {
                toPlayerPoints = new UserPoints
                {
                    Username = model.ToPlayer,
                    PendingPoints = 0,
                    CurrentPoints = 0,
                };
            }

            fromPlayerPoints.CurrentPoints -= model.PointChange;
            toPlayerPoints.PendingPoints += model.PointChange;

            UserPointsRepository.Upsert(fromPlayerPoints);
            UserPointsRepository.Upsert(toPlayerPoints);

            var logEntry = new PlayerLogEntry
            {
                Timestamp = DateTime.UtcNow.ToString("yyyy/MM/dd hh:mm:ss tt"),
                Message = model.UpdateMessage()
            };
            PlayerLogRepository.Upsert(logEntry);

            return true;
        }

        public List<UserPoints> GetTopCurrentPlayers(int toGet)
        {
            var userPoints = UserPointsRepository.GetWhere(x => true)
                .OrderByDescending(x => x.CurrentPoints + x.PendingPoints)
                .Take(toGet)
                .ToList();

            return userPoints;
        }

        public UserPoints GetUserPoints(string currentUser)
        {
            return UserPointsRepository.GetWhere(x => Matches(x.Username, currentUser))
                .FirstOrDefault();
        }
    }
}

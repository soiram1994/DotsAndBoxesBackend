using DotsAndBoxes.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotsAndBoxes.Services
{
    public class ActiveGames
    {
        public ActiveGames()
        {
            Games = new List<GameSession>();
        }
        public List<GameSession> Games { get; set; }

        public bool ActiveGameExists()
        {
            return Games.Any(g => g.Players.Count() == 1);
        }

        public int GetPlayerScore(string gameKey,int id)
        {
            return Games.SingleOrDefault(g => g.SessionKey == gameKey).Players.SingleOrDefault(p => p.Id == id).Score;
        }
        public GameSession GetActiveGame()
        {
            return Games.SingleOrDefault(g => g.Players.Count() == 1);
        }
        public bool ConnectionBelongsToActiveGame(string connectionid)
        {
            return Games.Any(g => g.Players.Any(p => p.ConnectionId == connectionid));
        }
        public string GetSessionKeyByConnection(string connection)
        {
            return Games.SingleOrDefault(g => g.Players.Any(p => p.ConnectionId == connection)).SessionKey;
        }
        public GameSession GetSessionByConnection(string connection)
        {
            return Games.SingleOrDefault(g => g.Players.Any(p => p.ConnectionId == connection));
        }

    }
}

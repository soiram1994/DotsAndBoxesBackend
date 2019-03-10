using DotsAndBoxes.Common.CommonModels;
using DotsAndBoxes.Common.MessageModels;
using DotsAndBoxes.Core.Entities;
using DotsAndBoxes.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DotsAndBoxes.Hubs
{
    [AllowAnonymous]
    public class GameHub:Hub
    {
        // private readonly GameChecker _gameChecker;
        private ActiveGames _gameSessions;
        public GameHub(ActiveGames gameSessions)
        {
            _gameSessions = gameSessions;
        }

        // When a player creates a new 
        public async Task NewGame(string d)
        {
            int dimension = Convert.ToInt32(d);
            if (_gameSessions.Games.Any(g => g.Players.Count() == 1))
            {
                await JoinGame();
            }
            var newKey = Guid.NewGuid().ToString();
            GameSession newGame = new GameSession(dimension)
            {
                
                SessionKey = newKey
            };
            newGame.AddPlayer(Context.ConnectionId);
            _gameSessions.Games.Add(newGame);
            await Groups.AddToGroupAsync(Context.ConnectionId, newKey.ToString());
            await Clients.Caller.SendAsync("WaitingPlayer2");
            await Clients.Others.SendAsync("EnableJoin");

        }


        //When a player makes a move
        public async Task DrawLine(Line line)
        {
            var gameSession = _gameSessions.GetSessionByConnection(Context.ConnectionId);
            var gameStatus = gameSession.PLay(line);
            if (gameStatus.Status == Status.Win)
            {
                string message;
                if (string.IsNullOrEmpty(gameStatus.WinnerName))
                    message = "Tie!";
                else
                    message = $"{gameStatus.WinnerName} has won!";
                await Clients.Group(gameSession.SessionKey).SendAsync("WinnerDecided",
                    message);
                _gameSessions.Games.Remove(gameSession);
                
            }
            else if (gameStatus.Status == Status.Score)
            {
                await Clients.Caller.SendAsync("DrawAndPlay",line);
                await Clients.GroupExcept(gameSession.SessionKey.ToString(), Context.ConnectionId).SendAsync("DrawAndWait", line);
            }
            else if(gameStatus.Status == Status.NoScore)
            {
                
                await Clients.Caller.SendAsync("DrawAndWait",line);
                await Clients.GroupExcept(gameSession.SessionKey,Context.ConnectionId).SendAsync("DrawAndPLay", line);
            }
            else if(gameStatus.Status == Status.Invalid)
            {
                await Clients.Caller.SendAsync("InvalidMove");
            }
            
        }
        //When a player connects to the hub it checks if there are any active games that he/she can join
        public override async Task OnConnectedAsync()
        {
            if (_gameSessions.ActiveGameExists())
                await Clients.Caller.SendAsync("EnableJoin");
            await base.OnConnectedAsync();
        }
        //If a game is already active with a player waiting 
        public async Task JoinGame()
        {
            var gameExists = false;
            
            do
            {
                gameExists = _gameSessions.ActiveGameExists();
                
            }
            while (!gameExists);
            var game = _gameSessions.GetActiveGame();
            var key = game.SessionKey;
            var added = _gameSessions.Games.SingleOrDefault(g => g.Players.Count() == 1).AddPlayer(Context.ConnectionId);
            if (!added)
                return;
            await Groups.AddToGroupAsync(Context.ConnectionId,key);
            if (!_gameSessions.ActiveGameExists())
                await Clients.Others.SendAsync("DisableJoin");
            await Clients.Group(key).SendAsync("StartGame",game.Dimension);
            await Clients.Caller.SendAsync("DisablePlayButton");
            
            
        }
        //When a player from an active game disconnects it informs the other player
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            if (_gameSessions.ConnectionBelongsToActiveGame(Context.ConnectionId))
            {
                await Clients.Group(_gameSessions.GetSessionKeyByConnection(Context.ConnectionId)).SendAsync("GameAborted");
                _gameSessions.Games.Remove(_gameSessions.GetSessionByConnection(Context.ConnectionId));
            }
            await  base.OnDisconnectedAsync(exception);
        }
    }
}

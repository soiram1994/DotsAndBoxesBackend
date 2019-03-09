using DotsAndBoxes.Common.CommonModels;
using DotsAndBoxes.Common.MessageModels;
using DotsAndBoxes.Core.Entities;
using DotsAndBoxes.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
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

        }


        
        public async Task DrawLine(string gameId,Line line)
        {
            var gameSession = _gameSessions.Games.SingleOrDefault(g => g.SessionKey == gameId);
            var gameStatus = gameSession.PLay(line);
            if (gameStatus.Status == Status.Win)
            {
                await Clients.Group(gameSession.SessionKey).SendAsync("WinnerDecided",
                    gameStatus.WinnerName);
            }
            else if (gameStatus.Status == Status.Score)
            {
                ViewBoard board = new ViewBoard
                {
                    DrawnLines = gameSession.Board.DrawnLines,
                    Player1_Score = gameSession.Players.SingleOrDefault(p=>p.Id==1).Score,
                    Player2_Score = gameSession.Players.SingleOrDefault(p=>p.Id==2).Score
                };
                await Clients.Caller.SendAsync("DrawAndPlay");
                await Clients.GroupExcept(gameSession.SessionKey.ToString(), Context.ConnectionId).SendAsync("UpdateBoardAndWait", board);
            }
            else if(gameStatus.Status == Status.NoScore)
            {
                ViewBoard board = new ViewBoard { DrawnLines = gameSession.Board.DrawnLines };
                await Clients.Caller.SendAsync("DrawAndWait");
                await Clients.AllExcept(Context.ConnectionId).SendAsync("UpdateBoardAndPLay", board);
            }
            else if(gameStatus.Status == Status.Invalid)
            {
                await Clients.Caller.SendAsync("InvalidMove");
            }
            
        }

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
                gameExists = _gameSessions.Games.Any(g => g.Players.Count() == 1);
                await Task.Delay(3000);
            }
            while (!gameExists);
            var game = _gameSessions.Games.SingleOrDefault(g => g.Players.Count() == 1);
            var key = game.SessionKey;
            _gameSessions.Games.SingleOrDefault(g => g.Players.Count() == 1).AddPlayer(Context.ConnectionId);
            await Groups.AddToGroupAsync(Context.ConnectionId,key);
            await Clients.Group(key).SendAsync("StartGame",game.Dimension);
            
        }
    }
}

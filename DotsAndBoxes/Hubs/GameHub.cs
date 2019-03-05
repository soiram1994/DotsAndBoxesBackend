using DotsAndBoxes.Common.CommonModels;
using DotsAndBoxes.Common.MessageModels;
using DotsAndBoxes.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotsAndBoxes.Hubs
{
    
    public class GameHub:Hub
    {
       // private readonly GameChecker _gameChecker;
        private ICollection<GameSession> _gameSessions=new List<GameSession>();


        [Route("{dimension}")]
        public async Task NewGame([FromRoute] int dimension)
        {
            var newKey = Guid.NewGuid();
            _gameSessions.Add(new GameSession(dimension) {SessionKey = newKey });
            await Groups.AddToGroupAsync(Context.ConnectionId, newKey.ToString());
            await Clients.Group(newKey.ToString()).SendAsync(newKey.ToString());

        }


        [Route("{gameId}")]
        public async Task DrawLine([FromRoute]Guid gameId,[FromBody]Line line)
        {
            var gameSession = _gameSessions.SingleOrDefault(g => g.SessionKey == gameId);
            var lineDrawn =  gameSession.Board.DrawLine(line.StartDot, line.EndDot);
            var score = gameSession.Board.CheckBox(line);
            var status = gameSession.CheckVictoryStatus();
            if (status == VictoryStatus.Win || status == VictoryStatus.Tie)
                await Clients.Group(gameSession.SessionKey.ToString()).SendAsync("WinnerDecided",
                    new GameStatus
                    {
                        VictoryStatus = status,
                        WinnerName = (gameSession.Player1_Score > gameSession.Player1_Score ?
                        gameSession.Player1.Desc : gameSession.PLayer2.Desc)
                    });
            else
            {
                if (score == Models.Score.NoScore)
                {
                    ViewBoard board = new ViewBoard { DrawnLines = gameSession.Board.DrawnLines };
                    await Clients.Caller.SendAsync("DrawAndWait");
                    await Clients.AllExcept(Context.ConnectionId).SendAsync("UpdateBoardAndPLay", board);
                }
                else
                {
                    ViewBoard board = new ViewBoard { DrawnLines = gameSession.Board.DrawnLines};
                    await Clients.Caller.SendAsync("DrawAndPlay");
                    await Clients.GroupExcept(gameSession.SessionKey.ToString(), Context.ConnectionId).SendAsync("UpdateBoardAndWait", board);
                }
            }
        }

        
        public async Task JoinGame()
        {
            var gameExists = false;
            do
            {
                gameExists = _gameSessions.Any(g => g.PLayer2 == null);
                await Task.Delay(3000);
            }
            while (gameExists);
            _gameSessions.SingleOrDefault(g => g.PLayer2 == null).PLayer2 = new Player { Id = 2, Desc = "Player2" };
        }
    }
}

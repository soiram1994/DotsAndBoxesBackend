using DotsAndBoxes.Common.CommonModels;
using DotsAndBoxes.Common.MessageModels;
using DotsAndBoxes.Core.Models;
using DotsAndBoxes.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DotsAndBoxes.Core.Entities
{
    public class GameSession
    {
       
        public GameSession(int dimension)
        {
            if (dimension < 2)
                throw new Exception("Enter a valid number for the board creation (*hint 2>=)");
            Board = new Board(dimension);
            Dimension = dimension;
            Players = new List<Player>();
            PlayerTurnId = 1;
            
        }
        public int Dimension { get; private set; }
        public string SessionKey { get; set; }
        public List<Player> Players { get; private set; } 
        public int PlayerTurnId { get; private set; }
        public Board  Board { get; private set; }
        public int NumberOfMovesLeft { get; private set; }

        public void AddPlayer(string ConnectionId)
        {
            if(Players.Count() == 0)
            {
                Players.Add(new Player { Id = 1, ConnectionId = ConnectionId, Desc = "Player1" });
            }
            else if(Players.Count() == 1)
            {
                Players.Add(new Player { Id = 2, ConnectionId = ConnectionId, Desc = "Player2" });
            }
            
        }

        public GameStatus PLay(Line line)
        {
            
            //draws line and checks if box has been created 
            var score = Board.CheckBox(line);
            //if the line cannot be drawn returns invalid
            if (score == Score.Invalid)

                return new GameStatus { Status = Status.Invalid };
            
            else if (score == Score.NoScore)
            {
                PlayerTurnId = Players.SingleOrDefault(p => p.Id != PlayerTurnId).Id;
                return new GameStatus { Status = Status.NoScore};
            }
            PassScore(score);
            var status = CheckVictoryStatus();
            if (status == VictoryStatus.Win)
            {
                return new GameStatus { Status = Status.Win, WinnerName = Players.SingleOrDefault(p => p.Id == PlayerTurnId).Desc };
            }
            else if(status == VictoryStatus.Tie)
            {
                return new GameStatus { Status = Status.Win };
            }
            else 
            {
                return new GameStatus { Status = Status.Score };
            }
         

        }
        //Passes the score and checks the game status (win/tie/undecided)
        private void PassScore(Score score)
        {
            var points = 1;
            if (score == Score.Double)
            {
                points = 2;
            }
            Players.SingleOrDefault(p => p.Id == PlayerTurnId).Score += points;
           //return CheckVictoryStatus();
            
        }
        public VictoryStatus CheckVictoryStatus()
        {
            var player1Score = Players.SingleOrDefault(p=>p.Id == 1).Score;
            var player2Score = Players.SingleOrDefault(p => p.Id == 2).Score;
            if (NumberOfMovesLeft == 0 && player1Score == player2Score)
            {
                return VictoryStatus.Tie;
            }
            
            if (Math.Min(player1Score, player2Score) + Board.Squares < Math.Max(player1Score, player2Score) )
                
            {
                
                return VictoryStatus.Win;
            }
            else
            {
                return VictoryStatus.Undecided;
            }
                
        }
    }
}

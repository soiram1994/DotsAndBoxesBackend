using DotsAndBoxes.Common.CommonModels;
using System;
using System.ComponentModel.DataAnnotations;

namespace DotsAndBoxes.Core.Models
{
    public class GameSession
    {
        [Key]
        public Guid Id { get; set; }
        public GameSession(int dimension)
        {
            if (dimension < 2)
                throw new Exception("Enter a valid number for the board creation (*hint bigger than 2)");
            Player1 = new Player { Id = 1, Desc = "Player1" };
            PLayer2 = new Player { Id = 2, Desc = "Player2" };
        }
        public int Player1_Score { get; set; }
        public int Player2_Score { get; set; }
        public Guid SessionKey { get; set; }
        public Player Player1 { get; private set; }
        public Player PLayer2 { get; private set; }
        public int LastPlayedPlayerId { get; set; }
        public Board  Board { get; set; }
        public int NumberOfMovesLeft { get; set; }

        //Iterates through the possible victory scenarios:
        //1.NumberOfMovesLeft==0 and the players' score is even
        //2.The availiable squares arent enough to even the score
        //3.NumberOfMovesLeft==0
       
        public VictoryStatus CheckVictoryStatus()
        {
            if (NumberOfMovesLeft==0&&Player1_Score==Player2_Score)
            {
                return VictoryStatus.Tie;
            }
            
            if (Math.Min(Player1_Score, Player1_Score) + Board.Squares - (Player1_Score + Player2_Score) < Math.Max(Player1_Score, Player2_Score) || (NumberOfMovesLeft == 0))
                
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

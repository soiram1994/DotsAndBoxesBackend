using DotsAndBoxes.Common.CommonModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotsAndBoxes.Common.MessageModels
{
    public class GameStatus
    {
        public VictoryStatus VictoryStatus { get; set; }
        public string WinnerName { get; set; }
    }
}

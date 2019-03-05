using DotsAndBoxes.Common.CommonModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotsAndBoxes.Common.MessageModels
{
    public class NewGameMessage
    {
        public Player Player1 { get; set; }
        public Player Player2 { get; set; }
        //public  { get; set; }
    }
}

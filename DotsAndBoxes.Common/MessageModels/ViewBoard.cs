using DotsAndBoxes.Common.CommonModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotsAndBoxes.Common.MessageModels
{
    public class ViewBoard
    {
        public List<Line> DrawnLines { get; set; }
        public int Player1_Score { get; set; }
        public int Player2_Score { get; set; }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DotsAndBoxes.Common.CommonModels
{
    public class Player
    {
        public int Id { get; set; }
        public string Desc { get; set; }
        public Guid ConnectionId { get; set; }
    }
}

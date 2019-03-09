using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotsAndBoxes.Common.CommonModels
{
    public struct Dot
    {

        //horizontal position
        [JsonProperty("x")]
        public int X { get; set; }
        //vertical position
        [JsonProperty("y")]
        public int Y { get; set; }
        //times it can be crosses
        public int TimesItCanBeCrossed { get; set; }


    }
}

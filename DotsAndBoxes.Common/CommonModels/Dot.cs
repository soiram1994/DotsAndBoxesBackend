using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotsAndBoxes.Common.CommonModels
{
    public class Dot
    {

        //horizontal position
        [JsonProperty("x")]
        public int X { get; set; }
        //vertical position
        [JsonProperty("y")]
        public int Y { get; set; }

        public override bool Equals(object obj)
        {
            var d = obj as Dot;
            if (d == null)
                return false;
            if (d.X != X || d.Y != Y)
                return false;
            return true;
        }




    }
}

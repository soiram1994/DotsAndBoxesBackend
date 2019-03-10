using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace DotsAndBoxes.Common.CommonModels
{
    public class Line
    {
        [JsonProperty("startdot")]
        public Dot StartDot { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public Axis Axis { get; set; }
        private Dot _endDot;
        [JsonProperty("enddot")]
        public Dot EndDot
        {
            get
            {
                return _endDot;
            }
            set
            {
                _endDot = value;
                CheckAxis();
            }
        }

        private void CheckAxis()
        {
            if (StartDot.X == EndDot.X)
                Axis = Axis.Horizontal;
            else
                Axis = Axis.Vertical;
        }
        public override bool Equals(object obj)
        {
            var l = obj as Line;
            if (l == null)
                return false;
            if (!l.StartDot.Equals(StartDot) || !l.EndDot.Equals(EndDot)||Axis!=l.Axis)
                return false;
            return true;
        }




    }
}

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

      


    }
}

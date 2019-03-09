using DotsAndBoxes.Common.CommonModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotsAndBoxes.Common.MessageModels
{
    public class GameStatus
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public Status Status { get; set; }
        public string WinnerName { get; set; }
    }
}

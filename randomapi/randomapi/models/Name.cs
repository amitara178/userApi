using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace randomapi.models
{
    public class Name
    {
        [JsonPropertyName("first")]
        public string First { get; set; }

        [JsonPropertyName("last")]
        public string Last { get; set; }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace randomapi.models
{
    public class Location
    {
        [JsonPropertyName("country")]
        public string Country { get; set; }
    }
}

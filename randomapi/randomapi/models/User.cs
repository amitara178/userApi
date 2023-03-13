using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace randomapi.models
{
    public class User
    {
        [JsonPropertyName("name")]
        public Name Name { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("phone")]
        public string Phone { get; set; }

        [JsonPropertyName("location")]
        public Location Location { get; set; }

        [JsonPropertyName("gender")]
        public string Gender { get; set; }

        [JsonPropertyName("age")]
        public string Age { get; set; }

        [JsonPropertyName("dob")]
        public Dob Dob { get; set; }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Text.Json.Serialization;
using System.Text.Json;





namespace randomapi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private static List<User> users = new List<User>();
        private readonly ILogger<UsersController> _logger;
        private readonly HttpClient _httpClient;

        public UsersController(ILogger<UsersController> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        [HttpGet("GetUserData")]
        public async Task<IEnumerable<User>> GetUserData(string gender)
        {
            
            string apiUrl = $"https://randomuser.me/api/?gender={gender}&results=10";
            HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

            
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                ApiResponse apiResponse = JsonSerializer.Deserialize<ApiResponse>(responseBody);
                return apiResponse.Results;
            }
            else
            {
                
                _logger.LogError("Error calling Random User Generator API: {0}", response.ReasonPhrase);
                return new List<User>();
            }
        }



        [HttpGet("mostpopularcountry")]
        public async Task<string> GetMostPopularCountry(string _gender)
        {
            List<User> users = new List<User>();
            int totalUsers = 5000; 
            int resultsPerRequest = 10; 
            int requestsNeeded = totalUsers / resultsPerRequest; 
            string gender = _gender; 

            
            for (int i = 0; i < requestsNeeded; i++)
            {
                IEnumerable<User> result = await GetUserData(gender);
                users.AddRange(result);
            }

            
            var countryCounts = users
                .GroupBy(u => u.Location.Country)
                .Select(group => new { Country = group.Key, Count = group.Count() });

            
            var mostPopularCountry = countryCounts.OrderByDescending(c => c.Count).FirstOrDefault();

            return mostPopularCountry.Country;
        }

        [HttpGet("listofmails")]
        public async Task<IEnumerable<string>> GetListOfMails()
        {
            List<User> users = new List<User>();
            int totalUsers = 30; 
            int resultsPerRequest = 10; 
            int requestsNeeded = totalUsers / resultsPerRequest; 

            
            for (int i = 0; i < requestsNeeded; i++)
            {
                IEnumerable<User> result = await GetUserData("male");
                users.AddRange(result);
            }

            
            var randomEmails = users.OrderBy(u => Guid.NewGuid()).Take(totalUsers).Select(u => u.Email);

            return randomEmails;
        }

      

        [HttpGet("oldest")]
        public async Task<string> GetTheOldestUser()
        {
            List<User> users = new List<User>();
            int totalUsers = 100; 
            int resultsPerRequest = 10; 
            int requestsNeeded = totalUsers / resultsPerRequest; 

            
            for (int i = 0; i < requestsNeeded; i++)
            {
                IEnumerable<User> result = await GetUserData("both");
                users.AddRange(result);
            }

            
            var oldestUser = users.OrderByDescending(u => u.Dob.Age).FirstOrDefault();
            return $"{oldestUser.Name.First} {oldestUser.Name.Last}, {oldestUser.Dob.Age}";
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateNewUser([FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest("User cannot be null.");
            }

            users.Add(user);

            return Ok("User added successfully.");
        }

        [HttpGet("newuser")]
        public IActionResult GetNewUser()
        {
            if (users.Count > 0)
            {
                User newUser = users.Last();
                return Ok(newUser);
            }
            else
            {
                return NotFound("No new user has been created.");
            }
        }

        [HttpPut("{UpdateNewUserByemail}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult UpdateUserData(string name, [FromBody] User updatedUser)
        {
            var userToUpdate = users.FirstOrDefault(u => u.Name == name);
            if (userToUpdate == null)
            {
                return BadRequest("User not found.");
            }

            userToUpdate.Name = updatedUser.Name;
            userToUpdate.Email = updatedUser.Email;
            userToUpdate.Dob.Date = updatedUser.Dob.Date;
            userToUpdate.Phone = updatedUser.Phone;
            userToUpdate.Location = updatedUser.Location;

            return NoContent();
        }
    }






}

/// <summary>
/// Clases
/// </summary>

    public class ApiResponse
    {
        [JsonPropertyName("results")]
        public List<User> Results { get; set; }
    }

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

public class Name
    {
        [JsonPropertyName("first")]
        public string First { get; set; }

        [JsonPropertyName("last")]
        public string Last { get; set; }
    }

    public class Location
    {
        [JsonPropertyName("country")]
        public string Country { get; set; }
    }

    public class Dob
    {
        [JsonPropertyName("date")]
        public string Date { get; set; }

        [JsonPropertyName("age")]
        public int Age { get; set; }
    }






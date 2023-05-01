using be.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Text;

namespace be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class ProfileController : ControllerBase
    {
        private readonly Settings _settings;
        private static HttpClient _httpClient = new HttpClient();
        private readonly string _baseUrl;
        private readonly TokenValidationParameters _tokenValidationParameters;

        public ProfileController(Settings settings, TokenValidationParameters tokenValidationParameters)
        {
            _settings = settings;
            _baseUrl = _settings.ServiceURLS["ProfileService"];
            _tokenValidationParameters = tokenValidationParameters;
        }



        //[HttpPost("program")]
        //[Authorize]
        //public async Task<ActionResult<string>> SaveToDB(Program_Matching_Criteria program)
        //{
        //    var authHeader = Request.Headers["Authorization"];
        //    string ProfileID = TokenDataRetrieval.GetProfileIDFromToken(authHeader, _tokenValidationParameters);string UserType = TokenDataRetrieval.GetProfileRoleFromToken(authHeader, _tokenValidationParameters);
        //    var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/api/QuizDB/program", program);
        //    response.EnsureSuccessStatusCode();
        //    return Ok(await response.Content.ReadAsStringAsync());
        //}
        [HttpPost("/Profile")]
        public async Task<IActionResult> CreateProfile([FromBody] object data)
        {
            var newdata = JsonConvert.DeserializeObject<JObject>(data.ToString());
            var authHeader = Request.Headers["Authorization"];
            string ProfileID = TokenDataRetrieval.GetProfileIDFromToken(authHeader, _tokenValidationParameters);
            string UserType = TokenDataRetrieval.GetProfileRoleFromToken(authHeader, _tokenValidationParameters);

            string url = $"{_baseUrl}/Profile/{ProfileID}";
            newdata["profile_id"] = ProfileID;
            newdata["role"] = UserType;
            string serial = JsonConvert.SerializeObject(newdata);
            var actioncontent = new StringContent(serial, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, actioncontent);
            //var response = await _httpClient.PostAsJsonAsync(url, newdata);
            string content = await response.Content.ReadAsStringAsync();
            return Ok(content);
        }
        [HttpPatch("/Profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] object data)
        {
            var newdata = JsonConvert.DeserializeObject<JObject>(data.ToString());
            var authHeader = Request.Headers["Authorization"];
            string ProfileID = TokenDataRetrieval.GetProfileIDFromToken(authHeader, _tokenValidationParameters);
            string UserType = TokenDataRetrieval.GetProfileRoleFromToken(authHeader, _tokenValidationParameters);
            string url = $"{_baseUrl}/Profile/{ProfileID}";
            string serial = JsonConvert.SerializeObject(newdata);
            var actioncontent = new StringContent(serial, Encoding.UTF8, "application/json");
            var response = await _httpClient.PatchAsync(url, actioncontent);
            string content = await response.Content.ReadAsStringAsync();
            return Ok(content);
        }
        [HttpGet("/Profile")]
        public async Task<IActionResult> GetProfile()
        {
            var authHeader = Request.Headers["Authorization"];
            string ProfileID = TokenDataRetrieval.GetProfileIDFromToken(authHeader, _tokenValidationParameters);string UserType = TokenDataRetrieval.GetProfileRoleFromToken(authHeader, _tokenValidationParameters);
            string url = $"{_baseUrl}/Profile/{ProfileID}";
            var response = await _httpClient.GetAsync(url);
            string content = await response.Content.ReadAsStringAsync();
            return Ok(content);
        }
        [HttpGet("/Profile/role")]
        public async Task<IActionResult> GetProfileRole()
        {
            var authHeader = Request.Headers["Authorization"];
            string ProfileID = TokenDataRetrieval.GetProfileIDFromToken(authHeader, _tokenValidationParameters);string UserType = TokenDataRetrieval.GetProfileRoleFromToken(authHeader, _tokenValidationParameters);
            string url = $"{_baseUrl}/Profile/{ProfileID}/role";
            var response = await _httpClient.GetAsync(url);
            string content = await response.Content.ReadAsStringAsync();
            return Ok(content);
        }
        [HttpGet("/Profile/check_accessibilty/{entityType}/{entity}")]
        public async Task<IActionResult> CheckAccessibility(string entityType, string entity)
        {
            var authHeader = Request.Headers["Authorization"];
            string ProfileID = TokenDataRetrieval.GetProfileIDFromToken(authHeader, _tokenValidationParameters);
            string UserType = TokenDataRetrieval.GetProfileRoleFromToken(authHeader, _tokenValidationParameters);
            string url = $"{_baseUrl}/Profile/{ProfileID}/check_accessibilty/{entityType}/{entity}";
            var response = await _httpClient.GetAsync(url);
            string content = await response.Content.ReadAsStringAsync();
            return Ok(content);
        }
        [HttpPost("/Profile/add_new_application")]
        public async Task<IActionResult> AddNewApplication([FromBody] object data)
        {
            var newdata = JsonConvert.DeserializeObject<JObject>(data.ToString());
            var authHeader = Request.Headers["Authorization"];
            string ProfileID = TokenDataRetrieval.GetProfileIDFromToken(authHeader, _tokenValidationParameters);
            string UserType = TokenDataRetrieval.GetProfileRoleFromToken(authHeader, _tokenValidationParameters);
            string url = $"{_baseUrl}/add_new_application/{ProfileID}";
            string serial = JsonConvert.SerializeObject(newdata);
            var actioncontent = new StringContent(serial, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, actioncontent);
            string content = await response.Content.ReadAsStringAsync();
            return Ok(content);
        }
        [HttpDelete("/Profile")]
        public async Task<IActionResult> DeleteProfile()
        {
            var authHeader = Request.Headers["Authorization"];
            string ProfileID = TokenDataRetrieval.GetProfileIDFromToken(authHeader, _tokenValidationParameters);
            string UserType = TokenDataRetrieval.GetProfileRoleFromToken(authHeader, _tokenValidationParameters);
            string url = $"{_baseUrl}/Profile/{ProfileID}";
            var response = await _httpClient.DeleteAsync(url);
            string content = await response.Content.ReadAsStringAsync();
            return Ok(content);
        }
        [HttpPatch("/Profile/quizresult")]
        public async Task<IActionResult> UpdateProfileQuizResult([FromBody] object data)
        {
            var newdata = JsonConvert.DeserializeObject<JObject>(data.ToString());
            var authHeader = Request.Headers["Authorization"];
            string ProfileID = TokenDataRetrieval.GetProfileIDFromToken(authHeader, _tokenValidationParameters);
            string UserType = TokenDataRetrieval.GetProfileRoleFromToken(authHeader, _tokenValidationParameters);
            string url = $"{_baseUrl}/Profile/set_quiz_result/{ProfileID}";
            string serial = JsonConvert.SerializeObject(newdata);
            var actioncontent = new StringContent(serial, Encoding.UTF8, "application/json");
            var response = await _httpClient.PatchAsync(url, actioncontent);
            string content = await response.Content.ReadAsStringAsync();
            return Ok(content);
        }
    }

    public static class HttpClientExtensions
    {
        public static async Task<HttpResponseMessage> PatchAsJsonAsync(this HttpClient _httpClient, string requestUri, object content)
        {
            var request = new HttpRequestMessage(new HttpMethod("PATCH"), requestUri)
            {
                Content = new StringContent(JsonConvert.SerializeObject(content))
            };
            request.Content.Headers.ContentType.MediaType = "application/json";

            return await _httpClient.SendAsync(request);
        }
    }

}

    

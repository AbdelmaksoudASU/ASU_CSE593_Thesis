using be.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UniProgramsController : ControllerBase
    {
        private readonly Settings _settings;
        private static HttpClient _httpClient = new HttpClient();
        private readonly string _baseUrl;
        private readonly TokenValidationParameters _tokenValidationParameters;


        public UniProgramsController(Settings settings, TokenValidationParameters tokenValidationParameters)
        {
            _settings = settings;
            _baseUrl = _settings.ServiceURLS["UniProgramsService"];
            _tokenValidationParameters = tokenValidationParameters;
        }

        [HttpPost("/EducationalEntity/{type}/{id}")]
        [Authorize]
        public async Task<IActionResult> CreateEducationalEntity(string type, string id, [FromBody] object data)
        {
            var authHeader = Request.Headers["Authorization"];
            string ProfileID = TokenDataRetrieval.GetProfileIDFromToken(authHeader, _tokenValidationParameters);
            string UserType = TokenDataRetrieval.GetProfileRoleFromToken(authHeader, _tokenValidationParameters);
            if (UserType != "content_creator")
            {
                return BadRequest("you do not have access");
            }
            var response = await _httpClient.GetAsync(
                $"{_settings.ServiceURLS["ProfileService"]}/Profile/{ProfileID}/check_accessibilty/{type}/{id}");
            response.EnsureSuccessStatusCode();
            var result_str = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<JObject>(result_str);
            if ((string)result["status"] == "success" && (bool)result["result"])
            {
                var json = JsonConvert.SerializeObject(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                response = await _httpClient.PostAsync($"{_baseUrl}/EducationalEntity/{type}/{id}", content);
                response.EnsureSuccessStatusCode();
                return Ok(await response.Content.ReadAsStringAsync());
            }
            else
            {
                return BadRequest("you do not have access");
            }
        }

        [HttpPatch("/EducationalEntity/{type}/{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateEducationalEntity(string type, string id, [FromBody] object data)
        {
            var newdata = JsonConvert.DeserializeObject<JObject>(data.ToString());
            var authHeader = Request.Headers["Authorization"];
            string ProfileID = TokenDataRetrieval.GetProfileIDFromToken(authHeader, _tokenValidationParameters);
            string UserType = TokenDataRetrieval.GetProfileRoleFromToken(authHeader, _tokenValidationParameters);
            if (UserType != "content_creator")
            {
                return BadRequest("you do not have access");
            }
            var response = await _httpClient.GetAsync(
                $"{_settings.ServiceURLS["ProfileService"]}/Profile/{ProfileID}/check_accessibilty/{type}/{id}");
            response.EnsureSuccessStatusCode();
            var result_str = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<JObject>(result_str);
            if ((string)result["status"] == "success" && (bool)result["result"])
            {
                var json = JsonConvert.SerializeObject(newdata);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                response = await _httpClient.PatchAsync($"{_baseUrl}/EducationalEntity/{type}/{id}", content);
                response.EnsureSuccessStatusCode();
                return Ok(await response.Content.ReadAsStringAsync());
            }
            else
            {
                return BadRequest("you do not have access");
            }
        }

        [HttpGet("/EducationalEntity/{type}/{id}")]
        public async Task<IActionResult> GetEducationalEntity(string type, string id)
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/EducationalEntity/{type}/{id}");

            return Ok(await response.Content.ReadAsStringAsync());
        }

        [HttpGet("/EducationalEntity/{type}")]
        public async Task<IActionResult> GetEducationalEntities(string type)
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/EducationalEntity/{type}");

            return Ok(await response.Content.ReadAsStringAsync());
        }

        [HttpPost("/EducationalEntityWithFilter/{type}")]
        public async Task<IActionResult> FilterEducationalEntities(string type, [FromBody] object data)
        {
            var newdata = JsonConvert.DeserializeObject<JObject>(data.ToString());
            var content = new StringContent(JsonConvert.SerializeObject(newdata), Encoding.UTF8, "application/json");
            //content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await _httpClient.PostAsync($"{_baseUrl}/EducationalEntityWithFilter/{type}", content);

            return Ok(await response.Content.ReadAsStringAsync());
        }

        [HttpDelete("/EducationalEntity/{type}/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteEducationalEntity(string type, string id)
        {
            var authHeader = Request.Headers["Authorization"];
            string ProfileID = TokenDataRetrieval.GetProfileIDFromToken(authHeader, _tokenValidationParameters);
            string UserType = TokenDataRetrieval.GetProfileRoleFromToken(authHeader, _tokenValidationParameters);
            if (UserType != "content_creator")
            {
                return BadRequest("you do not have access");
            }
            var response = await _httpClient.GetAsync(
                $"{_settings.ServiceURLS["ProfileService"]}/Profile/{ProfileID}/check_accessibilty/{type}/{id}");
            response.EnsureSuccessStatusCode();
            var result_str = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<JObject>(result_str);
            if ((string)result["status"] == "success" && (bool)result["result"])
            {
                response = await _httpClient.DeleteAsync($"{_baseUrl}/EducationalEntity/{type}/{id}");
                return Ok(await response.Content.ReadAsStringAsync());
            }
            else
            {
                return BadRequest("you do not have access");
            }
        }

        //[HttpPost("program")]
        //[Authorize]
        //public async Task<ActionResult<string>> SaveToDB(Program_Matching_Criteria program)
        //{
        //    var authHeader = Request.Headers["Authorization"];
        //    string ProfileID = TokenDataRetrieval.GetProfileIDFromToken(authHeader,_tokenValidationParameters);
        //    var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/api/QuizDB/program", program);
        //    response.EnsureSuccessStatusCode();
        //    return Ok(await response.Content.ReadAsStringAsync());
        //}

        //[HttpPatch("program")]
        //[Authorize]
        //public async Task<ActionResult<string>> UpdateDB(Dictionary<string, double> program, string ProgramName)
        //{
        //    var authHeader = Request.Headers["Authorization"];
        //    string ProfileID = TokenDataRetrieval.GetProfileIDFromToken(authHeader, _tokenValidationParameters);string UserType = TokenDataRetrieval.GetProfileRoleFromToken(authHeader, _tokenValidationParameters);

        //    var json = JsonConvert.SerializeObject(program);
        //    var content = new StringContent(json, Encoding.UTF8, "application/json");
        //    var response = await _httpClient.PatchAsync($"{_baseUrl}/api/QuizDB/program?ProgramName={ProgramName}", content);
        //    response.EnsureSuccessStatusCode();
        //    return Ok(await response.Content.ReadAsStringAsync());
        //}

        //[HttpDelete("program")]
        //[Authorize]
        //public async Task<ActionResult<string>> DeleteDB(string ProgramName)
        //{
        //    var authHeader = Request.Headers["Authorization"];
        //    string ProfileID = TokenDataRetrieval.GetProfileIDFromToken(authHeader, _tokenValidationParameters);string UserType = TokenDataRetrieval.GetProfileRoleFromToken(authHeader, _tokenValidationParameters);

        //    var response = await _httpClient.DeleteAsync($"{_baseUrl}/api/QuizDB/program?ProgramName={ProgramName}");
        //    response.EnsureSuccessStatusCode();
        //    return Ok(await response.Content.ReadAsStringAsync());
        //}

        //[HttpGet("program")]
        //public async Task<ActionResult<Program_Matching_Criteria>> GetDB(string ProgramName)
        //{
        //    var response = await _httpClient.GetAsync($"{_baseUrl}/api/QuizDB/program?ProgramName={ProgramName}");
        //    response.EnsureSuccessStatusCode();
        //    return Ok(await response.Content.ReadFromJsonAsync<Program_Matching_Criteria>());
        //}

        //[HttpPost("quizresult")]
        //public async Task<ActionResult<IEnumerable<string>>> Get_Quiz_Results(Program_Matching_Criteria Student_Scores)
        //{
        //    var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/api/QuizDB/quizresult", Student_Scores);
        //    response.EnsureSuccessStatusCode();
        //    return Ok(await response.Content.ReadFromJsonAsync<IEnumerable<string>>());
        //}



    }

}

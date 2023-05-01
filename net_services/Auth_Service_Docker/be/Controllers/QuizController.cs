using be.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Text;

namespace be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizController : ControllerBase
    {
        private readonly Settings _settings;
        private static HttpClient _httpClient = new HttpClient();
        private readonly string _baseUrl;
        private readonly TokenValidationParameters _tokenValidationParameters;

        public QuizController(Settings settings, TokenValidationParameters tokenValidationParameters)
        {
            _settings = settings;
            _baseUrl = _settings.ServiceURLS["QuizService"];
            _tokenValidationParameters = tokenValidationParameters;
        }

       

        [HttpPost("program")]
        [Authorize]
        public async Task<ActionResult<string>> SaveToDB(Program_Matching_Criteria program)
        {
            var authHeader = Request.Headers["Authorization"];
            string ProfileID = TokenDataRetrieval.GetProfileIDFromToken(authHeader,_tokenValidationParameters);
            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/api/QuizDB/program", program);
            response.EnsureSuccessStatusCode();
            return Ok(await response.Content.ReadAsStringAsync());
        }

        [HttpPatch("program")]
        [Authorize]
        public async Task<ActionResult<string>> UpdateDB(Dictionary<string, double> program, string ProgramName)
        {
            var authHeader = Request.Headers["Authorization"];
            string ProfileID = TokenDataRetrieval.GetProfileIDFromToken(authHeader, _tokenValidationParameters);string UserType = TokenDataRetrieval.GetProfileRoleFromToken(authHeader, _tokenValidationParameters);

            var json = JsonConvert.SerializeObject(program);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PatchAsync($"{_baseUrl}/api/QuizDB/program?ProgramName={ProgramName}", content);
            response.EnsureSuccessStatusCode();
            return Ok(await response.Content.ReadAsStringAsync());
        }

        [HttpDelete("program")]
        [Authorize]
        public async Task<ActionResult<string>> DeleteDB(string ProgramName)
        {
            var authHeader = Request.Headers["Authorization"];
            string ProfileID = TokenDataRetrieval.GetProfileIDFromToken(authHeader, _tokenValidationParameters);string UserType = TokenDataRetrieval.GetProfileRoleFromToken(authHeader, _tokenValidationParameters);

            var response = await _httpClient.DeleteAsync($"{_baseUrl}/api/QuizDB/program?ProgramName={ProgramName}");
            response.EnsureSuccessStatusCode();
            return Ok(await response.Content.ReadAsStringAsync());
        }

        [HttpGet("program")]
        public async Task<ActionResult<Program_Matching_Criteria>> GetDB(string ProgramName)
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/api/QuizDB/program?ProgramName={ProgramName}");
            response.EnsureSuccessStatusCode();
            return Ok(await response.Content.ReadFromJsonAsync<Program_Matching_Criteria>());
        }

        [HttpPost("quizresult")]
        public async Task<ActionResult<IEnumerable<string>>> Get_Quiz_Results(Program_Matching_Criteria Student_Scores)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/api/QuizDB/quizresult", Student_Scores);
            response.EnsureSuccessStatusCode();
            return Ok(await response.Content.ReadFromJsonAsync<IEnumerable<string>>());
        }

        

    }
}

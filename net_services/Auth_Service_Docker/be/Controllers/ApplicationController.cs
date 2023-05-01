using Azure;
using be.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Text;

namespace be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationController : ControllerBase
    {
        private readonly Settings _settings;
        private static HttpClient _httpClient = new HttpClient();
        private readonly string _baseUrl;
        private readonly TokenValidationParameters _tokenValidationParameters;

        public ApplicationController(Settings settings, TokenValidationParameters tokenValidationParameters)
        {
            _settings = settings;
            _baseUrl = _settings.ServiceURLS["ApplicationService"];
            _tokenValidationParameters = tokenValidationParameters;
        }

       

        [HttpPost("apply")]
        [Authorize]
        public async Task<ActionResult<string>> create_student_application([FromBody] object data)
        {
            var newdata = JsonConvert.DeserializeObject<JObject>(data.ToString());
            string key = await this.get_key(newdata);
            if (String.IsNullOrEmpty(key))
            {
                return BadRequest("failed to create application");
            }
            JObject customObject = new JObject();
            customObject["app_id"] = key;
            customObject["university"] = newdata["university"];
            customObject["program"] = newdata["program"];

            var authHeader = Request.Headers["Authorization"];
            string ProfileID = TokenDataRetrieval.GetProfileIDFromToken(authHeader, _tokenValidationParameters);
            string UserType = TokenDataRetrieval.GetProfileRoleFromToken(authHeader, _tokenValidationParameters);
            string url = $"{_settings.ServiceURLS["ProfileService"]}/add_new_application/{ProfileID}";
            string serial = JsonConvert.SerializeObject(customObject);
            var actioncontent = new StringContent(serial, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, actioncontent);
            string content = await response.Content.ReadAsStringAsync();
            return Ok(content);
        }

        [HttpPatch("update_student_application/{id}")]
        [Authorize]
        public async Task<ActionResult<string>> update_student_application(string id, [FromBody] object data)
        {
            var newdata = JsonConvert.DeserializeObject<JObject>(data.ToString());
            var url = $"{_baseUrl}/student_application/{id}";
            string serial = JsonConvert.SerializeObject(newdata);
            var actioncontent = new StringContent(serial, Encoding.UTF8, "application/json");
            var response = await _httpClient.PatchAsync(url, actioncontent);
            response.EnsureSuccessStatusCode();
            string content = await response.Content.ReadAsStringAsync();
            return Ok(content);
        }

        [HttpGet("student_application/{id}")]
        [Authorize]
        public async Task<ActionResult<string>> get_student_application(string id)
        {
            var url = $"{_baseUrl}/student_application/{id}";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string content = await response.Content.ReadAsStringAsync();
            return Ok(content);
        }

        [HttpPatch("update_student_application_status/{id}")]
        [Authorize]
        public async Task<ActionResult<string>> update_student_application_status(string id, [FromBody] object data)
        {
            var newdata = JsonConvert.DeserializeObject<JObject>(data.ToString());
            var url = $"{_baseUrl}/student_application_status/{id}";
            string serial = JsonConvert.SerializeObject(newdata);
            var actioncontent = new StringContent(serial, Encoding.UTF8, "application/json");
            var response = await _httpClient.PatchAsync(url, actioncontent);
            response.EnsureSuccessStatusCode();
            string content = await response.Content.ReadAsStringAsync();
            return Ok(content);
        }

        [HttpGet("student_application_status/{id}")]
        [Authorize]
        public async Task<ActionResult<string>> get_student_application_status(string id)
        {
            var url = $"{_baseUrl}/student_application_status/{id}";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string content = await response.Content.ReadAsStringAsync();
            return Ok(content);
        }

        [HttpPatch("update_student_application_status")]
        [Authorize]
        public async Task<ActionResult<string>> update_student_application_status_bulk([FromBody] object data)
        {
            var newdata = JsonConvert.DeserializeObject<JObject>(data.ToString());
            var url = $"{_baseUrl}/student_application_status";
            string serial = JsonConvert.SerializeObject(newdata);
            var actioncontent = new StringContent(serial, Encoding.UTF8, "application/json");
            var response = await _httpClient.PatchAsync(url, actioncontent);
            response.EnsureSuccessStatusCode();
            string content = await response.Content.ReadAsStringAsync();
            return Ok(content);
        }

        [HttpPost("filter_student_applications")]
        [Authorize]
        public async Task<ActionResult<string>> filter_students([FromBody] object data)
        {
            var authHeader = Request.Headers["Authorization"];
            string ProfileID = TokenDataRetrieval.GetProfileIDFromToken(authHeader, _tokenValidationParameters);
            string UserType = TokenDataRetrieval.GetProfileRoleFromToken(authHeader, _tokenValidationParameters);
            if (UserType != "uni_admin")
            {
                return BadRequest("you do not have access");
            }
            string field = await this.check_role_getuni(ProfileID);
            if (String.IsNullOrEmpty(field))
            {
                return BadRequest("you do not have access");
            }
            var newdata = JsonConvert.DeserializeObject<JObject>(data.ToString());
            var url = $"{_baseUrl}/filter_student_applications";
            string serial = JsonConvert.SerializeObject(newdata);
            var actioncontent = new StringContent(serial, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, actioncontent);
            response.EnsureSuccessStatusCode();
            string content = await response.Content.ReadAsStringAsync();
            return Ok(content);
        }

        private async Task<string> get_key(JObject data)
        {
            var url = $"{_baseUrl}/student_application";
            string serial = JsonConvert.SerializeObject(data);
            var actioncontent = new StringContent(serial, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, actioncontent);
            response.EnsureSuccessStatusCode();
            var result_str = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<JObject>(result_str);
            if ((string)result["status"] == "success" && !String.IsNullOrEmpty((string)result["key"]))
            {
                return (string)result["key"];
            }
            else
            {
                return "";
            }
        }
        private async Task<string> check_role_getuni(string ProfileID)
        {
            var response = await _httpClient.GetAsync($"{_settings.ServiceURLS["ProfileService"]}/Profile/{ProfileID}/university");
            response.EnsureSuccessStatusCode();
            var result_str = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<JObject>(result_str);
            if (!((string)result["status"] == "success"))
            {
                return "";
            }
            string field = (string)result["result"];
            return field;
        }

        [HttpPost("appform")]
        [Authorize]
        public async Task<IActionResult> CreateApplicationForm([FromBody] object data)
        {
            var newdata = JsonConvert.DeserializeObject<JObject>(data.ToString());
            var authHeader = Request.Headers["Authorization"];
            string ProfileID = TokenDataRetrieval.GetProfileIDFromToken(authHeader, _tokenValidationParameters);
            string UserType = TokenDataRetrieval.GetProfileRoleFromToken(authHeader, _tokenValidationParameters);
            if (UserType != "uni_admin")
            {
                return BadRequest("you do not have access");
            }
            string field = await this.check_role_getuni(ProfileID);
            if (String.IsNullOrEmpty(field))
            {
                return BadRequest("you do not have access");
            }
            newdata["university"] = field;
            var url = $"{_baseUrl}/appform";
            string serial = JsonConvert.SerializeObject(newdata);
            var actioncontent = new StringContent(serial, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, actioncontent);
            response.EnsureSuccessStatusCode();
            var result_str = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<JObject>(result_str);
            return Ok(result);
        }

        [HttpPatch("appform")]
        [Authorize]
        public async Task<IActionResult> UpdateApplicationForm([FromBody] object data)
        {
            var newdata = JsonConvert.DeserializeObject<JObject>(data.ToString());
            var authHeader = Request.Headers["Authorization"];
            string ProfileID = TokenDataRetrieval.GetProfileIDFromToken(authHeader, _tokenValidationParameters);
            string UserType = TokenDataRetrieval.GetProfileRoleFromToken(authHeader, _tokenValidationParameters);
            if (UserType != "uni_admin")
            {
                return BadRequest("you do not have access");
            }
            string field = await this.check_role_getuni(ProfileID);
            if (String.IsNullOrEmpty(field))
            {
                return BadRequest("you do not have access");
            }
            newdata["university"] = field;
            var url = $"{_baseUrl}/appform";
            string serial = JsonConvert.SerializeObject(newdata);
            var actioncontent = new StringContent(serial, Encoding.UTF8, "application/json");
            var response = await _httpClient.PatchAsync(url, actioncontent);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return Ok(result);
        }

        [HttpGet("appform/{id}")]
        public async Task<IActionResult> GetApplicationForm(string id)
        {
            var url = $"{_baseUrl}/appform/{id}";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return Ok(result);
        }



        //[HttpPost("quizresult")]
        //public async Task<ActionResult<IEnumerable<string>>> Get_Quiz_Results(Program_Matching_Criteria Student_Scores)
        //{
        //    var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/api/QuizDB/quizresult", Student_Scores);
        //    response.EnsureSuccessStatusCode();
        //    return Ok(await response.Content.ReadFromJsonAsync<IEnumerable<string>>());
        //}


        //private async Task<string> CreateApplicationFormAsync([FromBody] object data)
        //{
        //    var json = JsonConvert.SerializeObject(data);
        //    var content = new StringContent(json, Encoding.UTF8, "application/json");
        //    var response = await _httpClient.PostAsync($"{_baseUrl}/appform", content);
        //    response.EnsureSuccessStatusCode();
        //    return await response.Content.ReadAsStringAsync();
        //}

        //private async Task<string> UpdateApplicationFormAsync([FromBody] object data)
        //{
        //    var json = JsonConvert.SerializeObject(data);
        //    var content = new StringContent(json, Encoding.UTF8, "application/json");
        //    var response = await _httpClient.PatchAsync($"{_baseUrl}/appform", content);
        //    response.EnsureSuccessStatusCode();
        //    return await response.Content.ReadAsStringAsync();
        //}

        //private async Task<string> GetApplicationFormAsync(string id)
        //{
        //    var response = await _httpClient.GetAsync($"{_baseUrl}/appform/{id}");
        //    response.EnsureSuccessStatusCode();
        //    return await response.Content.ReadAsStringAsync();
        //}

        //private async Task<string> GetAllApplicationFormsAsync()
        //{
        //    var response = await _httpClient.GetAsync($"{_baseUrl}/appform");
        //    response.EnsureSuccessStatusCode();
        //    return await response.Content.ReadAsStringAsync();
        //}

        //private async Task<Dictionary<string, string>> CreateStudentApplicationAsync([FromBody] object data)
        //{
        //    var json = JsonConvert.SerializeObject(data);
        //    var content = new StringContent(json, Encoding.UTF8, "application/json");
        //    var response = await _httpClient.PostAsync($"{_baseUrl}/student_application", content);
        //    response.EnsureSuccessStatusCode();
        //    var result = await response.Content.ReadAsStringAsync();
        //    var key = JsonConvert.DeserializeObject<dynamic>(result).Item2.ToString();
        //    Dictionary<string, string> dic = new Dictionary<string, string>
        //    {
        //        { "res", result },
        //        { "key", key }
        //    };
        //    return dic;
        //}

        //private async Task<string> UpdateStudentApplicationAsync(string id, [FromBody] object data)
        //{
        //    var json = JsonConvert.SerializeObject(data);
        //    var content = new StringContent(json, Encoding.UTF8, "application/json");
        //    var response = await _httpClient.PatchAsync($"{_baseUrl}/student_application/{id}", content);
        //    response.EnsureSuccessStatusCode();
        //    return await response.Content.ReadAsStringAsync();
        //}

        //private async Task<string> GetStudentApplicationAsync(string id)
        //{
        //    var response = await _httpClient.GetAsync($"{_baseUrl}/student_application/{id}");
        //    response.EnsureSuccessStatusCode();
        //    return await response.Content.ReadAsStringAsync();
        //}

        //private async Task<string> GetStudentApplicationStatusAsync(string id)
        //{
        //    var response = await _httpClient.GetAsync($"{_baseUrl}/student_application_status/{id}");
        //    response.EnsureSuccessStatusCode();
        //    return await response.Content.ReadAsStringAsync();
        //}

        //private async Task<string> UpdateStudentApplicationStatusAsync(string id, string status)
        //{
        //    var data = new { status };
        //    var json = JsonConvert.SerializeObject(data);
        //    var content = new StringContent(json, Encoding.UTF8, "application/json");
        //    var response = await _httpClient.PatchAsync($"{_baseUrl}/student_application_status/{id}", content);
        //    response.EnsureSuccessStatusCode();
        //    return await response.Content.ReadAsStringAsync();
        //}



    }

}

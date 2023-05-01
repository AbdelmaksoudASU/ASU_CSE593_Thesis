using be.Data;
using be.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _context;
        private readonly Settings _settings;
        private readonly TokenValidationParameters _tokenValidationParameters;

        public AuthenticationController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            AppDbContext context,
            Settings settings,
            TokenValidationParameters tokenValidationParameters
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _settings = settings;
            _tokenValidationParameters = tokenValidationParameters;
        }

        [HttpPost("register-user")]
        public async Task<IActionResult> Register([FromBody] RegisterModel registerVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Please, provide all the required fields");
            }

            var userExists = await _userManager.FindByEmailAsync(registerVM.EmailAddress);
            if (userExists != null)
            {
                return BadRequest($"User {registerVM.EmailAddress} already exists");
            }

            ApplicationUser newUser = new ApplicationUser()
            {
                UserType = registerVM.UserType,
                ProfileID = Guid.NewGuid().ToString(),
                Email = registerVM.EmailAddress,
                UserName = registerVM.UserName,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            var result = await _userManager.CreateAsync(newUser, registerVM.Password);



            if (result.Succeeded) return Ok(result);//Ok("User created");
            //return BadRequest("User could not be created");
            return BadRequest(result);

        }

        [HttpPost("login-user")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Please, provide all required fields");
            }

            var userExists = await _userManager.FindByEmailAsync(loginVM.EmailAddress);
            if (userExists != null && await _userManager.CheckPasswordAsync(userExists, loginVM.Password))
            {
                var tokenValue = await GenerateJWTTokenAsync(userExists);
                return Ok(tokenValue);
            }
            return Unauthorized();
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenRequestModel tokenRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Please, provide all required fields");
            }
            var result = await VerifyAndGenerateTokenAsync(tokenRequest);
            return Ok(result);
        }

        private async Task<AuthResultModel> VerifyAndGenerateTokenAsync(TokenRequestModel tokenRequest)
        {
            var handler = new JwtSecurityTokenHandler();
            var stored_token = await _context.RefreshTokens.FirstOrDefaultAsync(x => x.Token == tokenRequest.RefreshToken);
            if (stored_token == null) {
                return null;
            }
            var db_user = await _userManager.FindByIdAsync(stored_token.UserId);
            try
            {
                var tokenCheckResults = handler.ValidateToken(tokenRequest.Token, 
                    _tokenValidationParameters, out var validatedToken);
                return await GenerateJWTTokenAsync(db_user, stored_token);

            }
            catch (SecurityTokenExpiredException)
            {
                if( stored_token.DateExpire >= DateTime.UtcNow)
                {
                    return await GenerateJWTTokenAsync(db_user, stored_token);
                }
                else
                {
                    return await GenerateJWTTokenAsync(db_user);
                }
            }
        }

        //[Authorize]
        //[HttpGet("confirmToken")]
        //public IActionResult GetProfileID()
        //{
        //    var authHeader = Request.Headers["Authorization"];
        //    if (authHeader.Count == 0)
        //    {
        //        // return an error response if Authorization header is missing
        //        return Unauthorized();
        //    }

        //    var token = authHeader.ToString().Split(" ")[1];
        //    var handler = new JwtSecurityTokenHandler();
        //    var validationParameters = _tokenValidationParameters;

        //    var claimsPrincipal = handler.ValidateToken(token, validationParameters, out var validatedToken);
        //    var jwtToken = (JwtSecurityToken)validatedToken;
        //    var nameId = jwtToken.Claims.First(c => c.Type == "nameid").Value;
        //    var response = new Dictionary<string, string> { { "profileid", nameId } };

        //    return Ok(response);
        //}

        private async Task<AuthResultModel> GenerateJWTTokenAsync(ApplicationUser user, RefreshToken rToken = null)
        {
            var authClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.NameId, user.ProfileID),
                new Claim(JwtRegisteredClaimNames.Typ, user.UserType),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var authSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_settings.JWT["Secret"]));

            var token = new JwtSecurityToken(
                issuer: _settings.JWT["Issuer"],
                audience: _settings.JWT["Audience"],
                expires: DateTime.UtcNow.AddMinutes(350),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256));

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            if(rToken != null)
            {
                var rTokenResponse = new AuthResultModel()
                {
                    Token = jwtToken,
                    RefreshToken = rToken.Token,
                    ExpiresAt = token.ValidTo
                };
                return rTokenResponse;
            }

            var refreshToken = new RefreshToken()
            {
                JwtId = token.Id,
                IsRevoked = false,
                UserId = user.Id,
                ProfileId = user.ProfileID,
                UserType = user.UserType,
                DateAdded = DateTime.UtcNow,
                DateExpire = DateTime.UtcNow.AddMonths(6),
                Token = Guid.NewGuid().ToString() + "-" + Guid.NewGuid().ToString()
            };
            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();


            var response = new AuthResultModel()
            {
                Token = jwtToken,
                RefreshToken = refreshToken.Token,
                ExpiresAt = token.ValidTo
            };

            return response;

        }
    }
}

using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Songcash.Model.Configuration;

namespace Songcash.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthenticationConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            var authenticationConfiguration = new AuthenticationConfiguration();
            configuration.GetSection("Authentication").Bind(authenticationConfiguration);

            _configuration = authenticationConfiguration;
        }

        [HttpGet("login")]
        public IActionResult Login()
        {
            return Challenge(new AuthenticationProperties { RedirectUri = "/Auth/callback" }, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet("callback")]
        public async Task<IActionResult> Callback()
        {
            var authenticateResult = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
            if (!authenticateResult.Succeeded) return BadRequest();

            var token = GenerateJwtToken(authenticateResult.Principal);

            return Ok(new { Token = token });
        }

        private string GenerateJwtToken(ClaimsPrincipal user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.Secret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.FindFirstValue(ClaimTypes.NameIdentifier)),
                new Claim(JwtRegisteredClaimNames.Email, user.FindFirstValue(ClaimTypes.Email)),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _configuration.Issuer,
                audience: _configuration.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [Authorize]
        [HttpGet("secure")]
        public IActionResult Secure()
        {
            return Ok("This is a secure endpoint!");
        }
    }
}
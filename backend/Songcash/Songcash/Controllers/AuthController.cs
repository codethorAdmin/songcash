using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Songcash.Model.Configuration;
using Songcash.Service;
using Songcash.Helpers;

namespace Songcash.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthenticationConfiguration _configuration;
    private readonly UserService _userService;

    public AuthController(IConfiguration configuration, UserService userService)
    {
        var authenticationConfiguration = new AuthenticationConfiguration();
        configuration.GetSection("Authentication").Bind(authenticationConfiguration);

        _configuration = authenticationConfiguration;
        _userService = userService;
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

        var rawToken = GenerateJwtToken(authenticateResult.Principal);

        var token = JwtHelper.FromRawTokenToJwtToken(rawToken);
        var email = token.Claims.FirstOrDefault(c => c.Type == "email")?.Value!;

        _ = await _userService.LoginOrRegister(email);

        return Ok(new { Token = rawToken });
    }

    private string GenerateJwtToken(ClaimsPrincipal user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.Secret));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        try
        {
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
                expires: DateTime.Now.AddHours(8),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        catch (Exception ex)
        {
            throw new UnauthorizedAccessException(ex.Message);
        }
    }
}

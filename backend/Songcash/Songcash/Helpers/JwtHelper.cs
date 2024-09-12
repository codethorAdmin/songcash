using System.IdentityModel.Tokens.Jwt;

namespace Songcash.Helpers;

public static class JwtHelper
{
    public static JwtSecurityToken FromRawTokenToJwtToken(string rawToken)
    {
        var handler = new JwtSecurityTokenHandler();
        return handler.ReadJwtToken(rawToken);
    }
}

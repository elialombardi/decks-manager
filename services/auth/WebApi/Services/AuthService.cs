using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace WebApi.Services
{
  public class AuthService(IHttpContextAccessor httpContextAccessor, IConfiguration configuration) : IAuthService
  {
    public string GetAuthId()
    {

      var auth = httpContextAccessor.HttpContext?.User;

      if (auth?.Identity == null || !auth.Identity.IsAuthenticated)
      {
        return string.Empty;
      }

      return auth.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
    }

    public string GenerateJwtToken(string userID, string role)
    {
      var key = configuration.GetValue<string>("JWT:SecretKey") ?? string.Empty;
      var issuer = configuration.GetValue<string>("JWT:Issuer") ?? string.Empty;
      var audience = configuration.GetValue<string>("JWT:Audience") ?? string.Empty;

      var handler = new JwtSecurityTokenHandler();

      var privateKey = Encoding.UTF8.GetBytes(key);

      var credentials = new SigningCredentials(
          new SymmetricSecurityKey(privateKey),
          SecurityAlgorithms.HmacSha256);

      var tokenDescriptor = new SecurityTokenDescriptor
      {
        SigningCredentials = credentials,
        Expires = DateTime.UtcNow.AddHours(1),
        Issuer = issuer,
        Audience = audience,
        Subject = new ClaimsIdentity([new Claim(ClaimTypes.Role, role), new Claim(ClaimTypes.NameIdentifier, userID)]),
      };

      var token = handler.CreateToken(tokenDescriptor);
      return handler.WriteToken(token);

    }
  }
}
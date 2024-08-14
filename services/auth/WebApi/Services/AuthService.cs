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
      var tokenHandler = new JwtSecurityTokenHandler();
      var key = Encoding.ASCII.GetBytes(configuration.GetValue<string>("JWT:SecretKey") ?? string.Empty);
      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity([new Claim(ClaimTypes.Role, role), new Claim(ClaimTypes.NameIdentifier, userID)]),
        Expires = DateTime.UtcNow.AddHours(1),
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
      };
      var token = tokenHandler.CreateToken(tokenDescriptor);
      return tokenHandler.WriteToken(token);
    }


  }
}
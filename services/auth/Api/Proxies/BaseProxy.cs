using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;

namespace Api.Proxies
{
  public class BaseProxy(IConfiguration configuration) : IBaseProxy
  {
    public string GenerateJwtToken()
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
        Subject = new ClaimsIdentity([new Claim(ClaimTypes.Role, "microservice")])
      };

      var token = handler.CreateToken(tokenDescriptor);
      return handler.WriteToken(token);
    }


  }
}
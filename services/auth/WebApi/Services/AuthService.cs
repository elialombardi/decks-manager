using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace WebApi.Services
{
  public class AuthService(IHttpContextAccessor httpContextAccessor) : IAuthService
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




  }
}
using System.Security.Claims;

namespace WebApi.Services
{
  public class AuthService(IHttpContextAccessor httpContextAccessor) : IAuthService
  {

    public string GetUserId()
    {

      var user = httpContextAccessor.HttpContext?.User;

      if (user?.Identity == null || !user.Identity.IsAuthenticated)
      {
        return string.Empty;
      }

      return user.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
    }

  }
}
namespace WebApi.Services
{
  public interface IAuthService
  {
    public string GetAuthId();
    public string GenerateJwtToken(string userID, string role);

  }
}
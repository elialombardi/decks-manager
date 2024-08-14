namespace Api.Proxies
{
  public interface IUserProxy
  {
    public Task<string?> CreateUser(string email);
  }
}
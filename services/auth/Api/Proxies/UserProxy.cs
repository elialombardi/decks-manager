using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace Api.Proxies
{
  public class UserProxy(HttpClient httpClient, IConfiguration configuration) : BaseProxy(configuration), IUserProxy
  {

    public async Task<string?> CreateUser(string email)
    {
      var userData = new { Email = email };
      var json = JsonSerializer.Serialize(userData);
      var data = new StringContent(json, Encoding.UTF8, "application/json");

      // Include JWT token in the Authorization header
      httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GenerateJwtToken());

      var response = await httpClient.PostAsync("users", data);
      response.EnsureSuccessStatusCode();

      var content = await response.Content.ReadAsStringAsync();
      var responseData = JsonSerializer.Deserialize<CreateUserResponse>(content);

      return responseData?.UserID;
    }

    public record CreateUserResponse(string UserID);
  }
}
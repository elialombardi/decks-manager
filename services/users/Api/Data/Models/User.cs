namespace Api.Data.Models;

public class User
{
  public Guid UserID { get; set; }
  public string? Username { get; set; }
  public required string Email { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }
  public DateTime? DeletedAt { get; set; }
}
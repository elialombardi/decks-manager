using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Data.Models;

public class Auth
{
  public Guid AuthID { get; set; }
  public required string UserID { get; set; }
  public required byte RoleID { get; set; }
  [ForeignKey("RoleID")]
  public Role Role { get; set; }
  public required string Email { get; set; }
  public required string Password { get; set; }
  public int LoginAttempts { get; set; }
  public bool IsBlocked { get; set; }
  public bool IsEmailConfirmed { get; set; }
  public string? PasswordChangeRequestCode { get; set; }
  public DateTime? PasswordChangeRequestDate { get; set; }
  public DateTime? LastLoginDate { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }
  public DateTime? DeletedAt { get; set; }
}

public record Role(byte RoleID, string Name);

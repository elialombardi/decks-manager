using Api.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.Users.Commands
{
  public class CreateUserCommandValidator : AbstractValidator<CreateUserCommandRequest>
  {
    private readonly UsersDbContext _context;

    public CreateUserCommandValidator(UsersDbContext context)
    {
      _context = context;

      RuleFor(x => x.Username)
        .NotEmpty()
        .WithMessage("Username is required.")
        .MustAsync(BeUniqueUserName).WithMessage("Username must be unique.");

      RuleFor(x => x.Email)
        .NotEmpty().EmailAddress().WithMessage("A valid email is required.")
        .MustAsync(BeUniqueEmail).WithMessage("Email must be unique.");
    }

    private async Task<bool> BeUniqueEmail(string email, CancellationToken cancellationToken)
    {
      var isUnique = await _context.Users.AllAsync(u => u.Email != email, cancellationToken);
      return isUnique;
    }

    private async Task<bool> BeUniqueUserName(string? username, CancellationToken cancellationToken)
    {
      var isUnique = await _context.Users.AllAsync(u => u.Username != username, cancellationToken);
      return isUnique;
    }
  }
}
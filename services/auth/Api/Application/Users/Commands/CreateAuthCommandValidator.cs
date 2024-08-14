using Api.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.Auths.Commands
{
  public class CreateAuthCommandValidator : AbstractValidator<CreateAuthCommand>
  {
    private readonly AuthDbContext _context;

    public CreateAuthCommandValidator(AuthDbContext context)
    {
      _context = context;

      RuleFor(x => x.Email)
        .NotEmpty().EmailAddress().WithMessage("A valid email is required.")
        .MustAsync(BeUniqueEmail).WithMessage("Email must be unique.");

      RuleFor(x => x.Password)
        .NotEmpty().WithMessage("Password is required.");
    }

    private async Task<bool> BeUniqueEmail(string email, CancellationToken cancellationToken)
    {
      var isUnique = await _context.Auths.AllAsync(u => u.Email != email, cancellationToken);
      return isUnique;
    }

  }
}
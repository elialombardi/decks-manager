using FluentValidation;

namespace Api.Features.Users.Commands
{
  public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
  {
    public DeleteUserCommandValidator()
    {
      RuleFor(x => x.UserID)
        .NotEmpty()
        .WithMessage("UserID is required.");
    }

  }
}
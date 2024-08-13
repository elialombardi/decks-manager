using System.Security.Claims;
using FluentValidation;
using MediatR;

namespace Api.Features.Users.Commands
{
  public record DeleteUserCommand(string UserID) : IRequest<Guid?>;
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
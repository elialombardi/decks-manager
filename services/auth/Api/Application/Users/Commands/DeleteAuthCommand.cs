using System.Security.Claims;
using FluentValidation;
using MediatR;

namespace Api.Features.Auths.Commands
{
  public record DeleteAuthCommand(string AuthID) : IRequest<Guid?>;
  public class DeleteAuthCommandValidator : AbstractValidator<DeleteAuthCommand>
  {
    public DeleteAuthCommandValidator()
    {
      RuleFor(x => x.AuthID)
        .NotEmpty()
        .WithMessage("AuthID is required.");
    }

  }
}
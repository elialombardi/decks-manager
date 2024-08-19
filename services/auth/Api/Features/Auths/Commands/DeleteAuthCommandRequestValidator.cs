using FluentValidation;

namespace Api.Features.Auths.Commands
{
  public class DeleteAuthCommandRequestValidator : AbstractValidator<DeleteAuthCommandRequest>
  {
    public DeleteAuthCommandRequestValidator()
    {
      RuleFor(x => x.AuthID)
        .NotEmpty()
        .WithMessage("AuthID is required.");
    }

  }
}
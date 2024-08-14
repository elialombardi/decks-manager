using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;

namespace Api.Application.Common
{


  public class ValidationBehavior<TRequest, TResponse>(IValidator<TRequest>? validator = null) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
  {
    private readonly IValidator<TRequest>? _validator = validator;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
      if (_validator != null)
      {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
          throw new ValidationException(validationResult.Errors);
        }
      }

      return await next();
    }
  }
}
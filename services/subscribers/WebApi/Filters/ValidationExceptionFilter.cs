using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApi.Filters
{
  public class ValidationExceptionFilter(ILogger<ValidationExceptionFilter> logger) : IExceptionFilter
  {
    public void OnException(ExceptionContext context)
    {
      if (context.Exception is ValidationException validationException)
      {
        logger.LogDebug("ValidationExceptionFilter triggered");

        var errors = validationException.Errors
            .Select(e => new { e.PropertyName, e.ErrorMessage })
            .ToList();

        context.Result = new BadRequestObjectResult(new { Errors = errors });
        context.ExceptionHandled = true;
      }
      else
      {
        logger.LogError(context.Exception, "An unhandled exception occurred.");

        context.Result = new StatusCodeResult(500);
        context.ExceptionHandled = true;
      }
    }
  }
}
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Inventory.Api.Filters;

public class ValidationFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        foreach (object? argument in context.ActionArguments.Values)
        {
            if (argument is null)
            {
                continue;
            }

            Type argumentType = argument.GetType();

            IValidator? validator = context.HttpContext.RequestServices
                .GetService(typeof(IValidator<>).MakeGenericType(argumentType)) as IValidator;

            if (validator is null)
            {
                continue;
            }

            ValidationContext<object> validationContext = new(argument);

            FluentValidation.Results.ValidationResult result = await validator.ValidateAsync(validationContext);

            if (!result.IsValid)
            {
                foreach (FluentValidation.Results.ValidationFailure failure in result.Errors)
                {
                    context.ModelState.AddModelError(failure.PropertyName, failure.ErrorMessage);
                }
            }
        }

        if (!context.ModelState.IsValid)
        {
            ValidationProblemDetails problemDetails = new(context.ModelState)
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "One or more validation errors occured.",
            };

            context.Result = new BadRequestObjectResult(problemDetails);
            return;
        }

        await next();
    }
}

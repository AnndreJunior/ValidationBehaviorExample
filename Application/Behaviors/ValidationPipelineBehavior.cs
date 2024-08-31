using Core.Shared;
using FluentValidation;
using MediatR;

namespace Application.Behaviors;

public sealed class ValidationPipelineBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Result
{
    private readonly IValidator<TRequest> _validators;

    public ValidationPipelineBehavior(IValidator<TRequest> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        // throw new Exception();

        if (_validators is null)
        {
            return await next();
        }

        Error[] errors = _validators.Validate(request)
            .Errors
            .Select(failure => new Error(
                failure.PropertyName,
                failure.ErrorMessage))
            .Distinct()
            .ToArray();
        // .Select(validator => validator.Validate((IValidationContext)request))
        // .SelectMany(validationResult => validationResult.Errors)
        // .Where(validationFailure => validationFailure is not null)
        // .Select(failure => new Error(
        //     failure.PropertyName,
        //     failure.ErrorMessage))
        // .Distinct()
        // .ToArray();
        if (errors.Length != 0)
        {
            return CreateValidationResult(errors);
        }

        return await next();
    }

    private static TResponse CreateValidationResult(Error[] errors)
    {
        if (typeof(TResponse) == typeof(Result))
        {
            return (ValidationResult.WithErrors(errors) as TResponse)!;
        }

        object validationResult = typeof(ValidationResult<>)
            .GetGenericTypeDefinition()
            .MakeGenericType(typeof(TResponse).GenericTypeArguments[0])
            .GetMethod(nameof(ValidationResult.WithErrors))!
            .Invoke(null, [errors])!;
        return (TResponse)validationResult;
    }
}

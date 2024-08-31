using Core.Shared;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Common.Api.Endpoints;

public abstract class EndpointsGroupBase
{
    public abstract void Map(WebApplication app);

    protected static IResult HandleFailure(Result result) =>
        result switch
        {
            { IsSuccess: true } => throw new InvalidOperationException(),
            IValidationResult validationResult =>
                TypedResults.BadRequest(
                    CreateValidationProblemDetails(
                        "Validation Error",
                        StatusCodes.Status400BadRequest,
                        result.Error,
                        validationResult.Errors)),
            _ =>
                TypedResults.BadRequest(
                    CreateProblemDetails(
                        "Bad Request",
                        StatusCodes.Status400BadRequest,
                        result.Error))
        };

    private static ValidationProblemDetails CreateValidationProblemDetails(
        string title,
        int status,
        Error error,
        Error[] errors) =>
        new()
        {
            Title= title,
            Type = error.Code,
            Detail = error.Message,
            Status = status,
            Errors = errors
                .GroupBy(x => x.Code)
                .ToDictionary(
                    x => x.Key,
                    x => x.Select(e => e.Message)
                        .ToArray())
        };

    private static ProblemDetails CreateProblemDetails(
        string title,
        int status,
        Error error,
        Error[]? errors = null) =>
        new()
        {
            Title = title,
            Type = error.Code,
            Detail = error.Message,
            Status = status,
            Extensions = { { nameof(errors), errors } }
        };
}

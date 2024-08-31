using Application.Handlers.TodoItem.Create;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Endpoints;

public sealed class TodoItem : EndpointsGroupBase
{
    public override void Map(WebApplication app)
    {
        var endpoints = app.MapGroup(this);
        endpoints.MapPost("", Create)
            .Produces<Core.Entities.TodoItem>(201)
            .ProducesValidationProblem();
    }

    static async Task<IResult> Create(CreateTodoItemRequest request, IMediator mediator)
    {
        var response = await mediator.Send(request);
        if (response.IsFailure)
        {
            return HandleFailure(response);
        }
        return TypedResults.Created(string.Empty, response.Value);
    }
}

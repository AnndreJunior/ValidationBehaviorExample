using Core.Shared;
using MediatR;

namespace Application.Handlers.TodoItem.Create;

public record CreateTodoItemRequest(string Title) : IRequest<Result<Core.Entities.TodoItem>>;

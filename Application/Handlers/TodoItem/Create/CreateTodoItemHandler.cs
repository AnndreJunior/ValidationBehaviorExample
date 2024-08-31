using Core.Shared;
using MediatR;

namespace Application.Handlers.TodoItem.Create;

public sealed class CreateTodoItemHandle
    : IRequestHandler<CreateTodoItemRequest, Result<Core.Entities.TodoItem>>
{
    public async Task<Result<Core.Entities.TodoItem>> Handle(CreateTodoItemRequest request, CancellationToken cancellationToken)
    {
        var todoItem = Core.Entities.TodoItem.CreateInstance(request.Title);
        return todoItem;
    }
}

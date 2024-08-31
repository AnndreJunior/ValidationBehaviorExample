using FluentValidation;

namespace Application.Handlers.TodoItem.Create;

public sealed class CreateTodoItemRequestValidator : AbstractValidator<CreateTodoItemRequest>
{
    public CreateTodoItemRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty();
    }
}

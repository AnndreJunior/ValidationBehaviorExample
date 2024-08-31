namespace Core.Entities;

public sealed class TodoItem
{
    private TodoItem(string title)
    {
        Id = Guid.NewGuid();
        Title = title;
    }

    public Guid Id { get; init; }
    public string Title { get; private set; } = string.Empty;

    public static TodoItem CreateInstance(string title)
    {
        TodoItem todoItem = new(title);
        return todoItem;
    }
}

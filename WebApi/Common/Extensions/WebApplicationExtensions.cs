using System.Reflection;

namespace WebApi.Common.Extensions;

public static class WebApplicationExtensions
{
    public static RouteGroupBuilder MapGroup(this WebApplication app, EndpointsGroupBase group)
    {
        string groupName = group.GetType().Name;
        return app.MapGroup(groupName.ToLower())
            .WithTags(groupName)
            .WithOpenApi();
    }

    public static void MapEndpoints(this WebApplication app)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var endpointsGroupBaseTypes = assembly.GetExportedTypes()
            .Where(t => t.IsSubclassOf(typeof(EndpointsGroupBase)));
        foreach (var type in endpointsGroupBaseTypes)
        {
            if (Activator.CreateInstance(type) is EndpointsGroupBase instance)
            {
                instance.Map(app);
            }
        }
    }
}

using System.Reflection;
using TimeManagement.Api.Endpoints;

namespace TimeManagement.Api.Extensions;

public static class EndpointExtensions
{
    public static void MapEndpointsFromAssembly(this WebApplication app)
    {
        var endpointTypes = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(type => type.IsSubclassOf(typeof(EndPoints)) && !type.IsAbstract);

        foreach (var endpointType in endpointTypes)
        {
            if (Activator.CreateInstance(endpointType) is EndPoints endpoint)
            {
                endpoint.MapEndpoints(app);
            }
        }
    }
}

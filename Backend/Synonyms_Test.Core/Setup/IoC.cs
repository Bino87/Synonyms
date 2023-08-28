using Microsoft.Extensions.DependencyInjection;
using Synonyms_Test.Core.Implementation;
using Synonyms_Test.Core.Interfaces;

namespace Synonyms_Test.Core.Setup;

public static class IoC
{
    public static void AddServices(IServiceCollection services)
    {
        services.AddSingleton<ISettings, Settings>();
        services.AddScoped<IResponseErrorHandler, ResponseErrorHandler>();
        services.AddTransient<IWordAdapter, WordAdapter>();
        services.AddTransient<IWordValidator, WordValidator>();
        services.AddTransient<IResponseFactory, ResponseFactory>();
        services.AddScoped<IEndPointLoggerFactory, EndPointLoggerFactory>();
        services.AddTransient<ITimeHandler, TimeHandler>();
    }
}
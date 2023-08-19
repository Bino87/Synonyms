using Microsoft.Extensions.DependencyInjection;
using Services.Interfaces;
using Services.Repositories;
using Services.Services;

namespace Services.IoC;

public static class IoC
{
    public static void AddServices(IServiceCollection services)
    {
        //Singleton since it needs to hold data between sessions
        services.AddSingleton<ISynonymData, SynonymData>();
        //Transient since it doesn't hold any data
        services.AddTransient<ISynonymRepository, SynonymRepository>();
        services.AddTransient<ISynonymService, SynonymService>();
    }
}
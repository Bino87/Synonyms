using Microsoft.Extensions.DependencyInjection;
using Synonyms_Test.Services.Implementation;
using Synonyms_Test.Services.Interfaces;

namespace Synonyms_Test.Services.Setup;

public static class IoC
{
    public static void AddServices(IServiceCollection services)
    {
        services.AddScoped<ISynonymService, SynonymService>();
       

        DataAccess.Setup.IoC.AddServices(services);
    }
}
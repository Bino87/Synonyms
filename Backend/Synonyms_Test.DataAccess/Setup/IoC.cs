using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Synonyms_Test.DataAccess.Context;
using Synonyms_Test.DataAccess.Implementation;
using Synonyms_Test.DataAccess.Interfaces;

namespace Synonyms_Test.DataAccess.Setup;

public static class IoC
{
    public static void AddServices(IServiceCollection services)
    {
        services.AddScoped<ISynonymsRepository, SynonymsRepository>();
        services.AddDbContext<WordContext>(options => options.UseInMemoryDatabase("InMemoryDbContext"), ServiceLifetime.Singleton)
            .AddDbContextFactory<WordContext>();
    }
}
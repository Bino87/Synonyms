using Microsoft.AspNetCore.Authentication;
using Synonyms_Test.MapperProfiles;

namespace Synonyms_Test.StartUp;

internal static class Startup
{
    internal static void AddServices(IServiceCollection services)
    {
        //Authentication
        services.AddAuthentication("BasicAuthentication")
            .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", _ => { });

        //Add Logging
        services.AddLogging();

        //Setup Mapper
        services.AddAutoMapper(typeof(GetSynonymsProfile), typeof(GetAllWordsProfile));
       

        //Add services
        Services.Setup.IoC.AddServices(services);
        Core.Setup.IoC.AddServices(services);
    }
}
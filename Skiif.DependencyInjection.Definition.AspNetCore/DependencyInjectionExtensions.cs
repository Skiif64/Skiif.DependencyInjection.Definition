using Microsoft.Extensions.DependencyInjection;
using Skiif.DependencyInjection.Definition.AspNetCore.Context;
using Skiif.DependencyInjection.Definition.Definition.Context;
using Skiif.DependencyInjection.Definition.DependencyInjection.Options;

namespace Microsoft.AspNetCore.Builder;
public static class DependencyInjectionExtensions
{
    public static WebApplicationBuilder AddDefinition(this WebApplicationBuilder builder, Action<DefinitionConfiguration>? configure = null)
    {
        var context = new ServiceDefinitionContext(builder.Services, builder.Configuration);
        builder.Services.AddDefinitions(context, configure);
        return builder;
    }

    public static async Task UseDefinitionAsync(this WebApplication application)
    {
        var context = new WebApplicationDefinitionContext(application);
        await application.Services.UseApplicationDefinitionsAsync(context);
    }
}

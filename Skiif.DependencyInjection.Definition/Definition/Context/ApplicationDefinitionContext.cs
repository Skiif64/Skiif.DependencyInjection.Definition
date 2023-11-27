using Microsoft.Extensions.Configuration;

namespace Skiif.DependencyInjection.Definition.Definition.Context;
public class ApplicationDefinitionContext : IApplicationDefinitionContext
{
    public IConfiguration Configuration { get; }
    public IServiceProvider ServiceProvider { get; }

    public ApplicationDefinitionContext(IServiceProvider serviceProvider, IConfiguration configuration)
    {
        ServiceProvider = serviceProvider;
        Configuration = configuration;
    }
}

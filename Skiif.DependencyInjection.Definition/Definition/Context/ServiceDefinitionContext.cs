using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Skiif.DependencyInjection.Definition.Definition.Context;
public class ServiceDefinitionContext : IServiceDefinitionContext
{
    public IConfiguration Configuration { get; }
    public IServiceCollection Services { get; }

    public ServiceDefinitionContext(IServiceCollection services, IConfiguration configuration)
    {
        Services = services;
        Configuration = configuration;
    }
}

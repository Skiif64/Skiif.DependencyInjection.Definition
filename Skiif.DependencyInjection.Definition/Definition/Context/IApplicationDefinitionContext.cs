using Microsoft.Extensions.Configuration;

namespace Skiif.DependencyInjection.Definition.Definition.Context;
public interface IApplicationDefinitionContext
{
    IConfiguration Configuration { get; }
    IServiceProvider ServiceProvider { get; }
}

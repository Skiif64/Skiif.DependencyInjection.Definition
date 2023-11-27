using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Skiif.DependencyInjection.Definition.Definition.Context;
public interface IServiceDefinitionContext
{
    IConfiguration Configuration { get; }
    IServiceCollection Services { get; }
}

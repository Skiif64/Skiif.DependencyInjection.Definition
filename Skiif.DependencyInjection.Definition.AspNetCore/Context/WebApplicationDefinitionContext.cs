using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Skiif.DependencyInjection.Definition.Definition.Context;

namespace Skiif.DependencyInjection.Definition.AspNetCore.Context;
public class WebApplicationDefinitionContext : IApplicationDefinitionContext
{
    public IConfiguration Configuration { get; }
    public IServiceProvider ServiceProvider { get; }
    public WebApplication Application { get; }

    public WebApplicationDefinitionContext(WebApplication application)
    {
        Application = application;
        ServiceProvider = application.Services;
        Configuration = application.Configuration;
    }
}

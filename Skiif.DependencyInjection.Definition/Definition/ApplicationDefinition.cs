using Skiif.DependencyInjection.Definition.Definition.Context;

namespace Skiif.DependencyInjection.Definition.Definition;
public class ApplicationDefinition : IApplicationDefinition
{
    public virtual bool IsRequired { get; } = true;
    public virtual int Priority { get; } = 0;

    public virtual Task ConfigureApplicationAsync(IApplicationDefinitionContext context)
    {
        return Task.CompletedTask;
    }

    public virtual void ConfigureServices(IServiceDefinitionContext context)
    {
       
    }
}

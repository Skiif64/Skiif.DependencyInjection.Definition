using Skiif.DependencyInjection.Definition.Definition.Context;

namespace Skiif.DependencyInjection.Definition.Definition;
public interface IApplicationDefinition
{
    /// <summary>
    /// Indicates whether to throw exception on caught exception
    /// </summary>
    bool IsRequired { get; }
    /// <summary>
    /// Priority to apply definition. Ordering by ascending
    /// </summary>
    int Priority { get; }
    /// <summary>
    /// Configure service collection
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    void ConfigureServices(IServiceDefinitionContext context);
    /// <summary>
    /// Configure application
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    Task ConfigureApplicationAsync(IApplicationDefinitionContext context);
}

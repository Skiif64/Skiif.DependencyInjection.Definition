using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Skiif.DependencyInjection.Definition.Definition;
using Skiif.DependencyInjection.Definition.Definition.Context;
using Skiif.DependencyInjection.Definition.DependencyInjection.Options;
using Skiif.DependencyInjection.Definition.Exceptions;

namespace Microsoft.Extensions.DependencyInjection;
public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddDefinitions(this IServiceCollection services, IConfiguration configuration, Action<DefinitionConfiguration>? configure = null)
        => AddDefinitions(services, configuration, configure);

    public static IServiceCollection AddDefinitions(this IServiceCollection services, IServiceDefinitionContext context, Action<DefinitionConfiguration>? configure = null)
    {
        var options = new DefinitionConfiguration();
        configure?.Invoke(options);

        var assembliesToScan = options.ScanningStrategy.GetAssemblies();

        var definedDefinitions = assembliesToScan
            .SelectMany(assembly => assembly.DefinedTypes)
            .Where(type => type.IsClass && !type.IsAbstract)
            .Where(type => type.IsAssignableTo(typeof(IApplicationDefinition)));

        foreach (var definitionType in definedDefinitions)
        {
            var definition = Activator.CreateInstance(definitionType) as IApplicationDefinition;
            if (definition is null)
            {
                throw new DefinitionRegistrationException(
                    $"Unable to create instance of {definitionType.Name}.");
            }
            try
            {
                definition.ConfigureServices(context);
            }
            catch (Exception exception)
            {
                if (definition.IsRequired)
                {
                    throw new DefinitionRegistrationException(
                        $"Unable to configure services." +
                        $" Definition name: {nameof(definition)}." +
                        $" See inner exception for details.", exception);
                }
            }
            services.AddTransient(typeof(IApplicationDefinition), definitionType);
        }

        return services;
    }

    public static async Task UseApplicationDefinitionsAsync(this IServiceProvider provider)
    {
        var configuration = provider.GetRequiredService<IConfiguration>();
        var context = new ApplicationDefinitionContext(provider, configuration);
        await UseApplicationDefinitionsAsync(provider, context);
    }

    public static async Task UseApplicationDefinitionsAsync(this IServiceProvider provider, IApplicationDefinitionContext context)
    {
        var loggerFactory = provider.GetService<ILoggerFactory>();
        var logger = loggerFactory?.CreateLogger<IApplicationDefinition>();//TODO: use normal type

        var registeredDefinitions = provider.GetServices<IApplicationDefinition>();
        if (registeredDefinitions is null || !registeredDefinitions.Any())
        {
            throw new DefinitionRegistrationException($"No one {nameof(IApplicationDefinition)} was found.");
        }
        logger?.LogDebug("[ApplicationDefinition] Found {count} definitions", registeredDefinitions.Count());

        var groupedDefinitions = registeredDefinitions
            .GroupBy(definition => definition.Priority)
            .OrderBy(group => group.Key);

        var exceptions = new List<Exception>();
        foreach (var definitionGroup in groupedDefinitions)
        {
            try
            {
                var tasks = new List<Task>();
                foreach (var definition in definitionGroup)
                {
                    tasks.Add(ApplyDefinition(context, definition, logger));
                }
                await Task.WhenAll(tasks).ConfigureAwait(false);
            }
            catch (DefinitionRegistrationException exception)
            {
                exceptions.Add(exception);
            }
        }

        if (exceptions.Count != 0)
        {
            throw new DefinitionRegistrationException(
                "One or more required definition throws exception. See inner exception for details.",
                exceptions.ToArray());
        }
    }

    public static IServiceProvider UseApplicationDefinitions(this IServiceProvider provider)
    {
        UseApplicationDefinitionsAsync(provider).Wait();
        return provider;
    }

    private static async Task ApplyDefinition(
        IApplicationDefinitionContext context,
        IApplicationDefinition definition,
        ILogger<IApplicationDefinition>? logger)
    {
        try
        {
            await definition.ConfigureApplicationAsync(context).ConfigureAwait(false);
            logger?.LogDebug("[ApplicationDefinition] Definition {name} successfully applied", nameof(definition));
        }
        catch (Exception exception)
        {
            logger?.LogError("[ApplicationDefinition] Definition {name} throws exception | exception: {exception}",
                nameof(definition), exception);
            if (definition.IsRequired)
            {
                throw new DefinitionRegistrationException("Required definition throws exception", exception);
            }
        }
    }
}

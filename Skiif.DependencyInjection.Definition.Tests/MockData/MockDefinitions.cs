using Microsoft.Extensions.DependencyInjection;
using Skiif.DependencyInjection.Definition.Definition;
using Skiif.DependencyInjection.Definition.Definition.Context;

namespace Skiif.DependencyInjection.Definition.Tests.MockData;
public class MockDefinition1 : IApplicationDefinition
{
    public bool IsRequired { get; }
    public int Priority { get; }

    public Task ConfigureApplicationAsync(IApplicationDefinitionContext context)
    {
        return Task.CompletedTask;
    }

    public void ConfigureServices(IServiceDefinitionContext context)
    {
        context.Services.AddTransient<MockService>();
    }
}

public class MockDefinition2 : IApplicationDefinition
{
    public bool IsRequired { get; }
    public int Priority { get; }

    public Task ConfigureApplicationAsync(IApplicationDefinitionContext context)
    {
        return Task.CompletedTask;
    }

    public void ConfigureServices(IServiceDefinitionContext context)
    {
        context.Services.AddTransient<MockService>();
    }
}

public class MockDefinition3 : IApplicationDefinition
{
    public bool IsRequired { get; }
    public int Priority { get; }

    public Task ConfigureApplicationAsync(IApplicationDefinitionContext context)
    {
        return Task.CompletedTask;
    }

    public void ConfigureServices(IServiceDefinitionContext context)
    {
        context.Services.AddTransient<MockService>();
    }
}

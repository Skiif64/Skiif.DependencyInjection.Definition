using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Skiif.DependencyInjection.Definition.Definition;
using Skiif.DependencyInjection.Definition.Definition.Context;
using Skiif.DependencyInjection.Definition.DependencyInjection.Options;
using Skiif.DependencyInjection.Definition.Tests.MockData;

namespace Skiif.DependencyInjection.Definition.Tests.Tests;
public class DependencyInjectionTests
{
    private readonly IServiceCollection _collection;
    private readonly IConfiguration _configuration;
    private readonly Action<DefinitionConfiguration> _definitionConfig;
    public DependencyInjectionTests()
    {
        _collection = new ServiceCollection();
        _configuration = new Mock<IConfiguration>().Object;
        _definitionConfig = x 
            => x.ScanningStrategy = AssemblyScanningStrategy.FromArray(typeof(MockDefinition1).Assembly);
    }

    [Fact]
    public void AddDefinitions_ShouldBeAdd3Definition_When3DefinitionDefined()
    {
        var context = new ServiceDefinitionContext(_collection, _configuration);

        _collection.AddDefinitions(context, _definitionConfig);

        var registered = _collection.Where(x => x.ServiceType == typeof(IApplicationDefinition));

        Assert.Equal(3, registered.Count());
    }

    [Fact]
    public void AddDefinitions_ShouldBeAdd3Services_When3ServicesDefined()
    {
        var context = new ServiceDefinitionContext(_collection, _configuration);

        _collection.AddDefinitions(context, _definitionConfig);

        var registered = _collection.Where(x => x.ServiceType == typeof(MockService));

        Assert.Equal(3, registered.Count());
    }

    [Fact]
    public async Task UseDefinitionsAsync_ShouldBeInvoke3Definitions_When3DefinitionsExists()
    {
        var mockDefinition1 = new Mock<IApplicationDefinition>();
        var mockDefinition2 = new Mock<IApplicationDefinition>();
        var mockDefinition3 = new Mock<IApplicationDefinition>();
        _collection.AddTransient(provider => mockDefinition1.Object);
        _collection.AddTransient(provider => mockDefinition2.Object);
        _collection.AddTransient(provider => mockDefinition3.Object);
        var provider = _collection.BuildServiceProvider();
        var context = new ApplicationDefinitionContext(provider, _configuration);

        await provider.UseApplicationDefinitionsAsync(context);

        mockDefinition1.Verify(x => x.ConfigureApplicationAsync(context), Times.Once);
        mockDefinition2.Verify(x => x.ConfigureApplicationAsync(context), Times.Once);
        mockDefinition3.Verify(x => x.ConfigureApplicationAsync(context), Times.Once);
    }
}

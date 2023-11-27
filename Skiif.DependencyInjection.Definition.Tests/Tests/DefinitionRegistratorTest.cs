using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Skiif.DependencyInjection.Definition.Definition;
using Skiif.DependencyInjection.Definition.Definition.Context;
using Skiif.DependencyInjection.Definition.DependencyInjection.Options;

namespace Skiif.DependencyInjection.Definition.Tests.Tests;
public class DefinitionRegistratorTest
{
    private readonly DefinitionConfiguration _options;
    private readonly IServiceCollection _serviceCollection;
    private readonly IConfiguration _configuration;
    //private readonly DefinitionRegistrator _sut;

    public DefinitionRegistratorTest()
    {
        _options = new DefinitionConfiguration();

        var scannerMock = new Mock<AssemblyScanningStrategy>();
        scannerMock.Setup(x => x.GetAssemblies())
            .Returns(new[] { typeof(DefinitionRegistratorTest).Assembly });
        _options.ScanningStrategy = scannerMock.Object;
        _serviceCollection = new ServiceCollection();
        _configuration = new Mock<IConfiguration>().Object;
        //_sut = new DefinitionRegistrator(_options);
    }
    [Fact]
    public void ApplyServiceDefinitions_ShouldRegister3Definition_When3Exists()
    {
        //_sut.ApplyServiceDefinitions(_serviceCollection, _configuration);

        var registeredDefinitions = _serviceCollection.Where(x => x.ServiceType == typeof(IApplicationDefinition));

        Assert.Equal(3, registeredDefinitions.Count());
    }

    [Fact]
    public async Task UseApplicationDefinitionsAsync_ShouldInvoke3Definitions_When3Exists()
    {
        var definition1Mock = new Mock<IApplicationDefinition>();
        var definition2Mock = new Mock<IApplicationDefinition>();
        var definition3Mock = new Mock<IApplicationDefinition>();
        _serviceCollection.AddTransient(provider => definition1Mock.Object);
        _serviceCollection.AddTransient(provider => definition2Mock.Object);
        _serviceCollection.AddTransient(provider => definition3Mock.Object);
        _serviceCollection.AddTransient(provider => _configuration);

        var provider = _serviceCollection.BuildServiceProvider();
        

        //await _sut.UseApplicationDefinitionsAsync(provider);

        definition1Mock.Verify(x => x.ConfigureApplicationAsync(It.IsAny<IApplicationDefinitionContext>()), Times.Once);
        definition2Mock.Verify(x => x.ConfigureApplicationAsync(It.IsAny<IApplicationDefinitionContext>()), Times.Once);
        definition3Mock.Verify(x => x.ConfigureApplicationAsync(It.IsAny<IApplicationDefinitionContext>()), Times.Once);
    }

    [Fact]
    public async Task UseApplicationDefinitionsAsync_ShouldInvokeDefinitonInOrder_WhenPriorityIsSet()
    {
        var definition1Mock = new Mock<IApplicationDefinition>();       
        var definition2Mock = new Mock<IApplicationDefinition>();
        var definition3Mock = new Mock<IApplicationDefinition>();
        definition1Mock.SetupGet(x => x.Priority)
           .Returns(0);
        definition2Mock.SetupGet(x => x.Priority)
           .Returns(1);
        definition3Mock.SetupGet(x => x.Priority)
           .Returns(2);
        _serviceCollection.AddTransient(provider => definition1Mock.Object);
        _serviceCollection.AddTransient(provider => definition2Mock.Object);
        _serviceCollection.AddTransient(provider => definition3Mock.Object);
        _serviceCollection.AddTransient(provider => _configuration);

        var provider = _serviceCollection.BuildServiceProvider();
        var sequence = new MockSequence();
        definition1Mock.InSequence(sequence)
            .Setup(x => x.ConfigureApplicationAsync(It.IsAny<IApplicationDefinitionContext>()));
        definition2Mock.InSequence(sequence)
            .Setup(x => x.ConfigureApplicationAsync(It.IsAny<IApplicationDefinitionContext>()));
        definition3Mock.InSequence(sequence)
            .Setup(x => x.ConfigureApplicationAsync(It.IsAny<IApplicationDefinitionContext>()));

        //await _sut.UseApplicationDefinitionsAsync(provider);

        definition1Mock.Verify(x => x.ConfigureApplicationAsync(It.IsAny<IApplicationDefinitionContext>()), Times.Once);
        definition2Mock.Verify(x => x.ConfigureApplicationAsync(It.IsAny<IApplicationDefinitionContext>()), Times.Once);
        definition3Mock.Verify(x => x.ConfigureApplicationAsync(It.IsAny<IApplicationDefinitionContext>()), Times.Once);
    }
}

namespace Skiif.DependencyInjection.Definition.DependencyInjection.Options;
public class DefinitionConfiguration
{
    public AssemblyScanningStrategy ScanningStrategy { get; set; } = AssemblyScanningStrategy.FromAppDomain;
}

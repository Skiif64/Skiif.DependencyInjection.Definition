using System.Reflection;

namespace Skiif.DependencyInjection.Definition.DependencyInjection.Options;
public abstract class AssemblyScanningStrategy
{
    public static AssemblyScanningStrategy FromDirectory 
        => new DirectoryScanningStrategy();
    public static AssemblyScanningStrategy FromAppDomain 
        => new DomainScanningStrategy();
    public static AssemblyScanningStrategy FromArray(params Assembly[] assemblies) 
        => new ArrayScanningStrategy(assemblies);

    public abstract IEnumerable<Assembly> GetAssemblies();
}

internal sealed class DirectoryScanningStrategy : AssemblyScanningStrategy
{
    public override IEnumerable<Assembly> GetAssemblies()
    {
        var assemblyDirectory = Environment.CurrentDirectory;
        var directoryInfo = new DirectoryInfo(assemblyDirectory);
        var files = directoryInfo.EnumerateFiles("*.dll");
        foreach(var file in files)
        {
            yield return Assembly.LoadFrom(file.FullName);
        }
    }
}

internal sealed class DomainScanningStrategy : AssemblyScanningStrategy
{
    public override IEnumerable<Assembly> GetAssemblies()
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        return assemblies;
    }
}

internal sealed class ArrayScanningStrategy : AssemblyScanningStrategy
{
    private readonly Assembly[] _assemblies;
    public ArrayScanningStrategy(Assembly[] assemblies)
    {
        _assemblies = assemblies;
    }
    public override IEnumerable<Assembly> GetAssemblies()
        => _assemblies;
}

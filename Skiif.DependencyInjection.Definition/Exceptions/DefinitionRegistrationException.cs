namespace Skiif.DependencyInjection.Definition.Exceptions;
public class DefinitionRegistrationException : Exception
{
    public DefinitionRegistrationException(string message) 
        : base(message)
    {
        
    }

    public DefinitionRegistrationException(string message, Exception innerException) 
        : base(message, innerException)
    {

    }

    public DefinitionRegistrationException(string message, params Exception[] innerExceptions)
       : base(message, new AggregateException(innerExceptions))
    {

    }
}

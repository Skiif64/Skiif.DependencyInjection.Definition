using System.Diagnostics.CodeAnalysis;

namespace Skiif.DependencyInjection.Definition.Definition.Context;
public static class ApplicationDefintionContextExtensions
{
    public static TContext Cast<TContext>(this IApplicationDefinitionContext context)
        where TContext : class, IApplicationDefinitionContext
    {
        if (context is TContext casted)
        {
            return casted;
        }

        throw new InvalidCastException($"Unable to cast {context.GetType().Name} to {typeof(TContext).Name}");
    }

    public static bool TryCast<TContext>(this IApplicationDefinitionContext context, [NotNullWhen(true)] out TContext? value)
        where TContext : class, IApplicationDefinitionContext
    {
        if (context is TContext casted)
        {
            value = casted;
            return true;
        }
        value = null;
        return false;
    }
}

using JetBrains.Annotations;
using Statiq.Common;

// ReSharper disable once CheckNamespace
namespace Statiq;

[PublicAPI]
public static class BootstrapperExtensions
{
    /// <summary>
    /// Add some <c>.well-known</c> files to the output pipelines.
    /// </summary>
    /// <param name="bootstrapper"></param>
    /// <typeparam name="TBootstrapper"></typeparam>
    /// <returns></returns>
    public static TBootstrapper WithWellKnown<TBootstrapper>(this TBootstrapper bootstrapper,
        Action<WellKnownBuilder<TBootstrapper>> action)
        where TBootstrapper : IBootstrapper
        => new WellKnownBuilder<TBootstrapper>(bootstrapper).Process(action);
}

public sealed class WellKnownBuilder<TBootstrapper>
    where TBootstrapper : IBootstrapper
{
    private readonly TBootstrapper _bootstrapper;

    public WellKnownBuilder(TBootstrapper bootstrapper)
    {
        _bootstrapper = bootstrapper;
    }

    public TBootstrapper Process(Action<WellKnownBuilder<TBootstrapper>> action)
    {
        action(this);
        return _bootstrapper;
    }

    internal WellKnownBuilder<TBootstrapper> WithBootstrapper(Action<TBootstrapper> action)
    {
        action(_bootstrapper);
        return this;
    }

    internal WellKnownBuilder<TBootstrapper> AddToInputPipeline(params IModule[] modules)
    {
        _bootstrapper.ConfigureEngine(engine => engine.Pipelines.Values
            .Where(p => p.GetType().Name == "Inputs")
            .ToList()
            .ForEach(pipeline => modules.ToList().ForEach(m =>
            {
                pipeline.InputModules.Add(m);
                engine.Logger.LogTrace(null, $"Adding {m.GetType().FullName} to pipeline: {pipeline.GetType().FullName}");
            }))
            );

        return this;
    }
}